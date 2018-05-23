using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 气站安全员考核表
    /// </summary>
    public class DataBase3SC_07_QZAQY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string KHX { get; set; }
        public string FZBZ { get; set; }
        public string KHBZ { get; set; }
        public string KHDF { get; set; }
    }
}