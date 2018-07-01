using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 4不考核名单
    /// </summary>
    public class DataBase1CX_CX_04BKH
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
        public string  GD { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否参与考核
        /// </summary>
        public string  SFCYKH { get; set; }

    }
}
