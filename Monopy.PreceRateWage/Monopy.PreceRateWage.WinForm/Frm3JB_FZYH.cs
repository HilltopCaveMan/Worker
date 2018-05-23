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
    public partial class Frm3JB_FZYH : Office2007Form
    {
        private string[] header;

        public Frm3JB_FZYH()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                return;
            }
            System.Reflection.FieldInfo[] fieldInfo = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ContextMenuStrip> contextMenuStrip = new List<ContextMenuStrip>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfo[i].FieldType == typeof(ButtonX))
                {
                    ((ButtonX)fieldInfo[i].GetValue(this)).Enabled = false;
                    //Controls.Find(fieldInfo[i].Name, true)[0].Enabled = false;
                    continue;
                }

                if (fieldInfo[i].FieldType == typeof(ContextMenuStrip))
                {
                    var tmp = (ContextMenuStrip)fieldInfo[i].GetValue(this);
                    contextMenuStrip.Add(tmp);
                    foreach (var item in tmp.Items)
                    {
                        if (item is ToolStripMenuItem t)
                        {
                            t.Enabled = false;
                        }
                    }
                }
            }

            var dic = MenuBarHelper.FrmControlEnable(Program.User.Code);
            foreach (var item in dic.Keys)
            {
                var cons = Controls.Find(item, true);
                if (cons.Length > 0)
                {
                    cons[0].Enabled = dic[item];
                    continue;
                }
                foreach (var contextMs in contextMenuStrip)
                {
                    foreach (var item2 in contextMs.Items)
                    {
                        if (item2 is ToolStripMenuItem x && x.Name == item)
                        {
                            x.Enabled = dic[item];
                        }
                    }
                }
            }
        }

        private void Frm3JB_FZYH_Load(object sender, EventArgs e)
        {
            UiControl();
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
            var datas = new BaseDal<DataBase3JB_FZYH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_FZYH(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[7].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$岗位名称$员工编码$姓名$熟练工天数$性别$单价$是否外协$工资".Split('$');
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
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——辅助验货月录入.xlsx");
        }

        private bool Recount(List<DataBase3JB_FZYH> list)
        {
            try
            {
                var male = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "辅助验货" && t.Gender == "男");
                var female = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "辅助验货" && t.Gender == "女");
                foreach (var item in list)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ERPContext"].ConnectionString))
                    {
                        object o = conn.ExecuteScalar("select top 1 rsex from hr_hi_person where cpsn_num='" + item.UserCode + "'");
                        if (o == null)
                        {
                            MessageBox.Show(item.UserCode + item.UserName + "ERP中无员工" + item.UserCode + "_" + item.UserName + ",信息,无法计算！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        item.Gander = o.ToString() == "1" ? "男" : "女";
                    }
                    item.Price = item.Gander == "男" ? male.DayWork_FZYH : female.DayWork_FZYH;
                    item.IsXcOrWx = new BaseDal<DataBase3JB_XCSJ_WX>().Get(t => t.UserCode == item.UserCode && t.Gwmc == "外协" && t.TheYear == item.TheYear && t.TheMonth == item.TheMonth) != null;
                    //改为（外协实出勤天数+人员出勤天数（熟练工+学徒））=外协应出勤天数，那么辅助验货的金额/2；
                    //如果出勤天数《应出勤天数，那么辅助验货的天数3种情况，1：实出勤+辅助验货天数《应出勤。那么辅助验货金额不变；2：实出勤+辅助验货天数》应出勤，应出勤-实出勤的天数，对应的辅助验货天数全额发放。其他辅助验货天数=一半。
                    decimal.TryParse(item.SlgDays, out decimal dayFzyh);
                    if (item.IsXcOrWx)
                    {
                        decimal dayYcq = MyDal.GetMonthTotalDays(new DateTime(item.TheYear, item.TheMonth, 1));
                        var dayWx_SCQ = new BaseDal<DataBase3JB_XCSJ_WX>().GetList(t => t.UserCode == item.UserCode && t.TheYear == item.TheYear && t.TheMonth == item.TheMonth).ToList().Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays));
                        var dayCq = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => t.UserCode == item.UserCode && t.TheYear == item.TheYear && t.TheMonth == item.TheMonth).ToList().Sum(t => (string.IsNullOrEmpty(t.StudyDay) ? 0M : Convert.ToDecimal(t.StudyDay)) + (string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay)));
                        var dayScq = dayWx_SCQ + dayCq;
                        if (dayScq == dayYcq)
                        {
                            item.Money = ((item.Gander == "男" ? Convert.ToDecimal(male.DayWork_FZYH) : Convert.ToDecimal(female.DayWork_FZYH)) * dayFzyh * (item.Gander == "男" ? Convert.ToDecimal(male.MoneyBase) : Convert.ToDecimal(female.MoneyBase))).ToString();
                        }
                        else if (dayScq < dayYcq)
                        {
                            if (dayScq + dayFzyh <= dayYcq)
                            {
                                item.Money = ((item.Gander == "男" ? Convert.ToDecimal(male.DayWork_FZYH) : Convert.ToDecimal(female.DayWork_FZYH)) * dayFzyh).ToString();
                            }
                            else
                            {
                                var dayF1 = dayYcq - dayScq;
                                var dayF2 = dayFzyh - dayF1;
                                item.Money = ((item.Gander == "男" ? Convert.ToDecimal(male.DayWork_FZYH) : Convert.ToDecimal(female.DayWork_FZYH)) * dayF1 + (item.Gander == "男" ? Convert.ToDecimal(male.DayWork_FZYH) : Convert.ToDecimal(female.DayWork_FZYH)) * dayF2 * (item.Gander == "男" ? Convert.ToDecimal(male.MoneyBase) : Convert.ToDecimal(female.MoneyBase))).ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("工号：" + item.UserCode + ",姓名:" + item.UserName + ",的外协出勤天数为：" + dayWx_SCQ.ToString() + ",线位出勤天数为：" + dayCq.ToString() + "大于本月总天数：" + dayYcq.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        item.Money = ((item.Gander == "男" ? Convert.ToDecimal(male.DayWork_FZYH) : Convert.ToDecimal(female.DayWork_FZYH)) * dayFzyh).ToString();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool YZ1(List<DataBase3JB_FZYH> list)
        {
            var list2 = Convert.ToDecimal(MyDal.GetTotalDataBase3JB_FZYH(list, dtp.Value).SlgDays);
            var mq = MyDal.GetMonthTotalDays(dtp.Value) - MyDal.GetDaysByWeek(dtp.Value, DayOfWeek.Sunday);
            var list5 = new BaseDal<BaseHeadcount>().GetList(t => t.Factory == "三厂" && t.DeptName == "检包车间" && t.Name == "辅助验货" && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0 : Convert.ToDecimal(t.UserCount));
            var result = list2 / mq <= list5;
            if (!result)
            {
                MessageBox.Show("公司定员：辅助验货员，合计：【" + list5.ToString() + "】人" + Environment.NewLine + "导入数据：辅助验货员，合计：【" + (list2 / mq).ToString() + "】人", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private bool YZ2(List<DataBase3JB_FZYH> list)
        {
            foreach (var item in list)
            {
                var yz = new BaseDal<DataBase3JB_PGYHZTS>().Get(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.UserCode == item.UserCode);
                if (yz != null)
                {
                    var yz2 = string.IsNullOrEmpty(yz.Days) ? 0M : Convert.ToDecimal(yz.Days);
                    var sj = string.IsNullOrEmpty(item.SlgDays) ? 0M : Convert.ToDecimal(item.SlgDays);
                    if (sj > yz2)
                    {
                        MessageBox.Show(item.UserCode + "," + item.UserName + "，品管验货：天数:" + yz2.ToString() + Environment.NewLine + "导入数据:天数：" + sj.ToString(), "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(item.UserCode + "," + item.UserName + "，没有品管验货数据！", "验证失败，无法导入，请检查！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            var datas = new BaseDal<DataBase3JB_PGYHZTS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (datas.Count == 0)
            {
                MessageBox.Show("没有【品管验货总人数】，无法导入【辅助验货】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<DataBase3JB_FZYH> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3JB_FZYH>().ReadExcel(openFileDlg.FileName, 2, 5);
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
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                }
                if (YZ1(list) && YZ2(list))
                {
                    if (Recount(list) && new BaseDal<DataBase3JB_FZYH>().Add(list) > 0)
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
                    MessageBox.Show("验证失败，无法导入！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<DataBase3JB_FZYH> list = dgv.DataSource as List<DataBase3JB_FZYH>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_FZYH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——辅助验货月.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            DataBase3JB_FZYH DataBase3JB_FZYH = new DataBase3JB_FZYH() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3JB_FZYH> frm = new FrmModify<DataBase3JB_FZYH>(DataBase3JB_FZYH, header, OptionType.Add, Text, 4, 4);
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
            List<DataBase3JB_FZYH> list = new BaseDal<DataBase3JB_FZYH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (Recount(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase3JB_FZYH>().Edit(item);
                }
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
                var DataBase3JB_FZYH = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_FZYH;
                if (DataBase3JB_FZYH != null)
                {
                    if (DataBase3JB_FZYH.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_FZYH> frm = new FrmModify<DataBase3JB_FZYH>(DataBase3JB_FZYH, header, OptionType.Modify, Text, 4, 4);
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
                var DataBase3JB_FZYH = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_FZYH;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3JB_FZYH != null)
                    {
                        if (DataBase3JB_FZYH.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_FZYH where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase3JB_FZYH> frm = new FrmModify<DataBase3JB_FZYH>(DataBase3JB_FZYH, header, OptionType.Delete, Text, 4, 4);
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