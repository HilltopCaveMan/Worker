using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 编制
    /// </summary>
    public class BaseHeadcount
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }

        /// <summary>
        /// 需求更改，新增加年和月
        /// </summary>
        public int TheYear { get; set; }

        /// <summary>
        /// 需求更改，新增加年和月
        /// </summary>
        public int TheMonth { get; set; }

        public string Factory { get; set; }
        public string No { get; set; }

        /// <summary>
        /// DL(direct labor),IDL(indirect labor),M(manager),S(staff)
        /// </summary>
        public string TypeName { get; set; }

        public string DeptName { get; set; }
        public string Line { get; set; }
        public string Name { get; set; }
        public string UserCount { get; set; }
    }
}