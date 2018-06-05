using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 修检工计件
    /// </summary>
    public class DataBase1XJ_XJGJJ
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
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string CHBM { get; set; }

        /// <summary>
        /// 存货名称
        /// </summary>
        public string  CHMC { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string  GH { get; set; }

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
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 修检计件
        /// </summary>
        public string XJJJ{ get; set; }
    }
}
