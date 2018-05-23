using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 一厂学徒台账
    /// </summary>
    public class DataBase1CC_XTTZ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 补助时间
        /// </summary>
        public DateTime TimeBZ { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string CJ { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }
    }
}
