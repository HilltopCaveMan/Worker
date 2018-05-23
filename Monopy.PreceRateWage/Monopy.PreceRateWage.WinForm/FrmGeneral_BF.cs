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
    public partial class FrmGeneral_BF : Office2007Form
    {
        private string[] header;
        private string[] header_tmp;
        private DateTime selectTime;

        public FrmGeneral_BF()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void InitCmbFactory(DateTime dateTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct Factory from DataBaseGeneral_GL where theYear=" + dateTime.Year.ToString()).ToList();
                list.Insert(0, "全部");
                cmbFactory.DataSource = list;
            }
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct detp from DataBaseGeneral_GL where theYear=" + selectTime.Year.ToString() + " and factory like '%" + factory + "%'").ToList();
                list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private void FrmGeneral_BF_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            selectTime = dtp.Value;
            InitCmbFactory(selectTime);
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
            var datas = new BaseDal<DataBaseGeneral_GL>().GetList(t => t.TheYear == selectTime.Year && t.UserCode.Contains(userCode) && t.Factory.Contains(factory) && t.Detp.Contains(detp), new OrderByExpression<DataBaseGeneral_GL, string>(t => t.No)).ToList();
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_GL(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            string tmpYear1 = selectTime.AddYears(-1).Year.ToString().Substring(2);
            string tmpYear2 = selectTime.Year.ToString().Substring(2);
            string tmp = "ID$年$操作时间$操作人$序号$工号$姓名$性别$年龄$厂区$部门$职位$出生日期$到职日期$" + tmpYear1 + "年现有工龄$" + tmpYear2 + "年工龄工资$" + tmpYear2 + "年累计工龄$" + tmpYear1 + "年累计事假30天及以上";
            header = tmp.Split('$');
            header_tmp = new string[header.Length - 1];
            for (int i = 1; i < header.Length; i++)
            {
                header_tmp[i - 1] = header[i];
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text, cmbFactory.Text, cmbDept.Text);
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
                List<DataBaseGeneral_GL> list = new BaseDal<DataBaseGeneral_GL>().GetList(t => t.TheYear == selectTime.Year, new OrderByExpression<DataBaseGeneral_GL, string>(t => t.UserCode)).ToList();
                list.Add(MyDal.GetTotalDataBaseGeneral_GL(list, selectTime));
                if (new ExcelHelper<DataBaseGeneral_GL>().WriteExcle(Application.StartupPath + "\\OutExcel\\模板导出通用——工龄.xlsx", saveFileDlg.FileName, list, 1, 4))
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

        private void BtnNew_Click(object sender, EventArgs e)
        {
            DataBaseGeneral_GL DataBaseGeneral_GL = new DataBaseGeneral_GL() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year };
            FrmModify<DataBaseGeneral_GL> frm = new FrmModify<DataBaseGeneral_GL>(DataBaseGeneral_GL, header_tmp, OptionType.Add, Text, 3, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
                InitCmbFactory(selectTime);
                btnSearch.PerformClick();
            }
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
                var DataBaseGeneral_GL = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_GL;
                if (DataBaseGeneral_GL != null)
                {
                    if (DataBaseGeneral_GL.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_GL> frm = new FrmModify<DataBaseGeneral_GL>(DataBaseGeneral_GL, header_tmp, OptionType.Modify, Text, 3, 0);
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
                var DataBaseGeneral_GL = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_GL;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_GL != null)
                    {
                        if (DataBaseGeneral_GL.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBaseGeneral_GL where TheYear=" + selectTime.Year.ToString()) > 0)
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
                        FrmModify<DataBaseGeneral_GL> frm = new FrmModify<DataBaseGeneral_GL>(DataBaseGeneral_GL, header_tmp, OptionType.Delete, Text, 3, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitCmbFactory(selectTime);
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }
    }
}