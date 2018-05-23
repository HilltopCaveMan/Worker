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
    public partial class Frm3JB_MCLBJJ : Office2007Form
    {
        private BaseDal<DataBaseDay> dalDay = new BaseDal<DataBaseDay>();
        private ExcelHelper<DataBase3JB_MCLBJJ> excelHelper = new ExcelHelper<DataBase3JB_MCLBJJ>();
        private string[] header;
        private DateTime selectTime;

        public Frm3JB_MCLBJJ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3JB_MCLBJJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            if (datas.Count > 0)
            {
                lblMc1.Text = datas[0].IsOkMc1 ? "成检记工验证成功" : "成检记工未验证或失败";
                lblMc1.BackColor = datas[0].IsOkMc1 ? Color.Green : Color.Red;
                lblMc2.Text = datas[0].IsOkMc2 ? "磨瓷验证成功" : "磨瓷未验证或失败";
                lblMc2.BackColor = datas[0].IsOkMc2 ? Color.Green : Color.Red;
                lblLb1.Text = datas[0].IsOkLb1 ? "冷补验证成功" : "冷补未验证或失败";
                lblLb1.BackColor = datas[0].IsOkLb1 ? Color.Green : Color.Red;
                lblLb2.Text = datas[0].IsOkLb2 ? "冷补计工验证成功" : "冷补计工未验证或失败";
                lblLb2.BackColor = datas[0].IsOkLb2 ? Color.Green : Color.Red;
                lblPg.Text = datas[0].IsOkYkcpg ? "配盖验证成功" : "配盖未验证或失败";
                lblPg.BackColor = datas[0].IsOkYkcpg ? Color.Green : Color.Red;
            }
            datas.Insert(0, MyDal.GetTotalDataBase3JB_MCLBJJ(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 10; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[10].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "cTime$cUser$isOKmc1$isOKmc2$isOkgp$isOklb1$isOklb2$年$月$序号$人员编码$姓名$品种$磨瓷数量$原库存配盖数量$冷补数量$磨瓷单价$原库存配盖单价$冷补单价$磨瓷金额$原库存金额$冷补金额$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text);
        }

        private void BtnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——磨瓷冷补.xlsx");
        }

        private bool Recount(List<DataBase3JB_MCLBJJ> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var bDayMc = dalDay.Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "磨瓷" && t.TypesName == item.PZ);
                    var bDayYkcpg = dalDay.Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "原库存配盖" && t.TypesName == item.PZ);
                    var bDayLb = dalDay.Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "冷补" && t.TypesName == item.PZ);
                    item.McUnitPrice = bDayMc == null ? "0" : bDayMc.UnitPrice;
                    item.YkcpgUnitPrice = bDayYkcpg == null ? "0" : bDayYkcpg.UnitPrice;
                    item.LbUnitPrice = bDayLb == null ? "0" : bDayLb.UnitPrice;
                    item.McMoney = string.IsNullOrEmpty(item.McCount) ? string.Empty : (Convert.ToDecimal(item.McCount) * Convert.ToDecimal(item.McUnitPrice)).ToString();
                    item.YkcpgMoney = string.IsNullOrEmpty(item.YkcpgCount) ? string.Empty : (Convert.ToDecimal(item.YkcpgCount) * Convert.ToDecimal(item.YkcpgUnitPrice)).ToString();
                    item.LbMoney = string.IsNullOrEmpty(item.LbCount) ? string.Empty : (Convert.ToDecimal(item.LbCount) * Convert.ToDecimal(item.LbUnitPrice)).ToString();
                    item.Money = ((string.IsNullOrEmpty(item.McMoney) ? 0m : Convert.ToDecimal(item.McMoney)) + (string.IsNullOrEmpty(item.YkcpgMoney) ? 0m : Convert.ToDecimal(item.YkcpgMoney)) + (string.IsNullOrEmpty(item.LbMoney) ? 0m : Convert.ToDecimal(item.LbMoney))).ToString();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_MCLBJJ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = excelHelper.ReadExcel(openFileDlg.FileName, 2, 10);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                foreach (var item in list)
                {
                    if (!MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }

                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = selectTime.Year;
                    item.TheMonth = selectTime.Month;
                }
                if (Recount(list) && new BaseDal<DataBase3JB_MCLBJJ>().Add(list) > 0)
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

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_MCLBJJ> list = dgv.DataSource as List<DataBase3JB_MCLBJJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (excelHelper.WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——磨瓷冷补.xlsx", saveFileDlg.FileName, list, 2, 10, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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

        private void BtnNew_Click(object sender, EventArgs e)
        {
            DataBase3JB_MCLBJJ dataBase3JB_MCLBJJ = new DataBase3JB_MCLBJJ() { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month };
            FrmModify<DataBase3JB_MCLBJJ> frm = new FrmModify<DataBase3JB_MCLBJJ>(dataBase3JB_MCLBJJ, header, OptionType.Add, Text, 7, 7);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
            }
        }

        private void BtnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3JB_MCLBJJ> list = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3JB_MCLBJJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var dataBase3JB_MCLBJJ = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_MCLBJJ;
                if (dataBase3JB_MCLBJJ.IsOkMc1 && dataBase3JB_MCLBJJ.IsOkMc2 && dataBase3JB_MCLBJJ.IsOkYkcpg && dataBase3JB_MCLBJJ.IsOkLb1 && dataBase3JB_MCLBJJ.IsOkLb2)
                {
                    MessageBox.Show("错误：已经核对正确，不能修改？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (dataBase3JB_MCLBJJ != null)
                {
                    if (dataBase3JB_MCLBJJ.PZ == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_MCLBJJ> frm = new FrmModify<DataBase3JB_MCLBJJ>(dataBase3JB_MCLBJJ, header, OptionType.Modify, Text, 9, 7);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var dataBase3JB_MCLBJJ = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_MCLBJJ;
                if (dataBase3JB_MCLBJJ.IsOkMc1 && dataBase3JB_MCLBJJ.IsOkMc2 && dataBase3JB_MCLBJJ.IsOkYkcpg && dataBase3JB_MCLBJJ.IsOkLb1 && dataBase3JB_MCLBJJ.IsOkLb2)
                {
                    MessageBox.Show("错误：已经核对正确，不能修改？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (dataBase3JB_MCLBJJ != null)
                    {
                        if (dataBase3JB_MCLBJJ.PZ == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_MCLBJJ where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString()) > 0)
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
                        FrmModify<DataBase3JB_MCLBJJ> frm = new FrmModify<DataBase3JB_MCLBJJ>(dataBase3JB_MCLBJJ, header, OptionType.Delete, Text, 7, 7);
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