using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_MCLBJJ_YZ3
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public bool DelFlag { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Chbm { get; set; }
        public string Chmc { get; set; }
        public string Lbbm { get; set; }
        public string Kyl { get; set; }
        public string Hg { get; set; }
        public string Dw { get; set; }
        public string C { get; set; }
        public string Lbx { get; set; }
        public string Hsx { get; set; }
        public string M { get; set; }
        public string Mx { get; set; }
        public string Hgl { get; set; }
    }
}