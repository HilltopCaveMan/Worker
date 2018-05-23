using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class BaseRole
    {
        [Key]
        public Guid Id { get; set; }

        public string RoleName { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
}