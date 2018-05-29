using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 线位人员出勤日录入、计算
    /// </summary>
    public class DataBase1JB_XWRYCQ
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public DateTime? CreateTime { get; set; }

        [Required]
        public string CreateUser { get; set; }

        [Required]
        public int TheYear { get; set; }

        [Required]
        public int TheMonth { get; set; }

        [Required]
        public int TheDay { get; set; }


        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 线位
        /// </summary>
        public string XW { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string GWMC { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 学徒天数
        /// </summary>
        public string StudyDay { get; set; }

        /// <summary>
        /// 熟练工天数
        /// </summary>
        public string WorkDay { get; set; }

        /// <summary>
        /// 当月实际出勤天数
        /// </summary>
        public string  DYSCQ { get; set; }

        /// <summary>
        /// 工资占比
        /// </summary>
        public string GZZB { get; set; }

        /// <summary>
        /// 总工资
        /// </summary>
        public string TotalGZ { get; set; }

        /// <summary>
        /// 岗位总出勤
        /// </summary>
        public string GWZCQ { get; set; }

        private string dgwgz;

        /// <summary>
        /// 单岗位工资
        /// </summary>
        public string DGWGZ
        {
            get
            {
                return string.IsNullOrEmpty(dgwgz) ? string.Empty : Convert.ToDecimal(dgwgz).ToString("0.0000");
            }
            set
            {
                dgwgz = value;
            }
        }

        /// <summary>
        /// 替班工资(总)
        /// </summary>
        public string  TotalTBGZE { get; set; }

        private string tbgze;

        /// <summary>
        /// 替班工资额（个人）
        /// </summary>
        public string TBGZE
        {
            get
            {
                return string.IsNullOrEmpty(tbgze) ? string.Empty : Convert.ToDecimal(tbgze).ToString("0.0000");
            }
            set
            {
                tbgze = value;
            }
        }

        /// <summary>
        /// 合计
        /// </summary>
        public string  HJ { get; set; }
    }
}