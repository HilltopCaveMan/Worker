using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 入库工计件（行转列）
    /// </summary>
    public class DataBase2JB_RKGJJ_Child
    {
        public Guid Id { get; set; }
        public int No { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; set; }
        //public string XM_Price { get; set; }
        //public string DW { get; set; }
        //public string Money { get; set; }
    }
}
