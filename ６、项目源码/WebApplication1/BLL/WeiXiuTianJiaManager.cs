using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mods;
using DAL;
using System.Data;

namespace BLL
{
    public class WeiXiuTianJiaManager
    {
        /// <summary>
        /// 获取在库中资产列表（业务层）
        /// </summary>
        public static DataTable HuoQuZaiKuZiChanLieBiao_BLL()
        {
            return WeiXiuTianJiaService.HuoQuZaiKuZiChanLieBiao();
        }

        /// <summary>
        /// 获取资产详细信息（业务层）
        /// </summary>
        public static GuDingZiChan HuoQuZiChanXiangXiXinXi_BLL(string ziChanID)
        {
            return WeiXiuTianJiaService.HuoQuZiChanXiangXiXinXi(ziChanID);
        }

        /// <summary>
        /// 保存维修记录（业务层）
        /// </summary>
        public static bool BaoCunWeiXiuJiLu_BLL(GuDingZiChan ziChan, int weiXiuShuLiang, DateTime songXiuRiQi,
                                               DateTime yuWanRiQi, decimal weiXiuJinE)
        {
            return WeiXiuTianJiaService.BaoCunWeiXiuJiLu(ziChan, weiXiuShuLiang, songXiuRiQi, yuWanRiQi, weiXiuJinE);
        }

        /// <summary>
        /// 验证维修表单（业务层）
        /// </summary>
        public static string YanZhengWeiXiuBiaoDan(string ziChanID, string weiXiuShuLiangText,
                                                  string songXiuRiQiText, string yuWanRiQiText,
                                                  string weiXiuJinEText)
        {
            if (string.IsNullOrEmpty(ziChanID) || ziChanID == "0")
            {
                return "请选择要维修的资产！";
            }

            if (string.IsNullOrWhiteSpace(weiXiuShuLiangText))
            {
                return "请输入维修数量！";
            }

            if (string.IsNullOrWhiteSpace(songXiuRiQiText))
            {
                return "请选择送修日期！";
            }

            if (string.IsNullOrWhiteSpace(yuWanRiQiText))
            {
                return "请选择预期完成日期！";
            }

            if (string.IsNullOrWhiteSpace(weiXiuJinEText))
            {
                return "请输入维修金额！";
            }

            return "";
        }

        /// <summary>
        /// 验证维修业务逻辑（业务层）
        /// </summary>
        public static string YanZhengWeiXiuYeWuLuoJi(GuDingZiChan ziChan, int weiXiuShuLiang, int dangQianShuLiang,
                                                    DateTime songXiuRiQi, DateTime yuWanRiQi, decimal weiXiuJinE)
        {
            // 检查维修数量是否超过当前库存
            if (weiXiuShuLiang > dangQianShuLiang)
            {
                return $"维修数量({weiXiuShuLiang})不能超过当前库存({dangQianShuLiang})！";
            }

            // 检查维修数量是否为正数
            if (weiXiuShuLiang <= 0)
            {
                return "维修数量必须大于0！";
            }

            // 检查资产状态（应该是在库中）
            if (ziChan.ZhuangTaiID != 1)
            {
                return "该资产当前不可维修！";
            }

            // 检查送修日期是否小于等于今天
            DateTime jinTian = DateTime.Today;
            if (songXiuRiQi > jinTian)
            {
                return "送修日期不能是未来！";
            }

            // 检查预期完成日期是否大于送修日期
            if (yuWanRiQi <= songXiuRiQi)
            {
                return "预期完成日期必须大于送修日期！";
            }

            // 检查维修金额是否为正数
            if (weiXiuJinE <= 0)
            {
                return "维修金额必须大于0！";
            }

            return "";
        }

        /// <summary>
        /// 安全解析日期（业务层）
        /// </summary>
        public static bool AnQuanJieXiRiQi(string riQiWenBen, out DateTime riQi)
        {
            return DateTime.TryParse(riQiWenBen, out riQi);
        }

        /// <summary>
        /// 安全解析十进制数字（业务层）
        /// </summary>
        public static bool AnQuanJieXiXiaoShu(string wenBen, out decimal jieGuo)
        {
            return decimal.TryParse(wenBen, out jieGuo);
        }

        /// <summary>
        /// 安全解析整数（业务层）
        /// </summary>
        public static bool AnQuanJieXiZhengShu(string wenBen, out int jieGuo)
        {
            return int.TryParse(wenBen, out jieGuo);
        }

        /// <summary>
        /// 获取当前日期字符串（业务层）
        /// </summary>
        public static string HuoQuDangQianRiQiWenBen()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取明天日期字符串（业务层）
        /// </summary>
        public static string HuoQuMingTianRiQiWenBen()
        {
            return DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
    }
}