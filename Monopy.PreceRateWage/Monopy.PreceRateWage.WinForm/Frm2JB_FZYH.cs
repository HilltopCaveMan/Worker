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
    public partial class Frm2JB_FZYH : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        private Frm2JB_FZYH()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm2JB_FZYH_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
        {
            var datas = new BaseDal<DataBase2JB_FZYH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2JB_FZYH(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$岗位名称$工号$姓名$性别$验货天数$单价$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——检包——辅助验货.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase2JB_FZYH> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBase2JB_FZYH>().ReadExcel(openFileDlg.FileName, 2, 5);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName) || item.UserCode == item.UserName)
                    {
                        list.RemoveAt(i);
                        if (i >= 0)
                        {
                            i--;
                        }
                        continue;
                    }
                    if (!MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                }

                if (Recount(list) && new BaseDal<DataBase2JB_FZYH>().Add(list) > 0)
                {
                    Enabled = true;
                    txtUserCode.Text = "";
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
                List<DataBase2JB_FZYH> list = dgv.DataSource as List<DataBase2JB_FZYH>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2JB_FZYH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——检包——辅助验货.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM-dd")))
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
            DataBase2JB_FZYH DataBaseGeneral_JC = new DataBase2JB_FZYH()
            {
                Id = Guid.NewGuid(),
                TheYear = dtp.Value.Year,
                TheMonth = dtp.Value.Month,
                CreateUser = Program.User.ToString(),
                CreateTime = Program.NowTime
            };
            FrmModify<DataBase2JB_FZYH> frm = new FrmModify<DataBase2JB_FZYH>(DataBaseGeneral_JC, header, OptionType.Add, Text, 4, 2);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
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

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2JB_FZYH DataBaseGeneral_JC)
                {
                    if (DataBaseGeneral_JC.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2JB_FZYH> frm = new FrmModify<DataBase2JB_FZYH>(DataBaseGeneral_JC, header, OptionType.Modify, Text, 4, 2);
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
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (dgv.SelectedRows[0].DataBoundItem is DataBase2JB_FZYH DataBaseGeneral_JC)
                    {
                        if (DataBaseGeneral_JC.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase2JB_FZYH>;
                                dgv.DataSource = null;
                                list.Remove(DataBaseGeneral_JC);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase2JB_FZYH>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase2JB_FZYH> frm = new FrmModify<DataBase2JB_FZYH>(DataBaseGeneral_JC, header, OptionType.Delete, Text, 4, 2);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private bool Recount(List<DataBase2JB_FZYH> list)
        {
            try
            {
                List<DataBaseMonth> listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G002" && t.WorkshopName.Contains("检包车间")).ToList();
                var pgyh = new BaseDal<DataBase1CC_PGYH>().GetList(t => t.FactoryNo == "G002" && t.CJ.Contains("检包车间") && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                if (pgyh == null || pgyh.Count == 0)
                {
                    MessageBox.Show("工厂：G002,车间：检包车间,没有品管数据，无法导入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                decimal totalCount = 0;
                foreach (var count in pgyh)
                {
                    decimal.TryParse(count.YHTS, out decimal ts);
                    totalCount += ts;
                }

                decimal totalDay = list.Sum(t => string.IsNullOrEmpty(t.DayCount) ? 0M : Convert.ToDecimal(t.DayCount));
                if (totalDay > totalCount)
                {
                    MessageBox.Show("验货总数的天数大于品管中的验货天数，品管中的验货天数为:" + totalCount + "，无法导入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                foreach (var item in list)
                {
                    var month = listMonth.Where(t => t.PostName == item.GWMC && t.Gender == item.XB).FirstOrDefault();
                    if (month == null)
                    {
                        MessageBox.Show("没有月工资数据，无法导入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    decimal.TryParse(item.DayCount, out decimal day);
                    item.DJ = month.DayWork_FZYH;
                    decimal.TryParse(item.DJ, out decimal xtrg);
                    item.Money = (xtrg * day).ToString();

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            var datas = new BaseDal<DataBase2JB_FZYH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (Recount(datas))
            {
                foreach (var item in datas)
                {
                    new BaseDal<DataBase2JB_FZYH>().Edit(item);
                }
                btnSearch.PerformClick();
                MessageBox.Show("重新计算完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("重新计算失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}