using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 03成型计件
    /// </summary>
    public class DataBase3CX_CX_03JJ
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
        /// 班组
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string BZName { get; set; }

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
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 一级品
        /// </summary>
        public string YJP { get; set; }

        /// <summary>
        /// 破损数
        /// </summary>
        public string PSS { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 一级率
        /// </summary>
        public string YJL { get; set; }

        /// <summary>
        /// 交坯数
        /// </summary>
        public string JPS { get; set; }

        /// <summary>
        /// 大裂数量
        /// </summary>
        public string DLSL { get; set; }

        /// <summary>
        /// 双锅次数
        /// </summary>
        public string SGCS { get; set; }

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
        /// 实际交坯率
        /// </summary>
        public string SJJPL { get; set; }

        /// <summary>
        /// 应交坯率
        /// </summary>
        public string YJPL { get; set; }

        /// <summary>
        /// 目标一级率
        /// </summary>
        public string MBYJL { get; set; }

        /// <summary>
        /// 目标一级率2
        /// </summary>
        public string MBYJL2 { get; set; }

        /// <summary>
        /// 应交坯率
        /// </summary>
        public string YJPL_CL { get; set; }

        /// <summary>
        /// 产量上浮基数比
        /// </summary>
        public string CLSFJSB { get; set; }

        /// <summary>
        /// 产量上浮单价
        /// </summary>
        public string CLSFDJ { get; set; }

        /// <summary>
        /// 质量上浮基数比
        /// </summary>
        public string ZLSFJSB { get; set; }

        /// <summary>
        /// 质量下浮基数比
        /// </summary>
        public string ZLXFJSB { get; set; }

        /// <summary>
        /// 质量上浮百分点
        /// </summary>
        public string ZLSFBFD { get; set; }

        /// <summary>
        /// 质量下浮百分点
        /// </summary>
        public string ZLXFBFD { get; set; }

        /// <summary>
        /// 上封顶单价
        /// </summary>
        public string SFDDJ { get; set; }

        /// <summary>
        /// 下封顶单价
        /// </summary>
        public string XFDDJ { get; set; }

        /// <summary>
        /// 双锅单价
        /// </summary>
        public string SGDJ { get; set; }

        /// <summary>
        /// 产量考核
        /// </summary>
        public string CLKH { get; set; }

        /// <summary>
        /// 质量考核
        /// </summary>
        public string ZLKH { get; set; }

        /// <summary>
        /// 单价变动最后
        /// </summary>
        public string DJBDZH { get; set; }

        /// <summary>
        /// 双锅金额
        /// </summary>
        public string SGJE { get; set; }

        /// <summary>
        /// 计件总额
        /// </summary>
        public string JJZE { get; set; }

        /// <summary>
        /// 工资总额
        /// </summary>
        public string GZZE { get; set; }
    }
}