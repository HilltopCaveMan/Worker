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
    public partial class Frm3JB_XCSJ_WX : Office2007Form
    {
        private string[] header;

        public Frm3JB_XCSJ_WX()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3JB_XCSJ_WX_Load(object sender, EventArgs e)
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
            var datas = new BaseDal<DataBase3JB_XCSJ_WX>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_XCSJ_WX(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[6].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$岗位名称$员工编码$姓名$熟练工天数$满勤天数$基本工资$实际工资".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——叉车司机外协月录入.xlsx");
        }

        private bool Recount(List<DataBase3JB_XCSJ_WX> list)
        {
            try
            {
                foreach (var item in list)
                {
                    if (item.Gwmc == "外协")
                    {
                        item.MqDays = MyDal.GetMonthTotalDays(dtp.Value).ToString();
                    }
                    else
                    {
                        item.MqDays = (MyDal.GetMonthTotalDays(dtp.Value) - MyDal.GetDaysByWeek(dtp.Value, DayOfWeek.Sunday)).ToString();
                    }

                    item.BaseSalary = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == item.Gwmc).MoneyBase;
                    if (string.IsNullOrEmpty(item.MqDays))
                    {
                        item.Money = string.Empty;
                    }
                    else
                    {
                        item.Money = ((string.IsNullOrEmpty(item.BaseSalary) ? 0M : Convert.ToDecimal(item.BaseSalary)) * (string.IsNullOrEmpty(item.SlgDays) ? 0M : Convert.ToDecimal(item.SlgDays)) / (Convert.ToDecimal(item.MqDays))).ToString();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool YZ_WX(List<DataBase3JB_XCSJ_WX> list)
        {
            var list2 = list.Where(t => t.Gwmc == "外协").ToList().Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays));
            var mq = list.Where(t => t.Gwmc == "外协").FirstOrDefault();
            if (mq != null)
            {
                var list3 = string.IsNullOrEmpty(mq.MqDays) ? 0M : Convert.ToDecimal(mq.MqDays);
                var list5 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "外协" && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0 : Convert.ToDecimal(t.UserCount));
                var result = list2 / list3 <= list5;
                if (!result)
                {
                    MessageBox.Show("公司定员：外协，合计：【" + list5.ToString() + "】人" + Environment.NewLine + "导入数据：外协，合计：【" + (list2 / list3).ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return result;
            }
            else
            {
                MessageBox.Show("导入数据没有【外协】，无法验证", "【外协】无法验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
        }

        private bool YZ_XC(List<DataBase3JB_XCSJ_WX> list)
        {
            var list2 = list.Where(t => t.Gwmc == "叉车司机").ToList().Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays));
            var mq = list.Where(t => t.Gwmc == "叉车司机").FirstOrDefault();
            if (mq != null)
            {
                var list3 = string.IsNullOrEmpty(mq.MqDays) ? 0M : Convert.ToDecimal(mq.MqDays);
                var list5 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "叉车司机" && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0 : Convert.ToDecimal(t.UserCount));
                var result = list2 / list3 <= list5;
                if (!result)
                {
                    MessageBox.Show("公司定员：叉车司机，合计：【" + list5.ToString() + "】人" + Environment.NewLine + "导入数据：叉车司机，合计：【" + (list2 / list3).ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return result;
            }
            else
            {
                MessageBox.Show("导入数据没有【叉车司机】，无法验证", "【叉车司机】无法验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_XCSJ_WX> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3JB_XCSJ_WX>().ReadExcel(openFileDlg.FileName, 2, 5);
                if (list == null)
                {
                    Enabled = true;
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
                if (Recount(list) && YZ_WX(list) && YZ_XC(list))
                {
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
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase3JB_XCSJ_WX>().Add(list) > 0)
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
                List<DataBase3JB_XCSJ_WX> list = dgv.DataSource as List<DataBase3JB_XCSJ_WX>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_XCSJ_WX>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——叉车司机外协月.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase3JB_XCSJ_WX DataBase3JB_XCSJ_WX = new DataBase3JB_XCSJ_WX() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3JB_XCSJ_WX> frm = new FrmModify<DataBase3JB_XCSJ_WX>(DataBase3JB_XCSJ_WX, header, OptionType.Add, Text, 4, 2);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3JB_XCSJ_WX> list = new BaseDal<DataBase3JB_XCSJ_WX>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3JB_XCSJ_WX>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var DataBase3JB_XCSJ_WX = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XCSJ_WX;
                if (DataBase3JB_XCSJ_WX != null)
                {
                    if (DataBase3JB_XCSJ_WX.UserCode == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_XCSJ_WX> frm = new FrmModify<DataBase3JB_XCSJ_WX>(DataBase3JB_XCSJ_WX, header, OptionType.Modify, Text, 4, 2);
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
                var DataBase3JB_XCSJ_WX = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XCSJ_WX;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3JB_XCSJ_WX != null)
                    {
                        if (DataBase3JB_XCSJ_WX.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_XCSJ_WX where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase3JB_XCSJ_WX> frm = new FrmModify<DataBase3JB_XCSJ_WX>(DataBase3JB_XCSJ_WX, header, OptionType.Delete, Text, 4, 2);
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