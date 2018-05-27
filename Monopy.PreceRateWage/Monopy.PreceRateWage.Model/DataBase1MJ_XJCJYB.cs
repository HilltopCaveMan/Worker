using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{

    /// <summary>
    /// 小件车间月表（验证用）
    /// </summary>
    public class DataBase1MJ_XJCJYB
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
        ///名称
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public string SCSL { get; set; }
    }
}
