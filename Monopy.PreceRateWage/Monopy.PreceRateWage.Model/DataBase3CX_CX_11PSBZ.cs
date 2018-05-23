using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 破损补助
    /// </summary>
    public class DataBase3CX_CX_11PSBZ
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
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public string HJJE { get; set; }
    }
}