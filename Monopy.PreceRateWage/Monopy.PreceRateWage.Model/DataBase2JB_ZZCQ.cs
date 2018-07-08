using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 2-1组装工出勤
    /// </summary>
    public class DataBase2JB_ZZCQ
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
        /// 出勤天数
        /// </summary>
        public string CQTS { get; set; }

        /// <summary>
        /// 个人计件
        /// </summary>
        public string GRJJ { get; set; }


    }
}
