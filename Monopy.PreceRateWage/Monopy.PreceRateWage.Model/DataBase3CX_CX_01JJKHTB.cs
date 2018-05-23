using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 01计件考核提报
    /// </summary>
    public class DataBase3CX_CX_01JJKHTB
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
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 搭档夫妻
        /// </summary>
        public string DHFQ { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 注修工号
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        public string PZMC { get; set; }

        /// <summary>
        /// 双锅数
        /// </summary>
        public string SGS { get; set; }

        /// <summary>
        /// 模型数
        /// </summary>
        public string MXS { get; set; }

        /// <summary>
        /// 注浆次数
        /// </summary>
        public string ZJCS { get; set; }

        /// <summary>
        /// 模型数2
        /// </summary>
        public string MXS2 { get; set; }

        /// <summary>
        /// 注浆次数2
        /// </summary>
        public string ZJCS2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}