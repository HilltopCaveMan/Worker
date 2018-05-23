using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 交通补助
    /// </summary>
    public class DataBaseGeneral_JT
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Dep { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string InTime { get; set; }
        public string Money { get; set; }
    }
}