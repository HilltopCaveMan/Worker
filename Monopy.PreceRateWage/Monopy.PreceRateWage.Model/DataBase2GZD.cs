using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 二厂汇总（工资单）
    /// </summary>
    public class DataBase2GZD
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 计算方式:每次计算时需要判断是否已经计算过了，区分每个车间的工资计算方式是否已经计算。
        /// </summary>
        public string CalculationType { get; set; }

        /// <summary>
        /// 厂区
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string Dept { get; set; }

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
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 入职补助
        /// </summary>
        public string RZBZ { get; set; }

        /// <summary>
        /// 变产补助
        /// </summary>
        public string BCBZ { get; set; }

        /// <summary>
        /// 补压坯
        /// </summary>
        public string BYP { get; set; }

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
        /// 辅助验货
        /// </summary>
        public string FZYH { get; set; }

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
        /// 车间奖金
        /// </summary>
        public string CJJJ { get; set; }

        /// <summary>
        /// 车间罚款
        /// </summary>
        public string CJFK { get; set; }

        /// <summary>
        /// 辅料奖罚
        /// </summary>
        public string FLFK { get; set; }

        /// <summary>
        /// 破损罚款
        /// </summary>
        public string PSFK { get; set; }

        /// <summary>
        /// 其他加项
        /// </summary>
        public string QTJX { get; set; }

        /// <summary>
        /// 其他减项
        /// </summary>
        public string QTJ { get; set; }
    }
}
