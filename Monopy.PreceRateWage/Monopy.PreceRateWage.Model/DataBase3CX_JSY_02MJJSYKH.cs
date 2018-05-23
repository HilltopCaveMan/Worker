using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 面具技术员考核
    /// </summary>
    public class DataBase3CX_JSY_02MJJSYKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        public string PZ { get; set; }
        public string GZHGLMB { get; set; }
        public string GZHGLSJ { get; set; }
        public string MBQXL { get; set; }
        public string SJQXL { get; set; }
        public string GZKHJE { get; set; }
        public string QXKHJE { get; set; }
        public string Money { get; set; }
    }
}