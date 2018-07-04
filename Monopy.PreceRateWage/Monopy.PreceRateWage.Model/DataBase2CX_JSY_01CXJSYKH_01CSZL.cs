using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 差手质量
    /// </summary>
    public class DataBase2CX_JSY_01CXJSYKH_01CSZL
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
        /// 人员编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 上月开窑量
        /// </summary>
        public string SYKYL { get; set; }

        /// <summary>
        /// 本月开窑量
        /// </summary>
        public string BYKYL { get; set; }

        /// <summary>
        /// 上月一级品
        /// </summary>
        public string SYYJP { get; set; }

        /// <summary>
        /// 本月一级品
        /// </summary>
        public string BYYJP { get; set; }

        /// <summary>
        /// 目标值
        /// </summary>
        public string MBZ { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public string SJWC { get; set; }
    }
}