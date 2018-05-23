using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 补发工资
    /// </summary>
    public class DataBaseGeneral_BF
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Factory { get; set; }
        public string Dep { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Money { get; set; }
    }
}