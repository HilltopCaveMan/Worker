using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 车间考核
    /// </summary>
    public class DataBase3MJ_CJKH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string GZ { get; set; }
        public string KHNR { get; set; }
        public string JFMX { get; set; }
        public string KHDF { get; set; }
        public string KHJE { get; set; }
        public string Money { get; set; }

        public override string ToString()
        {
            return UserCode + ":" + UserName + ",减分明细=" + JFMX + ",考核得分=" + KHDF;
        }
    }
}