using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mods
{
    /// <summary>
    /// 维修记录实体类
    /// </summary>
    public class WeiXiu
    {
        string _guZiID;
        string _guZiMing;
        int _weiXiuShuLiang;
        DateTime _songXiuRiQi;
        DateTime? _yuWanRiQi;
        DateTime? _shiWanRiQi;
        decimal _weiXiuJinE;

        /// <summary>
        /// 资产ID
        /// </summary>
        public string GuZiID { get => _guZiID; set => _guZiID = value; }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string GuZiMing { get => _guZiMing; set => _guZiMing = value; }

        /// <summary>
        /// 维修数量
        /// </summary>
        public int WeiXiuShuLiang { get => _weiXiuShuLiang; set => _weiXiuShuLiang = value; }

        /// <summary>
        /// 送修日期
        /// </summary>
        public DateTime SongXiuRiQi { get => _songXiuRiQi; set => _songXiuRiQi = value; }

        /// <summary>
        /// 预计完成日期
        /// </summary>
        public DateTime? YuWanRiQi { get => _yuWanRiQi; set => _yuWanRiQi = value; }

        /// <summary>
        /// 实际完成日期
        /// </summary>
        public DateTime? ShiWanRiQi { get => _shiWanRiQi; set => _shiWanRiQi = value; }

        /// <summary>
        /// 维修金额
        /// </summary>
        public decimal WeiXiuJinE { get => _weiXiuJinE; set => _weiXiuJinE = value; }

        // 搜索条件
        public string GuZiIDSearch { get; set; }
        public string GuZiMingSearch { get; set; }
    }
}