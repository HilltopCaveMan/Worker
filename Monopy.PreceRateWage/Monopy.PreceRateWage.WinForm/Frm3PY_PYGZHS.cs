using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3PY_PYGZHS : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$月份$工厂编码$工号$员工编码$员工姓名$工段$存货类别$类别名称$开窑量$一级品$缺陷数$一级率$指标$奖励单价$惩罚单价$奖罚$单价$计件金额$总金额$类型$单价$指标0$指标1$指标2$指标3$指标4$指标5$奖励1$奖励2$奖励3$奖励4$奖励5$考核金额$考核单价$计件金额$总金额".Split('$');

        public Frm3PY_PYGZHS()
        {
            InitializeComponent();
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

        private void Frm3PY_PYGZHS_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            var datas = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).DistinctBy(t => t.LBMC).Select(t => t.LBMC).ToList();
            datas.Insert(0, "全部");
            cmbLBMC.DataSource = datas;
            cmbLBMC.DisplayMember = "LBMC";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string lbmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && lbmc == "全部" ? true : t.LBMC.Equals(lbmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_PYGZHS(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 8; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[13].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgvSum(DateTime selectTime, string userCode, string userName, string lbmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && lbmc == "全部" ? true : t.LBMC.Equals(lbmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList().GroupBy(t => new { t.GH, t.UserCode, t.UserName, t.LX }).Select(t => new DataBase3PY_PYGZHS { GH = t.Key.GH, UserCode = t.Key.UserCode, UserName = t.Key.UserName, LX = t.Key.LX, E_KHJE = t.Sum(x => decimal.TryParse(x.E_KHJE, out decimal khje) ? khje : 0M).ToString(), E_JJJE = t.Sum(x => decimal.TryParse(x.E_JJJE, out decimal jjje) ? jjje : 0M).ToString(), E_Money = t.Sum(x => decimal.TryParse(x.E_Money, out decimal money) ? money : 0M).ToString() }).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_PYGZHS_Sum(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i == 8 || i == 9 || i == 10 || i == 25 || i == 38 || i == 40 || i == 41)
                {
                    dgv.Columns[i].Visible = true;
                }
                else
                {
                    dgv.Columns[i].Visible = false;
                }
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text, cmbLBMC.Text);
        }

        private void btnSearchSum_Click(object sender, EventArgs e)
        {
            RefDgvSum(dtp.Value, txtUserCode.Text, txtUserName.Text, cmbLBMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——喷釉——喷釉工资核算.xlsx");
        }

        private bool Recount(List<DataBase3PY_PYGZHS> list)
        {
            try
            {
                var listMonth = new BaseDal<DataBaseDay>().GetList(h => h.FactoryNo == "G003" && h.WorkshopName == "喷釉车间").ToList();
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.LX))
                    {
                        item.LX = "手工橱喷釉工";
                    }
                    if (item.LX == "机械手")
                    {
                        item.LX = "机器人喷釉工";
                    }
                    var dataDay = listMonth.Where(t => t.TypesType == item.LBMC && t.PostName == item.LX).FirstOrDefault();
                    if (dataDay == null)
                    {
                        MessageBox.Show($"基础数据没有：{item.LBMC},{item.LX},无法导入！请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        item.B_DJ = dataDay.UnitPrice;
                        item.B_ZB0 = 0.ToString();
                        item.B_ZB1 = dataDay.GRKHZB1;
                        item.B_ZB2 = dataDay.GRJSB1;
                        item.B_ZB3 = dataDay.GRJSB2;
                        item.B_ZB4 = dataDay.GRJSB3;
                        item.B_ZB5 = dataDay.GRJSB4;
                        item.B_JL1 = 0.ToString();
                        item.B_JL2 = dataDay.GRJLDJ1;
                        item.B_JL3 = dataDay.GRJLDJ2;
                        item.B_JL4 = dataDay.GRJLDJ3;
                        item.B_JL5 = dataDay.GRJLDJ4;
                        string[] tmp1 = { item.B_ZB0, item.B_ZB1, item.B_ZB2, item.B_ZB3, item.B_ZB4, item.B_ZB5 };
                        string[] tmp2 = { item.B_JL1, item.B_JL2, item.B_JL3, item.B_JL4, item.B_JL5 };
                        decimal jl = 0M;
                        decimal.TryParse(item.YJP, out decimal yjp);
                        decimal.TryParse(item.YJL, out decimal yjl);
                        for (int i = 0; i < tmp1.Length; i++)
                        {
                            if (i + 1 >= tmp1.Length)
                            {
                                MessageBox.Show($"范围不正确，一级率:{yjl}，请检查Excle是否正确或联系管理员！！");
                                return false;
                            }
                            if (yjl == 1M)
                            {
                                if (yjl >= (decimal.TryParse(tmp1[i], out decimal xx1) ? xx1 : 0M) && yjl <= (decimal.TryParse(tmp1[i + 1], out decimal xx2) ? xx2 : 0M))
                                {
                                    jl = (decimal.TryParse(tmp2[i], out decimal xx3) ? xx3 : 0M);
                                    break;
                                }
                            }
                            else
                            {
                                if (yjl >= (decimal.TryParse(tmp1[i], out decimal x1) ? x1 : 0M) && yjl < (decimal.TryParse(tmp1[i + 1], out decimal x2) ? x2 : 0M))
                                {
                                    jl = (decimal.TryParse(tmp2[i], out decimal x3) ? x3 : 0M);
                                    break;
                                }
                            }
                        }
                        item.E_KHDJ = jl.ToString();
                        item.E_KHJE = (yjp * jl).ToString();
                        item.E_JJJE = (yjp * (decimal.TryParse(item.B_DJ, out decimal x4) ? x4 : 0M)).ToString();
                        item.E_Money = ((decimal.TryParse(item.E_KHJE, out decimal x5) ? x5 : 0M) + (decimal.TryParse(item.E_JJJE, out decimal x6) ? x6 : 0M)).ToString();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

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
                    List<DataBase3PY_PYGZHS> list = new ExcelHelper<DataBase3PY_PYGZHS>().ReadExcel(openFileDlg.FileName, 1, 6, 0, 0, 0, true);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (string.IsNullOrEmpty(list[i].YF) || string.IsNullOrEmpty(list[i].GCBM))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                    foreach (var item in list)
                    {
                        if (string.IsNullOrEmpty(item.LX))
                        {
                            item.LX = "手工橱喷釉工";
                        }
                        if (item.LX == "机械手")
                        {
                            item.LX = "机器人喷釉工";
                        }
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        item.GH = string.IsNullOrEmpty(item.GH) ? string.Empty : item.GH;
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;

                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (Recount(list) && new BaseDal<DataBase3PY_PYGZHS>().Add(list) > 0)
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
                List<DataBase3PY_PYGZHS> list = dgv.DataSource as List<DataBase3PY_PYGZHS>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_PYGZHS>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——喷釉工资核算.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3PY_PYGZHS DataBase3PY_PYGZHS = new DataBase3PY_PYGZHS() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3PY_PYGZHS> frm = new FrmModify<DataBase3PY_PYGZHS>(DataBase3PY_PYGZHS, header, OptionType.Add, Text, 5, 16);
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
            List<DataBase3PY_PYGZHS> list = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3PY_PYGZHS>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportExcelSum_Click(object sender, EventArgs e)
        {
            btnSearchSum.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3PY_PYGZHS> list = dgv.DataSource as List<DataBase3PY_PYGZHS>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_PYGZHS>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——喷釉工个人工资.xlsx", saveFileDlg.FileName, list, 2, new List<int> { 8, 9, 10, 25, 38, 40, 41 }, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3PY_PYGZHS DataBase3PY_PYGZHS)
                {
                    if (DataBase3PY_PYGZHS.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3PY_PYGZHS> frm = new FrmModify<DataBase3PY_PYGZHS>(DataBase3PY_PYGZHS, header, OptionType.Modify, Text, 5, 16);
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
                var DataBase3PY_PYGZHS = dgv.SelectedRows[0].DataBoundItem as DataBase3PY_PYGZHS;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3PY_PYGZHS != null)
                    {
                        if (DataBase3PY_PYGZHS.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3PY_PYGZHS>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3PY_PYGZHS>().Delete(item);
                                }
                                btnSearch.PerformClick();

                                //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                //{
                                //    if (conn.Execute("delete from DataBase3PY_PYGZHS where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
                                //    {
                                //        btnSearch.PerformClick();
                                //        MessageBox.Show("全部删除完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    }
                                //    else
                                //    {
                                //        MessageBox.Show("删除失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    }
                                //}
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3PY_PYGZHS> frm = new FrmModify<DataBase3PY_PYGZHS>(DataBase3PY_PYGZHS, header, OptionType.Delete, Text, 5, 0);
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