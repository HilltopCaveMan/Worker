using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 报工核定员
    /// </summary>
    public class DataBase3SC_13_BG_DY
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
        /// 实际人数
        /// </summary>
        public string SJRS { get; set; }

        /// <summary>
        /// 定员
        /// </summary>
        public string DY { get; set; }

        /// <summary>
        /// 定员-实际人数
        /// </summary>
        public string Dy_Sjrs { get; set; }
    }
}