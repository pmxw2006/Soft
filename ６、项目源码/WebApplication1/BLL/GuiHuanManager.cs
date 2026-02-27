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
    public class GuiHuanManager
    {
        /// <summary>
        /// 获取借出中资产列表（业务层）
        /// </summary>
        public static DataTable HuoQuJieChuZiChanLieBiao_BLL()
        {
            return GuiHuanService.HuoQuJieChuZiChanLieBiao();
        }

        /// <summary>
        /// 获取资产详细信息（业务层）
        /// </summary>
        public static GuDingZiChan HuoQuZiChanXiangXiXinXi_BLL(string ziChanID)
        {
            return GuiHuanService.HuoQuZiChanXiangXiXinXi(ziChanID);
        }

        /// <summary>
        /// 保存归还记录（业务层）
        /// </summary>
        public static bool BaoCunGuiHuanJiLu_BLL(GuDingZiChan jieChuZiChan, int guiHuanShuLiang, string guiHuanGongSi, DateTime guiHuanRiQi)
        {
            return GuiHuanService.BaoCunGuiHuanJiLu(jieChuZiChan, guiHuanShuLiang, guiHuanGongSi, guiHuanRiQi);
        }

        /// <summary>
        /// 验证归还表单（业务层）
        /// </summary>
        public static string YanZhengGuiHuanBiaoDan(string ziChanID, string guiHuanShuLiangText, string guiHuanGongSi, string guiHuanRiQiText)
        {
            if (string.IsNullOrEmpty(ziChanID) || ziChanID == "0")
            {
                return "请选择要归还的资产！";
            }

            if (string.IsNullOrWhiteSpace(guiHuanShuLiangText))
            {
                return "请输入归还数量！";
            }

            if (string.IsNullOrWhiteSpace(guiHuanGongSi))
            {
                return "请输入归还公司名称！";
            }

            if (string.IsNullOrWhiteSpace(guiHuanRiQiText))
            {
                return "请选择归还日期！";
            }

            return "";
        }

        /// <summary>
        /// 验证归还业务逻辑（业务层）
        /// </summary>
        public static string YanZhengGuiHuanYeWuLuoJi(GuDingZiChan ziChan, int guiHuanShuLiang, int dangQianJieChuShuLiang, DateTime guiHuanRiQi)
        {
            // 检查归还数量是否超过借出数量
            if (guiHuanShuLiang > dangQianJieChuShuLiang)
            {
                return $"归还数量({guiHuanShuLiang})不能超过当前借出数量({dangQianJieChuShuLiang})！";
            }

            // 检查归还数量是否为正数
            if (guiHuanShuLiang <= 0)
            {
                return "归还数量必须大于0！";
            }

            // 检查资产状态（应该是借出中）
            if (ziChan.ZhuangTaiID != 3)
            {
                return "该资产当前不是借出状态！";
            }

            // 检查日期是否小于等于今天
            DateTime jinTian = DateTime.Today;
            if (guiHuanRiQi > jinTian)
            {
                return "归还日期不能是未来！";
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
    }
}