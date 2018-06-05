using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 修检线线长考核
    /// </summary>
    public class DataBase1XJ_XCKH
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
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 质量目标
        /// </summary>
        public string ZLMB { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public string SJWC { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public string SFWC { get; set; }

        /// <summary>
        /// 线长费
        /// </summary>
        public string XCF { get; set; }

        /// <summary>
        /// 差额
        /// </summary>
        public string CE { get; set; }

        /// <summary>
        /// 考核
        /// </summary>
        public string KH { get; set; }

        /// <summary>
        /// 考核上封顶
        /// </summary>
        public string SFD { get; set; }

        /// <summary>
        /// 考核下封顶
        /// </summary>
        public string XFD { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }
    }
}
