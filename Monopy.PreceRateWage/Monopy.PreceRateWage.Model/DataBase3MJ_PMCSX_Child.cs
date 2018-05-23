using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// PMC上线明细
    /// </summary>
    public class DataBase3MJ_PMCSX_Child
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string CPMC { get; set; }
        public string Count { get; set; }
        public string YM_Price { get; set; }
        public string DW { get; set; }
        public string Money { get; set; }
    }
}