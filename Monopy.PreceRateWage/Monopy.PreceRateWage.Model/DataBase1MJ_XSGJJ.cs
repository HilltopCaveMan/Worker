using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 卸石膏计件
    /// </summary>
    public class DataBase1MJ_XSGJJ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string RQ { get; set; }

        /// <summary>
        /// 吨数
        /// </summary>
        public string DS { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }
    }
}
