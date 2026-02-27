using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using DAL;

namespace WebApplication1.GuDinZiChan.GuDinZiChanChaoZuo
{

    public partial class ZiChanZhuTi : System.Web.UI.Page
    {
        private int DangQianYe
        {
            get
            {
                if (ViewState["DangQianYe"] == null)
                    return 1;
                return Convert.ToInt32(ViewState["DangQianYe"]);
            }
            set { ViewState["DangQianYe"] = value; }
        }

        private int ZongYeShu
        {
            get
            {
                if (ViewState["ZongYeShu"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["ZongYeShu"]);
            }
            set { ViewState["ZongYeShu"] = value; }
        }

        private const int MeiYeShuLiang = 4; // 每页显示4条

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 处理删除操作
                string GuZiID = Request.QueryString["GuZiID"];
                if (GuZiID != null)
                {
                    GuDingZiChanManager.ShanCuCangKu(GuZiID);
                    Response.Redirect("ZiChanZhuTi.aspx");
                    return;
                }

                // 首次加载
                DangQianYe = 1; // 重置为第一页
                LoadData(txtSearch.Text);
            }
        }

        private void LoadData(string keyword)
        {
            // 构建SQL
            string where = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                where = $" WHERE GuZiMing LIKE '%{keyword}%' OR CangKuMing LIKE '%{keyword}%'";
            }

            // 获取总数
            string countSql = $"SELECT COUNT(*) FROM GudinZiChan JOIN CangKu ON GudinZiChan.CangKuID = CangKu.CangKuID {where}";
            DataTable countDt = DBHelper.GetDataTable(countSql);
            int total = countDt.Rows.Count > 0 ? Convert.ToInt32(countDt.Rows[0][0]) : 0;

            // 计算总页数
            ZongYeShu = total > 0 ? (int)Math.Ceiling((double)total / MeiYeShuLiang) : 0;

            // 更新页面显示
            lblDangQianYe.Text = DangQianYe.ToString();
            lblZongYeShu.Text = ZongYeShu.ToString();
            divFenYe.Visible = total > 0;

            // 设置按钮状态
            btnShangYiYe.Enabled = DangQianYe > 1;
            btnXiaYiYe.Enabled = DangQianYe < ZongYeShu && ZongYeShu > 0;
            btnShouYe.Enabled = DangQianYe > 1;
            Button2.Enabled = DangQianYe < ZongYeShu && ZongYeShu > 0;

            // 分页查询
            if (total > 0)
            {
                int startRow = (DangQianYe - 1) * MeiYeShuLiang;
                string sql = $@"SELECT GuZiID, GuZiMing, CangKuMing, GuZiShuLiang, 
                                GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID 
                                FROM GudinZiChan 
                                JOIN CangKu ON GudinZiChan.CangKuID = CangKu.CangKuID
                                {where}
                                ORDER BY GuZiID 
                                OFFSET {startRow} ROWS FETCH NEXT {MeiYeShuLiang} ROWS ONLY";

                Repeater1.DataSource = DBHelper.GetDataTable(sql);
            }
            else
            {
                Repeater1.DataSource = null;
            }

            Repeater1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DangQianYe = 1; // 搜索时重置为第一页
            LoadData(txtSearch.Text);
        }

        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            DangQianYe = 1; // 清空时重置为第一页
            LoadData("");
        }

        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            DangQianYe = 1;
            LoadData(txtSearch.Text);
        }

        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe > 1)
            {
                DangQianYe--;
                LoadData(txtSearch.Text);
            }
        }

        protected void btnXiaYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe < ZongYeShu)
            {
                DangQianYe++;
                LoadData(txtSearch.Text);
            }
        }

        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            if (ZongYeShu > 0)
            {
                DangQianYe = ZongYeShu;
                LoadData(txtSearch.Text);
            }
        }
    }
}