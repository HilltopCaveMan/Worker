using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 成型计件--个人
    /// </summary>
    public class DataBase3CX_CX_03JJ_GR
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
        /// 班组编码
        /// </summary>
        public string BZBM { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string BZMC { get; set; }

        /// <summary>
        /// 员工（工号）
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 工资总额
        /// </summary>
        public string SumMoney { get; set; }

        /// <summary>
        /// 个人金额
        /// </summary>
        public string Money { get; set; }
    }
}