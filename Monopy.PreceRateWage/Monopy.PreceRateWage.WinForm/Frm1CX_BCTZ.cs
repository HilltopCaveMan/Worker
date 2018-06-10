using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm1CX_BCTZ : Office2007Form
    {
        public Frm1CX_BCTZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1CX_BCTZ_Load(object sender, EventArgs e)
        {
            dtp1.Value = new DateTime(Program.NowTime.AddYears(-1).Year, Program.NowTime.Month, 1);
            dtp2.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase1CX_BCTZ>().GetList(t => t.TimeBZ >= dtp1.Value && t.TimeBZ <= dtp2.Value && t.GD.Contains(CmbGD.Text) && t.XW.Contains(CmbXW.Text) && t.UserCode.Contains(CmbUserCode.Text) && t.UserName.Contains(CmbUserName.Text) && t.LB.Contains(CmbLB.Text)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            RefGrid(list);
        }

        /// <summary>
        /// 导入初始数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("导入初始数据，将会删除台账的所有数据，确定要删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                OpenFileDialog openFileDlg = new OpenFileDialog()
                {
                    Filter = "Excel文件|*.xlsx",
                };
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    ImportFile(openFileDlg.FileName);
                }
            }
        }

        private void dtp1_ValueChanged(object sender, EventArgs e)
        {
            InitUI();
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——变产补助台账.xlsx");
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            var list = new BaseDal<DataBase1CX_BCTZ>().GetList(t => t.TimeBZ >= dtp1.Value && t.TimeBZ <= dtp2.Value && t.GD.Contains(CmbGD.Text) && t.XW.Contains(CmbXW.Text) && t.UserCode.Contains(CmbUserCode.Text) && t.UserName.Contains(CmbUserName.Text) && t.LB.Contains(CmbLB.Text)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            var dt = GetDataTable(list);
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] header = "工段$线位$人员编号$姓名$原产品名称$变更产品名称$类别$变产时间$补助方案".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = $"梦牌一厂成型--上线补助台账-{dtp1.Value.Year}-{dtp1.Value.Month}至{dtp2.Value.Year}-{dtp2.Value.Month}.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "一厂上线补助台账", $"梦牌一厂成型--上线补助台账-{dtp1.Value.Year}-{dtp1.Value.Month}至{dtp2.Value.Year}-{dtp2.Value.Month}");
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
        }

        #endregion

        #region 调用方法

        private void RefCmbGD(List<DataBase1CX_BCTZ> list)
        {
            var listTmp = list.GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGD.DataSource = listTmp;
            CmbGD.DisplayMember = "GD";
            CmbGD.Text = string.Empty;
        }

        private void RefCmbXW(List<DataBase1CX_BCTZ> list)
        {
            var listTmp = list.GroupBy(t => t.XW).Select(t => t.Key).OrderBy(t => int.TryParse(t, out int it) ? it : int.MaxValue).ThenBy(t => t).ToList();
            CmbXW.DataSource = listTmp;
            CmbXW.DisplayMember = "XW";
            CmbXW.Text = string.Empty;
        }

        private void RefCmbUserCode(List<DataBase1CX_BCTZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase1CX_BCTZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void RefCmbLB(List<DataBase1CX_BCTZ> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase1CX_BCTZ>().GetList(t => t.TimeBZ >= dtp1.Value && t.TimeBZ <= dtp2.Value).ToList();
            RefCmbGD(list);
            RefCmbXW(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private DataTable GetDataTable(List<DataBase1CX_BCTZ> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            var listTmp = list.GroupBy(t => new { t.GD, t.XW, t.UserCode, t.UserName, t.BGCPMC, t.LB, t.BZFA }).Select(t => new DataBase1CX_BCTZ { GD = t.Key.GD, XW = t.Key.XW, UserCode = t.Key.UserCode, UserName = t.Key.UserName, YCPMC = t.Max(x => x.YCPMC), BGCPMC = t.Key.BGCPMC, LB = t.Key.LB, BCSJ = t.Max(x => x.BCSJ), BZFA = t.Key.BZFA, No = t.Min(x => int.TryParse(x.No, out int iNo) ? iNo : int.MaxValue).ToString() }).ToList();
            var listTmp2 = list.GroupBy(t => new { t.TheYear, t.TheMonth }).Select(t => new { t.Key.TheYear, t.Key.TheMonth }).OrderBy(t => t.TheYear).ThenBy(t => t.TheMonth).ToList();
            DataTable dt = DataTableHelper.GetDataTable(listTmp, 0);
            dt.Columns.Remove("Id");
            dt.Columns.Remove("CreateTime");
            dt.Columns.Remove("CreateUser");
            dt.Columns.Remove("TheYear");
            dt.Columns.Remove("TheMonth");
            dt.Columns.Remove("No");
            dt.Columns.Remove("TimeBZ");
            dt.Columns.Remove("Money");
            for (int i = 0; i < listTmp2.Count; i++)
            {
                int year = listTmp2[i].TheYear;
                int month = listTmp2[i].TheMonth;
                var columnsName = year.ToString() + "." + month.ToString();
                dt.Columns.Add(columnsName, typeof(string));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    var item = list.Where(t => t.TheYear == year && t.TheMonth == month && t.GD == dt.Rows[j]["GD"].ToString() && t.XW == dt.Rows[j]["XW"].ToString() && t.UserCode == dt.Rows[j]["UserCode"].ToString() && /*t.YCPMC == dt.Rows[j]["YCPMC"].ToString() &&*/ t.BGCPMC == dt.Rows[j]["BGCPMC"].ToString() && t.LB == dt.Rows[j]["LB"].ToString()).FirstOrDefault();
                    if (item != null)
                    {
                        dt.Rows[j][columnsName] = item.Money;
                    }
                }
            }

            DataRow dr = dt.NewRow();
            dr["GD"] = "合计";
            for (int i = 0; i < listTmp2.Count; i++)
            {
                int year = listTmp2[i].TheYear;
                int month = listTmp2[i].TheMonth;
                var columnsName = year.ToString() + "." + month.ToString();
                decimal d = 0M;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (decimal.TryParse(dt.Rows[j][columnsName].ToString(), out decimal dTmp))
                    {
                        d += dTmp;
                    }
                }
                dr[columnsName] = d.ToString();
            }
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        private void RefGrid(List<DataBase1CX_BCTZ> list)
        {
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            dgv.DataSource = null;
            dgv.DataSource = GetDataTable(list);
            if (dgv.DataSource != null)
            {
                string[] header = "工段$线位$人员编号$姓名$原产品名称$变更产品名称$类别$变产时间$补助方案".Split('$');
                for (int i = 0; i < header.Length; i++)
                {
                    dgv.Columns[i].HeaderText = header[i];
                }
                dgv.Rows[0].Frozen = true;
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
                dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
                dgv.ClearSelection();
            }
        }

        private List<DataBase1CX_BCTZ> ImportFile(string filePathName)
        {
            string sql = "delete from DataBase1CX_BCTZ";
            new BaseDal<DataBase1CX_BCTZ>().ExecuteSqlCommand(sql);
            var list = new List<DataBase1CX_BCTZ>();
            try
            {
                using (var fs = new FileStream(filePathName, FileMode.Open, FileAccess.Read))
                {
                    var workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    int i, j;
                    var rowTitle = sheet.GetRow(0);
                    var listTitl = new List<DateTime>();
                    for (i = rowTitle.FirstCellNum; i < rowTitle.LastCellNum; i++)
                    {
                        if (DateTime.TryParse(ExcelHelper.GetCellValue(rowTitle.Cells[i]), out DateTime date))
                        {
                            listTitl.Add(date);
                        }
                    }
                    int no = 1;
                    for (i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        for (j = 0; j < listTitl.Count; j++)
                        {
                            if (string.IsNullOrEmpty(ExcelHelper.GetCellValue(row.GetCell(9 + j))))
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(ExcelHelper.GetCellValue(row.GetCell(0))))
                            {
                                continue;
                            }
                            DataBase1CX_BCTZ t = new DataBase1CX_BCTZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = listTitl[j].Year, TheMonth = listTitl[j].Month, No = no.ToString() };
                            t.GD = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.XW = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.YCPMC = ExcelHelper.GetCellValue(row.GetCell(4));
                            t.BGCPMC = ExcelHelper.GetCellValue(row.GetCell(5));
                            t.LB = ExcelHelper.GetCellValue(row.GetCell(6));
                            t.BCSJ = ExcelHelper.GetCellValue(row.GetCell(7));
                            t.BZFA = ExcelHelper.GetCellValue(row.GetCell(8));
                            t.Money = ExcelHelper.GetCellValue(row.GetCell(9 + j));
                            t.TimeBZ = new DateTime(t.TheYear, t.TheMonth, 1);

                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Enabled = true;
                                return null;
                            }

                            list.Add(t);
                            no++;
                        }
                    }
                }
                if (new BaseDal<DataBase1CX_BCTZ>().Add(list) > 0)
                {
                    InitUI();
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
                MessageBox.Show($"导入失败，详细错误为：{ex.Message}");
            }
            return list;
        }

        #endregion










      
    }
}