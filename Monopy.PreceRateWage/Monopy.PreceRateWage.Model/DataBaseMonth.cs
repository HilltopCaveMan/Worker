using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBaseMonth
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// 创建年份
        /// </summary>
        [Required]
        public int CreateYear { get; set; }

        /// <summary>
        /// 创建月份
        /// </summary>
        [Required]
        public int CreateMonth { get; set; }

        /// <summary>
        /// 工厂编号
        /// </summary>
        [StringLength(10)]
        public string FactoryNo { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string WorkshopName { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [StringLength(50)]
        public string PostName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [StringLength(50)]
        public string Classification { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(50)]
        public string Gender { get; set; }

        /// <summary>
        /// 月份(线长费)
        /// </summary>
        [StringLength(50)]
        public string MonthData { get; set; }

        /// <summary>
        /// 产品类别
        /// </summary>
        [StringLength(50)]
        public string ProductType { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        [StringLength(50)]
        public string Types { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        [StringLength(50)]
        public string TypesType { get; set; }

        /// <summary>
        /// 师傅本月退还徒弟模型个数
        /// </summary>
        public string SFbackTD { get; set; }

        /// <summary>
        /// 辅助验货日工
        /// </summary>
        public string DayWork_FZYH { get; set; }

        /// <summary>
        /// 临时工日工
        /// </summary>
        public string DayWork_LSG { get; set; }

        /// <summary>
        /// 学徒日工
        /// </summary>
        public string DayWork_XT { get; set; }

        /// <summary>
        /// 弹性假日工
        /// </summary>
        public string DayWork_TXJ { get; set; }

        /// <summary>
        /// 班长/线长费
        /// </summary>
        public string MoneyShift { get; set; }

        /// <summary>
        /// 学徒基本工资占比
        /// </summary>
        public string ZB_XT_JB { get; set; }

        /// <summary>
        /// 学徒技能工资占比
        /// </summary>
        public string ZB_XT_JN { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string MoneyBase { get; set; }

        /// <summary>
        /// 技能工资
        /// </summary>
        public string MoneyJN { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        public string MoneyJB { get; set; }

        /// <summary>
        /// 全勤奖
        /// </summary>
        public string MoneyQQJ { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        public string MoneyKH { get; set; }

        /// <summary>
        /// 入职补助
        /// </summary>
        public string MoneyRZBZ { get; set; }

        /// <summary>
        /// 变产补助
        /// </summary>
        public string MoneyBCBZ { get; set; }

        public override string ToString()
        {
            return "工厂:【" + FactoryNo + "】,车间名称:【" + WorkshopName + "】,岗位名称:【" + PostName + "】,类别:【" + Classification + "】,性别:【" + Gender + "】";
        }
    }
}