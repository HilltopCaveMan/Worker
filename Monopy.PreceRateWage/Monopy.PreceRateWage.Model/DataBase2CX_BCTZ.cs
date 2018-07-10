using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 15变产台账
    /// </summary>
    public class DataBase2CX_BCTZ
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
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 原产品名称
        /// </summary>
        public string YCPMC { get; set; }

        /// <summary>
        /// 变更产品名称
        /// </summary>
        public string BGCPMC { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 变产时间
        /// </summary>
        public string BCSJ { get; set; }

        /// <summary>
        /// 补助方案
        /// </summary>
        public string BZFA { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }
    }
}
