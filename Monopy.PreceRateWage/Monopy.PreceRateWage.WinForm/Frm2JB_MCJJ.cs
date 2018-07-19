using Dapper;
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
    public partial class Frm2JB_MCJJ : Office2007Form
    {
        private BaseDal<DataBaseDay> dalDay = new BaseDal<DataBaseDay>();
        private ExcelHelper<DataBase2JB_MCJJ> excelHelper = new ExcelHelper<DataBase2JB_MCJJ>();
        private string[] header;
        private DateTime selectTime;

        public Frm2JB_MCJJ()
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
        private void Frm2JB_MCJJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——检包——磨瓷计件.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase2JB_MCJJ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = excelHelper.ReadExcel(openFileDlg.FileName, 2, 5);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {

                    if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                    {
                        list.RemoveAt(i);
                        if (i > 0)
                        {
                            i--;
                        }
                        else
                        {
                            i = -1;
                        }
                        continue;

                    }

                    if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }

                    list[i].CreateUser = Program.User.ToString();
                    list[i].CreateTime = Program.NowTime;
                    list[i].TheYear = dtp.Value.Year;
                    list[i].TheMonth = dtp.Value.Month;
                }
                if (Recount(list) && new BaseDal<DataBase2JB_MCJJ>().Add(list) > 0)
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase2JB_MCJJ> list = dgv.DataSource as List<DataBase2JB_MCJJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (excelHelper.WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——检包——磨瓷计件.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNew_Click(object sender, EventArgs e)
        {
            DataBase2JB_MCJJ DataBase2JB_MCJJ = new DataBase2JB_MCJJ() { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase2JB_MCJJ> frm = new FrmModify<DataBase2JB_MCJJ>(DataBase2JB_MCJJ, header, OptionType.Add, Text, 5, 5);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
            }
        }

        /// <summary>
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase2JB_MCJJ> list = new BaseDal<DataBase2JB_MCJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2JB_MCJJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase2JB_MCJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_MCJJ;
                if (DataBase2JB_MCJJ != null)
                {
                    if (DataBase2JB_MCJJ.PZ == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2JB_MCJJ> frm = new FrmModify<DataBase2JB_MCJJ>(DataBase2JB_MCJJ, header, OptionType.Modify, Text, 5, 5);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
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
                var DataBase2JB_MCJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_MCJJ;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2JB_MCJJ != null)
                    {
                        if (DataBase2JB_MCJJ.PZ == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase2JB_MCJJ where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase2JB_MCJJ> frm = new FrmModify<DataBase2JB_MCJJ>(DataBase2JB_MCJJ, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        #endregion

        #region 调用方法

        private void RefDgv(DateTime selectTime, string userCode)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2JB_MCJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            datas.Insert(0, MyDal.GetTotalDataBase2JB_MCJJ(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[10].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "cTime$cUser$年$月$序号$人员编码$姓名$品种$磨瓷数量$原库存磨瓷数量$磨瓷单价$原库存磨瓷单价$磨瓷金额$原库存金额$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private bool Recount(List<DataBase2JB_MCJJ> list)
        {
            try
            {

                var baseDay = dalDay.Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "检包车间");

                if (baseDay == null)
                {
                    MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (var item in list)
                {
                    var mCount = new BaseDal<DataBase2JB_PMCMC>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.LBBM.Contains(item.PZ));
                    var yCount = new BaseDal<DataBase2JB_PMCMCYKC>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.LBBM.Contains(item.PZ));
                    decimal m = 0M;
                    decimal y = 0M;

                    if (mCount == null && !string.IsNullOrEmpty(item.McCount))
                    {
                        MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的品种为：【" + item.PZ + "】磨瓷月报数据，无法计算，导入磨瓷月报后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (yCount == null && !string.IsNullOrEmpty(item.YkcpgCount))
                    {
                        MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的品种为：【" + item.PZ + "】磨瓷原库存月报数据，无法计算，导入磨瓷原库存月报后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (mCount != null)
                    {
                        decimal.TryParse(mCount.HG, out m);
                    }
                    if (yCount != null)
                    {
                        decimal.TryParse(yCount.HG, out y);
                    }


                    decimal.TryParse(item.McCount, out decimal mc);
                    decimal.TryParse(item.YkcpgCount, out decimal yc);
                    if (mc > m && m != 0)
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,品种：【" + item.PZ + "】的磨瓷数量大于PMC磨瓷月报中对应的合格数" + Environment.NewLine + "PMC磨瓷月报中合格数为：【" + m.ToString() + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (yc > y && y != 0)
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,品种：【" + item.PZ + "】的原库存磨瓷数量大于PMC磨瓷原库存月报中对应的合格数" + Environment.NewLine + "PMC磨瓷原库存月报中合格数为：【" + y.ToString() + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    var bDayMc = dalDay.Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "检包车间" && t.Classification == "磨瓷" && t.TypesName == item.PZ);
                    item.McUnitPrice = bDayMc == null ? "0" : bDayMc.UnitPrice;
                    item.YkcpgUnitPrice = bDayMc == null ? "0" : bDayMc.UnitPrice;

                    item.McMoney = string.IsNullOrEmpty(item.McCount) ? string.Empty : (Convert.ToDecimal(item.McCount) * Convert.ToDecimal(item.McUnitPrice)).ToString();
                    item.YkcpgMoney = string.IsNullOrEmpty(item.YkcpgCount) ? string.Empty : (Convert.ToDecimal(item.YkcpgCount) * Convert.ToDecimal(item.YkcpgUnitPrice)).ToString();
                    item.Money = ((string.IsNullOrEmpty(item.McMoney) ? 0m : Convert.ToDecimal(item.McMoney)) + (string.IsNullOrEmpty(item.YkcpgMoney) ? 0m : Convert.ToDecimal(item.YkcpgMoney))).ToString();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}