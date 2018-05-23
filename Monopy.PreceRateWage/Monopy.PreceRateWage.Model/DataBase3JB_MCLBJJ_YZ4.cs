using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase3JB_MCLBJJ_YZ4
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public bool DelFlag { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string Lx { get; set; }
        public string Kyl { get; set; }
        public string Hgs { get; set; }
        public string Hgl { get; set; }
    }
}