using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

namespace WebApplication1.XiTong.RenYuanGuanLi
{
    public partial class TianJiaZhangHu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTianJia_Click(object sender, EventArgs e)
        {
            YongHu yongHu = new YongHu();
            yongHu.YongHuName = TextBox1.Text;
            yongHu.XingMing = txtXingMing.Text;
            yongHu.ShenFenZheng = txtShenFenZheng.Text;
            yongHu.BuMen = ddlBuMen.Text;
            yongHu.MiMa = BLL.YongHuManager.JiaMi(this.txtMiMa.Text.Trim());

            try
            {
                // 使用BLL层方法
                bool panduan1 = BLL.YongHuManager.TianJiaYongHu_BLL(yongHu);

                if (panduan1)  // 这里直接判断布尔值
                {
                    // 显示成功提示
                    divChengGong.Visible = true;
                    divChengGong.InnerText = "用户添加成功！";
                    divCuoWu.Visible = false;

                    // 添加JavaScript效果
                    string script = "<script type='text/javascript'>";
                    script += "$(function() {";
                    script += "  $('#" + divChengGong.ClientID + "').fadeIn(300);";
                    script += "  setTimeout(function() {";
                    script += "    $('#" + divChengGong.ClientID + "').fadeOut(300);";
                    script += "  }, 3000);"; // 3秒后自动消失
                    script += "});";
                    script += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccess", script);
                    // 清空表单
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                // 显示错误提示
                ShowError($"添加用户失败: {ex.Message}");
            }
        }

        protected void btnFanHui_Click(object sender, EventArgs e)
        {
            Response.Redirect("/XiTong/RenYuanGuanLi/RenYuanGuanLi.aspx");
        }

        private void ShowError(string errorMessage)
        {
            // 显示错误提示
            divCuoWu.Visible = true;
            divCuoWu.InnerText = errorMessage;
            divChengGong.Visible = false;

            // 添加JavaScript效果（与成功提示类似）
            string script = "<script type='text/javascript'>";
            script += "$(function() {";
            script += "  $('#" + divCuoWu.ClientID + "').fadeIn(300);";
            script += "  setTimeout(function() {";
            script += "    $('#" + divCuoWu.ClientID + "').fadeOut(300);";
            script += "  }, 3000);"; // 3秒后自动消失
            script += "});";
            script += "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script);
        }

        private void ClearForm()
        {
            try
            {
                // 清空文本框
                TextBox1.Text = "";
                txtXingMing.Text = "";
                txtShenFenZheng.Text = "";
                txtMiMa.Text = "";
                txtQueRenMiMa.Text = "";

                // 重置下拉列表
                ddlBuMen.SelectedIndex = 0;

                // 清除任何可能的ViewState或隐藏字段
                ViewState.Clear();
            }
            catch (Exception ex)
            {
                // 记录错误，但不影响主流程
                System.Diagnostics.Debug.WriteLine("清空表单时出错：" + ex.Message);
            }
        }
    }
}