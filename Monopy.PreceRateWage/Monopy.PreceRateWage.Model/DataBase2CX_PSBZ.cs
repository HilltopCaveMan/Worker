using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 5破损补助
    /// </summary>
    public class DataBase2CX_PSBZ
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
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string SL { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public string ZB { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }

        /// <summary>
        /// 人员编码
        /// </summary>
        public string UserCode { get; set; }
    }
}
