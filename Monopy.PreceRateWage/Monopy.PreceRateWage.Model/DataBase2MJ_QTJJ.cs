using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 9其他计件
    /// </summary>
    public class DataBase2MJ_QTJJ
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
        /// 岗位
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        public string PZMC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string SL { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 金额合计
        /// </summary>
        public string JEHJ { get; set; }
    }
}
