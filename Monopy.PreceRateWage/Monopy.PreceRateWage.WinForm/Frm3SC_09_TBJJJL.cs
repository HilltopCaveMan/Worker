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
    public partial class Frm3SC_09_TBJJJL : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$存货名称$标污$倒重漏错".Split('$');

        public Frm3SC_09_TBJJJL()
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

        private void Frm3SC_09_TBJJJL_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtCHMC.Text = string.Empty;
            UiControl();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string chmc)
        {
            var datas = new BaseDal<DataBase3SC_09_TBJJJL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CHMC.Contains(chmc)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_09_TBJJJL(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCHMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——贴标检验记录.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_09_TBJJJL> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3SC_09_TBJJJL>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item.CHMC.Contains("合计") || !item.CHMC.StartsWith("H", true, null))
                    {
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    if (string.IsNullOrEmpty(item.BW))
                    {
                        item.BW = 0.ToString();
                    }
                    if (string.IsNullOrEmpty(item.DZLC))
                    {
                        item.DZLC = 0.ToString();
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                }
                var hj = MyDal.GetTotalDataBase3SC_09_TBJJJL(list);
                decimal.TryParse(hj.BW, out decimal bw);
                decimal.TryParse(hj.DZLC, out decimal dzlc);
                var hj8 = new BaseDal<DataBase3SC_08_TBSJQXS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().Sum(t => decimal.TryParse(t.QX, out decimal d) ? d : 0M);
                if (hj8 != bw + dzlc)
                {
                    MessageBox.Show($"贴标检验记录的合计：【{(bw + dzlc).ToString()}】不等于贴标实际缺陷数：【{hj8.ToString()}】", "数据错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (new BaseDal<DataBase3SC_09_TBJJJL>().Add(list) > 0)
                {
                    Enabled = true;
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_09_TBJJJL> list = dgv.DataSource as List<DataBase3SC_09_TBJJJL>;
                if (new ExcelHelper<DataBase3SC_09_TBJJJL>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——贴标检验记录.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM-dd")))
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            var max = new BaseDal<DataBase3SC_09_TBJJJL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderByDescending(t => t.No).FirstOrDefault();
            string maxId;
            if (max == null)
            {
                maxId = 1.ToString();
            }
            else
            {
                maxId = (Convert.ToInt32(max.No) + 1).ToString();
            }
            DataBase3SC_09_TBJJJL DataBase3SC_09_TBJJJL = new DataBase3SC_09_TBJJJL() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxId };
            FrmModify<DataBase3SC_09_TBJJJL> frm = new FrmModify<DataBase3SC_09_TBJJJL>(DataBase3SC_09_TBJJJL, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_09_TBJJJL DataBase3SC_09_TBJJJL)
                {
                    if (DataBase3SC_09_TBJJJL.No.Equals("合计"))
                    {
                        MessageBox.Show("合计不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3SC_09_TBJJJL> frm = new FrmModify<DataBase3SC_09_TBJJJL>(DataBase3SC_09_TBJJJL, header, OptionType.Modify, Text, 5, 0);
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
                var DataBase3SC_09_TBJJJL = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_09_TBJJJL;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_09_TBJJJL.No.Equals("合计"))
                    {
                        if (MessageBox.Show("删除全部？不可恢复！", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            var list = dgv.DataSource as List<DataBase3SC_09_TBJJJL>;
                            list.RemoveAt(0);
                            foreach (var item in list)
                            {
                                new BaseDal<DataBase3SC_09_TBJJJL>().Delete(item);
                            }
                            btnSearch.PerformClick();
                        }
                        return;
                    }
                    if (DataBase3SC_09_TBJJJL != null)
                    {
                        FrmModify<DataBase3SC_09_TBJJJL> frm = new FrmModify<DataBase3SC_09_TBJJJL>(DataBase3SC_09_TBJJJL, header, OptionType.Delete, Text, 5, 0);
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