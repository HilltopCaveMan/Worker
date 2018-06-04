using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase1SC_HSY_BCKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 修补不合格数
        /// </summary>
        public string XBBHGS { get; set; }

        /// <summary>
        /// 修补率
        /// </summary>
        public string XBL { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string MB { get; set; }

        /// <summary>
        /// 考核单价
        /// </summary>
        public string KHDJ { get; set; }

        /// <summary>
        /// 奖罚
        /// </summary>
        public string JF { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string  KHJJ { get; set; }
    }
}
