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
    public partial class Frm3PY_BG : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$类别$工段$工种$人员编码$姓名$工号$熟练工天数$应出勤天数$学徒工天数$合计$基本工资$技能工资$加班工资$学徒基本工资占比$学徒技能工资占比$喷釉班组考核占比$基本工资额$技能工资额$加班工资额$班组考核工资$修检个人考核工资$修检计件工资$擦绺工个人考核$喷釉计件$喷釉考核$擦水计件$擦水考核$金额".Split('$');
        private string[] headerYz = "年$月$类别$工段$工种$总出勤$实际人数$定员$定员-实际人数,操作人".Split('$');

        public Frm3PY_BG()
        {
            InitializeComponent();
        }

        private void Frm3PY_BG_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string code)
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
            var datas = new BaseDal<DataBase3PY_BG>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.GH.Contains(code)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_BG(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[11].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgvYZ(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3PY_BG_YZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => t.LB).ThenBy(t => t.GD).ThenBy(t => t.GZ).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < headerYz.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerYz[i];
            }
            dgv.Columns[0].Visible = false;
            dgv.Columns[11].Visible = false;
            for (int i = 1; i < datas.Count; i++)
            {
                if (!datas[i].IsOK)
                {
                    dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text, txtCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——喷釉——报工.xlsx");
        }

        private bool Recount(List<DataBase3PY_BG> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算！");
                return false;
            }
            //try
            //{
            var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "喷釉车间");
            var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "喷釉车间");
            var listBzKhSum = new BaseDal<DataBase3PY_BZ_KH_Sum>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var listJxgKh = new BaseDal<DataBase3PY_JXG_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var listClgKh = new BaseDal<DataBase3PY_CLG_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var lstPygzhs = new BaseDal<DataBase3PY_PYGZHS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList().GroupBy(t => new { t.GH, t.UserCode, t.UserName, t.LX }).Select(t => new DataBase3PY_PYGZHS { GH = t.Key.GH, UserCode = t.Key.UserCode, UserName = t.Key.UserName, LX = t.Key.LX, E_KHJE = t.Sum(x => decimal.TryParse(x.E_KHJE, out decimal khje) ? khje : 0M).ToString(), E_JJJE = t.Sum(x => decimal.TryParse(x.E_JJJE, out decimal jjje) ? jjje : 0M).ToString(), E_Money = t.Sum(x => decimal.TryParse(x.E_Money, out decimal money) ? money : 0M).ToString() }).ToList();

            foreach (var item in list)
            {
                item.PYJJ = string.Empty;
                item.PYKH = string.Empty;
                item.CSJJ = string.Empty;
                item.CSKH = string.Empty;
                decimal.TryParse(item.SLGTS, out decimal g);
                decimal.TryParse(item.XTGTS, out decimal i);
                decimal.TryParse(item.YCQTS, out decimal h);

                item.SCQTS = (g + i).ToString();
                var month = listMonth.Where(t => t.PostName == item.GZ && t.Classification == item.LB).FirstOrDefault();
                if (month == null)
                {
                    item.JBGZ = 0.ToString();
                    item.JNGZ = 0.ToString();
                    item.JiaBGZ = 0.ToString();
                    item.XTJBGZZB = 0.ToString();
                    item.XTJNGZZB = 0.ToString();
                }
                else
                {
                    item.JBGZ = month.MoneyBase;
                    item.JNGZ = month.MoneyJN;
                    item.JiaBGZ = month.MoneyJB;
                    item.XTJBGZZB = month.ZB_XT_JB;
                    item.XTJNGZZB = month.ZB_XT_JN;
                }
                var day = listDay.Where(t => t.PostName == item.GZ && t.Classification == item.LB).FirstOrDefault();
                if (day == null)
                {
                    item.PYBZKHZB = 0.ToString();
                }
                else
                {
                    item.PYBZKHZB = day.ZB_PY_BZKH;
                }
                decimal.TryParse(item.JBGZ, out decimal K);
                decimal.TryParse(item.XTJBGZZB, out decimal n);
                decimal.TryParse(item.JNGZ, out decimal l);
                decimal.TryParse(item.XTJNGZZB, out decimal o);
                decimal.TryParse(item.JiaBGZ, out decimal m);

                //item.JBGZE = (h == 0) ? 0.ToString() : (K / h * g + K / h * i * n).ToString();
                item.JBGZE = (h == 0) ? 0.ToString() : (K * (g / h) + K * (i / h) * n).ToString();
                //item.JNGZE = (h == 0) ? 0.ToString() : (l / h * g + l / h * i * o).ToString();
                item.JNGZE = (h == 0) ? 0.ToString() : (l * (g / h) + l * (i / h) * o).ToString();

                item.JiaBGZE = (h == 0) ? 0.ToString() : (m / h * (g + i)).ToString();
                //item.JiaBGZE = (h == 0) ? 0.ToString() : (m / ((g + i) / h)).ToString();

                var bzkh = listBzKhSum.Where(t => t.LB == item.LB && t.GD == item.GD && t.GZ == item.GZ).FirstOrDefault();
                if (bzkh != null)
                {
                    decimal.TryParse(bzkh.KHJE, out decimal khje);
                    //item.BZKHGZ = (khje / h * g).ToString();
                    item.BZKHGZ = (khje * (g / h)).ToString();
                }

                var jxgKh = listJxgKh.Where(t => t.GZ == item.GZ && t.UserCode == item.UserCode && t.GH == item.GH).FirstOrDefault();
                if (jxgKh != null)
                {
                    decimal.TryParse(jxgKh.KHJE, out decimal khje);
                    //item.JXGRKHGZ = (khje / h * g).ToString();
                    item.JXGRKHGZ = (khje * (g / h)).ToString();
                    item.JXJJGZ = jxgKh.JJJE;
                }

                var clgKh = listClgKh.Where(t => t.GZ == item.GZ && t.GH == item.GH).FirstOrDefault();
                if (clgKh != null)
                {
                    decimal.TryParse(clgKh.KHJE, out decimal clgKhje);
                    //item.CLGGRKH = (clgKhje / h * g).ToString();
                    item.CLGGRKH = (clgKhje * (g / h)).ToString();
                }
            }

            foreach (var item in list.Where(t => t.GZ == "机器人喷釉工" || t.GZ == "手工橱喷釉工"))
            {
                var jjjeSum = lstPygzhs.Where(t => t.GH == item.GH).Sum(t => decimal.TryParse(t.E_JJJE, out decimal e_money) ? e_money : 0M);
                var khjeSum = lstPygzhs.Where(t => t.GH == item.GH).Sum(t => decimal.TryParse(t.E_KHJE, out decimal e_money) ? e_money : 0M);
                var scqSum = list.Where(t => t.GH == item.GH && t.GZ == item.GZ).Sum(t => decimal.TryParse(t.SCQTS, out decimal e_scq) ? e_scq : 0M);
                decimal.TryParse(item.SCQTS, out decimal scqts);
                if (scqts > 0)
                {
                    item.PYJJ = (jjjeSum / scqSum * scqts).ToString();
                    item.PYKH = (khjeSum / scqSum * scqts).ToString();
                }
            }

            foreach (var item in list.Where(t => t.GZ == "机器人擦水工"))
            {
                var jqrpyJJSum = list.Where(t => t.GZ == "机器人喷釉工" && t.GH == item.GH).Sum(t => decimal.TryParse(t.PYJJ, out decimal pyjj) ? pyjj : 0M);
                var jqrpyKHSum = list.Where(t => t.GZ == "机器人喷釉工" && t.GH == item.GH).Sum(t => decimal.TryParse(t.PYKH, out decimal pyjj) ? pyjj : 0M);

                var scqSum = list.Where(t => t.GH == item.GH && t.GZ == "机器人喷釉工").Sum(t => decimal.TryParse(t.SCQTS, out decimal e_scq) ? e_scq : 0M);
                decimal.TryParse(item.SCQTS, out decimal scqts);
                decimal.TryParse(item.PYBZKHZB, out decimal pybzkhzb);
                item.CSJJ = (jqrpyJJSum / scqSum * scqts * pybzkhzb).ToString();
                item.CSKH = (jqrpyKHSum / scqSum * scqts * pybzkhzb).ToString();
            }

            foreach (var item in list.Where(t => t.GZ == "手工橱擦水工"))
            {
                var jqrpyJJSum = list.Where(t => t.GZ == "手工橱喷釉工" && t.GH == item.GH).Sum(t => decimal.TryParse(t.PYJJ, out decimal pyjj) ? pyjj : 0M);
                var jqrpyKHSum = list.Where(t => t.GZ == "手工橱喷釉工" && t.GH == item.GH).Sum(t => decimal.TryParse(t.PYKH, out decimal pyjj) ? pyjj : 0M);

                var scqSum = list.Where(t => t.GH == item.GH && t.GZ == "手工橱喷釉工").Sum(t => decimal.TryParse(t.SCQTS, out decimal e_scq) ? e_scq : 0M);
                decimal.TryParse(item.SCQTS, out decimal scqts);
                decimal.TryParse(item.PYBZKHZB, out decimal pybzkhzb);
                item.CSJJ = (jqrpyJJSum / scqSum * scqts * pybzkhzb).ToString();
                item.CSKH = (jqrpyKHSum / scqSum * scqts * pybzkhzb).ToString();
            }

            foreach (var item in list)
            {
                decimal.TryParse(item.JBGZE, out decimal q);
                decimal.TryParse(item.JNGZE, out decimal r);
                decimal.TryParse(item.JiaBGZE, out decimal s);
                decimal.TryParse(item.BZKHGZ, out decimal t);
                decimal.TryParse(item.JXGRKHGZ, out decimal u);
                decimal.TryParse(item.JXJJGZ, out decimal v);
                decimal.TryParse(item.CLGGRKH, out decimal w);
                decimal.TryParse(item.PYJJ, out decimal x);
                decimal.TryParse(item.PYKH, out decimal y);
                decimal.TryParse(item.CSJJ, out decimal z);
                decimal.TryParse(item.CSKH, out decimal aa);
                item.HJ = (q + r + s + t + u + v + w + x + y + z + aa).ToString();
            }

            //foreach (var item in list.Where(t => t.GZ == "机器人喷釉工").GroupBy(t => t.GH))
            //{
            //}

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
                //try
                //{
                List<DataBase3PY_BG> list = new ExcelHelper<DataBase3PY_BG>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.LB) || string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                    if (!string.IsNullOrEmpty(item.UserCode) && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (Recount(list) && new BaseDal<DataBase3PY_BG>().Add(list) > 0)
                {
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
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
                List<DataBase3PY_BG> list = dgv.DataSource as List<DataBase3PY_BG>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_BG>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——报工.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3PY_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3PY_BG DataBase3PY_BG = new DataBase3PY_BG() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3PY_BG> frm = new FrmModify<DataBase3PY_BG>(DataBase3PY_BG, header, OptionType.Add, Text, 5, 19);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3PY_BG> list = new BaseDal<DataBase3PY_BG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (Recount(list))
            {
                Enabled = false;
                foreach (var item in list)
                {
                    new BaseDal<DataBase3PY_BG>().Edit(item);
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3PY_BG DataBase3PY_BG)
                {
                    if (DataBase3PY_BG.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3PY_BG> frm = new FrmModify<DataBase3PY_BG>(DataBase3PY_BG, header, OptionType.Modify, Text, 5, 19);
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
                var DataBase3PY_BG = dgv.SelectedRows[0].DataBoundItem as DataBase3PY_BG;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3PY_BG != null)
                    {
                        if (DataBase3PY_BG.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3PY_BG>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3PY_BG>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3PY_BG> frm = new FrmModify<DataBase3PY_BG>(DataBase3PY_BG, header, OptionType.Delete, Text, 5, 0);
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
            new BaseDal<DataBase3PY_BG_YZ>().ExecuteSqlCommand("delete from DataBase3PY_BG_YZ where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
            var list = new BaseDal<DataBase3PY_BG>().GetList(t => t.TheYear.Equals(dtp.Value.Year) && t.TheMonth.Equals(dtp.Value.Month)).ToList().GroupBy(t => new { t.LB, t.GD, t.GZ }).Select(t => new DataBase3PY_BG_YZ { Id = Guid.NewGuid(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, LB = t.Key.LB, GD = t.Key.GD, GZ = t.Key.GZ, SLGTS = t.Sum(x => decimal.TryParse(x.SLGTS, out decimal d) ? d : 0M), CreateUser = t.FirstOrDefault().CreateUser, IsOK = true }).ToList();
            foreach (var item in list)
            {
                switch (item.GD)
                {
                    case "一段":
                        item.Sjrs = item.SLGTS / n_Ycq1.Value;
                        break;

                    case "二段":
                        item.Sjrs = item.SLGTS / n_Ycq2.Value;
                        break;

                    case "三段":
                        item.Sjrs = item.SLGTS / n_Ycq3.Value;
                        break;
                }
                var itemDy = new BaseDal<BaseHeadcount>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory == "三厂" && t.DeptName == "喷釉车间" && t.Line == item.LB && t.Name == item.GZ);
                if (itemDy == null)
                {
                    //报警——没有此定员
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "定员验证失败，无此定员", Msg = $"类别：{item.LB},工种：{item.GZ},定员中没有定员信息！", MsgClass = "喷釉报警", UserCode = item.CreateUser.Split('_')[0] }, true);
                    item.IsOK = false;
                    continue;
                }
                item.Dy = (decimal.TryParse(itemDy.UserCount, out decimal d_dy) ? d_dy : 0m) / 3M;
                item.Dy_Sjrs = item.Dy - item.Sjrs;
                if (item.Dy_Sjrs < 0)
                {
                    //报警--定员小于实际人数
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "定员验证失败，定员小于实际人数", Msg = $"类别：{item.LB},工种：{item.GZ}，总出勤：{item.Sjrs}，实际人数：{item.Sjrs}，定员：{item.Dy}，定员-实际人数：{item.Dy_Sjrs},定员小于实际人数", MsgClass = "喷釉报警", UserCode = item.CreateUser.Split('_')[0] }, true);
                    item.IsOK = false;
                    continue;
                }
            }
            if (new BaseDal<DataBase3PY_BG_YZ>().Add(list) > 0)
            {
                if (MessageBox.Show("验证完成，查看验证信息！", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnSearchYZ.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("验证失败，请检查操作和网络，或和管理员联系！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BJ(DataBaseMsg dataBaseMsg, bool isToHr)
        {
            new BaseDal<DataBaseMsg>().Add(dataBaseMsg);
            if (isToHr)
            {
                dataBaseMsg.ID = Guid.NewGuid();
                dataBaseMsg.UserCode = Program.HrCode;
                new BaseDal<DataBaseMsg>().Add(dataBaseMsg);
            }
        }

        private void btnSearchYZ_Click(object sender, EventArgs e)
        {
            RefDgvYZ(dtp.Value);
        }
    }
}