using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 开窑报工
    /// </summary>
    public class DataBase3SC_05_KYBG
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GD { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string GRKYL { get; set; }
        public string CQ { get; set; }
        public string DJ { get; set; }
        public string JE { get; set; }
        public string KH { get; set; }
    }
}