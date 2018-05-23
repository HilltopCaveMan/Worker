using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_GYMJ_02_PMCBJYB : Office2007Form
    {
        public Frm3CX_GYMJ_02_PMCBJYB()
        {
            InitializeComponent();
        }

        private void Frm3CX_GYMJ_02_PMCBJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtGH.Text = string.Empty;
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private List<DataBase3CX_GYMJ_02_PMCBJYB> ImportFile(string filePathName)
        {
            var list = new List<DataBase3CX_GYMJ_02_PMCBJYB>();
            try
            {
                using (var fs = new FileStream(filePathName, FileMode.Open, FileAccess.Read))
                {
                    var workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    int i, j;
                    var rowTitle = sheet.GetRow(1);
                    for (i = 2; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        DataBase3CX_GYMJ_02_PMCBJYB t = new DataBase3CX_GYMJ_02_PMCBJYB { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = (i - 1).ToString() };
                        t.Factory = ExcelHelper.GetCellValue(row.GetCell(0));
                        t.GH = ExcelHelper.GetCellValue(row.GetCell(1));
                        t.GD = ExcelHelper.GetCellValue(row.GetCell(2));
                        t.UserCode = ExcelHelper.GetCellValue(row.GetCell(3));
                        t.UserName = ExcelHelper.GetCellValue(row.GetCell(4));
                        t.HJ = ExcelHelper.GetCellValue(row.GetCell(5));
                        t.DL = ExcelHelper.GetCellValue(row.GetCell(6));
                        t.DLJE = ExcelHelper.GetCellValue(row.GetCell(7));
                        t.JE = ExcelHelper.GetCellValue(row.GetCell(8));
                        if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                        {
                            continue;
                        }
                        if (t.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }

                        t.Childs = new List<DataBase3CX_GYMJ_02_PMCBJYB_Child>();
                        for (j = 9; j < row.LastCellNum; j++)
                        {
                            var x = new DataBase3CX_GYMJ_02_PMCBJYB_Child { Id = Guid.NewGuid() };
                            x.No = j - 8;
                            x.Name = ExcelHelper.GetCellValue(rowTitle.GetCell(j));
                            x.Count = ExcelHelper.GetCellValue(row.GetCell(j));
                            if (i == 2 && !string.IsNullOrEmpty(x.Name))
                            {
                                t.Childs.Add(x);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(x.Count) && !string.IsNullOrEmpty(x.Name))
                                {
                                    t.Childs.Add(x);
                                }
                            }
                        }
                        list.Add(t);
                    }
                }
                if (new BaseDal<DataBase3CX_GYMJ_02_PMCBJYB>().Add(list) > 0)
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
                MessageBox.Show($"导入失败，详细错误为：{ex.Message}");
            }
            return list;
        }

        private DataTable GetData(List<DataBase3CX_GYMJ_02_PMCBJYB> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            DataTable dt = DataTableHelper.GetDataTable(list);
            dt.Columns.Remove("Childs");
            for (int i = 0; i < list.Count; i++)
            {
                foreach (var item in list[i].Childs.OrderBy(t => t.No))
                {
                    string name = item.Name;
                    if (!dt.Columns.Contains(name))
                    {
                        dt.Columns.Add(name, typeof(string));
                    }
                    dt.Rows[i][name] = item.Count;
                }
            }
            DataRow dr_sum = dt.NewRow();
            decimal[] tp = new decimal[dt.Columns.Count - 11];
            for (int j = 11; j < dt.Columns.Count; j++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tp[j - 11] += (decimal.TryParse(dt.Rows[i][j].ToString(), out decimal m) ? m : 0M);
                }
                dr_sum[j] = tp[j - 11];
            }
            dr_sum["factory"] = "合计";
            dt.Rows.InsertAt(dr_sum, 0);
            return dt;
        }

        private void RefGrid(List<DataBase3CX_GYMJ_02_PMCBJYB> list)
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
            dgv.DataSource = null;
            DataTable dt = GetData(list);
            if (dt == null)
            {
                return;
            }
            string[] header = "id$时间$操作人$年$月$No$工厂$工号$工段$员工编码$姓名$合计$大裂$大裂金额$金额".Split('$');
            dgv.DataSource = dt;
            for (int i = 0; i < header.Length; i++)
            {
                if (i < 6)
                {
                    dgv.Columns[i].Visible = false;
                }
                else
                {
                    dgv.Columns[i].HeaderText = header[i];
                }
            }
            dgv.Columns[14].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.ClearSelection();
        }

        private void Frm3CX_GYMJ_02_PMCBJYB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_GYMJ_02_PMCBJYB_DragDrop(object sender, DragEventArgs e)
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
            var list = ImportFile(path);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase3CX_GYMJ_02_PMCBJYB>().GetList(t => t.TheYear.Equals(dtp.Value.Year) && t.TheMonth.Equals(dtp.Value.Month) && t.GH.Contains(txtGH.Text) && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : decimal.MaxValue).ToList();
            RefGrid(list);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型高压面具——PMC半检月报.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                ImportFile(openFileDlg.FileName);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            //经过测试，使用下面一行代码速度巨慢。
            //DataTable dt = dgv.DataSource as DataTable;
            var list = new BaseDal<DataBase3CX_GYMJ_02_PMCBJYB>().GetList(t => t.TheYear.Equals(dtp.Value.Year) && t.TheMonth.Equals(dtp.Value.Month) && t.GH.Contains(txtGH.Text) && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : decimal.MaxValue).ToList();
            DataTable dt = GetData(list);

            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < 6; i++)
            {
                dt.Columns.RemoveAt(0);
            }
            string[] header = "工厂$工号$工段$员工编码$姓名$合计$大裂$大裂金额$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = $"梦牌三厂成型--高压面具--PMC半检月报-{dtp.Value.Year}-{dtp.Value.Month}.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "成检计件", "三工厂" + dtp.Value.ToString("yyyy年MM月dd日") + "成型车间高压面具PMC半检月报");
                try
                {
                    if (sfd.FileName.Substring(sfd.FileName.Length - 4) != ".xls")
                    {
                        sfd.FileName += ".xls";
                    }
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                    {
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        if (MessageBox.Show("马上打开？", "导出成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
                if (string.IsNullOrEmpty(id))
                {
                    if (MessageBox.Show("要删除【" + dtp.Value.ToString("yyyy年MM月") + "】所有数据吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            var list = conn.Query<DataBase3CX_GYMJ_02_PMCBJYB>("select * from DataBase3CX_GYMJ_02_PMCBJYB where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);
                            conn.Open();
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlChild = "delete from DataBase3CX_GYMJ_02_PMCBJYB_Child where DataBase3CX_GYMJ_02_PMCBJYB_id=@id";
                                string sqlMain = "delete from DataBase3CX_GYMJ_02_PMCBJYB where id=@id";
                                foreach (var item in list)
                                {
                                    conn.Execute(sqlChild, new { id = item.Id.ToString() }, dbTransaction, null, null);
                                    conn.Execute(sqlMain, new { id = item.Id.ToString() }, dbTransaction, null, null);
                                }
                                dbTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                dbTransaction.Rollback();
                                MessageBox.Show("删除失败，请检查网络和操作！详细错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                dbTransaction.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("要删除选中的记录吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            conn.Open();
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlChild = "delete from DataBase3CX_GYMJ_02_PMCBJYB_Child where DataBase3CX_GYMJ_02_PMCBJYB_id=@id";
                                string sqlMain = "delete from DataBase3CX_GYMJ_02_PMCBJYB where id=@id";
                                conn.Execute(sqlChild, new { id = id }, dbTransaction, null, null);
                                conn.Execute(sqlMain, new { id = id }, dbTransaction, null, null);
                                dbTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                dbTransaction.Rollback();
                                MessageBox.Show("删除失败，请检查网络和操作！详细错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                dbTransaction.Dispose();
                            }
                        }
                    }
                }
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
    }
}