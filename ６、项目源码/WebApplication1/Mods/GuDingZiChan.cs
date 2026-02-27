using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mods
{
    /// <summary>
    /// 固定资产
    /// </summary>
    public class GuDingZiChan
    {
        string _guZiID;
        string _guZiMing;
        string _cangKuID;
        string _guZiShuLiang;
        Decimal _guZiJiaZhi;
        DateTime _chuChangRiQi;
        int _zhuangTaiID;
        string _leixing;
        string _gongsi;
        /// <summary>
        /// 固定资产编号
        /// </summary>
        public string GuZiID { get => _guZiID; set => _guZiID = value; }
        /// <summary>
        /// 固定资产名称
        /// </summary>
        public string GuZiMing { get => _guZiMing; set => _guZiMing = value; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public string CangKuID { get => _cangKuID; set => _cangKuID = value; }
        /// <summary>
        /// 固定资产数量
        /// </summary>
        public string GuZiShuLiang { get => _guZiShuLiang; set => _guZiShuLiang = value; }
        /// <summary>
        /// 固定资产价值
        /// </summary>
        public decimal GuZiJiaZhi { get => _guZiJiaZhi; set => _guZiJiaZhi = value; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime ChuChangRiQi { get => _chuChangRiQi; set => _chuChangRiQi = value; }
        /// <summary>
        /// 状态编号
        /// </summary>
        public int ZhuangTaiID { get => _zhuangTaiID; set => _zhuangTaiID = value; }
        /// <summary>
        /// 借还类型
        /// </summary>
        public string Leixing { get => _leixing; set => _leixing = value; }
        /// <summary>
        /// 借出公司
        /// </summary>
        public string Gongsi { get => _gongsi; set => _gongsi = value; }
    }
}
