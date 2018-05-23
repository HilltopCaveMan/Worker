using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 雇主责任险
    /// </summary>
    public class DataBaseGeneral_GZZRX
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }
        public string Factory { get; set; }
        public string Detp { get; set; }
        public string Crew { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Number { get; set; }

        /// <summary>
        /// 个人返还+
        /// </summary>
        public string MoneyPlus { get; set; }

        /// <summary>
        /// 个人担负-
        /// </summary>
        public string MoneyMinus { get; set; }

        public string Remark { get; set; }
    }
}