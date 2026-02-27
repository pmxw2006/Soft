using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.GuDinZiChan.GuDingZiChanTongJi
{
    public partial class ChaXunJeiHuanZiChan : System.Web.UI.Page
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
                LoadData();
            }
        }

        private void LoadData()
        {
            // 获取搜索条件
            string ziChanBianHao = txtZiChanBianHao.Value.Trim();
            string ziChanMing = txtZiChanMing.Value.Trim();
            string jieHuanLeiXing = ddlJieHuanLeiXing.Value;

            // 构建SQL查询条件
            List<string> whereConditions = new List<string>();

            if (!string.IsNullOrEmpty(ziChanBianHao))
            {
                whereConditions.Add($"JieHuan.GuZiID LIKE '%{ziChanBianHao.Replace("'", "''")}%'");
            }

            if (!string.IsNullOrEmpty(ziChanMing))
            {
                whereConditions.Add($"GudinZiChan.GuZiMing LIKE '%{ziChanMing.Replace("'", "''")}%'");
            }

            if (!string.IsNullOrEmpty(jieHuanLeiXing))
            {
                whereConditions.Add($"JieHuan.LeiXin = '{jieHuanLeiXing.Replace("'", "''")}'");
            }

            string whereClause = whereConditions.Count > 0 ?
                "WHERE " + string.Join(" AND ", whereConditions) : "";

            // 获取总数
            string countSql = $@"SELECT COUNT(*) 
                        FROM JieHuan 
                        JOIN GudinZiChan ON JieHuan.GuZiID = GudinZiChan.GuZiID
                        {whereClause}";
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
            btnMoYe.Enabled = DangQianYe < ZongYeShu && ZongYeShu > 0;

            // 分页查询
            if (total > 0)
            {
                int startRow = (DangQianYe - 1) * MeiYeShuLiang;
                string sql = $@"SELECT JieHuan.GuZiID, GudinZiChan.GuZiMing, JieHuan.GongSi, 
                               JieHuan.ShuLiang, JieHuan.RiQi, JieHuan.LeiXin as LeiXing
                        FROM JieHuan 
                        JOIN GudinZiChan ON JieHuan.GuZiID = GudinZiChan.GuZiID
                        {whereClause}
                        ORDER BY JieHuan.RiQi DESC 
                        OFFSET {startRow} ROWS FETCH NEXT {MeiYeShuLiang} ROWS ONLY";

                RepeaterJieHuan.DataSource = DBHelper.GetDataTable(sql);
            }
            else
            {
                RepeaterJieHuan.DataSource = null;
            }

            RepeaterJieHuan.DataBind();
        }

        protected void btnSouSuo_Click(object sender, EventArgs e)
        {
            DangQianYe = 1; // 搜索时重置为第一页
            LoadData();
        }

        protected void btnQingKong_Click(object sender, EventArgs e)
        {
            txtZiChanBianHao.Value = "";
            txtZiChanMing.Value = "";
            ddlJieHuanLeiXing.Value = "";
            DangQianYe = 1; // 清空时重置为第一页
            LoadData();
        }

        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            DangQianYe = 1;
            LoadData();
        }

        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe > 1)
            {
                DangQianYe--;
                LoadData();
            }
        }

        protected void btnXiaYiYe_Click(object sender, EventArgs e)
        {
            if (DangQianYe < ZongYeShu)
            {
                DangQianYe++;
                LoadData();
            }
        }

        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            if (ZongYeShu > 0)
            {
                DangQianYe = ZongYeShu;
                LoadData();
            }
        }
    }
}
