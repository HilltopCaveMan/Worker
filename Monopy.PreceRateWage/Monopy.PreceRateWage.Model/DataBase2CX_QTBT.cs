using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 其他补贴
    /// </summary>
    public class DataBase2CX_QTBT
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
        /// 补助标准
        /// </summary>
        public string BZBZ { get; set; }

        /// <summary>
        /// 补助天数
        /// </summary>
        public string BZTS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 实出勤天数
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 补助金额
        /// </summary>
        public string BZJE { get; set; }
    }
}
