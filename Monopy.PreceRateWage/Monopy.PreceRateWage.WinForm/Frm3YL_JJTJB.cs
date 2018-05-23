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
    public partial class Frm3YL_JJTJB : Office2007Form
    {
        private string[] header;

        public Frm3YL_JJTJB()
        {
            InitializeComponent();
        }

        private void Frm3YL_JJTJB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        private void InitUI()
        {
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            var list = new BaseDal<DataBase3YL_JJTJB>().GetList(t => t.TheYear == Program.NowTime.Year && t.TheMonth == Program.NowTime.Month).DistinctBy(t => t.GW).OrderBy(t => t.GW).ToList();
            list.Insert(0, new DataBase3YL_JJTJB() { GW = "全部" });
            cmbLine.DataSource = list;
            cmbLine.DisplayMember = "GW";
            cmbLine.ValueMember = "GW";
        }

        private void RefDgv(DateTime selectTime, string gw, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = true;
            }
            var list = new BaseDal<DataBase3YL_JJTJB>().GetList(t => gw != "全部" ? t.GW == gw && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) : t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            list.Insert(0, MyDal.GetTotalDataBase3YL_JJTJB(list));
            dgv.DataSource = list;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间	操作人	年	月	序号	岗位	工号	姓名	计件工资	加釉工	清釉工	是否班长	班长费	是否全勤	全勤奖".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbLine.Text, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——原料——计件统计.xlsx");
        }

        private bool Recount(List<DataBase3YL_JJTJB> list)
        {
            try
            {
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间").ToList();
                foreach (var item in list)
                {
                    if (item.IsBZ.Contains("是") || item.GW.Contains("加釉工"))
                    {
                        var dataBaseMonth = listMonth.Where(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间" && t.PostName == item.GW).FirstOrDefault();
                        if (dataBaseMonth == null)
                        {
                            MessageBox.Show("注意：月基础数据不完整！【" + item.GW + "】，不月基础数据不存在，请检查文档是否正确或联系人资负责人！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        item.BZF = item.IsBZ.Contains("是") ? dataBaseMonth.MoneyShift : string.Empty;
                        item.IsQQJ = item.GW.Contains("加釉工") ? true : false;
                        item.QQJ = item.IsQQJ ? dataBaseMonth.MoneyQQJ : string.Empty;
                    }
                }
                return true;
            }
            catch
            {
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
                Enabled = false;
                var list = new ExcelHelper<DataBase3YL_JJTJB>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 3);
                if (list == null)
                {
                    Enabled = true;
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName) || item.UserCode == item.UserName)
                    {
                        list.RemoveAt(i);
                        i--;
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
                    item.No = (i + 1).ToString();
                }
                if (Recount(list) && new BaseDal<DataBase3YL_JJTJB>().Add(list) > 0)
                {
                    Enabled = true;
                    InitUI();
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Enabled = true;
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
                List<DataBase3YL_JJTJB> list = dgv.DataSource as List<DataBase3YL_JJTJB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3YL_JJTJB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——原料——计件统计.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var max = new BaseDal<DataBase3YL_JJTJB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderByDescending(t => t.No).FirstOrDefault();
            string maxId;
            if (max == null)
            {
                maxId = 1.ToString();
            }
            else
            {
                maxId = (Convert.ToInt32(max.No) + 1).ToString();
            }
            DataBase3YL_JJTJB DataBase3YL_JJTJB = new DataBase3YL_JJTJB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxId };
            FrmModify<DataBase3YL_JJTJB> frm = new FrmModify<DataBase3YL_JJTJB>(DataBase3YL_JJTJB, header, OptionType.Add, Text, 5, 3);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnSearch.PerformClick();
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3YL_JJTJB> list = dgv.DataSource as List<DataBase3YL_JJTJB>;
            dgv.DataSource = null;
            Enabled = false;
            list.RemoveAt(0);
            if (Recount(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase3YL_JJTJB>().Edit(item);
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Enabled = true;
                MessageBox.Show("重新计算失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3YL_JJTJB DataBase3YL_JJTJB)
                {
                    if (DataBase3YL_JJTJB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3YL_JJTJB> frm = new FrmModify<DataBase3YL_JJTJB>(DataBase3YL_JJTJB, header, OptionType.Modify, Text, 5, 3);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
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
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var DataBase3YL_JJTJB = dgv.SelectedRows[0].DataBoundItem as DataBase3YL_JJTJB;
                    if (DataBase3YL_JJTJB != null)
                    {
                        if (DataBase3YL_JJTJB.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                List<DataBase3YL_JJTJB> list = dgv.DataSource as List<DataBase3YL_JJTJB>;
                                dgv.DataSource = null;
                                Enabled = false;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3YL_JJTJB>().Delete(item);
                                }
                                Enabled = true;
                                btnSearch.PerformClick();
                                MessageBox.Show("全部删除完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3YL_JJTJB> frm = new FrmModify<DataBase3YL_JJTJB>(DataBase3YL_JJTJB, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitUI();
        }
    }
}