using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 原料计件核算（计件明细）
    /// </summary>
    public class DataBase3YL_JJHS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
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
        /// 数量
        /// </summary>
        public string SL { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 计件工资
        /// </summary>
        public string JJGZ { get; set; }
    }
}