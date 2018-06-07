using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 开发试烧
    /// </summary>
    public class DataBase1JB_KFSS
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
        public string Pz { get; set; }

        /// <summary>
        /// 磨瓷数量
        /// </summary>
        public string McCount { get; set; }

        /// <summary>
        /// 冷补数量
        /// </summary>
        public string LbCount { get; set; }

        /// <summary>
        /// 磨瓷
        /// </summary>
        public string McUP { get; set; }

        /// <summary>
        /// 冷补
        /// </summary>
        public string LbUP { get; set; }

        /// <summary>
        /// 磨瓷金额
        /// </summary>
        public string McMoney { get; set; }

        /// <summary>
        /// 冷补金额
        /// </summary>
        public string LbMoney { get; set; }

        /// <summary>
        /// 金额
        /// </summary>

        public string Money
        {
            get
            {
                return ((string.IsNullOrEmpty(McMoney) ? 0M : Convert.ToDecimal(McMoney)) + (string.IsNullOrEmpty(LbMoney) ? 0M : Convert.ToDecimal(LbMoney))).ToString();
            }
        }
    }
}
