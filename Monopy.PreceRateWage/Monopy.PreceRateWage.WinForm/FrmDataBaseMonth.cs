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
    public partial class FrmDataBaseMonth : Office2007Form
    {
        private ExcelHelper<DataBaseMonth> excelHelper = new ExcelHelper<DataBaseMonth>();
        private string[] header;

        public FrmDataBaseMonth()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void FrmDataBaseMonth_Load(object sender, EventArgs e)
        {
            RefFactory();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            RefWorkshopName();
            btnSearch.PerformClick();
        }

        private void RefFactory()
        {
            var list = new BaseDal<DataBaseMonth>().GetList().DistinctBy(t => t.FactoryNo).OrderBy(t => t.FactoryNo).ToList();
            list.Insert(0, new DataBaseMonth() { FactoryNo = "全部" });
            cmbFactoryNo.DataSource = list;
            cmbFactoryNo.DisplayMember = "FactoryNo";
            cmbFactoryNo.ValueMember = "FactoryNo";
        }

        private void RefWorkshopName()
        {
            var list = new BaseDal<DataBaseMonth>().GetList().DistinctBy(t => t.WorkshopName).OrderBy(t => t.WorkshopName).ToList();
            list.Insert(0, new DataBaseMonth() { WorkshopName = "全部" });
            cmbWorkshopName.DataSource = list;
            cmbWorkshopName.DisplayMember = "WorkshopName";
            cmbWorkshopName.ValueMember = "WorkshopName";
        }

        private void RefDgv(DateTime dateTime, string factoryNo = "全部", string workshopName = "全部")
        {
            dgv.DataSource = new BaseDal<DataBaseMonth>().GetList(t => (factoryNo != "全部" ? t.FactoryNo == factoryNo : true) && (workshopName != "全部" ? t.WorkshopName == workshopName : true) && t.CreateYear == dateTime.Year && t.CreateMonth == dateTime.Month).ToList().OrderBy(t => t.FactoryNo).ThenBy(t => t.WorkshopName).ThenBy(t => t.PostName).ThenBy(t => t.Classification).ThenBy(t => t.ProductType).ThenBy(t => int.TryParse(t.MonthData, out int i) ? i : int.MaxValue).ToList(); ;
            dgv.Columns[0].Visible = false;
            header = "年	月	工厂编码	车间名称	岗位名称	类别	性别	月份	产品类别	品种	品种类别	师傅本月退还徒弟模型个数	辅助验货日工	临时工日工	学徒日工	弹性假日工	线长/班长费	学徒基本工资占比	学徒技能工资占比	基本工资	技能工资	加班工资	全勤奖	考核金额	入职补助	变产补助".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbFactoryNo.Text, cmbWorkshopName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板基础数据——月工资.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var list = excelHelper.ReadExcel(openFileDlg.FileName, 1, 3);
                foreach (var item in list)
                {
                    var mTp = item.GetType();
                    var propertyInfo = mTp.GetProperties();
                    for (int i = 0; i < propertyInfo.Length; i++)
                    {
                        var x = propertyInfo[i].GetValue(item, null);
                        if (x == null)
                        {
                            propertyInfo[i].SetValue(item, string.Empty, null);
                        }
                    }
                    item.CreateYear = dtp.Value.Year;
                    item.CreateMonth = dtp.Value.Month;
                    var tmp = new BaseDal<DataBaseMonth>().Get(t => t.CreateYear == item.CreateYear && t.CreateMonth == item.CreateMonth && t.FactoryNo == item.FactoryNo && t.WorkshopName == item.WorkshopName && t.PostName == item.PostName && t.Classification == item.Classification && t.Gender == item.Gender);
                    if (tmp != null)
                    {
                        MessageBox.Show("已经存在：" + item.ToString(), "数据错误，导入失败！", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Enabled = true;
                        return;
                    }
                }
                if (new BaseDal<DataBaseMonth>().Add(list) > 0)
                {
                    Enabled = true;
                    FrmDataBaseMonth_Load(null, null);
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var list = dgv.DataSource as List<DataBaseMonth>;

                if (excelHelper.WriteExcle(Application.StartupPath + "\\Excel\\模板基础数据——月工资.xlsx", saveFileDlg.FileName, list, 2, 3, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            DataBaseMonth dataBaseMonth = new DataBaseMonth() { ID = Guid.NewGuid() };
            FrmModify<DataBaseMonth> frm = new FrmModify<DataBaseMonth>(dataBaseMonth, header, OptionType.Add, Text);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnSearch.PerformClick();
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var dataBaseMonth = dgv.SelectedRows[0].DataBoundItem as DataBaseMonth;
                if (dataBaseMonth != null)
                {
                    FrmModify<DataBaseMonth> frm = new FrmModify<DataBaseMonth>(dataBaseMonth, header, OptionType.Modify, Text);
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
                    var dataBaseMonth = dgv.SelectedRows[0].DataBoundItem as DataBaseMonth;
                    if (dataBaseMonth != null)
                    {
                        FrmModify<DataBaseMonth> frm = new FrmModify<DataBaseMonth>(dataBaseMonth, header, OptionType.Delete, Text);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
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

        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("警告：数据删除后不能恢复，确定要【全部删除】？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBaseMonth>;
                if (list.Count == 0)
                {
                    MessageBox.Show("没有数据，请查询要删除的数据后，再点全部删除！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (var item in list)
                {
                    new BaseDal<DataBaseMonth>().Delete(item);
                }
                btnSearch.PerformClick();
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (cmbFactoryNo.Text == "全部")
            {
                MessageBox.Show("请选择/导入【厂区】！");
                cmbFactoryNo.Focus();
                return;
            }

            if (dtp.Value.Year == dtpTo.Value.Year && dtp.Value.Month == dtpTo.Value.Month)
            {
                MessageBox.Show("日期一致，无法导入复制！");
                return;
            }
            var data = dgv.DataSource as List<DataBaseMonth>;
            if (data.Count == 1)
            {
                MessageBox.Show("没有数据，请先查询得到结果后再导入复制");
                return;
            }
            if (MessageBox.Show("导入时会删除导入日期的已有所有数据，确定要导入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                new BaseDal<DataBaseMonth>().ExecuteSqlCommand("delete from DataBaseMonths where CreateYear=" + dtpTo.Value.Year + " and CreateMonth=" + dtpTo.Value.Month);
                var list = new List<DataBaseMonth>();
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].FactoryNo == cmbFactoryNo.Text)
                    {
                        var tmp = data[i];
                        tmp.ID = Guid.NewGuid();
                        tmp.CreateYear = dtpTo.Value.Year;
                        tmp.CreateMonth = dtpTo.Value.Month;
                        list.Add(tmp);
                    }

                }
                new BaseDal<DataBaseMonth>().Add(list);
                btnSearch.PerformClick();
                MessageBox.Show("导入完成");
            }
        }
    }
}