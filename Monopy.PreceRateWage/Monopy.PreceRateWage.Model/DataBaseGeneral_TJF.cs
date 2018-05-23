using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 体检费
    /// </summary>
    public class DataBaseGeneral_TJF
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Factory { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Detp { get; set; }
        public string Crew { get; set; }
        public string Jobs { get; set; }
        public string InTime { get; set; }
        public string WorkBeforeOrAfter { get; set; }
        public string Money { get; set; }
        public string Remark { get; set; }
    }
}