using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 幼儿园补助
    /// </summary>
    public class DataBaseGeneral_YEY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Dep { get; set; }
        public string FUserCode { get; set; }
        public string FUserName { get; set; }
        public string FInTime { get; set; }
        public string MUserCode { get; set; }
        public string MUserName { get; set; }
        public string MInTime { get; set; }
        public string ChildName { get; set; }
        public string Teacher { get; set; }
        public string Money { get; set; }
        public string Kindergarten { get; set; }
    }
}