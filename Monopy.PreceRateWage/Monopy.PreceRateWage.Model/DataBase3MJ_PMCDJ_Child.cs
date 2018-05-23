using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 日PMC大件明细(子表行转列)
    /// </summary>
    public class DataBase3MJ_PMCDJ_Child
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string CPMC { get; set; }
        public string Count { get; set; }
        public string ZM_Price { get; set; }
        public string XM_Price { get; set; }
        public string DW { get; set; }
        public string Money { get; set; }
    }
}