using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class BaseUser
    {
        [Key]
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string DepName { get; set; }
        public string Remark { get; set; }
        public DateTime? LastTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }

        public override string ToString()
        {
            return Code + "_" + Name;
        }
    }

    public class BaseUserComparer : IEqualityComparer<BaseUser>
    {
        public bool Equals(BaseUser x, BaseUser y)
        {
            return x.Code.Equals(y.Code);
        }

        public int GetHashCode(BaseUser obj)
        {
            return obj.GetHashCode();
        }
    }
}