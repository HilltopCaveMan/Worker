using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 工段产品达成率
    /// </summary>
    public class DataBase1CX_DNG_01CXDNGKH_01DCL
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
        /// 品种
        /// </summary>
        public string PZ { get; set; }

        /// <summary>
        /// 成型计划合格品数
        /// </summary>
        public string HGPS { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public string SJWC { get; set; }

        /// <summary>
        /// 达成数量
        /// </summary>
        public string DCSJ { get; set; }
       
    }
}
