using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;

namespace WebApplication1.MuBanYe
{
    public partial class XiTong : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 检查登录状态和权限
                JianChaDengLuHeQuanXian();
            }
        }
        // 检查登录和权限
        private void JianChaDengLuHeQuanXian()
        {
            // 1. 检查Session
            if (Session["ID"] == null)
            {
                // Session没有，检查Cookie
                if (Request.Cookies["ID"] != null && !string.IsNullOrEmpty(Request.Cookies["ID"].Value))
                {
                    string cookieZhi = Request.Cookies["ID"].Value;

                    // 调用BLL层进行Cookie自动登录
                    DengLuJieGuo jieguo = YongHuManager.ChuLiCookieZiDongDengLu(cookieZhi);

                    if (jieguo.Zhuangtai == "ChengGong")
                    {
                        // 自动登录成功
                        Session["ID"] = jieguo.YongHuID;
                        Session["BuMen"] = jieguo.BuMen;

                        // 检查权限
                        ChuLiQuanXianTiaoZhuan();
                    }
                    else
                    {
                        // Cookie自动登录失败，跳转到登录页
                        Response.Redirect("/DengLu/DengLuYe.aspx");
                    }
                }
                else
                {
                    // 没有Session也没有Cookie，跳转到登录页
                    Response.Redirect("/DengLu/DengLuYe.aspx");
                }
            }
            else
            {
                // 已有Session，检查权限
                ChuLiQuanXianTiaoZhuan();
            }
        }

        // 处理权限跳转
        private void ChuLiQuanXianTiaoZhuan()
        {
            string yongHuID = Session["ID"]?.ToString();
            string buMen = Session["BuMen"]?.ToString();
            string dangQianYeMian = Request.Url.AbsolutePath;

            // 调用BLL层检查权限
            string tiaoZhuanUrl = YongHuManager.JianChaQuanXian(yongHuID, buMen, dangQianYeMian);

            if (!string.IsNullOrEmpty(tiaoZhuanUrl))
            {
                Response.Redirect(tiaoZhuanUrl);
            }
        }
    }
}