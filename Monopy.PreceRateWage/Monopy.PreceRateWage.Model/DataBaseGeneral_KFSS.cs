using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 开发试烧
    /// </summary>
    public class DataBaseGeneral_KFSS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string No { get; set; }
        public string Dept { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string LB { get; set; }
        public string SL { get; set; }
        public string Money { get; set; }
    }
}