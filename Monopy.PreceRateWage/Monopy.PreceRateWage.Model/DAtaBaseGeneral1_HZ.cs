using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 一厂汇总
    /// </summary>
    public class DAtaBaseGeneral1_HZ
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string CJ { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string GWMC { get; set; }

        /// <summary>
        /// 人员编码
        /// </summary>
        public string RYBM { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string XM { get; set; }

        /// <summary>
        /// 计件工资1
        /// </summary>
        public string JJGZ1 { get; set; }

        /// <summary>
        /// 计件工资2
        /// </summary>
        public string JJGZ2 { get; set; }

        /// <summary>
        /// 计件工资3
        /// </summary>
        public string JJGZ3 { get; set; }

        /// <summary>
        /// 计件工资4
        /// </summary>
        public string JJGZ4 { get; set; }

        /// <summary>
        /// 考核工资1
        /// </summary>
        public string KHGZ1 { get; set; }

        /// <summary>
        /// 考核工资2
        /// </summary>
        public string KHGZ2 { get; set; }

        /// <summary>
        /// 考核工资3
        /// </summary>
        public string KHGZ3 { get; set; }

        /// <summary>
        /// 考核工资4
        /// </summary>
        public string KHGZ4 { get; set; }

        /// <summary>
        /// 日工
        /// </summary>
        public string RG { get; set; }

        /// <summary>
        /// 试烧补助
        /// </summary>
        public string SSBZ { get; set; }

        /// <summary>
        /// 破损补助
        /// </summary>
        public string PSBZ { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 罚款
        /// </summary>
        public string FK { get; set; }

        /// <summary>
        /// 辅料奖罚
        /// </summary>
        public string FLJF { get; set; }

        /// <summary>
        /// 破损罚款
        /// </summary>
        public string PSFK { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string QT { get; set; }
    }
}
