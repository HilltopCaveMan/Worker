using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 9磨瓷计件
    /// </summary>
    public class DataBase2JB_MCJJ
    {
        [Key]
        public Guid ID { get; set; }

        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }


        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 人员编码
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
        /// 磨瓷数量
        /// </summary>
        public string McCount { get; set; }

        /// <summary>
        /// 原库存磨瓷数量
        /// </summary>
        public string YkcpgCount { get; set; }

        /// <summary>
        /// 磨瓷单价
        /// </summary>
        public string McUnitPrice { get; set; }

        /// <summary>
        /// 原库存磨瓷单价
        /// </summary>
        public string YkcpgUnitPrice { get; set; }

        /// <summary>
        /// 磨瓷金额
        /// </summary>
        public string McMoney { get; set; }

        /// <summary>
        /// 原库存金额
        /// </summary>
        public string YkcpgMoney { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }
    }
}