using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 模型数，7通导入并导出。
    /// </summary>
    public class DataBase3CX_CX_02MXS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string YF { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string BZName { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 工号（短）
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string CHBM { get; set; }

        /// <summary>
        /// 存货名称
        /// </summary>
        public string CHMC { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        public string LBBM { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string LBMC { get; set; }

        /// <summary>
        /// 双锅次数
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
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 交坯数
        /// </summary>
        public string JPS { get; set; }
    }
}