using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBaseMsg
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string UserCode { get; set; }

        [Required]
        public string MsgTitle { get; set; }

        [Required]
        public string MsgClass { get; set; }

        public string Msg { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public string CreateUser { get; set; }
    }
}