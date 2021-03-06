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
using System.Text;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3MJ_XJCJYB : Office2007Form
    {
        private string[] header;

        public Frm3MJ_XJCJYB()
        {
            InitializeComponent();
        }

        private void RefDgv(DateTime selectTime, string cpmc)
        {
            var datas = new BaseDal<DataBase3MJ_XJCJYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : 0).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3MJ_XJCJYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$产品名称$单位$生产数量".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——模具——小件车间月报.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3MJ_XJCJYB> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3MJ_XJCJYB>().ReadExcel(openFileDlg.FileName, 4, 6, 0, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                StringBuilder sb = new StringBuilder();
                var listBase = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间").ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.CPMC))
                    {
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    //if (new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间" && t.TypesName == item.CPMC) == null)
                    if (listBase.Where(t => t.TypesName == item.CPMC).Count() == 0)
                    {
                        sb.Append("【" + item.CPMC + "】,");
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                    item.No = (i + 1).ToString();
                }
                if (sb.Length > 0)
                {
                    MessageBox.Show("基础数据库中没有信息，导入时自动过滤的有：" + Environment.NewLine + sb.ToString().TrimEnd(','), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (new BaseDal<DataBase3MJ_XJCJYB>().Add(list) > 0)
                {
                    Enabled = true;
                    txtCPMC.Text = "";
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
                List<DataBase3MJ_XJCJYB> list = dgv.DataSource as List<DataBase3MJ_XJCJYB>;
                if (new ExcelHelper<DataBase3MJ_XJCJYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——模具——小件车间月报.xlsx", saveFileDlg.FileName, list, 4, 6, 0, 0, 2, 1, dtp.Value.ToString("yyyy-MM")))
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
            DataBase3MJ_XJCJYB DataBase3MJ_XJCJYB = new DataBase3MJ_XJCJYB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3MJ_XJCJYB> frm = new FrmModify<DataBase3MJ_XJCJYB>(DataBase3MJ_XJCJYB, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtCPMC.Text = string.Empty;
                btnSearch.PerformClick();
                btnRecount.PerformClick();
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
                var DataBase3MJ_XJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase3MJ_XJCJYB;
                if (DataBase3MJ_XJCJYB != null)
                {
                    if (DataBase3MJ_XJCJYB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3MJ_XJCJYB> frm = new FrmModify<DataBase3MJ_XJCJYB>(DataBase3MJ_XJCJYB, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                        btnRecount.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3MJ_XJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase3MJ_XJCJYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3MJ_XJCJYB != null)
                    {
                        if (DataBase3MJ_XJCJYB.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3MJ_XJCJYB where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase3MJ_XJCJYB> frm = new FrmModify<DataBase3MJ_XJCJYB>(DataBase3MJ_XJCJYB, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                        }
                    }
                }
            }
        }

        private void Frm3MJXJCJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtCPMC.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            var datas = new BaseDal<DataBase3MJ_XJCJYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().GroupBy(t => t.CPMC).Select(t => new { CPMC = t.Key, Count = t.Sum(x => decimal.TryParse(x.SCSL, out decimal sclj) ? sclj : 0M) }).Where(t => t.Count > 0M).ToList();
            if (datas.Count == 0)
            {
                MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                foreach (var item in datas)
                {
                    string sql = "SELECT sum(CAST( b.[Count] as NUMERIC(18,3))) from DataBase3MJ_PMCXJ a left JOIN DataBase3MJ_PMCXJ_Child b on a.Id=b.DataBase3MJ_PMCXJ_Id where a.TheYear=@TheYear and a.TheMonth=@TheMonth and b.CPMC=@cpmc";
                    var obj = conn.ExecuteScalar(sql, new { TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, cpmc = item.CPMC });
                    if (obj != null)
                    {
                        decimal.TryParse(obj.ToString(), out decimal mx);
                        if (mx > item.Count)
                        {
                            //报警
                            string userCode = conn.ExecuteScalar("SELECT a.CreateUser from DataBase3MJ_PMCXJ a left JOIN DataBase3MJ_PMCXJ_Child b on a.Id=b.DataBase3MJ_PMCXJ_Id where a.TheYear=@TheYear and a.TheMonth=@TheMonth and b.CPMC=@cpmc", new { TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, cpmc = item.CPMC }).ToString().Split('_')[0];
                            bj(userCode, item.CPMC, mx, item.Count);
                        }
                    }
                }
            }
            MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "验证完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 验证报警
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="cpmb"></param>
        /// <param name="mx"></param>
        /// <param name="yb"></param>
        private void bj(string userCode, string cpmb, decimal mx, decimal yb)
        {
            var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = userCode, MsgTitle = "模具小件月报数量大于明细数量", MsgClass = "模具小件月报数量大于明细数量", Msg = dtp.Value.ToString("yyyy年MM月") + "产品名称:【" + cpmb + "】，月报数量：【" + yb.ToString() + "】，日PMC小件明细数量：【" + mx.ToString() + "】。明细大于月报，验证未通过！", IsDone = false, IsRead = false, CreateTime = Program.NowTime, CreateUser = "系统报警" };
            new BaseDal<DataBaseMsg>().Add(msg);
            msg.ID = Guid.NewGuid();
            msg.UserCode = Program.HrCode;
            new BaseDal<DataBaseMsg>().Add(msg);
        }
    }
}