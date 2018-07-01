using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 8个人计件
    /// </summary>
    public class DataBase1CX_CX_06JJ_GR
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
        /// 考核工资
        /// </summary>
        public string KHGZ { get; set; }

        /// <summary>
        /// 计件工资
        /// </summary>
        public string JJGZ { get; set; }

        /// <summary>
        /// 考核工资个人
        /// </summary>
        public string KHGZGR { get; set; }

        /// <summary>
        /// 计件工资个人
        /// </summary>
        public string JJGZGR { get; set; }

        /// <summary>
        /// 个人合计
        /// </summary>
        public string GRHJ { get; set; }

    }
}
