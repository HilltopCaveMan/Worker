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
    public partial class Frm3PY_JXG_KH : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工种$工号$类别名称$员工编码$员工姓名$开窑量$一级品$裂体$实际完成$个人指标$最低开窑量（件）$个人考核金额2$考核金额$计件金额".Split('$');

        public Frm3PY_JXG_KH()
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

        private void Frm3PY_JXG_KH_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
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
            var datas = new BaseDal<DataBase3PY_JXG_KH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_JXG_KH(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
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
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——喷釉——喷釉修检工个人考核.xlsx");
        }

        private bool Recount(List<DataBase3PY_JXG_KH> list)
        {
            try
            {
                var listday = new BaseDal<DataBaseDay>().GetList(h => h.FactoryNo == "G003" && h.WorkshopName == "喷釉车间").ToList();
                foreach (var item in list)
                {
                    decimal.TryParse(item.YJP, out decimal yjp);
                    var dayData = listday.Where(t => t.PostName.Equals(item.GZ)).FirstOrDefault();
                    if (dayData == null)
                    {
                        MessageBox.Show($"基础数据没有三厂喷釉车间【{item.GZ}】数据！无法导入！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (!decimal.TryParse(dayData.GRJSB1, out decimal x1) || !decimal.TryParse(dayData.GRJLDJ1, out decimal x2) || !decimal.TryParse(dayData.UnitPrice, out decimal x3))
                    {
                        MessageBox.Show($"基础数据没有三厂喷釉车间【{item.GZ}】数据错误，【个人基数比1】或【个人奖励单价1】或【单价】错误！无法导入！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    item.GRZB = dayData.GRKHZB1;
                    item.ZDKYL = dayData.DXKYL;
                    if (decimal.TryParse(item.KYL, out decimal kyl))
                    {
                        item.SJWC = (1 - yjp / kyl).ToString();

                        decimal.TryParse(item.ZDKYL, out decimal zdkyl);
                        decimal.TryParse(item.SJWC, out decimal sjwc);
                        decimal.TryParse(item.GRZB, out decimal grzb);
                        if (kyl <= zdkyl)
                        {
                            item.KHJE2 = 0.ToString();
                        }
                        else
                        {
                            decimal.TryParse(item.LT, out decimal lt);
                            decimal.TryParse(dayData.GRKHZB2, out decimal grkhzb2);//0.02
                            decimal.TryParse(dayData.GRJSB2, out decimal grjsb2);//0.01
                            decimal.TryParse(dayData.GRFKDJ2, out decimal grfkdj2);//3

                            if (lt / kyl <= grkhzb2)
                            {
                                item.KHJE2 = 0.ToString();
                            }
                            else
                            {
                                item.KHJE2 = ((grkhzb2 - lt / kyl) / grjsb2 * grfkdj2).ToString();
                            }
                        }

                        if (kyl >= zdkyl && sjwc >= grzb)
                        {
                            item.KHJE = 0.ToString();
                        }
                        else
                        {
                            if (kyl >= zdkyl && sjwc <= grzb)
                            {
                                item.KHJE = ((grzb - sjwc) / x1 * x2).ToString();
                            }
                            else
                            {
                                item.KHJE = 0.ToString();
                            }
                        }
                    }
                    else
                    {
                        item.KHJE = 0.ToString();
                    }

                    item.JJJE = (yjp * x3).ToString();
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
                    List<DataBase3PY_JXG_KH> list = new ExcelHelper<DataBase3PY_JXG_KH>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (string.IsNullOrEmpty(item.UserCode) && string.IsNullOrEmpty(item.UserName))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                        if (item.UserName == null)
                        {
                            item.UserName = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        item.GH = string.IsNullOrEmpty(item.GH) ? string.Empty : item.GH;
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;
                    }
                    if (Recount(list) && new BaseDal<DataBase3PY_JXG_KH>().Add(list) > 0)
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
                List<DataBase3PY_JXG_KH> list = dgv.DataSource as List<DataBase3PY_JXG_KH>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_JXG_KH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——喷釉修检工个人考核.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3PY_JXG_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3PY_JXG_KH DataBase3PY_JXG_KH = new DataBase3PY_JXG_KH() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3PY_JXG_KH> frm = new FrmModify<DataBase3PY_JXG_KH>(DataBase3PY_JXG_KH, header, OptionType.Add, Text, 5, 5);
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
            List<DataBase3PY_JXG_KH> list = new BaseDal<DataBase3PY_JXG_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3PY_JXG_KH>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3PY_JXG_KH DataBase3PY_JXG_KH)
                {
                    if (DataBase3PY_JXG_KH.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3PY_JXG_KH> frm = new FrmModify<DataBase3PY_JXG_KH>(DataBase3PY_JXG_KH, header, OptionType.Modify, Text, 5, 5);
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
                var DataBase3PY_JXG_KH = dgv.SelectedRows[0].DataBoundItem as DataBase3PY_JXG_KH;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3PY_JXG_KH != null)
                    {
                        if (DataBase3PY_JXG_KH.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3PY_JXG_KH>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3PY_JXG_KH>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3PY_JXG_KH> frm = new FrmModify<DataBase3PY_JXG_KH>(DataBase3PY_JXG_KH, header, OptionType.Delete, Text, 5, 0);
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