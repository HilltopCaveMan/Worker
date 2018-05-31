using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 磨瓷原库存月报PMC
    /// </summary>
    public class DataBase1JB_PMCMCYKC
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
        /// 类别名称
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string CHBM { get; set; }

        /// <summary>
        /// 存货名称
        /// </summary>
        public string CHMC { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        public string LBBM { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 合格
        /// </summary>
        public string HG { get; set; }

        /// <summary>
        /// 回磨
        /// </summary>
        public string HM { get; set; }

        /// <summary>
        /// 磨等外
        /// </summary>
        public string MDW { get; set; }

        /// <summary>
        /// 其它
        /// </summary>
        public string QT { get; set; }

        /// <summary>
        /// 磨残
        /// </summary>
        public string MC { get; set; }

        /// <summary>
        /// 风惊
        /// </summary>
        public string FJ { get; set; }

        /// <summary>
        /// 超标
        /// </summary>
        public string CB { get; set; }

        /// <summary>
        /// 磨修
        /// </summary>
        public string MX { get; set; }

        /// <summary>
        /// 原修
        /// </summary>
        public string YX { get; set; }

        /// <summary>
        /// 漏检
        /// </summary>
        public string LJ { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public string HGL { get; set; }

        /// <summary>
        /// 最终合格率
        /// </summary>
        public string ZZHGL { get; set; }
    }
}
