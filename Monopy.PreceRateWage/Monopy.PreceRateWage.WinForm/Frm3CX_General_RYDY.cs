using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_General_RYDY : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$类别$工号$人员编码$姓名$出勤天数$满勤天数$金额".Split('$');
        private string _dept;

        private Frm3CX_General_RYDY()
        {
            InitializeComponent();
        }

        public Frm3CX_General_RYDY(string args) : this()
        {
            try
            {
                _dept = args.Split('-')[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
                return;
            }
        }

        private void Frm3CX_General_RYDY_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gh, string userCode, string userName)
        {
            var datas = new BaseDal<DataBase3CX_General_RYDY>().GetList(t => t.TheType == _dept && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => t.GH).ThenBy(t => t.UserCode).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }

            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtGH.Text, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具高压水箱——工号人员编码对应.xlsx");
        }

        private List<DataBase3CX_General_RYDY> Import(string filePath)
        {
            List<DataBase3CX_General_RYDY> list = null;
            list = new ExcelHelper<DataBase3CX_General_RYDY>().ReadExcel(filePath, 2, 6);
            if (list == null)
            {
                return list;
            }
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName))
                {
                    list.RemoveAt(i);
                    i--;
                    continue;
                }
                if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                {
                    MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                item.TheType = _dept;
                item.CreateUser = Program.User.ToString();
                item.CreateTime = Program.NowTime;
                item.TheYear = dtp.Value.Year;
                item.TheMonth = dtp.Value.Month;
            }
            return list;
        }

        private bool Recount(List<DataBase3CX_General_RYDY> list)
        {
            foreach (var item in list)
            {
                decimal.TryParse(item.CQTS, out decimal scq);
                decimal.TryParse(item.MQTS, out decimal ycq);
                if (ycq == 0M)
                {
                    MessageBox.Show($"没有工号为【{item.GH}】，的满勤天数不正确【{item.MQTS}】的数据，无法导入，请检查！");
                    return false;
                }
                decimal je = 0M;
                if (_dept.Contains("面具"))
                {
                    var data = new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.GH == item.GH).FirstOrDefault();
                    if (data == null)
                    {
                        MessageBox.Show($"【高压面具】【工资核算表】中，没有工号为【{item.GH}】的数据，无法导入，请检查！");
                        return false;
                    }
                    decimal.TryParse(data.ZJE, out je);
                }
                else if (_dept.Contains("水箱"))
                {
                    var data = new BaseDal<DataBase3CX_GYSX_02_GYSX>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.GH1 == item.GH).FirstOrDefault();
                    if (data == null)
                    {
                        MessageBox.Show($"【高压水箱】【工资核算表】中，没有工号为【{item.GH}】的数据，无法导入，请检查！");
                        return false;
                    }
                    decimal.TryParse(data.DRGZ, out je);
                }
                else
                {
                    MessageBox.Show("参数错误，只支持【面具】、【水箱】");
                    return false;
                }
                item.Money = (je * scq / ycq).ToString();
            }
            return true;
        }

        private void SaveImportData(string filePath)
        {
            Enabled = false;
            var list = Import(filePath);
            if (list != null)
            {
                if (Recount(list) && new BaseDal<DataBase3CX_General_RYDY>().Add(list) > 0)
                {
                    Enabled = true;
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Enabled = true;
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Enabled = true;
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                SaveImportData(openFileDlg.FileName);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3CX_General_RYDY> list = dgv.DataSource as List<DataBase3CX_General_RYDY>;
                if (new ExcelHelper<DataBase3CX_General_RYDY>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型高压面具高压水箱——工号人员编码对应.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
                {
                    if (MessageBox.Show("导出成功，立即打开？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Process.Start(saveFileDlg.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("导出错误，请检查后，再试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase3CX_General_RYDY DataBase3CX_General_RYDY = new DataBase3CX_General_RYDY() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, TheType = _dept };
            FrmModify<DataBase3CX_General_RYDY> frm = new FrmModify<DataBase3CX_General_RYDY>(DataBase3CX_General_RYDY, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnSearch.PerformClick();
            }
        }

        private void dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView grid = (DataGridView)sender;

                if (e.RowIndex >= 0)
                {
                    grid.ClearSelection();
                    grid.Rows[e.RowIndex].Selected = true;
                    if (e.ColumnIndex > -1)
                    {
                        grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                }
            }
        }

        private void Frm3CX_General_RYDY_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_General_RYDY_DragDrop(object sender, DragEventArgs e)
        {
            var tmp = ((Array)e.Data.GetData(DataFormats.FileDrop));
            if (tmp.Length != 1)
            {
                MessageBox.Show("一次只允许导入一个文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var path = tmp.GetValue(0).ToString();
            if (StringHelper.IsDir(path))
            {
                MessageBox.Show("请选择一个文件而不是文件夹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var extension = FileHelper.GetExtension(path);
            if (!extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show($"请选择一个Excel文件，当前选择的文件扩展名为：【{extension}】。{Environment.NewLine}为了所有用户的兼容性一致，请使用新格式（扩展名为【xlsx】）！{ Environment.NewLine }旧的xls格式，请用excle2007以上版本另存……", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveImportData(path);
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3CX_General_RYDY DataBase3CX_General_RYDY)
                {
                    FrmModify<DataBase3CX_General_RYDY> frm = new FrmModify<DataBase3CX_General_RYDY>(DataBase3CX_General_RYDY, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var item = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_General_RYDY;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (item != null)
                    {
                        FrmModify<DataBase3CX_General_RYDY> frm = new FrmModify<DataBase3CX_General_RYDY>(item, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要全部删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase3CX_General_RYDY>;
                foreach (var i in list)
                {
                    new BaseDal<DataBase3CX_General_RYDY>().Delete(i);
                }
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var list = new BaseDal<DataBase3CX_General_RYDY>().GetList(t => t.TheType.Contains(_dept) && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_General_RYDY>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}