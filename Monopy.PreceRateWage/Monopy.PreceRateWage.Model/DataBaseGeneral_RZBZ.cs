using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 入职补助(日)
    /// </summary>
    public class DataBaseGeneral_RZBZ
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Dept { get; set; }
        public string No { get; set; }
        public string GZ { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Day_BZ { get; set; }
        public string XTRG { get; set; }
        public string Money { get; set; }
    }
}