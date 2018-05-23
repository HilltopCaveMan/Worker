using System;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3CX_General_JB
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 高压面具;高压水箱
        /// </summary>
        public string TheType { get; set; }

        public string No { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string QGGWLB { get; set; }
        public string QGGWMC { get; set; }
        public string JBMQTS { get; set; }
        public string JBTS { get; set; }
        public string JBGZ { get; set; }
        public string Money { get; set; }
    }
}