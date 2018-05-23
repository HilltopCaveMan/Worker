using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 上线补助
    /// </summary>
    public class DataBase3CX_CX_04SXBZ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 原产品名称
        /// </summary>
        public string YCPMC { get; set; }

        /// <summary>
        /// 变更产品名称
        /// </summary>
        public string BGCPMC { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 应给人数
        /// </summary>
        public string YGRS { get; set; }

        /// <summary>
        /// 实给人数
        /// </summary>
        public string SGRS { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public string SXSJ { get; set; }

        /// <summary>
        /// 上线第几月
        /// </summary>
        public string SXDJY { get; set; }

        /// <summary>
        /// je（不用）
        /// </summary>
        public string JE_NotUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public string SJJE { get; set; }

        /// <summary>
        /// 当月出勤天数
        /// </summary>
        public string DYCQTS { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQTS { get; set; }

        /// <summary>
        /// 补助金额
        /// </summary>
        public string BZJE { get; set; }

        /// <summary>
        /// 是否补余额
        /// </summary>
        public bool IsBYE { get; set; }
    }
}