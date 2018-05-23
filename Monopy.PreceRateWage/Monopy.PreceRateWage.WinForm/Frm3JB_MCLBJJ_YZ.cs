using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3JB_MCLBJJ_YZ : Office2007Form
    {
        private string[] header;

        public Frm3JB_MCLBJJ_YZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3JB_MCLBJJ_YZ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            if (datas.Count > 0)
            {
                lblMc1.Text = datas[0].IsOkMc1 ? "成检记工验证成功" : "成检记工未验证或失败";
                lblMc1.BackColor = datas[0].IsOkMc1 ? Color.Green : Color.Red;
                lblMc2.Text = datas[0].IsOkMc2 ? "磨瓷验证成功" : "磨瓷未验证或失败";
                lblMc2.BackColor = datas[0].IsOkMc2 ? Color.Green : Color.Red;
                lblLb1.Text = datas[0].IsOkLb1 ? "冷补验证成功" : "冷补未验证或失败";
                lblLb1.BackColor = datas[0].IsOkLb1 ? Color.Green : Color.Red;
                lblLb2.Text = datas[0].IsOkLb2 ? "冷补计工验证成功" : "冷补计工未验证或失败";
                lblLb2.BackColor = datas[0].IsOkLb2 ? Color.Green : Color.Red;
                lblPg.Text = datas[0].IsOkYkcpg ? "配盖验证成功" : "配盖未验证或失败";
                lblPg.BackColor = datas[0].IsOkYkcpg ? Color.Green : Color.Red;
            }
            datas.Insert(0, MyDal.GetTotalDataBase3JB_MCLBJJ(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 10; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[10].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "ID$cTime$cUser$isOKmc1$isOKmc2$isOkgp$isOklb1$isOklb2$年$月$序号$人员编码$姓名$品种$磨瓷数量$原库存配盖数量$冷补数量$磨瓷单价$原库存配盖单价$冷补单价$磨瓷金额$原库存金额$冷补金额$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text);
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

        private string GetCellValue(ICell cell)
        {
            string result = string.Empty;
            if (cell == null)
            {
                return null;
            }
            switch (cell.CellType)
            {
                case CellType.Unknown:
                    break;

                case CellType.Numeric:
                    result = cell.NumericCellValue == 0 ? string.Empty : cell.NumericCellValue.ToString();
                    break;

                case CellType.String:
                    result = cell.StringCellValue;
                    break;

                case CellType.Formula:
                    result = cell.NumericCellValue == 0 ? string.Empty : cell.NumericCellValue.ToString();
                    break;

                case CellType.Blank:
                    break;

                case CellType.Boolean:
                    result = cell.BooleanCellValue.ToString();
                    break;

                case CellType.Error:
                    result = cell.ErrorCellValue.ToString();
                    break;
            }
            return result;
        }

        private void BtnMc1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;
                    try
                    {
                        workbook = WorkbookFactory.Create(fs);
                    }
                    catch
                    {
                        MessageBox.Show("Excle文件导入失败，请检查！导入时Office Excle文件不能被其它程序打开！！");
                    }
                    ISheet sheet = workbook.GetSheetAt(0);
                    IRow row = sheet.GetRow(sheet.LastRowNum);
                    ICell cell_Sx = row.GetCell(row.LastCellNum - 2);
                    ICell cell_Lt = row.GetCell(row.LastCellNum - 1);
                    DataBase3JB_MCLBJJ_YZ1 yz1 = new DataBase3JB_MCLBJJ_YZ1() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), DelFlag = false, TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, Sxl = GetCellValue(cell_Sx), Ltl = GetCellValue(cell_Lt) };
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        conn.Execute("delete from DataBase3JB_MCLBJJ_YZ1 where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
                    }
                    new BaseDal<DataBase3JB_MCLBJJ_YZ1>().Add(yz1);
                    if (YZMc1(yz1))
                    {
                        RefDgv(dtp.Value, txtUserCode.Text);
                    }
                }
            }
        }

        private void BtnMc2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var excelHelper = new ExcelHelper<DataBase3JB_MCLBJJ_YZ2>();
                var list = excelHelper.ReadExcel(openFileDlg.FileName, 3, 6, 1, 0);
                if (list == null)
                {
                    MessageBox.Show("导入Excle失败，请检查导入文件是否正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var list2 = new List<DataBase3JB_MCLBJJ_YZ2>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i].Chbm) && !string.IsNullOrEmpty(list[i].Chmc) && list[i].Chbm != list[i].Chmc)
                    {
                        list[i].CreateTime = Program.NowTime;
                        list[i].CreateUser = Program.User.ToString();
                        list[i].TheYear = dtp.Value.Year;
                        list[i].TheMonth = dtp.Value.Month;
                        list2.Add(list[i]);
                    }
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute("delete from DataBase3JB_MCLBJJ_YZ2 where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
                    conn.Execute("update DataBase3JB_MCLBJJ set McCount=null where LEN(McCount)=0");
                }
                new BaseDal<DataBase3JB_MCLBJJ_YZ2>().Add(list2);
                list = null;
                if (YZMc2(list2))
                {
                    txtUserCode.Text = string.Empty;
                    RefDgv(dtp.Value, txtUserCode.Text);
                }
            }
        }

        private void BtnLb1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var excelHelper = new ExcelHelper<DataBase3JB_MCLBJJ_YZ3>();
                var list = excelHelper.ReadExcel(openFileDlg.FileName, 3, 6, 1, 0);
                if (list == null)
                {
                    MessageBox.Show("导入Excle失败，请检查导入文件是否正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var list2 = new List<DataBase3JB_MCLBJJ_YZ3>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i].Chbm) && !string.IsNullOrEmpty(list[i].Chmc) && list[i].Chbm != list[i].Chmc)
                    {
                        list[i].CreateTime = Program.NowTime;
                        list[i].CreateUser = Program.User.ToString();
                        list[i].TheYear = dtp.Value.Year;
                        list[i].TheMonth = dtp.Value.Month;
                        list2.Add(list[i]);
                    }
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute("delete from DataBase3JB_MCLBJJ_YZ3 where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
                    conn.Execute("update DataBase3JB_MCLBJJ set lbcount=null where LEN(lbcount)=0");
                }
                new BaseDal<DataBase3JB_MCLBJJ_YZ3>().Add(list2);
                list = null;
                if (YZLb1(list2))
                {
                    txtUserCode.Text = string.Empty;
                    RefDgv(dtp.Value, txtUserCode.Text);
                }
            }
        }

        private void BtnLb2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var excelHelper = new ExcelHelper<DataBase3JB_MCLBJJ_YZ4>();
                var list = excelHelper.ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    MessageBox.Show("导入Excle失败，请检查导入文件是否正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute("delete from DataBase3JB_MCLBJJ_YZ4 where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
                }

                var yz = list.Where(t => t.Lx == "喷标扫釉").FirstOrDefault();
                if (yz == null)
                {
                    MessageBox.Show("不包括【喷标扫釉】，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                yz.CreateTime = Program.NowTime;
                yz.CreateUser = Program.User.ToString();
                yz.TheYear = dtp.Value.Year;
                yz.TheMonth = dtp.Value.Month;
                new BaseDal<DataBase3JB_MCLBJJ_YZ4>().Add(yz);
                if (YZLb2(yz))
                {
                    txtUserCode.Text = string.Empty;
                    RefDgv(dtp.Value, txtUserCode.Text);
                }
            }
        }

        private void BtnPg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var excelHelper = new ExcelHelper<DataBase3JB_MCLBJJ_YZ5>();
                var list = excelHelper.ReadExcel(openFileDlg.FileName, 3, 6, 1, 0);
                if (list == null)
                {
                    MessageBox.Show("导入Excle失败，请检查导入文件是否正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var list2 = new List<DataBase3JB_MCLBJJ_YZ5>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i].Chbm) && !string.IsNullOrEmpty(list[i].Chmc) && list[i].Chbm != list[i].Chmc)
                    {
                        list[i].CreateTime = Program.NowTime;
                        list[i].CreateUser = Program.User.ToString();
                        list[i].TheYear = dtp.Value.Year;
                        list[i].TheMonth = dtp.Value.Month;
                        list2.Add(list[i]);
                    }
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute($"delete from DataBase3JB_MCLBJJ_YZ5 where theYear={dtp.Value.Year.ToString()} and theMonth={dtp.Value.Month.ToString()}");
                    conn.Execute("update DataBase3JB_MCLBJJ set Ykcpgcount=null where LEN(Ykcpgcount)=0");
                }
                new BaseDal<DataBase3JB_MCLBJJ_YZ5>().Add(list2);
                list = null;
                if (YZPg(list2))
                {
                    txtUserCode.Text = string.Empty;
                    RefDgv(dtp.Value, txtUserCode.Text);
                }
            }
        }

        private void BtnViewMc1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            dgv.DataSource = new BaseDal<DataBase3JB_MCLBJJ_YZ1>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            header = "ID$创建时间$创建人$是否删除$年$月$水箱类$连体类".Split('$');
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].HeaderText = header[i];
                if (i <= 3)
                {
                    dgv.Columns[i].Visible = false;
                }
            }
        }

        private void BtnViewMc2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            dgv.DataSource = new BaseDal<DataBase3JB_MCLBJJ_YZ2>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = false;
            }
            dgv.Columns[1].Visible = true;
            dgv.Columns[1].HeaderText = "操作时间";
            dgv.Columns[2].Visible = true;
            dgv.Columns[2].HeaderText = "操作人";
            dgv.Columns[4].Visible = true;
            dgv.Columns[4].HeaderText = "年";
            dgv.Columns[5].Visible = true;
            dgv.Columns[5].HeaderText = "月";
            dgv.Columns[6].Visible = true;
            dgv.Columns[6].HeaderText = "存货名称";
            dgv.Columns[7].Visible = true;
            dgv.Columns[7].HeaderText = "存货编码";
            dgv.Columns[10].Visible = true;
            dgv.Columns[10].HeaderText = "合格";
        }

        private void BtnViewLb1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            dgv.DataSource = new BaseDal<DataBase3JB_MCLBJJ_YZ3>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = false;
            }
            dgv.Columns[1].Visible = true;
            dgv.Columns[1].HeaderText = "操作时间";
            dgv.Columns[2].Visible = true;
            dgv.Columns[2].HeaderText = "操作人";
            dgv.Columns[4].Visible = true;
            dgv.Columns[4].HeaderText = "年";
            dgv.Columns[5].Visible = true;
            dgv.Columns[5].HeaderText = "月";
            dgv.Columns[7].Visible = true;
            dgv.Columns[7].HeaderText = "存货编码";
            dgv.Columns[10].Visible = true;
            dgv.Columns[10].HeaderText = "合格";
        }

        private void BtnViewLb2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            dgv.DataSource = new BaseDal<DataBase3JB_MCLBJJ_YZ4>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = false;
            }
            dgv.Columns[1].Visible = true;
            dgv.Columns[1].HeaderText = "操作时间";
            dgv.Columns[2].Visible = true;
            dgv.Columns[2].HeaderText = "操作人";
            dgv.Columns[4].Visible = true;
            dgv.Columns[4].HeaderText = "年";
            dgv.Columns[5].Visible = true;
            dgv.Columns[5].HeaderText = "月";
            dgv.Columns[6].Visible = true;
            dgv.Columns[6].HeaderText = "类型";
            dgv.Columns[7].Visible = true;
            dgv.Columns[7].HeaderText = "开窑量";
            dgv.Columns[8].Visible = true;
            dgv.Columns[8].HeaderText = "合格数";
            dgv.Columns[9].Visible = true;
            dgv.Columns[9].HeaderText = "合格率";
        }

        private void BtnViewPg_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            dgv.DataSource = new BaseDal<DataBase3JB_MCLBJJ_YZ5>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = false;
            }
            dgv.Columns[1].Visible = true;
            dgv.Columns[1].HeaderText = "操作时间";
            dgv.Columns[2].Visible = true;
            dgv.Columns[2].HeaderText = "操作人";
            dgv.Columns[4].Visible = true;
            dgv.Columns[4].HeaderText = "年";
            dgv.Columns[5].Visible = true;
            dgv.Columns[5].HeaderText = "月";
            dgv.Columns[6].Visible = true;
            dgv.Columns[6].HeaderText = "存货编码";
            dgv.Columns[7].Visible = true;
            dgv.Columns[7].HeaderText = "存货名称";
            dgv.Columns[10].Visible = true;
            dgv.Columns[10].HeaderText = "合格";
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——成检计件.xlsx");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——PMC磨瓷月报.xlsx");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——PMC冷补月报.xlsx");
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——PMC冷补记功表.xlsx");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——PMC原库存磨瓷月报.xlsx");
        }

        private bool YZMc1(DataBase3JB_MCLBJJ_YZ1 yz1)
        {
            //正式开始验证
            //磨箱口配盖
            var list = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.PZ == "磨箱口配盖").ToList();
            decimal cj = list.Sum(t => string.IsNullOrEmpty(t.McCount) ? 0M : Convert.ToDecimal(t.McCount));
            decimal yz = string.IsNullOrEmpty(yz1.Sxl) ? 0M : Convert.ToDecimal(yz1.Sxl);
            if (cj > yz)
            {
                MessageBox.Show("磨箱口配盖的车间数量为：" + cj.ToString() + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证不通过！！");
                return false;
            }
            else
            {
                MessageBox.Show("磨箱口配盖的车间数量为：" + cj.ToString() + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证通过！！");
            }
            //连体配盖
            list = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.PZ == "连体配盖").ToList();
            cj = list.Sum(t => string.IsNullOrEmpty(t.McCount) ? 0M : Convert.ToDecimal(t.McCount));
            yz = string.IsNullOrEmpty(yz1.Ltl) ? 0M : Convert.ToDecimal(yz1.Ltl);
            if (cj > yz)
            {
                MessageBox.Show("连体配盖的车间数量为：" + cj.ToString() + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证不通过！！");
                return false;
            }
            else
            {
                MessageBox.Show("连体配盖的车间数量为：" + cj.ToString() + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证通过！！");
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                conn.Execute("update DataBase3JB_MCLBJJ set IsOkMc1='True' where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            }
            txtUserCode.Text = string.Empty;
            return true;
        }

        private void btnYZMc1_Click(object sender, EventArgs e)
        {
            var yz1 = new BaseDal<DataBase3JB_MCLBJJ_YZ1>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).FirstOrDefault();
            if (yz1 == null)
            {
                MessageBox.Show("没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (YZMc1(yz1))
            {
                RefDgv(dtp.Value, txtUserCode.Text);
            }
        }

        private bool YZMc2(List<DataBase3JB_MCLBJJ_YZ2> list2)
        {
            //var listGc = new List<DataBase3JB_MCLBJJ>();
            //string sql = "select pz,sum(cast(isnull(mccount,0) as numeric(18,3))) as McCount from database3jb_mclbjj where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + " group by pz having sum(cast(isnull(mccount,0) as numeric(18,3)))>0";
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //    listGc = conn.Query<DataBase3JB_MCLBJJ>(sql).ToList();
            //}
            var listGc = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().GroupBy(t => t.PZ).Select(t => new { PZ = t.Key, McCount = t.Sum(x => decimal.TryParse(x.McCount, out decimal d) ? d : 0M) }).ToList();

            //开始验证
            foreach (var item in listGc)
            {
                if (item.McCount == 0M)
                {
                    continue;
                }
                if (item.PZ == "磨箱口配盖" || item.PZ == "连体配盖")
                {
                    continue;
                }
                var yz = list2.Where(t => t.Chmc == item.PZ).Sum(t => string.IsNullOrEmpty(t.Hg) ? 0M : Convert.ToDecimal(t.Hg));
                if (yz > 0)
                {
                    if (Convert.ToDecimal(item.McCount) > yz)
                    {
                        MessageBox.Show("车间【" + item.PZ + "】数量为：" + item.McCount + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证不通过！！");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("车间【" + item.PZ + "】数量为：" + item.McCount + ",验证无数据！" + Environment.NewLine + "验证不通过！！");
                    return false;
                }
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                conn.Execute("update DataBase3JB_MCLBJJ set IsOkMc2='True' where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            }
            MessageBox.Show("磨瓷验证通过！！");
            return true;
        }

        private void btnYZMc2_Click(object sender, EventArgs e)
        {
            var list2 = new BaseDal<DataBase3JB_MCLBJJ_YZ2>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            if (list2 == null || list2.Count == 0)
            {
                MessageBox.Show("没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (YZMc2(list2))
            {
                txtUserCode.Text = string.Empty;
                RefDgv(dtp.Value, txtUserCode.Text);
            }
        }

        private bool YZLb1(List<DataBase3JB_MCLBJJ_YZ3> list2)
        {
            //var listGc = new List<DataBase3JB_MCLBJJ>();
            //string sql = "select pz,sum(cast(isnull(lbcount,0) as numeric(18,3))) as lbCount from database3jb_mclbjj where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + " group by pz having sum(cast(isnull(lbcount,0) as numeric(18,3)))>0";
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //    listGc = conn.Query<DataBase3JB_MCLBJJ>(sql).ToList();
            //}
            var listGc = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().GroupBy(t => t.PZ).Select(t => new { PZ = t.Key, LbCount = t.Sum(x => decimal.TryParse(x.LbCount, out decimal d) ? d : 0M) }).ToList();

            //开始验证
            foreach (var item in listGc)
            {
                if (item.LbCount == 0M)
                {
                    continue;
                }
                if (item.PZ == "喷标扫釉")
                {
                    continue;
                }
                var yz = list2.Where(t => t.Chmc == item.PZ).Sum(t => string.IsNullOrEmpty(t.Hg) ? 0M : Convert.ToDecimal(t.Hg));
                if (yz > 0)
                {
                    if (Convert.ToDecimal(item.LbCount) > Convert.ToDecimal(yz))
                    {
                        MessageBox.Show("车间【" + item.PZ + "】数量为：" + item.LbCount + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证不通过！！");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("车间【" + item.PZ + "】数量为：" + item.LbCount + ",验证无数据！" + Environment.NewLine + "验证不通过！！");
                    return false;
                }
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                conn.Execute("update DataBase3JB_MCLBJJ set IsOkLb1='True' where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            }
            MessageBox.Show("冷补验证通过！！");
            return true;
        }

        private void btnYZLb1_Click(object sender, EventArgs e)
        {
            var list2 = new BaseDal<DataBase3JB_MCLBJJ_YZ3>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            if (list2 == null || list2.Count == 0)
            {
                MessageBox.Show("没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (YZLb1(list2))
            {
                txtUserCode.Text = string.Empty;
                RefDgv(dtp.Value, txtUserCode.Text);
            }
        }

        private bool YZLb2(DataBase3JB_MCLBJJ_YZ4 yz)
        {
            //开始验证
            //var gc = new DataBase3JB_MCLBJJ();
            //string sql = "SELECT PZ,SUM(CAST(ISNULL(LbCount,0) AS NUMERIC(18,3))) AS LbCount FROM dbo.DataBase3JB_MCLBJJ WHERE PZ='喷标扫釉' and TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + " GROUP BY PZ";
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //    gc = conn.Query<DataBase3JB_MCLBJJ>(sql).FirstOrDefault();
            //}

            var gc = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.PZ == "喷标扫釉").ToList().GroupBy(t => t.PZ).Select(t => new { PZ = t.Key, LbCount = t.Sum(x => decimal.TryParse(x.LbCount, out decimal d) ? d : 0M) }).ToList().FirstOrDefault();

            if (gc != null)
            {
                var gcCount = gc.LbCount;
                if (gcCount == 0M)
                {
                    MessageBox.Show("工厂没有填写喷标扫釉" + Environment.NewLine + "验证通过！！");
                    return true;
                }

                var yzCount = string.IsNullOrEmpty(yz.Hgs) ? 0m : Convert.ToDecimal(yz.Hgs);
                if (gcCount > yzCount)
                {
                    MessageBox.Show("喷标扫釉,数量为：" + gcCount + ",验证数量为：" + yzCount + Environment.NewLine + "验证不通过！！");
                    return false;
                }
                else
                {
                    MessageBox.Show("喷标扫釉,数量为：" + gcCount + ",验证数量为：" + yzCount + Environment.NewLine + "验证通过！！");
                }
            }
            else
            {
                MessageBox.Show("工厂没有填写喷标扫釉" + Environment.NewLine + "验证通过！！");
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                conn.Execute("update DataBase3JB_MCLBJJ set IsOkLb2='True' where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            }
            MessageBox.Show("冷补记工验证通过！！");
            return true;
        }

        private void btnYZLb2_Click(object sender, EventArgs e)
        {
            var yz = new BaseDal<DataBase3JB_MCLBJJ_YZ4>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList().Where(t => t.Lx == "喷标扫釉").FirstOrDefault();
            if (yz == null)
            {
                MessageBox.Show("不包括【喷标扫釉】，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (YZLb2(yz))
            {
                txtUserCode.Text = string.Empty;
                RefDgv(dtp.Value, txtUserCode.Text);
            }
        }

        private bool YZPg(List<DataBase3JB_MCLBJJ_YZ5> list2)
        {
            //var listGc = new List<DataBase3JB_MCLBJJ>();

            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //    listGc = conn.Query<DataBase3JB_MCLBJJ>($"select pz,sum(cast(isnull(Ykcpgcount,0) as numeric(18,3))) as Ykcpgcount from database3jb_mclbjj where TheYear={dtp.Value.Year} and TheMonth={dtp.Value.Month} group by pz having sum(cast(isnull(Ykcpgcount,0) as numeric(18,3)))>0").ToList();
            //}
            var listGc = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().GroupBy(t => t.PZ).Select(t => new { PZ = t.Key, Ykcpgcount = t.Sum(x => decimal.TryParse(x.YkcpgCount, out decimal d) ? d : 0m) }).ToList();

            //开始验证

            foreach (var item in listGc)
            {
                if (item.Ykcpgcount == 0M)
                {
                    continue;
                }
                var yz = list2.Where(t => t.Chmc == item.PZ).Sum(t => string.IsNullOrEmpty(t.Hg) ? 0M : Convert.ToDecimal(t.Hg));
                if (yz > 0)
                {
                    if (Convert.ToDecimal(item.Ykcpgcount) > yz)
                    {
                        MessageBox.Show("车间" + item.PZ + "数量为：" + item.Ykcpgcount + ",验证数量为：" + yz.ToString() + Environment.NewLine + "验证不通过！！");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("车间" + item.PZ + "数量为：" + item.Ykcpgcount + ",验证无数据！" + Environment.NewLine + "验证不通过！！");
                    return false;
                }
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                conn.Execute("update DataBase3JB_MCLBJJ set IsOkYkcpg='True' where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            }
            MessageBox.Show("原库存配盖验证通过！！");
            return true;
        }

        private void btnYZPg_Click(object sender, EventArgs e)
        {
            var list2 = new BaseDal<DataBase3JB_MCLBJJ_YZ5>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.DelFlag == false).ToList();
            if (YZPg(list2))
            {
                txtUserCode.Text = string.Empty;
                RefDgv(dtp.Value, txtUserCode.Text);
            }
        }
    }
}