﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 11成型计件导入
    /// </summary>
    public class DataBase2CX_CXJJ
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
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 搭伙
        /// </summary>
        public string DH { get; set; }

        /// <summary>
        /// 夫妻合并
        /// </summary>
        public string FQHB { get; set; }

        /// <summary>
        /// 工资额
        /// </summary>
        public string GZE { get; set; }

    }
}
