﻿using DevComponents.DotNetBar;
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
    public partial class FrmBaseGeneral_RZBZ : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;
        private string _factoryNo;
        private string _dept;

        private FrmBaseGeneral_RZBZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        public FrmBaseGeneral_RZBZ(string args) : this()
        {
            try
            {
                _factoryNo = args.Split('-')[0];
                _dept = args.Split('-')[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
                return;
            }
        }

        private void FrmBaseGeneral_RZBZ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode)
        {
            var datas = new BaseDal<DataBaseGeneral_RZBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.FactoryNo == _factoryNo && t.Dept == _dept).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_RZBZ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "年$月$厂区$部门$序号$工种$人员编码$姓名$补助天数$学徒日工$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板通用——入职补助.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBaseGeneral_RZBZ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBaseGeneral_RZBZ>().ReadExcel(openFileDlg.FileName, 2, 5);
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
                    item.FactoryNo = _factoryNo;
                    item.Dept = _dept;
                    item.TheYear = selectTime.Year;
                    item.TheMonth = selectTime.Month;
                }

                if (Recount(list) && new BaseDal<DataBaseGeneral_RZBZ>().Add(list) > 0)
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
                List<DataBaseGeneral_RZBZ> list = dgv.DataSource as List<DataBaseGeneral_RZBZ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_RZBZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出通用——入职补助.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, selectTime.ToString("yyyy-MM-dd") + "-" + _dept))
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
            DataBaseGeneral_RZBZ DataBaseGeneral_JC = new DataBaseGeneral_RZBZ() { Id = Guid.NewGuid(), TheYear = selectTime.Year, TheMonth = selectTime.Month, FactoryNo = _factoryNo, Dept = _dept };
            FrmModify<DataBaseGeneral_RZBZ> frm = new FrmModify<DataBaseGeneral_RZBZ>(DataBaseGeneral_JC, header, OptionType.Add, Text, 4, 2);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_RZBZ DataBaseGeneral_JC)
                {
                    if (DataBaseGeneral_JC.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_RZBZ> frm = new FrmModify<DataBaseGeneral_RZBZ>(DataBaseGeneral_JC, header, OptionType.Modify, Text, 4, 2);
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
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_RZBZ DataBaseGeneral_JC)
                    {
                        if (DataBaseGeneral_JC.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBaseGeneral_RZBZ>;
                                dgv.DataSource = null;
                                list.Remove(DataBaseGeneral_JC);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBaseGeneral_RZBZ>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBaseGeneral_RZBZ> frm = new FrmModify<DataBaseGeneral_RZBZ>(DataBaseGeneral_JC, header, OptionType.Delete, Text, 4, 2);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private bool Recount(List<DataBaseGeneral_RZBZ> list)
        {
            try
            {
                List<DataBaseMonth> listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == _factoryNo && t.WorkshopName.Contains(_dept)).ToList();
                foreach (var item in list)
                {
                    var month = listMonth.Where(t => t.PostName == item.GZ).FirstOrDefault();
                    if (month == null)
                    {
                        MessageBox.Show("工厂：" + _factoryNo + ",车间：" + _dept + ",工种：" + item.GZ + ",基础数据中，没有数据，无法导入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    item.XTRG = month.DayWork_XT;
                    item.Money = ((decimal.TryParse(item.XTRG, out decimal xtrg) ? xtrg : 0M) * (decimal.TryParse(item.Day_BZ, out decimal day) ? day : 0M)).ToString();
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
            var datas = new BaseDal<DataBaseGeneral_RZBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.FactoryNo == _factoryNo && t.Dept == _dept).ToList();
            if (Recount(datas))
            {
                foreach (var item in datas)
                {
                    new BaseDal<DataBaseGeneral_RZBZ>().Edit(item);
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