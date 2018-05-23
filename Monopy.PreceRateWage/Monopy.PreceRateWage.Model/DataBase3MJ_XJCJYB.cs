using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 小件车间月表（验证用）
    /// </summary>
    public class DataBase3MJ_XJCJYB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string CPMC { get; set; }
        public string DW { get; set; }
        public string SCSL { get; set; }
    }
}