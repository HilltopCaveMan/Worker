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
    public partial class Frm2CX_PMCJP : Office2007Form
    {
        public Frm2CX_PMCJP()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2CX_PMCJP_Load(object sender, EventArgs e)
        {
            InitUI();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
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

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——成型——精坯月报.xlsx");
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
                    List<DataBase2CX_PMCJP> list = new List<DataBase2CX_PMCJP>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i, j;
                        for (i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow rowFirst = sheet.GetRow(0);
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            DataBase2CX_PMCJP t = new DataBase2CX_PMCJP { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = i.ToString();
                            t.GC = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.GH = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.GD = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(4));
                            t.HJ = ExcelHelper.GetCellValue(row.GetCell(5));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            t.Childs = new List<DataBase2CX_PMCJP_Child>();
                            for (j = 6; j < row.LastCellNum; j++)
                            {
                                var x = new DataBase2CX_PMCJP_Child { Id = Guid.NewGuid() };
                                string cpmc = ExcelHelper.GetCellValue(rowFirst.GetCell(j));
                                x.No = j - 6;
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
                    if (new BaseDal<DataBase2CX_PMCJP>().Add(list) > 0)
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
            //                  0   1    2    3    4     5
            string[] header = "Id$Time$User$Year$Month$序号$工厂$工号$工段$员工编码$姓名$合计".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }

            dt.Columns.RemoveAt(5);
            dt.Columns.RemoveAt(4);
            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);
            dt.Columns.RemoveAt(0);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "梦牌二厂成型--精坯月报-" + dtp.Value.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "精坯月报", "二工厂" + dtp.Value.ToString("yyyy年MM月") + "精坯月报");
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
                            var list = conn.Query<DataBase2CX_PMCJP>("select * from DataBase2CX_PMCJP where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " ");
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2CX_PMCJP where id=@id";
                                string sqlChild = "delete from DataBase2CX_PMCJP_Child where DataBase2CX_PMCJP_Id=@id";
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
                                string sqlMain = "delete from DataBase2CX_PMCJP where id=@id";
                                string sqlChild = "delete from DataBase2CX_PMCJP_Child where DataBase2CX_PMCJP_Id=@id";
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

        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase2CX_PMCJP>().GetList().ToList();
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }

        private void RefCmbUserCode(List<DataBase2CX_PMCJP> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2CX_PMCJP> list)
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
            var datas = new List<DataBase2CX_PMCJP>();

            datas = new BaseDal<DataBase2CX_PMCJP>().GetList(t => (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && t.TheYear == dtp.Year && t.TheMonth == dtp.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            RefGrid(datas);

            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefGrid(List<DataBase2CX_PMCJP> list)
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
            for (int i = 0; i < list.Count; i++)
            {

                foreach (var item in list[i].Childs.OrderBy(t => t.No))
                {
                    string cpmc = item.CPMC;
                    if (!dt.Columns.Contains(cpmc))
                    {
                        dt.Columns.Add(cpmc, typeof(string));
                    }
                    dt.Rows[i][cpmc] = item.Count;
                }
            }
            string[] header = "Id$Time$User$Year$Month$序号$工厂$工号$工段$员工编码$姓名$合计".Split('$');

            dgv.DataSource = dt;
            for (int i = 0; i < header.Length; i++)
            {
                if (i < 6)
                {
                    dgv.Columns[i].Visible = false;
                }
                dgv.Columns[i].HeaderText = header[i];
            }

            dgv.Columns[5].Frozen = true;

            dgv.ClearSelection();
        }

        #endregion

    }
}
