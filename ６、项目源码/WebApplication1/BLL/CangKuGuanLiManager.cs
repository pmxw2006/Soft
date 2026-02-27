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
    public class CangKuGuanLiManager
    {
        /// <summary>
        /// 添加仓库分类
        /// </summary>
        /// <param name="CangKuMing"></param>
        /// <returns></returns>
        public static bool XinZenCangKu(string CangKuMing)
        {
            return CangKuGuanLiServices.XinZenCangKu(CangKuMing);
        }

        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="CangKuMing"></param>
        /// <returns></returns>
        public static DataTable ChaXunCangKu()
        {
            return CangKuGuanLiServices.ChaXunCangKu();
        }

       

        /// <summary>
        /// 添加仓库
        /// </summary>
        /// <param name="CangKuMing"></param>
        /// <param name="LeiXingID"></param>
        /// <returns></returns>
        public static bool XinZenCangKuDongXi(string CangKuMing, int LeiXingID)
        {
            return CangKuGuanLiServices.XinZenCangKuDongXi(CangKuMing, LeiXingID);
        }

        /// <summary>
        /// 修改仓库姓名
        /// </summary>
        /// <param name="CangKuID"></param>
        /// <param name="CangKuXingMin"></param>
        /// <returns></returns>
        public static bool CangKuXiGai(string CangKuID, string CangKuXingMin)
        {
            return CangKuGuanLiServices.CangKuXiGai(CangKuID, CangKuXingMin);
        }

        /// <summary>
        /// 显示仓库信息
        /// </summary>
        /// <param name="CangKuID"></param>
        /// <returns></returns>
        public static DataTable XianShiXinXi(string CangKuID)
        {
            return CangKuGuanLiServices.XianShiXinXi(CangKuID);
        }

        public static DataTable ChaXunCangKuMoHu(string BianHao, string CangKuMing)
        {
            return CangKuGuanLiServices.ChaXunCangKuMoHu(BianHao, CangKuMing);
        }

        public static DataTable ChaXunCangKuFenLei()
        {
            return CangKuGuanLiServices.ChaXunCangKuFenLei();
        }

        /// <summary>
        /// 检查分类名称是否已存在
        /// </summary>
        /// <param name="CangKuMing">分类名称</param>
        /// <returns>true=已存在，false=不存在</returns>
        public static bool ChaXunFenLeiShiFouCunZai(string CangKuMing)
        {
            return CangKuGuanLiServices.ChaXunFenLeiShiFouCunZai(CangKuMing);
        }

        public static bool ChaXunCangKuShiFouCunZai(string CangKuMing, int LeiXingID = 0)
        {
            return CangKuGuanLiServices.ChaXunCangKuShiFouCunZai(CangKuMing, LeiXingID);
        }

        public static int HuoQuCangKuFenLeiID(string CangKuID)
{
    return CangKuGuanLiServices.HuoQuCangKuFenLeiID(CangKuID);
}
    }
}


