using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_02_ZY : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$类型$序号$产品名称$颜色$开窑量$产品等级_合格$产品等级_等外$产品等级_残$产品等级_冷补修$产品等级_回烧$产品等级_磨$产品等级_磨修$合格率$原料缺陷_铜脏$原料缺陷_铁脏$原料缺陷_泥脏$原料缺陷_小计$缺陷率$成型缺陷_成走$成型缺陷_内裂$成型缺陷_裂底$成型缺陷_裂体$成型缺陷_裂眼$成型缺陷_修糙$成型缺陷_棕眼$成型缺陷_成脏$成型缺陷_泥绺$成型缺陷_坯泡$成型缺陷_崩渣$成型缺陷_冲刷$成型缺陷_卡球$成型缺陷_吹脏$成型缺陷_小计$缺陷率$修挡板$修检_裂体$修检_修糙$修检_棕眼$修检_成脏$修检_卡球$修检_滚釉$修检_爆釉$修检_坯渣$修检_小计$缺陷率$喷釉缺陷_滚釉$喷釉缺陷_滚釉釉薄$喷釉缺陷_滚釉釉厚$喷釉缺陷_滚釉釉缕$喷釉缺陷_滚釉爆釉$喷釉缺陷_滚釉坯渣$喷釉缺陷_滚釉坯磕$喷釉缺陷_滚釉釉脏$喷釉缺陷_滚釉刮边$喷釉缺陷_滚釉釉粘$喷釉缺陷_滚釉坯脏$喷釉缺陷_滚釉小计$缺陷率$回烧缺陷_修补不合格$回烧缺陷_漏补$回烧缺陷_缺陷$回烧缺陷_小计$缺陷率$装窑缺陷_装走$装窑缺陷_装粘$装窑缺陷_装磕$装窑缺陷_装脏$装窑缺陷_刮边$装窑缺陷_小计$缺陷率$开窑缺陷_出磕$开窑缺陷_划釉$开窑缺陷_小计$缺陷率$烧窑缺陷_桔釉$烧窑缺陷_氧化$烧窑缺陷_烧泡$烧窑缺陷_烧裂$烧窑缺陷_烧生$烧窑缺陷_窑脏$烧窑缺陷_风惊$烧窑缺陷_小计$缺陷率$标污$烧成缺陷率$吹脏$缺陷率$崩脏$缺陷率".Split('$');
        private string[] headerXCG = "创建日期$创建人$年$月$总窑开窑量$烧成总缺陷率$开窑缺陷率$实际缺陷率$目标缺陷$考核金额".Split('$');
        private string[] headerKYG = "创建日期$创建人$年$月$开窑量$总窑月报的开窑缺陷率$实际一级率$目标一级率$开窑工定员$考核金额".Split('$');

        public Frm3SC_02_ZY()
        {
            InitializeComponent();
        }

        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                return;
            }
            System.Reflection.FieldInfo[] fieldInfo = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ContextMenuStrip> contextMenuStrip = new List<ContextMenuStrip>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfo[i].FieldType == typeof(ButtonX))
                {
                    ((ButtonX)fieldInfo[i].GetValue(this)).Enabled = false;
                    //Controls.Find(fieldInfo[i].Name, true)[0].Enabled = false;
                    continue;
                }

                if (fieldInfo[i].FieldType == typeof(ContextMenuStrip))
                {
                    var tmp = (ContextMenuStrip)fieldInfo[i].GetValue(this);
                    contextMenuStrip.Add(tmp);
                    foreach (var item in tmp.Items)
                    {
                        if (item is ToolStripMenuItem t)
                        {
                            t.Enabled = false;
                        }
                    }
                }
            }

            var dic = MenuBarHelper.FrmControlEnable(Program.User.Code);
            foreach (var item in dic.Keys)
            {
                var cons = Controls.Find(item, true);
                if (cons.Length > 0)
                {
                    cons[0].Enabled = dic[item];
                    continue;
                }
                foreach (var contextMs in contextMenuStrip)
                {
                    foreach (var item2 in contextMs.Items)
                    {
                        if (item2 is ToolStripMenuItem x && x.Name == item)
                        {
                            x.Enabled = dic[item];
                        }
                    }
                }
            }
        }

        private void Frm3SC_02_ZY_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string cpmc)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheType == 1 && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            dgv.DataSource = null;
            dgv.DataSource = datas;
            for (int i = 0; i < 7; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[8].Visible = false;

            dgv.Columns[9].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgvXCG(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerXCG.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerXCG[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void RefDgvKYG(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerKYG.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerKYG[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        private void btnSearchXCG_Click(object sender, EventArgs e)
        {
            RefDgvXCG(dtp.Value);
        }

        private void btnSearchKYG_Click(object sender, EventArgs e)
        {
            RefDgvKYG(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx");
        }

        private DataBase3SC_0203_ZY_HS_XCGKH Recount_XCG(List<DataBase3SC_0203_ZY_HS> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【吸尘工考核】！");
                return null;
            }
            DataBase3SC_0203_ZY_HS_XCGKH xCGKH = new DataBase3SC_0203_ZY_HS_XCGKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };

            try
            {
                var xj = list.Where(t => t.CPMC == "小计").OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                xCGKH.ZYKYL = xj.KYL;
                xCGKH.SCZQXL = (decimal.TryParse(xj.SCQXL, out decimal scqxl) ? scqxl / 100 : 0M).ToString();
                xCGKH.KYQXL = (decimal.TryParse(xj.KYQX_QXL, out decimal kyqxl) ? kyqxl / 100 : 0m).ToString();
                xCGKH.SJQXL = (scqxl / 100 - kyqxl / 100).ToString();
                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "吸尘工");
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "吸尘工");
                xCGKH.MBQX = dataDay.GRKHZB1;
                decimal.TryParse(xCGKH.ZYKYL, out decimal cj);
                var cm = scqxl / 100 - kyqxl / 100;
                decimal.TryParse(xCGKH.MBQX, out decimal cn);
                decimal.TryParse(dataMonth.MoneyKH, out decimal d500);
                decimal.TryParse(dataDay.GRFKDJ1, out decimal d02);
                if (cj == 0)
                {
                    xCGKH.KHJE = 0.ToString();
                }
                else
                {
                    if (cm <= cn)
                    {
                        xCGKH.KHJE = d500.ToString();
                    }
                    else
                    {
                        if (d500 - cj * (cm - cn) * d02 < 0)
                        {
                            xCGKH.KHJE = 0.ToString();
                        }
                        else
                        {
                            xCGKH.KHJE = (d500 - cj * (cm - cn) * d02).ToString();
                        }
                    }
                }
                return xCGKH;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private DataBase3SC_0203_ZY_HS_KYGKH Recount_KYG(List<DataBase3SC_0203_ZY_HS> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【开窑工考核】！");
                return null;
            }
            DataBase3SC_0203_ZY_HS_KYGKH kYGKH = new DataBase3SC_0203_ZY_HS_KYGKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };
            try
            {
                var xj = list.Where(t => t.CPMC == "小计").OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                kYGKH.KYL = xj.KYL;
                kYGKH.ZYYBDKYQXL = (decimal.TryParse(xj.KYQX_QXL, out decimal zYYBDKYQXL) ? zYYBDKYQXL / 100 : 0M).ToString();
                kYGKH.SJYJL = (1 - zYYBDKYQXL / 100).ToString();
                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "开窑工");
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "开窑工");
                kYGKH.MBYJL = dataDay.GRKHZB1;
                var baseDG = new BaseDal<BaseHeadcount>().Get(t => t.Factory == "三厂" && t.DeptName == "烧成车间" && t.TheMonth == dtp.Value.Month && t.TheYear == dtp.Value.Year && t.Name == "开窑工");
                kYGKH.KYGDY = baseDG.UserCount;
                decimal.TryParse(kYGKH.KYL, out decimal cj);
                decimal.TryParse(kYGKH.SJYJL, out decimal cl);
                decimal.TryParse(kYGKH.MBYJL, out decimal cm);
                decimal.TryParse(dataDay.GRFKDJ1, out decimal d04);
                if (cj == 0)
                {
                    kYGKH.KHJE = 0.ToString();
                }
                else
                {
                    kYGKH.KHJE = ((cl - cm) * cj * d04).ToString();
                }
                return kYGKH;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
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
                try
                {
                    List<DataBase3SC_0203_ZY_HS> list = new ExcelHelper<DataBase3SC_0203_ZY_HS>().ReadExcel(openFileDlg.FileName, 4, 7, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        item.TheType = 1;
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase3SC_0203_ZY_HS>().Add(list) > 0)
                    {
                        btnRecount.PerformClick();
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_0203_ZY_HS> list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS>;
                if (new ExcelHelper<DataBase3SC_0203_ZY_HS>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx", saveFileDlg.FileName, list, 4, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_0203_ZY_HS DataBase3SC_0203_ZY_HS = new DataBase3SC_0203_ZY_HS() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString(), TheType = 1 };
            FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Add, Text, 6, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_0203_ZY_HS> list = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.TheType == 1).ToList();

            new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_XCGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
            new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_KYGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);

            if (new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().Add(Recount_XCG(list)) > 0 && new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().Add(Recount_KYG(list)) > 0)
            {
                MessageBox.Show("吸尘工、开窑工考核，计算成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportExcelXCG_Click(object sender, EventArgs e)
        {
            btnSearchXCG.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_0203_ZY_HS_XCGKH> list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS_XCGKH>;
                if (new ExcelHelper<DataBase3SC_0203_ZY_HS_XCGKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——吸尘工考核.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnExportExcelKYG_Click(object sender, EventArgs e)
        {
            btnSearchKYG.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_0203_ZY_HS_KYGKH> list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS_KYGKH>;
                if (new ExcelHelper<DataBase3SC_0203_ZY_HS_KYGKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——开窑工考核和计件.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_0203_ZY_HS DataBase3SC_0203_ZY_HS)
                {
                    FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Modify, Text, 6, 0);
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
                var DataBase3SC_0203_ZY_HS = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_0203_ZY_HS;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_0203_ZY_HS != null)
                    {
                        FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Delete, Text, 6, 0);
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
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS>;
                foreach (var item in list)
                {
                    new BaseDal<DataBase3SC_0203_ZY_HS>().Delete(item);
                }
                new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_XCGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
                new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_KYGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }
    }
}