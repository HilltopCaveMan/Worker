using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 技术部实验补助
    /// </summary>
    public class DataBase2PY_JSBZ
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
        /// 车间
        /// </summary>
        public string CJ { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string GW { get; set; }

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
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public string JS { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public string ZB { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>
        public string JE { get; set; }
    }
}
