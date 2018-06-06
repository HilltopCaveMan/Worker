using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 软件导入
    /// </summary>
    public class DataBase1PY_RJDR
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
        /// 类型
        /// </summary>
        public string LX { get; set; }

        /// <summary>
        /// 年月
        /// </summary>
        public string YearMonth { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 工号
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
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 一级率
        /// </summary>
        public string YJV { get; set; }

        /// <summary>
        /// 考核
        /// </summary>
        public string KH { get; set; }

        /// <summary>
        /// 计件
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        public string JDRQ { get; set; }

        /// <summary>
        /// 建档人 
        /// </summary>
        public string JDR { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string LX_Remark { get; set; }

        /// <summary>
        /// 配件金额
        /// </summary>
        public string PJJJ { get; set; }

        /// <summary>
        /// 计件合计
        /// </summary>

        public string JJHJ { get; set; }
    }
}
