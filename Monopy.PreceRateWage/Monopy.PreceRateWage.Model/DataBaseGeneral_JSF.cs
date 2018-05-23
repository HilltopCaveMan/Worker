using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 招工介绍费
    /// </summary>
    public class DataBaseGeneral_JSF
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Factory { get; set; }
        public string No { get; set; }
        public string Bjs_UserCode { get; set; }
        public string Bjs_UserName { get; set; }
        public string Bjs_Cz { get; set; }
        public string Bjs_Bz { get; set; }
        public string Bjs_InTime { get; set; }
        public string Js_UserName { get; set; }
        public string Js_UserCode { get; set; }
        public string Js_Bm { get; set; }
        public string Js_Bz { get; set; }
        public string Js_InTime { get; set; }
        public string Jsf { get; set; }
        public string Jsf_Times { get; set; }
        public string Remark { get; set; }
    }
}