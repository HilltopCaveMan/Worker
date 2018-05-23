using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    ///加班审批
    /// </summary>
    public class DataBaseGeneral_JBSPB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Dept { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 缺岗岗位类别
        /// </summary>
        public string QGGWLB { get; set; }

        /// <summary>
        /// 缺岗岗位名称
        /// </summary>
        public string QGGWMC { get; set; }

        /// <summary>
        /// 缺岗岗位满勤天数
        /// </summary>
        public string QGGWMQTS { get; set; }

        /// <summary>
        /// 加班天数
        /// </summary>
        public string JBTS { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string JBGZ { get; set; }

        public string Money { get; set; }
    }
}