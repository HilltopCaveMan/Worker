using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 03成型计件
    /// </summary>
    public class DataBase2CX_CX_06JJ
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
        /// 员工编码
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
        /// 存货名称(品种)
        /// </summary>
        public string CHMC { get; set; }

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
        /// 模型数
        /// </summary>
        public string MXS { get; set; }

        /// <summary>
        /// 注浆次数
        /// </summary>
        public string ZJCS { get; set; }

        /// <summary>
        /// 模型数1
        /// </summary>
        public string MXS1 { get; set; }

        /// <summary>
        /// 注浆次数1
        /// </summary>
        public string ZJCS1 { get; set; }

        /// <summary>
        /// 实际交坯率
        /// </summary>
        public string SJJPL { get; set; }

        /// <summary>
        /// 产量指标
        /// </summary>
        public string CLZB { get; set; }

        /// <summary>
        /// 产量超件奖单价
        /// </summary>
        public string CLCJJDJ { get; set; }

        /// <summary>
        /// 产量亏件罚单价
        /// </summary>
        public string CLCJKDJ { get; set; }

        /// <summary>
        /// 质量底线指标
        /// </summary>
        public string ZLDXZB { get; set; }

        /// <summary>
        /// 质量奋斗指标
        /// </summary>
        public string ZLFDZB { get; set; }

        /// <summary>
        /// 产量考核
        /// </summary>
        public string CLKH { get; set; }

        /// <summary>
        /// 质量考核
        /// </summary>
        public string ZLKH { get; set; }

        /// <summary>
        /// 考核工资
        /// </summary>
        public string KHGZ { get; set; }

        /// <summary>
        /// 计件工资
        /// </summary>
        public string JJGZ { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }

        /// <summary>
        /// 残扣率
        /// </summary>
        public string CKL { get; set; }

        /// <summary>
        /// 是否学徒期
        /// </summary>
        public string  SFXTQ { get; set; }

        /// <summary>
        /// 个别调整单价
        /// </summary>
        public string GBTZDJ { get; set; }

    }
}