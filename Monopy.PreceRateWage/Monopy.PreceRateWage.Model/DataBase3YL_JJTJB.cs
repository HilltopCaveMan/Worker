using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 原料计件统计表
    /// </summary>
    public class DataBase3YL_JJTJB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GW { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string JJGZ { get; set; }
        public string JYG { get; set; }
        public string QYG { get; set; }
        public string IsBZ { get; set; }
        public string BZF { get; set; }
        public bool IsQQJ { get; set; }
        public string QQJ { get; set; }
    }
}