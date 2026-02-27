using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.Data;
using System.Security.Cryptography;

namespace BLL
{
    public class YongHuManager
    {
        public static bool ChaXun_BLL(YongHu yongHu)
        {
            string YongHuID = YongHuService.ChaXun(yongHu);
            if (YongHuID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ChaXun_wang_BLL(YongHu yongHu)
        {
            string YongHuID = YongHuService.ChaXun_wang(yongHu);
            if (YongHuID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool XiuGai_wang_BLL(YongHu yongHu)
        {
            return YongHuService.XiuGai_wang(yongHu);
        }
        public static bool ChaXun_gai_BLL(YongHu yongHu)
        {
            string YongHuID = YongHuService.ChaXun_gai(yongHu);
            if (YongHuID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="shuru"></param>
        /// <returns></returns>
        public static string JiaMi(string shuru)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] zijieshuzu = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(shuru));
                StringBuilder goujianqi = new StringBuilder();

                for (int i = 0; i < zijieshuzu.Length; i++)
                {
                    goujianqi.Append(zijieshuzu[i].ToString("x2"));
                }

                return goujianqi.ToString();
            }
        }

        // 处理Cookie自动登录
        public static DengLuJieGuo ChuLiCookieZiDongDengLu(string cookieZhi)
        {
            DengLuJieGuo jieguo = new DengLuJieGuo();

            try
            {
                // 从DAL获取所有用户
                DataTable suoyouyonghu = YongHuService.HuoQuSuoYouYongHu();

                if (suoyouyonghu == null || suoyouyonghu.Rows.Count == 0)
                {
                    jieguo.Zhuangtai = "MeiYouYongHu";
                    return jieguo;
                }

                // 遍历比对
                foreach (DataRow hang in suoyouyonghu.Rows)
                {
                    string yongHuID = hang["YongHuID"].ToString();

                    // 加密比对
                    if (JiaMi(yongHuID) == cookieZhi)
                    {
                        // 找到匹配用户
                        jieguo.Zhuangtai = "ChengGong";
                        jieguo.YongHuID = yongHuID;
                        jieguo.BuMen = hang["BuMen"].ToString();
                        return jieguo;
                    }
                }

                // 没找到
                jieguo.Zhuangtai = "WuXiaoCookie";
                return jieguo;
            }
            catch
            {
                jieguo.Zhuangtai = "CuoWu";
                return jieguo;
            }
        }

        /// <summary>
        /// 检查用户权限并返回应跳转的页面
        /// </summary>
        public static string JianChaQuanXian(string yongHuID, string buMen, string dangQianYeMian)
        {
            // 如果部门为空，重新查询
            if (string.IsNullOrEmpty(buMen) && !string.IsNullOrEmpty(yongHuID))
            {
                buMen = YongHuService.HuoQuYongHuBuMen(yongHuID);
            }

            // 无法获取部门信息
            if (string.IsNullOrEmpty(buMen))
            {
                return "/DengLu/DengLuYe.aspx";
            }

            // 判断应该跳转到哪个主页
            if (buMen == "系统")
            {
                // 系统权限用户
                if (dangQianYeMian.ToLower().Contains("/gudinzichan/"))
                {
                    // 当前在固定资产页面，但用户是系统权限，跳转到系统主页
                    return "/XiTong/ZhuTi.aspx";
                }
                // 否则留在当前页面
                return "";
            }
            else if (buMen == "固定资产")
            {
                // 固定资产权限用户
                if (dangQianYeMian.ToLower().Contains("/xitong/"))
                {
                    // 当前在系统页面，但用户是固定资产权限，跳转到固定资产主页
                    return "/GuDinZiChan/ZhuTi.aspx";
                }
                // 否则留在当前页面
                return "";
            }
            else
            {
                // 其他部门，跳转到登录页
                return "/DengLu/DengLuYe.aspx";
            }
        }

        /// <summary>
        /// 获取用户个人信息（业务层）
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <returns>用户对象</returns>
        public static YongHu HuoQuGeRenXinXi_BLL(string yongHuID)
        {
            return YongHuService.HuoQuGeRenXinXi(yongHuID);
        }

        /// <summary>
        /// 修改用户个人信息（业务层）
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <param name="yongHuMing">用户名</param>
        /// <param name="xingMing">真实姓名</param>
        /// <returns>是否成功</returns>
        public static bool XiuGaiGeRenXinXi_BLL(string yongHuID, string yongHuMing, string xingMing)
        {
            return YongHuService.XiuGaiGeRenXinXi(yongHuID, yongHuMing, xingMing);
        }
        /// <summary>
        /// 添加新用户（业务层）
        /// </summary>
        /// <param name="yongHu">用户对象</param>
        /// <returns>是否添加成功</returns>
        public static bool TianJiaYongHu_BLL(YongHu yongHu)
        {
            // 调用DAL层方法
            return YongHuService.TianJiaYongHu(yongHu);
        }

        /// <summary>
        /// 获取用户数据（业务层）
        /// </summary>
        /// <param name="yongHu">用户对象（包含搜索条件）</param>
        /// <param name="kaishiweizhi">开始位置</param>
        /// <param name="meiYeShuLiang">每页数量</param>
        /// <returns>DataTable数据</returns>
        public static DataTable HuoQuYongHuShuJu_BLL(YongHu yongHu, int kaishiweizhi, int meiYeShuLiang)
        {
            return YongHuService.HuoQuYongHuShuJu(yongHu, kaishiweizhi, meiYeShuLiang);
        }

        /// <summary>
        /// 获取总记录数（业务层）
        /// </summary>
        /// <param name="yongHu">用户对象（包含搜索条件）</param>
        /// <returns>总记录数</returns>
        public static int HuoQuZongJiLuShu_BLL(YongHu yongHu)
        {
            return YongHuService.HuoQuZongJiLuShu(yongHu);
        }

        /// <summary>
        /// 删除用户（业务层）
        /// </summary>
        /// <param name="yongHuID">用户ID</param>
        /// <returns>是否删除成功</returns>
        public static bool ShanChuYongHu_BLL(string yongHuID)
        {
            return YongHuService.ShanChuYongHu(yongHuID);
        }
    }
}
