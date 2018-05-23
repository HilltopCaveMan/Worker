using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Design;

namespace Monopy.PreceRateWage.Model
{
    public class BaseGroup
    {
        [Category("Data")]
        [Description("编号，不能手工修改")]
        [ReadOnly(true)]
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("编号（必填）且不能重复（为了省事没有校验，请自己检查）")]
        public string GroupID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("显示文字")]
        public string Name { get; set; }

        /// <summary>
        /// 是否选中Tab（针对父级）
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("默认选中（只针对父级有效）")]
        public bool IsChecked { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("父编号")]
        public string ParentID { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("提示信息")]
        public string Tooltip { get; set; }

        /// <summary>
        /// 菜单类(全称)
        /// </summary>
        [Category("Action")]
        [DefaultValue("")]
        [Description("显示那个窗体，类全名称（包括命名空间）")]
        public string GroupClass { get; set; }

        /// <summary>
        /// 是否开始组
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("是否开始组")]
        public bool IsBeginGroup { get; set; }

        /// <summary>
        /// 是否在ribbonBar中显示（三级菜单中找一个二级）
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("是否二级菜单")]
        public bool IsOnRibbonBar { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("图标")]
        [Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=11.0.0.4, Culture=neutral,  PublicKeyToken=2c9ff1fddc42653c", typeof(UITypeEditor))]
        public string Symbol { get; set; }

        /// <summary>
        /// 是否弹出显示（true：弹出显示；false：多窗口显示）
        /// </summary>
        [Category("Action")]
        [DefaultValue(false)]
        [Description("是否弹出显示（true：弹出显示；false：多窗口显示）")]
        public bool Showdialog { get; set; }

        /// <summary>
        /// 是否可以关闭（不涉及弹出窗口）
        /// </summary>
        [Category("Action")]
        [DefaultValue(true)]
        [Description("是否可以关闭（不涉及弹出窗口）")]
        public bool CloseButtonVisible { get; set; }

        /// <summary>
        /// c(controls)@+窗口内部控件名，用$分割：控件名用和显示名(或说明)，多个用分号;分隔
        /// a(args)@+各个参数，用$分割每一个参数
        /// c和a用~分割，
        /// 注意，~、@、$、;有特殊用途，配置的时候不要使用。
        /// 例如：c@btnNew$新增按钮;btnDelete$删除按钮~a@3-jb-Pg
        /// c@BtnPMCCheckYes$PMC确认;BtnPMCCheckNo$PMC退回;BtnPGCheckYes$品管确认;BtnPGCheckNo$品管退回
        /// </summary>
        [Category("Action")]
        [DefaultValue("")]
        [Description("窗口内部控件名，用$分割：显示名和控件名用，多个用分号;分隔")]
        public string Paras { get; set; }
    }
}