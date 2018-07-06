using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 1包装计件
    /// </summary>
    public class DataBase2JB_BZJJ
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
        /// 岗位名称
        /// </summary>
        public string GWMC { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string SL { get; set; }

        /// <summary>
        /// 返包数量
        /// </summary>
        public string FBSL { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 对应品种
        /// </summary>
        public string DYPZ { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>

        public string JJJE { get; set; }
    }
}
