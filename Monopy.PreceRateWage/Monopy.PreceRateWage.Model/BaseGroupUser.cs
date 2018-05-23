using System;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 单独对人授权
    /// </summary>
    public class BaseGroupUser
    {
        [Key]
        public Guid Id { get; set; }

        public virtual BaseGroup Group { get; set; }
        public virtual BaseUser User { get; set; }

        /// <summary>
        /// 窗口内部控件名是否可用，用$分割（1可用；0禁用）
        /// </summary>
        public string Paras { get; set; }
    }
}