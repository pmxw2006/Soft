using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Model;

namespace WebApplication1.XiTong.RenYuanGuanLi
{
    public partial class RenYuanGuanLi : System.Web.UI.Page
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
        /// <summary>
        /// 总记录数%每页显示4条记录
        /// </summary>
        private int qumo = 0;
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

            // 创建YongHu对象来传递搜索条件
            YongHu souSuoTiaoJian = new YongHu
            {
                // 使用YongHuID属性存储用户账号搜索条件
                YongHuID = txtYongHuZhangHao.Value,
                XingMing = txtXingMing.Value,
                BuMen = ddlBuMen.Value
            };

            // 使用BLL层获取总记录数（使用YongHu对象）
            zongJiLuShu = BLL.YongHuManager.HuoQuZongJiLuShu_BLL(souSuoTiaoJian);

            //计算总页数
            zongYeShu = (int)(zongJiLuShu / meiYeShuLiang);
            qumo = (int)(zongJiLuShu % meiYeShuLiang);
            //如果取模大于0则代表有余数则总页数+1
            if (qumo > 0)
            {
                zongYeShu++;
            }
            //如果总页数为0则+1
            if (zongYeShu == 0)
            {
                zongYeShu = 1;
            }
            // 确保当前页在有效范围内
            if (dangQianYe < 1)
            {
                dangQianYe = 1;
            }
            if (dangQianYe > zongYeShu)
            {
                dangQianYe = zongYeShu;
            }
            //计算开始位置
            int kaishiweizhi = (int)(dangQianYe - 1) * meiYeShuLiang;

            // 使用BLL层获取数据（使用YongHu对象）
            DataTable biaodan = BLL.YongHuManager.HuoQuYongHuShuJu_BLL(
                souSuoTiaoJian, kaishiweizhi, meiYeShuLiang);

            RepeaterYongHu.DataSource = biaodan;
            RepeaterYongHu.DataBind();

            //更新分页页数
            lblDangQianYe.Text = dangQianYe.ToString();
            lblZongYeShu.Text = zongYeShu.ToString();

            // 显示分页控件
            divFenYe.Visible = true;

            // 设置按钮状态
            SheZhiAnNiuZhuangTai();
        }

        // 设置分页按钮状态
        private void SheZhiAnNiuZhuangTai()
        {
            // 如果是第一页，禁用首页和上一页
            btnShouYe.Enabled = (dangQianYe > 1);
            btnShangYiYe.Enabled = (dangQianYe > 1);

            // 如果是最后一页，禁用下一页和末页
            btnXiaYiYe.Enabled = (dangQianYe < zongYeShu);
            btnMoYe.Enabled = (dangQianYe < zongYeShu);
        }

        // 搜索按钮点击事件
        protected void btnSouSuo_Click(object sender, EventArgs e)
        {
            // 搜索时回到第一页
            lblDangQianYe.Text = "1";
            //根据搜索条件查询用户
            BangDingYongHuShuJu();
        }

        // 清空按钮点击事件
        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtYongHuZhangHao.Value = "";
            txtXingMing.Value = "";
            ddlBuMen.Value = "";
            // 清空后回到第一页
            lblDangQianYe.Text = "1";
            BangDingYongHuShuJu();
        }

        // 分页按钮事件
        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            // 跳转到第一页
            lblDangQianYe.Text = "1";
            BangDingYongHuShuJu();
        }

        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            // 上一页
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);
            if (dangQianYe > 1)
            {
                lblDangQianYe.Text = (dangQianYe - 1).ToString();
                BangDingYongHuShuJu();
            }
        }

        protected void btnXiaYiYe_Click(object sender, EventArgs e)
        {
            // 下一页
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);

            if (dangQianYe < zongYeShu)
            {
                lblDangQianYe.Text = (dangQianYe + 1).ToString();
                BangDingYongHuShuJu();
            }
        }

        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            // 末页
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);
            lblDangQianYe.Text = zongYeShu.ToString();
            BangDingYongHuShuJu();
        }

        // 删除按钮命令事件
        protected void btnShanChu_Command(object sender, EventArgs e)
        {
            string yongHuID = tckHiddenZhangHaoInfo.Text;

            try
            {
                // 使用BLL层执行删除操作
                bool jieGuo = BLL.YongHuManager.ShanChuYongHu_BLL(yongHuID);

                if (jieGuo)
                {
                    // 重新绑定数据
                    BangDingYongHuShuJu();
                }
            }
            catch (Exception ex)
            {
                // 错误提示
                string script = $"<script>alert('删除时出错：{ex.Message}');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "error", script);
            }
        }
    }
}