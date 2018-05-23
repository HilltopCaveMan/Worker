using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 开窑工考核和计件
    /// </summary>
    public class DataBase3SC_0203_ZY_HS_KYGKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string KYL { get; set; }
        public string ZYYBDKYQXL { get; set; }
        public string SJYJL { get; set; }
        public string MBYJL { get; set; }
        public string KYGDY { get; set; }
        public string KHJE { get; set; }
    }
}