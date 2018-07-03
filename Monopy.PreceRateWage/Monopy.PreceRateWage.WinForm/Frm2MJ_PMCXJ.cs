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
    public partial class Frm2MJ_PMCXJ : Office2007Form
    {
        public Frm2MJ_PMCXJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2MJ_PMCXJ_Load(object sender, EventArgs e)
        {
            dtp.text = Program.NowTime.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 日查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            var list = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.RQ == dateTime.Day.ToString() && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => t.No).ToList();
            RefGrid(list);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 前一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            SearchDay(-1);
        }

        /// <summary>
        /// 后一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            SearchDay(1);
        }

        /// <summary>
        /// 月查询（明细）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthSearch_Click(object sender, EventArgs e)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            var list = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => t.UserCode).ThenBy(t => t.No).ThenBy(t => t.RQ).ToList();
            RefGrid(list);
            dgv.ContextMenuStrip = null;
        }

        /// <summary>
        /// 月查询（汇总）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthSumSearch_Click(object sender, EventArgs e)
        {
            RefGrid(Convert.ToDateTime(dtp.text));
            dgv.ContextMenuStrip = null;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——模具——注模小件.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    DateTime dateTime = Convert.ToDateTime(dtp.text);
                    List<DataBase2MJ_PMCXJ> list = new List<DataBase2MJ_PMCXJ>();
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i, j;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            IRow rowFirst = sheet.GetRow(1);
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            DataBase2MJ_PMCXJ t = new DataBase2MJ_PMCXJ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = (i - 1).ToString();
                            t.GW = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(2));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            t.RQ = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.Childs = new List<DataBase2MJ_PMCXJ_Child>();
                            for (j = 4; j < row.LastCellNum; j++)
                            {
                                var x = new DataBase2MJ_PMCXJ_Child { Id = Guid.NewGuid() };
                                string cpmc = ExcelHelper.GetCellValue(rowFirst.GetCell(j));
                                x.No = j - 4;
                                x.CPMC = cpmc;
                                x.Count = ExcelHelper.GetCellValue(row.GetCell(j));
                                if (!string.IsNullOrEmpty(x.Count) && !string.IsNullOrEmpty(cpmc))
                                {
                                    t.Childs.Add(x);
                                }
                            }
                            list.Add(t);
                        }
                    }
                    if (Recount(list, dateTime) && new BaseDal<DataBase2MJ_PMCXJ>().Add(list) > 0)
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

        /// <summary>
        /// 日导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //                  0   1   2   3   4   5  6   7   8       9   10
            string[] header = "金额$ID$time$user$年$月$序号$岗位$人员编码$姓名$日期".Split('$');
            for (int i = 0; i < 11; i++)
            {
                dt.Columns[i].Caption = header[i];
            }
            dt.Columns.RemoveAt(6);
            dt.Columns.RemoveAt(5);
            dt.Columns.RemoveAt(4);
            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);

            SaveFileDialog sfd = new SaveFileDialog();
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            sfd.FileName = "梦牌二厂模具--注模小件(日)-" + dateTime.ToString("yyyy年MM月dd日") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "成检计件", "二工厂" + dateTime.ToString("yyyy年MM月dd日") + "模具车间小件计件（日）");
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
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            var list = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.RQ == dateTime.Day.ToString()).ToList().OrderBy(t => t.No).ToList();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("无数据，无法重新计算！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dgv.DataSource = null;
            Enabled = false;
            Recount(list, dateTime);
            foreach (var item in list)
            {
                foreach (var child in item.Childs)
                {
                    new BaseDal<DataBase2MJ_PMCXJ_Child>().Edit(child);
                }
                new BaseDal<DataBase2MJ_PMCXJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 月导出（明细）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            btnMonthSearch.PerformClick();
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //                  0   1   2   3   4   5  6   7   8       9   10
            string[] header = "金额$ID$time$user$年$月$序号$岗位$人员编码$姓名$日期".Split('$');
            for (int i = 0; i < 11; i++)
            {
                dt.Columns[i].Caption = header[i];
            }
            dt.Columns.RemoveAt(6);
            dt.Columns.RemoveAt(5);
            dt.Columns.RemoveAt(4);
            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);

            SaveFileDialog sfd = new SaveFileDialog();
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            sfd.FileName = "梦牌二厂模具--注模小件（月明细）-" + dateTime.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "成检计件", "二工厂" + dateTime.ToString("yyyy年MM月") + "模具车间小件计件月明细");
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
            btnMonthSearch.PerformClick();
        }

        /// <summary>
        /// 月导出（汇总）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthSumExportExcel_Click(object sender, EventArgs e)
        {
            btnMonthSumSearch.PerformClick();
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            sfd.FileName = "梦牌二厂模具--注模小件（月汇总）-" + dateTime.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "成检计件", "二工厂" + dateTime.ToString("yyyy年MM月") + "模具车间小件计件月汇总");
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
            btnMonthSumSearch.PerformClick();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
                DateTime dateTime = Convert.ToDateTime(dtp.text);
                if (string.IsNullOrEmpty(id))
                {
                    if (MessageBox.Show("要删除" + dtp.text + "所有数据吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            var list = conn.Query<DataBase2MJ_PMCXJ>("select * from DataBase2MJ_PMCXJ where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " and rq=" + dateTime.Day);
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2MJ_PMCXJ where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " and rq=" + dateTime.Day;
                                string sqlChild = "delete from DataBase2MJ_PMCXJ_Child where DataBase2MJ_PMCXJ_id=@id";
                                foreach (var item in list)
                                {
                                    conn.Execute(sqlChild, new { id = item.Id.ToString() }, dbTransaction, null, null);

                                }
                                conn.Execute(sqlMain, dbTransaction, null, null);
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
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2MJ_PMCXJ where id=@id";
                                string sqlChild = "delete from DataBase2MJ_PMCXJ_Child where DataBase2MJ_PMCXJ_id=@id";
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

        #endregion

        #region 调用方法

        private bool Recount(List<DataBase2MJ_PMCXJ> list, DateTime dateTime)
        {
            try
            {
                var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dateTime.Year && h.CreateMonth == dateTime.Month && h.FactoryNo == "G002" && h.WorkshopName == "模具车间").ToList();
                foreach (var t in list)
                {
                    foreach (var x in t.Childs)
                    {
                        string cpmc = x.CPMC;
                        var baseDay = listMonth.Where(h => h.FactoryNo == "G002" && h.WorkshopName == "模具车间" && h.PostName.Contains("注模") && h.TypesName == cpmc).FirstOrDefault();
                        if (baseDay == null)
                        {
                            x.ZM_Price = 0.ToString();
                        }
                        else
                        {
                            x.ZM_Price = baseDay.UnitPrice;
                            x.DW = baseDay.TypesUnit;
                        }

                        decimal.TryParse(x.Count, out decimal count);
                        x.Money = (Convert.ToDecimal(x.ZM_Price) * count).ToString();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void RefGrid(DateTime dateTime)
        {
            var listSum = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).GroupBy(t => new { t.UserCode, t.GW }).Select(t => new { UserCode = t.Key.UserCode, GW = t.Key.GW }).OrderBy(t => t.GW).ThenBy(t => t.UserCode);
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            dgv.DataSource = null;
            if (listSum.Count() == 0)
            {
                return;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("金额", typeof(decimal));
            dt.Columns.Add("岗位", typeof(string));
            dt.Columns.Add("人员编码", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("年月", typeof(string));
            dt.Rows.Clear();
            DataRow dr_Zmdj = dt.NewRow();
            dr_Zmdj["岗位"] = "注模单价";
            dt.Rows.InsertAt(dr_Zmdj, 0);
            DataRow dr_dw = dt.NewRow();
            dr_dw["岗位"] = "单位";
            dt.Rows.InsertAt(dr_dw, 1);
            foreach (var itemSum in listSum)
            {
                DataRow dataRow = dt.NewRow();
                var list = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.UserCode == itemSum.UserCode && t.GW == itemSum.GW && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
                foreach (var itemMain in list)
                {
                    dataRow["姓名"] = itemMain.UserName;
                    foreach (var item in itemMain.Childs.OrderBy(t => t.No))
                    {
                        string cpmc = item.CPMC;
                        if (!dt.Columns.Contains(cpmc))
                        {
                            dt.Columns.Add(cpmc, typeof(string));
                            dt.Rows[0][cpmc] = item.ZM_Price;
                            dt.Rows[1][cpmc] = item.DW;
                        }
                        if (string.IsNullOrEmpty(dataRow[cpmc].ToString()))
                        {
                            dataRow[cpmc] = item.Count;
                        }
                        else
                        {
                            decimal.TryParse(dataRow[cpmc].ToString(), out decimal dCpmc);
                            decimal.TryParse(item.Count, out decimal count);
                            dataRow[cpmc] = (dCpmc + count).ToString();
                        }
                        decimal.TryParse(dataRow["金额"].ToString(), out decimal je);
                        decimal.TryParse(item.Money, out decimal money);
                        dataRow["金额"] = (je + money).ToString();
                    }
                }
                dataRow["岗位"] = itemSum.GW;
                dataRow["人员编码"] = itemSum.UserCode;
                dataRow["年月"] = dateTime.Year.ToString() + "年" + dateTime.Month.ToString() + "月";
                dt.Rows.Add(dataRow);
            }
            DataRow dr_sum = dt.NewRow();
            dr_sum["金额"] = dt.Compute("SUM(" + dt.Columns[0].ColumnName + ")", "");
            decimal[] tp = new decimal[dt.Columns.Count - 5];
            for (int j = 5; j < dt.Columns.Count; j++)
            {
                for (int i = 2; i < dt.Rows.Count; i++)
                {
                    tp[j - 5] += (decimal.TryParse(dt.Rows[i][j].ToString(), out decimal m) ? m : 0M);
                }
                dr_sum[j] = tp[j - 5];
            }
            dr_sum["岗位"] = "合计";
            dt.Rows.InsertAt(dr_sum, 2);
            dgv.DataSource = dt;
            dgv.Columns[4].Frozen = true;
            for (int i = 0; i < 3; i++)
            {
                dgv.Rows[i].Frozen = true;
                if (i == 2)
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                }
                else
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.LimeGreen;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Blue;
                }
            }
            for (int i = 5; i < dgv.Columns.Count; i++)
            {
                if (string.IsNullOrEmpty(dgv.Rows[1].Cells[i].Value.ToString()))
                {
                    //dgv.Columns[i].DefaultCellStyle.Font = new Font("宋体", 10, FontStyle.Strikeout);
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
            }

            dgv.ClearSelection();
        }

        private void RefGrid(List<DataBase2MJ_PMCXJ> list)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            dgv.DataSource = null;
            if (list.Count == 0)
            {
                return;
            }
            DataTable dt = DataTableHelper.GetDataTable(list);
            dt.Columns.Remove("Childs");
            dt.Columns.Add("Money", typeof(decimal));
            dt.Columns["Money"].SetOrdinal(0);
            DataRow dr_Zmdj = dt.NewRow();
            dr_Zmdj["GW"] = "注模(小)单价";
            dt.Rows.InsertAt(dr_Zmdj, 0);
            DataRow dr_dw = dt.NewRow();
            dr_dw["GW"] = "单位";
            dt.Rows.InsertAt(dr_dw, 1);
            for (int i = 0; i < list.Count; i++)
            {
                foreach (var item in list[i].Childs.OrderBy(t => t.No))
                {
                    string cpmc = item.CPMC;
                    if (!dt.Columns.Contains(cpmc))
                    {
                        dt.Columns.Add(cpmc, typeof(string));
                    }
                    dt.Rows[i + 2][cpmc] = item.Count;
                    dt.Rows[0][cpmc] = item.ZM_Price;
                    dt.Rows[1][cpmc] = item.DW;
                    string gw = list[i].GW;
                    decimal.TryParse(item.Count, out decimal sl);
                    decimal.TryParse(dt.Rows[i + 2]["Money"].ToString(), out decimal je);
                    decimal.TryParse(item.Money, out decimal money);
                    dt.Rows[i + 2]["Money"] = (je + money);
                }
            }
            DataRow dr_sum = dt.NewRow();
            dr_sum["Money"] = dt.Compute("SUM(" + dt.Columns[0].ColumnName + ")", "");
            decimal[] tp = new decimal[dt.Columns.Count - 11];
            for (int j = 11; j < dt.Columns.Count; j++)
            {
                for (int i = 2; i < dt.Rows.Count; i++)
                {
                    tp[j - 11] += (decimal.TryParse(dt.Rows[i][j].ToString(), out decimal m) ? m : 0M);
                }
                dr_sum[j] = tp[j - 11];
            }
            dr_sum["GW"] = "合计";
            dr_sum["Id"] = string.Empty;
            dt.Rows.InsertAt(dr_sum, 2);
            string[] header = "金额$ID$time$user$Year$Month$序号$岗位$人员编码$姓名$日期".Split('$');
            dgv.DataSource = dt;
            for (int i = 0; i < header.Length; i++)
            {
                if (i >= 1 && i <= 6)
                {
                    dgv.Columns[i].Visible = false;
                }
                else
                {
                    dgv.Columns[i].HeaderText = header[i];
                }
            }
            dgv.Columns[10].Frozen = true;
            for (int i = 0; i < 3; i++)
            {
                dgv.Rows[i].Frozen = true;
                if (i == 2)
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                }
                else
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.LimeGreen;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Blue;
                }
            }
            for (int i = 11; i < dgv.Columns.Count; i++)
            {
                if (string.IsNullOrEmpty(dgv.Rows[1].Cells[i].Value.ToString()))
                {
                    //dgv.Columns[i].DefaultCellStyle.Font = new Font("宋体", 10, FontStyle.Strikeout);
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
            }
            dgv.ClearSelection();
        }
        private void SearchDay(int day)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            dateTime = dateTime.AddDays(day);
            dtp.text = dateTime.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        #endregion

    }
}