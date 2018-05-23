using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_GYMJ_081_ZLWCHZB : Office2007Form
    {
        public Frm3CX_GYMJ_081_ZLWCHZB()
        {
            InitializeComponent();
        }

        private void btnBulid_Click(object sender, EventArgs e)
        {
            var oldList = new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (oldList.Count > 0)
            {
                if (MessageBox.Show("数据已经存在，删除后，再生成？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach (var item in oldList)
                    {
                        new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().Delete(item);
                    }
                }
                else
                {
                    return;
                }
            }

            var dataDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "高压面具").ToList();
            var dataMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "高压面具").ToList();
            if (dataDay == null || dataDay.Count == 0)
            {
                MessageBox.Show("没有指标基础数据，请联系管理员");
                return;
            }
            if (dataMonth == null || dataMonth.Count == 0)
            {
                MessageBox.Show("没有月基础数据，请联系管理员");
                return;
            }
            var bzf = new BaseDal<DataBase3CX_GYMJ_07_BZFGY>().GetList(t => t.TheType.Contains("面具") && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (bzf == null || bzf.Count == 0)
            {
                MessageBox.Show("没有导入【班长费】，请先导入，才能生成！");
                return;
            }
            var cxyb = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (cxyb == null || cxyb.Count == 0)
            {
                MessageBox.Show("没有导入【成型月报】，请先导入，才能生成！");
                return;
            }
            var pmc = new BaseDal<DataBase3CX_GYMJ_01_PMCDDGYJJJP>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (pmc == null || pmc.Count == 0)
            {
                MessageBox.Show("没有导入【PMC调度高压计件交坯】，请先导入，才能生成！");
                return;
            }

            var cjbg = new BaseDal<DataBase3CX_GYMJ_05_CJBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => t.GH).ThenBy(t => t.CPMC).ToList();
            if (cjbg == null || cjbg.Count == 0)
            {
                MessageBox.Show("没有导入【车间报工】，请先导入，才能生成！");
                return;
            }
            var list = new List<DataBase3CX_GYMJ_081_ZLWCHZB>();
            for (int i = 0; i < cjbg.Count; i++)
            {
                var item = new DataBase3CX_GYMJ_081_ZLWCHZB { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = (i + 1).ToString(), GH = cjbg[i].GH, BZCLZL_03_CPLX = cjbg[i].CPMC, BZJPQK_03_YYJWJP = cjbg[i].YYJWJP, ZJCS1 = cjbg[i].ZJCS1, ZJCS2 = cjbg[i].ZJCS2, ZJCS3 = cjbg[i].ZJCS3, ZJCS4 = cjbg[i].ZJCS4, MXS1 = cjbg[i].MXS1, MXS2 = cjbg[i].MXS2, MXS3 = cjbg[i].MXS3, MXS4 = cjbg[i].MXS4 };
                var jbb = new BaseDal<DataBase3CX_GYMJ_06_JPB>().Get(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.GH == item.GH);
                if (jbb == null)
                {
                    MessageBox.Show($"交坯表没有【{item.GH}】的数据，或者没有导入交坯表，无法生成！");
                    return;
                }
                item.JPS = jbb.Childs.Where(t => t.Name == item.BZCLZL_03_CPLX).ToList().Sum(t => decimal.TryParse(t.Count, out decimal d) ? d : 0M).ToString();
                var tmp = dataDay.Where(t => !string.IsNullOrEmpty(t.KHJEXFD)).FirstOrDefault();
                if (tmp == null)
                {
                    MessageBox.Show("指标数据没有【考核金额下封顶】数据，无法生成！");
                    return;
                }
                item.BZJPQK_02_BJPBZS = tmp.KHJEXFD;
                tmp = dataDay.Where(t => t.TypesName == item.BZCLZL_03_CPLX).FirstOrDefault();
                if (tmp == null || string.IsNullOrEmpty(tmp.UnitPrice))
                {
                    MessageBox.Show($"指标数据没有品种名称为【{item.BZCLZL_03_CPLX}】的【单价】数据，无法生成！");
                    return;
                }
                var tmp2 = cxyb.Where(t => t.GH == item.GH && t.CPMC == item.BZCLZL_03_CPLX).GroupBy(t => new { t.GH, t.CPMC }).Select(t => new DataBase3CX_General_CXYB { GH = t.Key.GH, CPMC = t.Key.CPMC, YJP = t.Sum(x => decimal.TryParse(x.YJP, out decimal d1) ? d1 : 0M).ToString(), KYL = t.Sum(x => decimal.TryParse(x.KYL, out decimal d2) ? d2 : 0M).ToString(), YJL = (t.Sum(x => decimal.TryParse(x.YJP, out decimal d1) ? d1 : 0M) / t.Sum(x => decimal.TryParse(x.KYL, out decimal d2) ? d2 : 0M)).ToString() }).FirstOrDefault();
                item.BZJPKQ_01_CPJPJJBZ = tmp.UnitPrice;
                if (tmp == null || string.IsNullOrEmpty(tmp.GRKHZB2))
                {
                    MessageBox.Show($"指标数据没有品种名称为【{item.BZCLZL_03_CPLX}】的【个人考核指标2】数据，无法生成！");
                    return;
                }
                item.BZCLZL_05_BZCXLL = tmp.GRKHZB2;
                if (tmp == null || string.IsNullOrEmpty(tmp.GRKHZB1))
                {
                    MessageBox.Show($"指标数据没有品种名称为【{item.BZCLZL_03_CPLX}】的【个人考核指标1】数据，无法生成！");
                    return;
                }
                item.BZCLZL_04_BZJPL = tmp.GRKHZB1;

                if (tmp2 == null || string.IsNullOrEmpty(tmp2.YJL))
                {
                    MessageBox.Show($"成型月报没有，工号【{item.GH}】,品种名称【{item.BZCLZL_03_CPLX}】的【一级率】数据，无法生成！");
                    return;
                }
                item.BZCLZL_07_SJCXLL = ((decimal.TryParse(tmp2.YJP, out decimal d_yjl) ? d_yjl : 0M) / (decimal.TryParse(tmp2.KYL, out decimal d_kyl) ? d_kyl : 0M)).ToString();

                var tmp3 = pmc.Where(t => t.PZ == item.BZCLZL_03_CPLX).FirstOrDefault();
                if (tmp3 == null || string.IsNullOrEmpty(tmp3.JHJPS))
                {
                    MessageBox.Show($"PMC调度高压计件交坯没有,品种名称【{item.BZCLZL_03_CPLX}】，的【计划交坯数】数据，无法生成！");
                    return;
                }
                item.BZZJQK_04_JHJPS = ((decimal.TryParse(tmp3.JHJPS, out decimal d_jhjps) ? d_jhjps : 0M) / (bzf.Count)).ToString();

                item.BZZJQK_03_SJJPS = item.JPS;

                list.Add(item);
            }

            var tmplist = list.DistinctBy(t => t.GH).ToList();
            foreach (var item in list)
            {
                decimal.TryParse(item.JPS, out decimal s);
                decimal.TryParse(item.MXS1, out decimal x);
                decimal.TryParse(item.MXS2, out decimal y);
                decimal.TryParse(item.MXS3, out decimal z);
                decimal.TryParse(item.MXS4, out decimal aa);
                decimal.TryParse(item.ZJCS1, out decimal t);
                decimal.TryParse(item.ZJCS2, out decimal u);
                decimal.TryParse(item.ZJCS3, out decimal v);
                decimal.TryParse(item.ZJCS4, out decimal w);

                if (s == 0M)
                {
                    item.BZCLZL_06_SJJPL = 0.ToString();
                }
                else
                {
                    item.BZCLZL_06_SJJPL = (s / (x * t + y * u + z * v + aa * w)).ToString();
                }
            }
            foreach (var item in tmplist)
            {
                var list2 = list.Where(t => t.GH == item.GH).ToList();
                decimal r = 0M;
                foreach (var item1 in list2)
                {
                    r += (decimal.TryParse(item1.BZJPKQ_01_CPJPJJBZ, out decimal d1) ? d1 : 0m) * (decimal.TryParse(item1.BZJPQK_03_YYJWJP, out decimal d2) ? d2 : 0M);
                }
                decimal i = 0M;
                foreach (var item1 in list2)
                {
                    i += ((decimal.TryParse(item1.BZCLZL_06_SJJPL, out decimal m) ? m : 0M) * (decimal.TryParse(item1.BZCLZL_07_SJCXLL, out decimal n) ? n : 0M)) / ((decimal.TryParse(item1.BZCLZL_04_BZJPL, out decimal k) ? k : 0M) * (decimal.TryParse(item1.BZCLZL_05_BZCXLL, out decimal l) ? l : 0M));
                }
                decimal gg = 0M;
                decimal ff = 0M;
                foreach (var item1 in list2)
                {
                    gg += (decimal.TryParse(item1.BZZJQK_04_JHJPS, out decimal d1) ? d1 : 0M);
                    ff += (decimal.TryParse(item1.BZZJQK_03_SJJPS, out decimal d2) ? d2 : 0M);
                }
                foreach (var item1 in list2)
                {
                    item1.BZJPQK_04_YJPKKJE = r.ToString();
                    item1.BZCLZL_02_CPZHZLKPXS = (i / list2.Count).ToString();
                    var dmonth = dataMonth.Where(t => !string.IsNullOrEmpty(t.MoneyBCBZ)).FirstOrDefault();
                    if (dmonth == null)
                    {
                        MessageBox.Show("基础数据没有【变产补助】，无法计算，请检查或联系管理员！");
                        return;
                    }
                    if (i / list2.Count >= 1M)
                    {
                        item1.BZCLZL_01_ZLJJSX2000 = dmonth.MoneyBCBZ;
                    }
                    else
                    {
                        item1.BZCLZL_01_ZLJJSX2000 = ((decimal.TryParse(dmonth.MoneyBCBZ, out decimal d1) ? d1 : 0M) * (i / list2.Count)).ToString();
                    }
                    if (gg == 0M)
                    {
                        item1.BZZJQK_02_JHWCL = 0.ToString();
                    }
                    else
                    {
                        item1.BZZJQK_02_JHWCL = (ff / gg).ToString();
                    }

                    dmonth = dataMonth.Where(t => !string.IsNullOrEmpty(t.MoneyKH)).FirstOrDefault();
                    if (dmonth == null)
                    {
                        MessageBox.Show("基础数据没有【考核金额】，无法计算，请检查或联系管理员！");
                        return;
                    }
                    decimal.TryParse(item.BZZJQK_02_JHWCL, out decimal ee);
                    if (ee >= 1)
                    {
                        item1.BZZJQK_01_ZJKHJESX1000 = dmonth.MoneyKH;
                    }
                    else
                    {
                        item1.BZZJQK_01_ZJKHJESX1000 = ((decimal.TryParse(dmonth.MoneyKH, out decimal d1) ? d1 : 0m) * ee).ToString();
                    }
                    item1.ZJE = ((decimal.TryParse(item1.BZZJQK_01_ZJKHJESX1000, out decimal dx1) ? dx1 : 0M) + (decimal.TryParse(item1.BZCLZL_01_ZLJJSX2000, out decimal dx2) ? dx2 : 0M)).ToString();
                }
            }
            if (new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().Add(list) > 0)
            {
                btnSearch.PerformClick();
            }
            else
            {
                MessageBox.Show("生成失败，请检查操作和网络，或联系管理员！");
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var oldList = new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => t.GH).ThenBy(t => t.BZCLZL_03_CPLX).ToList();
            string[] header = "创建日期$创建人$年$月$序号$工号$总金额$注浆考核金额$计划完成率$实际交坯数$计划交坯数$质量奖金$产品综合质量考评系数$产品类型$标准交坯率(%)$标准成型良率(%)$实际交坯率(%)$实际成型良率(%)$产品检坯计件标准（元/件）$班检坯标准数（个）$月应检未检坯（个）$月检坯扣款金额（元）$交坯数$注浆次数1$注浆次数2$注浆次数3$注浆次数4$模型数1$模型数2$模型数3$模型数4".Split('$');
            dgv.DataSource = oldList;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
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
                List<DataBase3CX_GYMJ_081_ZLWCHZB> list = dgv.DataSource as List<DataBase3CX_GYMJ_081_ZLWCHZB>;
                if (new ExcelHelper<DataBase3CX_GYMJ_081_ZLWCHZB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型高压面具——质量汇总表.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void Frm3CX_GYMJ_081_ZLWCHZB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }
    }
}