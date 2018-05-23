using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class BaseRoleUser
    {
        [Key]
        public Guid Id { get; set; }

        public virtual BaseRole Role { get; set; }

        public virtual BaseUser User { get; set; }
    }
}