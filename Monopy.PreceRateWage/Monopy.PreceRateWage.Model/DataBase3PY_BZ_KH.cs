using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 喷釉班组考核
    /// </summary>
    public class DataBase3PY_BZ_KH
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 工段——喷釉段
        /// </summary>
        public string GD_PYD { get; set; }

        /// <summary>
        /// 工段——修检段
        /// </summary>
        public string GD_XJD { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 喷釉——开窑量
        /// </summary>
        public string PY_KYL { get; set; }

        /// <summary>
        /// 喷釉——一级品
        /// </summary>
        public string PY_YJP { get; set; }

        /// <summary>
        /// 修检线——开窑量
        /// </summary>
        public string XJX_KYL { get; set; }

        /// <summary>
        /// 修检线——一级品
        /// </summary>
        public string XJX_YJP { get; set; }

        /// <summary>
        /// 修检线——滚釉
        /// </summary>
        public string XJX2_GY { get; set; }

        /// <summary>
        /// 修检线——爆釉
        /// </summary>
        public string XJX2_BY { get; set; }

        /// <summary>
        /// 修检线——坯渣
        /// </summary>
        public string XJX2_PZ { get; set; }

        /// <summary>
        /// 班组——实际完成
        /// </summary>
        public string BZ_SJWC { get; set; }

        /// <summary>
        /// 多功能喷釉工——实际完成
        /// </summary>
        public string DGNPYG_SJWC { get; set; }

        /// <summary>
        /// 机器人维护工——实际完成
        /// </summary>
        public string JQRWHG_SJWC { get; set; }

        /// <summary>
        /// 擦绺找枪——实际完成
        /// </summary>
        public string CLZQ_SJWC { get; set; }

        /// <summary>
        /// 班组——班组考核指标
        /// </summary>
        public string BZ_BZKHZB { get; set; }

        /// <summary>
        /// 班组——班组基数比
        /// </summary>
        public string BZ_BZJSB { get; set; }

        /// <summary>
        /// 班组——班组奖励单价
        /// </summary>
        public string BZ_BZJLDJ { get; set; }

        /// <summary>
        /// 机器人维护工——班组考核指标
        /// </summary>
        public string JQRWHG_BZKHZB { get; set; }

        /// <summary>
        /// 机器人维护工——班组基数比
        /// </summary>
        public string JQRWHG_BZJSB { get; set; }

        /// <summary>
        /// 机器人维护工——班组奖励单价
        /// </summary>
        public string JQRWHG_BZJLDJ { get; set; }

        /// <summary>
        /// 多功能喷釉工——班组考核指标
        /// </summary>
        public string DGNPYG_BZKHZB { get; set; }

        /// <summary>
        /// 多功能喷釉工——班组基数比
        /// </summary>
        public string DGNPYG_BZJSB { get; set; }

        /// <summary>
        /// 多功能喷釉工——班组奖励单价
        /// </summary>
        public string DGNPYG_BZJLDJ { get; set; }

        /// <summary>
        /// 擦绺找枪班组——班组考核指标
        /// </summary>
        public string CLZQBZ_BZKHZB { get; set; }

        /// <summary>
        /// 修检班组——班组考核指标
        /// </summary>
        public string JXBZ_BZKHZB { get; set; }

        /// <summary>
        /// 修检班组——班组基数比
        /// </summary>
        public string JXBZ_BZJSB { get; set; }

        /// <summary>
        /// 修检班组——班组奖励单价
        /// </summary>
        public string JXBZ_BZJLDJ { get; set; }

        /// <summary>
        /// 擦水班组——班组考核指标
        /// </summary>
        public string CSBZ_BZKHZB { get; set; }

        /// <summary>
        /// 擦水班组——班组基数比
        /// </summary>
        public string CSBZ_BZJSB { get; set; }

        /// <summary>
        /// 擦水班组——班组奖励单价
        /// </summary>
        public string CSBZ_BZJLDJ { get; set; }

        /// <summary>
        /// 擦绺工——喷釉班组考核占比
        /// </summary>
        public string CLG_PYBZKHZB { get; set; }

        /// <summary>
        /// 上坯工——喷釉班组考核占比
        /// </summary>
        public string SPG_PYBZKHZB { get; set; }
    }
}