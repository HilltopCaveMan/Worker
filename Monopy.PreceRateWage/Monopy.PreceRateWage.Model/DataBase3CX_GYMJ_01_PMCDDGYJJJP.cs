using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 1PMC调度高压计件交坯
    /// </summary>
    public class DataBase3CX_GYMJ_01_PMCDDGYJJJP
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
        /// 计划交坯数
        /// </summary>
        public string JHJPS { get; set; }
    }
}