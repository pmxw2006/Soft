using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.XiTong.CangKuGuanLi
{
    public partial class ZiChanTongJi : System.Web.UI.Page
    {
        // 在类中添加分页相关变量
        private int dangQianYe = 1; // 当前页码
        private int zongYeShu = 0;  // 总页数
        private int zongJiLuShu = 0; // 总记录数
        private int meiYeShuLiang = 4; // 每页显示4条
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                

                // 初始化页码为1
                lblDangQianYe.Text = "1";

                // 绑定下拉框数据 - 添加"全部分类"选项
                DataTable dt = CangKuGuanLiManager.ChaXunCangKu();
                DropDownList1.Items.Clear(); // 清空现有项

                // 添加"全部分类"选项
                DropDownList1.Items.Add(new ListItem("全部分类", ""));

                // 添加数据库中的分类
                DataView dv = new DataView(dt);
                dv.RowFilter = "LeiXingI IS NOT NULL AND LeiXingI <> ''";
                dv.Sort = "LeiXingI";
                DataTable distinctTypes = dv.ToTable(true, "LeiXingI");

                foreach (DataRow row in distinctTypes.Rows)
                {
                    string typeName = row["LeiXingI"].ToString();
                    DropDownList1.Items.Add(new ListItem(typeName, typeName));
                }

                // 绑定数据
                BangDingShuJu();
            }
        }

        // 修改搜索按钮的Click事件
        protected void Button1_Click(object sender, EventArgs e)
        {
            string BianHao = TextBox2.Text.Trim();
            string CangKuMing = TextBox3.Text.Trim();
            string LeiXing = DropDownList1.SelectedValue; // 获取选中的分类值

            // 搜索时回到第一页
            lblDangQianYe.Text = "1";

            // 存储搜索条件到Session或ViewState
            ViewState["SearchBianHao"] = BianHao;
            ViewState["SearchCangKuMing"] = CangKuMing;
            ViewState["SearchLeiXing"] = LeiXing;

            // 根据搜索条件重新绑定数据
            BangDingShuJuWithSearch();
        }

        // 新增方法：带搜索条件的数据绑定
        private void BangDingShuJuWithSearch()
        {
            // 获取搜索条件
            string BianHao = ViewState["SearchBianHao"] as string ?? "";
            string CangKuMing = ViewState["SearchCangKuMing"] as string ?? "";
            string LeiXing = ViewState["SearchLeiXing"] as string ?? "";

            // 获取当前页码
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);

            // 获取所有数据（根据搜索条件）
            DataTable quanBuShuJu;
            if (string.IsNullOrEmpty(LeiXing) && string.IsNullOrEmpty(BianHao) && string.IsNullOrEmpty(CangKuMing))
            {
                // 没有搜索条件，获取所有数据
                quanBuShuJu = CangKuGuanLiManager.ChaXunCangKu();
            }
            else
            {
                // 有搜索条件，调用模糊查询
                quanBuShuJu = CangKuGuanLiManager.ChaXunCangKuMoHu(BianHao, CangKuMing);

                // 如果有选中分类，就进行筛选
                if (!string.IsNullOrEmpty(LeiXing))
                {
                    DataView dv = quanBuShuJu.DefaultView;
                    dv.RowFilter = $"LeiXingI = '{LeiXing}'";
                    quanBuShuJu = dv.ToTable();
                }
            }

            zongJiLuShu = quanBuShuJu.Rows.Count;

            // 计算总页数
            zongYeShu = (int)Math.Ceiling(zongJiLuShu * 1.0 / meiYeShuLiang);
            if (zongYeShu == 0) zongYeShu = 1; // 至少有一页

            // 确保当前页在有效范围内
            if (dangQianYe < 1) dangQianYe = 1;
            if (dangQianYe > zongYeShu) dangQianYe = zongYeShu;

            // 分页：取当前页的数据
            DataTable dangQianYeShuJu = new DataTable();
            dangQianYeShuJu = quanBuShuJu.Clone(); // 复制表结构

            int kaiShiWeiZhi = (dangQianYe - 1) * meiYeShuLiang;
            int jieShuWeiZhi = Math.Min(kaiShiWeiZhi + meiYeShuLiang, zongJiLuShu);

            for (int i = kaiShiWeiZhi; i < jieShuWeiZhi; i++)
            {
                dangQianYeShuJu.ImportRow(quanBuShuJu.Rows[i]);
            }

            Repeater1.DataSource = dangQianYeShuJu;
            Repeater1.DataBind();

            // 更新分页信息
            lblDangQianYe.Text = dangQianYe.ToString();
            lblZongYeShu.Text = zongYeShu.ToString();

            // 显示分页控件
            divFenYe.Visible = true;

            // 设置按钮状态
            
        }
        // 修改清空按钮事件
        protected void btnMoYe_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            TextBox2.Text = "";
            TextBox3.Text = "";
            DropDownList1.SelectedIndex = 0; // 选择"全部分类"

            // 清空存储的搜索条件
            ViewState["SearchBianHao"] = "";
            ViewState["SearchCangKuMing"] = "";
            ViewState["SearchLeiXing"] = "";

            // 清空后回到第一页
            lblDangQianYe.Text = "1";

            // 重新绑定所有数据
            BangDingShuJu();
        }

        // 修改分页按钮事件，使其支持搜索条件
        protected void btnShouYe_Click(object sender, EventArgs e)
        {
            // 跳转到第一页
            lblDangQianYe.Text = "1";
            if (HasSearchCondition())
            {
                BangDingShuJuWithSearch();
            }
            else
            {
                BangDingShuJu();
            }
        }

        protected void btnShangYiYe_Click(object sender, EventArgs e)
        {
            // 上一页
            dangQianYe = Convert.ToInt32(lblDangQianYe.Text);
            if (dangQianYe > 1)
            {
                lblDangQianYe.Text = (dangQianYe - 1).ToString();
                if (HasSearchCondition())
                {
                    BangDingShuJuWithSearch();
                }
                else
                {
                    BangDingShuJu();
                }
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
                if (HasSearchCondition())
                {
                    BangDingShuJuWithSearch();
                }
                else
                {
                    BangDingShuJu();
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // 末页
            zongYeShu = Convert.ToInt32(lblZongYeShu.Text);
            lblDangQianYe.Text = zongYeShu.ToString();
            if (HasSearchCondition())
            {
                BangDingShuJuWithSearch();
            }
            else
            {
                BangDingShuJu();
            }
        }

        // 新增辅助方法：检查是否有搜索条件
        private bool HasSearchCondition()
        {
            string BianHao = ViewState["SearchBianHao"] as string ?? "";
            string CangKuMing = ViewState["SearchCangKuMing"] as string ?? "";
            string LeiXing = ViewState["SearchLeiXing"] as string ?? "";

            return !(string.IsNullOrEmpty(BianHao) && string.IsNullOrEmpty(CangKuMing) && string.IsNullOrEmpty(LeiXing));
        }

        // 修改BangDingShuJu方法，确保在没有搜索条件时使用
        private void BangDingShuJu()
        {
            // 清空搜索条件
            ViewState["SearchBianHao"] = "";
            ViewState["SearchCangKuMing"] = "";
            ViewState["SearchLeiXing"] = "";

            // 确保下拉框默认选中"全部分类"
            if (DropDownList1.Items.Count > 0)
            {
                DropDownList1.SelectedIndex = 0;
            }

            // 清空文本框
            TextBox2.Text = "";
            TextBox3.Text = "";

            // 重新获取所有数据
            BangDingShuJuWithSearch();
        }
    }
}
