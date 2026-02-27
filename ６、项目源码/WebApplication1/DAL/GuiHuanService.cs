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
    public class GuiHuanService
    {
        /// <summary>
        /// 获取借出中的资产列表
        /// </summary>
        public static DataTable HuoQuJieChuZiChanLieBiao()
        {
            string chaXun = @"
                SELECT 
                    jc.GuZiID, 
                    jc.GuZiMing, 
                    jc.GuZiShuLiang,
                    jc.ZhuangTaiID,
                    COALESCE((
                        SELECT TOP 1 j.GongSi 
                        FROM JieHuan j 
                        INNER JOIN GudinZiChan g ON g.GuZiID = j.GuZiID
                        WHERE g.GuZiMing = REPLACE(jc.GuZiMing, '(借出)', '')
                        AND j.LeiXin = '借出'
                        ORDER BY j.RiQi DESC
                    ), '') as Gongsi
                FROM GudinZiChan jc
                WHERE jc.ZhuangTaiID = 3
                AND jc.GuZiMing LIKE '%(借出)%'
                ORDER BY jc.GuZiMing";

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

            string chaXun = @"select GuZiID, GuZiMing, GuZiShuLiang, ZhuangTaiID, GuZiJiaZhi, CangKuID, ChuChangRiQi 
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
                ziChan.GuZiJiaZhi = Convert.ToDecimal(hang["GuZiJiaZhi"]);
                ziChan.CangKuID = hang["CangKuID"].ToString();
                ziChan.ChuChangRiQi = Convert.ToDateTime(hang["ChuChangRiQi"]);

                // 获取公司信息
                string jieChuMingCheng = ziChan.GuZiMing;
                string yuanShiMingCheng = jieChuMingCheng.Replace("(借出)", "").Trim();

                string chaXunYuanShi = @"select top 1 GuZiID from GudinZiChan 
                                       where GuZiMing = @GuZiMing 
                                       and ZhuangTaiID = 1
                                       order by GuZiID";

                SqlParameter[] yuanShiCanShu = {
                    new SqlParameter("@GuZiMing", yuanShiMingCheng)
                };

                DataTable yuanShiBiao = DBHelper.GetDataTable(chaXunYuanShi, yuanShiCanShu);

                if (yuanShiBiao != null && yuanShiBiao.Rows.Count > 0)
                {
                    string yuanShiID = yuanShiBiao.Rows[0]["GuZiID"].ToString();

                    string chaXunGongSi = @"select top 1 GongSi from JieHuan 
                                          where GuZiID = @GuZiID 
                                          and LeiXin = '借出'
                                          order by RiQi desc";

                    SqlParameter[] gongSiCanShu = {
                        new SqlParameter("@GuZiID", yuanShiID)
                    };

                    DataTable gongSiBiao = DBHelper.GetDataTable(chaXunGongSi, gongSiCanShu);

                    if (gongSiBiao != null && gongSiBiao.Rows.Count > 0)
                    {
                        ziChan.Gongsi = gongSiBiao.Rows[0]["GongSi"].ToString();
                    }
                }

                return ziChan;
            }

            return null;
        }

        /// <summary>
        /// 保存归还记录
        /// </summary>
        public static bool BaoCunGuiHuanJiLu(GuDingZiChan jieChuZiChan, int guiHuanShuLiang, string guiHuanGongSi, DateTime guiHuanRiQi)
        {
            using (SqlConnection lianJie = new SqlConnection(DBHelper.connstring))
            {
                lianJie.Open();

                using (SqlTransaction shiWu = lianJie.BeginTransaction())
                {
                    try
                    {
                        string jieChuZiChanID = jieChuZiChan.GuZiID;

                        // 1. 获取借出记录的详细信息
                        string huoQuJieChuXinXiSQL = @"select GuZiMing, CangKuID, GuZiJiaZhi, ChuChangRiQi 
                                                     from GudinZiChan where GuZiID = @GuZiID";

                        string jieChuGuZiMing = "";
                        string jieChuCangKuID = "";
                        decimal jieChuJiaZhi = 0;
                        DateTime jieChuChuChangRiQi = DateTime.Now;

                        using (SqlCommand mingLing = new SqlCommand(huoQuJieChuXinXiSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiID", jieChuZiChanID));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    jieChuGuZiMing = duZhe["GuZiMing"].ToString();
                                    jieChuCangKuID = duZhe["CangKuID"].ToString();
                                    jieChuJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                    jieChuChuChangRiQi = Convert.ToDateTime(duZhe["ChuChangRiQi"]);
                                }
                                else
                                {
                                    throw new Exception("获取借出资产信息失败！");
                                }
                            }
                        }

                        // 2. 通过资产名称查找对应的原始记录
                        string yuanShiMingCheng = jieChuGuZiMing.Replace("(借出)", "").Trim();

                        string chaXunYuanShiSQL = @"select top 1 GuZiID, GuZiShuLiang, GuZiJiaZhi 
                                                   from GudinZiChan 
                                                   where GuZiMing = @GuZiMing 
                                                   and ZhuangTaiID = 1
                                                   order by GuZiID";

                        string yuanShiGuZiID = "";
                        int yuanShiShuLiang = 0;
                        decimal yuanShiJiaZhi = 0;

                        using (SqlCommand mingLing = new SqlCommand(chaXunYuanShiSQL, lianJie, shiWu))
                        {
                            mingLing.Parameters.Add(new SqlParameter("@GuZiMing", yuanShiMingCheng));

                            using (SqlDataReader duZhe = mingLing.ExecuteReader())
                            {
                                if (duZhe.Read())
                                {
                                    yuanShiGuZiID = duZhe["GuZiID"].ToString();
                                    yuanShiShuLiang = Convert.ToInt32(duZhe["GuZiShuLiang"]);
                                    yuanShiJiaZhi = Convert.ToDecimal(duZhe["GuZiJiaZhi"]);
                                }
                            }
                        }

                        // 3. 获取原始资产ID用于处理JieHuan表
                        string jieHuanGuZiID = !string.IsNullOrEmpty(yuanShiGuZiID) ? yuanShiGuZiID : jieChuZiChanID;

                        // 4. 处理JieHuan表
                        int dangQianJieChuShuLiang = Convert.ToInt32(jieChuZiChan.GuZiShuLiang);

                        if (guiHuanShuLiang == dangQianJieChuShuLiang)
                        {
                            // 全部归还
                            string gaiWeiGuiHuanSQL = @"
                                UPDATE JieHuan 
                                SET LeiXin = '归还',
                                    RiQi = @GuiHuanRiQi
                                WHERE GuZiID = @GuZiID 
                                AND GongSi = @JieChuGongSi
                                AND ShuLiang = @JieChuShuLiang
                                AND LeiXin = '借出'";

                            string jieChuGongSi = jieChuZiChan.Gongsi;

                            SqlParameter[] gaiWeiGuiHuanCanShu = new SqlParameter[4];
                            gaiWeiGuiHuanCanShu[0] = new SqlParameter("@GuZiID", jieHuanGuZiID);
                            gaiWeiGuiHuanCanShu[1] = new SqlParameter("@GuiHuanRiQi", guiHuanRiQi);
                            gaiWeiGuiHuanCanShu[2] = new SqlParameter("@JieChuGongSi", jieChuGongSi);
                            gaiWeiGuiHuanCanShu[3] = new SqlParameter("@JieChuShuLiang", dangQianJieChuShuLiang);

                            using (SqlCommand mingLing = new SqlCommand(gaiWeiGuiHuanSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.AddRange(gaiWeiGuiHuanCanShu);
                                int gengXinHangShu = mingLing.ExecuteNonQuery();

                                if (gengXinHangShu == 0)
                                {
                                    string chaRuJieHuanSQL = @"insert into JieHuan (GuZiID, GongSi, ShuLiang, RiQi, LeiXin) 
                                                             values (@GuZiID, @GongSi, @ShuLiang, @RiQi, @LeiXin)";

                                    SqlParameter[] chaRuJieHuanCanShu = new SqlParameter[5];
                                    chaRuJieHuanCanShu[0] = new SqlParameter("@GuZiID", jieHuanGuZiID);
                                    chaRuJieHuanCanShu[1] = new SqlParameter("@GongSi", guiHuanGongSi);
                                    chaRuJieHuanCanShu[2] = new SqlParameter("@ShuLiang", guiHuanShuLiang);
                                    chaRuJieHuanCanShu[3] = new SqlParameter("@RiQi", guiHuanRiQi);
                                    chaRuJieHuanCanShu[4] = new SqlParameter("@LeiXin", "归还");

                                    using (SqlCommand chaRuMingLing = new SqlCommand(chaRuJieHuanSQL, lianJie, shiWu))
                                    {
                                        chaRuMingLing.Parameters.AddRange(chaRuJieHuanCanShu);
                                        chaRuMingLing.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 部分归还
                            string jieChuGongSi = jieChuZiChan.Gongsi;

                            string jianShaoJieChuSQL = @"
                                UPDATE JieHuan 
                                SET ShuLiang = ShuLiang - @GuiHuanShuLiang
                                WHERE GuZiID = @GuZiID 
                                AND GongSi = @JieChuGongSi
                                AND ShuLiang >= @GuiHuanShuLiang
                                AND LeiXin = '借出'";

                            SqlParameter[] jianShaoJieChuCanShu = new SqlParameter[3];
                            jianShaoJieChuCanShu[0] = new SqlParameter("@GuZiID", jieHuanGuZiID);
                            jianShaoJieChuCanShu[1] = new SqlParameter("@JieChuGongSi", jieChuGongSi);
                            jianShaoJieChuCanShu[2] = new SqlParameter("@GuiHuanShuLiang", guiHuanShuLiang);

                            using (SqlCommand mingLing = new SqlCommand(jianShaoJieChuSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.AddRange(jianShaoJieChuCanShu);
                                mingLing.ExecuteNonQuery();
                            }
                        }

                        // 5. 更新或创建原始资产记录
                        if (!string.IsNullOrEmpty(yuanShiGuZiID))
                        {
                            // 存在原始记录，更新数量
                            int xinZaiKuShuLiang = yuanShiShuLiang + guiHuanShuLiang;
                            decimal xinZaiKuJiaZhi = yuanShiJiaZhi + (jieChuJiaZhi / dangQianJieChuShuLiang * guiHuanShuLiang);

                            string gengXinYuanShiSQL = @"update GudinZiChan 
                                                        set GuZiShuLiang = @XinShuLiang,
                                                            GuZiJiaZhi = @XinJiaZhi
                                                        where GuZiID = @GuZiID";

                            SqlParameter[] gengXinYuanShiCanShu = new SqlParameter[3];
                            gengXinYuanShiCanShu[0] = new SqlParameter("@XinShuLiang", xinZaiKuShuLiang);
                            gengXinYuanShiCanShu[1] = new SqlParameter("@XinJiaZhi", xinZaiKuJiaZhi);
                            gengXinYuanShiCanShu[2] = new SqlParameter("@GuZiID", yuanShiGuZiID);

                            using (SqlCommand mingLing = new SqlCommand(gengXinYuanShiSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.AddRange(gengXinYuanShiCanShu);
                                mingLing.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // 不存在原始记录，创建新记录
                            decimal guiHuanJiaZhi = jieChuJiaZhi / dangQianJieChuShuLiang * guiHuanShuLiang;

                            string chaRuXinZiChanSQL = @"insert into GudinZiChan (GuZiMing, CangKuID, GuZiShuLiang, GuZiJiaZhi, ChuChangRiQi, ZhuangTaiID) 
                                                       values (@GuZiMing, @CangKuID, @GuZiShuLiang, @GuZiJiaZhi, @ChuChangRiQi, @ZhuangTaiID)";

                            SqlParameter[] chaRuXinZiChanCanShu = new SqlParameter[6];
                            chaRuXinZiChanCanShu[0] = new SqlParameter("@GuZiMing", yuanShiMingCheng);
                            chaRuXinZiChanCanShu[1] = new SqlParameter("@CangKuID", jieChuCangKuID);
                            chaRuXinZiChanCanShu[2] = new SqlParameter("@GuZiShuLiang", guiHuanShuLiang);
                            chaRuXinZiChanCanShu[3] = new SqlParameter("@GuZiJiaZhi", guiHuanJiaZhi);
                            chaRuXinZiChanCanShu[4] = new SqlParameter("@ChuChangRiQi", jieChuChuChangRiQi);
                            chaRuXinZiChanCanShu[5] = new SqlParameter("@ZhuangTaiID", 1);

                            using (SqlCommand mingLing = new SqlCommand(chaRuXinZiChanSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.AddRange(chaRuXinZiChanCanShu);
                                mingLing.ExecuteNonQuery();
                            }
                        }

                        // 6. 更新借出记录
                        int xinJieChuShuLiang = dangQianJieChuShuLiang - guiHuanShuLiang;
                        decimal xinJieChuJiaZhi = jieChuJiaZhi / dangQianJieChuShuLiang * xinJieChuShuLiang;

                        if (xinJieChuShuLiang == 0)
                        {
                            // 全部归还，删除借出记录
                            string shanChuSQL = @"delete from GudinZiChan where GuZiID = @GuZiID";

                            SqlParameter shanChuCanShu = new SqlParameter("@GuZiID", jieChuZiChanID);

                            using (SqlCommand mingLing = new SqlCommand(shanChuSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.Add(shanChuCanShu);
                                mingLing.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // 部分归还，更新借出记录
                            string gengXinJieChuSQL = @"update GudinZiChan 
                                                       set GuZiShuLiang = @XinShuLiang,
                                                           GuZiJiaZhi = @XinJiaZhi
                                                       where GuZiID = @GuZiID";

                            SqlParameter[] gengXinJieChuCanShu = new SqlParameter[3];
                            gengXinJieChuCanShu[0] = new SqlParameter("@XinShuLiang", xinJieChuShuLiang);
                            gengXinJieChuCanShu[1] = new SqlParameter("@XinJiaZhi", xinJieChuJiaZhi);
                            gengXinJieChuCanShu[2] = new SqlParameter("@GuZiID", jieChuZiChanID);

                            using (SqlCommand mingLing = new SqlCommand(gengXinJieChuSQL, lianJie, shiWu))
                            {
                                mingLing.Parameters.AddRange(gengXinJieChuCanShu);
                                mingLing.ExecuteNonQuery();
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

                        throw new Exception("保存失败：" + ex.Message);
                    }
                }
            }
        }
    }
}