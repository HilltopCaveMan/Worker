using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 技术员考核
    /// </summary>
    public class DataBase3SC_11_JSYKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string KYL { get; set; }
        public string YJP { get; set; }
        public string QXS { get; set; }
        public string BWQXS { get; set; }
        public string KYQXS { get; set; }
    }
}