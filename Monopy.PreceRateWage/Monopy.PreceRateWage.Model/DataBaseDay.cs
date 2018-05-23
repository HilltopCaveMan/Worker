using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBaseDay
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
        [Required]
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
        /// 检包线位
        /// </summary>
        [StringLength(50)]
        public string JBXW { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        public string TypesName { get; set; }

        /// <summary>
        /// 品种对应类别
        /// </summary>
        [StringLength(50)]
        public string TypesType { get; set; }

        /// <summary>
        /// 品种单位
        /// </summary>
        [StringLength(50)]
        public string TypesUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice { get; set; }

        /// <summary>
        /// 检包计件工资占比
        /// </summary>
        public string ZB_JB_JJGZ { get; set; }

        /// <summary>
        /// 喷釉班组考核占比
        /// </summary>
        public string ZB_PY_BZKH { get; set; }

        /// <summary>
        /// 班组考核指标
        /// </summary>
        public string BZKHZB { get; set; }

        /// <summary>
        /// 班组基数比
        /// </summary>
        public string BZJSB { get; set; }

        /// <summary>
        /// 班组奖励单价
        /// </summary>
        public string BZJLDJ { get; set; }

        /// <summary>
        /// 个人考核指标1
        /// </summary>
        public string GRKHZB1 { get; set; }

        /// <summary>
        /// 个人基数比1
        /// </summary>
        public string GRJSB1 { get; set; }

        /// <summary>
        /// 个人奖励单价1
        /// </summary>
        public string GRJLDJ1 { get; set; }

        /// <summary>
        /// 个人罚款单价1
        /// </summary>
        public string GRFKDJ1 { get; set; }

        /// <summary>
        /// 个人考核指标2
        /// </summary>
        public string GRKHZB2 { get; set; }

        /// <summary>
        /// 个人基数比2
        /// </summary>
        public string GRJSB2 { get; set; }

        /// <summary>
        /// 个人奖励单价2
        /// </summary>
        public string GRJLDJ2 { get; set; }

        /// <summary>
        /// 个人罚款单价2
        /// </summary>
        public string GRFKDJ2 { get; set; }

        /// <summary>
        /// 个人考核指标3
        /// </summary>
        public string GRKHZB3 { get; set; }

        /// <summary>
        /// 个人基数比3
        /// </summary>
        public string GRJSB3 { get; set; }

        /// <summary>
        /// 个人奖励单价3
        /// </summary>
        public string GRJLDJ3 { get; set; }

        /// <summary>
        /// 个人罚款单价3
        /// </summary>
        public string GRFKDJ3 { get; set; }

        /// <summary>
        /// 个人考核指标4
        /// </summary>
        public string GRKHZB4 { get; set; }

        /// <summary>
        /// 个人基数比4
        /// </summary>
        public string GRJSB4 { get; set; }

        /// <summary>
        /// 个人奖励单价4
        /// </summary>
        public string GRJLDJ4 { get; set; }

        /// <summary>
        /// 个人罚款单价4
        /// </summary>
        public string GRFKDJ4 { get; set; }

        /// <summary>
        /// 考核金额上封顶
        /// </summary>
        public string KHJESFD { get; set; }

        /// <summary>
        /// 考核金额下封顶
        /// </summary>
        public string KHJEXFD { get; set; }

        /// <summary>
        /// 底线开窑量
        /// </summary>
        public string DXKYL { get; set; }

        public override string ToString()
        {
            return "工厂:【" + FactoryNo + "】,车间名称:【" + WorkshopName + "】,岗位名称:【" + PostName + "】,类别:【" + Classification + "】,检包线位:【" + JBXW + "】,品种名称:【" + TypesName + "】,品种对应类别:【" + TypesType + "】";
        }
    }
}