using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 一厂检包计件日录入
    /// </summary>
    public class DataBase1JB_JJRLR
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
        /// 类别
        /// </summary>
        public virtual ICollection<DataBase1JB_JJRLR_Child> Childs { get; set; }

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
    }
}