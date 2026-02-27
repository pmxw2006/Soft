using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.GuDinZiChan.GuDingZiChanTongJi
{
    public partial class ChaXunZaiKuZiChan : System.Web.UI.Page
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

        private const int MeiYeShuLiang = 10; // 每页显示10条

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 首次加载
                DangQianYe = 1; // 重置为第一页
                LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
            }
        }

        // 获取开始日期
        private DateTime? GetStartDate()
        {
            if (string.IsNullOrEmpty(txtStartDate.Text))
                return null;

            DateTime date;
            if (DateTime.TryParse(txtStartDate.Text, out date))
                return date;
            return null;
        }

        // 获取结束日期
        private DateTime? GetEndDate()
        {
            if (string.IsNullOrEmpty(txtEndDate.Text))
                return null;

            DateTime date;
            if (DateTime.TryParse(txtEndDate.Text, out date))
                return date;
            return null;
        }

        private void LoadData(string keyword, DateTime? startDate, DateTime? endDate)
        {
            // 构建SQL查询条件
            List<string> whereConditions = new List<string>();

            // 添加状态ID=1的条件（在库资产）
            whereConditions.Add("ZhuangTaiID = 1");  // 这行是新增的

            if (!string.IsNullOrEmpty(keyword))
            {
                whereConditions.Add($"(GuZiMing LIKE '%{keyword.Replace("'", "''")}%' OR CangKuMing LIKE '%{keyword.Replace("'", "''")}%')");
            }

            // 添加日期查询条件
            if (startDate.HasValue)
            {
                whereConditions.Add($"ChuChangRiQi >= '{startDate.Value.ToString("yyyy-MM-dd")}'");
            }

            if (endDate.HasValue)
            {
                // 结束日期需要包含当天，所以加1天
                DateTime endDatePlusOne = endDate.Value.AddDays(1);
                whereConditions.Add($"ChuChangRiQi < '{endDatePlusOne.ToString("yyyy-MM-dd")}'");
            }

            string whereClause = whereConditions.Count > 0 ?
                "WHERE " + string.Join(" AND ", whereConditions) : "";


            // 获取总数
            string countSql = $"SELECT COUNT(*) FROM GudinZiChan JOIN CangKu ON GudinZiChan.CangKuID = CangKu.CangKuID {whereClause}";
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
                                {whereClause}
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

        protected void btnSouSuo_Click(object sender, EventArgs e)
        {
            DangQianYe = 1; // 搜索时重置为第一页
            LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
        }

        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            DangQianYe = 1; // 清空时重置为第一页
            LoadData("", null, null);
        }

        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            DangQianYe = 1;
            LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
        }

        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe > 1)
            {
                DangQianYe--;
                LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
            }
        }

        protected void btnXiaYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe < ZongYeShu)
            {
                DangQianYe++;
                LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
            }
        }

        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            if (ZongYeShu > 0)
            {
                DangQianYe = ZongYeShu;
                LoadData(txtSearch.Text, GetStartDate(), GetEndDate());
            }
        }
    }
}
