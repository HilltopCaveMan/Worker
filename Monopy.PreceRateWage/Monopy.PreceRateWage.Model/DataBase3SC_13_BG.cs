using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 烧成报工
    /// </summary>
    public class DataBase3SC_13_BG
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string GH { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 学徒天数
        /// </summary>
        public string XTTS { get; set; }

        /// <summary>
        /// 熟练工天数
        /// </summary>
        public string SLGTS { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQTS { get; set; }

        /// <summary>
        /// 合计—— 实出勤（学徒+熟练工）
        /// </summary>
        public string HJ_SCQTS { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string JBGZ { get; set; }

        /// <summary>
        /// 基本工资额
        /// </summary>
        public string JBGZE { get; set; }

        /// <summary>
        /// 技术员考核工资
        /// </summary>
        public string JSYKHGZ { get; set; }

        /// <summary>
        /// 开窑计件
        /// </summary>
        public string KYJJ { get; set; }

        /// <summary>
        /// 开窑考核
        /// </summary>
        public string KYKH { get; set; }

        /// <summary>
        /// 回烧计件和考核
        /// </summary>
        public string HSJJHKH { get; set; }

        /// <summary>
        /// 气站考核
        /// </summary>
        public string QZKH { get; set; }

        /// <summary>
        /// 补瓷计件和考核
        /// </summary>
        public string BCJJHKH { get; set; }

        /// <summary>
        /// 烧窑小小
        /// </summary>
        public string SYGX { get; set; }

        /// <summary>
        /// 贴标工线上考核
        /// </summary>
        public string TBGXSKH { get; set; }

        /// <summary>
        /// 贴标工线下考核
        /// </summary>
        public string TBGXXKH { get; set; }

        /// <summary>
        /// 吸尘工考核
        /// </summary>
        public string XCGKH { get; set; }

        /// <summary>
        /// 装窑计件和考核
        /// </summary>
        public string ZYJJHKH { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ_Money { get; set; }
    }
}