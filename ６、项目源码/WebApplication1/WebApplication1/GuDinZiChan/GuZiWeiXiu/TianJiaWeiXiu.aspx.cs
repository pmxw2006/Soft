using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mods;
using BLL;

namespace WebApplication1.GuDinZiChan.GuZiWeiXiu
{
    public partial class TianJiaWeiXiu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 页面首次加载时绑定资产下拉列表
                BangDingZiChanXiaLa();

                // 设置默认日期（使用BLL层）
                txtSongXiuRiQi.Text = WeiXiuTianJiaManager.HuoQuDangQianRiQiWenBen();
                txtYuWanRiQi.Text = WeiXiuTianJiaManager.HuoQuMingTianRiQiWenBen();
            }
        }

        // 绑定资产下拉列表
        private void BangDingZiChanXiaLa()
        {
            try
            {
                // 使用BLL层获取在库中资产列表
                DataTable shuJuBiao = WeiXiuTianJiaManager.HuoQuZaiKuZiChanLieBiao_BLL();

                // 将DataTable转换为对象列表
                List<GuDingZiChan> ziChanLieBiao = new List<GuDingZiChan>();

                foreach (DataRow hang in shuJuBiao.Rows)
                {
                    GuDingZiChan ziChan = new GuDingZiChan();
                    ziChan.GuZiID = hang["GuZiID"].ToString();
                    ziChan.GuZiMing = hang["GuZiMing"].ToString();
                    ziChan.GuZiShuLiang = hang["GuZiShuLiang"].ToString();
                    ziChanLieBiao.Add(ziChan);
                }

                // 绑定下拉列表
                ddlGuZi.DataSource = ziChanLieBiao;
                ddlGuZi.DataTextField = "GuZiMing";
                ddlGuZi.DataValueField = "GuZiID";
                ddlGuZi.DataBind();

                // 添加默认选项
                ddlGuZi.Items.Insert(0, new ListItem("-- 请选择资产 --", ""));
            }
            catch (Exception ex)
            {
                ShowError("加载资产列表失败：" + ex.Message);
            }
        }

        // 资产选择改变事件
        protected void ddlGuZi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取选中的资产ID
            string xuanZeZiChanID = ddlGuZi.SelectedValue;

            if (!string.IsNullOrEmpty(xuanZeZiChanID) && xuanZeZiChanID != "0")
            {
                try
                {
                    // 根据资产ID获取资产信息（使用BLL层）
                    GuDingZiChan ziChan = WeiXiuTianJiaManager.HuoQuZiChanXiangXiXinXi_BLL(xuanZeZiChanID);

                    if (ziChan != null)
                    {
                        txtDangQianShuLiang.Text = ziChan.GuZiShuLiang;
                    }
                    else
                    {
                        txtDangQianShuLiang.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    ShowError("获取资产信息失败：" + ex.Message);
                    txtDangQianShuLiang.Text = "";
                    txtWeiXiuShuLiang.Text = "";
                }
            }
            else
            {
                // 清空当前数量和维修数量
                txtDangQianShuLiang.Text = "";
                txtWeiXiuShuLiang.Text = "";
            }
        }

        // 保存维修记录
        protected void btnBaoCun_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 验证表单数据（使用BLL层）
                string yanZhengJieGuo = WeiXiuTianJiaManager.YanZhengWeiXiuBiaoDan(
                    ddlGuZi.SelectedValue,
                    txtWeiXiuShuLiang.Text,
                    txtSongXiuRiQi.Text,
                    txtYuWanRiQi.Text,
                    txtWeiXiuJinE.Text);

                if (!string.IsNullOrEmpty(yanZhengJieGuo))
                {
                    ShowError(yanZhengJieGuo);
                    return;
                }

                // 2. 获取选中的资产信息
                string xuanZeZiChanID = ddlGuZi.SelectedValue;
                if (string.IsNullOrEmpty(xuanZeZiChanID))
                {
                    ShowError("请选择资产！");
                    return;
                }

                // 3. 获取资产对象（使用BLL层）
                GuDingZiChan ziChan = WeiXiuTianJiaManager.HuoQuZiChanXiangXiXinXi_BLL(xuanZeZiChanID);
                if (ziChan == null)
                {
                    ShowError("获取资产信息失败！");
                    return;
                }

                // 4. 获取维修数量
                int weiXiuShuLiang;
                if (!WeiXiuTianJiaManager.AnQuanJieXiZhengShu(txtWeiXiuShuLiang.Text, out weiXiuShuLiang))
                {
                    ShowError("请输入有效的维修数量！");
                    return;
                }

                // 5. 获取其他表单数据
                // 安全地解析日期（使用BLL层）
                DateTime songXiuRiQi;
                if (!WeiXiuTianJiaManager.AnQuanJieXiRiQi(txtSongXiuRiQi.Text, out songXiuRiQi))
                {
                    ShowError("请输入有效的送修日期格式！");
                    return;
                }

                DateTime yuWanRiQi;
                if (!WeiXiuTianJiaManager.AnQuanJieXiRiQi(txtYuWanRiQi.Text, out yuWanRiQi))
                {
                    ShowError("请输入有效的预期完成日期格式！");
                    return;
                }

                decimal weiXiuJinE;
                if (!WeiXiuTianJiaManager.AnQuanJieXiXiaoShu(txtWeiXiuJinE.Text, out weiXiuJinE))
                {
                    ShowError("请输入有效的维修金额！");
                    return;
                }

                // 6. 验证资产数量
                int dangQianShuLiang;
                if (!int.TryParse(ziChan.GuZiShuLiang, out dangQianShuLiang))
                {
                    ShowError("资产数量无效！");
                    return;
                }

                // 7. 业务逻辑验证（使用BLL层）
                string yeWuYanZhengJieGuo = WeiXiuTianJiaManager.YanZhengWeiXiuYeWuLuoJi(
                    ziChan,
                    weiXiuShuLiang,
                    dangQianShuLiang,
                    songXiuRiQi,
                    yuWanRiQi,
                    weiXiuJinE);

                if (!string.IsNullOrEmpty(yeWuYanZhengJieGuo))
                {
                    ShowError(yeWuYanZhengJieGuo);
                    return;
                }

                // 8. 保存到数据库（使用BLL层）
                bool chengGong = WeiXiuTianJiaManager.BaoCunWeiXiuJiLu_BLL(
                    ziChan,
                    weiXiuShuLiang,
                    songXiuRiQi,
                    yuWanRiQi,
                    weiXiuJinE);

                if (chengGong)
                {
                    ShowSuccess("维修记录保存成功！");
                    ClearForm();
                }
                else
                {
                    ShowError("保存失败，请重试！");
                }
            }
            catch (Exception ex)
            {
                ShowError("保存失败：" + ex.Message);
            }
        }

        // 取消按钮
        protected void btnQuXiao_Click(object sender, EventArgs e)
        {
            Response.Redirect("GuZiWeiXiu.aspx");
        }

        // 清空表单方法
        private void ClearForm()
        {
            try
            {
                // 重新绑定下拉列表
                BangDingZiChanXiaLa();

                // 清空文本框
                txtDangQianShuLiang.Text = "";
                txtWeiXiuShuLiang.Text = "";
                txtWeiXiuJinE.Text = "";

                // 重置日期为默认值（使用BLL层）
                txtSongXiuRiQi.Text = WeiXiuTianJiaManager.HuoQuDangQianRiQiWenBen();
                txtYuWanRiQi.Text = WeiXiuTianJiaManager.HuoQuMingTianRiQiWenBen();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("清空表单时出错：" + ex.Message);
            }
        }

        // 显示成功提示
        private void ShowSuccess(string message)
        {
            divChengGong.InnerText = message;
            divChengGong.Visible = true;
            divCuoWu.Visible = false;

            string script = "<script type='text/javascript'>";
            script += "$(function() {";
            script += "  $('#" + divChengGong.ClientID + "').fadeIn(300);";
            script += "  setTimeout(function() {";
            script += "    $('#" + divChengGong.ClientID + "').fadeOut(300);";
            script += "  }, 3000);";
            script += "});";
            script += "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccess", script);
        }

        // 显示错误提示
        private void ShowError(string message)
        {
            divCuoWu.InnerText = message;
            divCuoWu.Visible = true;
            divChengGong.Visible = false;

            string script = "<script type='text/javascript'>";
            script += "$(function() {";
            script += "  $('#" + divCuoWu.ClientID + "').fadeIn(300);";
            script += "  setTimeout(function() {";
            script += "    $('#" + divCuoWu.ClientID + "').fadeOut(300);";
            script += "  }, 3000);";
            script += "});";
            script += "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script);
        }
    }
}