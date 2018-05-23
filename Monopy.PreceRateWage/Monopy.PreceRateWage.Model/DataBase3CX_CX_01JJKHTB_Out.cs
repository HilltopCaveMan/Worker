using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 01计件考核提报的导入7通
    /// </summary>
    public class DataBase3CX_CX_01JJKHTB_Out
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
        /// 班组编码
        /// </summary>
        public string BZBM { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string BZMC { get; set; }

        /// <summary>
        /// 员工（工号）
        /// </summary>
        public string GH { get; set; }
    }
}