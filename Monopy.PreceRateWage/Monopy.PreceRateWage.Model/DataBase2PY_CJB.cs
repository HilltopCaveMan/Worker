using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 1车间报工
    /// </summary>
    public class DataBase2PY_CJB
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
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 出勤天数
        /// </summary>
        public string CQTS { get; set; }

        /// <summary>
        /// 学徒天数
        /// </summary>
        public string XTTS { get; set; }

        /// <summary>
        /// 喷釉工（手工）考核
        /// </summary>
        public string PYGSGKH { get; set; }

        /// <summary>
        /// 喷釉工（手工）计件
        /// </summary>
        public string PYGSGJJ { get; set; }

        /// <summary>
        /// 喷釉工（机械）考核
        /// </summary>
        public string PYGJXKH { get; set; }

        /// <summary>
        /// 喷釉工（机械）计件
        /// </summary>
        public string PYGJXJJ { get; set; }

        /// <summary>
        /// 擦水工（手工）考核
        /// </summary>
        public string CSGSGKH { get; set; }

        /// <summary>
        /// 擦水工（手工）计件
        /// </summary>
        public string CSGSGJJ { get; set; }

        /// <summary>
        /// 擦水工（机械）考核
        /// </summary>
        public string CSGJJKH { get; set; }

        /// <summary>
        /// 擦水工（机械）计件
        /// </summary>
        public string CSGJXJJ { get; set; }

    }
}
