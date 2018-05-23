using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 入职补助(月)
    /// </summary>
    public class DataBaseGeneral_RZBZ_Month
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

        /// <summary>
        /// 第几个月
        /// </summary>
        public string DJGY { get; set; }

        /// <summary>
        /// 补助天数
        /// </summary>
        public string BZTS { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQTS { get; set; }

        /// <summary>
        /// 学徒月工资
        /// </summary>
        public string XTYGZ { get; set; }

        public string Money { get; set; }
    }
}