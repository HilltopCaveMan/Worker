using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 公司奖惩
    /// </summary>
    public class DataBaseGeneral_JC_Factory
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Factory { get; set; }
        public string Detp { get; set; }
        public string Jobs { get; set; }
        public string TheType { get; set; }
        public string TheTime { get; set; }
        public string Content { get; set; }
        public string Money { get; set; }
        public string JCId { get; set; }
    }
}