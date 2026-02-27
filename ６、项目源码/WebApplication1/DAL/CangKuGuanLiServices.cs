using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CangKuGuanLiServices
    {
        /// <summary>
        /// 添加仓库分类 - 使用参数化查询防止SQL注入
        /// </summary>
        /// <param name="CangKuMing">仓库分类名称</param>
        /// <returns>是否添加成功</returns>
        public static bool XinZenCangKu(string CangKuMing)
        {
            // 先检查分类是否已存在
            if (ChaXunFenLeiShiFouCunZai(CangKuMing))
            {
                return false; // 分类已存在，不添加
            }

            // 使用参数化查询防止SQL注入
            string sql = "insert into LeiXin values(@CangKuMing)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CangKuMing", SqlDbType.NVarChar) { Value = CangKuMing }
            };
            return DBHelper.ExcNonQuery(sql, parameters);
        }

        /// <summary>
        /// 查询仓库 - 静态查询，无需参数化
        /// </summary>
        /// <returns>仓库信息数据表</returns>
        public static DataTable ChaXunCangKu()
        {
            string sql = "select CangKuID,CangKuMing,LeiXingI from CangKu join LeiXin on CangKu.LeiXingID=LeiXin.LeiXingID";
            return DBHelper.GetDataTable(sql);
        }

      

        /// <summary>
        /// 添加仓库 - 使用参数化查询防止SQL注入
        /// </summary>
        /// <param name="CangKuMing">仓库名称</param>
        /// <param name="LeiXingID">分类ID</param>
        /// <returns>是否添加成功</returns>
        public static bool XinZenCangKuDongXi(string CangKuMing, int LeiXingID)
        {
            // 使用参数化查询
            string sql = "insert into CangKu (CangKuMing, LeiXingID) values(@CangKuMing, @LeiXingID)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CangKuMing", SqlDbType.NVarChar) { Value = CangKuMing },
                new SqlParameter("@LeiXingID", SqlDbType.Int) { Value = LeiXingID }
            };
            return DBHelper.ExcNonQuery(sql, parameters);
        }

        /// <summary>
        /// 修改仓库名称 - 使用参数化查询防止SQL注入
        /// </summary>
        /// <param name="CangKuID">仓库ID</param>
        /// <param name="CangKuXingMin">新仓库名称</param>
        /// <returns>是否修改成功</returns>
        public static bool CangKuXiGai(string CangKuID, string CangKuXingMin)
        {
            // 先验证ID是否为数字
            if (!int.TryParse(CangKuID, out int cangKuId))
            {
                return false;
            }

            // 使用参数化查询
            string sql = "update CangKu set CangKuMing = @CangKuXingMin where CangKuID = @CangKuID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CangKuXingMin", SqlDbType.NVarChar) { Value = CangKuXingMin },
                new SqlParameter("@CangKuID", SqlDbType.Int) { Value = cangKuId }
            };
            return DBHelper.ExcNonQuery(sql, parameters);
        }

        /// <summary>
        /// 查询仓库信息 - 使用参数化查询防止SQL注入
        /// </summary>
        /// <param name="CangKuID">仓库ID</param>
        /// <returns>仓库信息数据表</returns>
        public static DataTable XianShiXinXi(string CangKuID)
        {
            // 先验证ID是否为数字
            if (!int.TryParse(CangKuID, out int cangKuId))
            {
                return new DataTable(); // 返回空表
            }

            // 使用参数化查询
            string sql = "select CangKuMing from CangKu where CangKuID=@CangKuID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CangKuID", SqlDbType.Int) { Value = cangKuId }
            };
            return DBHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 模糊查询仓库 - 使用参数化查询防止SQL注入
        /// </summary>
        /// <param name="BianHao">仓库编号（模糊匹配）</param>
        /// <param name="CangKuMing">仓库名称（模糊匹配）</param>
        /// <returns>查询结果数据表</returns>
        public static DataTable ChaXunCangKuMoHu(string BianHao, string CangKuMing)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select CangKuID,CangKuMing,LeiXingI from CangKu join LeiXin on CangKu.LeiXingID=LeiXin.LeiXingID where 1=1");

            List<SqlParameter> parameters = new List<SqlParameter>();

            // 处理仓库编号模糊查询
            if (!string.IsNullOrEmpty(BianHao))
            {
                sqlBuilder.Append(" and CangKuID like @BianHao");
                parameters.Add(new SqlParameter("@BianHao", SqlDbType.NVarChar)
                {
                    Value = "%" + BianHao.Trim() + "%"
                });
            }

            // 处理仓库名称模糊查询
            if (!string.IsNullOrEmpty(CangKuMing))
            {
                sqlBuilder.Append(" and CangKuMing like @CangKuMing");
                parameters.Add(new SqlParameter("@CangKuMing", SqlDbType.NVarChar)
                {
                    Value = "%" + CangKuMing.Trim() + "%"
                });
            }

            return DBHelper.GetDataTable(sqlBuilder.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// 查询仓库分类 - 静态查询，无需参数化
        /// </summary>
        /// <returns>仓库分类数据表</returns>
        public static DataTable ChaXunCangKuFenLei()
        {
            string sql = "select LeiXingID, LeiXingI from LeiXin";
            return DBHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 检查分类名称是否已存在
        /// </summary>
        /// <param name="CangKuMing">分类名称</param>
        /// <returns>true=已存在，false=不存在</returns>
        public static bool ChaXunFenLeiShiFouCunZai(string CangKuMing)
        {
            if (string.IsNullOrWhiteSpace(CangKuMing))
            {
                return false;
            }

            // 使用参数化查询，精确匹配分类名称（不区分大小写）
            string sql = "SELECT COUNT(1) FROM LeiXin WHERE LeiXingI = @CangKuMing COLLATE Chinese_PRC_CS_AI";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CangKuMing", SqlDbType.NVarChar) { Value = CangKuMing.Trim() }
            };

            try
            {
                // 获取查询结果
                DataTable dt = DBHelper.GetDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0; // 如果count>0，表示已存在
                }
                return false;
            }
            catch (Exception)
            {
                // 如果查询出错，假设不存在
                return false;
            }
        }
        public static bool ChaXunCangKuShiFouCunZai(string CangKuMing, int LeiXingID = 0)
        {
            if (string.IsNullOrWhiteSpace(CangKuMing))
            {
                return false;
            }

            // 构建查询SQL
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT COUNT(1) FROM CangKu WHERE CangKuMing = @CangKuMing COLLATE Chinese_PRC_CS_AI");

            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@CangKuMing", SqlDbType.NVarChar) { Value = CangKuMing.Trim() }
    };

            // 如果指定了分类ID，则同时检查分类
            if (LeiXingID > 0)
            {
                sqlBuilder.Append(" AND LeiXingID = @LeiXingID");
                parameters.Add(new SqlParameter("@LeiXingID", SqlDbType.Int) { Value = LeiXingID });
            }

            try
            {
                // 获取查询结果
                DataTable dt = DBHelper.GetDataTable(sqlBuilder.ToString(), parameters.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0; // 如果count>0，表示已存在
                }
                return false;
            }
            catch (Exception)
            {
                // 如果查询出错，假设不存在
                return false;
            }
        }
        public static int HuoQuCangKuFenLeiID(string CangKuID)
        {
            if (!int.TryParse(CangKuID, out int cangKuId))
            {
                return 0;
            }

            string sql = "SELECT LeiXingID FROM CangKu WHERE CangKuID = @CangKuID";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@CangKuID", SqlDbType.Int) { Value = cangKuId }
            };

            try
            {
                DataTable dt = DBHelper.GetDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["LeiXingID"]);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}

