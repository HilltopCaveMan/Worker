using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 11四厂学徒
    /// </summary>
    public class DataBase2MJ_SCXTDay
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
        /// 补助天数
        /// </summary>
        public string BZTS { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQTS { get; set; }

        /// <summary>
        /// 学徒标准
        /// </summary>
        public string XTBZ { get; set; }

        /// <summary>
        /// 学徒月工资
        /// </summary>
        public string XTYGZ { get; set; }

        /// <summary>
        /// 实出勤
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 补助金额
        /// </summary>
        public string BZJE { get; set; }

        /// <summary>
        /// 累计已得学徒天数
        /// </summary>
        public string LJXTTS { get; set; }
    }
}
