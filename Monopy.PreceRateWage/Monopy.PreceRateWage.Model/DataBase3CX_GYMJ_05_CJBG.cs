using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 5车间报工导入
    /// </summary>
    public class DataBase3CX_GYMJ_05_CJBG
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GH { get; set; }
        public string CPMC { get; set; }
        public string ZJCS1 { get; set; }
        public string ZJCS2 { get; set; }
        public string ZJCS3 { get; set; }
        public string ZJCS4 { get; set; }
        public string MXS1 { get; set; }
        public string MXS2 { get; set; }
        public string MXS3 { get; set; }
        public string MXS4 { get; set; }
        public string YYJWJP { get; set; }
    }
}