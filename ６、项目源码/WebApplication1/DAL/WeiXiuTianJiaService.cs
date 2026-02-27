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
    public class WeiXiuTianJiaService
    {
        /// <summary>
        /// 获取在库中的资产列表（用于维修）
        /// </summary>
        public static DataTable HuoQuZaiKuZiChanLieBiao()
        {
            string chaXun = @"select GuZiID, GuZiMing, GuZiShuLiang 
                            from GudinZiChan 
                            where ZhuangTaiID = 1
                            order by GuZiMing";

            return DBHelper.GetDataTable(chaXun);
        }

        /// <summary>
        /// 获取资产详细信息
        /// </summary>
        public static GuDingZiChan HuoQuZiChanXiangXiXinXi(string ziChanID)
        {
            if (string.IsNullOrEmpty(ziChanID))
            {
                return null;
            }

            string chaXun = @"select GuZiID, GuZiMing, GuZiShuLiang, ZhuangTaiID 
                            from GudinZiChan 
                            where GuZiID = @ZiChanID";

            SqlParameter[] canShu = {
                new SqlParameter("@ZiChanID", ziChanID)
            };

            DataTable shuJuBiao = DBHelper.GetDataTable(chaXun, canShu);

            if (shuJuBiao != null && shuJuBiao.Rows.Count > 0)
            {
                DataRow hang = shuJuBiao.Rows[0];

                GuDingZiChan ziChan = new GuDingZiChan();
                ziChan.GuZiID = hang["GuZiID"].ToString();
                ziChan.GuZiMing = hang["GuZiMing"].ToString();
                ziChan.GuZiShuLiang = hang["GuZiShuLiang"].ToString();
                ziChan.ZhuangTaiID = Convert.ToInt32(hang["ZhuangTaiID"]);

                return ziChan;
            }

            return null;
        }

        /// <summary>
        /// 保存维修记录
        /// </summary>
        public static bool BaoCunWeiXiuJiLu(GuDingZiChan ziChan, int weiXiuShuLiang, DateTime songXiuRiQi,
                                          DateTime yuWanRiQi, decimal weiXiuJinE)
        {
            using (SqlConnection lianJie = new SqlConnection(DBHelper.connstring))
            {
                lianJie.Open();

                using (SqlTransaction shiWu = lianJie.BeginTransaction())
                {
                    try
                    {
                        string guZiID = ziChan.GuZiID;
                        int dangQianShuLiang = Convert.ToInt32(ziChan.GuZiShuLiang);

                        // 1. 获取资产完整信息
                        string huoQuXinXiSQL = @"select GuZiMing, CangKuID, GuZiJiaZhi, ChuChangRiQi 
                                               from GudinZiChan where GuZiID = @GuZiID";

                        string guZiMing = "";
                        string cangKuID = "";
                        decimal guZiJiaZhi = 0;
                        DateTime chuChangRiQi = DateTime.Now;

                        using (SqlCommand mingLing = new SqlCommand(huoQuXinXiSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", guZiID));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    guZiMing = duZhe["GuZiMing"].ToString();
                                    cangKuID = duZhe["CangKuID"].ToString();
                                    guZiJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                    chuChangRiQi = Convert.ToDateTime(duZhe["ChuChangRiQi"]);
                                }
                                else
                                {
                                    throw new Exception("获取资产信息失败！");
                                }
                            }
                        }

                        // 2. 更新原有固定资产记录（减少在库数量）
                        int xinZaiKuShuLiang = dangQianShuLiang - weiXiuShuLiang;

                        string gengXinSQL = @"update GudinZiChan 
                                             set GuZiShuLiang = @XinShuLiang
                                             where GuZiID = @GuZiID";

                        SqlParameter[] gengXinCanShu = new SqlParameter[2];
                        gengXinCanShu[0] = new SqlParameter("@XinShuLiang", xinZaiKuShuLiang);
                        gengXinCanShu[1] = new SqlParameter("@GuZiID", guZiID);

                        using (SqlCommand mingLing = new SqlCommand(gengXinSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.AddRange(gengXinCanShu);
                            int yingXiangHang = mingLing.ExecuteNonQuery();
                            if (yingXiangHang == 0)
                            {
                                throw new Exception("更新资产信息失败！");
                            }
                        }

                        // 3. 新增一条固定资产记录（维修中的部分）
                        decimal weiXiuJiaZhi = guZiJiaZhi / dangQianShuLiang * weiXiuShuLiang;

                        string chaRuWeiXiuSQL = @"insert into GudinZiChan (GuZiMing, CangKuID, GuZiShuLiang, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID) 
                                                values (@GuZiMing, @CangKuID, @GuZiShuLiang, @GuZiJiaZhi, @ChuChangRiQi, @ZhuangTaiID)";

                        SqlParameter[] chaRuWeiXiuCanShu = new SqlParameter[6];
                        chaRuWeiXiuCanShu[0] = new SqlParameter("@GuZiMing", guZiMing + "(维修中)");
                        chaRuWeiXiuCanShu[1] = new SqlParameter("@CangKuID", cangKuID);
                        chaRuWeiXiuCanShu[2] = new SqlParameter("@GuZiShuLiang", weiXiuShuLiang);
                        chaRuWeiXiuCanShu[3] = new SqlParameter("@GuZiJiaZhi", weiXiuJiaZhi);
                        chaRuWeiXiuCanShu[4] = new SqlParameter("@ChuChangRiQi", chuChangRiQi);
                        chaRuWeiXiuCanShu[5] = new SqlParameter("@ZhuangTaiID", 2);

                        using (SqlCommand mingLing = new SqlCommand(chaRuWeiXiuSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.AddRange(chaRuWeiXiuCanShu);
                            mingLing.ExecuteNonQuery();
                        }

                        // 4. 插入维修记录到WeiXiu表
                        string chaRuWeiXiuBiaoSQL = @"insert into WeiXiu (GuZiID, WeiXiuShuLiang, SongXiuRiQi, YuWanRiQi, WeiXiuJinE) 
                                                     values (@GuZiID, @WeiXiuShuLiang, @SongXiuRiQi, @YuWanRiQi, @WeiXiuJinE)";

                        SqlParameter[] chaRuWeiXiuBiaoCanShu = new SqlParameter[5];
                        chaRuWeiXiuBiaoCanShu[0] = new SqlParameter("@GuZiID", guZiID);
                        chaRuWeiXiuBiaoCanShu[1] = new SqlParameter("@WeiXiuShuLiang", weiXiuShuLiang);
                        chaRuWeiXiuBiaoCanShu[2] = new SqlParameter("@SongXiuRiQi", songXiuRiQi);
                        chaRuWeiXiuBiaoCanShu[3] = new SqlParameter("@YuWanRiQi", yuWanRiQi);
                        chaRuWeiXiuBiaoCanShu[4] = new SqlParameter("@WeiXiuJinE", weiXiuJinE);

                        using (SqlCommand mingLing = new SqlCommand(chaRuWeiXiuBiaoSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.AddRange(chaRuWeiXiuBiaoCanShu);
                            mingLing.ExecuteNonQuery();
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

                        throw new Exception("保存失败：" + ex.Message);
                    }
                }
            }
        }
    }
}