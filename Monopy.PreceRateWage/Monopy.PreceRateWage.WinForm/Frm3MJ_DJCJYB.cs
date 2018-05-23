using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3MJ_DJCJYB : Office2007Form
    {
        private string[] header;

        public Frm3MJ_DJCJYB()
        {
            InitializeComponent();
        }

        private void RefDgv(DateTime selectTime, string cpmc)
        {
            var datas = new BaseDal<DataBase3MJ_DJCJYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3MJ_DJCJYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$产品名称$单位$上期结存$生产累计$播出累计$破损累计$期末结存$备注".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——模具——大件车间月报.xlsx");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase3MJ_DJCJYB> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase3MJ_DJCJYB>().ReadExcel(openFileDlg.FileName, 5, 6, 1, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                var listBase = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间").ToList();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    //if (new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间" && t.TypesName == item.CPMC) == null)
                    if (listBase.Where(t => t.TypesName == item.CPMC).Count() == 0)
                    {
                        sb.Append("【" + item.CPMC + "】,");
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                    item.No = (i + 1).ToString();
                }
                if (sb.Length > 0)
                {
                    MessageBox.Show("基础数据库中没有信息，导入时自动过滤的有：" + Environment.NewLine + sb.ToString().TrimEnd(','), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (new BaseDal<DataBase3MJ_DJCJYB>().Add(list) > 0)
                {
                    Enabled = true;
                    txtCPMC.Text = "";
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！" + Environment.NewLine + sb.ToString().TrimEnd(','), "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<DataBase3MJ_DJCJYB> list = dgv.DataSource as List<DataBase3MJ_DJCJYB>;
                if (new ExcelHelper<DataBase3MJ_DJCJYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——模具——大件车间月报.xlsx", saveFileDlg.FileName, list, 5, 6, 0, 0, 2, 6, dtp.Value.ToString("yyyy-MM"), 1))
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
            DataBase3MJ_DJCJYB DataBase3MJ_DJCJYB = new DataBase3MJ_DJCJYB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3MJ_DJCJYB> frm = new FrmModify<DataBase3MJ_DJCJYB>(DataBase3MJ_DJCJYB, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtCPMC.Text = string.Empty;
                btnSearch.PerformClick();
                btnRecount.PerformClick();
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
                var DataBase3MJ_DJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase3MJ_DJCJYB;
                if (DataBase3MJ_DJCJYB != null)
                {
                    if (DataBase3MJ_DJCJYB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3MJ_DJCJYB> frm = new FrmModify<DataBase3MJ_DJCJYB>(DataBase3MJ_DJCJYB, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                        btnRecount.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3MJ_DJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase3MJ_DJCJYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3MJ_DJCJYB != null)
                    {
                        if (DataBase3MJ_DJCJYB.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3MJ_DJCJYB where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
                                    {
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
                        FrmModify<DataBase3MJ_DJCJYB> frm = new FrmModify<DataBase3MJ_DJCJYB>(DataBase3MJ_DJCJYB, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                        }
                    }
                }
            }
        }

        private void Frm3MJ_DJCJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtCPMC.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            string sql = "SELECT b.CPMC,sum(CAST( b.[Count] as NUMERIC(18,3))) Count,MAX( a.CreateUser) from DataBase3MJ_PMCDJ a left JOIN DataBase3MJ_PMCDJ_Child b on a.Id=b.DataBase3MJ_PMCDJ_Id where a.TheYear=" + dtp.Value.Year + " and a.TheMonth=" + dtp.Value.Month + " and b.CPMC is not null group by b.CPMC";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                var datas = conn.ExecuteReader(sql);
                dt.Load(datas);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var txt = dt.Rows[i][0].ToString();
                var t1 = new BaseDal<DataBase3MJ_DJCJYB>().GetList(t => t.CPMC == txt && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                var t2 = t1.GroupBy(t => t.CPMC).ToList();
                var yb = t2.Select(t => new { CPMC = t.Key, Count = t.Sum(x => decimal.TryParse(x.SCLJ, out decimal d) ? d : 0M) }).ToList();

                if (yb == null || yb.Count == 0)
                {
                    var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = dt.Rows[i][2].ToString(), MsgTitle = "模具大件月报不存在PMC大件中的：【" + dt.Rows[i][0].ToString() + "】", MsgClass = "模具大件月报不存在PMC大件中的：【" + dt.Rows[i][0].ToString() + "】", Msg = dtp.Value.ToString("yyyy年MM月") + "，模具大件月报不存在PMC大件中的：【" + dt.Rows[i][0].ToString() + "】", IsDone = false, IsRead = false, CreateTime = Program.NowTime, CreateUser = "系统报警" };
                    new BaseDal<DataBaseMsg>().Add(msg);
                    msg.ID = Guid.NewGuid();
                    msg.UserCode = Program.HrCode;
                    new BaseDal<DataBaseMsg>().Add(msg);
                }
                else
                {
                    decimal.TryParse(dt.Rows[i][1].ToString(), out decimal yb_Count);
                    if (yb_Count > yb[0].Count)
                    {
                        bj(dt.Rows[i][2].ToString().Split('_')[0], yb[0].CPMC, yb_Count, yb[0].Count);
                    }
                }
            }
            NewMethod();
        }

        private void NewMethod()
        {
            var datas = new BaseDal<DataBase3MJ_DJCJYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().GroupBy(t => t.CPMC).Select(t => new { User = t.Max(x => x.CreateUser), CPMC = t.Key, Count = t.Sum(x => decimal.TryParse(x.SCLJ, out decimal sclj) ? sclj : 0M) }).Where(t => t.Count > 0M).ToList();
            if (datas.Count == 0)
            {
                MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                foreach (var item in datas)
                {
                    string sql = "SELECT sum(CAST( b.[Count] as NUMERIC(18,3))) from DataBase3MJ_PMCDJ a left JOIN DataBase3MJ_PMCDJ_Child b on a.Id=b.DataBase3MJ_PMCDJ_Id where a.TheYear=@TheYear and a.TheMonth=@TheMonth and b.CPMC=@cpmc";
                    var obj = conn.ExecuteScalar(sql, new { TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, cpmc = item.CPMC });
                    if (obj != null)
                    {
                        decimal.TryParse(obj.ToString(), out decimal mx);
                        if (mx > item.Count)
                        {
                            //报警
                            string userCode = conn.ExecuteScalar("SELECT a.CreateUser from DataBase3MJ_PMCDJ a left JOIN DataBase3MJ_PMCDJ_Child b on a.Id=b.DataBase3MJ_PMCDJ_Id where a.TheYear=@TheYear and a.TheMonth=@TheMonth and b.CPMC=@cpmc", new { TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, cpmc = item.CPMC }).ToString().Split('_')[0];
                            bj(userCode, item.CPMC, mx, item.Count);
                        }
                    }
                }
            }
            MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "验证完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 验证报警
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="cpmb"></param>
        /// <param name="mx"></param>
        /// <param name="yb"></param>
        private void bj(string userCode, string cpmb, decimal mx, decimal yb)
        {
            var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = userCode, MsgTitle = "模具大件月报数量小于明细数量", MsgClass = "模具大件月报数量小于细数量", Msg = dtp.Value.ToString("yyyy年MM月") + "产品名称:【" + cpmb + "】，月报数量：【" + yb.ToString() + "】，日PMC大件明细数量：【" + mx.ToString() + "】。模具大件月报数量小于明细数量，验证未通过！", IsDone = false, IsRead = false, CreateTime = Program.NowTime, CreateUser = "系统报警" };
            new BaseDal<DataBaseMsg>().Add(msg);
            msg.ID = Guid.NewGuid();
            msg.UserCode = Program.HrCode;
            new BaseDal<DataBaseMsg>().Add(msg);
        }
    }
}