using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 车票
    /// </summary>
    public class DataBaseGeneral_CP
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
        public string Address { get; set; }
        public string MoveLine { get; set; }
        public string MoveTime { get; set; }
        public string Money { get; set; }
        public string Audit { get; set; }
        public string Remark { get; set; }
    }
}