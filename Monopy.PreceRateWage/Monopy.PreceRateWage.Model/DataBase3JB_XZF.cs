using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 检包线长费
    /// </summary>
    public class DataBase3JB_XZF
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Line { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string DayCq { get; set; }
        public string DBValue { get; set; }
        public string DayYcq { get; set; }
        public string Money { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
}