using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 原料考核表（加釉工）
    /// </summary>
    public class DataBase3YL_JYGKHB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 考核项
        /// </summary>
        public string KHX { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public string FZ { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public string DF { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }

        /// <summary>
        /// 考核总额
        /// </summary>
        public string KHZE { get; set; }
    }
}