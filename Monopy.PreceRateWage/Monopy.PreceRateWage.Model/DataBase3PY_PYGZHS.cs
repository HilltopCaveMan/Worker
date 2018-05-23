using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 喷釉工资核算
    /// </summary>
    public class DataBase3PY_PYGZHS
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 月份（文件带的格式为yyyyMM）
        /// </summary>
        public string YF { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string GCBM { get; set; }

        /// <summary>
        /// 工号（短工号）
        /// </summary>
        public string GH { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 存货类别
        /// </summary>
        public string CHLB { get; set; }

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
        /// 缺陷数
        /// </summary>
        public string QXX { get; set; }

        /// <summary>
        /// 一级率
        /// </summary>
        public string YJL { get; set; }

        /// <summary>
        /// 指标
        /// </summary>
        public string ZB { get; set; }

        /// <summary>
        /// 奖励单价
        /// </summary>
        public string JLDJ { get; set; }

        /// <summary>
        /// 惩罚单价
        /// </summary>
        public string CFDJ { get; set; }

        /// <summary>
        /// 奖罚
        /// </summary>
        public string JF { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }

        /// <summary>
        /// 计件金额
        /// </summary>
        public string JJJE { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string ZJE { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string LX { get; set; }

        /// <summary>
        /// 基础_单价
        /// </summary>
        public string B_DJ { get; set; }

        /// <summary>
        /// 基础_指标0
        /// </summary>
        public string B_ZB0 { get; set; }

        /// <summary>
        /// 基础_指标1
        /// </summary>
        public string B_ZB1 { get; set; }

        /// <summary>
        /// 基础_指标2
        /// </summary>
        public string B_ZB2 { get; set; }

        /// <summary>
        /// 基础_指标3
        /// </summary>
        public string B_ZB3 { get; set; }

        /// <summary>
        /// 基础_指标4
        /// </summary>
        public string B_ZB4 { get; set; }

        /// <summary>
        /// 基础_指标5
        /// </summary>
        public string B_ZB5 { get; set; }

        /// <summary>
        /// 基础_奖励1
        /// </summary>
        public string B_JL1 { get; set; }

        /// <summary>
        /// 基础_奖励2
        /// </summary>
        public string B_JL2 { get; set; }

        /// <summary>
        /// 基础_奖励3
        /// </summary>
        public string B_JL3 { get; set; }

        /// <summary>
        /// 基础_奖励4
        /// </summary>
        public string B_JL4 { get; set; }

        /// <summary>
        /// 基础_奖励5
        /// </summary>
        public string B_JL5 { get; set; }

        /// <summary>
        /// 结果_考核金额
        /// </summary>
        public string E_KHJE { get; set; }

        /// <summary>
        /// 结果_考核单价
        /// </summary>
        public string E_KHDJ { get; set; }

        /// <summary>
        /// 结果_计件金额
        /// </summary>
        public string E_JJJE { get; set; }

        /// <summary>
        /// 结果_总金额
        /// </summary>
        public string E_Money { get; set; }
    }
}