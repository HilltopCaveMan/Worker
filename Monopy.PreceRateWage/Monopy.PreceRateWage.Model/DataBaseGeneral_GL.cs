using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 工龄
    /// </summary>
    public class DataBaseGeneral_GL
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Factory { get; set; }
        public string Detp { get; set; }
        public string Jobs { get; set; }
        public string Birthday { get; set; }
        public string EntryTime { get; set; }
        public string BeforeMoney { get; set; }
        public string NowMoney { get; set; }
        public string Money { get; set; }
        public string BeforeLeaveDays { get; set; }
    }
}