using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 学徒补助（日）
    /// </summary>
    public class DataBaseGeneral_XTDay
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryNo { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string CJ { get; set; }

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
        /// 学徒日工资
        /// </summary>
        public string XTRGZ { get; set; }

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
        /// 学徒总额
        /// </summary>
        public string XTZE { get; set; }

        /// <summary>
        /// 上线金额
        /// </summary>
        public string SXJE { get; set; }

        /// <summary>
        /// 核对
        /// </summary>
        public string HD { get; set; }
    }
}
