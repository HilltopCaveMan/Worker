using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 成型技术员考核
    /// </summary>
    public class DataBase3CX_JSY_01CXJSYKH_02KH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string GD { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string GDZTZL_MB { get; set; }
        public string GDZTZL_SJWC { get; set; }
        public string GDCSZL_MB { get; set; }
        public string GDCSZL_SJWC { get; set; }
        public string GDGDCL_MB { get; set; }
        public string GDGDCL_SJWC { get; set; }

        public string GDZTZL_CL { get; set; }
        public string GDZTZL_KHJE { get; set; }
        public string GDCSZL_CL { get; set; }
        public string GDCSZL_KHJE { get; set; }
        public string GDGDCL_CL { get; set; }
        public string GDGDCL_KHJE { get; set; }
        public string HJ { get; set; }
    }
}