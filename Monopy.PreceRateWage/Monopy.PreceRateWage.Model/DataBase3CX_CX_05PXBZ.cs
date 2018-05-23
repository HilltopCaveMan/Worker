using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 培训补助
    /// </summary>
    public class DataBase3CX_CX_05PXBZ
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
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 所注品种
        /// </summary>
        public string SZPZ { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 第几月
        /// </summary>
        public string DJY { get; set; }

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
        /// 实出勤天数
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQ { get; set; }

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