using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using BLL;
using Model;

namespace WebApplication1.DengLu
{
    public partial class WangJiMiMa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 尝试自动登录
                ChangShiZiDongDengLu();
            }
        }
        // 自动登录方法
        private void ChangShiZiDongDengLu()
        {
            // 检查是否有Cookie
            if (Request.Cookies["ID"] != null && !string.IsNullOrEmpty(Request.Cookies["ID"].Value))
            {
                string cookieZhi = Request.Cookies["ID"].Value;

                // 调用BLL层处理
                DengLuJieGuo jieguo = YongHuManager.ChuLiCookieZiDongDengLu(cookieZhi);

                if (jieguo.Zhuangtai == "ChengGong")
                {
                    // 登录成功
                    Session["ID"] = jieguo.YongHuID;  // Session存原始ID

                    // 根据部门跳转
                    if (jieguo.BuMen == "系统")
                    {
                        Response.Redirect("/XiTong/ZhuTi.aspx");
                    }
                    else if (jieguo.BuMen == "固定资产")
                    {
                        Response.Redirect("/GuDinZiChan/ZhuTi.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('您的部门输出有疑问请及时反馈否则无法登录');</script>");
                    }
                }
                else if (jieguo.Zhuangtai == "WuXiaoCookie")
                {
                    // Cookie无效，清除它
                    QingChuWuXiaoCookie();
                }
            }
        }
        // 清除无效Cookie
        private void QingChuWuXiaoCookie()
        {
            if (Request.Cookies["ID"] != null)
            {
                HttpCookie wuxiaoCookie = new HttpCookie("ID");
                wuxiaoCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(wuxiaoCookie);
            }
        }
        protected void Submit1_Click(object sender, EventArgs e)
        {
            try
            {

                YongHu yongHu = new YongHu();
                yongHu.YongHuID = this.Text1.Text.Trim();
                yongHu.ShenFenZheng = this.shenfenzheng.Text.Trim();
                yongHu.MiMa = BLL.YongHuManager.JiaMi(this.Password1.Text.Trim());
                if (YongHuManager.ChaXun_wang_BLL(yongHu))
                {

                    // 执行修改并处理结果
                    if (YongHuManager.XiuGai_wang_BLL(yongHu))
                    {
                        Response.Write("<script>alert('密码修改成功');</script>");
                        Response.Redirect("DengLuYe.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('密码修改失败，请重试');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('基本信息不正确无法修改密码');</script>");
                }
            }
            catch (Exception)
            {

                Response.Write("<script>alert('基本信息不正确无法修改密码');</script>");
            }
        }
    }
}