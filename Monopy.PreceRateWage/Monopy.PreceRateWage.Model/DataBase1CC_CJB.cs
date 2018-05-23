using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 01仓储车间报导入
    /// </summary>
    public class DataBase1CC_CJB
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
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 计件
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 发货员占比计件
        /// </summary>
        public string FHYZB { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 实出勤
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public virtual ICollection<DataBase1CC_CJB_Child> Childs { get; set; }
    }
}