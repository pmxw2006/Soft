using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.XiTong.CangKuGuanLi
{
    public partial class CangKuXioGai : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 加载原有的仓库名称用于显示
                string cangKuID = Request.QueryString["CangKuID"];
                if (!string.IsNullOrEmpty(cangKuID))
                {
                    try
                    {
                        // 从数据库获取原仓库名称并显示
                        string sql = $"SELECT CangKuMing FROM CangKu WHERE CangKuID = '{cangKuID}'";
                        DataTable dt = DBHelper.GetDataTable(sql);

                        if (dt.Rows.Count > 0)
                        {
                            string yuanCangKuMing = dt.Rows[0]["CangKuMing"].ToString();

                            // 显示原仓库名称提示
                            PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-ti-shi";
                            LabXiaoXi.Text = "提示：当前要修改的仓库名称是 <strong>" + yuanCangKuMing + "</strong>。请在下方的输入框中输入新的仓库名称。";
                            PanXiaoXi.Visible = true;

                            // 保存原名称到ViewState，供比较使用
                            ViewState["YuanCangKuMing"] = yuanCangKuMing;

                            // 设置输入框提示
                            WenBenCangKuMing.Attributes["placeholder"] = "请输入新的仓库名称（原名称：" + yuanCangKuMing + "）";
                        }
                        else
                        {
                            PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                            LabXiaoXi.Text = "提示：未找到指定的仓库信息。";
                            PanXiaoXi.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                        LabXiaoXi.Text = "提示：加载仓库信息时出现错误，请稍后重试。";
                        PanXiaoXi.Visible = true;
                    }
                }
                else
                {
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "提示：未指定要修改的仓库，请从仓库管理页面选择要修改的仓库。";
                    PanXiaoXi.Visible = true;
                }
            }
        }

        protected void AnNiuGengGai_Click(object sender, EventArgs e)
        {
            // 获取用户输入
            string xinCangKuMing = WenBenCangKuMing.Text.Trim();

            // 获取原仓库名称
            string yuanCangKuMing = ViewState["YuanCangKuMing"] as string;

            // 验证输入是否为空
            if (string.IsNullOrEmpty(xinCangKuMing))
            {
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "温馨提示：新的仓库名称不能为空哦，请输入您想要修改的名称。";
                PanXiaoXi.Visible = true;
                return;
            }

            // 验证长度
            if (xinCangKuMing.Length > 50)
            {
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "温馨提示：仓库名称太长了，请控制在50个字符以内。";
                PanXiaoXi.Visible = true;
                return;
            }

            // 检查是否与原名称相同
            if (!string.IsNullOrEmpty(yuanCangKuMing) && xinCangKuMing == yuanCangKuMing)
            {
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-ti-shi";
                LabXiaoXi.Text = "温馨提示：您输入的名称与原仓库名称相同，无需修改。<br>如果您确定要修改，请输入不同的名称。";
                PanXiaoXi.Visible = true;
                return;
            }

            // 执行修改操作
            string CanShu = Request.QueryString["CangKuID"];
            string CangKuXingMin = xinCangKuMing;

            try
            {
                CangKuGuanLiManager.CangKuXiGai(CanShu, CangKuXingMin);

                // 成功提示 - 显示友好信息
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cheng-gong";
                LabXiaoXi.Text = "修改成功！<br>仓库名称已从 <strong>" + yuanCangKuMing + "</strong> 修改为 <strong>" + xinCangKuMing + "</strong>。<br>页面将在3秒后自动返回仓库管理页面...";
                PanXiaoXi.Visible = true;

                // 清空输入框
                WenBenCangKuMing.Text = "";

                // 3秒后跳转
                ScriptManager.RegisterStartupScript(this, GetType(), "redirect",
                    "setTimeout(function(){ window.location.href = 'ZiChanTongJi.aspx'; }, 3000);", true);
            }
            catch (Exception ex)
            {
                // 失败提示 - 显示友好的错误信息
                string errorMessage = "很抱歉，修改失败了！";

                if (ex.Message.Contains("重复") || ex.Message.Contains("已存在"))
                {
                    errorMessage = "修改失败：这个仓库名称已经存在了，请尝试使用其他名称。";
                }
                else if (ex.Message.Contains("外键") || ex.Message.Contains("关联"))
                {
                    errorMessage = "修改失败：该仓库可能正在使用中，暂时无法修改。";
                }
                else if (ex.Message.Contains("数据库") || ex.Message.Contains("连接"))
                {
                    errorMessage = "修改失败：系统暂时无法连接到数据库，请稍后重试。";
                }
                else
                {
                    errorMessage = "修改失败：" + ex.Message;
                }

                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = errorMessage;
                PanXiaoXi.Visible = true;
            }
        }
    }
}