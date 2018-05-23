using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_General_CXYB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$产品编码$工号$员工编码$姓名$产品名称$工厂编码$开窑量$一级品$一级率$成走缺损率$内裂缺损率$裂底缺损率$裂体缺损率$裂眼缺损率$修糙缺损率$棕眼缺损率$成脏缺损率$泥绺缺损率$坯泡缺损率$崩渣缺损率$冲刷缺损率$卡球缺损率$吹脏缺损率$降级$破损".Split('$');

        public Frm3CX_General_CXYB()
        {
            InitializeComponent();
        }

        private void Frm3CX_General_CXYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gh, string userCode, string userName, string cpmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_General_CXYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.Rows[0].Frozen = true;
            dgv.Columns[14].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtGH.Text, txtUserCode.Text, txtUserName.Text, txtCPMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具——成型月报.xlsx");
        }

        private List<DataBase3CX_General_CXYB> Import(string filePath)
        {
            List<DataBase3CX_General_CXYB> list = null;
            list = new ExcelHelper<DataBase3CX_General_CXYB>().ReadExcel(filePath, 1, 6, 0, 0, 0, true);
            if (list == null)
            {
                return list;
            }
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if ((string.IsNullOrEmpty(item.CPBM) || (item.CPBM.Contains("合") && item.CPBM.Contains("计"))) || (string.IsNullOrEmpty(item.GH) && string.IsNullOrEmpty(item.UserCode) && string.IsNullOrEmpty(item.UserName)))
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
                item.CreateUser = Program.User.ToString();
                item.CreateTime = Program.NowTime;
                item.TheYear = dtp.Value.Year;
                item.TheMonth = dtp.Value.Month;
                //var code = list[i].UserCode;
                //if (code.StartsWith("M") && code.Length == "M16624".Length)
                //{
                //    var gh = list[i].GH;
                //    if (gh.Length < 3)
                //    {
                //        gh = gh.PadLeft(3, '0');
                //        list[i].GH = gh;
                //    }
                //}
            }
            return list;
        }

        private void SaveImportData(string filePath)
        {
            Enabled = false;
            var list = Import(filePath);
            if (list != null)
            {
                if (new BaseDal<DataBase3CX_General_CXYB>().Add(list) > 0)
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
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件数据错误或没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<DataBase3CX_General_CXYB> list = dgv.DataSource as List<DataBase3CX_General_CXYB>;
                if (new ExcelHelper<DataBase3CX_General_CXYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具——成型月报.xlsx", saveFileDlg.FileName, list, 1, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var max = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderByDescending(t => t.No).FirstOrDefault();
            string maxId;
            if (max == null)
            {
                maxId = 1.ToString();
            }
            else
            {
                maxId = (Convert.ToInt32(max.No) + 1).ToString();
            }
            DataBase3CX_General_CXYB DataBase3CX_General_CXYB = new DataBase3CX_General_CXYB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxId };
            FrmModify<DataBase3CX_General_CXYB> frm = new FrmModify<DataBase3CX_General_CXYB>(DataBase3CX_General_CXYB, header, OptionType.Add, Text, 5, 0);
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

        private void Frm3CX_General_CXYB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_General_CXYB_DragDrop(object sender, DragEventArgs e)
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
                var DataBase3CX_General_CXYB = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_General_CXYB;
                if (DataBase3CX_General_CXYB != null)
                {
                    FrmModify<DataBase3CX_General_CXYB> frm = new FrmModify<DataBase3CX_General_CXYB>(DataBase3CX_General_CXYB, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var item = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_General_CXYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (item != null)
                    {
                        if (item.CPBM.Equals("合计"))
                        {
                            if (MessageBox.Show("确定要全部删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3CX_General_CXYB>;
                                list.RemoveAt(0);
                                foreach (var i in list)
                                {
                                    new BaseDal<DataBase3CX_General_CXYB>().Delete(i);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3CX_General_CXYB> frm = new FrmModify<DataBase3CX_General_CXYB>(item, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }
    }
}