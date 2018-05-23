using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_MCLBJJ
    {
        [Key]
        public Guid ID { get; set; }

        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public bool IsOkMc1 { get; set; }
        public bool IsOkMc2 { get; set; }
        public bool IsOkYkcpg { get; set; }
        public bool IsOkLb1 { get; set; }
        public bool IsOkLb2 { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string PZ { get; set; }
        public string McCount { get; set; }
        public string YkcpgCount { get; set; }
        public string LbCount { get; set; }
        public string McUnitPrice { get; set; }
        public string YkcpgUnitPrice { get; set; }
        public string LbUnitPrice { get; set; }
        public string McMoney { get; set; }
        public string YkcpgMoney { get; set; }
        public string LbMoney { get; set; }
        public string Money { get; set; }
    }
}