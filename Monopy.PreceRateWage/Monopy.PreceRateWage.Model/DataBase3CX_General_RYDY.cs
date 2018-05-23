using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 人员对应
    /// </summary>
    public class DataBase3CX_General_RYDY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 高压面具;高压水箱
        /// </summary>
        public string TheType { get; set; }

        public string GH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 出勤天数新增
        /// </summary>
        public string CQTS { get; set; }

        /// <summary>
        /// 满勤天数新增
        /// </summary>
        public string MQTS { get; set; }

        /// <summary>
        /// 金额新增
        /// </summary>
        public string Money { get; set; }
    }
}