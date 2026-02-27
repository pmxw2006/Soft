using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.GuDinZiChan.GuDinZiChanChaoZuo
{
    public partial class XinZenZiChan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = "SELECT CangKuID, CangKuMing FROM CangKu";
                XiaLaSuoShuCangKu.DataSource = DBHelper.GetDataTable(sql);
                XiaLaSuoShuCangKu.DataTextField = "CangKuMing";
                XiaLaSuoShuCangKu.DataValueField = "CangKuID";
                XiaLaSuoShuCangKu.DataBind();
            }

        }

        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <returns>错误消息，如果验证通过返回空字符串</returns>
        private string ValidateInput()
        {
            // 验证资产名称
            if (string.IsNullOrWhiteSpace(WenBenZiChanMingCheng.Text))
                return "资产名称不能为空！";

            if (WenBenZiChanMingCheng.Text.Trim().Length > 50)
                return "资产名称不能超过50个字符！";

            // 验证所属仓库
            if (string.IsNullOrEmpty(XiaLaSuoShuCangKu.SelectedValue))
                return "请选择所属仓库！";

            // 验证仓库ID是否为数字
            if (!int.TryParse(XiaLaSuoShuCangKu.SelectedValue, out int cangKuId))
                return "仓库选择无效！";

            // 验证资产数量
            if (string.IsNullOrWhiteSpace(WenBenZiChanShuLiang.Text))
                return "资产数量不能为空！";

            if (!int.TryParse(WenBenZiChanShuLiang.Text, out int shuLiang))
                return "资产数量必须为整数！";

            if (shuLiang <= 0)
                return "资产数量必须大于0！";

            // 验证资产价值
            if (string.IsNullOrWhiteSpace(WenBenZiChanJiaZhi.Text))
                return "资产价值不能为空！";

            if (!decimal.TryParse(WenBenZiChanJiaZhi.Text, out decimal jiaZhi))
                return "资产价值必须为数字！";

            if (jiaZhi <= 0)
                return "资产价值必须大于0！";

            // 验证出厂日期
            if (string.IsNullOrWhiteSpace(WenBenChuChangRiQi.Text))
                return "出厂日期不能为空！";

            if (!DateTime.TryParse(WenBenChuChangRiQi.Text, out DateTime chuChangRiQi))
                return "出厂日期格式不正确！";

            if (chuChangRiQi > DateTime.Now)
                return "出厂日期不能超过今天！";

            return ""; // 验证通过
        }

        /// <summary>
        /// 使用参数化查询插入资产数据
        /// </summary>
        /// <returns>是否插入成功</returns>

        private bool InsertAsset()
        {
            // 使用参数化查询防止SQL注入
            string sql = @"INSERT INTO GudinZiChan 
                          (GuZiMing, CangKuID, GuZiShuLiang, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID) 
                          VALUES (@GuZiMing, @CangKuID, @GuZiShuLiang, @GuZiJiaZhi, @ChuChangRiQi, 1)";

            // 创建参数数组
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GuZiMing", SqlDbType.NVarChar, 50)
                {
                    Value = WenBenZiChanMingCheng.Text.Trim()
                },
                new SqlParameter("@CangKuID", SqlDbType.Int)
                {
                    Value = int.Parse(XiaLaSuoShuCangKu.SelectedValue)
                },
                new SqlParameter("@GuZiShuLiang", SqlDbType.Int)
                {
                    Value = int.Parse(WenBenZiChanShuLiang.Text)
                },
                new SqlParameter("@GuZiJiaZhi", SqlDbType.Decimal)
                {
                    Value = decimal.Parse(WenBenZiChanJiaZhi.Text)
                },
                new SqlParameter("@ChuChangRiQi", SqlDbType.Date)
                {
                    Value = DateTime.Parse(WenBenChuChangRiQi.Text)
                }
            };

            return DBHelper.ExcNonQuery(sql, parameters);
        }


        protected void btnFanHui_Click(object sender, EventArgs e)
        {
            Response.Redirect("ZiChanZhuTi.aspx");
        }

        protected void AnNiuBaoCun_Click(object sender, EventArgs e)
        {
            string errorMessage = ValidateInput();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // 显示错误消息
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = errorMessage;
                PanXiaoXi.Visible = true;
                return;
            }

            // 3. 验证通过，执行数据库操作（使用参数化查询防止SQL注入）
            try
            {
                if (InsertAsset())
                {
                    // 插入成功，跳转到资产列表页面
                    Response.Redirect("ZiChanZhuTi.aspx");
                }
                else
                {
                    // 插入失败
                    PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                    LabXiaoXi.Text = "资产添加失败，请稍后重试！";
                    PanXiaoXi.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                PanXiaoXi.CssClass = "xiao-xi-pan xiao-xi-cuo-wu";
                LabXiaoXi.Text = "系统错误：" + ex.Message;
                PanXiaoXi.Visible = true;
            }
        }
    }
}