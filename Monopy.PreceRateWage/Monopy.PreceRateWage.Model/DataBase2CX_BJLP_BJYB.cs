using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 3半检月报大类PMC
    /// </summary>
    public class DataBase2CX_BJLP_BJYB
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
        /// 工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        public string  LBBM { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string LBMC{ get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string GC { get; set; }

        /// <summary>
        /// 工段编码
        /// </summary>
        public string GDBM { get; set; }

        /// <summary>
        /// 工段名称
        /// </summary>
        public string GDMC { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 降级
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 破损
        /// </summary>
        public string PS { get; set; }

        /// <summary>
        /// 指标
        /// </summary>
        public string ZB { get; set; }

        /// <summary>
        /// 奖励单价
        /// </summary>
        public string JLDJ { get; set; }

        /// <summary>
        /// 罚款单价
        /// </summary>
        public string FKDJ { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string KHJE { get; set; }
    }
}
