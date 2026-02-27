using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;

namespace WebApplication1.GuDinZiChan.GuZiJieHuan
{
    public partial class GuZiJieHuan : System.Web.UI.Page
    {
        /// <summary>
        /// 每页显示4条记录
        /// </summary>
        private int meiYeShuLiang = 4;
        /// <summary>
        /// 当前页码
        /// </summary>
        private int dangQianYe = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        private int zongYeShu = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        private int zongJiLuShu = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 初始化页码为1
                lblDangQianYe.Text = "1";
                BangDingYongHuShuJu();
            }
        }

        // 绑定用户数据
        private void BangDingYongHuShuJu()
        {
            // 获取当前页码
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);

            // 获取搜索条件
            string guZiID = txtZiChanBianHao.Value;
            string guZiMing = txtZiChanMing.Value;
            string leixing = ddlJieHuanLeiXing.Value;

            // 使用BLL层获取总记录数
            zongJiLuShu = JieHuanManager.HuoQuZongJiLuShu_BLL(guZiID, guZiMing, leixing);

            // 计算分页参数
            JieHuanManager.JiSuanFenYeCanShu(ref dangQianYe, ref zongYeShu, zongJiLuShu, meiYeShuLiang);

            // 计算开始位置
            int kaishiweizhi = (dangQianYe - 1) * meiYeShuLiang;

            // 获取数据并绑定
            DataTable dt = JieHuanManager.HuoQuJieHuanShuJu_BLL(guZiID, guZiMing, leixing, kaishiweizhi, meiYeShuLiang);
            RepeaterJieHuan.DataSource = dt;
            RepeaterJieHuan.DataBind();

            //更新分页页数
            lblDangQianYe.Text = dangQianYe.ToString();
            lblZongYeShu.Text = zongYeShu.ToString();

            // 显示分页控件
            divFenYe.Visible = dt.Rows.Count > 0 || zongJiLuShu > 0;

            // 设置按钮状态
            bool shouYeKeYong = false, shangYiYeKeYong = false, xiaYiYeKeYong = false, moYeKeYong = false;
            JieHuanManager.SheZhiAnNiuZhuangTai(ref shouYeKeYong, ref shangYiYeKeYong,
                                              ref xiaYiYeKeYong, ref moYeKeYong,
                                              dangQianYe, zongYeShu);

            btnShouYe.Enabled = shouYeKeYong;
            btnShangYiYe.Enabled = shangYiYeKeYong;
            btnXiaYiYe.Enabled = xiaYiYeKeYong;
            btnMoYe.Enabled = moYeKeYong;
        }

        //搜索
        protected void btnSouSuo_Click(object sender, EventArgs e)
        {
            // 搜索时回到第一页
            lblDangQianYe.Text = "1";
            BangDingYongHuShuJu();
        }

        //清空
        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtZiChanBianHao.Value = "";
            txtZiChanMing.Value = "";
            ddlJieHuanLeiXing.Value = "";
            // 清空后回到第一页
            lblDangQianYe.Text = "1";
            BangDingYongHuShuJu();
        }

        /// <summary>
        /// 首页
        /// </summary>
        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            // 跳转到第一页
            lblDangQianYe.Text = "1";
            BangDingYongHuShuJu();
        }

        /// <summary>
        /// 上一页
        /// </summary>
        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);
            if (dangQianYe > 1)
            {
                lblDangQianYe.Text = (dangQianYe - 1).ToString();
                BangDingYongHuShuJu();
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        protected void btnXiaYiYe_Click(object sender, EventArgs e)
        {
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);

            if (dangQianYe < zongYeShu)
            {
                lblDangQianYe.Text = (dangQianYe + 1).ToString();
                BangDingYongHuShuJu();
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);
            lblDangQianYe.Text = zongYeShu.ToString();
            BangDingYongHuShuJu();
        }
    }
}