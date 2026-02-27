using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;

namespace WebApplication1.XiTong.GeRenZhongXin
{
    public partial class XiuGaiMiMa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AnNiuBaoCun_Click(object sender, EventArgs e)
        {
            YongHu yongHu = new YongHu();
            yongHu.YongHuID = Session["ID"].ToString();
            yongHu.MiMa = BLL.YongHuManager.JiaMi(this.WenBenDangQianMiMa.Text.Trim());
            if (YongHuManager.ChaXun_gai_BLL(yongHu))
            {
                yongHu.MiMa = BLL.YongHuManager.JiaMi(this.WenBenXinMiMa.Text.Trim());
                // 执行修改并处理结果
                if (YongHuManager.XiuGai_wang_BLL(yongHu))
                {
                    Response.Write("<script>alert('密码修改成功');</script>");
                    Response.Redirect("/XiTong/GeRenZhongXin/GeRenXinXi.aspx");
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
    }
}