using System;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_FZYH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Gwmc { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string SlgDays { get; set; }
        public string Gander { get; set; }
        public string Price { get; set; }
        public bool IsXcOrWx { get; set; }
        public string Money { get; set; }
    }
}