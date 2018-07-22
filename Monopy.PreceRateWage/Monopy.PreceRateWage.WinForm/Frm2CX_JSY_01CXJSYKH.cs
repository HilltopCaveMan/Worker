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
    public partial class Frm2CX_JSY_01CXJSYKH : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$工段整体质量【目标】$工段整体质量【实际】$工段差手质量【目标】$工段差手质量【实际】$工段产量【目标】$工段产量【实际】$工段整体质量【差率】$工段整体质量【考核金额】$工段差手质量【差率】$工段差手质量【考核金额】$工段产量【差率】$工段产量【考核金额】$合计$实出勤$应出勤".Split('$');
        private string[] header1 = "创建日期$创建人$年$月$序号$人员编码$姓名$工号$上月开窑量$本月开窑量$上月一加品$本月一级品$目标值$实际完成".Split('$');

        public Frm2CX_JSY_01CXJSYKH()
        {
            InitializeComponent();
        }

        private void Frm2CX_JSY_01CXJSYKH_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2CX_JSY_01CXJSYKH_02KH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < header.Length; i++)
            {
                if (i < 6)
                {
                    dgv.Columns[i].Visible = false;
                }
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void RefDgv1(DateTime selectTime)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < header1.Length; i++)
            {
                if (i < 6)
                {
                    dgv.Columns[i].Visible = false;
                }
                dgv.Columns[i + 1].HeaderText = header1[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        private void btnSearchCS_Click(object sender, EventArgs e)
        {
            RefDgv1(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——技术员——成型技术员考核.xlsx");
        }

        private bool Recount(List<DataBase2CX_JSY_01CXJSYKH_02KH> list)
        {
            try
            {
                //System.Globalization.CultureInfo Culinfo = System.Globalization.CultureInfo.CreateSpecificCulture("zh-Cn");
                var dataDay = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "成型车间" && t.PostName == "技术员").ToList();

                foreach (var item in list)
                {
                    //工段整体质量
                    decimal.TryParse(item.GDZTZL_MB, out decimal d4);
                    decimal.TryParse(item.GDZTZL_SJWC, out decimal e4);
                    decimal j4 = d4 - e4;
                    item.GDZTZL_MB = d4.ToString();
                    item.GDZTZL_SJWC = e4.ToString();
                    item.GDZTZL_CL = (j4).ToString();

                    //工段差手质量
                    decimal.TryParse(item.GDCSZL_MB, out decimal f4);
                    decimal.TryParse(item.GDCSZL_SJWC, out decimal g4);
                    decimal l4 = f4 - g4;
                    item.GDCSZL_MB = f4.ToString();
                    item.GDCSZL_SJWC = g4.ToString();
                    item.GDCSZL_CL = (l4).ToString();

                    //工段工段产量
                    decimal.TryParse(item.GDGDCL_MB, out decimal h4);
                    decimal.TryParse(item.GDGDCL_SJWC, out decimal i4);
                    decimal n4 = i4 - h4;
                    item.GDGDCL_SJWC = (i4).ToString();
                    item.GDGDCL_MB = (h4).ToString();
                    item.GDGDCL_CL = (n4).ToString();

                    var dataDayZTZL = dataDay.Where(t => t.Classification == "工段整体质量").FirstOrDefault();
                    var dataDayCSZL = dataDay.Where(t => t.Classification == "工段差手质量").FirstOrDefault();
                    var dataDayZTCL = dataDay.Where(t => t.Classification == "工段整体产量").FirstOrDefault();
                    if (dataDayZTZL == null || dataDayCSZL == null || dataDayZTCL == null)
                    {
                        MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    //工段整体质量
                    if (e4 >= d4 * 2)
                    {
                        item.GDZTZL_KHJE = "0";
                    }
                    else if (j4 >= 0)
                    {
                        item.GDZTZL_KHJE = dataDayZTZL.UnitPrice;
                    }
                    else
                    {
                        decimal.TryParse(dataDayZTZL.UnitPrice, out decimal UnitPrice);
                        item.GDZTZL_KHJE = (UnitPrice - UnitPrice * (e4 - d4) / d4).ToString();
                    }
                    decimal.TryParse(item.GDZTZL_KHJE, out decimal ztzlkh);
                    decimal.TryParse(dataDayZTZL.KHJESFD, out decimal ZTZLsfd);
                    decimal.TryParse(dataDayZTZL.KHJEXFD, out decimal ZTZLxfd);
                    if (ztzlkh > ZTZLsfd)
                    {
                        item.GDZTZL_KHJE = ZTZLsfd.ToString();
                    }
                    if (ztzlkh < ZTZLxfd)
                    {
                        item.GDZTZL_KHJE = ZTZLxfd.ToString();
                    }

                    //工段差手质量

                    if (g4 >= f4 * 2)
                    {
                        item.GDCSZL_KHJE = "0";
                    }
                    else if (l4 >= 0)
                    {
                        item.GDCSZL_KHJE = dataDayCSZL.UnitPrice;
                    }
                    else
                    {
                        decimal.TryParse(dataDayCSZL.UnitPrice, out decimal UnitPrice);
                        item.GDCSZL_KHJE = (UnitPrice - UnitPrice * (g4 - f4) / f4).ToString();
                    }

                    decimal.TryParse(item.GDCSZL_KHJE, out decimal cszlkh);
                    decimal.TryParse(dataDayCSZL.KHJESFD, out decimal cszlsfd);
                    decimal.TryParse(dataDayCSZL.KHJEXFD, out decimal cszlxfd);
                    if (cszlkh > cszlsfd)
                    {
                        item.GDCSZL_KHJE = cszlsfd.ToString();
                    }
                    if (cszlkh < cszlxfd)
                    {
                        item.GDCSZL_KHJE = cszlxfd.ToString();
                    }

                    //工段整体产量

                    decimal.TryParse(dataDayZTCL.UnitPrice, out decimal ztclUnitPrice);
                    decimal.TryParse(dataDayZTCL.GRJSB1, out decimal grjsb);
                    decimal.TryParse(dataDayZTCL.GRJLDJ1, out decimal grjldj);
                    item.GDGDCL_KHJE = (ztclUnitPrice + n4 / grjsb * grjldj).ToString();

                    decimal.TryParse(item.GDGDCL_KHJE, out decimal ztclkh);
                    decimal.TryParse(dataDayZTCL.KHJESFD, out decimal ZTCLsfd);
                    decimal.TryParse(dataDayZTCL.KHJEXFD, out decimal ZTCLxfd);
                    if (ztclkh > ZTCLsfd)
                    {
                        item.GDGDCL_KHJE = ZTCLsfd.ToString();
                    }
                    if (ztclkh < ZTCLxfd)
                    {
                        item.GDGDCL_KHJE = ZTCLxfd.ToString();
                    }

                    List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("二厂") && t.Dept.Contains("成型") && t.Position == "技术员").ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

                    var data = datas.Where(x => x.UserCode == item.UserCode && x.UserName == item.UserName).FirstOrDefault();
                    if (data == null)
                    {
                        item.SCQ = "0";
                        item.YCQ = "0";
                        item.HJ = "0";
                        MessageBox.Show("没有【" + item.UserName + "】的出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        item.SCQ = data.DayScq;
                        item.YCQ = data.DayYcq;
                        decimal.TryParse(item.GDZTZL_KHJE, out decimal ZTZL_KHJE);
                        decimal.TryParse(item.GDCSZL_KHJE, out decimal CSZL_KHJE);
                        decimal.TryParse(item.GDGDCL_KHJE, out decimal GDCL_KHJE);
                        decimal.TryParse(item.SCQ, out decimal scq);
                        decimal.TryParse(item.YCQ, out decimal ycq);
                        item.HJ = ((ZTZL_KHJE + CSZL_KHJE + GDCL_KHJE) / ycq * scq).ToString();
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Import(string fileName)
        {
            var list = new ExcelHelper<DataBase2CX_JSY_01CXJSYKH_01CSZL>().ReadExcel(fileName, 3, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            var listCSZL = new List<DataBase2CX_JSY_01CXJSYKH_01CSZL>();
            var listKH = new List<DataBase2CX_JSY_01CXJSYKH_02KH>();

            var tmp = list.Where(t => t.UserCode.Contains("差手质量")).FirstOrDefault();
            if (tmp == null)
            {
                MessageBox.Show("Excel文件样式与模板不一致。下方的【差手质量】有一整行和考核表隔开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int flag = Convert.ToInt32(tmp.No);
            for (int i = 0; i < flag - 1; i++)
            {
                var item = list[i];
                var kh = new DataBase2CX_JSY_01CXJSYKH_02KH { Id = item.Id, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), GD = item.UserCode, UserCode = item.UserName, UserName = item.GH, GDZTZL_MB = item.SYKYL, GDZTZL_SJWC = item.BYKYL, GDCSZL_MB = item.SYYJP, GDCSZL_SJWC = item.BYYJP, GDGDCL_MB = item.MBZ, GDGDCL_SJWC = item.SJWC, No = item.No, TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
                listKH.Add(kh);
            }
            for (int i = flag + 1; i < list.Count; i++)
            {
                var item = list[i];
                if (string.IsNullOrEmpty(item.GH) && string.IsNullOrEmpty(item.UserCode))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                {
                    MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                item.CreateTime = Program.NowTime;
                item.CreateUser = Program.User.ToString();
                item.TheYear = dtp.Value.Year;
                item.TheMonth = dtp.Value.Month;
                listCSZL.Add(item);
            }
            if (Recount(listKH) && new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().Add(listCSZL) > 0 && new BaseDal<DataBase2CX_JSY_01CXJSYKH_02KH>().Add(listKH) > 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Enabled = true;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Import(openFileDlg.FileName);
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
                var list = new BaseDal<DataBase2CX_JSY_01CXJSYKH_02KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
                if (new ExcelHelper<DataBase2CX_JSY_01CXJSYKH_02KH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——技术员——成型技术员考核.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnExportExcel2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var list1 = new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();

                if (new ExcelHelper<DataBase2CX_JSY_01CXJSYKH_01CSZL>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——技术员——成型技术员差手质量.xlsx", saveFileDlg.FileName, list1, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var list1 = new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();

            foreach (var item in list1)
            {
                new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().Edit(item);
            }

            var list = new BaseDal<DataBase2CX_JSY_01CXJSYKH_02KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
            if (Recount(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase2CX_JSY_01CXJSYKH_02KH>().Edit(item);
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("操作失败，请检查网络和操作！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().ExecuteSqlCommand("delete from DataBase2CX_JSY_01CXJSYKH_01CSZL where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);
                new BaseDal<DataBase2CX_JSY_01CXJSYKH_01CSZL>().ExecuteSqlCommand("delete from DataBase2CX_JSY_01CXJSYKH_02KH where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);

                btnSearch.PerformClick();
            }
        }

        private void Frm2CX_JSY_01CXJSYKH_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm2CX_JSY_01CXJSYKH_DragDrop(object sender, DragEventArgs e)
        {
            var tmp = ((Array)e.Data.GetData(DataFormats.FileDrop));
            if (tmp.Length != 1)
            {
                MessageBox.Show("一次只允许导入一个文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var path = tmp.GetValue(0).ToString();
            if (StringHelper.IsDir(path))
            {
                MessageBox.Show("请选择一个文件而不是文件夹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var extension = FileHelper.GetExtension(path);
            if (!extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show($"请选择一个Excel文件，当前选择的文件扩展名为：【{extension}】。{Environment.NewLine}为了所有用户的兼容性一致，请使用新格式（扩展名为【xlsx】）！{ Environment.NewLine }旧的xls格式，请用excle2007以上版本另存……", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Import(path);
        }
    }
}