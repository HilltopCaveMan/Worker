using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3JB_XWRYCQ : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public Frm3JB_XWRYCQ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3JB_XWRYCQ_Load(object sender, EventArgs e)
        {
            dtp.text = Program.NowTime.ToString("yyyy-M-d");
            InitUI();
            btnSearch.PerformClick();
        }

        private void InitUI()
        {
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            var list = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day).DistinctBy(t => t.XW).OrderBy(t => t.XW).ToList();
            list.Insert(0, new DataBase3JB_XWRYCQ() { XW = "全部" });
            cmbLine.DataSource = list;
            cmbLine.DisplayMember = "XW";
            cmbLine.ValueMember = "XW";
        }

        private void RefDgv(int year, int month, string line, string userCode, string userName)
        {
            var list = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => line != "全部" ? t.XW == line && t.TheYear == year && t.TheMonth == month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) : t.TheYear == year && t.TheMonth == month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().GroupBy(t => new { t.XWType, t.XW, t.GWMC, t.UserCode, t.UserName }).Select(t => new DataBase3JB_XWRYCQ { XWType = t.Key.XWType, XW = t.Key.XW, GWMC = t.Key.GWMC, UserCode = t.Key.UserCode, UserName = t.Key.UserName, TheYear = year, TheMonth = month, TheDay = 0, StudyDay = t.Sum(x => string.IsNullOrEmpty(x.StudyDay) ? 0M : Convert.ToDecimal(x.StudyDay)).ToString(), WorkDay = t.Sum(x => string.IsNullOrEmpty(x.WorkDay) ? 0m : Convert.ToDecimal(x.WorkDay)).ToString(), DGWGZ = t.Sum(x => string.IsNullOrEmpty(x.DGWGZ) ? 0M : Convert.ToDecimal(x.DGWGZ)).ToString(), RZBZGZ = t.Sum(x => string.IsNullOrEmpty(x.RZBZGZ) ? 0M : Convert.ToDecimal(x.RZBZGZ)).ToString(), TBGZE = t.Sum(x => string.IsNullOrEmpty(x.TBGZE) ? 0M : Convert.ToDecimal(x.TBGZE)).ToString() }).OrderBy(t => t.XWType).ThenBy(t => t.XW).ThenBy(t => t.GWMC).ThenBy(t => t.UserCode).ToList();
            list.Insert(0, MyDal.GetTotalDataBase3JB_XWRYCQ(list, selectTime));
            dgv.DataSource = list;
            for (int i = 0; i < 7; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            ////for (int i = 14; i < dgv.Columns.Count; i++)
            ////{
            ////    dgv.Columns[i].Visible = false;
            ////}
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间	操作人	年	月	日	序号	线位占比线位	线位	工位名称	工号	姓名	学徒天数	熟练工天数	工资占比	总工资	岗位总出勤	单岗位工资	学徒日工	入职补助工资	加班工资	替班工资额".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void RefDgv(DateTime selectTime, string line, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = true;
            }
            var list = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => line != "全部" ? t.XW == line && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) : t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            list.Insert(0, MyDal.GetTotalDataBase3JB_XWRYCQ(list, selectTime));
            dgv.DataSource = list;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间	操作人	年	月	日	序号	线位占比线位	线位	工位名称	工号	姓名	学徒天数	熟练工天数	工资占比	总工资	岗位总出勤	单岗位工资	学徒日工	入职补助工资	加班工资	替班工资额".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private bool CheckTime(string strTime)
        {
            if (string.IsNullOrEmpty(strTime) || !DateTime.TryParse(strTime, out selectTime))
            {
                MessageBox.Show("请选择日期，或日期错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp.Focus();
                return false;
            }
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            RefDgv(selectTime, cmbLine.Text, txtUserCode.Text, txtUserName.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——线位人员出勤日录入日计算.xlsx");
        }

        private void RecountSetp1(List<DataBase3JB_XWRYCQ> list)
        {
            bool isAllIn = true;
            foreach (var item in list)
            {
                DataBaseDay dataBaseDay = new BaseDal<DataBaseDay>().Get(t => t.WorkshopName == "检包车间" && t.PostName == item.GWMC && t.JBXW == item.XWType);
                if (dataBaseDay == null)
                {
                    isAllIn = false;
                    continue;
                }
                item.GZZB = dataBaseDay.ZB_JB_JJGZ;
                List<DataBase3JB_XWRKHGP> dataBase3JB_XWRKHGPs = new BaseDal<DataBase3JB_XWRKHGP>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.TheDay == item.TheDay).ToList();
                if (dataBase3JB_XWRKHGPs.Count == 0)
                {
                    isAllIn = false;
                    continue;
                }
                DataBase3JB_XWRKHGP dataBase3JB_XWRKHGP = MyDal.GetTotalDataBase_JB_XWRKHGP(dataBase3JB_XWRKHGPs, selectTime);
                switch (item.XW)
                {
                    case "1":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L1;
                        break;

                    case "2":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L2;
                        break;

                    case "3":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L3;
                        break;

                    case "4":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L4;
                        break;

                    case "5":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L5;
                        break;

                    case "6":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L6;
                        break;

                    case "7":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L7;
                        break;

                    case "8":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L8;
                        break;

                    case "9":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L9;
                        break;

                    case "10":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L10;
                        break;

                    case "11":
                        item.TotalGZ = dataBase3JB_XWRKHGP.L11;
                        break;

                    default:
                        item.TotalGZ = dataBase3JB_XWRKHGP.L1_8;
                        break;
                }
                var tmpp = list.Where(t => t.XWType == item.XWType && t.XW == item.XW && t.GWMC == item.GWMC && !string.IsNullOrEmpty(t.WorkDay));

                item.GWZCQ = tmpp.Sum(t => string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay)).ToString();
                item.DGWGZ = (string.IsNullOrEmpty(item.GZZB) || string.IsNullOrEmpty(item.TotalGZ) || string.IsNullOrEmpty(item.GWZCQ) || string.IsNullOrEmpty(item.WorkDay)) ? string.Empty : ((Convert.ToDecimal(item.GZZB) * Convert.ToDecimal(item.TotalGZ)) / Convert.ToDecimal(item.GWZCQ) * Convert.ToDecimal(item.WorkDay)).ToString();
                DataBaseMonth dataBaseMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == item.GWMC);
                if (dataBaseMonth == null)
                {
                    isAllIn = false;
                    continue;
                }
                item.XTRG = dataBaseMonth.DayWork_XT;
                item.RZBZGZ = string.IsNullOrEmpty(item.XTRG) || string.IsNullOrEmpty(item.StudyDay) ? string.Empty : (Convert.ToDecimal(item.XTRG) * Convert.ToDecimal(item.StudyDay)).ToString();
            }

            if (!isAllIn)
            {
                MessageBox.Show("注意：【线位入库合格品】或【基础资料】数据不完整！工资无法计算，请检查录入是否有误，PMC是否数量完成，基础数据是否完整！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RecountSetp2(List<DataBase3JB_XWRYCQ> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (list.Where(t => t.XWType == item.XWType && t.XW == item.XW && t.GWMC == item.UserName).Count() > 0)
                {
                    var listJB = list.Where(t => t.XWType == item.XWType && t.XW == item.XW && t.GWMC == item.UserName).ToList();
                    if (listJB.Count == 0)
                    {
                        continue;
                    }
                    listJB = list.Where(t => t.XWType == item.XWType && t.XW == item.XW && t.UserName == item.UserName).ToList();
                    decimal totalJBGZ = listJB.Sum(t => string.IsNullOrEmpty(t.DGWGZ) ? 0 : Convert.ToDecimal(t.DGWGZ));
                    for (int j = 0; j < list.Count; j++)
                    {
                        var a = list[j];
                        if (a.XWType == item.XWType && a.XW == item.XW && a.GWMC == item.UserName)
                        {
                            a.JBGZ = totalJBGZ == 0 ? string.Empty : totalJBGZ.ToString();
                            a.TBGZE = string.IsNullOrEmpty(a.JBGZ) || string.IsNullOrEmpty(a.GWZCQ) || string.IsNullOrEmpty(a.WorkDay) ? string.Empty : (Convert.ToDecimal(a.JBGZ) / Convert.ToDecimal(a.GWZCQ) * Convert.ToDecimal(a.WorkDay)).ToString();
                        }
                    }
                }
            }
        }

        private bool YZ1(List<DataBase3JB_XWRYCQ> list)
        {
            var list2 = list.Where(t => t.GWMC != "配送" && t.GWMC != "装袋" && !(t.XWType == "1-7" && t.GWMC == "试气")).GroupBy(t => new { t.XWType, t.XW, t.GWMC }).Select(t => new { XWType = t.Key.XWType, XW = t.Key.XW, GWMC = t.Key.GWMC, hj = t.Sum(x => string.IsNullOrEmpty(x.WorkDay) ? 0M : Convert.ToDecimal(x.WorkDay)) });
            foreach (var item in list2)
            {
                var headcount = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Line == item.XW && t.Name == item.GWMC && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).FirstOrDefault();
                if (headcount == null || string.IsNullOrEmpty(headcount.UserCount) || Convert.ToDecimal(headcount.UserCount) < item.hj)
                {
                    //验证失败
                    MessageBox.Show("公司定员：线位：【" + headcount.Line + "】工种：【" + headcount.Name + "】定员：【" + headcount.UserCount + "】" + Environment.NewLine + "导入数据：线位：【" + item.XW + "】工种：【" + item.GWMC + "】熟练工天数：【" + item.hj + "】", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool YZ2(List<DataBase3JB_XWRYCQ> list)
        {
            var list2 = list.Where(t => t.XWType == "1-7" && t.GWMC == "试气" && t.UserCode.Length == 6);
            var tmp1 = list2.Count() == 0 ? 0M : list2.GroupBy(t => t.GWMC).Select(t => new { GWMC = t.Key, hj = t.Sum(x => string.IsNullOrEmpty(x.WorkDay) ? 0M : Convert.ToDecimal(x.WorkDay)) }).FirstOrDefault().hj;
            if (tmp1 == 0M)
            {
                return true;
            }
            var list3 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "试气" && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && new string[] { "1", "2", "3", "4", "5", "6", "7" }.Contains(t.Line)).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0M : Convert.ToDecimal(t.UserCount));
            var result = tmp1 <= list3;
            if (!result)
            {
                MessageBox.Show("公司定员：1-7线，试气合计：【" + list3.ToString() + "】人" + Environment.NewLine + "导入数据：1-7线，试气合计：【" + list2.ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private bool YZ3(List<DataBase3JB_XWRYCQ> list)
        {
            var list2 = list.Where(t => t.GWMC == "配送").Sum(t => string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay));
            var list4 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "配送" && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0M : Convert.ToDecimal(t.UserCount));
            var result = list2 <= list4;
            if (!result)
            {
                MessageBox.Show("公司定员：配送，合计：【" + list4.ToString() + "】人" + Environment.NewLine + "导入数据：配送，合计：【" + list2.ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private bool YZ4(List<DataBase3JB_XWRYCQ> list)
        {
            var list3 = list.Where(t => t.GWMC == "装袋").Sum(t => string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay));
            var list5 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "装袋" && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0 : Convert.ToDecimal(t.UserCount));
            var result = list3 <= list5;
            if (!result)
            {
                MessageBox.Show("公司定员：装袋，合计：【" + list5.ToString() + "】人" + Environment.NewLine + "导入数据：装袋，合计：【" + list3.ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private bool YZ5(List<DataBase3JB_XWRYCQ> list)
        {
            var list1 = list.Where(t => t.UserCode.Length == 6).GroupBy(t => t.UserCode).Select(t => new { UserCode = t.Key, Count = t.Sum(x => (string.IsNullOrEmpty(x.StudyDay) ? 0M : Convert.ToDecimal(x.StudyDay)) + (string.IsNullOrEmpty(x.WorkDay) ? 0M : Convert.ToDecimal(x.WorkDay))) }).Where(t => t.Count > 1M);
            if (list1.Count() > 0)
            {
                MessageBox.Show("导入的文档中：【" + list1.FirstOrDefault().UserCode + "】，天数大于1，无法导入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var list = new ExcelHelper<DataBase3JB_XWRYCQ>().ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    Enabled = true;
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName))
                    {
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
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
                    item.TheDay = selectTime.Day;
                }
                if (YZ1(list) && YZ2(list) && YZ3(list) && YZ4(list) && YZ5(list))
                {
                    //RecountSetp1(list);
                    //RecountSetp2(list);
                    if (new BaseDal<DataBase3JB_XWRYCQ>().Add(list) > 0)
                    {
                        Enabled = true;
                        InitUI();
                        btnSearch.PerformClick();
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                        MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                Enabled = true;
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_XWRYCQ> list = dgv.DataSource as List<DataBase3JB_XWRYCQ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_XWRYCQ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——线位人员出勤日录入日计算导出.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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
            if (!CheckTime(dtp.text))
            {
                return;
            }
            DataBase3JB_XWRYCQ dataBase3JB_XWRYCQ = new DataBase3JB_XWRYCQ() { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month, TheDay = selectTime.Day };
            FrmModify<DataBase3JB_XWRYCQ> frm = new FrmModify<DataBase3JB_XWRYCQ>(dataBase3JB_XWRYCQ, header, OptionType.Add, Text, 5, 8);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnSearch.PerformClick();
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_XWRYCQ> list = dgv.DataSource as List<DataBase3JB_XWRYCQ>;
            dgv.DataSource = null;
            Enabled = false;
            list.RemoveAt(0);
            RecountSetp1(list);
            RecountSetp2(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3JB_XWRYCQ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var dataBase3JB_XWRYCQ = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XWRYCQ;
                if (dataBase3JB_XWRYCQ != null)
                {
                    if (dataBase3JB_XWRYCQ.XWType == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_XWRYCQ> frm = new FrmModify<DataBase3JB_XWRYCQ>(dataBase3JB_XWRYCQ, header, OptionType.Modify, Text, 5, 8);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
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
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var dataBase3JB_XWRYCQ = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_XWRYCQ;
                    if (dataBase3JB_XWRYCQ != null)
                    {
                        if (dataBase3JB_XWRYCQ.XWType == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                List<DataBase3JB_XWRYCQ> list = dgv.DataSource as List<DataBase3JB_XWRYCQ>;
                                dgv.DataSource = null;
                                Enabled = false;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3JB_XWRYCQ>().Delete(item);
                                }
                                Enabled = true;
                                btnSearch.PerformClick();
                                MessageBox.Show("全部删除完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3JB_XWRYCQ> frm = new FrmModify<DataBase3JB_XWRYCQ>(dataBase3JB_XWRYCQ, header, OptionType.Delete, Text, 5, 8);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void btnMonthSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            RefDgv(selectTime.Year, selectTime.Month, cmbLine.Text, txtUserCode.Text, txtUserName.Text);
            dgv.ContextMenuStrip = null;
        }

        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            btnMonthSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_XWRYCQ> list = dgv.DataSource as List<DataBase3JB_XWRYCQ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_XWRYCQ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——线位人员出勤日录入日计算导出.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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

        private void buttonX1_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(-1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        private void dtp_SelectedValueChange(object sender, string Item)
        {
            CheckTime(dtp.text);
            InitUI();
        }
    }
}