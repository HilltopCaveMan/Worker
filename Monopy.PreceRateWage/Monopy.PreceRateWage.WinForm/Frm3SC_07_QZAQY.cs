using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_07_QZAQY : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$人员编码$姓名$考核项$分值比重$考核标准$考核得分".Split('$');

        public Frm3SC_07_QZAQY()
        {
            InitializeComponent();
        }

        private void Frm3SC_07_QZAQY_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
        {
            var datas = new BaseDal<DataBase3SC_07_QZAQY>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
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
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——气站安全员.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_07_QZAQY> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3SC_07_QZAQY>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (!decimal.TryParse(item.FZBZ, out decimal fzbz) || !decimal.TryParse(item.KHDF, out decimal khdf))
                    {
                        MessageBox.Show("【分值比重】或者【考核得分】不正确，应该填写数字！");
                        Enabled = true;
                        return;
                    }
                    if (khdf > fzbz)
                    {
                        MessageBox.Show("【考核得分】大于【分值比重】，请检查！");
                        Enabled = true;
                        return;
                    }
                    if (item.UserName.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
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
                if (new BaseDal<DataBase3SC_07_QZAQY>().Add(list) > 0)
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_07_QZAQY> list = dgv.DataSource as List<DataBase3SC_07_QZAQY>;
                if (new ExcelHelper<DataBase3SC_07_QZAQY>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——气站安全员.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var max = new BaseDal<DataBase3SC_07_QZAQY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderByDescending(t => t.No).FirstOrDefault();
            string maxId;
            if (max == null)
            {
                maxId = 1.ToString();
            }
            else
            {
                maxId = (Convert.ToInt32(max.No) + 1).ToString();
            }
            DataBase3SC_07_QZAQY DataBase3SC_07_QZAQY = new DataBase3SC_07_QZAQY() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxId };
            FrmModify<DataBase3SC_07_QZAQY> frm = new FrmModify<DataBase3SC_07_QZAQY>(DataBase3SC_07_QZAQY, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
                txtUserName.Text = string.Empty;
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
                var DataBase3SC_07_QZAQY = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_07_QZAQY;
                if (DataBase3SC_07_QZAQY != null)
                {
                    FrmModify<DataBase3SC_07_QZAQY> frm = new FrmModify<DataBase3SC_07_QZAQY>(DataBase3SC_07_QZAQY, header, OptionType.Modify, Text, 5, 0);
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
                var DataBase3SC_07_QZAQY = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_07_QZAQY;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_07_QZAQY != null)
                    {
                        FrmModify<DataBase3SC_07_QZAQY> frm = new FrmModify<DataBase3SC_07_QZAQY>(DataBase3SC_07_QZAQY, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void 删除全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除全部？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase3SC_07_QZAQY>;
                foreach (var item in list)
                {
                    new BaseDal<DataBase3SC_07_QZAQY>().Delete(item);
                }
                btnSearch.PerformClick();
            }
        }
    }
}