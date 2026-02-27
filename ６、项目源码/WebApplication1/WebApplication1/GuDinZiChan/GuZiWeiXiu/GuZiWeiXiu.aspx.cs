using Mods;
using BLL;
using System.Data;
using System;

namespace WebApplication1.GuDinZiChan.GuZiWeiXiu
{
    public partial class GuZiWeiXiu : System.Web.UI.Page
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
                BangDingWeiXiuShuJu();
            }
        }

        // 绑定维修数据
        private void BangDingWeiXiuShuJu()
        {
            // 获取当前页码
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);

            // 获取搜索条件
            string guZiID = txtZiChanBianHao.Value;
            string guZiMing = txtZiChanMing.Value;

            // 使用BLL层获取总记录数
            zongJiLuShu = WeiXiuManager.HuoQuWeiXiuZongShu_BLL(guZiID, guZiMing);

            // 计算分页参数
            WeiXiuManager.JiSuanFenYeCanShu(ref dangQianYe, ref zongYeShu, zongJiLuShu, meiYeShuLiang);

            // 计算开始位置
            int kaishiweizhi = (dangQianYe - 1) * meiYeShuLiang;

            // 获取数据并绑定（使用BLL层）
            DataTable dt = WeiXiuManager.HuoQuWeiXiuLieBiao_BLL(guZiID, guZiMing, kaishiweizhi, meiYeShuLiang);
            RepeaterWeiXiu.DataSource = dt;
            RepeaterWeiXiu.DataBind();

            // 更新分页页数
            lblDangQianYe.Text = dangQianYe.ToString();
            lblZongYeShu.Text = zongYeShu.ToString();

            // 显示分页控件
            divFenYe.Visible = true;

            // 设置按钮状态
            bool shouYeKeYong = false, shangYiYeKeYong = false, xiaYiYeKeYong = false, moYeKeYong = false;
            WeiXiuManager.SheZhiAnNiuZhuangTai(ref shouYeKeYong, ref shangYiYeKeYong,
                                              ref xiaYiYeKeYong, ref moYeKeYong,
                                              dangQianYe, zongYeShu);

            btnShouYe.Enabled = shouYeKeYong;
            btnShangYiYe.Enabled = shangYiYeKeYong;
            btnXiaYiYe.Enabled = xiaYiYeKeYong;
            btnMoYe.Enabled = moYeKeYong;
        }

        // 搜索
        protected void btnSouSuo_Click(object sender, EventArgs e)
        {
            // 搜索时回到第一页
            lblDangQianYe.Text = "1";
            BangDingWeiXiuShuJu();
        }

        // 清空
        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtZiChanBianHao.Value = "";
            txtZiChanMing.Value = "";
            // 清空后回到第一页
            lblDangQianYe.Text = "1";
            BangDingWeiXiuShuJu();
        }

        // 完修按钮命令事件
        protected void btnShanChu_Command(object sender, EventArgs e)
        {
            try
            {
                string guZiID = hiddenGuZiID.Value;

                // 验证参数（使用BLL层）
                string yanZhengJieGuo = WeiXiuManager.YanZhengWanXiuCanShu(guZiID);
                if (!string.IsNullOrEmpty(yanZhengJieGuo))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + yanZhengJieGuo.Replace("'", "\\'") + "');", true);
                    return;
                }

                // 验证ID是否匹配
                string hiddenGuZiID1 = hiddenGuZiID.Value;
                if (guZiID != hiddenGuZiID1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('固定资产ID不匹配！');", true);
                    return;
                }

                // 使用当天日期作为实际完成日期（使用BLL层）
                DateTime shiWanRiQi = WeiXiuManager.HuoQuDangQianRiQi();

                // 调用完修方法（使用BLL层）
                bool chengGong = WeiXiuManager.WanChengWeiXiu_BLL(guZiID, shiWanRiQi);

                if (chengGong)
                {
                    // 刷新页面
                    BangDingWeiXiuShuJu();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('完修操作失败，请重试！');", true);
                }
            }
            catch (Exception ex)
            {
                string xiaoXi = "完修操作失败：" + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + xiaoXi.Replace("'", "\\'") + "');", true);
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            // 跳转到第一页
            lblDangQianYe.Text = "1";
            BangDingWeiXiuShuJu();
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
                BangDingWeiXiuShuJu();
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
                BangDingWeiXiuShuJu();
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);
            lblDangQianYe.Text = zongYeShu.ToString();
            BangDingWeiXiuShuJu();
        }
    }
}