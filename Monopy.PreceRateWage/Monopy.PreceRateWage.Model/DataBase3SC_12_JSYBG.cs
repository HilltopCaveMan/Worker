using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 技术员报工
    /// </summary>
    public class DataBase3SC_12_JSYBG
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GD { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string KYL { get; set; }
        public string ZYQXS { get; set; }
        public string BWQX { get; set; }
        public string KYQX { get; set; }
        public string QXL { get; set; }
        public string MBQXL { get; set; }
        public string JE { get; set; }
    }
}