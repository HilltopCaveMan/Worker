using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 8贴标实际缺陷数
    /// </summary>
    public class DataBase3SC_08_TBSJQXS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 缺陷
        /// </summary>
        public string QX { get; set; }
    }
}