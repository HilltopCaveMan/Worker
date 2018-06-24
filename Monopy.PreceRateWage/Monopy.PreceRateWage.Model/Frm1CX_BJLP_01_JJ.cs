using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 计件
    /// </summary>
    public class DataBase1CX_BJLP_01_JJ
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
        /// 员工编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string ZW { get; set; }

        /// <summary>
        /// 应出勤（天数）
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 实出勤（天数）
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>
        public string JJJE { get; set; }
    }
}