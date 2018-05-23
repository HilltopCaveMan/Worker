using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 离职扣款
    /// </summary>
    public class DataBaseGeneral_LZ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Factory { get; set; }
        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Detp { get; set; }
        public string Crew { get; set; }
        public string Jobs { get; set; }
        public string LZType { get; set; }
        public string GZF { get; set; }
        public string KQK { get; set; }
        public string GJ { get; set; }
        public string PXF { get; set; }
        public string Money { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Remark { get; set; }
        public string Confirm { get; set; }
    }
}