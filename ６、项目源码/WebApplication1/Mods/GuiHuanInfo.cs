using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mods
{
    /// <summary>
    /// 归还信息实体类
    /// </summary>
    public class GuiHuanInfo
    {
        string _jieChuZiChanID;
        int _guiHuanShuLiang;
        string _guiHuanGongSi;
        DateTime _guiHuanRiQi;

        /// <summary>
        /// 借出资产ID
        /// </summary>
        public string JieChuZiChanID { get => _jieChuZiChanID; set => _jieChuZiChanID = value; }

        /// <summary>
        /// 归还数量
        /// </summary>
        public int GuiHuanShuLiang { get => _guiHuanShuLiang; set => _guiHuanShuLiang = value; }

        /// <summary>
        /// 归还公司
        /// </summary>
        public string GuiHuanGongSi { get => _guiHuanGongSi; set => _guiHuanGongSi = value; }

        /// <summary>
        /// 归还日期
        /// </summary>
        public DateTime GuiHuanRiQi { get => _guiHuanRiQi; set => _guiHuanRiQi = value; }
    }
}