using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 泥、釉料出库单-釉浆
    /// </summary>
    public class DataBase2YL_NYLYLJJ_01YJ
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
        /// 釉浆名称
        /// </summary>
        public string YJMC { get; set; }

        /// <summary>
        /// 生产数量(吨)
        /// </summary>
        public string SCSL { get; set; }

        /// <summary>
        /// 材料出库数量(公斤)
        /// </summary>
        public string CKSL { get; set; }

        /// <summary>
        /// 金额（元）
        /// </summary>
        public string JE { get; set; }
    }
}
