using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 报工
    /// </summary>
    public class DataBase3PY_BG
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string No { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public string GZ { get; set; }

        public string UserCode { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 工号（短）
        /// </summary>
        public string GH { get; set; }

        /// <summary>
        /// 熟练工天数
        /// </summary>
        public string SLGTS { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        public string YCQTS { get; set; }

        /// <summary>
        /// 学徒工天数
        /// </summary>
        public string XTGTS { get; set; }

        /// <summary>
        /// 实出勤天数（熟练工+学徒工）
        /// </summary>
        public string SCQTS { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string JBGZ { get; set; }

        /// <summary>
        /// 技能工资
        /// </summary>
        public string JNGZ { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        public string JiaBGZ { get; set; }

        /// <summary>
        /// 学徒基本工资占比
        /// </summary>
        public string XTJBGZZB { get; set; }

        /// <summary>
        /// 学徒技能工资占比
        /// </summary>
        public string XTJNGZZB { get; set; }

        /// <summary>
        /// 喷釉班组考核占比
        /// </summary>
        public string PYBZKHZB { get; set; }

        /// <summary>
        /// 基本工资额
        /// </summary>
        public string JBGZE { get; set; }

        /// <summary>
        /// 技能工资额
        /// </summary>
        public string JNGZE { get; set; }

        /// <summary>
        /// 加班工资额
        /// </summary>
        public string JiaBGZE { get; set; }

        /// <summary>
        /// 班组考核工资
        /// </summary>
        public string BZKHGZ { get; set; }

        /// <summary>
        /// 修检个人考核工资
        /// </summary>
        public string JXGRKHGZ { get; set; }

        /// <summary>
        /// 修检计件工资
        /// </summary>
        public string JXJJGZ { get; set; }

        /// <summary>
        /// 擦绺工个人考核
        /// </summary>
        public string CLGGRKH { get; set; }

        /// <summary>
        /// 喷釉计件
        /// </summary>
        public string PYJJ { get; set; }

        /// <summary>
        /// 喷釉考核
        /// </summary>
        public string PYKH { get; set; }

        /// <summary>
        /// 擦水计件
        /// </summary>
        public string CSJJ { get; set; }

        /// <summary>
        /// 擦水考核
        /// </summary>
        public string CSKH { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string HJ { get; set; }
    }

    public class DataBase3PY_BG_YZ
    {
        public Guid Id { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string LB { get; set; }
        public string GD { get; set; }
        public string GZ { get; set; }
        public decimal SLGTS { get; set; }
        public decimal Sjrs { get; set; }
        public decimal Dy { get; set; }
        public decimal Dy_Sjrs { get; set; }
        public string CreateUser { get; set; }
        public bool IsOK { get; set; }
    }
}