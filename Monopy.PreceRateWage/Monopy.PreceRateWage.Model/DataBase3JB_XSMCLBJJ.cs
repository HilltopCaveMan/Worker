using System;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_XSMCLBJJ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Pz { get; set; }
        public string McCount { get; set; }
        public string LbCount { get; set; }
        public string McUP { get; set; }
        public string LbUP { get; set; }
        public string McMoney { get; set; }
        public string LbMoney { get; set; }

        public string Money
        {
            get
            {
                return ((string.IsNullOrEmpty(McMoney) ? 0M : Convert.ToDecimal(McMoney)) + (string.IsNullOrEmpty(LbMoney) ? 0M : Convert.ToDecimal(LbMoney))).ToString();
            }
        }
    }
}