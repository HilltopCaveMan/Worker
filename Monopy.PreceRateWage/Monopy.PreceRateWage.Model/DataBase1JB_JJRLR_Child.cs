using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase1JB_JJRLR_Child
    {
        public Guid Id { get; set; }
        public int No { get; set; }

        /// <summary>
        /// 类别名称
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
    }
}
