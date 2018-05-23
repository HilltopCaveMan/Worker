using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_06_HSJYADL : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工号$类别编码$类别名称$员工编码$姓名$工厂$工段编码$工段名称$开窑量$一级品$修补不合格_降级$修补不合格_破损$漏补_降级$漏补_破损$缺陷_降级$缺陷_破损".Split('$');

        public Frm3SC_06_HSJYADL()
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

        private void Frm3SC_06_HSJYADL_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
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
            var datas = new BaseDal<DataBase3SC_06_HSJYADL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_06_HSJYADL(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[13].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ColumnHeadersVisible = true;
            dgv.RowHeadersVisible = true;
            dgv.ContextMenuStrip = contextMenuStrip1;
            dgv.ClearSelection();
        }

        private void RefDgv(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_06_HSJYADL_SCBCGKH>().Get(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month);
            if (datas == null)
            {
                return;
            }
            var list = new List<object>();
            var x1 = new { a1 = "回烧窑开窑量", a2 = "单价", a3 = "", a4 = "", a5 = "金额" };
            list.Add(x1);
            var x2 = new { a1 = datas.HSYKYL, a2 = datas.DJ, a3 = "", a4 = "", a5 = datas.JE1 };
            list.Add(x2);
            var x3 = new { a1 = "回烧窑开窑量", a2 = "一级品", a3 = "实际缺陷率", a4 = "指标", a5 = "金额" };
            list.Add(x3);
            var x4 = new { a1 = datas.HSYKYL, a2 = datas.YJP, a3 = datas.SJQXL, a4 = datas.ZB, a5 = datas.JE2 };
            list.Add(x4);
            var x7 = new { a1 = "合计", a2 = "", a3 = "", a4 = "", a5 = ((decimal.TryParse(datas.JE1, out decimal je1) ? je1 : 0M) + (decimal.TryParse(datas.JE2, out decimal je2) ? je2 : 0M)).ToString() };
            list.Add(x7);

            dgv.DataSource = list;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtGH.Text, txtUserCode.Text, txtUserName.Text);
        }

        private void btnSearchBCG_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——回烧检验按大类.xlsx");
        }

        private DataBase3SC_06_HSJYADL_SCBCGKH Recount_XCG(List<DataBase3SC_06_HSJYADL> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【烧成补瓷工考核】！");
                return null;
            }
            DataBase3SC_06_HSJYADL_SCBCGKH sCBCGKH = new DataBase3SC_06_HSJYADL_SCBCGKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };

            try
            {
                var xj = MyDal.GetTotalDataBase3SC_06_HSJYADL(list);
                sCBCGKH.HSYKYL = xj.KYL;
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "烧成补瓷工");
                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "烧成补瓷工");
                sCBCGKH.DJ = dataDay.UnitPrice;
                decimal.TryParse(sCBCGKH.HSYKYL, out decimal q1);
                decimal.TryParse(sCBCGKH.DJ, out decimal r1);
                sCBCGKH.JE1 = (q1 * r1).ToString();
                sCBCGKH.YJP = xj.YJP;
                decimal.TryParse(sCBCGKH.YJP, out decimal r2);
                sCBCGKH.SJQXL = (1 - r2 / q1).ToString();
                sCBCGKH.ZB = dataDay.GRKHZB1;
                decimal.TryParse(sCBCGKH.SJQXL, out decimal s2);
                decimal.TryParse(sCBCGKH.ZB, out decimal t2);
                decimal.TryParse(dataDay.GRJLDJ1, out decimal d1);
                decimal.TryParse(dataDay.KHJESFD, out decimal d800);

                if (s2 >= t2)
                {
                    sCBCGKH.JE2 = 0.ToString();
                }
                else
                {
                    if (q1 * (t2 - s2) * d1 >= d800)
                    {
                        sCBCGKH.JE2 = d800.ToString();
                    }
                    else
                    {
                        sCBCGKH.JE2 = (q1 * (t2 - s2) * d1).ToString();
                    }
                }

                return sCBCGKH;
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
                    List<DataBase3SC_06_HSJYADL> list = new ExcelHelper<DataBase3SC_06_HSJYADL>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string gh = string.Empty;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (item.GH.Contains("工号"))
                        {
                            gh = item.GH.Split(':')[1].Trim();
                            list.RemoveAt(i);
                            i--;
                            continue;
                        }
                        if (string.IsNullOrEmpty(item.LBBM) || string.IsNullOrEmpty(item.LBMC) || string.IsNullOrEmpty(item.UserCode))
                        {
                            list.RemoveAt(i);
                            i--;
                            continue;
                        }
                        item.GH = gh;
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase3SC_06_HSJYADL>().Add(list) > 0)
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
                List<DataBase3SC_06_HSJYADL> list = dgv.DataSource as List<DataBase3SC_06_HSJYADL>;
                if (new ExcelHelper<DataBase3SC_06_HSJYADL>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——回烧检验按大类.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_06_HSJYADL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_06_HSJYADL DataBase3SC_06_HSJYADL = new DataBase3SC_06_HSJYADL() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3SC_06_HSJYADL> frm = new FrmModify<DataBase3SC_06_HSJYADL>(DataBase3SC_06_HSJYADL, header, OptionType.Add, Text, 6, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            new BaseDal<DataBase3SC_06_HSJYADL_SCBCGKH>().ExecuteSqlCommand("delete from DataBase3SC_06_HSJYADL_SCBCGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
            List<DataBase3SC_06_HSJYADL> list = new BaseDal<DataBase3SC_06_HSJYADL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();

            if (list.Count > 0)
            {
                if (new BaseDal<DataBase3SC_06_HSJYADL_SCBCGKH>().Add(Recount_XCG(list)) > 0)
                {
                    MessageBox.Show("烧成补瓷工考核，计算成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("没有数据无法重新计算", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportExcelBCG_Click(object sender, EventArgs e)
        {
            btnSearchBCG.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var data = new BaseDal<DataBase3SC_06_HSJYADL_SCBCGKH>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                try
                {
                    FileStream file = File.OpenRead(Application.StartupPath + "\\Excel\\模板三厂——烧成——烧成补瓷工考核.xlsx");
                    IWorkbook workbook = WorkbookFactory.Create(file);
                    ICellStyle style = workbook.CreateCellStyle();
                    style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    ISheet sheet = workbook.GetSheetAt(0);

                    var tCell = sheet.GetRow(0).GetCell(0);
                    tCell.CellStyle = style;
                    string sCell = ExcelHelper.GetCellValue(tCell).ToUpper();
                    sCell = sCell.Replace("XXXX", dtp.Value.Year.ToString());
                    sCell = sCell.Replace("XX", dtp.Value.Month.ToString());
                    tCell.SetCellValue(sCell);

                    tCell = sheet.GetRow(2).GetCell(0);
                    tCell.SetCellValue(data.HSYKYL);
                    tCell = sheet.GetRow(2).GetCell(1);
                    tCell.SetCellValue(data.DJ);
                    tCell = sheet.GetRow(2).GetCell(4);
                    tCell.SetCellValue(data.JE1);

                    tCell = sheet.GetRow(4).GetCell(0);
                    tCell.SetCellValue(data.HSYKYL);
                    tCell = sheet.GetRow(4).GetCell(1);
                    tCell.SetCellValue(data.YJP);
                    tCell = sheet.GetRow(4).GetCell(2);
                    tCell.SetCellValue(data.SJQXL);
                    tCell = sheet.GetRow(4).GetCell(3);
                    tCell.SetCellValue(data.ZB);
                    tCell = sheet.GetRow(4).GetCell(4);
                    tCell.SetCellValue(data.JE2);

                    tCell = sheet.GetRow(5).GetCell(4);
                    tCell.SetCellValue(((decimal.TryParse(data.JE1, out decimal je1) ? je1 : 0M) + (decimal.TryParse(data.JE2, out decimal je2) ? je2 : 0M)).ToString());

                    FileStream sw = File.Create(saveFileDlg.FileName);
                    workbook.Write(sw);
                    sw.Close();
                    sw.Dispose();
                    file.Close();
                    file.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出错误，请检查后，再试！" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }

                //if (new ExcelHelper<DataBase3SC_0203_ZY_HS_KYGKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——装窑工小考核.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
                //{
                if (MessageBox.Show("导出成功，立即打开？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Process.Start(saveFileDlg.FileName);
                }
                //}
                //else
                //{
                //    MessageBox.Show("导出错误，请检查后，再试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_06_HSJYADL DataBase3SC_06_HSJYADL)
                {
                    FrmModify<DataBase3SC_06_HSJYADL> frm = new FrmModify<DataBase3SC_06_HSJYADL>(DataBase3SC_06_HSJYADL, header, OptionType.Modify, Text, 6, 0);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_06_HSJYADL DataBase3SC_06_HSJYADL)
                {
                    if (DataBase3SC_06_HSJYADL.No.Equals("合计"))
                    {
                        if (MessageBox.Show("要全部删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            var list = dgv.DataSource as List<DataBase3SC_06_HSJYADL>;
                            dgv.DataSource = null;
                            Enabled = false;
                            list.RemoveAt(0);
                            foreach (var item in list)
                            {
                                new BaseDal<DataBase3SC_06_HSJYADL>().Delete(item);
                            }
                            btnRecount.PerformClick();
                            Enabled = true;
                            btnSearch.PerformClick();
                        }
                        return;
                    }
                    if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        FrmModify<DataBase3SC_06_HSJYADL> frm = new FrmModify<DataBase3SC_06_HSJYADL>(DataBase3SC_06_HSJYADL, header, OptionType.Delete, Text, 6, 0);
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