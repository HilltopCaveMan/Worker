using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// PMC上线
    /// </summary>
    public class DataBase3MJ_PMCSX
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
        public virtual ICollection<DataBase3MJ_PMCSX_Child> Childs { get; set; }
    }
}