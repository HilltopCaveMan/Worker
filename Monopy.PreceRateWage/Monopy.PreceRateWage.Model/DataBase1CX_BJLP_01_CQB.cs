using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 车间出勤表
    /// </summary>
    public class DataBase1CX_BJLP_01_CQB
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
        /// 员工编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string BMMC { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string ZW { get; set; }

        /// <summary>
        /// 应出勤（天数）
        /// </summary>
        public string YCQ { get; set; }

        /// <summary>
        /// 实出勤（天数）
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 本月抵扣调休天数
        /// </summary>
        public string BYDK { get; set; }

        /// <summary>
        /// 带薪假天数
        /// </summary>
        public string DXJ { get; set; }

        /// <summary>
        /// 加班计薪天数
        /// </summary>
        public string JBJX { get; set; }

        /// <summary>
        /// 弹性假天数
        /// </summary>
        public string TXJ { get; set; }

        /// <summary>
        /// 迟到早退次数
        /// </summary>
        public string CDZT { get; set; }

        /// <summary>
        /// 旷工天数
        /// </summary>
        public string KGTS { get; set; }

        /// <summary>
        /// 事假（天数）
        /// </summary>
        public string SJ { get; set; }

        /// <summary>
        /// 病假（天数）
        /// </summary>
        public string BJ { get; set; }

        /// <summary>
        /// 考勤合计
        /// </summary>
        public string KQHJ { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }
    }
}