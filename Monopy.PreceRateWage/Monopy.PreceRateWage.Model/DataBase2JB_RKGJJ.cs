using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 4入库工计件PMC
    /// </summary>
    public class DataBase2JB_RKGJJ
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
        /// 金额
        /// </summary>
        public string JE { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public virtual ICollection<DataBase2JB_RKGJJ_Child> Childs { get; set; }
    }
}