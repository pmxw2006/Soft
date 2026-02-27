using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GuDingZiChanServices
    {

            /// <summary>
            /// 查询资产 - 静态查询，无需参数化，直接返回所有资产信息
            /// </summary>
            /// <returns>包含所有资产信息的数据表</returns>
            public static DataTable XianShiXinXi()
            {
                // 此查询没有用户输入参数，直接使用静态SQL语句，安全
                string sql = "select GuZiID,GuZiMing,CangKuMing,GuZiShuLiang,GuZiJiaZhi,ChuChangRiQi,ZhuangTaiID from GudinZiChan join CangKu on GudinZiChan.CangKuID=CangKu.CangKuID";
                return DBHelper.GetDataTable(sql);
            }

            /// <summary>
            /// 删除资产 - 使用参数化查询防止SQL注入
            /// </summary>
            /// <param name="GuZiID">资产ID</param>
            /// <returns>是否删除成功</returns>
            public static bool ShanCuCangKu(string GuZiID)
            {
                // 验证输入是否为有效数字
                if (!int.TryParse(GuZiID, out int guZiId))
                {
                    return false; // 无效ID，直接返回失败
                }

                // 使用参数化查询防止SQL注入
                string sql = "delete GudinZiChan where GuZiID=@GuZiID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@GuZiID", SqlDbType.Int) { Value = guZiId }
                };
                return DBHelper.ExcNonQuery(sql, parameters);
            }

            /// <summary>
            /// 模糊查询资产 - 使用参数化查询防止SQL注入
            /// </summary>
            /// <param name="keyword">搜索关键词</param>
            /// <returns>包含查询结果的数据表</returns>
            public static DataTable MoHuChaXun(string keyword)
            {
                // 清理输入，移除首尾空格
                string cleanKeyword = keyword?.Trim() ?? string.Empty;

                // 使用参数化查询防止SQL注入
                string sql = @"SELECT * FROM GudinZiChan 
                         JOIN CangKu ON GudinZiChan.CangKuID=CangKu.CangKuID
                         WHERE GuZiMing LIKE @Keyword 
                         OR CangKuMing LIKE @Keyword";

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Keyword", SqlDbType.NVarChar)
                {
                    Value = "%" + cleanKeyword + "%"
                }
                };

                return DBHelper.GetDataTable(sql, parameters);
            }
        }
    }

