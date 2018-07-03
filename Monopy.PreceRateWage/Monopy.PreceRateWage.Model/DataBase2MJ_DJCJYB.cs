using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 大件车间月报（验证用）
    /// </summary>
    public class DataBase2MJ_DJCJYB
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
        /// 产品名称
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// 上期结存
        /// </summary>
        public string SQJC { get; set; }

        /// <summary>
        /// 当日生产
        /// </summary>
        public string DRSC { get; set; }

        /// <summary>
        /// 生产累计
        /// </summary>
        public string SCLJ { get; set; }

        /// <summary>
        /// 调拨入库
        /// </summary>
        public string DBRK { get; set; }

        /// <summary>
        /// 当日拨出
        /// </summary>
        public string DRBC { get; set; }

        /// <summary>
        /// 拔出累计
        /// </summary>
        public string BCLJ { get; set; }

        /// <summary>
        /// 调拨出库
        /// </summary>
        public string DBCK { get; set; }

        /// <summary>
        /// 当日破损
        /// </summary>
        public string DRPS { get; set; }

        /// <summary>
        /// 破损累计
        /// </summary>
        public string PSLJ { get; set; }

        /// <summary>
        /// 期末结存
        /// </summary>
        public string QMJC { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
