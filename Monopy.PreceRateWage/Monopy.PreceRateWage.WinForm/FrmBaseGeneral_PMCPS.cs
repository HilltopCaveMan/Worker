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
    public partial class FrmBaseGeneral_PMCPS : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$工厂$车间$序号$工号$编码$姓名$连体数量$连体单价$大套数量$大套单价$小件数量$小件单价$盖数量$盖单价$合计金额".Split('$');
        private string _factoryNo;
        private string _dept;

        private FrmBaseGeneral_PMCPS()
        {
            InitializeComponent();
        }

        public FrmBaseGeneral_PMCPS(string args) : this()
        {
            try
            {
                _factoryNo = args.Split('-')[0];
                _dept = args.Split('-')[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
                return;
            }
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

        private void FrmBaseGeneral_PMCPS_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string code)
        {
            var datas = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.GH.Contains(code) && t.FactoryNo == _factoryNo && t.Dept == _dept).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_PMCPS(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 8; i++)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text, txtCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板通用——PMC破损.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBaseGeneral_PMCPS> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBaseGeneral_PMCPS>().ReadExcel(openFileDlg.FileName, 3, 8, 0, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    //if ((string.IsNullOrEmpty(item.GH) && /*item.GH.Contains("合计") ||*/ string.IsNullOrEmpty(item.UserCode) && string.IsNullOrEmpty(item.UserName)) || item.GH.Contains("合") || item.UserCode.Contains("合") || (item.UserName.Contains("合") && item.UserName.Contains("计")))

                    //{
                    //    list.RemoveAt(i);
                    //    if (i >= 0)
                    //    {
                    //        i--;
                    //    }
                    //    continue;
                    //}
                    if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName))
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
                    if (!string.IsNullOrEmpty(item.UserCode) && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (_factoryNo != "G003" && !string.IsNullOrEmpty(item.Money) && item.UserCode.Substring(0, 1).ToUpper() != "M")
                    {
                        MessageBox.Show("工号：【" + item.GH + "】,姓名：【" + item.UserName + "】,必须有对应的M编码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    item.Money = string.IsNullOrEmpty(item.Money) ? 0.ToString() : item.Money;
                    item.GH = string.IsNullOrEmpty(item.GH) ? string.Empty : item.GH;
                    item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                    item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;
                    item.FactoryNo = _factoryNo;
                    item.Dept = _dept;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                    item.CreateTime = Program.NowTime;
                    item.CreateUser = Program.User.ToString();
                }

                if (new BaseDal<DataBaseGeneral_PMCPS>().Add(list) > 0)
                {
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                List<DataBaseGeneral_PMCPS> list = dgv.DataSource as List<DataBaseGeneral_PMCPS>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_PMCPS>().WriteExcle(Application.StartupPath + "\\Excel\\模板通用——PMC破损.xlsx", saveFileDlg.FileName, list, 3, 8, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM-dd") + "-" + _dept))
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
            DataBaseGeneral_PMCPS DataBaseGeneral_JC = new DataBaseGeneral_PMCPS() { Id = Guid.NewGuid(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, FactoryNo = _factoryNo, Dept = _dept };
            FrmModify<DataBaseGeneral_PMCPS> frm = new FrmModify<DataBaseGeneral_PMCPS>(DataBaseGeneral_JC, header, OptionType.Add, Text, 7, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_PMCPS DataBaseGeneral_JC)
                {
                    if (DataBaseGeneral_JC.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_PMCPS> frm = new FrmModify<DataBaseGeneral_PMCPS>(DataBaseGeneral_JC, header, OptionType.Modify, Text, 7, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
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
                    if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_PMCPS DataBaseGeneral_JC)
                    {
                        if (DataBaseGeneral_JC.GH == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBaseGeneral_PMCPS>;
                                dgv.DataSource = null;
                                list.Remove(DataBaseGeneral_JC);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBaseGeneral_PMCPS>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBaseGeneral_PMCPS> frm = new FrmModify<DataBaseGeneral_PMCPS>(DataBaseGeneral_JC, header, OptionType.Delete, Text, 7, 0);
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