using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 卸车原料统计
    /// </summary>
    public class DataBase2YL_XCYLTJ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string SL { get; set; }
    }
}
