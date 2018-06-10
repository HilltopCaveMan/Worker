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
    public partial class Frm1MJ_YMJJ : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$岗位$日期$人员编号$姓名$撤换全线$撤/换单线$科玛全包分体撤换全线$科玛全包分体撤/换单线$撤换全线$撤/换单线$科玛全包分体撤换全线$科玛全包分体撤/换单线$段位$线位$撤下品种$数量$上线品种$数量$原线位模型数$备注$金额".Split('$');
        public Frm1MJ_YMJJ()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1MJ_YMJJ_Load(object sender, EventArgs e)
        {
            dtp.text = Program.NowTime.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dateTime = Convert.ToDateTime(dtp.text);
            var list = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.RQ == dateTime.Day.ToString() && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => t.No).ToList();
            RefGrid(list, dateTime);
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
            var list = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().OrderBy(t => t.UserCode).ThenBy(t => t.No).ThenBy(t => t.RQ).ToList();
            RefGrid(list, dateTime);
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
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——模具——运模计件.xlsx");
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
                    List<DataBase1MJ_YMJJ> list = new List<DataBase1MJ_YMJJ>();
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
                            DataBase1MJ_YMJJ t = new DataBase1MJ_YMJJ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.GW = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(4));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            t.RQ = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.CHQX1 = ExcelHelper.GetCellValue(row.GetCell(5));
                            t.CHDX1 = ExcelHelper.GetCellValue(row.GetCell(6));
                            t.KMCHQX1 = ExcelHelper.GetCellValue(row.GetCell(7));
                            t.KMCHDX1 = ExcelHelper.GetCellValue(row.GetCell(8));
                            t.CHQX2 = ExcelHelper.GetCellValue(row.GetCell(9));
                            t.CHDX2 = ExcelHelper.GetCellValue(row.GetCell(10));
                            t.KMCHQX2 = ExcelHelper.GetCellValue(row.GetCell(11));
                            t.KMCHDX2 = ExcelHelper.GetCellValue(row.GetCell(12));
                            t.DW = ExcelHelper.GetCellValue(row.GetCell(13));
                            t.XW = ExcelHelper.GetCellValue(row.GetCell(14));
                            t.CXPZ = ExcelHelper.GetCellValue(row.GetCell(15));
                            t.CXPZSL = ExcelHelper.GetCellValue(row.GetCell(16));
                            t.SXPZ = ExcelHelper.GetCellValue(row.GetCell(17));
                            t.SXPZSL = ExcelHelper.GetCellValue(row.GetCell(18));
                            t.YXWMXS = ExcelHelper.GetCellValue(row.GetCell(19));
                            t.BZ = ExcelHelper.GetCellValue(row.GetCell(20));
                            list.Add(t);
                        }
                    }
                    if (Recount(list, dateTime) && new BaseDal<DataBase1MJ_YMJJ>().Add(list) > 0)
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

            DateTime dateTime = Convert.ToDateTime(dtp.text);
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1MJ_YMJJ> list = dgv.DataSource as List<DataBase1MJ_YMJJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase1MJ_YMJJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——模具——运模计件.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dateTime.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.RQ == dateTime.Day.ToString()).ToList().OrderBy(t => t.No).ToList();
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
                new BaseDal<DataBase1MJ_YMJJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1MJ_YMJJ DataBase1MJ_YMJJ)
                {
                    FrmModify<DataBase1MJ_YMJJ> frm = new FrmModify<DataBase1MJ_YMJJ>(DataBase1MJ_YMJJ, header, OptionType.Modify, Text, 5, 1);
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
                DateTime dateTime = Convert.ToDateTime(dtp.text);
                var DataBase1MJ_YMJJ = dgv.SelectedRows[0].DataBoundItem as DataBase1MJ_YMJJ;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1MJ_YMJJ != null)
                    {
                        if (DataBase1MJ_YMJJ.No == "合计" || DataBase1MJ_YMJJ.No == "运模单价")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dateTime.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase1MJ_YMJJ where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString()) > 0)
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
                        FrmModify<DataBase1MJ_YMJJ> frm = new FrmModify<DataBase1MJ_YMJJ>(DataBase1MJ_YMJJ, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                        }
                    }
                }

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

        private bool Recount(List<DataBase1MJ_YMJJ> list, DateTime dateTime)
        {
            try
            {
                var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dateTime.Year && h.CreateMonth == dateTime.Month && h.FactoryNo == "G001" && h.WorkshopName == "模具车间" && h.Classification == "运模").ToList();
                foreach (var t in list)
                {
                    var baseQX = listMonth.Where(h => h.PostName == t.GW && h.TypesType == "撤换全线").FirstOrDefault().UnitPrice;
                    var baseDX = listMonth.Where(h => h.PostName == t.GW && h.TypesType == "撤/换单线").FirstOrDefault().UnitPrice;
                    var baseKMQX = listMonth.Where(h => h.PostName == t.GW && h.TypesType == "科玛全包分体撤换全线").FirstOrDefault().UnitPrice;
                    var baseKMDX = listMonth.Where(h => h.PostName == t.GW && h.TypesType == "科玛全包分体撤/换单线").FirstOrDefault().UnitPrice;
                    decimal.TryParse(t.CHQX1, out decimal qx);
                    decimal.TryParse(t.CHDX1, out decimal dx);
                    decimal.TryParse(t.KMCHQX1, out decimal kmqx);
                    decimal.TryParse(t.KMCHDX1, out decimal kmdx);
                    decimal.TryParse(baseQX, out decimal qxdj);
                    decimal.TryParse(baseDX, out decimal dxdj);
                    decimal.TryParse(baseKMQX, out decimal kmqxdj);
                    decimal.TryParse(baseKMDX, out decimal kmdxdj);
                    t.JE = (qx * qxdj + dx * dxdj + kmqx * kmqxdj + kmdx * kmdxdj).ToString();
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
            List<DataBase1MJ_YMJJ> datas = new List<DataBase1MJ_YMJJ>();
            var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dateTime.Year && h.CreateMonth == dateTime.Month && h.FactoryNo == "G001" && h.WorkshopName == "模具车间" && h.Classification == "运模" && h.PostName == "运模工").ToList();

            var baseQX = listMonth.Where(h => h.TypesType == "撤换全线").FirstOrDefault().UnitPrice;
            var baseDX = listMonth.Where(h => h.TypesType == "撤/换单线").FirstOrDefault().UnitPrice;
            var baseKMQX = listMonth.Where(h => h.TypesType == "科玛全包分体撤换全线").FirstOrDefault().UnitPrice;
            var baseKMDX = listMonth.Where(h => h.TypesType == "科玛全包分体撤/换单线").FirstOrDefault().UnitPrice;

            var listSum = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode.Contains(txtUserCode.Text) && t.UserName.Contains(txtUserName.Text)).ToList().GroupBy(t => new { t.UserCode, t.GW }).Select(t => new { UserCode = t.Key.UserCode, GW = t.Key.GW }).OrderBy(t => t.GW).ThenBy(t => t.UserCode);
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

            foreach (var itemSum in listSum)
            {
                DataBase1MJ_YMJJ data = new DataBase1MJ_YMJJ();

                var list = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.UserCode == itemSum.UserCode && t.GW == itemSum.GW && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
                data.GW = list[0].GW;
                data.RQ = dateTime.Year.ToString() + "年" + dateTime.Month.ToString() + "月";
                data.UserCode = list[0].UserCode;
                data.UserName = list[0].UserName;
                data.CHQX1 = list.Sum(t => decimal.TryParse(t.CHQX1, out decimal d) ? d : 0M).ToString();
                data.CHDX1 = list.Sum(t => decimal.TryParse(t.CHDX1, out decimal d) ? d : 0M).ToString();
                data.KMCHQX1 = list.Sum(t => decimal.TryParse(t.KMCHQX1, out decimal d) ? d : 0M).ToString();
                data.KMCHDX1 = list.Sum(t => decimal.TryParse(t.KMCHDX1, out decimal d) ? d : 0M).ToString();
                data.CHQX2 = list.Sum(t => decimal.TryParse(t.CHQX2, out decimal d) ? d : 0M).ToString();
                data.CHDX2 = list.Sum(t => decimal.TryParse(t.CHDX2, out decimal d) ? d : 0M).ToString();
                data.KMCHQX2 = list.Sum(t => decimal.TryParse(t.KMCHQX2, out decimal d) ? d : 0M).ToString();
                data.KMCHDX2 = list.Sum(t => decimal.TryParse(t.KMCHDX2, out decimal d) ? d : 0M).ToString();
                data.CXPZSL = list.Sum(t => decimal.TryParse(t.CXPZSL, out decimal d) ? d : 0M).ToString();
                data.SXPZSL = list.Sum(t => decimal.TryParse(t.SXPZSL, out decimal d) ? d : 0M).ToString();
                data.YXWMXS = list.Sum(t => decimal.TryParse(t.YXWMXS, out decimal d) ? d : 0M).ToString();
                data.JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString();
                data.DW = list[0].DW;
                data.XW = list[0].XW;
                data.CXPZ = list[0].CXPZ;
                data.SXPZ = list[0].SXPZ;
                datas.Add(data);
            }
            datas.Insert(0, MyDal.GetTotalDataBase1MJ_YMJJ(datas));
            datas.Insert(1, new DataBase1MJ_YMJJ { GW = "运模单价", CHQX1 = baseQX, CHDX1 = baseDX, KMCHQX1 = baseKMQX, KMCHDX1 = baseKMDX });
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.LimeGreen;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Blue;
            dgv.Rows[1].Frozen = true;
            dgv.Rows[1].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void RefGrid(List<DataBase1MJ_YMJJ> list, DateTime dateTime)
        {
            var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dateTime.Year && h.CreateMonth == dateTime.Month && h.FactoryNo == "G001" && h.WorkshopName == "模具车间" && h.Classification == "运模" && h.PostName == "运模工").ToList();
            string baseQX = "0";
            string baseDX = "0";
            string baseKMQX = "";
            string baseKMDX = "0";
            if (listMonth == null || listMonth.Count == 0)
            {
                MessageBox.Show("没有" + dateTime.Year + "年" + dateTime.Month + "月的指标数据，先导入指标数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                baseQX = listMonth.Where(h => h.TypesType == "撤换全线").FirstOrDefault().UnitPrice;
                baseDX = listMonth.Where(h => h.TypesType == "撤/换单线").FirstOrDefault().UnitPrice;
                baseKMQX = listMonth.Where(h => h.TypesType == "科玛全包分体撤换全线").FirstOrDefault().UnitPrice;
                baseKMDX = listMonth.Where(h => h.TypesType == "科玛全包分体撤/换单线").FirstOrDefault().UnitPrice;
            }


            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            list.Insert(0, MyDal.GetTotalDataBase1MJ_YMJJ(list));
            list.Insert(1, new DataBase1MJ_YMJJ { No = "运模单价", CHQX1 = baseQX, CHDX1 = baseDX, KMCHQX1 = baseKMQX, KMCHDX1 = baseKMDX });
            dgv.DataSource = list;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[1].Frozen = true;
            dgv.Rows[1].DefaultCellStyle.BackColor = Color.LimeGreen;
            dgv.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Blue;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
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