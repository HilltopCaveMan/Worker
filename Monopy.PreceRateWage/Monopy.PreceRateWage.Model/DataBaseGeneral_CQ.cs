using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 出勤
    /// </summary>
    public class DataBaseGeneral_CQ
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
        public string Dept { get; set; }
        public string Position { get; set; }
        public string DayYcq { get; set; }
        public string DayScq { get; set; }
        public string DayDktx { get; set; }
        public string DayDx { get; set; }
        public string DayJbjx { get; set; }
        public string DayTxj { get; set; }
        public string CountCdzt { get; set; }
        public string DayKg { get; set; }
        public string DaySj { get; set; }
        public string DayBj { get; set; }
        public string DayTotal { get; set; }
        public string Remark { get; set; }
    }
}