using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 辅助验货（检包除外）
    /// </summary>
    public class DataBaseGeneral_FZYH
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Dept { get; set; }
        public string No { get; set; }
        public string GWMC { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string DayCount { get; set; }
        public string JG { get; set; }
        public string Money { get; set; }
    }
}