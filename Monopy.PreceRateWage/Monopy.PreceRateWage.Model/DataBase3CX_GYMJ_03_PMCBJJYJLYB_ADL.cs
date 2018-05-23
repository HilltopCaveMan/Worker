using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 3PMC半检检验记录月报（按大类）(作废)
    /// </summary>
    public class DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GH { get; set; }
        public string LBBM { get; set; }
        public string LBMC { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string GDBM { get; set; }
        public string GDMC { get; set; }
        public string KYL { get; set; }
        public string YJP { get; set; }
        public string JJ { get; set; }
        public string PS { get; set; }
    }
}