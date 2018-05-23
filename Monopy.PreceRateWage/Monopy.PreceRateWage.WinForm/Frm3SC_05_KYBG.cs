using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_05_KYBG : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$个人开窑量$出勤$单价$金额$考核".Split('$');

        public Frm3SC_05_KYBG()
        {
            InitializeComponent();
        }

        private void Frm3SC_05_KYBG_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
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
            var datas = new BaseDal<DataBase3SC_05_KYBG>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_05_KYBG(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[dgv.Columns.Count - 1].Visible = false;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[13].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——开窑报工.xlsx");
        }

        private bool Recount(List<DataBase3SC_05_KYBG> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算");
                return false;
            }
            try
            {
                var sum = MyDal.GetTotalDataBase3SC_05_KYBG(list);
                decimal.TryParse(sum.CQ, out decimal sumCq);
                decimal.TryParse(sum.GRKYL, out decimal grkyl);
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "开窑工");
                if (dataDay == null)
                {
                    MessageBox.Show("基础数据没有【开窑工】数据，无法导入和计算");
                    return false;
                }
                decimal.TryParse(dataDay.UnitPrice, out decimal dj);
                var zy = new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                if (zy == null)
                {
                    MessageBox.Show("请先导入计算【总窑】");
                    return false;
                }
                decimal.TryParse(zy.KYL, out decimal kyl);
                if (grkyl > kyl)
                {
                    MessageBox.Show($"个人开窑量：【{grkyl.ToString()}】,大于总窑开窑量：【{kyl.ToString()}】，无法导入");
                    return false;
                }
                decimal.TryParse(zy.KHJE, out decimal zykhje);
                decimal.TryParse(zy.KYGDY, out decimal zyDy);
                foreach (var item in list)
                {
                    decimal.TryParse(item.GRKYL, out decimal d);
                    decimal.TryParse(item.CQ, out decimal itemCq);

                    item.DJ = dj.ToString();
                    if (itemCq > 0)
                    {
                        item.JE = (dj * d).ToString();
                        //item.KH = (zykhje * zyDy / sumCq * itemCq).ToString();
                    }
                    else
                    {
                        item.JE = 0.ToString();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                    List<DataBase3SC_05_KYBG> list = new ExcelHelper<DataBase3SC_05_KYBG>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (item.UserCode == item.UserName)
                        {
                            list.RemoveAt(i);
                            i--;
                            continue;
                        }
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                        if (item.UserName == null)
                        {
                            item.UserName = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;
                    }
                    if (Recount(list) && new BaseDal<DataBase3SC_05_KYBG>().Add(list) > 0)
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
                List<DataBase3SC_05_KYBG> list = dgv.DataSource as List<DataBase3SC_05_KYBG>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3SC_05_KYBG>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——烧成——开窑报工.xlsx", saveFileDlg.FileName, list, 2, 6, 1, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_05_KYBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_05_KYBG DataBase3SC_05_KYBG = new DataBase3SC_05_KYBG() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3SC_05_KYBG> frm = new FrmModify<DataBase3SC_05_KYBG>(DataBase3SC_05_KYBG, header, OptionType.Add, Text, 5, 3);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3SC_05_KYBG> list = new BaseDal<DataBase3SC_05_KYBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3SC_05_KYBG>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_05_KYBG DataBase3SC_05_KYBG)
                {
                    if (DataBase3SC_05_KYBG.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3SC_05_KYBG> frm = new FrmModify<DataBase3SC_05_KYBG>(DataBase3SC_05_KYBG, header, OptionType.Modify, Text, 5, 3);
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
                var DataBase3SC_05_KYBG = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_05_KYBG;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_05_KYBG != null)
                    {
                        if (DataBase3SC_05_KYBG.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3SC_05_KYBG>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3SC_05_KYBG>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3SC_05_KYBG> frm = new FrmModify<DataBase3SC_05_KYBG>(DataBase3SC_05_KYBG, header, OptionType.Delete, Text, 5, 0);
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