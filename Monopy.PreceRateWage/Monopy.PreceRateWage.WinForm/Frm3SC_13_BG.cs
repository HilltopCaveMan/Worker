using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_13_BG : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工种$工段$工号$人员编码$姓名$学徒天数$熟练工天数$应出勤天数$合计$基本工资$基本工资额$技术员考核工资$开窑计件$开窑考核$回烧计件和考核$气站考核$补瓷计件和考核$烧窑工小$贴标工线上考核$贴标工线下考核$吸尘工考核$装窑计件和考核$合计".Split('$');
        private string[] header_DY = "创建日期$创建人$年$月$序号$工种$实际人数$定员$定员-实际人数".Split('$');
        private string[] header_Money = "创建日期$创建人$年$月$序号$工种$基本工资额$加班金额$实际工资(基本工资额+加班金额)$定员$基本工资$定员工资$差异（定员工资-实际工资）".Split('$');

        public Frm3SC_13_BG()
        {
            InitializeComponent();
        }

        private void Frm3SC_13_BG_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            var datas = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).DistinctBy(t => t.GZ).Select(t => t.GZ).ToList().OrderBy(x => x).ToList();
            datas.Insert(0, "全部");
            cmbLBMC.DataSource = datas;
            cmbLBMC.DisplayMember = "GZ";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string gz)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && (gz.Equals("全部") ? true : t.GZ.Equals(gz))).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_13_BG(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[13].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgv(DateTime selectTime, string gz)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3SC_13_BG_DY>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gz == "全部" ? true : t.GZ.Contains(gz))).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : decimal.MaxValue).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header_DY.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header_DY[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void RefDgvMoney(DateTime selectTime, string gz)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3SC_13_BG_GZYZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gz == "全部" ? true : t.GZ.Contains(gz))).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : decimal.MaxValue).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header_Money.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header_Money[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text, cmbLBMC.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——报工.xlsx");
        }

        private bool Recount(List<DataBase3SC_13_BG> list)
        {
            //try
            //{
            foreach (var item in list)
            {
                decimal.TryParse(item.XTTS, out decimal f);
                decimal.TryParse(item.SLGTS, out decimal g);
                decimal.TryParse(item.YCQTS, out decimal h);
                var i = f + g;
                item.HJ_SCQTS = i.ToString();
                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == item.GZ && t.Classification == item.GH);
                var j = 0M;
                if (dataMonth != null)
                {
                    decimal.TryParse(dataMonth.MoneyBase, out j);
                }
                item.JBGZ = j.ToString();
                var k = j * i / h;
                item.JBGZE = k.ToString();
                if (item.GZ.Contains("技术员"))
                {
                    var jsy = new BaseDal<DataBase3SC_12_JSYBG>().Get(t => t.GD == item.GD && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                    if (jsy != null)
                    {
                        item.JSYKHGZ = ((decimal.TryParse(jsy.JE, out decimal jsyJe) ? jsyJe : 0M) * i / h).ToString();
                    }
                    else
                    {
                        item.JSYKHGZ = 0.ToString();
                    }
                }
                else
                {
                    item.JSYKHGZ = 0.ToString();
                }
                decimal.TryParse(item.JSYKHGZ, out decimal l);
                var m = 0M;
                var n = 0M;
                if (item.GZ.Contains("开窑工"))
                {
                    var kyg = new BaseDal<DataBase3SC_05_KYBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode && t.GD == item.GD).ToList();
                    m = kyg.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M);
                    //n = kyg.Sum(t => decimal.TryParse(t.KH, out decimal d) ? d : 0M);
                    item.KYJJ = m.ToString();
                    ///////////////////////////////////////////2018-1-16---begin///////////////////////////////////////
                    var kygkh = new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                    if (kygkh != null)
                    {
                        var kh = kygkh.KHJE;
                        if (decimal.TryParse(kh, out decimal d_kh))
                        {
                            n = d_kh * g / h;
                        }
                    }

                    ///////////////////////////////////////////2018-1-16---end///////////////////////////////////////
                    //item.KYKH = n.ToString();
                }
                item.KYKH = n.ToString();
                var o = 0M;
                if (item.GZ.Contains("装窑工小"))
                {
                    var zygx = new BaseDal<DataBase3SC_04_3DZY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode).ToList();
                    o = zygx.Sum(t => decimal.TryParse(t.HSGZ, out decimal d) ? d : 0M);
                }
                item.HSJJHKH = o.ToString();
                var p = 0M;
                if (item.GZ.Contains("气站"))
                {
                    //var qzaqy = new BaseDal<DataBase3SC_07_QZAQY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode).ToList();
                    //p = qzaqy.Sum(t => decimal.TryParse(t.KHDF, out decimal d) ? d : 0m);
                    decimal.TryParse(dataMonth.MoneyKH, out decimal mKH);//500
                    var qzaqy = new BaseDal<DataBase3SC_07_QZAQY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode).ToList();
                    var fzbz = qzaqy.Sum(t => decimal.TryParse(t.FZBZ, out decimal d) ? d : 0M);
                    var khdf = qzaqy.Sum(t => decimal.TryParse(t.KHDF, out decimal d) ? d : 0M);
                    p = mKH * khdf / fzbz * g / h;
                }
                item.QZKH = p.ToString();
                var q = 0M;
                if (item.GZ.Contains("烧成补瓷工"))
                {
                    var bcghj = list.Where(t => t.GZ == "烧成补瓷工").ToList().Sum(t => (decimal.TryParse(t.XTTS, out decimal d1) ? d1 : 0M) + (decimal.TryParse(t.SLGTS, out decimal d2) ? d2 : 0M));
                    var bcg = new BaseDal<DataBase3SC_06_HSJYADL_SCBCGKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                    q = bcg.Sum(t => (decimal.TryParse(t.JE1, out decimal d1) ? d1 : 0m) + (decimal.TryParse(t.JE2, out decimal d2) ? d2 : 0m)) * i / bcghj;
                }
                item.BCJJHKH = q.ToString();
                var r = 0M;
                if (item.GZ.Contains("烧窑工小"))
                {
                    var sygx = new BaseDal<DataBase3SC_0203_ZY_HS_SXYKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                    r = sygx.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) / h * i;
                }
                item.SYGX = r.ToString();
                var s = 0m;
                if (item.GZ.Contains("贴标工线上"))
                {
                    var sygxs = new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.GD == item.GD).ToList();
                    s = sygxs.Sum(t => decimal.TryParse(t.JE2, out decimal d) ? d : 0M) / h * i;
                }
                item.TBGXSKH = s.ToString();

                var tt = 0m;
                if (item.GZ.Contains("贴标工线下"))
                {
                    var sygxs = new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.GD == item.GD).ToList();
                    tt = sygxs.Sum(t => decimal.TryParse(t.JE1, out decimal d) ? d : 0M) / h * i;
                }
                item.TBGXXKH = tt.ToString();

                var u = 0M;
                if (item.GZ.Contains("吸尘工"))
                {
                    var xcg = new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                    u = xcg.Sum(t => decimal.TryParse(t.KHJE, out decimal d) ? d : 0M) / h * i;
                }
                item.XCGKH = u.ToString();

                var v = 0M;
                if (item.GZ.Contains("装窑工"))
                {
                    var zyg = new BaseDal<DataBase3SC_01_ZYJJHKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode).ToList();
                    v = zyg.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M);
                }
                //判断同一工号下有几个装窑工
                var zyg_more = list.Where(t => t.UserCode == item.UserCode && t.GZ.Contains("装窑工")).ToList().OrderByDescending(t => decimal.TryParse(t.SLGTS, out decimal d_slg) ? d_slg : 0m).ToList().FirstOrDefault();
                if (zyg_more != null && zyg_more.Id == item.Id)
                {
                    item.ZYJJHKH = v.ToString();
                }
                else
                {
                    v = 0M;
                    item.ZYJJHKH = v.ToString();
                }
                item.HJ_Money = (k + l + m + n + o + p + q + r + s + tt + u + v).ToString();
            }
            return true;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<DataBase3SC_13_BG> list = new ExcelHelper<DataBase3SC_13_BG>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (string.IsNullOrEmpty(list[i].GZ) || string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                    var dataMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo.Equals("G003") && t.WorkshopName.Equals("烧成车间")).ToList();
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (dataMonth.Where(t => t.PostName.Equals(item.GZ)).FirstOrDefault() == null)
                        {
                            MessageBox.Show($"工种【{item.GZ}】,基础数据库中没有设定，无法导入，请检查或联系人资相关人员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        item.GH = string.IsNullOrEmpty(item.GH) ? string.Empty : item.GH;
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;

                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    var result = new BaseDal<DataBase3SC_13_BG>().Add(list) > 0;
                    if (result)
                    {
                        btnRecount.PerformClick();
                        //MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_13_BG> list = dgv.DataSource as List<DataBase3SC_13_BG>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3SC_13_BG>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——烧成——报工.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_13_BG DataBase3SC_13_BG = new DataBase3SC_13_BG() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3SC_13_BG> frm = new FrmModify<DataBase3SC_13_BG>(DataBase3SC_13_BG, header, OptionType.Add, Text, 5, 15);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                txtUserCode.Text = string.Empty;
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_13_BG> list = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3SC_13_BG>().Edit(item);
            }
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_13_BG DataBase3SC_13_BG)
                {
                    if (DataBase3SC_13_BG.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3SC_13_BG> frm = new FrmModify<DataBase3SC_13_BG>(DataBase3SC_13_BG, header, OptionType.Modify, Text, 5, 15);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3SC_13_BG = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_13_BG;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_13_BG != null)
                    {
                        if (DataBase3SC_13_BG.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3SC_13_BG>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3SC_13_BG>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3SC_13_BG> frm = new FrmModify<DataBase3SC_13_BG>(DataBase3SC_13_BG, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void btnYZ_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_13_BG> list = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据无法验证！");
                return;
            }
            var list2 = new List<DataBase3SC_13_BG>();
            foreach (var item in list)
            {
                if (item.GZ == "开窑工" && item.GD == "三段")
                {
                }
                else
                {
                    list2.Add(item);
                }
            }
            new BaseDal<DataBase3SC_13_BG_DY>().ExecuteSqlCommand("delete from DataBase3SC_13_BG_DY where theYear =" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            var list3 = list2.GroupBy(t => t.GZ).Select(t => new DataBase3SC_13_BG_DY { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, GZ = t.Key, SJRS = t.Sum(x => (decimal.TryParse(x.SLGTS, out decimal g) ? g : 0m) / (decimal.TryParse(x.YCQTS, out decimal h) ? h : 0m)).ToString(), No = t.Min(x => decimal.TryParse(x.No, out decimal d) ? d : 0M).ToString() }).ToList();
            var list4 = new BaseDal<BaseHeadcount>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory == "三厂" && t.DeptName.Contains("烧成"));
            foreach (var item in list3)
            {
                var dy = list4.Where(t => t.Name.Equals(item.GZ)).FirstOrDefault();
                if (dy == null)
                {
                    MessageBox.Show($"没有定员：【{item.GZ}】，无法计算");
                    return;
                }
                item.DY = dy.UserCount;
                item.Dy_Sjrs = ((decimal.TryParse(item.DY, out decimal d1) ? d1 : 0M) - (decimal.TryParse(item.SJRS, out decimal d2) ? d2 : 0M)).ToString();
            }
            if (new BaseDal<DataBase3SC_13_BG_DY>().Add(list3) > 0)
            {
                if (MessageBox.Show("验证完成，马上查看结果？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    btnSearchYZ.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("验证保存失败，请检查网络和操作！");
            }
        }

        private void btnSearchYZ_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbLBMC.Text);
        }

        private void btnExportExcelDY_Click(object sender, EventArgs e)
        {
            btnSearchYZ.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_13_BG_DY> list = dgv.DataSource as List<DataBase3SC_13_BG_DY>;
                if (new ExcelHelper<DataBase3SC_13_BG_DY>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——烧成——定员验证.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnJEYZ_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_13_BG> list = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据无法验证！");
                return;
            }
            var listBG = new List<DataBase3SC_13_BG>();
            foreach (var item in list)
            {
                if (item.GZ == "开窑工" && item.GD == "三段")
                {
                }
                else
                {
                    listBG.Add(item);
                }
            }
            new BaseDal<DataBase3SC_13_BG_GZYZ>().ExecuteSqlCommand("delete from DataBase3SC_13_BG_GZYZ where theYear =" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString());
            //定员
            var listDY = new BaseDal<BaseHeadcount>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory == "三厂" && t.DeptName == "烧成车间").ToList();
            var list3 = listDY.GroupBy(t => t.Name).Select(t => new DataBase3SC_13_BG_GZYZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, GZ = t.Key, DY = t.Sum(x => decimal.TryParse(x.UserCount, out decimal g) ? g : 0m).ToString() }).ToList();
            //月基础
            var listBaseMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间").ToList();
            //加班
            var listJB = new BaseDal<DataBaseGeneral_JBSPB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.FactoryNo == "G003" && t.Dept == "烧成车间").GroupBy(t => t.QGGWMC).ToList().Select(t => new DataBaseGeneral_JBSPB { QGGWMC = t.Key, Money = t.Sum(x => decimal.TryParse(x.Money, out decimal d) ? d : 0M).ToString() }).ToList();
            foreach (var item in list3)
            {
                var tmp = listBG.Where(t => t.GZ == item.GZ).ToList();
                if (tmp != null && tmp.Count > 0)
                {
                    item.No = tmp.Min(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToString();
                }
                var bg = listBG.Where(t => t.GZ == item.GZ).ToList().Sum(t => (decimal.TryParse(t.JBGZ, out decimal d) ? d : 0m) / (decimal.TryParse(t.YCQTS, out decimal d1) ? d1 : 0m) * (decimal.TryParse(t.SLGTS, out decimal d3) ? d3 : 0m));
                item.JBGZE = bg.ToString();
                var t1 = listJB.Where(t => t.QGGWMC == item.GZ).FirstOrDefault();
                if (t1 != null)
                {
                    item.JBJE = t1.Money;
                }
                else
                {
                    item.JBJE = 0.ToString();
                }
                item.SJGZ = (bg + (decimal.TryParse(item.JBJE, out decimal d2) ? d2 : 0M)).ToString();
                var t3 = listBaseMonth.Where(t => t.PostName == item.GZ).FirstOrDefault();
                var moneyBase = 0M;
                if (t3 != null)
                {
                    decimal.TryParse(t3.MoneyBase, out moneyBase);
                }
                item.JBGZ = moneyBase.ToString();
                item.DYGZ = ((decimal.TryParse(item.DY, out decimal dy) ? dy : 0m) * moneyBase).ToString();
                item.CY = ((decimal.TryParse(item.DYGZ, out decimal dx) ? dx : 0M) - (decimal.TryParse(item.SJGZ, out decimal dyy) ? dyy : 0M)).ToString();
            }
            if (new BaseDal<DataBase3SC_13_BG_GZYZ>().Add(list3) > 0)
            {
                if (MessageBox.Show("验证完成，马上查看结果？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    btnSearchJEYZ.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("验证保存失败，请检查网络和操作！");
            }
        }

        private void btnExportExcelJEYZ_Click(object sender, EventArgs e)
        {
            btnSearchJEYZ.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_13_BG_GZYZ> list = dgv.DataSource as List<DataBase3SC_13_BG_GZYZ>;
                if (new ExcelHelper<DataBase3SC_13_BG_GZYZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——烧成——金额验证.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnSearchJEYZ_Click(object sender, EventArgs e)
        {
            RefDgvMoney(dtp.Value, cmbLBMC.Text);
        }
    }
}