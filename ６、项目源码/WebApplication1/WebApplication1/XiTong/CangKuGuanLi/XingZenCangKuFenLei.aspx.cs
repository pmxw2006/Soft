using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.XiTong.CangKuGuanLi
{
    public partial class XingZenCangKuFenLei : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 页面初始化代码
                WenBenFenLeiMingCheng.Focus(); // 让文本框获得焦点
                PanXiaoXi.Visible = false; // 默认隐藏消息面板
            }
        }
        protected void AnNiuChuangJianFenLei_Click(object sender, EventArgs e)
        {
            // 1. 清除之前的消息
            PanXiaoXi.Visible = false;
            LabXiaoXi.Text = "";

            // 2. 验证输入
            string errorMessage = ValidateInput();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // 显示错误消息
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = errorMessage;
                PanXiaoXi.Visible = true;
                return;
            }

            // 3. 验证通过，执行数据库操作
            try
            {
                string cangkm = WenBenFenLeiMingCheng.Text.Trim();

                // 检查是否已存在相同分类名称 - 调用BLL层的方法
                bool exists = CangKuGuanLiManager.ChaXunFenLeiShiFouCunZai(cangkm);
                if (exists)
                {
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "该分类名称已存在，请使用其他名称！";
                    PanXiaoXi.Visible = true;
                    return;
                }

                // 调用BLL层方法添加分类
                bool result = CangKuGuanLiManager.XinZenCangKu(cangkm);

                if (result)
                {
                    // 插入成功，显示成功消息并跳转
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cheng-gong";
                    LabXiaoXi.Text = "仓库分类创建成功！正在跳转...";
                    PanXiaoXi.Visible = true;

                    // 清空输入框
                    WenBenFenLeiMingCheng.Text = "";

                    // 添加JavaScript跳转
                    string script = "setTimeout(function() { window.location.href = 'ZiChanTongJi.aspx'; }, 1500);";
                    ClientScript.RegisterStartupScript(this.GetType(), "RedirectScript", script, true);
                }
                else
                {
                    // 插入失败
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "仓库分类创建失败，可能是分类名称已存在，请检查后重试！";
                    PanXiaoXi.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "系统错误：" + ex.Message;
                PanXiaoXi.Visible = true;
            }
        }
        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <returns>错误消息，如果验证通过返回空字符串</returns>
        private string ValidateInput()
        {
            // 验证分类名称
            if (string.IsNullOrWhiteSpace(WenBenFenLeiMingCheng.Text))
                return "分类名称不能为空！";

            string fenLeiMingCheng = WenBenFenLeiMingCheng.Text.Trim();

            if (fenLeiMingCheng.Length > 50)
                return "分类名称不能超过50个字符！";

            // 验证分类名称格式（只能包含中文、字母、数字）
            if (!System.Text.RegularExpressions.Regex.IsMatch(fenLeiMingCheng, @"^[\u4e00-\u9fa5a-zA-Z0-9]+$"))
                return "分类名称只能包含中文、字母和数字！";

            return ""; // 验证通过
        }
    }
}