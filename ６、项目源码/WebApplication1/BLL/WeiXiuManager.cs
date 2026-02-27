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
    public class WeiXiuManager
    {
        /// <summary>
        /// 获取维修记录总数（业务层）
        /// </summary>
        public static int HuoQuWeiXiuZongShu_BLL(string guZiID, string guZiMing)
        {
            return WeiXiuService.HuoQuWeiXiuZongShu(guZiID, guZiMing);
        }

        /// <summary>
        /// 获取维修记录列表（业务层）
        /// </summary>
        public static DataTable HuoQuWeiXiuLieBiao_BLL(string guZiID, string guZiMing, int kaishiweizhi, int meiYeShuLiang)
        {
            return WeiXiuService.HuoQuWeiXiuLieBiao(guZiID, guZiMing, kaishiweizhi, meiYeShuLiang);
        }

        /// <summary>
        /// 完成维修操作（业务层）
        /// </summary>
        public static bool WanChengWeiXiu_BLL(string guZiID, DateTime shiWanRiQi)
        {
            return WeiXiuService.WanChengWeiXiu(guZiID, shiWanRiQi);
        }

        /// <summary>
        /// 计算分页相关参数（业务层）
        /// </summary>
        public static void JiSuanFenYeCanShu(ref int dangQianYe, ref int zongYeShu, int zongJiLuShu, int meiYeShuLiang)
        {
            // 计算总页数
            zongYeShu = zongJiLuShu / meiYeShuLiang;
            int qumo = zongJiLuShu % meiYeShuLiang;

            // 如果取模大于0则代表有余数则总页数+1
            if (qumo > 0)
            {
                zongYeShu++;
            }

            // 如果总页数为0则+1
            if (zongYeShu == 0)
            {
                zongYeShu = 1;
            }

            // 确保当前页在有效范围内
            if (dangQianYe < 1)
            {
                dangQianYe = 1;
            }
            if (dangQianYe > zongYeShu)
            {
                dangQianYe = zongYeShu;
            }
        }

        /// <summary>
        /// 设置分页按钮状态（业务层）
        /// </summary>
        public static void SheZhiAnNiuZhuangTai(ref bool shouYeKeYong, ref bool shangYiYeKeYong,
                                              ref bool xiaYiYeKeYong, ref bool moYeKeYong,
                                              int dangQianYe, int zongYeShu)
        {
            // 如果是第一页，禁用首页和上一页
            shouYeKeYong = (dangQianYe > 1);
            shangYiYeKeYong = (dangQianYe > 1);

            // 如果是最后一页，禁用下一页和末页
            xiaYiYeKeYong = (dangQianYe < zongYeShu);
            moYeKeYong = (dangQianYe < zongYeShu);
        }

        /// <summary>
        /// 获取当前日期（业务层）
        /// </summary>
        public static DateTime HuoQuDangQianRiQi()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 验证完修操作参数（业务层）
        /// </summary>
        public static string YanZhengWanXiuCanShu(string guZiID)
        {
            if (string.IsNullOrEmpty(guZiID))
            {
                return "获取固定资产ID失败！";
            }

            return "";
        }
    }
}