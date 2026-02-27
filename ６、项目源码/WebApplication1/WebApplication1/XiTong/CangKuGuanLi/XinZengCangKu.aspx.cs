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
    public partial class XinZengCangKu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    // 绑定仓库分类下拉框数据
                    BangDingCangKuFenLei();
                    PanXiaoXi.Visible = false; // 默认隐藏消息面板
                }
            }
        }
        private void BangDingCangKuFenLei()
        {
            // 查询仓库分类数据
            DataTable dt = CangKuGuanLiManager.ChaXunCangKuFenLei();

            XiaLaCangKuFenLei.DataSource = dt;
            XiaLaCangKuFenLei.DataTextField = "LeiXingI"; // 显示字段
            XiaLaCangKuFenLei.DataValueField = "LeiXingID"; // 值字段
            XiaLaCangKuFenLei.DataBind();

            // 添加默认选项
            XiaLaCangKuFenLei.Items.Insert(0, new ListItem("--请选择仓库分类--", ""));
        }

        protected void AnNiuChuangJian_Click(object sender, EventArgs e)
        {
            // 1. 清除之前的消息
            PanXiaoXi.Visible = false;
            LabXiaoXi.Text = "";

            try
            {
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

                // 3. 获取输入值
                string cangKuMing = WenBenCangKuMingCheng.Text.Trim();
                string leiXingID = XiaLaCangKuFenLei.SelectedValue;

                if (string.IsNullOrEmpty(leiXingID))
                {
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "请选择仓库分类！";
                    PanXiaoXi.Visible = true;
                    return;
                }

                int leiXingId = int.Parse(leiXingID);

                // 4. 检查仓库是否已存在（仅检查同一分类下）
                bool exists = CangKuGuanLiManager.ChaXunCangKuShiFouCunZai(cangKuMing, leiXingId);
                if (exists)
                {
                    // 获取选中的分类名称用于友好提示
                    string selectedTypeName = XiaLaCangKuFenLei.SelectedItem.Text;

                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = $"在【{selectedTypeName}】分类下已存在同名仓库，请使用其他名称！";
                    PanXiaoXi.Visible = true;
                    return;
                }

                // 5. 调用BLL层方法添加仓库
                bool result = CangKuGuanLiManager.XinZenCangKuDongXi(cangKuMing, leiXingId);

                if (result)
                {
                    // 插入成功，显示成功消息并跳转
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cheng-gong";
                    LabXiaoXi.Text = "仓库创建成功！正在跳转...";
                    PanXiaoXi.Visible = true;

                    // 清空输入框
                    WenBenCangKuMingCheng.Text = "";

                    // 添加JavaScript跳转
                    string script = "setTimeout(function() { window.location.href = 'ZiChanTongJi.aspx'; }, 1500);";
                    ClientScript.RegisterStartupScript(this.GetType(), "RedirectScript", script, true);
                }
                else
                {
                    // 插入失败 - 使用友好提示
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "仓库创建失败，请检查输入信息或稍后重试！";
                    PanXiaoXi.Visible = true;
                }
            }
            catch (FormatException)
            {
                // 数字格式转换错误
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "分类ID格式不正确！";
                PanXiaoXi.Visible = true;
            }
            catch (Exception ex)
            {
                // 异常处理 - 使用友好的通用错误提示，不显示技术细节
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "改名称以被占用！";
                PanXiaoXi.Visible = true;

                // 记录详细错误到日志
                System.Diagnostics.Debug.WriteLine("新增仓库时发生错误: " + ex.Message);
            }
        }
        
        private string ValidateInput()
        {
            // 验证仓库分类
            if (string.IsNullOrEmpty(XiaLaCangKuFenLei.SelectedValue))
                return "请选择仓库分类！";

            // 验证仓库名称
            if (string.IsNullOrWhiteSpace(WenBenCangKuMingCheng.Text))
                return "仓库名称不能为空！";

            string cangKuMing = WenBenCangKuMingCheng.Text.Trim();

            if (cangKuMing.Length > 50)
                return "仓库名称不能超过50个字符！";

            // 验证仓库名称格式（只能包含中文、字母、数字）
            if (!System.Text.RegularExpressions.Regex.IsMatch(cangKuMing, @"^[\u4e00-\u9fa5a-zA-Z0-9]+$"))
                return "仓库名称只能包含中文、字母和数字！";

            return ""; // 验证通过
        }
    }
}

    
