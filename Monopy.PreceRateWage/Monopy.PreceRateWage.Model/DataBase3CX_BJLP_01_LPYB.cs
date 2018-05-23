using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 半检拉坯——拉坯月报
    /// </summary>
    public class DataBase3CX_BJLP_01_LPYB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Factory { get; set; }
        public string GH { get; set; }
        public string GD { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string HJ { get; set; }
        public string DL { get; set; }
        public string JE { get; set; }

        public virtual ICollection<DataBase3CX_BJLP_01_LPYB_Child> Childs { get; set; }

        //public string BPD { get; set; }
        //public string DTL { get; set; }
        //public string DQ { get; set; }
        //public string G { get; set; }
        //public string LTL { get; set; }
        //public string PL { get; set; }
        //public string SXL { get; set; }
        //public string XK { get; set; }
        //public string ZL { get; set; }
    }
}