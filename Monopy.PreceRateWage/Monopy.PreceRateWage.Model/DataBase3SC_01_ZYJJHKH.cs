using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 装窑计件和考核
    /// </summary>
    public class DataBase3SC_01_ZYJJHKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string LX { get; set; }
        public string YearMonth { get; set; }
        public string Factory { get; set; }
        public string Code { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string KYL { get; set; }
        public string YJP { get; set; }
        public string YJV { get; set; }
        public string KH { get; set; }
        public string JJ { get; set; }
        public string HJ { get; set; }
        public string JDRQ { get; set; }
        public string JDR { get; set; }
        public string LX_Remark { get; set; }
    }
}