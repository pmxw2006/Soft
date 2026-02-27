using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class DengLuJieGuo
    {
        public string Zhuangtai { get; set; }   // 状态：成功/无效Cookie/没有用户/错误
        public string YongHuID { get; set; }    // 用户ID
        public string BuMen { get; set; }       // 部门
    }
}
