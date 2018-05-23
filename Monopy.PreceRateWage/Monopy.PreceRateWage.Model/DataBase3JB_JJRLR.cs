using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 三厂检包计件日录入
    /// </summary>
    public class DataBase3JB_JJRLR
    {
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [Required]
        public int TheYear { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        [Required]
        public int TheMonth { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        [Required]
        public int TheDay { get; set; }

        private string je;

        public string JE
        {
            get
            {
                return string.IsNullOrEmpty(je) ? string.Empty : Convert.ToDecimal(je).ToString("0.0000");
            }
            set
            {
                je = value;
            }
        }

        public string No { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string GW { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工厂-刷连体
        /// </summary>
        public string F_1 { get; set; }

        /// <summary>
        /// 工厂-刷水箱
        /// </summary>
        public string F_2 { get; set; }

        /// <summary>
        /// 工厂-刷大套
        /// </summary>
        public string F_3 { get; set; }

        public string F_B_1 { get; set; }
        public string F_B_2 { get; set; }
        public string F_B_3 { get; set; }
        public string F_B_4 { get; set; }
        public string F_B_5 { get; set; }

        /// <summary>
        /// PMC-包水箱盖含5755S
        /// </summary>
        public string PMC_1 { get; set; }

        /// <summary>
        /// PMC-裸套袋连体码托
        /// </summary>
        public string PMC_2 { get; set; }

        /// <summary>
        /// PMC-裸套袋大套、水箱、面具码托
        /// </summary>
        public string PMC_3 { get; set; }

        /// <summary>
        /// PMC-装卸车（各厂调货）
        /// </summary>
        public string PMC_4 { get; set; }

        /// <summary>
        /// PMC-验货贴P.0-2（802盆及大套）
        /// </summary>
        public string PMC_5 { get; set; }

        /// <summary>
        /// PMC-贴验货贴P0（全贴）
        /// </summary>
        public string PMC_6 { get; set; }

        /// <summary>
        /// PMC-贴发货贴
        /// </summary>
        public string PMC_7 { get; set; }

        /// <summary>
        /// PMC-卸配件
        /// </summary>
        public string PMC_8 { get; set; }

        /// <summary>
        /// PMC-裸套袋
        /// </summary>
        public string PMC_9 { get; set; }

        /// <summary>
        /// PMC-码包
        /// </summary>
        public string PMC_10 { get; set; }

        public string PMC_11 { get; set; }
        public string PMC_12 { get; set; }
        public string PMC_13 { get; set; }
        public string PMC_14 { get; set; }
        public string PMC_15 { get; set; }
        public string PMC_16 { get; set; }
        public string PMC_17 { get; set; }
        public string PMC_18 { get; set; }

        public string PMC_19 { get; set; }
        public string PMC_20 { get; set; }
        public string PMC_21 { get; set; }
        public string PMC_22 { get; set; }
        public string PMC_23 { get; set; }

        public string PMC_B_1 { get; set; }
        public string PMC_B_2 { get; set; }
        public string PMC_B_3 { get; set; }
        public string PMC_B_4 { get; set; }
        public string PMC_B_5 { get; set; }

        public string PG_1 { get; set; }
        public string PG_2 { get; set; }
        public string PG_3 { get; set; }
        public string PG_4 { get; set; }
        public string PG_5 { get; set; }
        public string PG_6 { get; set; }
        public string PG_7 { get; set; }
        public string PG_8 { get; set; }

        public string PG_B_1 { get; set; }
        public string PG_B_2 { get; set; }
        public string PG_B_3 { get; set; }
        public string PG_B_4 { get; set; }
        public string PG_B_5 { get; set; }

        public string KF_1 { get; set; }
        public string KF_B_1 { get; set; }
        public string KF_B_2 { get; set; }
        public string KF_B_3 { get; set; }
        public string KF_B_4 { get; set; }
        public string KF_B_5 { get; set; }

        public string WX_PMCDD_1 { get; set; }

        public string WX_1 { get; set; }
        public string WX_B_1 { get; set; }
        public string WX_B_2 { get; set; }
        public string WX_B_3 { get; set; }
        public string WX_B_4 { get; set; }
        public string WX_B_5 { get; set; }

        /// <summary>
        /// 工厂提报时间
        /// </summary>
        [Required]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 工厂提报人
        /// </summary>
        [Required]
        public string CreateUser { get; set; }

        /// <summary>
        /// 修改时间（管理员）
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人（管理员）
        /// </summary>
        public string ModifyUser { get; set; }

        /// <summary>
        /// PMC审核状态ture：通过；false：退回
        /// </summary>
        public bool IsPMC_Check { get; set; }

        /// <summary>
        /// PMC审核时间
        /// </summary>
        public DateTime? PMC_Time { get; set; }

        /// <summary>
        /// PMC审核人
        /// </summary>
        public string PMC_User { get; set; }

        /// <summary>
        /// PMC经理审核时间
        /// </summary>
        public DateTime? PMC_Manager_Time { get; set; }

        /// <summary>
        /// PMC经理
        /// </summary>
        public string PMC_Manager_User { get; set; }

        /// <summary>
        /// PMC经理是否审核
        /// </summary>
        public bool IsPMC_Manager
        {
            get
            {
                return PMC_Manager_Time != null;
            }
        }

        /// <summary>
        /// PG审核状态ture：通过；false：退回
        /// </summary>
        public bool IsPG_Check { get; set; }

        /// <summary>
        /// 品管审核时间
        /// </summary>
        public DateTime? PG_Time { get; set; }

        /// <summary>
        /// 品管审核人
        /// </summary>
        public string PG_User { get; set; }

        /// <summary>
        /// 品管经理审核时间
        /// </summary>
        public DateTime? PG_Manager_Time { get; set; }

        /// <summary>
        /// 品管经理
        /// </summary>
        public string PG_Manager_User { get; set; }

        /// <summary>
        /// 品管经理是否审核
        /// </summary>
        public bool IsPG_Manager
        {
            get
            {
                return PG_Manager_Time != null;
            }
        }

        /// <summary>
        /// KF审核状态ture：通过；false：退回
        /// </summary>
        public bool IsKF_Check { get; set; }

        /// <summary>
        /// 开发审核时间
        /// </summary>
        public DateTime? KF_Time { get; set; }

        /// <summary>
        /// 开始审核人
        /// </summary>
        public string KF_User { get; set; }

        /// <summary>
        /// 开发经理审核时间
        /// </summary>
        public DateTime? KF_Manager_Time { get; set; }

        /// <summary>
        /// 开发经理
        /// </summary>
        public string KF_Manager_User { get; set; }

        /// <summary>
        /// 开发经理是否审核
        /// </summary>
        public bool IsKF_Manager
        {
            get
            {
                return KF_Manager_Time != null;
            }
        }

        /// <summary>
        /// WX审核状态ture：通过；false：退回
        /// </summary>
        public bool IsWX_Check { get; set; }

        /// <summary>
        /// 外销审核时间
        /// </summary>
        public DateTime? WX_Time { get; set; }

        /// <summary>
        /// 外销审核人
        /// </summary>
        public string WX_User { get; set; }

        /// <summary>
        /// 外销经理审核时间
        /// </summary>
        public DateTime? WX_Manager_Time { get; set; }

        /// <summary>
        /// 外销经理
        /// </summary>
        public string WX_Manager_User { get; set; }

        /// <summary>
        /// 外销经理是否审核
        /// </summary>
        public bool IsWX_Manager
        {
            get
            {
                return WX_Manager_Time != null;
            }
        }

        /// <summary>
        /// PMC调度审核状态ture：通过；false：退回
        /// </summary>
        public bool IsPMCDD_Check { get; set; }

        /// <summary>
        /// PMC调度审核时间
        /// </summary>
        public DateTime? PMCDD_Time { get; set; }

        /// <summary>
        /// PMC调度审核人
        /// </summary>
        public string PMCDD_User { get; set; }

        /// <summary>
        /// 外销和PMC调度是否审核（针对2个部门同时审核的“售后配件”）
        /// </summary>
        public bool IsWX_PMCDD
        {
            get
            {
                return IsWX_Check && IsPMCDD_Check;
            }
        }

        public bool IsWX_PMC_Manager
        {
            get
            {
                return IsWX_Manager && IsPMC_Manager;
            }
        }
    }
}