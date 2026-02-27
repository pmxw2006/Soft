using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using DAL;

namespace WebApplication1.GuDinZiChan.GeRenZhongXin
{
    public partial class GeRenXinXi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["ID"] != null)
            {
                // 使用 BLL 层的方法获取用户信息
                YongHu yongHu = BLL.YongHuManager.HuoQuGeRenXinXi_BLL(Session["ID"].ToString());
                if (yongHu != null)
                {
                    WenBenYongHuZhangHao.Text = yongHu.YongHuID;
                    WenBenYongHuMing.Text = yongHu.YongHuName;
                    WenBenBuMen.Text = yongHu.BuMen;
                    WenBenXingMing.Text = yongHu.XingMing;
                    WenBenShenFenZheng.Text = yongHu.ShenFenZheng;
                }
            }
        }

        protected void AnNiuBaoCun_Click(object sender, EventArgs e)
        {
            // 使用 BLL 层的方法修改用户信息
            bool result = BLL.YongHuManager.XiuGaiGeRenXinXi_BLL(
                Session["ID"].ToString(),
                WenBenYongHuMing.Text,
                WenBenXingMing.Text
            );

            // 重新查询并显示更新后的信息
            YongHu yongHu = BLL.YongHuManager.HuoQuGeRenXinXi_BLL(Session["ID"].ToString());
            if (yongHu != null)
            {
                WenBenYongHuZhangHao.Text = yongHu.YongHuID;
                WenBenYongHuMing.Text = yongHu.YongHuName;
                WenBenBuMen.Text = yongHu.BuMen;
                WenBenXingMing.Text = yongHu.XingMing;
                WenBenShenFenZheng.Text = yongHu.ShenFenZheng;
            }
        }
    }
}