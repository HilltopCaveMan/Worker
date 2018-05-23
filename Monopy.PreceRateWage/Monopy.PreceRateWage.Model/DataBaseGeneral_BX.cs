using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 保险
    /// </summary>
    public class DataBaseGeneral_BX
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Factory { get; set; }
        public string Detp { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string BX { get; set; }
        public string GJJ { get; set; }
        public string BX_DW { get; set; }
        public string GJJ_DW { get; set; }
    }
}