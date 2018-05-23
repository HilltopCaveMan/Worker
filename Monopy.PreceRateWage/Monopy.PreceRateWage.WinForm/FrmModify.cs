using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmModify<T> : Office2007Form where T : class, new()
    {
        private T _t;
        private string[] _header;
        private OptionType _optiontype;
        private string _caption;
        private TableLayoutPanel tlp = new TableLayoutPanel();
        private CCWin.SkinControl.SkinLabel[] lbls;
        private CCWin.SkinControl.SkinTextBox[] txts;
        private int _beginProperty = 0;
        private int _endProperty = 0;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="t">泛型</param>
        /// <param name="header">名称</param>
        /// <param name="optionType">操作类型（新增、修改、删除）</param>
        /// <param name="caption">标题</param>
        /// <param name="beginProperty">开始属性位置</param>
        /// <param name="endProperty">结束属性位置，倒数个数！</param>
        public FrmModify(T t, string[] header, OptionType optionType, string caption, int beginProperty = 0, int endProperty = 0)
        {
            InitializeComponent();
            EnableGlass = false;
            _t = t;
            _header = header;
            _optiontype = optionType;
            _caption = caption;
            _beginProperty = beginProperty;
            _endProperty = endProperty;
        }

        private void FrmModify_Load(object sender, EventArgs e)
        {
            string text = string.Empty;
            switch (_optiontype)
            {
                case OptionType.Add:
                    _caption += "——新增";
                    text = "确定新增";
                    break;

                case OptionType.Delete:
                    _caption += "——删除";
                    text = "确定删除";
                    break;

                case OptionType.Modify:
                    _caption += "——修改";
                    text = "确定修改";
                    break;
            }
            Text = _caption;
            btnYes.Text = text;

            var tmp = _t.GetType().GetProperties();
            lbls = new CCWin.SkinControl.SkinLabel[tmp.Length - 1 - _beginProperty - _endProperty];
            txts = new CCWin.SkinControl.SkinTextBox[tmp.Length - 1 - _beginProperty - _endProperty];
            tlp.ColumnCount = 4;
            tlp.RowCount = Convert.ToInt32(Math.Ceiling(((tmp.Length - 1 - _beginProperty - _endProperty) * 1.0) / tlp.ColumnCount * 2));
            tlp.Dock = DockStyle.Fill;
            for (int i = 0; i < tlp.ColumnCount; i++)
            {
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / tlp.ColumnCount));
            }
            float f = 100 / tlp.RowCount;
            for (int j = 0; j < tlp.RowCount; j++)
            {
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, f));
            }
            for (int i = 0; i < lbls.Length; i++)
            {
                CCWin.SkinControl.SkinLabel lbl = new CCWin.SkinControl.SkinLabel()
                {
                    Dock = DockStyle.Fill,
                    Text = _header[i + _beginProperty] + ":  ",
                    Name = "lbl$" + tmp[i + 1 + _beginProperty].Name,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                tlp.Controls.Add(lbl, Convert.ToInt32((i % 3)) * 2, Convert.ToInt32(i / 3));
                CCWin.SkinControl.SkinTextBox txt = new CCWin.SkinControl.SkinTextBox()
                {
                    Dock = DockStyle.Fill,
                    Text = tmp[i + 1 + _beginProperty].GetValue(_t, null) != null ? tmp[i + 1 + _beginProperty].GetValue(_t, null).ToString() : "",
                    Name = "txt$" + tmp[i + 1 + _beginProperty].Name,
                    Enabled = _optiontype == OptionType.Delete ? false : true
                };
                tlp.Controls.Add(txt, i % 3 * 2 + 1, i / 3);
            }
            tableLayoutPanel1.Controls.Add(tlp, 0, 0);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            foreach (var item in tlp.Controls)
            {
                if (item is CCWin.SkinControl.SkinTextBox)
                {
                    var txt = item as CCWin.SkinControl.SkinTextBox;
                    var txtProp = _t.GetType().GetProperties();
                    foreach (var prop in txtProp)
                    {
                        if (prop.Name == txt.Name.Split('$')[1])
                        {
                            _t.GetType().GetProperty(prop.Name).SetValue(_t, txt.Text, null);
                        }
                    }
                }
            }
            int result = 0;
            switch (_optiontype)
            {
                case OptionType.Add:
                    result = new BaseDal<T>().Add(_t);
                    break;

                case OptionType.Delete:
                    result = new BaseDal<T>().Delete(_t);
                    break;

                case OptionType.Modify:
                    result = new BaseDal<T>().Edit(_t);
                    break;
            }
            if (result >= 0)
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
            else
            {
                MessageBox.Show("失败，请检查操作和网络是否正常！", btnYes.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}