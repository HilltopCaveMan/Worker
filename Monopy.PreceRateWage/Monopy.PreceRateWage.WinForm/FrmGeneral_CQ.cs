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
    public partial class FrmGeneral_CQ : Office2007Form
    {
        private string[] header;

        public FrmGeneral_CQ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void InitCmbFactory(DateTime dateTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct Factory from DataBaseGeneral_CQ where theYear=" + dateTime.Year.ToString() + " and theMonth=" + dateTime.Month.ToString()).ToList();
                list.Insert(0, "全部");
                cmbFactory.DataSource = list;
                cmbFactory.DisplayMember = "Factory";
            }
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct dept from DataBaseGeneral_CQ where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString() + " and factory like '%" + factory + "%'").ToList();
                list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private void FrmGeneral_CQ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string factory, string detp, string zw, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            factory = factory == "全部" ? string.Empty : factory;
            detp = detp == "全部" ? string.Empty : detp;
            var datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.Factory.Contains(factory) && t.Dept.Contains(detp) && t.UserName.Contains(userName) && (zw == "全部" ? true : t.Position.Contains(zw))).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_CQ(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间$操作人$年$月$厂区$序号$人员编码$人员姓名$部门名称$职位$应出勤（天数）$实出勤（天数）$本月抵扣调休天数$带薪假天数$加班计薪天数$弹性假天数$迟到早退次数$旷工天数$事假（天数）$病假（天数）$考勤合计$备注".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, cmbFactory.Text, cmbDept.Text, cmbZW.Text, txtUserName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板通用——出勤.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbF.Text))
            {
                MessageBox.Show("请选择/输入【厂区】！");
                cmbF.Focus();
                return;
            }

            List<DataBaseGeneral_CQ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBaseGeneral_CQ>().ReadExcel(openFileDlg.FileName, 2, 6);
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
                    }
                    else
                    {
                        if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Enabled = true;
                            return;
                        }
                        list[i].CreateUser = Program.User.ToString();
                        list[i].CreateTime = Program.NowTime;
                        list[i].TheYear = dtp.Value.Year;
                        list[i].TheMonth = dtp.Value.Month;
                        list[i].Factory = cmbF.Text;
                    }
                }
                if (new BaseDal<DataBaseGeneral_CQ>().Add(list) > 0)
                {
                    Enabled = true;
                    txtUserCode.Text = "";
                    btnSearch.PerformClick();
                    InitCmbFactory(dtp.Value);
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
                List<DataBaseGeneral_CQ> list = dgv.DataSource as List<DataBaseGeneral_CQ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_CQ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出通用——出勤.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            DataBaseGeneral_CQ DataBaseGeneral_CQ = new DataBaseGeneral_CQ() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBaseGeneral_CQ> frm = new FrmModify<DataBaseGeneral_CQ>(DataBaseGeneral_CQ, header, OptionType.Add, Text, 4, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtUserCode.Text = string.Empty;
                InitCmbFactory(dtp.Value);
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
                var DataBaseGeneral_CQ = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_CQ;
                if (DataBaseGeneral_CQ != null)
                {
                    if (DataBaseGeneral_CQ.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_CQ> frm = new FrmModify<DataBaseGeneral_CQ>(DataBaseGeneral_CQ, header, OptionType.Modify, Text, 4, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitCmbFactory(dtp.Value);
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBaseGeneral_CQ = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_CQ;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_CQ != null)
                    {
                        if (DataBaseGeneral_CQ.UserCode == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                List<DataBaseGeneral_CQ> list = dgv.DataSource as List<DataBaseGeneral_CQ>;
                                dgv.DataSource = null;
                                Enabled = false;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBaseGeneral_CQ>().Delete(item);
                                }
                                Enabled = true;
                                btnSearch.PerformClick();
                                MessageBox.Show("全部删除完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                //{
                                //    if (conn.Execute("delete from DataBaseGeneral_CQ where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
                                //    {
                                //        InitCmbFactory(dtp.Value);
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
                        FrmModify<DataBaseGeneral_CQ> frm = new FrmModify<DataBaseGeneral_CQ>(DataBaseGeneral_CQ, header, OptionType.Delete, Text, 4, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitCmbFactory(dtp.Value);
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitCmbFactory(dtp.Value);
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbZW.DataSource = null;
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            string dept = cmbDept.Text == "全部" ? string.Empty : cmbDept.Text;
            List<string> list = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                list = conn.Query<string>("select distinct Position from DataBaseGeneral_CQ where Position is not null and theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString() + " and factory like '%" + factory + "%' and dept like '%" + dept + "%' order by Position").ToList();
            }
            list.Insert(0, "全部");
            cmbZW.DataSource = list;
            cmbZW.SelectedIndex = 0;
        }
    }
}