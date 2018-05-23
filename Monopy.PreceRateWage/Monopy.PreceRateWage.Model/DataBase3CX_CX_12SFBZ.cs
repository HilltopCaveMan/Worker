using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 师傅补助
    /// </summary>
    public class DataBase3CX_CX_12SFBZ
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
        /// 师傅人员编码
        /// </summary>
        public string SFCode { get; set; }

        /// <summary>
        /// 师傅工号
        /// </summary>
        public string SFGH { get; set; }

        /// <summary>
        /// 师傅工段
        /// </summary>
        public string SFGD { get; set; }

        /// <summary>
        /// 师傅姓名
        /// </summary>
        public string SFName { get; set; }

        /// <summary>
        /// 徒弟工号
        /// </summary>
        public string TDGH { get; set; }

        /// <summary>
        /// 徒弟工段
        /// </summary>
        public string TDGD { get; set; }

        /// <summary>
        /// 徒弟姓名
        /// </summary>
        public string TDName { get; set; }

        /// <summary>
        /// 补贴月份_第几个月
        /// </summary>
        public string BTYF_DJGY { get; set; }

        /// <summary>
        /// 补贴标准
        /// </summary>
        public string BTBZ { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 徒弟计件
        /// </summary>
        public string TDJJ { get; set; }

        /// <summary>
        /// 师傅补助
        /// </summary>
        public string SFBZ { get; set; }
    }
}