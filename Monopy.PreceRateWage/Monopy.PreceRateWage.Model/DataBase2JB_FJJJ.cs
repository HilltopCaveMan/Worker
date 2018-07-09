using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 20复检计件
    /// </summary>
    public class DataBase2JB_FJJJ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        public string No { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string  LB { get; set; }

        /// <summary>
        /// 品种对应类别
        /// </summary>
        public string PZDYLB { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string  SL { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>
        public string JJJE { get; set; }

        /// <summary>
        /// PG审核状态ture：通过；false：退回
        /// </summary>
        public bool IsPG_Check { get; set; }

        /// <summary>
        /// 品管审核时间
        /// </summary>
        public DateTime? PG_Time { get; set; }

        /// <summary>
        /// 品管审核人
        /// </summary>
        public string PG_User { get; set; }
    }
}