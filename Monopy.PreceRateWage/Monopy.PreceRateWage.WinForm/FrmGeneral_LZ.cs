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
    public partial class FrmGeneral_LZ : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public FrmGeneral_LZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void InitCmbFactory(DateTime dateTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct Factory from DataBaseGeneral_LZ where theYear=" + dateTime.Year.ToString() + " and theMonth=" + dateTime.Month.ToString()).ToList();
                list.Insert(0, "全部");
                cmbFactory.DataSource = list;
            }
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct detp from DataBaseGeneral_LZ where theYear=" + selectTime.Year.ToString() + " and theMonth=" + selectTime.Month.ToString() + " and factory like '%" + factory + "%'").ToList();
                list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private void FrmGeneral_LZ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            selectTime = dtp.Value;
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string factory, string detp)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            factory = factory == "全部" ? string.Empty : factory;
            detp = detp == "全部" ? string.Empty : detp;
            var datas = new BaseDal<DataBaseGeneral_LZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.Factory.Contains(factory) && t.Detp.Contains(detp)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_LZ(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间$操作人$年$月$厂区$序号$工号$姓名$部门$班组$岗位$离职类别$工服扣款金额$考勤卡扣款$工具扣款$培训费$合计$到职日期$离职日期$备注$确认人".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text, cmbFactory.Text, cmbDept.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板通用——离职扣款.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbF.Text))
            {
                MessageBox.Show("请选择/输入【厂区】！");
                cmbF.Focus();
                return;
            }

            List<DataBaseGeneral_LZ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBaseGeneral_LZ>().ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName) || list[i].UserCode == list[i].UserName)
                    {
                        list.RemoveAt(i);
                        i--;
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
                    list[i].TheYear = selectTime.Year;
                    list[i].TheMonth = selectTime.Month;
                    list[i].Factory = cmbF.Text;
                }
                if (new BaseDal<DataBaseGeneral_LZ>().Add(list) > 0)
                {
                    Enabled = true;
                    txtUserCode.Text = "";
                    btnSearch.PerformClick();
                    InitCmbFactory(selectTime);
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
                List<DataBaseGeneral_LZ> list = dgv.DataSource as List<DataBaseGeneral_LZ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                List<int> propertysNos = new List<int>
                {
                    6,
                    7,
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,14,15,16,17,18,19,20,21,
                    1,
                    2
                };

                if (new ExcelHelper<DataBaseGeneral_LZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出通用——离职扣款.xlsx", saveFileDlg.FileName, list, 2, propertysNos, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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
            DataBaseGeneral_LZ DataBaseGeneral_LZ = new DataBaseGeneral_LZ() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month };
            FrmModify<DataBaseGeneral_LZ> frm = new FrmModify<DataBaseGeneral_LZ>(DataBaseGeneral_LZ, header, OptionType.Add, Text, 4, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
                InitCmbFactory(selectTime);
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
                var DataBaseGeneral_LZ = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_LZ;
                if (DataBaseGeneral_LZ != null)
                {
                    if (DataBaseGeneral_LZ.UserCode == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_LZ> frm = new FrmModify<DataBaseGeneral_LZ>(DataBaseGeneral_LZ, header, OptionType.Modify, Text, 4, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitCmbFactory(selectTime);
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBaseGeneral_LZ = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_LZ;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_LZ != null)
                    {
                        if (DataBaseGeneral_LZ.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBaseGeneral_LZ where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString()) > 0)
                                    {
                                        InitCmbFactory(selectTime);
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
                        FrmModify<DataBaseGeneral_LZ> frm = new FrmModify<DataBaseGeneral_LZ>(DataBaseGeneral_LZ, header, OptionType.Delete, Text, 4, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitCmbFactory(selectTime);
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            InitCmbFactory(selectTime);
        }
    }
}