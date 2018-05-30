using Dapper;
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
    public partial class Frm1JB_PMCMC : Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$类别名称$存货编码$存货名称$类别编码$开窑量$合格$回磨$磨等外$其它$磨残$风惊$超标$磨修$原修$漏检$合格率$最终合格率".Split('$');

        public Frm1JB_PMCMC()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1JB_PMCMC_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbLB.Text, CmbCHMC.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——检包——磨瓷月报.xlsx");
        }

        /// <summary>
        /// 导入模板
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
                    List<DataBase1JB_PMCMC> list = new List<DataBase1JB_PMCMC>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i, j;
                        string lb = string.Empty;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            //IRow rowFirst = sheet.GetRow(1);
                            bool sign = true;
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            if (ExcelHelper.GetCellValue(row.GetCell(0)).Contains("类别名称") && lb != ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1])
                            {
                                sign = false;
                                lb = ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1];
                            }
                            if (sign)
                            {
                                DataBase1JB_PMCMC t = new DataBase1JB_PMCMC { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                                t.No = (i - 2).ToString();
                                t.LB = lb;
                                if (string.IsNullOrEmpty(ExcelHelper.GetCellValue(row.GetCell(2))))
                                {
                                    t.LBBM = lb + "合计";
                                }
                                else
                                {
                                    t.LBBM = ExcelHelper.GetCellValue(row.GetCell(3));
                                }
                                t.CHBM = ExcelHelper.GetCellValue(row.GetCell(1));
                                t.CHMC = ExcelHelper.GetCellValue(row.GetCell(2));
                                t.KYL = ExcelHelper.GetCellValue(row.GetCell(4));
                                t.HG = ExcelHelper.GetCellValue(row.GetCell(5));
                                t.HM = ExcelHelper.GetCellValue(row.GetCell(6));
                                t.MDW = ExcelHelper.GetCellValue(row.GetCell(7));
                                t.QT = ExcelHelper.GetCellValue(row.GetCell(8));
                                t.MC = ExcelHelper.GetCellValue(row.GetCell(9));
                                t.FJ = ExcelHelper.GetCellValue(row.GetCell(10));
                                t.CB = ExcelHelper.GetCellValue(row.GetCell(11));
                                t.MX = ExcelHelper.GetCellValue(row.GetCell(12));
                                t.YX = ExcelHelper.GetCellValue(row.GetCell(13));
                                t.LJ = ExcelHelper.GetCellValue(row.GetCell(14));
                                t.HGL = ExcelHelper.GetCellValue(row.GetCell(15));
                                t.ZZHGL = ExcelHelper.GetCellValue(row.GetCell(16));
                                list.Add(t);
                            }
                        }
                    }
                    if (new BaseDal<DataBase1JB_PMCMC>().Add(list) > 0)
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
                List<DataBase1JB_PMCMC> list = dgv.DataSource as List<DataBase1JB_PMCMC>;
                if (new ExcelHelper<DataBase1JB_PMCMC>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——检包——磨瓷月报.xlsx", saveFileDlg.FileName, list, 5, 6, 0, 0, 2, 6, dtp.Value.ToString("yyyy-MM"), 1))
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
                var DataBase1JB_PMCMC = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_PMCMC;
                if (DataBase1JB_PMCMC != null)
                {
                    if (DataBase1JB_PMCMC.LBBM.Contains("合计"))
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1JB_PMCMC> frm = new FrmModify<DataBase1JB_PMCMC>(DataBase1JB_PMCMC, header, OptionType.Modify, Text, 5, 0);
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
                var DataBase1JB_PMCMC = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_PMCMC;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1JB_PMCMC != null)
                    {
                        if (DataBase1JB_PMCMC.LBBM.Contains("合计"))
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有" + DataBase1JB_PMCMC.LB + "数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase1JB_PMCMC where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString() + "and LB = " + DataBase1JB_PMCMC.LB) > 0)
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
                        FrmModify<DataBase1JB_PMCMC> frm = new FrmModify<DataBase1JB_PMCMC>(DataBase1JB_PMCMC, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase1JB_PMCMC>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    new BaseDal<DataBase1JB_PMCMC>().Delete(item);
                }
                InitUI();
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }
        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase1JB_PMCMC>().GetList().ToList();
            RefCmbCHMC(list);
            RefCmbLB(list);
        }

        private void RefCmbCHMC(List<DataBase1JB_PMCMC> list)
        {
            var listTmp = list.GroupBy(t => t.CHMC).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbCHMC.DataSource = listTmp;
            CmbCHMC.DisplayMember = "CH";
            CmbCHMC.Text = "全部";
        }

        private void RefCmbLB(List<DataBase1JB_PMCMC> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefDgv(DateTime dtp, string lb, string chmc)
        {
            var datas = new BaseDal<DataBase1JB_PMCMC>().GetList(t => t.TheYear == dtp.Year && t.TheMonth == dtp.Month && (lb == "全部" ? true : t.LB.Contains(lb)) && (chmc == "全部" ? true : t.CHMC.Contains(chmc))).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();

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
