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

namespace WebApplication1.GuDinZiChan.GuZiJieHuan
{
    public partial class TianJiaGuiHuan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 页面首次加载时绑定资产下拉列表
                BangDingZiChanXiaLa();

                // 设置默认日期为今天
                txtGuiHuanRiQi.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        // 绑定资产下拉列表
        private void BangDingZiChanXiaLa()
        {
            try
            {
                // 使用BLL层获取资产列表
                DataTable shuJuBiao = GuiHuanManager.HuoQuJieChuZiChanLieBiao_BLL();

                // 将DataTable转换为对象列表
                List<GuDingZiChan> ziChanLieBiao = new List<GuDingZiChan>();

                foreach (DataRow hang in shuJuBiao.Rows)
                {
                    GuDingZiChan ziChan = new GuDingZiChan();
                    ziChan.GuZiID = hang["GuZiID"].ToString();
                    ziChan.GuZiMing = hang["GuZiMing"].ToString();
                    ziChan.GuZiShuLiang = hang["GuZiShuLiang"].ToString();
                    ziChan.ZhuangTaiID = Convert.ToInt32(hang["ZhuangTaiID"]);
                    ziChan.Gongsi = hang["Gongsi"].ToString();
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
                    GuDingZiChan ziChan = GuiHuanManager.HuoQuZiChanXiangXiXinXi_BLL(xuanZeZiChanID);

                    if (ziChan != null)
                    {
                        txtDangQianShuLiang.Text = ziChan.GuZiShuLiang;
                        txtGuiHuanGongSi.Text = ziChan.Gongsi;
                    }
                    else
                    {
                        txtDangQianShuLiang.Text = "";
                        txtGuiHuanGongSi.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    ShowError("获取资产信息失败：" + ex.Message);
                    txtDangQianShuLiang.Text = "";
                    txtGuiHuanGongSi.Text = "";
                }
            }
            else
            {
                // 清空当前数量和公司
                txtDangQianShuLiang.Text = "";
                txtGuiHuanGongSi.Text = "";
            }
        }

        // 保存归还记录
        protected void btnBaoCun_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 验证表单数据（使用BLL层）
                string yanZhengJieGuo = GuiHuanManager.YanZhengGuiHuanBiaoDan(
                    ddlGuZi.SelectedValue,
                    txtGuiHuanShuLiang.Text,
                    txtGuiHuanGongSi.Text,
                    txtGuiHuanRiQi.Text);

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

                // 3. 获取资产对象（借出中的记录）
                GuDingZiChan jieChuZiChan = GuiHuanManager.HuoQuZiChanXiangXiXinXi_BLL(xuanZeZiChanID);
                if (jieChuZiChan == null)
                {
                    ShowError("获取资产信息失败！");
                    return;
                }

                // 4. 获取归还数量
                int guiHuanShuLiang;
                if (!int.TryParse(txtGuiHuanShuLiang.Text, out guiHuanShuLiang))
                {
                    ShowError("请输入有效的归还数量！");
                    return;
                }

                // 5. 获取其他表单数据
                string guiHuanGongSi = txtGuiHuanGongSi.Text.Trim();

                // 安全地解析日期（使用BLL层）
                DateTime guiHuanRiQi;
                if (!GuiHuanManager.AnQuanJieXiRiQi(txtGuiHuanRiQi.Text, out guiHuanRiQi))
                {
                    ShowError("请输入有效的日期格式！");
                    return;
                }

                // 6. 验证资产数量
                int dangQianJieChuShuLiang;
                if (!int.TryParse(jieChuZiChan.GuZiShuLiang, out dangQianJieChuShuLiang))
                {
                    ShowError("资产数量无效！");
                    return;
                }

                // 7. 业务逻辑验证（使用BLL层）
                string yeWuYanZhengJieGuo = GuiHuanManager.YanZhengGuiHuanYeWuLuoJi(
                    jieChuZiChan,
                    guiHuanShuLiang,
                    dangQianJieChuShuLiang,
                    guiHuanRiQi);

                if (!string.IsNullOrEmpty(yeWuYanZhengJieGuo))
                {
                    ShowError(yeWuYanZhengJieGuo);
                    return;
                }

                // 8. 保存到数据库（使用BLL层）
                bool chengGong = GuiHuanManager.BaoCunGuiHuanJiLu_BLL(
                    jieChuZiChan,
                    guiHuanShuLiang,
                    guiHuanGongSi,
                    guiHuanRiQi);

                if (chengGong)
                {
                    ShowSuccess("归还记录保存成功！");
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
            Response.Redirect("GuZiJieHuan.aspx");
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
                txtGuiHuanShuLiang.Text = "";
                txtGuiHuanGongSi.Text = "";

                // 重置归还日期为今天
                txtGuiHuanRiQi.Text = DateTime.Now.ToString("yyyy-MM-dd");
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