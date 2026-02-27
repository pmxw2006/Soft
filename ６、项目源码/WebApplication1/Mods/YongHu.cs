using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    /// <summary>
    /// 用户
    /// </summary>
    public class YongHu
    {
        string _yongHuID;
        string _yongHuName;
        string _miMa;
        string _xingMing;
        string _shenFenZheng;
        string _buMen;
        /// <summary>
        /// 用户账号
        /// </summary>
        public string YongHuID { get => _yongHuID; set => _yongHuID = value; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string YongHuName { get => _yongHuName; set => _yongHuName = value; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string MiMa { get => _miMa; set => _miMa = value; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string XingMing { get => _xingMing; set => _xingMing = value; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string ShenFenZheng { get => _shenFenZheng; set => _shenFenZheng = value; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string BuMen { get => _buMen; set => _buMen = value; }
    }
}