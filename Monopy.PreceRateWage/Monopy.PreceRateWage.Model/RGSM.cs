namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 角色 权限（菜单） 选择 模型
    /// </summary>
    public class RGSM
    {
        /// <summary>
        /// 操作的菜单
        /// </summary>
        public BaseGroup Group { get; set; }

        /// <summary>
        /// 设置的参照
        /// </summary>
        public string Paras { get; set; }
    }
}