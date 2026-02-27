using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL
{
   public class GuDingZiChanManager
    {
        /// <summary>
        /// 显示固定资产
        /// </summary>
        /// <returns></returns>
        public static DataTable XianShiXinXi()
        {

            return GuDingZiChanServices.XianShiXinXi();
        }
        /// <summary>
        /// 删除固定资产
        /// </summary>
        /// <param name="GuZiID"></param>
        /// <returns></returns>
        public static bool ShanCuCangKu(string GuZiID)
        {

            return GuDingZiChanServices.ShanCuCangKu(GuZiID);
        }
        public static DataTable MoHuChaXun(string keyword)
        {
            return GuDingZiChanServices.MoHuChaXun(keyword);
        }
    }
}
