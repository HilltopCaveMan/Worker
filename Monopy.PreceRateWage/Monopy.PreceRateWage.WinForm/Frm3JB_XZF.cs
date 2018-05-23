using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3JB_XZF : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public Frm3JB_XZF()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3JB_XZF_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            selectTime = dtp.Value;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3JB_XZF>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_XZF(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 3; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "年$月$序号$产线$工号$姓名$月实出勤$月工资$应出勤$金额$操作时间$操作人".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\InExcel\\模板三厂——检包——线长费模板.xlsx");
        }

        private bool Recount(List<DataBase3JB_XZF> list)
        {
            try
            {
                var xzf = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.MonthData == "线长费");
                if (xzf == null)
                {
                    MessageBox.Show("基础数据线长费未设置！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                foreach (var item in list)
                {
                    var cq = new BaseDal<DataBaseGeneral_CQ>().Get(t => t.UserCode == item.UserCode && t.TheYear == item.TheYear && t.TheMonth == item.TheMonth);
                    if (cq == null)
                    {
                        MessageBox.Show("工号:" + item.UserCode + "没有出勤信息，无法计算！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    item.DBValue = xzf.MoneyShift;
                    item.DayYcq = cq.DayYcq;
                    if (decimal.TryParse(item.DayCq, out decimal daycq) && daycq > 0)
                    {
                        item.Money = (Convert.ToDecimal(item.DBValue) / Convert.ToDecimal(item.DayYcq) * daycq).ToString();
                    }
                    else
                    {
                        item.Money = 0.ToString();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_XZF> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBase3JB_XZF>().ReadExcel(openFileDlg.FileName, 2, 3);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Enabled = true;
                            return;
                        }

                        list[i].CreateUser = Program.User.ToString();
                        list[i].CreateTime = Program.NowTime;
                        list[i].TheYear = selectTime.Year;
                        list[i].TheMonth = selectTime.Month;
                    }
                }
                if (Recount(list) && new BaseDal<DataBase3JB_XZF>().Add(list) > 0)
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
                List<DataBase3JB_XZF> list = dgv.DataSource as List<DataBase3JB_XZF>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_XZF>().WriteExcle(Application.StartupPath + "\\OutExcel\\模板导出三厂——检包——线长费模板.xlsx", saveFileDlg.FileName, list, 2, 3, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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
            DataBase3JB_XZF DataBase3JB_XZF = new DataBase3JB_XZF() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month };
            FrmModify<DataBase3JB_XZF> frm = new FrmModify<DataBase3JB_XZF>(DataBase3JB_XZF, header, OptionType.Add, Text, 2, 5);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3JB_XZF> list = new BaseDal<DataBase3JB_XZF>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3JB_XZF>().Edit(item);
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
                var DataBase3JB_XZF = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XZF;
                if (DataBase3JB_XZF != null)
                {
                    if (DataBase3JB_XZF.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_XZF> frm = new FrmModify<DataBase3JB_XZF>(DataBase3JB_XZF, header, OptionType.Modify, Text, 2, 5);
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
                var DataBase3JB_XZF = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XZF;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3JB_XZF != null)
                    {
                        if (DataBase3JB_XZF.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_XZF where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString()) > 0)
                                    {
                                        btnSearch.PerformClick();
                                        MessageBox.Show("全部删除完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("删除失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3JB_XZF> frm = new FrmModify<DataBase3JB_XZF>(DataBase3JB_XZF, header, OptionType.Delete, Text, 2, 2);
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