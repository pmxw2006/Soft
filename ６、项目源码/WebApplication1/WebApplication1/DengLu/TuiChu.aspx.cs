using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.DengLu
{
    public partial class TuiChu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 清除Cookie
            HttpCookie cookie = new HttpCookie("ID");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            // 清除Session
            Session.Clear();

            // 跳转到登录页
            Response.Redirect("DengLuYe.aspx");
        }
    }
}