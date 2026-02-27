using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace DAL
{
    public class DBHelper
    {
        public static string connstring = "server=.;database=Soft;integrated security=true;";
        public static SqlConnection emm = null;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void lnitConnection()
        {
            if (emm == null)
            {
                //如果连接对象不存在则连接数据库
                emm = new SqlConnection(connstring);
            }
            if (emm.State == ConnectionState.Closed)
            {
                //如果连接对象关闭则打开连接
                emm.Open();
            }
            else if (emm.State == ConnectionState.Broken)
            {
                //如果连接中断或错误则中断数据库再打开数据库
                emm.Close();
                emm.Open();
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strSql, params SqlParameter[] parameters)
        {
            DataTable emm1 = new DataTable();

            // 每次创建新的连接，避免并发问题
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(emm1);
                }
            }
            return emm1;
        }
        /// <summary>
        /// 增加、删除、修改
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool ExcNonQuery(string strSql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
}