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
    public partial class FrmDataBaseDay : Office2007Form
    {
        private ExcelHelper<DataBaseDay> excelHelper = new ExcelHelper<DataBaseDay>();
        private string[] header;

        public FrmDataBaseDay()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void FrmDataBaseDay_Load(object sender, EventArgs e)
        {
            RefFactory();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefFactory()
        {
            var list = new BaseDal<DataBaseDay>().GetList().DistinctBy(t => t.FactoryNo).OrderBy(t => t.FactoryNo).ToList();
            list.Insert(0, new DataBaseDay() { FactoryNo = "全部" });
            cmbFactoryNo.DataSource = list;
            cmbFactoryNo.DisplayMember = "FactoryNo";
            cmbFactoryNo.ValueMember = "FactoryNo";
        }

        private void cmbFactoryNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBaseDay>().GetList(t => cmbFactoryNo.Text == "全部" ? true : t.FactoryNo == cmbFactoryNo.Text).DistinctBy(t => t.WorkshopName).OrderBy(t => t.WorkshopName).ToList();
            list.Insert(0, new DataBaseDay() { WorkshopName = "全部" });
            cmbWorkshopName.DataSource = list;
            cmbWorkshopName.DisplayMember = "WorkshopName";
            cmbWorkshopName.ValueMember = "WorkshopName";
        }

        private void cmbWorkshopName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list1 = new BaseDal<DataBaseDay>().GetList(t => (cmbFactoryNo.Text == "全部" ? true : t.FactoryNo == cmbFactoryNo.Text) && (cmbWorkshopName.Text == "全部" ? true : t.WorkshopName == cmbWorkshopName.Text)).DistinctBy(t => t.PostName).OrderBy(t => t.PostName).ToList();
            list1.Insert(0, new DataBaseDay() { PostName = "全部" });
            cmbPostName.DataSource = list1;
            cmbPostName.DisplayMember = "PostName";
            cmbPostName.ValueMember = "PostName";

            var list2 = new BaseDal<DataBaseDay>().GetList(t => (cmbFactoryNo.Text == "全部" ? true : t.FactoryNo == cmbFactoryNo.Text) && (cmbWorkshopName.Text == "全部" ? true : t.WorkshopName == cmbWorkshopName.Text)).DistinctBy(t => t.Classification).OrderBy(t => t.Classification).ToList();
            list2.Insert(0, new DataBaseDay() { Classification = "全部" });
            cmbClassification.DataSource = list2;
            cmbClassification.DisplayMember = "Classification";
            cmbClassification.ValueMember = "Classification";

            var list3 = new BaseDal<DataBaseDay>().GetList(t => (cmbFactoryNo.Text == "全部" ? true : t.FactoryNo == cmbFactoryNo.Text) && (cmbWorkshopName.Text == "全部" ? true : t.WorkshopName == cmbWorkshopName.Text)).DistinctBy(t => t.TypesType).OrderBy(t => t.TypesType).ToList();
            list3.Insert(0, new DataBaseDay() { TypesType = "全部" });
            cmbTypesType.DataSource = list3;
            cmbTypesType.DisplayMember = "TypesType";
            cmbTypesType.ValueMember = "TypesType";
        }

        private void RefDgv(DateTime dateTime, string factoryNo = "全部", string workshopName = "全部", string typesName = "", string postName = "全部", string classification = "全部", string typesType = "全部")
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            dgv.DataSource = new BaseDal<DataBaseDay>().GetList(t => (factoryNo != "全部" ? t.FactoryNo == factoryNo : true) && (workshopName != "全部" ? t.WorkshopName == workshopName : true) && (typesName != string.Empty ? t.TypesName.Contains(typesName) : true) && (ckbDJ.Checked ? string.IsNullOrEmpty(t.UnitPrice) : true) && (postName == "全部" ? true : t.PostName.Equals(postName)) && (classification == "全部" ? true : t.Classification.Equals(classification)) && (typesType == "全部" ? true : t.TypesType.Equals(typesType)) && t.CreateYear == dateTime.Year && t.CreateMonth == dateTime.Month, new OrderByExpression<DataBaseDay, string>(t => t.FactoryNo), new OrderByExpression<DataBaseDay, string>(t => t.WorkshopName), new OrderByExpression<DataBaseDay, string>(t => t.JBXW), new OrderByExpression<DataBaseDay, string>(t => t.PostName), new OrderByExpression<DataBaseDay, string>(t => t.Classification), new OrderByExpression<DataBaseDay, string>(t => t.TypesName)).ToList();
            dgv.Columns[0].Visible = false;
            dgv.Columns[6].Frozen = true;
            header = "年	月	工厂编码	车间名称	岗位名称	类别	检包线位	品种名称	品种对应类别	品种单位	单价	检包计件工资占比	喷釉班组考核占比	班组考核指标	班组基数比	班组奖励单价	个人考核指标1(应交坯率)	个人基数比1(目标一级率)	个人奖励单价1(目标一级率2)	个人罚款单价1(应交坯率（产量）)	个人考核指标2(产量上浮基数比)	个人基数比2(产量上浮单价)	个人奖励单价2(质量上浮基数比)	个人罚款单价2(质量上浮百分点)	个人考核指标3(质量下浮基数比)	个人基数比3(质量下浮百分点)	个人奖励单价3(上封顶单价（%）)	个人罚款单价3(下封顶单价（%）)	个人考核指标4	个人基数比4	个人奖励单价4	个人罚款单价4	考核金额上封顶	考核金额下封顶	底线开窑量".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
                if (dgv.Columns[i + 1].HeaderText.Contains("("))
                {
                    dgv.Columns[i + 1].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red, Font = new Font(dgv.Font, FontStyle.Bold) };
                }
            }
            dgv.ClearSelection();
        }

        private void UpdateJBXW()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                string sql = "UPDATE DataBaseDays SET JBXW=''  where JBXW is NULL";
                conn.Execute(sql);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbFactoryNo.Text, cmbWorkshopName.Text, txtTypesName.Text, cmbPostName.Text, cmbClassification.Text, cmbTypesType.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板基础数据——指标.xlsx");
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
                    var tmp = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == item.CreateYear && t.CreateMonth == item.CreateMonth && t.FactoryNo == item.FactoryNo && t.WorkshopName == item.WorkshopName && t.PostName == item.PostName && t.Classification == item.Classification && t.JBXW == item.JBXW && t.TypesName == item.TypesName && t.TypesType == item.TypesType);
                    if (tmp != null)
                    {
                        MessageBox.Show("已经存在：" + item.ToString(), "数据错误，导入失败！", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Enabled = true;
                        return;
                    }
                }
                new BaseDal<DataBaseDay>().Add(list);
                Enabled = true;
                FrmDataBaseDay_Load(null, null);
                UpdateJBXW();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (excelHelper.WriteExcle(Application.StartupPath + "\\Excel\\模板基础数据——指标.xlsx", saveFileDlg.FileName, dgv.DataSource as List<DataBaseDay>, 2, 3, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            DataBaseDay dataBaseDay = new DataBaseDay() { ID = Guid.NewGuid() };
            FrmModify<DataBaseDay> frm = new FrmModify<DataBaseDay>(dataBaseDay, header, OptionType.Add, Text);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnSearch.PerformClick();
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var dataBaseDay = dgv.SelectedRows[0].DataBoundItem as DataBaseDay;
                if (dataBaseDay != null)
                {
                    FrmModify<DataBaseDay> frm = new FrmModify<DataBaseDay>(dataBaseDay, header, OptionType.Modify, Text);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        UpdateJBXW();
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
                    var dataBaseDay = dgv.SelectedRows[0].DataBoundItem as DataBaseDay;
                    if (dataBaseDay != null)
                    {
                        FrmModify<DataBaseDay> frm = new FrmModify<DataBaseDay>(dataBaseDay, header, OptionType.Delete, Text);
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
                var list = dgv.DataSource as List<DataBaseDay>;
                if (list.Count == 0)
                {
                    MessageBox.Show("没有数据，请查询要删除的数据后，再点全部删除！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (var item in list)
                {
                    new BaseDal<DataBaseDay>().Delete(item);
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
            var data = dgv.DataSource as List<DataBaseDay>;
            if (data.Count == 1)
            {
                MessageBox.Show("没有数据，请先查询得到结果后再导入复制");
                return;
            }
            if (MessageBox.Show("导入时会删除导入日期的已有所有数据，确定要导入？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                new BaseDal<DataBaseDay>().ExecuteSqlCommand("delete from DataBaseDays where CreateYear=" + dtpTo.Value.Year + " and CreateMonth=" + dtpTo.Value.Month);
                var list = new List<DataBaseDay>();
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
                new BaseDal<DataBaseDay>().Add(list);
                btnSearch.PerformClick();
                MessageBox.Show("导入完成");
            }
        }
    }
}