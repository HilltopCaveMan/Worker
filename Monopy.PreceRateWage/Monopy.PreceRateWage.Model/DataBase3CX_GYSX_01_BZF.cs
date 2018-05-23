using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 高压水箱——班长费
    /// </summary>
    public class DataBase3CX_GYSX_01_BZF
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GH { get; set; }
        public string BYGH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string BZF { get; set; }
    }
}