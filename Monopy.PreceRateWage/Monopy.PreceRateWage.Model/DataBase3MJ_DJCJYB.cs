using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 大件车间月报（验证用）
    /// </summary>
    public class DataBase3MJ_DJCJYB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string CPMC { get; set; }
        public string DW { get; set; }
        public string SQJC { get; set; }
        public string SCLJ { get; set; }
        public string BCLJ { get; set; }
        public string PSLJ { get; set; }
        public string QMJC { get; set; }
        public string Remark { get; set; }
    }
}