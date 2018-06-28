using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 多能工考核
    /// </summary>
    public class DataBase1CX_DNG_01CXDNGKH_02KH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
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
        /// 工段产品达成目标
        /// </summary>
        public string GDCPDC_MB { get; set; }

        /// <summary>
        /// 工段产品达成实际完成
        /// </summary>
        public string GDCPDCL_SJWC { get; set; }

        /// <summary>
        /// 工段整体产量目标
        /// </summary>
        public string GDZTCL_MB { get; set; }

        /// <summary>
        /// 工段整体产量实际完成
        /// </summary>
        public string GDZTCL_SJWC { get; set; }

        /// <summary>
        /// 工段整体质量差率
        /// </summary>
        public string GDZTZL_CL { get; set; }

        /// <summary>
        /// 工段整体质量考核金额
        /// </summary>
        public string GDZTZL_KHJE { get; set; }

        /// <summary>
        /// 工段产品达成差率
        /// </summary>
        public string GDCPDC_CL { get; set; }

        /// <summary>
        /// 工段产品达成考核金额
        /// </summary>
        public string GDCPDC_KHJE { get; set; }

        /// <summary>
        /// 工段整体产量差率
        /// </summary>
        public string GDZTCL_CL { get; set; }

        /// <summary>
        /// 工段整体产量考核金额
        /// </summary>
        public string GDZTCL_KHJE { get; set; }

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
        public string  YCQ { get; set; }
    }
}
