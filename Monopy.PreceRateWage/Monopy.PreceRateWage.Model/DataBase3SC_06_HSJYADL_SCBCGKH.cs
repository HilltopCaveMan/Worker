using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 烧成补瓷工考核
    /// </summary>
    public class DataBase3SC_06_HSJYADL_SCBCGKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string HSYKYL { get; set; }
        public string DJ { get; set; }
        public string JE1 { get; set; }
        public string YJP { get; set; }
        public string SJQXL { get; set; }
        public string ZB { get; set; }
        public string JE2 { get; set; }
    }
}