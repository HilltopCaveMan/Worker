using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 模具日工提报
    /// </summary>
    public class DataBase1MJ_RGTB
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
        /// 岗位
        /// </summary>
        public string  GW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string RQ { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public string TS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string  BZ { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 日工合计
        /// </summary>
        public string RGHJ { get; set; }
    }
}
