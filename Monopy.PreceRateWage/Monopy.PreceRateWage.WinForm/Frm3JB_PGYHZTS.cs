﻿using Dapper;
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
    public partial class Frm3JB_PGYHZTS : Office2007Form
    {
        private string[] header;

        public Frm3JB_PGYHZTS()
        {
            InitializeComponent();
            EnableGlass = false;
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

        private void Frm3JB_PGYHZTS_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode)
        {
            var datas = new BaseDal<DataBase3JB_PGYHZTS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_PGYHZTS(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$车间$人员编号$姓名$验货天数".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——品管验货总天数.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_PGYHZTS> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBase3JB_PGYHZTS>().ReadExcel(openFileDlg.FileName, 2, 5);
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
                }

                if (new BaseDal<DataBase3JB_PGYHZTS>().Add(list) > 0)
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
                List<DataBase3JB_PGYHZTS> list = dgv.DataSource as List<DataBase3JB_PGYHZTS>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_PGYHZTS>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——品管验货总天数.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            DataBase3JB_PGYHZTS DataBase3JB_PGYHZTS = new DataBase3JB_PGYHZTS() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3JB_PGYHZTS> frm = new FrmModify<DataBase3JB_PGYHZTS>(DataBase3JB_PGYHZTS, header, OptionType.Add, Text, 4, 0);
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
                var DataBase3JB_PGYHZTS = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_PGYHZTS;
                if (DataBase3JB_PGYHZTS != null)
                {
                    if (DataBase3JB_PGYHZTS.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_PGYHZTS> frm = new FrmModify<DataBase3JB_PGYHZTS>(DataBase3JB_PGYHZTS, header, OptionType.Modify, Text, 4, 0);
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
                var DataBase3JB_PGYHZTS = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_PGYHZTS;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3JB_PGYHZTS != null)
                    {
                        if (DataBase3JB_PGYHZTS.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_PGYHZTS where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase3JB_PGYHZTS> frm = new FrmModify<DataBase3JB_PGYHZTS>(DataBase3JB_PGYHZTS, header, OptionType.Delete, Text, 4, 0);
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