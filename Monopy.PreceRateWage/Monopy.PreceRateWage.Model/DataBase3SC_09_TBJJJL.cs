using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 9贴标检验记录
    /// </summary>
    public class DataBase3SC_09_TBJJJL
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 存货名称
        /// </summary>
        public string CHMC { get; set; }

        /// <summary>
        /// 标污
        /// </summary>
        public string BW { get; set; }

        /// <summary>
        /// 倒重漏错
        /// </summary>
        public string DZLC { get; set; }
    }
}