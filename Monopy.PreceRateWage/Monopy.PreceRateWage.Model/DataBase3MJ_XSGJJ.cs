using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 卸石膏计件
    /// </summary>
    public class DataBase3MJ_XSGJJ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string PZ { get; set; }
        public string RQ { get; set; }
        public string DS { get; set; }
        public string DJ { get; set; }
        public string Money { get; set; }
    }
}