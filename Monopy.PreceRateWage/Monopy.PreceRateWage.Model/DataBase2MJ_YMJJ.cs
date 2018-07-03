using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 运模计件日
    /// </summary>
    public class DataBase2MJ_YMJJ
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
        /// 岗位
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string RQ { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// 撤换全线
        /// </summary>
        public string CHQX1 { get; set; }

        /// <summary>
        /// 撤/换单线
        /// </summary>
        public string CHDX1 { get; set; }

        /// <summary>
        /// 撤换全线
        /// </summary>
        public string CHQX2 { get; set; }

        /// <summary>
        /// 撤/换单线
        /// </summary>
        public string CHDX2 { get; set; }

        /// <summary>
        /// 段位
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 撤下品种
        /// </summary>
        public string CXPZ { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string CXPZSL { get; set; }

        /// <summary>
        /// 上线品种
        /// </summary>
        public string SXPZ { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string SXPZSL { get; set; }

        /// <summary>
        /// 原线位模型数
        /// </summary>
        public string YXWMXS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }
    }
}
