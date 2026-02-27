using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CangKuGuanLi
    {
        int _cangKuID;
        string _cangKuMing;
        int _leiXingID;
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int CangKuID { get => _cangKuID; set => _cangKuID = value; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string CangKuMing { get => _cangKuMing; set => _cangKuMing = value; }
        /// <summary>
        /// 类型编号
        /// </summary>
        public int LeiXingID { get => _leiXingID; set => _leiXingID = value; }
    }
}
