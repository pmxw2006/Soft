using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class JieHuanService
    {
        /// <summary>
        /// 获取借用记录列表（带分页和搜索）
        /// </summary>
        public static DataTable HuoQuJieHuanShuJu(string guZiID, string guZiMing, string leixing, int kaishiweizhi, int meiYeShuLiang)
        {
            string sql = @"
                SELECT JieHuan.GuZiID, GuZiMing, GongSi, ShuLiang, RiQi, LeiXin as LeiXing 
                FROM JieHuan 
                JOIN GudinZiChan ON JieHuan.GuZiID = GudinZiChan.GuZiID";

            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(guZiID))
            {
                tiaoJian.Add("JieHuan.GuZiID like @GuZiID");
                canShu.Add(new SqlParameter("@GuZiID", "%" + guZiID + "%"));
            }

            if (!string.IsNullOrEmpty(guZiMing))
            {
                tiaoJian.Add("GuZiMing like @GuZiMing");
                canShu.Add(new SqlParameter("@GuZiMing", "%" + guZiMing + "%"));
            }

            if (!string.IsNullOrEmpty(leixing))
            {
                tiaoJian.Add("LeiXin = @LeiXin");
                canShu.Add(new SqlParameter("@LeiXin", leixing));
            }

            if (tiaoJian.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", tiaoJian);
            }

            sql += " ORDER BY JieHuan.GuZiID";

            // 添加分页参数
            sql += " OFFSET @kaishiweizhi ROWS FETCH NEXT @meiYeShuLiang ROWS ONLY";
            canShu.Add(new SqlParameter("@kaishiweizhi", kaishiweizhi));
            canShu.Add(new SqlParameter("@meiYeShuLiang", meiYeShuLiang));

            return DBHelper.GetDataTable(sql, canShu.ToArray());
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        public static int HuoQuZongJiLuShu(string guZiID, string guZiMing, string leixing)
        {
            string sql = "SELECT COUNT(*) FROM JieHuan JOIN GudinZiChan ON JieHuan.GuZiID = GudinZiChan.GuZiID";

            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(guZiID))
            {
                tiaoJian.Add("JieHuan.GuZiID like @GuZiID");
                canShu.Add(new SqlParameter("@GuZiID", "%" + guZiID + "%"));
            }

            if (!string.IsNullOrEmpty(guZiMing))
            {
                tiaoJian.Add("GuZiMing like @GuZiMing");
                canShu.Add(new SqlParameter("@GuZiMing", "%" + guZiMing + "%"));
            }

            if (!string.IsNullOrEmpty(leixing))
            {
                tiaoJian.Add("LeiXin = @LeiXin");
                canShu.Add(new SqlParameter("@LeiXin", leixing));
            }

            if (tiaoJian.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", tiaoJian);
            }

            DataTable shuLiangBiao = DBHelper.GetDataTable(sql, canShu.ToArray());
            return Convert.ToInt32(shuLiangBiao.Rows[0][0]);
        }
    }
}