using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 7吊链地摊个别导入
    /// </summary>
    public class DataBase2CX_CX_07DLDT
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
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }
    }
}
