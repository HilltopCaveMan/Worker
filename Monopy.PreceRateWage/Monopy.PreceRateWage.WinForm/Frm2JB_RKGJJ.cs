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
    public partial class Frm2JB_RKGJJ : Office2007Form
    {
        public Frm2JB_RKGJJ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2JB_RKGJJ_Load(object sender, EventArgs e)
        {
            InitUI();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——检包——入库工计件.xlsx");
        }

        /// <summary>
        /// 删除后重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.Value);
            var list = new BaseDal<DataBase2JB_RKGJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList().OrderBy(t => t.No).ToList();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("无数据，无法重新计算！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dgv.DataSource = null;
            Enabled = false;
            Recount(list);
            foreach (var item in list)
            {
                foreach (var child in item.Childs)
                {
                    new BaseDal<DataBase2JB_RKGJJ_Child>().Edit(child);
                }
                new BaseDal<DataBase2JB_RKGJJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();

            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    List<DataBase2JB_RKGJJ> list = new List<DataBase2JB_RKGJJ>();
                    DateTime dateTime = DateTime.Now;
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
                            DataBase2JB_RKGJJ t = new DataBase2JB_RKGJJ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.GWMC = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(3));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            t.Childs = new List<DataBase2JB_RKGJJ_Child>();
                            for (j = 4; j < row.LastCellNum; j++)
                            {
                                var x = new DataBase2JB_RKGJJ_Child { Id = Guid.NewGuid() };
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
                    if (Recount(list) && new BaseDal<DataBase2JB_RKGJJ>().Add(list) > 0)
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
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //                  0   1    2    3    4     5      6       7       8    9
            string[] header = "Id$Time$User$Year$Month$序号$岗位名称$人员编码$姓名$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }

            dt.Columns.RemoveAt(4);
            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);
            dt.Columns.RemoveAt(0);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "梦牌二厂检包--入库工计件-" + dtp.Value.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "入库工计件", "二工厂" + dtp.Value.ToString("yyyy年MM月") + "入库工计件明细");
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
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            RefDgv(dtp.Value, CmbUserCode.Text, CmbUserName.Text);

        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase2JB_RKGJJ>().GetList().ToList();
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }

        private void RefCmbUserCode(List<DataBase2JB_RKGJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2JB_RKGJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime dtp, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new List<DataBase2JB_RKGJJ>();

            datas = new BaseDal<DataBase2JB_RKGJJ>().GetList(t => (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && t.TheYear == dtp.Year && t.TheMonth == dtp.Month).ToList().OrderBy(t => t.UserCode).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            RefGrid(datas);

            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefGrid(List<DataBase2JB_RKGJJ> list)
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

            DataRow dj = dt.NewRow();
            dj["UserName"] = "单价";
            dt.Rows.InsertAt(dj, 0);
            var listZC = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dtp.Value.Year && h.CreateMonth == dtp.Value.Month && h.FactoryNo == "G002" && h.WorkshopName == "检包车间").ToList();

            for (int i = 0; i < list.Count; i++)
            {
                foreach (var item in list[i].Childs.OrderBy(t => t.No))
                {
                    string cpmc = item.CPMC;
                    if (!dt.Columns.Contains(cpmc))
                    {
                        dt.Columns.Add(cpmc, typeof(string));
                    }
                    dt.Rows[i + 1][cpmc] = item.Count;
                    dt.Rows[0][cpmc] = item.Price;
                }
            }
            DataRow dr_sum = dt.NewRow();

            decimal[] tp = new decimal[dt.Columns.Count - 4];
            for (int j = 4; j < dt.Columns.Count; j++)
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    tp[j - 4] += (decimal.TryParse(dt.Rows[i][j].ToString(), out decimal m) ? m : 0M);
                }
                dr_sum[j] = tp[j - 4];
            }
            dr_sum["GWMC"] = "合计";
            dr_sum["No"] = string.Empty;
            dr_sum["UserCode"] = string.Empty;
            dr_sum["UserName"] = string.Empty;
            dt.Rows.InsertAt(dr_sum, 1);
            string[] header = "Id$Time$User$Year$Month$序号$岗位名称$人员编码$姓名$金额".Split('$');
            dt.DefaultView.Sort = "No";
            dt = dt.DefaultView.ToTable();
            dgv.DataSource = dt;
            for (int i = 0; i < header.Length; i++)
            {
                if (i < 5)
                {
                    dgv.Columns[i].Visible = false;
                }
                else
                {
                    dgv.Columns[i].HeaderText = header[i];
                }
            }
            dgv.Columns[5].Frozen = true;
            dgv.Rows[1].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.LimeGreen;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Blue;
            
            dgv.ClearSelection();
        }

        private bool Recount(List<DataBase2JB_RKGJJ> list)
        {
            try
            {
                var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dtp.Value.Year && h.CreateMonth == dtp.Value.Month && h.FactoryNo == "G002" && h.WorkshopName == "检包车间").ToList();

                if (listMonth == null || listMonth.Count == 0)
                {
                    MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (var t in list)
                {
                    decimal totalJj = 0;
                    foreach (var x in t.Childs)
                    {
                        string cpmc = x.CPMC;
                        DataBaseDay baseDay = null;

                        baseDay = listMonth.Where(h => h.TypesType == cpmc && h.PostName == t.GWMC).FirstOrDefault();

                        if (baseDay == null)
                        {
                            x.Price = 0.ToString();
                        }
                        else
                        {
                            x.Price = baseDay.UnitPrice;
                        }
                        decimal.TryParse(x.Count, out decimal count);
                        decimal.TryParse(x.Price, out decimal price);
                        totalJj += price * count;
                    }
                    t.JE = totalJj.ToString();

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var id = dgv.SelectedRows[0].Cells["id"].Value.ToString();
                DateTime dateTime = Convert.ToDateTime(dtp.Value);
                if (string.IsNullOrEmpty(id))
                {
                    if (MessageBox.Show("要删除" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            var list = conn.Query<DataBase2JB_RKGJJ>("select * from DataBase2JB_RKGJJ where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " ");
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2JB_RKGJJ where id=@id";
                                string sqlChild = "delete from DataBase2JB_RKGJJ_Child where DataBase2JB_RKGJJ_Id=@id";
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
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2JB_RKGJJ where id=@id";
                                string sqlChild = "delete from DataBase2JB_RKGJJ_Child where DataBase2JB_RKGJJ_Id=@id";
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

    }
}
