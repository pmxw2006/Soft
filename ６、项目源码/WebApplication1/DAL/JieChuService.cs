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
    public class JieChuService
    {
        /// <summary>
        /// 获取在库中的资产列表
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
        /// 保存借出记录
        /// </summary>
        public static bool BaoCunJieChuJiLu(GuDingZiChan ziChan, int jieChuShuLiang, string zuJieGongSi, DateTime niHuanRiQi)
        {
            using (SqlConnection lianJie = new SqlConnection(DBHelper.connstring))
            {
                lianJie.Open();

                using (SqlTransaction shiWu = lianJie.BeginTransaction())
                {
                    try
                    {
                        // 1. 插入借出记录到JieHuan表
                        string chaRuJieHuanSQL = @"insert into JieHuan (GuZiID, GongSi, ShuLiang, RiQi, LeiXin) 
                                                 values (@GuZiID, @GongSi, @ShuLiang, @RiQi, @LeiXin)";

                        SqlParameter[] chaRuJieHuanCanShu = new SqlParameter[5];
                        chaRuJieHuanCanShu[0] = new SqlParameter("@GuZiID", ziChan.GuZiID);
                        chaRuJieHuanCanShu[1] = new SqlParameter("@GongSi", zuJieGongSi);
                        chaRuJieHuanCanShu[2] = new SqlParameter("@ShuLiang", jieChuShuLiang);
                        chaRuJieHuanCanShu[3] = new SqlParameter("@RiQi", niHuanRiQi);
                        chaRuJieHuanCanShu[4] = new SqlParameter("@LeiXin", "借出");

                        using (SqlCommand mingLing = new SqlCommand(chaRuJieHuanSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.AddRange(chaRuJieHuanCanShu);
                            mingLing.ExecuteNonQuery();
                        }

                        // 2. 获取资产完整信息
                        string huoQuXinXiSQL = @"select GuZiMing, CangKuID, GuZiJiaZhi, ChuChangRiQi 
                                               from GudinZiChan where GuZiID = @GuZiID";

                        string guZiMing = "";
                        string cangKuID = "";
                        decimal guZiJiaZhi = 0;
                        DateTime chuChangRiQi = DateTime.Now;
                        string guZiID = ziChan.GuZiID;

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

                        // 3. 更新原有固定资产记录（减少在库数量）
                        int dangQianShuLiang = Convert.ToInt32(ziChan.GuZiShuLiang);
                        int xinZaiKuShuLiang = dangQianShuLiang - jieChuShuLiang;

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

                        // 4. 新增一条固定资产记录（借出中的部分）
                        decimal jieChuJiaZhi = guZiJiaZhi / dangQianShuLiang * jieChuShuLiang;

                        string chaRuZiChanSQL = @"insert into GudinZiChan (GuZiMing, CangKuID, GuZiShuLiang, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID) 
                                                 values (@GuZiMing, @CangKuID, @GuZiShuLiang, @GuZiJiaZhi, @ChuChangRiQi, @ZhuangTaiID)";

                        SqlParameter[] chaRuZiChanCanShu = new SqlParameter[6];
                        chaRuZiChanCanShu[0] = new SqlParameter("@GuZiMing", guZiMing + "(借出)");
                        chaRuZiChanCanShu[1] = new SqlParameter("@CangKuID", cangKuID);
                        chaRuZiChanCanShu[2] = new SqlParameter("@GuZiShuLiang", jieChuShuLiang);
                        chaRuZiChanCanShu[3] = new SqlParameter("@GuZiJiaZhi", jieChuJiaZhi);
                        chaRuZiChanCanShu[4] = new SqlParameter("@ChuChangRiQi", chuChangRiQi);
                        chaRuZiChanCanShu[5] = new SqlParameter("@ZhuangTaiID", 3);

                        using (SqlCommand mingLing = new SqlCommand(chaRuZiChanSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.AddRange(chaRuZiChanCanShu);
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