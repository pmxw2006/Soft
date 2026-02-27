using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class JieHuanManager
    {
        /// <summary>
        /// 获取借用记录数据（业务层）
        /// </summary>
        public static DataTable HuoQuJieHuanShuJu_BLL(string guZiID, string guZiMing, string leixing, int kaishiweizhi, int meiYeShuLiang)
        {
            return JieHuanService.HuoQuJieHuanShuJu(guZiID, guZiMing, leixing, kaishiweizhi, meiYeShuLiang);
        }

        /// <summary>
        /// 获取总记录数（业务层）
        /// </summary>
        public static int HuoQuZongJiLuShu_BLL(string guZiID, string guZiMing, string leixing)
        {
            return JieHuanService.HuoQuZongJiLuShu(guZiID, guZiMing, leixing);
        }

        /// <summary>
        /// 计算分页相关参数
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
        /// 设置分页按钮状态
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
    }
}