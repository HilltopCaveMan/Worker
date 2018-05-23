using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 班组考核结果
    /// </summary>
    public class DataBase3PY_BZ_KH_Sum
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }
    }
}