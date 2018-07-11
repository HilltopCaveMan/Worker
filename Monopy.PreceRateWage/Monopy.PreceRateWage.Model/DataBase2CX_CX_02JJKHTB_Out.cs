using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase2CX_CX_02JJKHTB_Out
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

        /// <summary>
        /// 员工编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 人员编码班组
        /// </summary>
        public string RYBMBZ { get; set; }
    }
}
