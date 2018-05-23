using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 6交坯表
    /// </summary>
    public class DataBase3CX_GYMJ_06_JPB
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
        public virtual ICollection<DataBase3CX_GYMJ_06_JPB_Child> Childs { get; set; }
    }
}