using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 差手质量
    /// </summary>
    public class DataBase3CX_JSY_01CXJSYKH_01CSZL
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string GH { get; set; }
        public string SYKYL { get; set; }
        public string BYKYL { get; set; }
        public string SYYJP { get; set; }
        public string BYYJP { get; set; }
        public string MBZ { get; set; }
        public string SJWC { get; set; }
    }
}