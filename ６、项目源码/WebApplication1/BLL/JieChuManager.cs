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
    public class JieChuManager
    {
        /// <summary>
        /// 获取在库中资产列表（业务层）
        /// </summary>
        public static DataTable HuoQuZaiKuZiChanLieBiao_BLL()
        {
            return JieChuService.HuoQuZaiKuZiChanLieBiao();
        }

        /// <summary>
        /// 获取资产详细信息（业务层）
        /// </summary>
        public static GuDingZiChan HuoQuZiChanXiangXiXinXi_BLL(string ziChanID)
        {
            return JieChuService.HuoQuZiChanXiangXiXinXi(ziChanID);
        }

        /// <summary>
        /// 保存借出记录（业务层）
        /// </summary>
        public static bool BaoCunJieChuJiLu_BLL(GuDingZiChan ziChan, int jieChuShuLiang, string zuJieGongSi, DateTime niHuanRiQi)
        {
            return JieChuService.BaoCunJieChuJiLu(ziChan, jieChuShuLiang, zuJieGongSi, niHuanRiQi);
        }

        /// <summary>
        /// 验证借出表单（业务层）
        /// </summary>
        public static string YanZhengJieChuBiaoDan(string ziChanID, string jieChuShuLiangText, string zuJieGongSi, string niHuanRiQiText)
        {
            if (string.IsNullOrEmpty(ziChanID) || ziChanID == "0")
            {
                return "请选择要借出的资产！";
            }

            if (string.IsNullOrWhiteSpace(jieChuShuLiangText))
            {
                return "请输入借出数量！";
            }

            if (string.IsNullOrWhiteSpace(zuJieGongSi))
            {
                return "请输入租借公司名称！";
            }

            if (string.IsNullOrWhiteSpace(niHuanRiQiText))
            {
                return "请选择拟还日期！";
            }

            return "";
        }

        /// <summary>
        /// 验证借出业务逻辑（业务层）
        /// </summary>
        public static string YanZhengJieChuYeWuLuoJi(GuDingZiChan ziChan, int jieChuShuLiang, int dangQianShuLiang, DateTime niHuanRiQi)
        {
            // 检查借出数量是否超过当前库存
            if (jieChuShuLiang > dangQianShuLiang)
            {
                return $"借出数量({jieChuShuLiang})不能超过当前库存({dangQianShuLiang})！";
            }

            // 检查借出数量是否为正数
            if (jieChuShuLiang <= 0)
            {
                return "借出数量必须大于0！";
            }

            // 检查资产状态（应该是在库中）
            if (ziChan.ZhuangTaiID != 1)
            {
                return "该资产当前不可借出，请选择其他资产！";
            }

            // 检查日期是否大于今天
            DateTime jinTian = DateTime.Today;
            if (niHuanRiQi <= jinTian)
            {
                return "拟还日期必须大于今天！";
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
        /// 获取明天日期字符串（业务层）
        /// </summary>
        public static string HuoQuMingTianRiQiWenBen()
        {
            return DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
    }
}