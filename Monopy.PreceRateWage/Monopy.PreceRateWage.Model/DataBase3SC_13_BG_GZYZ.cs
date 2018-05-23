using System;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3SC_13_BG_GZYZ
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
        /// 基本工资额
        /// </summary>
        public string JBGZE { get; set; }

        /// <summary>
        /// 加班金额
        /// </summary>
        public string JBJE { get; set; }

        /// <summary>
        /// 实际工资(基本工资额+加班金额)
        /// </summary>
        public string SJGZ { get; set; }

        /// <summary>
        /// 定员
        /// </summary>
        public string DY { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string JBGZ { get; set; }

        /// <summary>
        /// 定员工资
        /// </summary>
        public string DYGZ { get; set; }

        /// <summary>
        /// 差异（定员工资-实际工资）
        /// </summary>
        public string CY { get; set; }
    }
}