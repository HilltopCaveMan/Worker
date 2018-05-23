using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 喷釉破损（车间）
    /// </summary>
    public class DataBase3PY_PS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Money { get; set; }
    }
}