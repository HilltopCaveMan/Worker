using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 线位入库合格品日录入日计算
    /// </summary>
    public class DataBase1JB_XWRKHGP
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
        /// 品种名称
        /// </summary>
        [Required]
        public string TypesName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        public string X1 { get; set; }

        public string X2 { get; set; }

        public string X3 { get; set; }

        public string X4 { get; set; }

        public string X5 { get; set; }

        public string X6 { get; set; }

        public string X7 { get; set; }

        public string X8 { get; set; }

        public string X9 { get; set; }

        public string X10 { get; set; }
        public string X11 { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string Total
        {
            get
            {
                decimal tmp = (string.IsNullOrEmpty(X1) ? 0 : Convert.ToDecimal(X1)) +
                    (string.IsNullOrEmpty(X2) ? 0 : Convert.ToDecimal(X2)) +
                    (string.IsNullOrEmpty(X3) ? 0 : Convert.ToDecimal(X3)) +
                    (string.IsNullOrEmpty(X4) ? 0 : Convert.ToDecimal(X4)) +
                    (string.IsNullOrEmpty(X5) ? 0 : Convert.ToDecimal(X5)) +
                    (string.IsNullOrEmpty(X6) ? 0 : Convert.ToDecimal(X6)) +
                    (string.IsNullOrEmpty(X7) ? 0 : Convert.ToDecimal(X7)) +
                    (string.IsNullOrEmpty(X8) ? 0 : Convert.ToDecimal(X8)) +
                    (string.IsNullOrEmpty(X9) ? 0 : Convert.ToDecimal(X9)) +
                    (string.IsNullOrEmpty(X10) ? 0 : Convert.ToDecimal(X10)) +
                    (string.IsNullOrEmpty(X11) ? 0 : Convert.ToDecimal(X11));
                return tmp == 0 ? string.Empty : tmp.ToString();
            }
        }

        public string UnitPrice { get; set; }

        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string L4 { get; set; }
        public string L5 { get; set; }
        public string L6 { get; set; }
        public string L7 { get; set; }
        public string L8 { get; set; }
        public string L9 { get; set; }
        public string L10 { get; set; }
        public string L11 { get; set; }

        public string L1_8
        {
            get
            {
                decimal tmp = (string.IsNullOrEmpty(L1) ? 0 : Convert.ToDecimal(L1)) +
                    (string.IsNullOrEmpty(L2) ? 0 : Convert.ToDecimal(L2)) +
                    (string.IsNullOrEmpty(L3) ? 0 : Convert.ToDecimal(L3)) +
                    (string.IsNullOrEmpty(L4) ? 0 : Convert.ToDecimal(L4)) +
                    (string.IsNullOrEmpty(L5) ? 0 : Convert.ToDecimal(L5)) +
                    (string.IsNullOrEmpty(L6) ? 0 : Convert.ToDecimal(L6)) +
                    (string.IsNullOrEmpty(L7) ? 0 : Convert.ToDecimal(L7)) +
                    (string.IsNullOrEmpty(L8) ? 0 : Convert.ToDecimal(L8)) +
                    (string.IsNullOrEmpty(L11) ? 0 : Convert.ToDecimal(L11));
                return tmp == 0 ? string.Empty : tmp.ToString();
            }
        }
    }
}
