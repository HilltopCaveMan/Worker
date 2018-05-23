using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_MCLBJJ_YZ5
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
        public string Hm { get; set; }
        public string Mdw { get; set; }
        public string Qt { get; set; }
        public string Mc { get; set; }
        public string Fj { get; set; }
        public string Cb { get; set; }
        public string Mx { get; set; }
        public string Yx { get; set; }
        public string Lj { get; set; }
        public string Hgl { get; set; }
        public string Zzhgl { get; set; }
    }
}