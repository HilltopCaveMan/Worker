using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 半检拉坯——拉坯月报
    /// </summary>
    public class DataBase1CX_BJLP_01_LPYB
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
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 大裂
        /// </summary>
        public string DL { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }

        public virtual ICollection<DataBase1CX_BJLP_01_LPYB_Child> Childs { get; set; }

        //public string BPD { get; set; }
        //public string DTL { get; set; }
        //public string DQ { get; set; }
        //public string G { get; set; }
        //public string LTL { get; set; }
        //public string PL { get; set; }
        //public string SXL { get; set; }
        //public string XK { get; set; }
        //public string ZL { get; set; }
    }
}