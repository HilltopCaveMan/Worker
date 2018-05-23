using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 三段装窑
    /// </summary>
    public class DataBase3SC_04_3DZY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GZ { get; set; }
        public string GD { get; set; }
        public string GH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string HSTS { get; set; }
        public string HSGZ { get; set; }
    }
}