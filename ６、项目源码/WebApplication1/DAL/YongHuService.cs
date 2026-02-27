using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class YongHuService
    {
        public static string ChaXun(YongHu yongHu)
        {
            string YongHuID = null;
            string sql = "select YongHuID from YongHu where YongHuID = @ZhangHu and MiMa = @MiMa";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZhangHu", yongHu.YongHuID),
                new SqlParameter("@MiMa", yongHu.MiMa)
            };

            DataTable panduan = DBHelper.GetDataTable(sql, parameters);

            // 修复：先检查DataTable和Rows是否存在
            if (panduan != null && panduan.Rows.Count > 0)
            {
                // 检查第一行是否存在且不为空
                if (panduan.Rows[0] != null &&
                    panduan.Rows[0]["YongHuID"] != null &&
                    panduan.Rows[0]["YongHuID"] != DBNull.Value)
                {
                    YongHuID = panduan.Rows[0]["YongHuID"].ToString();
                }
            }

            return YongHuID;
        }

        public static string ChaXun_wang(YongHu yongHu)
        {
            string YongHuID = null;
            string sql = "select YongHuID from YongHu where YongHuID = @ZhangHu and ShenFenZheng = @ShenFenZheng";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZhangHu", yongHu.YongHuID),
                new SqlParameter("@ShenFenZheng", yongHu.ShenFenZheng)
            };

            DataTable panduan = DBHelper.GetDataTable(sql, parameters);

            // 修复：先检查DataTable和Rows是否存在
            if (panduan != null && panduan.Rows.Count > 0)
            {
                // 检查第一行是否存在且不为空
                if (panduan.Rows[0] != null &&
                    panduan.Rows[0]["YongHuID"] != null &&
                    panduan.Rows[0]["YongHuID"] != DBNull.Value)
                {
                    string tempID = panduan.Rows[0]["YongHuID"].ToString();
                    if (!string.IsNullOrEmpty(tempID))
                    {
                        YongHuID = tempID;
                    }
                }
            }

            return YongHuID;
        }

        public static bool XiuGai_wang(YongHu yongHu)
        {
            string sql1 = "update YongHu set MiMa = @MiMa where YongHuID = @ZhangHu";

            SqlParameter[] parameters1 =
            {
                new SqlParameter("@MiMa", yongHu.MiMa),
                new SqlParameter("@ZhangHu", yongHu.YongHuID)
            };

            return DBHelper.ExcNonQuery(sql1, parameters1);
        }

        // 获取所有用户
        public static DataTable HuoQuSuoYouYongHu()
        {
            string sql = "select YongHuID, BuMen from YongHu";
            return DBHelper.GetDataTable(sql);
        }

        // 根据用户ID获取部门
        public static string HuoQuYongHuBuMen(string yongHuID)
        {
            string sql = "select BuMen from YongHu where YongHuID = @ZhangHu";
            SqlParameter[] canshu = { new SqlParameter("@ZhangHu", yongHuID) };
            DataTable jieguo = DBHelper.GetDataTable(sql, canshu);

            // 修复：更严谨的检查
            if (jieguo != null && jieguo.Rows.Count > 0)
            {
                // 检查第一行是否存在
                if (jieguo.Rows[0] != null)
                {
                    // 检查BuMen列是否存在且不为空
                    if (jieguo.Columns.Contains("BuMen") &&
                        jieguo.Rows[0]["BuMen"] != null &&
                        jieguo.Rows[0]["BuMen"] != DBNull.Value)
                    {
                        return jieguo.Rows[0]["BuMen"].ToString();
                    }
                }
            }
            return "";
        }
        public static string ChaXun_gai(YongHu yongHu)
        {
            string YongHuID = null;
            string sql = "select YongHuID from YongHu where YongHuID = @ZhangHu and MiMa = @MiMa";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZhangHu", yongHu.YongHuID),
                new SqlParameter("@MiMa", yongHu.MiMa)
            };

            DataTable panduan = DBHelper.GetDataTable(sql, parameters);

            // 修复：先检查DataTable和Rows是否存在
            if (panduan != null && panduan.Rows.Count > 0)
            {
                // 检查第一行是否存在且不为空
                if (panduan.Rows[0] != null &&
                    panduan.Rows[0]["YongHuID"] != null &&
                    panduan.Rows[0]["YongHuID"] != DBNull.Value)
                {
                    string tempID = panduan.Rows[0]["YongHuID"].ToString();
                    if (!string.IsNullOrEmpty(tempID))
                    {
                        YongHuID = tempID;
                    }
                }
            }

            return YongHuID;
        }

        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <returns>用户对象</returns>
        public static YongHu HuoQuGeRenXinXi(string yongHuID)
        {
            string sql = "select YongHuID,YongHuMing,BuMen,XingMing,ShenFenZheng from YongHu where YongHuID = @ZhangHu";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZhangHu", yongHuID),
            };

            DataTable panduan = DBHelper.GetDataTable(sql, parameters);

            if (panduan != null && panduan.Rows.Count > 0)
            {
                YongHu yongHu = new YongHu();
                yongHu.YongHuID = panduan.Rows[0]["YongHuID"].ToString();
                yongHu.YongHuName = panduan.Rows[0]["YongHuMing"].ToString();
                yongHu.BuMen = panduan.Rows[0]["BuMen"].ToString();
                yongHu.XingMing = panduan.Rows[0]["XingMing"].ToString();
                yongHu.ShenFenZheng = panduan.Rows[0]["ShenFenZheng"].ToString();
                return yongHu;
            }

            return null;
        }
        
        /// <summary>
        /// 修改用户个人信息
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <param name="yongHuMing">用户名</param>
        /// <param name="xingMing">真实姓名</param>
        /// <returns>是否成功</returns>
        public static bool XiuGaiGeRenXinXi(string yongHuID, string yongHuMing, string xingMing)
        {
            string sql = "update YongHu set YongHuMing=@YongHuMing , XingMing=@XingMing where YongHuID = @ZhangHu";

            SqlParameter[] parameters =
            {
                new SqlParameter("@YongHuMing", yongHuMing),
                new SqlParameter("@XingMing", xingMing),
                new SqlParameter("@ZhangHu", yongHuID),
            };

            return DBHelper.ExcNonQuery(sql, parameters);
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="yongHu">用户对象</param>
        /// <returns>是否添加成功</returns>
        public static bool TianJiaYongHu(YongHu yongHu)
        {
            string sql = "insert YongHu(YongHuMing,MiMa,XingMing,ShenFenZheng,BuMen) values(@YongHuMing,@MiMa,@XingMing,@ShenFenZheng,@BuMen)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@YongHuMing", yongHu.YongHuName),
                new SqlParameter("@MiMa", yongHu.MiMa),
                new SqlParameter("@XingMing", yongHu.XingMing),
                new SqlParameter("@ShenFenZheng", yongHu.ShenFenZheng),
                new SqlParameter("@BuMen", yongHu.BuMen)
            };

            return DBHelper.ExcNonQuery(sql, parameters);
        }

        /// <summary>
        /// 获取用户数据（使用YongHu对象传递搜索条件）
        /// </summary>
        /// <param name="yongHu">用户对象（包含搜索条件）</param>
        /// <param name="kaishiweizhi">开始位置</param>
        /// <param name="meiYeShuLiang">每页数量</param>
        /// <returns>DataTable数据</returns>
        public static DataTable HuoQuYongHuShuJu(YongHu yongHu, int kaishiweizhi, int meiYeShuLiang)
        {
            // 从数据库获取用户数据并绑定到Repeater
            string sql = "select YongHuID,YongHuMing,XingMing,ShenFenZheng,BuMen from YongHu";

            // 如果有搜索条件，添加where子句
            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(yongHu.YongHuID)) // 使用YongHuID作为用户账号
            {
                tiaoJian.Add("YongHuID like @ZhangHu");
                canShu.Add(new SqlParameter("@ZhangHu", "%" + yongHu.YongHuID + "%"));
            }

            if (!string.IsNullOrEmpty(yongHu.XingMing))
            {
                tiaoJian.Add("XingMing like @XingMing");
                canShu.Add(new SqlParameter("@XingMing", "%" + yongHu.XingMing + "%"));
            }

            if (!string.IsNullOrEmpty(yongHu.BuMen))
            {
                tiaoJian.Add("BuMen = @BuMen");
                canShu.Add(new SqlParameter("@BuMen", yongHu.BuMen));
            }

            // 如果有搜索条件，修改SQL语句
            if (tiaoJian.Count > 0)
            {
                sql += " where " + string.Join(" and ", tiaoJian);
            }

            sql += " order by YongHuID";

            // 添加分页参数
            sql += " offset @kaishiweizhi rows fetch next @meiYeShuLiang rows only";
            canShu.Add(new SqlParameter("@kaishiweizhi", kaishiweizhi));
            canShu.Add(new SqlParameter("@meiYeShuLiang", meiYeShuLiang));

            return DBHelper.GetDataTable(sql, canShu.ToArray());
        }

        /// <summary>
        /// 获取总记录数（使用YongHu对象传递搜索条件）
        /// </summary>
        /// <param name="yongHu">用户对象（包含搜索条件）</param>
        /// <returns>总记录数</returns>
        public static int HuoQuZongJiLuShu(YongHu yongHu)
        {
            string sql = "select count(*) from YongHu";

            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(yongHu.YongHuID)) // 使用YongHuID作为用户账号
            {
                tiaoJian.Add("YongHuID like @ZhangHu");
                canShu.Add(new SqlParameter("@ZhangHu", "%" + yongHu.YongHuID + "%"));
            }

            if (!string.IsNullOrEmpty(yongHu.XingMing))
            {
                tiaoJian.Add("XingMing like @XingMing");
                canShu.Add(new SqlParameter("@XingMing", "%" + yongHu.XingMing + "%"));
            }

            if (!string.IsNullOrEmpty(yongHu.BuMen))
            {
                tiaoJian.Add("BuMen = @BuMen");
                canShu.Add(new SqlParameter("@BuMen", yongHu.BuMen));
            }

            // 如果有搜索条件，修改SQL语句
            if (tiaoJian.Count > 0)
            {
                sql += " where " + string.Join(" and ", tiaoJian);
            }

            DataTable shuLiangBiao = DBHelper.GetDataTable(sql, canShu.ToArray());
            return Convert.ToInt32(shuLiangBiao.Rows[0][0]);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <returns>是否删除成功</returns>
        public static bool ShanChuYongHu(string yongHuID)
        {
            //执行删除操作
            string sql = "delete from YongHu where YongHuID = @ZhangHu";
            SqlParameter[] canShu = { new SqlParameter("@ZhangHu", yongHuID) };
            return DBHelper.ExcNonQuery(sql, canShu);
        }
    }
}