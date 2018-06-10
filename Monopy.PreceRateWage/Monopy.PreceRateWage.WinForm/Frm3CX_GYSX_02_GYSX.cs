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
    public partial class Frm3CX_GYSX_02_GYSX : Office2007Form
    {
        public Frm3CX_GYSX_02_GYSX()
        {
            InitializeComponent();
        }

        private void btnBulid_Click(object sender, EventArgs e)
        {
            var oldList = new BaseDal<DataBase3CX_GYSX_02_GYSX>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (oldList.Count > 0)
            {
                if (MessageBox.Show("数据已经存在，删除后，再生成？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach (var item in oldList)
                    {
                        new BaseDal<DataBase3CX_GYSX_02_GYSX>().Delete(item);
                    }
                }
                else
                {
                    return;
                }
            }

            var dataDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "高压水箱").ToList();
            if (dataDay == null || dataDay.Count == 0)
            {
                MessageBox.Show("没有指标基础数据，请联系管理员");
                return;
            }
            var bz = new BaseDal<BaseHeadcount>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory == "三厂" && t.DeptName.Contains("成型") && t.Name.Contains("水箱")).ToList();
            if (bz == null || bz.Count == 0)
            {
                MessageBox.Show("没有编制，请联系管理员");
                return;
            }
            //var dataMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "高压水箱").ToList();
            //if (dataMonth == null || dataMonth.Count == 0)
            //{
            //    MessageBox.Show("没有月基础数据，请联系管理员");
                return;
            //}
            var bzf = new BaseDal<DataBase3CX_GYMJ_07_BZFGY>().GetList(t => t.TheType.Contains("水箱") && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (bzf == null || bzf.Count == 0)
            {
                MessageBox.Show("没有导入【班长费】，请先导入，才能生成！");
                return;
            }
            var cxyb = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.CPMC.Contains("高压箱")).ToList();
            if (cxyb == null || cxyb.Count == 0)
            {
                MessageBox.Show("没有导入【成型月报】或【成型月报】中没有【产品名称】包含【高压箱】的产品，请先导入，才能生成！");
                return;
            }

            var list = new List<DataBase3CX_GYSX_02_GYSX>();
            for (int i = 0; i < bzf.Count; i++)
            {
                var item = new DataBase3CX_GYSX_02_GYSX { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = (i + 1).ToString(), GH1 = bzf[i].GH, GH2 = bzf[i].GH };
                var kyl = cxyb.Where(t => t.GH == item.GH1).ToList().Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M);
                item.KYL = kyl.ToString();
                var yjp = cxyb.Where(t => t.GH == item.GH1).ToList().Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M);
                item.YJP = yjp.ToString();
                if (yjp == 0M)
                {
                    item.YJL = 0.ToString();
                }
                else
                {
                    item.YJL = (yjp / kyl).ToString();
                }
                decimal.TryParse(item.YJL, out decimal d3);
                var tmp = dataDay.Where(t => !string.IsNullOrEmpty(t.BZKHZB)).FirstOrDefault();
                if (tmp == null)
                {
                    MessageBox.Show("指标基础数据中没有【班组考核指标】，请联系管理员");
                    return;
                }
                decimal.TryParse(tmp.BZKHZB, out decimal e3);

                item.MBYJL = e3.ToString();
                decimal.TryParse(tmp.BZJSB, out decimal d1);
                decimal.TryParse(tmp.BZJLDJ, out decimal d5);
                if (d3 <= e3)
                {
                    item.ZLSFDJ = 0.ToString();
                }
                else
                {
                    item.ZLSFDJ = ((d3 - e3) / d1 * d5).ToString();
                }
                decimal.TryParse(item.ZLSFDJ, out decimal f3);
                item.DJ = tmp.UnitPrice;
                decimal.TryParse(item.DJ, out decimal g3);
                var h3 = yjp * (f3 + g3);
                item.GZ = h3.ToString();
                item.DY = bz.FirstOrDefault().UserCount;
                decimal.TryParse(item.DY, out decimal i3);
                item.DRGZ = (h3 / (i3 / bzf.Count)).ToString();
                list.Add(item);
            }

            if (new BaseDal<DataBase3CX_GYSX_02_GYSX>().Add(list) > 0)
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
            var oldList = new BaseDal<DataBase3CX_GYSX_02_GYSX>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => t.GH1).ThenBy(t => t.No).ToList();
            string[] header = "创建日期$创建人$年$月$序号$工号1$工号$开窑量$一级品$一级率$目标一级率$质量上浮单价$单价$工资$定员$单人工资".Split('$');
            dgv.DataSource = oldList;
            for (int i = 0; i < 7; i++)
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
                List<DataBase3CX_GYSX_02_GYSX> list = dgv.DataSource as List<DataBase3CX_GYSX_02_GYSX>;
                if (new ExcelHelper<DataBase3CX_GYSX_02_GYSX>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型高压水箱——质量汇总表.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void Frm3CX_GYSX_02_GYSX_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }
    }
}