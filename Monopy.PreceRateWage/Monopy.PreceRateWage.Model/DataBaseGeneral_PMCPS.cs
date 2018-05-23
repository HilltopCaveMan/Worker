using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// PMC破损（多个车间共用）
    /// </summary>
    public class DataBaseGeneral_PMCPS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Dept { get; set; }
        public string No { get; set; }
        public string GH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Lt_Sl { get; set; }
        public string Lt_Dj { get; set; }
        public string Dt_Sl { get; set; }
        public string Dt_Dj { get; set; }
        public string Xj_Sl { get; set; }
        public string Xj_Dj { get; set; }
        public string G_Sl { get; set; }
        public string G_Dj { get; set; }
        public string Money { get; set; }
    }
}