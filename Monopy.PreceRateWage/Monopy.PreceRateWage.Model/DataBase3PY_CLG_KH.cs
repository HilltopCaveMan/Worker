using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 擦绺工个人考核
    /// </summary>
    public class DataBase3PY_CLG_KH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 工号（短工号）
        /// </summary>
        public string GH { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public string SJWC { get; set; }

        /// <summary>
        /// 指标
        /// </summary>
        public string ZB { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }
    }
}