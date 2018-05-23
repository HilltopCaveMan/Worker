using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// PMC小件
    /// </summary>
    public class DataBase3MJ_PMCXJ
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
        public string RQ { get; set; }
        public virtual ICollection<DataBase3MJ_PMCXJ_Child> Childs { get; set; }
    }
}