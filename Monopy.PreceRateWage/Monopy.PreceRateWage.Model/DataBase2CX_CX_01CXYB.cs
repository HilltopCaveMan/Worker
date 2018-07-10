using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 1成型月报
    /// </summary>
    public class DataBase2CX_CX_01CXYB
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
        /// 产品编码
        /// </summary>
        public string CPBM { get; set; }

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
        /// 产品名称
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string GCBM { get; set; }

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
        public string YJL { get; set; }

        /// <summary>
        /// 成走缺损率
        /// </summary>
        public string QX01 { get; set; }

        /// <summary>
        /// 内裂缺损率
        /// </summary>
        public string QX02 { get; set; }

        /// <summary>
        /// 裂底缺损率
        /// </summary>
        public string QX03 { get; set; }

        /// <summary>
        /// 裂体缺损率
        /// </summary>
        public string QX04 { get; set; }

        /// <summary>
        /// 裂眼缺损率
        /// </summary>
        public string QX05 { get; set; }

        /// <summary>
        /// 修糙缺损率
        /// </summary>
        public string QX06 { get; set; }

        /// <summary>
        /// 棕眼缺损率
        /// </summary>
        public string QX07 { get; set; }

        /// <summary>
        /// 成脏缺损率
        /// </summary>
        public string QX08 { get; set; }

        /// <summary>
        /// 泥绺缺损率
        /// </summary>
        public string QX09 { get; set; }

        /// <summary>
        /// 坯泡缺损率
        /// </summary>
        public string QX10 { get; set; }

        /// <summary>
        /// 崩渣缺损率
        /// </summary>
        public string QX11 { get; set; }

        /// <summary>
        /// 崩渣缺损率
        /// </summary>
        public string QX12 { get; set; }

        /// <summary>
        /// 卡球缺损率
        /// </summary>
        public string QX13 { get; set; }

        /// <summary>
        /// 吹脏缺损率
        /// </summary>
        public string QX14 { get; set; }

        /// <summary>
        /// 降级
        /// </summary>
        public string JJ { get; set; }

        /// <summary>
        /// 破损
        /// </summary>
        public string PS { get; set; }
    }
}
