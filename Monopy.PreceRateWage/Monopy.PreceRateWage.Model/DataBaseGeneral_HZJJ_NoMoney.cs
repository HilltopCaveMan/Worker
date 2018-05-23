using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 互助基金不扣钱
    /// </summary>
    public class DataBaseGeneral_HZJJ_NoMoney
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Dept { get; set; }
        public string LzType { get; set; }
        public string Factory { get; set; }
    }
}