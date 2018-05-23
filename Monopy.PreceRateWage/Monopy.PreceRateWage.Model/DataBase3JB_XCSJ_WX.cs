using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 叉车司机外协
    /// </summary>
    public class DataBase3JB_XCSJ_WX
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
        public string MqDays { get; set; }
        public string BaseSalary { get; set; }
        private string money;

        public string Money
        {
            get
            {
                return string.IsNullOrEmpty(money) ? string.Empty : Convert.ToDecimal(money).ToString("0.0000");
            }
            set
            {
                money = value;
            }
        }
    }
}