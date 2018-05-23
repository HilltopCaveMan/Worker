using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_03_HS : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$类型$序号$产品名称$颜色$开窑量$产品等级_合格$产品等级_等外$产品等级_残$产品等级_冷补修$产品等级_回烧$产品等级_磨$产品等级_磨修$合格率$原料缺陷_铜脏$原料缺陷_铁脏$原料缺陷_泥脏$原料缺陷_小计$缺陷率$成型缺陷_成走$成型缺陷_内裂$成型缺陷_裂底$成型缺陷_裂体$成型缺陷_裂眼$成型缺陷_修糙$成型缺陷_棕眼$成型缺陷_成脏$成型缺陷_泥绺$成型缺陷_坯泡$成型缺陷_崩渣$成型缺陷_冲刷$成型缺陷_卡球$成型缺陷_吹脏$成型缺陷_小计$缺陷率$修挡板$修检_裂体$修检_修糙$修检_棕眼$修检_成脏$修检_卡球$修检_滚釉$修检_爆釉$修检_坯渣$修检_小计$缺陷率$喷釉缺陷_滚釉$喷釉缺陷_滚釉釉薄$喷釉缺陷_滚釉釉厚$喷釉缺陷_滚釉釉缕$喷釉缺陷_滚釉爆釉$喷釉缺陷_滚釉坯渣$喷釉缺陷_滚釉坯磕$喷釉缺陷_滚釉釉脏$喷釉缺陷_滚釉刮边$喷釉缺陷_滚釉釉粘$喷釉缺陷_滚釉坯脏$喷釉缺陷_滚釉小计$缺陷率$回烧缺陷_修补不合格$回烧缺陷_漏补$回烧缺陷_缺陷$回烧缺陷_小计$缺陷率$装窑缺陷_装走$装窑缺陷_装粘$装窑缺陷_装磕$装窑缺陷_装脏$装窑缺陷_刮边$装窑缺陷_小计$缺陷率$开窑缺陷_出磕$开窑缺陷_划釉$开窑缺陷_小计$缺陷率$烧窑缺陷_桔釉$烧窑缺陷_氧化$烧窑缺陷_烧泡$烧窑缺陷_烧裂$烧窑缺陷_烧生$烧窑缺陷_窑脏$烧窑缺陷_风惊$烧窑缺陷_小计$缺陷率$标污$烧成缺陷率$吹脏$缺陷率$崩脏$缺陷率".Split('$');
        private string[] headerSXY = "创建日期$创建人$年$月$总窑开窑量$回烧窑合格品$回烧窑磨$回烧贡献率$指标$金额".Split('$');
        private string[] headerZYGX = "创建日期$创建人$年$月$回烧窑开窑量$单价$金额$一级品$实际合格率$指标$金额$缺陷数$实际缺陷率$指标$金额$合计".Split('$');

        public Frm3SC_03_HS()
        {
            InitializeComponent();
        }

        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                return;
            }
            System.Reflection.FieldInfo[] fieldInfo = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ContextMenuStrip> contextMenuStrip = new List<ContextMenuStrip>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfo[i].FieldType == typeof(ButtonX))
                {
                    ((ButtonX)fieldInfo[i].GetValue(this)).Enabled = false;
                    //Controls.Find(fieldInfo[i].Name, true)[0].Enabled = false;
                    continue;
                }

                if (fieldInfo[i].FieldType == typeof(ContextMenuStrip))
                {
                    var tmp = (ContextMenuStrip)fieldInfo[i].GetValue(this);
                    contextMenuStrip.Add(tmp);
                    foreach (var item in tmp.Items)
                    {
                        if (item is ToolStripMenuItem t)
                        {
                            t.Enabled = false;
                        }
                    }
                }
            }

            var dic = MenuBarHelper.FrmControlEnable(Program.User.Code);
            foreach (var item in dic.Keys)
            {
                var cons = Controls.Find(item, true);
                if (cons.Length > 0)
                {
                    cons[0].Enabled = dic[item];
                    continue;
                }
                foreach (var contextMs in contextMenuStrip)
                {
                    foreach (var item2 in contextMs.Items)
                    {
                        if (item2 is ToolStripMenuItem x && x.Name == item)
                        {
                            x.Enabled = dic[item];
                        }
                    }
                }
            }
        }

        private void Frm3SC_03_HS_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string cpmc)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheType == 2 && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            dgv.DataSource = null;
            dgv.DataSource = datas;
            dgv.ColumnHeadersVisible = true;
            dgv.RowHeadersVisible = true;
            for (int i = 0; i < 7; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[8].Visible = false;

            dgv.Columns[9].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgvSXY(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS_SXYKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().ToList();
            dgv.DataSource = datas;
            dgv.ColumnHeadersVisible = true;
            dgv.RowHeadersVisible = true;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerSXY.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerSXY[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void RefDgvZYG(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_0203_ZY_HS_ZYGXKH>().Get(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month);
            var list = new List<object>();
            var x1 = new { a1 = "回烧窑开窑量", a2 = "单价", a3 = "", a4 = "", a5 = "金额" };
            list.Add(x1);
            var x2 = new { a1 = datas.HSYKYL, a2 = datas.DJ, a3 = "", a4 = "", a5 = datas.JE1 };
            list.Add(x2);
            var x3 = new { a1 = "回烧窑开窑量", a2 = "一级品", a3 = "实际合格率", a4 = "指标", a5 = "金额" };
            list.Add(x3);
            var x4 = new { a1 = datas.HSYKYL, a2 = datas.YJP, a3 = datas.SJHGL, a4 = datas.ZB2, a5 = datas.JE2_GRKH1 };
            list.Add(x4);
            var x5 = new { a1 = "回烧窑开窑量", a2 = "缺陷数", a3 = "实际缺陷率", a4 = "指标", a5 = "金额" };
            list.Add(x5);
            var x6 = new { a1 = datas.HSYKYL, a2 = datas.QXS, a3 = datas.SJQXL, a4 = datas.ZB3, a5 = datas.JE3_GRKH2 };
            list.Add(x6);
            var x7 = new { a1 = "合计", a2 = "", a3 = "", a4 = "", a5 = datas.ZJ };
            list.Add(x7);

            dgv.DataSource = list;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        private void btnSearchSXY_Click(object sender, EventArgs e)
        {
            RefDgvSXY(dtp.Value);
        }

        private void btnSearchZYG_Click(object sender, EventArgs e)
        {
            RefDgvZYG(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx");
        }

        private DataBase3SC_0203_ZY_HS_SXYKH Recount_SXY(List<DataBase3SC_0203_ZY_HS> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【烧小窑考核】！");
                return null;
            }
            DataBase3SC_0203_ZY_HS_SXYKH sXYKH = new DataBase3SC_0203_ZY_HS_SXYKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };

            try
            {
                var zyxj = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.TheType == 1 && t.CPMC == "小计").ToList().OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                sXYKH.ZYKYL = zyxj.KYL;
                var xj = list.Where(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.TheType == 2 && t.CPMC == "小计").ToList().OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                sXYKH.HSYHGP = xj.CPDJ_HG;
                sXYKH.HSYM = xj.CPDJ_M;
                decimal.TryParse(sXYKH.ZYKYL, out decimal cj);
                decimal.TryParse(sXYKH.HSYHGP, out decimal ck);
                decimal.TryParse(sXYKH.HSYM, out decimal cl);
                if (cj == 0)
                {
                    sXYKH.HSGXL = 0.ToString();
                }
                else
                {
                    sXYKH.HSGXL = ((ck + cl) / cj).ToString();
                }
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "烧窑工小");
                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "烧窑工小");
                sXYKH.ZB = dataDay.GRKHZB1;
                decimal.TryParse(sXYKH.HSGXL, out decimal cm);
                decimal.TryParse(sXYKH.ZB, out decimal cn);
                decimal.TryParse(dataMonth.MoneyKH, out decimal d300);
                decimal.TryParse(dataDay.GRJSB1, out decimal d01);
                decimal.TryParse(dataDay.GRJLDJ1, out decimal d50);
                if (cm <= cn)
                {
                    sXYKH.JE = d300.ToString();
                }
                else
                {
                    if ((d300 - (cm - cn) / d01 * d50) <= 0)
                    {
                        sXYKH.JE = 0.ToString();
                    }
                    else
                    {
                        sXYKH.JE = (d300 - (cm - cn) / d01 * d50).ToString();
                    }
                }
                return sXYKH;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private DataBase3SC_0203_ZY_HS_ZYGXKH Recount_ZYGX(List<DataBase3SC_0203_ZY_HS> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【开窑工考核】！");
                return null;
            }
            DataBase3SC_0203_ZY_HS_ZYGXKH zYGXKH = new DataBase3SC_0203_ZY_HS_ZYGXKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };
            try
            {
                var zj = list.Where(t => t.CPMC == "总计").FirstOrDefault();
                zYGXKH.HSYKYL = zj.KYL;
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "装窑工小");
                zYGXKH.DJ = dataDay.UnitPrice;
                decimal.TryParse(zYGXKH.HSYKYL, out decimal cj1);
                decimal.TryParse(zYGXKH.DJ, out decimal ck1);
                zYGXKH.JE1 = (cj1 * ck1).ToString();

                zYGXKH.YJP = ((decimal.TryParse(zj.CPDJ_HG, out decimal hg) ? hg : 0M) + (decimal.TryParse(zj.CPDJ_M, out decimal m) ? m : 0M)).ToString();
                decimal.TryParse(zYGXKH.YJP, out decimal ck2);
                var cl2 = ck2 / cj1;
                zYGXKH.SJHGL = cl2.ToString();
                zYGXKH.ZB2 = dataDay.GRKHZB1;
                decimal.TryParse(zYGXKH.ZB2, out decimal cm2);
                decimal.TryParse(dataDay.GRJSB1, out decimal d01);
                decimal.TryParse(dataDay.GRJLDJ1, out decimal d60);
                zYGXKH.JE2_GRKH1 = ((cl2 - cm2) / d01 * d60).ToString();

                zYGXKH.QXS = ((decimal.TryParse(zj.ZYQX_01, out decimal z1) ? z1 : 0M) + (decimal.TryParse(zj.ZYQX_02, out decimal z2) ? z2 : 0M) + (decimal.TryParse(zj.ZYQX_03, out decimal z3) ? z3 : 0M) + (decimal.TryParse(zj.ZYQX_04, out decimal z4) ? z4 : 0M) + (decimal.TryParse(zj.KYQX_03, out decimal k1) ? k1 : 0M) + (decimal.TryParse(zj.BW, out decimal bw) ? bw : 0M)).ToString();
                decimal.TryParse(zYGXKH.QXS, out decimal ck3);
                zYGXKH.SJQXL = (ck3 / cj1).ToString();
                zYGXKH.ZB3 = dataDay.GRKHZB2;
                decimal.TryParse(zYGXKH.ZB3, out decimal cm3);
                decimal.TryParse(dataDay.GRJLDJ2, out decimal d7);
                decimal.TryParse(dataDay.GRFKDJ2, out decimal d10);

                decimal.TryParse(zYGXKH.SJQXL, out decimal cl3);

                if (cl3 <= cm3)
                {
                    zYGXKH.JE3_GRKH2 = ((cm3 - cl3) * cj1 * d7).ToString();
                }
                else
                {
                    if (cl3 > cm3)
                    {
                        zYGXKH.JE3_GRKH2 = ((cm3 - cl3) * cj1 * d10).ToString();
                    }
                    if (cl3 == 0)
                    {
                        zYGXKH.JE3_GRKH2 = 0.ToString();
                    }
                }
                zYGXKH.ZJ = ((decimal.TryParse(zYGXKH.JE1, out decimal je1) ? je1 : 0M) + (decimal.TryParse(zYGXKH.JE2_GRKH1, out decimal je2) ? je2 : 0M) + (decimal.TryParse(zYGXKH.JE3_GRKH2, out decimal je3) ? je3 : 0M)).ToString();

                return zYGXKH;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
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
                    List<DataBase3SC_0203_ZY_HS> list = new ExcelHelper<DataBase3SC_0203_ZY_HS>().ReadExcel(openFileDlg.FileName, 4, 7, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        item.TheType = 2;
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase3SC_0203_ZY_HS>().Add(list) > 0)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                        MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<DataBase3SC_0203_ZY_HS> list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS>;
                if (new ExcelHelper<DataBase3SC_0203_ZY_HS>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx", saveFileDlg.FileName, list, 4, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_0203_ZY_HS DataBase3SC_0203_ZY_HS = new DataBase3SC_0203_ZY_HS() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString(), TheType = 2 };
            FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Add, Text, 6, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_0203_ZY_HS> list = new BaseDal<DataBase3SC_0203_ZY_HS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.TheType == 2).ToList();

            new BaseDal<DataBase3SC_0203_ZY_HS_SXYKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_SXYKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
            new BaseDal<DataBase3SC_0203_ZY_HS_ZYGXKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_ZYGXKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);

            if (new BaseDal<DataBase3SC_0203_ZY_HS_SXYKH>().Add(Recount_SXY(list)) > 0 && new BaseDal<DataBase3SC_0203_ZY_HS_ZYGXKH>().Add(Recount_ZYGX(list)) > 0)
            {
                MessageBox.Show("烧小窑、装窑工小，计算成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportExcelSXY_Click(object sender, EventArgs e)
        {
            btnSearchSXY.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_0203_ZY_HS_SXYKH> list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS_SXYKH>;
                if (new ExcelHelper<DataBase3SC_0203_ZY_HS_SXYKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——烧小窑考核.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnExportExcelZYG_Click(object sender, EventArgs e)
        {
            btnSearchZYG.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var data = new BaseDal<DataBase3SC_0203_ZY_HS_ZYGXKH>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                try
                {
                    FileStream file = File.OpenRead(Application.StartupPath + "\\Excel\\模板三厂——烧成——装窑工小考核.xlsx");
                    IWorkbook workbook = WorkbookFactory.Create(file);
                    ICellStyle style = workbook.CreateCellStyle();
                    style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    ISheet sheet = workbook.GetSheetAt(0);

                    var tCell = sheet.GetRow(0).GetCell(0);
                    tCell.CellStyle = style;
                    string sCell = ExcelHelper.GetCellValue(tCell).ToUpper();
                    sCell = sCell.Replace("XXXX", dtp.Value.Year.ToString());
                    sCell = sCell.Replace("XX", dtp.Value.Month.ToString());
                    tCell.SetCellValue(sCell);

                    tCell = sheet.GetRow(2).GetCell(0);
                    tCell.SetCellValue(data.HSYKYL);
                    tCell = sheet.GetRow(2).GetCell(1);
                    tCell.SetCellValue(data.DJ);
                    tCell = sheet.GetRow(2).GetCell(4);
                    tCell.SetCellValue(data.JE1);

                    tCell = sheet.GetRow(4).GetCell(0);
                    tCell.SetCellValue(data.HSYKYL);
                    tCell = sheet.GetRow(4).GetCell(1);
                    tCell.SetCellValue(data.YJP);
                    tCell = sheet.GetRow(4).GetCell(2);
                    tCell.SetCellValue(data.SJHGL);
                    tCell = sheet.GetRow(4).GetCell(3);
                    tCell.SetCellValue(data.ZB2);
                    tCell = sheet.GetRow(4).GetCell(4);
                    tCell.SetCellValue(data.JE2_GRKH1);

                    tCell = sheet.GetRow(6).GetCell(0);
                    tCell.SetCellValue(data.HSYKYL);
                    tCell = sheet.GetRow(6).GetCell(1);
                    tCell.SetCellValue(data.QXS);
                    tCell = sheet.GetRow(6).GetCell(2);
                    tCell.SetCellValue(data.SJQXL);
                    tCell = sheet.GetRow(6).GetCell(3);
                    tCell.SetCellValue(data.ZB3);
                    tCell = sheet.GetRow(6).GetCell(4);
                    tCell.SetCellValue(data.JE3_GRKH2);

                    tCell = sheet.GetRow(7).GetCell(4);
                    tCell.SetCellValue(data.ZJ);

                    FileStream sw = File.Create(saveFileDlg.FileName);
                    workbook.Write(sw);
                    sw.Close();
                    sw.Dispose();
                    file.Close();
                    file.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出错误，请检查后，再试！" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }

                //if (new ExcelHelper<DataBase3SC_0203_ZY_HS_KYGKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——装窑工小考核.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
                //{
                if (MessageBox.Show("导出成功，立即打开？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Process.Start(saveFileDlg.FileName);
                }
                //}
                //else
                //{
                //    MessageBox.Show("导出错误，请检查后，再试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                Enabled = true;
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_0203_ZY_HS DataBase3SC_0203_ZY_HS)
                {
                    FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Modify, Text, 6, 0);
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
                var DataBase3SC_0203_ZY_HS = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_0203_ZY_HS;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_0203_ZY_HS != null)
                    {
                        FrmModify<DataBase3SC_0203_ZY_HS> frm = new FrmModify<DataBase3SC_0203_ZY_HS>(DataBase3SC_0203_ZY_HS, header, OptionType.Delete, Text, 6, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase3SC_0203_ZY_HS>;
                foreach (var item in list)
                {
                    new BaseDal<DataBase3SC_0203_ZY_HS>().Delete(item);
                }
                new BaseDal<DataBase3SC_0203_ZY_HS_XCGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_XCGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
                new BaseDal<DataBase3SC_0203_ZY_HS_KYGKH>().ExecuteSqlCommand("delete from DataBase3SC_0203_ZY_HS_KYGKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }
    }
}