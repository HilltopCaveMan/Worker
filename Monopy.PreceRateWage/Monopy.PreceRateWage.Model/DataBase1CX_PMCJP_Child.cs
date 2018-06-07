using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 精坯月报（行转列）
    /// </summary>
    public class DataBase1CX_PMCJP_Child
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
       
    }
}
