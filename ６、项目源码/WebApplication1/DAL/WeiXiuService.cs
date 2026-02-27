using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mods;

namespace DAL
{
    public class WeiXiuService
    {
        /// <summary>
        /// 获取维修记录总数
        /// </summary>
        public static int HuoQuWeiXiuZongShu(string guZiID, string guZiMing)
        {
            string sql = "select count(*) from WeiXiu join GudinZiChan on WeiXiu.GuZiID=GudinZiChan.GuZiID";

            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(guZiID))
            {
                tiaoJian.Add("WeiXiu.GuZiID like @GuZiID");
                canShu.Add(new SqlParameter("@GuZiID", "%" + guZiID + "%"));
            }

            if (!string.IsNullOrEmpty(guZiMing))
            {
                tiaoJian.Add("GuZiMing like @GuZiMing");
                canShu.Add(new SqlParameter("@GuZiMing", "%" + guZiMing + "%"));
            }

            if (tiaoJian.Count > 0)
            {
                sql += " where " + string.Join(" and ", tiaoJian);
            }

            DataTable shuLiangBiao = DBHelper.GetDataTable(sql, canShu.ToArray());
            return Convert.ToInt32(shuLiangBiao.Rows[0][0]);
        }

        /// <summary>
        /// 获取维修记录列表（带分页）
        /// </summary>
        public static DataTable HuoQuWeiXiuLieBiao(string guZiID, string guZiMing, int kaishiweizhi, int meiYeShuLiang)
        {
            string sql = @"select WeiXiu.GuZiID,GuZiMing,WeiXiuShuLiang,SongXiuRiQi,YuWanRiQi,ShiWanRiQi,WeiXiuJinE 
                         from WeiXiu join GudinZiChan on WeiXiu.GuZiID=GudinZiChan.GuZiID";

            List<SqlParameter> canShu = new List<SqlParameter>();
            List<string> tiaoJian = new List<string>();

            if (!string.IsNullOrEmpty(guZiID))
            {
                tiaoJian.Add("WeiXiu.GuZiID like @GuZiID");
                canShu.Add(new SqlParameter("@GuZiID", "%" + guZiID + "%"));
            }

            if (!string.IsNullOrEmpty(guZiMing))
            {
                tiaoJian.Add("GuZiMing like @GuZiMing");
                canShu.Add(new SqlParameter("@GuZiMing", "%" + guZiMing + "%"));
            }

            if (tiaoJian.Count > 0)
            {
                sql += " where " + string.Join(" and ", tiaoJian);
            }

            sql += " order by WeiXiu.GuZiID";

            // 添加分页参数
            sql += " offset @kaishiweizhi rows fetch next @meiYeShuLiang rows only";
            canShu.Add(new SqlParameter("@kaishiweizhi", kaishiweizhi));
            canShu.Add(new SqlParameter("@meiYeShuLiang", meiYeShuLiang));

            return DBHelper.GetDataTable(sql, canShu.ToArray());
        }

        /// <summary>
        /// 完成维修操作
        /// </summary>
        public static bool WanChengWeiXiu(string guZiID, DateTime shiWanRiQi)
        {
            using (SqlConnection lianJie = new SqlConnection(DBHelper.connstring))
            {
                lianJie.Open();

                using (SqlTransaction shiWu = lianJie.BeginTransaction())
                {
                    try
                    {
                        int weiXiuGuZiID = Convert.ToInt32(guZiID);

                        // 1. 获取维修记录信息
                        string chaXunWeiXiuSQL = @"select WeiXiuShuLiang, SongXiuRiQi, WeiXiuJinE 
                                          from WeiXiu 
                                          where GuZiID = @GuZiID
                                          and ShiWanRiQi is null
                                          order by SongXiuRiQi desc";

                        int weiXiuShuLiang = 0;
                        DateTime songXiuRiQi = DateTime.Now;
                        decimal weiXiuJinE = 0;

                        using (SqlCommand mingLing = new SqlCommand(chaXunWeiXiuSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", weiXiuGuZiID));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    weiXiuShuLiang = Convert.ToInt32(duZhe["WeiXiuShuLiang"]);
                                    songXiuRiQi = Convert.ToDateTime(duZhe["SongXiuRiQi"]);
                                    weiXiuJinE = Convert.ToDecimal(duZhe["WeiXiuJinE"]);
                                }
                                else
                                {
                                    throw new Exception("未找到待完成的维修记录！");
                                }
                            }
                        }

                        // 2. 更新维修记录
                        string gengXinWeiXiuSQL = @"update WeiXiu 
                                           set ShiWanRiQi = @ShiWanRiQi
                                           where GuZiID = @GuZiID 
                                           and SongXiuRiQi = @SongXiuRiQi
                                           and ShiWanRiQi is null";

                        using (SqlCommand mingLing = new SqlCommand(gengXinWeiXiuSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@ShiWanRiQi", shiWanRiQi));
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", weiXiuGuZiID));
                            mingLing.Parameters.Add(new SqlParameter("@SongXiuRiQi", songXiuRiQi));

                            int yingXiangHang = mingLing.ExecuteNonQuery();

                            if (yingXiangHang == 0)
                            {
                                throw new Exception("更新维修记录失败！");
                            }
                        }

                        // 3. 查找对应的维修中资产记录
                        string yuanShiGuZiMing = "";
                        int weiXiuZiChanID = 0;
                        string weiXiuGuZiMing = "";
                        string weiXiuCangKuID = "";
                        decimal weiXiuJiaZhi = 0;
                        DateTime chuChangRiQi = DateTime.Now;
                        int zhuangTaiID = 0;

                        string chaXunWeiXiuZiChanSQL = @"select GuZiID, GuZiMing, CangKuID, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID
                                                 from GudinZiChan 
                                                 where GuZiID = @GuZiID";

                        using (SqlCommand mingLing = new SqlCommand(chaXunWeiXiuZiChanSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", weiXiuGuZiID));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    weiXiuZiChanID = Convert.ToInt32(duZhe["GuZiID"]);
                                    yuanShiGuZiMing = duZhe["GuZiMing"].ToString();
                                    weiXiuGuZiMing = duZhe["GuZiMing"].ToString();
                                    weiXiuCangKuID = duZhe["CangKuID"].ToString();
                                    weiXiuJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                    chuChangRiQi = Convert.ToDateTime(duZhe["ChuChangRiQi"]);
                                    zhuangTaiID = Convert.ToInt32(duZhe["ZhuangTaiID"]);
                                }
                                else
                                {
                                    throw new Exception($"找不到ID为{weiXiuGuZiID}的资产记录！");
                                }
                            }
                        }

                        // 如果找到的记录状态不是维修中(2)，尝试查找维修中记录
                        if (zhuangTaiID != 2)
                        {
                            string chaXunWeiXiuMingChengSQL = @"select GuZiID, GuZiMing, CangKuID, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID
                                                        from GudinZiChan 
                                                        where (GuZiMing like @GuZiMing + '(维修)%' 
                                                               or GuZiMing like @GuZiMing + '(维修中)%')
                                                        and ZhuangTaiID = 2";

                            using (SqlCommand mingLing = new SqlCommand(chaXunWeiXiuMingChengSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.Add(new SqlParameter("@GuZiMing", yuanShiGuZiMing));

                                using (SqlDataReader duZhe = mingLing.ExecuteReader())
                                {
                                    if (duZhe.Read())
                                    {
                                        weiXiuZiChanID = Convert.ToInt32(duZhe["GuZiID"]);
                                        yuanShiGuZiMing = duZhe["GuZiMing"].ToString();
                                        weiXiuGuZiMing = duZhe["GuZiMing"].ToString();
                                        weiXiuCangKuID = duZhe["CangKuID"].ToString();
                                        weiXiuJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                        chuChangRiQi = Convert.ToDateTime(duZhe["ChuChangRiQi"]);
                                        zhuangTaiID = Convert.ToInt32(duZhe["ZhuangTaiID"]);
                                    }
                                }
                            }
                        }

                        // 验证是否找到了维修中记录
                        if (zhuangTaiID != 2)
                        {
                            throw new Exception("该资产不是维修中状态！当前状态：" + zhuangTaiID);
                        }

                        // 4. 恢复原始资产名称
                        string jingHuaYuanShiMingCheng = weiXiuGuZiMing
                            .Replace("(维修)", "")
                            .Replace("(维修中)", "")
                            .Trim();

                        // 5. 查找原始在库记录
                        string chaXunYuanShiSQL = @"select GuZiID, GuZiShuLiang, GuZiJiaZhi 
                                           from GudinZiChan 
                                           where GuZiMing = @GuZiMing 
                                           and ZhuangTaiID = 1";

                        int yuanShiGuZiID = 0;
                        int yuanShiShuLiang = 0;
                        decimal yuanShiJiaZhi = 0;

                        using (SqlCommand mingLing = new SqlCommand(chaXunYuanShiSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiMing", jingHuaYuanShiMingCheng));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    yuanShiGuZiID = Convert.ToInt32(duZhe["GuZiID"]);
                                    yuanShiShuLiang = Convert.ToInt32(duZhe["GuZiShuLiang"]);
                                    yuanShiJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                }
                            }
                        }

                        if (yuanShiGuZiID > 0)
                        {
                            // 6. 更新原始在库记录
                            int xinZaiKuShuLiang = yuanShiShuLiang + weiXiuShuLiang;
                            decimal meiJianJiaZhi = 0;
                            if (weiXiuShuLiang > 0)
                            {
                                meiJianJiaZhi = weiXiuJiaZhi / weiXiuShuLiang;
                            }
                            decimal weiXiuZiChanZongJiaZhi = meiJianJiaZhi * weiXiuShuLiang;
                            decimal zongJiaZhi = yuanShiJiaZhi + weiXiuZiChanZongJiaZhi;

                            string gengXinYuanShiSQL = @"update GudinZiChan 
                                                set GuZiShuLiang = @XinShuLiang,
                                                    GuZiJiaZhi = @XinJiaZhi
                                                where GuZiID = @GuZiID";

                            using (SqlCommand mingLing = new SqlCommand(gengXinYuanShiSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.Add(new SqlParameter("@XinShuLiang", xinZaiKuShuLiang));
                                mingLing.Parameters.Add(new SqlParameter("@XinJiaZhi", zongJiaZhi));
                                mingLing.Parameters.Add(new SqlParameter("@GuZiID", yuanShiGuZiID));

                                int yingXiangHang = mingLing.ExecuteNonQuery();
                                if (yingXiangHang == 0)
                                {
                                    throw new Exception("更新在库资产失败！");
                                }
                            }
                        }
                        else
                        {
                            // 7. 如果找不到原始记录，创建新的在库记录
                            decimal meiJianJiaZhi = 0;
                            if (weiXiuShuLiang > 0)
                            {
                                meiJianJiaZhi = weiXiuJiaZhi / weiXiuShuLiang;
                            }
                            decimal weiXiuZiChanZongJiaZhi = meiJianJiaZhi * weiXiuShuLiang;

                            string chaRuXinZiChanSQL = @"insert into GudinZiChan 
                                               (GuZiMing, CangKuID, GuZiShuLiang, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID) 
                                               values (@GuZiMing, @CangKuID, @GuZiShuLiang, @GuZiJiaZhi, @ChuChangRiQi, @ZhuangTaiID)";

                            using (SqlCommand mingLing = new SqlCommand(chaRuXinZiChanSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.Add(new SqlParameter("@GuZiMing", jingHuaYuanShiMingCheng));
                                mingLing.Parameters.Add(new SqlParameter("@CangKuID", weiXiuCangKuID));
                                mingLing.Parameters.Add(new SqlParameter("@GuZiShuLiang", weiXiuShuLiang));
                                mingLing.Parameters.Add(new SqlParameter("@GuZiJiaZhi", weiXiuZiChanZongJiaZhi));
                                mingLing.Parameters.Add(new SqlParameter("@ChuChangRiQi", chuChangRiQi));
                                mingLing.Parameters.Add(new SqlParameter("@ZhuangTaiID", 1));
                                mingLing.ExecuteNonQuery();
                            }
                        }

                        // 8. 删除或更新维修中记录
                        string shanChuWeiXiuZiChanSQL = @"delete from GudinZiChan 
                                                where GuZiID = @GuZiID
                                                and ZhuangTaiID = 2";

                        using (SqlCommand mingLing = new SqlCommand(shanChuWeiXiuZiChanSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", weiXiuZiChanID));
                            int yingXiangHang = mingLing.ExecuteNonQuery();

                            if (yingXiangHang == 0)
                            {
                                string gengXinZhuangTaiSQL = @"update GudinZiChan 
                                                       set ZhuangTaiID = 1,
                                                           GuZiMing = @XinGuZiMing
                                                       where GuZiID = @GuZiID
                                                       and ZhuangTaiID = 2";

                                using (SqlCommand gengXinMingLing = new SqlCommand(gengXinZhuangTaiSQL, lianJie, shiWu))
                                {
                                    gengXinMingLing.Parameters.Add(new SqlParameter("@XinGuZiMing", jingHuaYuanShiMingCheng));
                                    gengXinMingLing.Parameters.Add(new SqlParameter("@GuZiID", weiXiuZiChanID));
                                    yingXiangHang = gengXinMingLing.ExecuteNonQuery();

                                    if (yingXiangHang == 0)
                                    {
                                        throw new Exception("处理维修中资产记录失败！");
                                    }
                                }
                            }
                        }

                        shiWu.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            shiWu.Rollback();
                        }
                        catch
                        {
                            // 忽略回滚错误
                        }

                        throw new Exception("完修操作失败：" + ex.Message);
                    }
                }
            }
        }
    }
}