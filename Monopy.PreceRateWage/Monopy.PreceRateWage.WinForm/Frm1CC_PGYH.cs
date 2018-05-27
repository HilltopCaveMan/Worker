using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm1CC_PGYH : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$车间$人员编码$姓名$验货天数".Split('$');
        public Frm1CC_PGYH()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初期加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1CC_PGYH_Load(object sender, EventArgs e)
        {
            InitUI();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, comCJ.Text, CmbUserCode.Text, CmbUserName.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——仓储——品管验货天数.xlsx");
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
                    List<DataBase1CC_PGYH> list = new List<DataBase1CC_PGYH>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            IRow rowFirst = sheet.GetRow(1);
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            DataBase1CC_PGYH t = new DataBase1CC_PGYH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.CJ = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.YHTS = ExcelHelper.GetCellValue(row.GetCell(4));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            list.Add(t);
                        }
                    }
                    if (new BaseDal<DataBase1CC_PGYH>().Add(list) > 0)
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
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1CC_PGYH> list = dgv.DataSource as List<DataBase1CC_PGYH>;
                if (new ExcelHelper<DataBase1CC_PGYH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——仓储——品管验货天数.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1CC_PGYH DataBase1CC_PGYH)
                {
                    FrmModify<DataBase1CC_PGYH> frm = new FrmModify<DataBase1CC_PGYH>(DataBase1CC_PGYH, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                    }
                }
            }
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
                var id = dgv.SelectedRows[0].Cells["id"].Value.ToString();
                DateTime dateTime = Convert.ToDateTime(dtp.Value);
                if (string.IsNullOrEmpty(id))
                {
                    if (MessageBox.Show("要删除" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            var list = conn.Query<DataBase1CC_PGYH>("select * from DataBase1CC_PGYH where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " ");
                            conn.Open();
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase1CC_PGYH where id=@id";
                                foreach (var item in list)
                                {
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
                            conn.Open();
                            IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase1CC_PGYH where id=@id";
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
            var list = new BaseDal<DataBase1CC_PGYH>().GetList().ToList();
            RefComCJ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }
        private void RefComCJ(List<DataBase1CC_PGYH> list)
        {
            var listTmp = list.GroupBy(t => t.CJ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            comCJ.DataSource = listTmp;
            comCJ.DisplayMember = "CJ";
            comCJ.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase1CC_PGYH> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase1CC_PGYH> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string cj, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1CC_PGYH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (cj == "全部" ? true : t.CJ.Contains(cj)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode)) && (userName == "全部" ? true : t.UserName.Contains(userName))).ToList().OrderBy(t => t.No).ThenBy(t => t.UserCode).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1CC_PGYH(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();


        }

        #endregion
    }
}
