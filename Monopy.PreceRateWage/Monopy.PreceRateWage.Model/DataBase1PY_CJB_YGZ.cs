using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 11手工擦水月工资
    /// </summary>
    public class DataBase1PY_CJB_YGZ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 人员编码
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
        /// 实出勤
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }
    }
}
