using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 部门奖惩
    /// </summary>
    public class DataBaseGeneral_JC_Dept
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Dept { get; set; }

        /// <summary>
        /// 类别：车间/成检/品管
        /// </summary>
        public string TheType { get; set; }

        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string J { get; set; }
        public string C { get; set; }
        public string Reason { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
}