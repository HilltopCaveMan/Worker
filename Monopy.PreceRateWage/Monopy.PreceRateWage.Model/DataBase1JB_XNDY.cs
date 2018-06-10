using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 线内定员日验证
    /// </summary>
    public class DataBase1JB_XNDY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 编制
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 实际编制
        /// </summary>
        public string SJBZ { get; set; }

        /// <summary>
        /// 对数
        /// </summary>
        public string DS { get; set; }
    }
}
