using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mods
{
    /// <summary>
    /// 借出信息实体类
    /// </summary>
    public class JieChuInfo
    {
        string _ziChanID;
        int _jieChuShuLiang;
        string _zuJieGongSi;
        DateTime _niHuanRiQi;

        /// <summary>
        /// 资产ID
        /// </summary>
        public string ZiChanID { get => _ziChanID; set => _ziChanID = value; }

        /// <summary>
        /// 借出数量
        /// </summary>
        public int JieChuShuLiang { get => _jieChuShuLiang; set => _jieChuShuLiang = value; }

        /// <summary>
        /// 租借公司
        /// </summary>
        public string ZuJieGongSi { get => _zuJieGongSi; set => _zuJieGongSi = value; }

        /// <summary>
        /// 拟还日期
        /// </summary>
        public DateTime NiHuanRiQi { get => _niHuanRiQi; set => _niHuanRiQi = value; }
    }
}