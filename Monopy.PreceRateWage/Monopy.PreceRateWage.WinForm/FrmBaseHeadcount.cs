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
    public partial class FrmBaseHeadcount : Office2007Form
    {
        private string[] header;

        public FrmBaseHeadcount()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void InitCmbFactory()
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //    List<string> list = conn.Query<string>("select distinct Factory from BaseHeadcounts").ToList();
            //    list.Insert(0, "全部");
            //    cmbFactory.DataSource = list;
            //}
            List<string> list = new List<string>();
            list.Insert(0, "全部");
            list.Insert(1, "一厂");
            list.Insert(2, "二厂");
            list.Insert(3, "三厂");
            cmbFactory.DataSource = list;
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct deptName from BaseHeadcounts where factory like '%" + factory + "%'").ToList();
                list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private void FrmBaseHeadcount_Load(object sender, EventArgs e)
        {
            InitCmbFactory();
            txtName.Text = string.Empty;
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(string name, string factory, string detp, DateTime dateTime)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            factory = factory == "全部" ? string.Empty : factory;
            detp = detp == "全部" ? string.Empty : detp;
            var datas = new BaseDal<BaseHeadcount>().GetList(t => t.Factory.Contains(factory) && t.DeptName.Contains(detp) && t.Name.Contains(name) && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalBaseHeadcount(datas, dtp.Value));
            dgv.DataSource = datas;
            for (int i = 0; i < 3; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间$操作人$年$月$厂区$序号$人员类别$单位名称$线位$新岗位名称$编制".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(txtName.Text, cmbFactory.Text, cmbDept.Text, dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\InExcel\\模板通用——编制.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbF.Text))
            {
                MessageBox.Show("请选择/输入【厂区】！");
                cmbF.Focus();
                return;
            }

            List<BaseHeadcount> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<BaseHeadcount>().ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].Name))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        list[i].CreateUser = Program.User.ToString();
                        list[i].CreateTime = Program.NowTime;
                        list[i].Factory = cmbF.Text;
                        list[i].TheYear = dtp.Value.Year;
                        list[i].TheMonth = dtp.Value.Month;
                    }
                }
                if (new BaseDal<BaseHeadcount>().Add(list) > 0)
                {
                    Enabled = true;
                    txtName.Text = "";
                    InitCmbFactory();
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
                List<BaseHeadcount> list = (List<BaseHeadcount>)dgv.DataSource;
                var tmp = list[0];
                list.RemoveAt(0);
                list.Add(tmp);
                if (new ExcelHelper<BaseHeadcount>().WriteExcle(Application.StartupPath + "\\\\OutExcel\\模板通用——编制.xlsx", saveFileDlg.FileName, list,2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                list.Insert(0, tmp);
                Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            BaseHeadcount BaseHeadcount = new BaseHeadcount() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<BaseHeadcount> frm = new FrmModify<BaseHeadcount>(BaseHeadcount, header, OptionType.Add, Text, 4, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtName.Text = string.Empty;
                InitCmbFactory();
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
                var BaseHeadcount = dgv.SelectedRows[0].DataBoundItem as BaseHeadcount;
                if (BaseHeadcount != null)
                {
                    if (BaseHeadcount.DeptName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<BaseHeadcount> frm = new FrmModify<BaseHeadcount>(BaseHeadcount, header, OptionType.Modify, Text, 4, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitCmbFactory();
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var BaseHeadcount = dgv.SelectedRows[0].DataBoundItem as BaseHeadcount;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (BaseHeadcount != null)
                    {
                        if (BaseHeadcount.DeptName == "合计")
                        {
                            if (MessageBox.Show("要删除，所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                List<BaseHeadcount> list = dgv.DataSource as List<BaseHeadcount>;
                                list.RemoveAt(0);
                                for (int i = 0; i < list.Count; i++)
                                {
                                    new BaseDal<BaseHeadcount>().Delete(list[i]);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<BaseHeadcount> frm = new FrmModify<BaseHeadcount>(BaseHeadcount, header, OptionType.Delete, Text, 4, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitCmbFactory();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (dtp.Value.Year == dtpTo.Value.Year && dtp.Value.Month == dtpTo.Value.Month)
            {
                MessageBox.Show("日期一致，无法导入复制！");
                return;
            }
            var data = dgv.DataSource as List<BaseHeadcount>;
            if (data.Count == 1)
            {
                MessageBox.Show("没有数据，请先查询得到结果后再导入复制");
                return;
            }
            if (MessageBox.Show("导入时会删除导入日期的已有所有数据，确定要导入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                new BaseDal<BaseHeadcount>().ExecuteSqlCommand("delete from BaseHeadcounts where theyear=" + dtpTo.Value.Year + " and theMonth=" + dtpTo.Value.Month);
                var list = new List<BaseHeadcount>();
                for (int i = 1; i < data.Count; i++)
                {
                    var tmp = data[i];
                    tmp.Id = Guid.NewGuid();
                    tmp.TheYear = dtpTo.Value.Year;
                    tmp.TheMonth = dtpTo.Value.Month;
                    tmp.CreateTime = Program.NowTime;
                    tmp.CreateUser = Program.User.ToString();
                    list.Add(tmp);
                }
                new BaseDal<BaseHeadcount>().Add(list);
                btnSearch.PerformClick();
                MessageBox.Show("导入完成");
            }
        }
    }
}