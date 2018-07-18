using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Frm1MJ_DJCJYB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$产品名称$单位$上期结存$当日生产$生产累计$调拨入库$当日拨出$拨出累计$调拨出库$当日破损$破损累计$期末结存$备注".Split('$');
        public Frm1MJ_DJCJYB()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1MJ_DJCJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtCPMC.Text = string.Empty;
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——模具——大件车间月报.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase1MJ_DJCJYB> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase1MJ_DJCJYB>().ReadExcel(openFileDlg.FileName, 3, 6, 0, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                var listBase = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G001" && t.WorkshopName == "模具车间").ToList();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (listBase.Where(t => t.TypesName == item.CPMC).Count() == 0)
                    {
                        sb.Append("【" + item.CPMC + "】,");
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    if (string.IsNullOrEmpty(item.CPMC))
                    {
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
                if (NewMethod(list) && new BaseDal<DataBase1MJ_DJCJYB>().Add(list) > 0)
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1MJ_DJCJYB> list = dgv.DataSource as List<DataBase1MJ_DJCJYB>;
                if (new ExcelHelper<DataBase1MJ_DJCJYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板一厂——模具——大件车间月报.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase1MJ_DJCJYB DataBase1MJ_DJCJYB = new DataBase1MJ_DJCJYB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase1MJ_DJCJYB> frm = new FrmModify<DataBase1MJ_DJCJYB>(DataBase1MJ_DJCJYB, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtCPMC.Text = string.Empty;
                btnSearch.PerformClick();
                btnRecount.PerformClick();
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            string sql = "SELECT b.CPMC,sum(CAST( b.[Count] as NUMERIC(18,3))) Count,MAX( a.CreateUser) from DataBase1MJ_PMCDJ a left JOIN DataBase1MJ_PMCDJ_Child b on a.Id=b.DataBase1MJ_PMCDJ_Id where a.TheYear=" + dtp.Value.Year + " and a.TheMonth=" + dtp.Value.Month + " and b.CPMC is not null group by b.CPMC";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                var datas = conn.ExecuteReader(sql);
                dt.Load(datas);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var txt = dt.Rows[i][0].ToString();
                var t1 = new BaseDal<DataBase1MJ_DJCJYB>().GetList(t => t.CPMC == txt && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                var t2 = t1.GroupBy(t => t.CPMC).ToList();
                var yb = t2.Select(t => new { CPMC = t.Key, Count = t.Sum(x => decimal.TryParse(x.SCLJ, out decimal d) ? d : 0M) }).ToList();

                if (yb == null || yb.Count == 0)
                {
                    MessageBox.Show( dtp.Value.ToString("yyyy年MM月") + "，模具大件月报不存在PMC大件中的：【" + dt.Rows[i][0].ToString() + "】");
                  
                }
                else
                {
                    decimal.TryParse(dt.Rows[i][1].ToString(), out decimal yb_Count);
                    if (yb_Count > yb[0].Count)
                    {
                        MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "产品名称:【" + yb[0].CPMC + "】，月报数量：【" + yb[0].Count + "】，日PMC大件明细数量：【" + yb_Count + "】。模具大件月报数量小于明细数量!!!");
                       
                    }
                }
            }
            var list = new BaseDal<DataBase1MJ_DJCJYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            NewMethod(list);
            MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "验证完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase1MJ_DJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase1MJ_DJCJYB;
                if (DataBase1MJ_DJCJYB != null)
                {
                    if (DataBase1MJ_DJCJYB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1MJ_DJCJYB> frm = new FrmModify<DataBase1MJ_DJCJYB>(DataBase1MJ_DJCJYB, header, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                        btnRecount.PerformClick();
                    }
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase1MJ_DJCJYB = dgv.SelectedRows[0].DataBoundItem as DataBase1MJ_DJCJYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1MJ_DJCJYB != null)
                    {
                        if (DataBase1MJ_DJCJYB.CPMC == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase1MJ_DJCJYB where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
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
                        FrmModify<DataBase1MJ_DJCJYB> frm = new FrmModify<DataBase1MJ_DJCJYB>(DataBase1MJ_DJCJYB, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                            btnRecount.PerformClick();
                        }
                    }
                }
            }
        }


        #endregion

        #region 调用方法

        private void RefDgv(DateTime selectTime, string cpmc)
        {
            var datas = new BaseDal<DataBase1MJ_DJCJYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1MJ_DJCJYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;

            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
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

        private bool NewMethod(List<DataBase1MJ_DJCJYB> list)
        {
            var datas = list.GroupBy(t => t.CPMC).Select(t => new { User = t.Max(x => x.CreateUser), CPMC = t.Key, Count = t.Sum(x => decimal.TryParse(x.SCLJ, out decimal sclj) ? sclj : 0M) }).Where(t => t.Count > 0M).ToList();
            if (datas.Count == 0)
            {
                MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "没有数据，无法验证！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                foreach (var item in datas)
                {
                    string sql = "SELECT sum(CAST( b.[Count] as NUMERIC(18,3))) from DataBase1MJ_PMCDJ a left JOIN DataBase1MJ_PMCDJ_Child b on a.Id=b.DataBase1MJ_PMCDJ_Id where a.TheYear=@TheYear and a.TheMonth=@TheMonth and b.CPMC=@cpmc";
                    var obj = conn.ExecuteScalar(sql, new { TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, cpmc = item.CPMC });
                    if (obj != null)
                    {
                        decimal.TryParse(obj.ToString(), out decimal mx);
                        if (mx > item.Count)
                        {
                            MessageBox.Show(dtp.Value.ToString("yyyy年MM月") + "产品名称:【" + item.CPMC + "】，月报数量：【" + item.Count + "】，日PMC大件明细数量：【" + mx.ToString() + "】。模具大件月报数量小于明细数量!!!");
                            return false;
                        }
                    }
                }
            }
            return true;

        }



        #endregion


    }
}
