using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class BaseGroupRole
    {
        [Key]
        public Guid ID { get; set; }

        public virtual BaseGroup Group { get; set; }

        public string Paras { get; set; }

        public virtual BaseRole Role { get; set; }
    }
}