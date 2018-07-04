using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 成型技术员考核
    /// </summary>
    public class DataBase2CX_JSY_01CXJSYKH_02KH
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
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 人员编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工段整体质量目标
        /// </summary>
        public string GDZTZL_MB { get; set; }

        /// <summary>
        /// 工段整体质量实际完成
        /// </summary>
        public string GDZTZL_SJWC { get; set; }

        /// <summary>
        /// 工段差手质量目标
        /// </summary>
        public string GDCSZL_MB { get; set; }

        /// <summary>
        /// 工段差手质量实际完成
        /// </summary>
        public string GDCSZL_SJWC { get; set; }

        /// <summary>
        /// 工段产量目标
        /// </summary>
        public string GDGDCL_MB { get; set; }

        /// <summary>
        /// 工段产量实际完成
        /// </summary>
        public string GDGDCL_SJWC { get; set; }

        /// <summary>
        /// 工段整体质量差率
        /// </summary>
        public string GDZTZL_CL { get; set; }

        /// <summary>
        /// 工段整体质量考核金额
        /// </summary>
        public string GDZTZL_KHJE { get; set; }

        /// <summary>
        /// 工段差手质量差率
        /// </summary>
        public string GDCSZL_CL { get; set; }

        /// <summary>
        /// 工段差手质量考核金额
        /// </summary>
        public string GDCSZL_KHJE { get; set; }

        /// <summary>
        /// 工段工段产量差率
        /// </summary>
        public string GDGDCL_CL { get; set; }

        /// <summary>
        /// 工段工段产量考核金额
        /// </summary>
        public string GDGDCL_KHJE { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 实出勤
        /// </summary>
        public string SCQ { get; set; }

        /// <summary>
        /// 应出勤
        /// </summary>
        public string YCQ { get; set; }
    }
}