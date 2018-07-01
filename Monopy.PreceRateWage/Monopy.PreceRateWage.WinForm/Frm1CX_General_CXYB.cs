using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm1CX_General_CXYB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$产品编码$工号$员工编码$姓名$产品名称$工厂编码$开窑量$一级品$一级率$成走缺损率$内裂缺损率$裂底缺损率$裂体缺损率$裂眼缺损率$修糙缺损率$棕眼缺损率$成脏缺损率$泥绺缺损率$坯泡缺损率$崩渣缺损率$冲刷缺损率$卡球缺损率$吹脏缺损率$降级$破损".Split('$');

        public Frm1CX_General_CXYB()
        {
            InitializeComponent();
        }

        private void Frm1CX_General_CXYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gh, string userCode, string userName)
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
            var datas = new BaseDal<DataBase1CX_General_CXYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1CX_General_CXYB(datas));
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
            RefDgv(dtp.Value, txtGH.Text, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具——成型月报.xlsx");
        }

        private void Import(string filePath)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<DataBase1CX_General_CXYB> list = new List<DataBase1CX_General_CXYB>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i;
                        string lb = string.Empty;
                        for (i = 1; i <= sheet.LastRowNum; i++)
                        {
                            bool sign = true;
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            if (ExcelHelper.GetCellValue(row.GetCell(0)).Contains("产品编码") && lb != ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1])
                            {
                                sign = false;
                                lb = ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1];
                            }
                            if (sign)
                            {
                                DataBase1CX_General_CXYB t = new DataBase1CX_General_CXYB { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                                t.No = (i - 2).ToString();
                                t.CPBM = lb;
                                if (string.IsNullOrEmpty(ExcelHelper.GetCellValue(row.GetCell(2))))
                                {
                                    if (string.IsNullOrEmpty(ExcelHelper.GetCellValue(sheet.GetRow(i - 1).GetCell(2))))
                                    {
                                        t.GCBM = "合计";
                                        t.CPBM = string.Empty;
                                    }
                                    else
                                    {
                                        t.GCBM = lb + "合计";
                                    }
                                }
                                else
                                {
                                    t.GCBM = ExcelHelper.GetCellValue(row.GetCell(5));
                                }
                                t.GH = ExcelHelper.GetCellValue(row.GetCell(1));
                                t.UserCode = ExcelHelper.GetCellValue(row.GetCell(2));
                                t.UserName = ExcelHelper.GetCellValue(row.GetCell(3));
                                t.CPMC = ExcelHelper.GetCellValue(row.GetCell(4));
                                t.KYL = ExcelHelper.GetCellValue(row.GetCell(6));
                                t.YJP = ExcelHelper.GetCellValue(row.GetCell(7));
                                t.YJL = ExcelHelper.GetCellValue(row.GetCell(8));
                                t.QX01 = ExcelHelper.GetCellValue(row.GetCell(9));
                                t.QX02 = ExcelHelper.GetCellValue(row.GetCell(10));
                                t.QX03 = ExcelHelper.GetCellValue(row.GetCell(11));
                                t.QX04 = ExcelHelper.GetCellValue(row.GetCell(12));
                                t.QX05 = ExcelHelper.GetCellValue(row.GetCell(13));
                                t.QX06 = ExcelHelper.GetCellValue(row.GetCell(14));
                                t.QX07 = ExcelHelper.GetCellValue(row.GetCell(15));
                                t.QX08 = ExcelHelper.GetCellValue(row.GetCell(16));
                                t.QX09 = ExcelHelper.GetCellValue(row.GetCell(17));
                                t.QX10 = ExcelHelper.GetCellValue(row.GetCell(18));
                                t.QX11 = ExcelHelper.GetCellValue(row.GetCell(19));
                                t.QX12 = ExcelHelper.GetCellValue(row.GetCell(20));
                                t.QX13 = ExcelHelper.GetCellValue(row.GetCell(21));
                                t.QX14 = ExcelHelper.GetCellValue(row.GetCell(22));
                                t.JJ = ExcelHelper.GetCellValue(row.GetCell(23));
                                t.PS = ExcelHelper.GetCellValue(row.GetCell(24));

                                list.Add(t);
                            }
                        }
                    }
                    if (new BaseDal<DataBase1CX_General_CXYB>().Add(list) > 0)
                    {
                        btnSearch.PerformClick();
                        MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveImportData(string filePath)
        {
            Import(filePath);
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
                List<DataBase1CX_General_CXYB> list = dgv.DataSource as List<DataBase1CX_General_CXYB>;
                if (new ExcelHelper<DataBase1CX_General_CXYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具——成型月报.xlsx", saveFileDlg.FileName, list, 1, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var max = new BaseDal<DataBase1CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderByDescending(t => t.No).FirstOrDefault();
            string maxId;
            if (max == null)
            {
                maxId = 1.ToString();
            }
            else
            {
                maxId = (Convert.ToInt32(max.No) + 1).ToString();
            }
            DataBase1CX_General_CXYB DataBase1CX_General_CXYB = new DataBase1CX_General_CXYB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxId };
            FrmModify<DataBase1CX_General_CXYB> frm = new FrmModify<DataBase1CX_General_CXYB>(DataBase1CX_General_CXYB, header, OptionType.Add, Text, 5, 0);
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

        private void Frm1CX_General_CXYB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm1CX_General_CXYB_DragDrop(object sender, DragEventArgs e)
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
                var DataBase1CX_General_CXYB = dgv.SelectedRows[0].DataBoundItem as DataBase1CX_General_CXYB;
                if (DataBase1CX_General_CXYB != null)
                {
                    FrmModify<DataBase1CX_General_CXYB> frm = new FrmModify<DataBase1CX_General_CXYB>(DataBase1CX_General_CXYB, header, OptionType.Modify, Text, 5, 0);
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
                var item = dgv.SelectedRows[0].DataBoundItem as DataBase1CX_General_CXYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (item != null)
                    {
                        if (item.CPBM.Equals("合计"))
                        {
                            if (MessageBox.Show("确定要全部删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase1CX_General_CXYB>;
                                list.RemoveAt(0);
                                foreach (var i in list)
                                {
                                    new BaseDal<DataBase1CX_General_CXYB>().Delete(i);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase1CX_General_CXYB> frm = new FrmModify<DataBase1CX_General_CXYB>(item, header, OptionType.Delete, Text, 5, 0);
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