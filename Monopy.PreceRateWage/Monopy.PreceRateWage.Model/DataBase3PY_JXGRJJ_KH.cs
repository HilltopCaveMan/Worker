using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 喷釉修检工个人考核
    /// </summary>
    public class DataBase3PY_JXG_KH
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
        /// 工号（短工号）
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string LBMC { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 裂体
        /// </summary>
        public string LT { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public string SJWC { get; set; }

        /// <summary>
        /// 个人指标
        /// </summary>
        public string GRZB { get; set; }

        /// <summary>
        /// 最低开窑量
        /// </summary>
        public string ZDKYL { get; set; }

        /// <summary>
        /// 个人考核金额2
        /// </summary>
        public string KHJE2 { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>
        public string JJJE { get; set; }
    }
}