using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mods
{
    /// <summary>
    /// 借用记录实体类
    /// </summary>
    public class JieHuan
    {
        int _guZiID;
        string _guZiMing;
        string _gongSi;
        int _shuLiang;
        DateTime _riQi;
        string _leiXing;

        /// <summary>
        /// 资产编号
        /// </summary>
        public int GuZiID { get => _guZiID; set => _guZiID = value; }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string GuZiMing { get => _guZiMing; set => _guZiMing = value; }

        /// <summary>
        /// 公司
        /// </summary>
        public string GongSi { get => _gongSi; set => _gongSi = value; }

        /// <summary>
        /// 数量
        /// </summary>
        public int ShuLiang { get => _shuLiang; set => _shuLiang = value; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime RiQi { get => _riQi; set => _riQi = value; }

        /// <summary>
        /// 类型
        /// </summary>
        public string LeiXing { get => _leiXing; set => _leiXing = value; }

        // 搜索条件
        public string GuZiIDSearch { get; set; }
        public string GuZiMingSearch { get; set; }
        public string LeixingSearch { get; set; }
    }
}