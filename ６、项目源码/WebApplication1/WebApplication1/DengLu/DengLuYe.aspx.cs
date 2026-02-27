using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using BLL;
using Model;
using DAL;

namespace WebApplication1.DengLu
{
    public partial class DengLuYe : System.Web.UI.Page
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

        // 登录按钮点击事件
        protected void Button1_Click(object sender, EventArgs e)
        {
            // 创建用户对象
            YongHu yonghu = new YongHu();
            yonghu.YongHuID = this.TextBox1.Text.Trim();  // 原始用户ID
            try
            {

                // 使用BLL层加密密码
                yonghu.MiMa = YongHuManager.JiaMi(this.TextBox2.Text.Trim());

                // 使用原有的BLL方法验证登录
                if (YongHuManager.ChaXun_BLL(yonghu))
                {
                    string yuanShiYongHuID = yonghu.YongHuID;  // 保存原始用户ID

                    // 判断是否勾选"记住密码"
                    if (CheckBox1.Checked)
                    {
                        // 对原始用户ID进行加密
                        string jiaMiYongHuID = YongHuManager.JiaMi(yuanShiYongHuID);
                        HttpCookie cookie = new HttpCookie("ID", jiaMiYongHuID);
                        cookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(cookie);
                    }

                    // Session中保存原始用户ID
                    Session["ID"] = yuanShiYongHuID;

                    // 使用DAL层的新方法获取部门信息
                    string bumen = YongHuService.HuoQuYongHuBuMen(yuanShiYongHuID);
                    Session["bumen"] = bumen;
                    if (bumen == "系统")
                    {
                        Response.Redirect("/XiTong/ZhuTi.aspx");
                    }
                    else if (bumen == "固定资产")
                    {
                        Response.Redirect("/GuDinZiChan/ZhuTi.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('您的部门输出有疑问请及时反馈否则无法登录');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('账号密码错误请重新输入');</script>");
                }
            }
            catch (Exception)
            {

                Response.Write("<script>alert('账号密码错误请重新输入');</script>");
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
    }
}