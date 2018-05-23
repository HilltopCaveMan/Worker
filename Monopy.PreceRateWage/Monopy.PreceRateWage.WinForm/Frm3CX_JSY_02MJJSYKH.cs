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
    public partial class Frm3CX_JSY_02MJJSYKH : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$品种$干燥合格率目标$干燥合格率实际$目标缺陷率$实际缺陷率$干燥考核金额$缺陷考核金额$总考核金额".Split('$');

        public Frm3CX_JSY_02MJJSYKH()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3CX_JSY_02MJJSYKH_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_JSY_02MJJSYKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_JSY_02MJJSYKH(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——技术员——成型面具技术员考核.xlsx");
        }

        private bool Recount(List<DataBase3CX_JSY_02MJJSYKH> list)
        {
            try
            {
                var listYb = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                if (listYb.Count == 0)
                {
                    MessageBox.Show("没有本月的成型月报，无法导入，需先导入月报！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listDataDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "技术员" && t.Classification == "面具技术员").ToList();
                if (listDataDay.Count == 0)
                {
                    MessageBox.Show("指标数据没有【面具技术员】,无法导入，请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                foreach (var item in list)
                {
                    decimal kyl = listYb.Where(t => t.CPMC == item.PZ).ToList().Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0m);
                    decimal yjp = listYb.Where(t => t.CPMC == item.PZ).ToList().Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0m);
                    decimal e3 = 1M - yjp / kyl;
                    item.SJQXL = e3.ToString();
                    var itemDataDay = listDataDay.Where(t => t.TypesName == item.PZ).FirstOrDefault();
                    decimal.TryParse(itemDataDay.GRKHZB1, out decimal d240);
                    decimal.TryParse(itemDataDay.GRJSB1, out decimal d1);
                    decimal.TryParse(itemDataDay.GRJLDJ1, out decimal d20);
                    decimal.TryParse(itemDataDay.KHJESFD, out decimal d300s);
                    decimal.TryParse(itemDataDay.KHJEXFD, out decimal d300x);
                    decimal.TryParse(item.GZHGLMB, out decimal b3);
                    decimal.TryParse(item.GZHGLSJ, out decimal c3);
                    decimal.TryParse(item.MBQXL, out decimal d3);
                    if ((d240 + (c3 - b3) / d1 * d20) >= d300s)
                    {
                        item.GZKHJE = d300s.ToString();
                    }
                    else
                    {
                        if ((d240 + (c3 - b3) / d1 * d20) <= 0)
                        {
                            item.GZKHJE = 0.ToString();
                        }
                        else
                        {
                            item.GZKHJE = (d240 + (c3 - b3) / d1 * d20).ToString();
                        }
                    }

                    if ((d300x - d300x * (e3 - d3) / d3) >= d300x)
                    {
                        item.QXKHJE = d300x.ToString();
                    }
                    else
                    {
                        if ((d300x - d300x * (e3 - d3) / d3) <= 0)
                        {
                            item.QXKHJE = 0.ToString();
                        }
                        else
                        {
                            item.QXKHJE = (d300x - d300x * (e3 - d3) / d3).ToString();
                        }
                    }
                    item.Money = ((decimal.TryParse(item.GZKHJE, out decimal f3) ? f3 : 0M) + (decimal.TryParse(item.QXKHJE, out decimal g3) ? g3 : 0M)).ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Import(string fileName)
        {
            List<DataBase3CX_JSY_02MJJSYKH> list = new ExcelHelper<DataBase3CX_JSY_02MJJSYKH>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].PZ) || (list[i].PZ.Contains("合计")))
                {
                    list.RemoveAt(i);
                    i--;
                }
                else
                {
                    list[i].CreateUser = Program.User.ToString();
                    list[i].CreateTime = Program.NowTime;
                    list[i].TheYear = dtp.Value.Year;
                    list[i].TheMonth = dtp.Value.Month;
                }
            }
            if (Recount(list) && new BaseDal<DataBase3CX_JSY_02MJJSYKH>().Add(list) > 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Enabled = true;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Import(openFileDlg.FileName);
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
                List<DataBase3CX_JSY_02MJJSYKH> list = dgv.DataSource as List<DataBase3CX_JSY_02MJJSYKH>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3CX_JSY_02MJJSYKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——技术员——成型面具技术员考核.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                list.Remove(hj);
                list.Insert(0, hj);
                Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase3CX_JSY_02MJJSYKH DataBase3CX_JSY_02MJJSYKH = new DataBase3CX_JSY_02MJJSYKH() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3CX_JSY_02MJJSYKH> frm = new FrmModify<DataBase3CX_JSY_02MJJSYKH>(DataBase3CX_JSY_02MJJSYKH, header, OptionType.Add, Text, 5, 4);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_JSY_02MJJSYKH> list = new BaseDal<DataBase3CX_JSY_02MJJSYKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_JSY_02MJJSYKH>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3CX_JSY_02MJJSYKH = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_JSY_02MJJSYKH;
                if (DataBase3CX_JSY_02MJJSYKH != null)
                {
                    if (DataBase3CX_JSY_02MJJSYKH.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3CX_JSY_02MJJSYKH> frm = new FrmModify<DataBase3CX_JSY_02MJJSYKH>(DataBase3CX_JSY_02MJJSYKH, header, OptionType.Modify, Text, 5, 4);
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
                var DataBase3CX_JSY_02MJJSYKH = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_JSY_02MJJSYKH;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_JSY_02MJJSYKH != null)
                    {
                        if (DataBase3CX_JSY_02MJJSYKH.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3CX_JSY_02MJJSYKH>;
                                dgv.DataSource = null;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3CX_JSY_02MJJSYKH>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3CX_JSY_02MJJSYKH> frm = new FrmModify<DataBase3CX_JSY_02MJJSYKH>(DataBase3CX_JSY_02MJJSYKH, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void Frm3CX_JSY_02MJJSYKH_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_JSY_02MJJSYKH_DragDrop(object sender, DragEventArgs e)
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
            Import(path);
        }
    }
}