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
    public partial class Frm1CX_DNG_01CXDNGKH : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$工段整体质量【目标】$工段整体质量【实际】$工段产品达成【目标】$工段产品达成【实际】$工段整体产量【目标】$工段整体产量【实际】$工段整体质量【差率】$工段整体质量【考核金额】$工段产品达成【差率】$工段产品达成【考核金额】$工段整体产量【差率】$工段整体产量【考核金额】$合计$实出勤$应出勤".Split('$');
        private string[] header1 = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$品种$成型计划合格品数$实际完成$达成数量".Split('$');

        public Frm1CX_DNG_01CXDNGKH()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1CX_DNG_01CXDNGKH_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询考核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbGD.Text, CmbUserCode.Text, CmbUserName.Text);
        }

        /// <summary>
        /// 查询产品达成率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCS_Click(object sender, EventArgs e)
        {
            RefDgv1(dtp.Value, CmbGD.Text, CmbUserCode.Text, CmbUserName.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——多能工——成型多能工考核.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 导出考核
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
                var list = new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
                if (new ExcelHelper<DataBase1CX_DNG_01CXDNGKH_02KH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——多能工——成型多能工考核.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 导出产品达成率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                var list1 = new BaseDal<DataBase1CX_DNG_01CXDNGKH_01DCL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();

                if (new ExcelHelper<DataBase1CX_DNG_01CXDNGKH_01DCL>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——多能工——成型多能工产品达成率.xlsx", saveFileDlg.FileName, list1, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;

            var list = new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
            if (Recount(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().Edit(item);
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Enabled = true;
                MessageBox.Show("操作失败，请检查网络和操作！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        /// <summary>
        /// 全部删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                new BaseDal<DataBase1CX_DNG_01CXDNGKH_01DCL>().ExecuteSqlCommand("delete from DataBase1CX_DNG_01CXDNGKH_01DCL where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);
                new BaseDal<DataBase1CX_DNG_01CXDNGKH_01DCL>().ExecuteSqlCommand("delete from DataBase1CX_DNG_01CXDNGKH_02KH where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);

                btnSearch.PerformClick();
            }
        }

        private void Frm1CX_DNG_01CXDNGKH_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm1CX_DNG_01CXDNGKH_DragDrop(object sender, DragEventArgs e)
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

        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().GetList().ToList();
            RefCmbGD(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }
        private void RefCmbGD(List<DataBase1CX_DNG_01CXDNGKH_02KH> list)
        {
            var listTmp = list.GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGD.DataSource = listTmp;
            CmbGD.DisplayMember = "GD";
            CmbGD.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase1CX_DNG_01CXDNGKH_02KH> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase1CX_DNG_01CXDNGKH_02KH> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gd, string userCode, string userName)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gd == "全部" ? true : t.GD == gd) && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
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

        private void RefDgv1(DateTime selectTime, string gd, string userCode, string userName)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1CX_DNG_01CXDNGKH_01DCL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gd == "全部" ? true : t.GD == gd) && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
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


        private bool Recount(List<DataBase1CX_DNG_01CXDNGKH_02KH> list)
        {
            try
            {
                System.Globalization.CultureInfo Culinfo = System.Globalization.CultureInfo.CreateSpecificCulture("zh-Cn");
                var dataDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G001" && t.WorkshopName == "成型车间" && t.PostName == "多能工");

                foreach (var item in list)
                {
                    //工段整体质量
                    decimal.TryParse(item.GDZTZL_MB, out decimal d4);
                    decimal.TryParse(item.GDZTZL_SJWC, out decimal e4);
                    decimal j4 = d4 - e4;
                    item.GDZTZL_MB = d4.ToString("P", Culinfo);
                    item.GDZTZL_SJWC = e4.ToString("P", Culinfo);
                    item.GDZTZL_CL = (j4).ToString("P", Culinfo);

                    //工段产品达成
                    decimal.TryParse(item.GDCPDC_MB, out decimal f4);
                    decimal.TryParse(item.GDCPDCL_SJWC, out decimal g4);
                    decimal l4 = g4 - f4;
                    item.GDCPDC_CL = (l4).ToString();

                    //工段整体产量
                    decimal.TryParse(item.GDZTCL_MB, out decimal h4);
                    decimal.TryParse(item.GDZTCL_SJWC, out decimal i4);
                    decimal n4 = i4 - h4;
                    item.GDZTCL_SJWC = (i4).ToString("P", Culinfo);
                    item.GDZTCL_MB = (h4).ToString("P", Culinfo);
                    item.GDZTCL_CL = (n4).ToString("P", Culinfo);

                    var dataDayZTZL = dataDay.Where(t => t.Classification == "工段整体质量").FirstOrDefault();
                    var dataDayCPDC = dataDay.Where(t => t.Classification == "工段产品达成").FirstOrDefault();
                    var dataDayZTCL = dataDay.Where(t => t.Classification == "工段整体产量").FirstOrDefault();
                    if (dataDayZTZL == null || dataDayCPDC == null || dataDayZTCL == null)
                    {
                        MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    //工段整体质量
                    if (j4 >= 0)
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

                    //工段产品达成

                    decimal.TryParse(dataDayCPDC.UnitPrice, out decimal CPDCUnitPrice);
                    item.GDCPDC_KHJE = (g4 / f4 * CPDCUnitPrice).ToString();

                    decimal.TryParse(item.GDCPDC_KHJE, out decimal cpdc);
                    decimal.TryParse(dataDayCPDC.KHJESFD, out decimal CPDCsfd);
                    decimal.TryParse(dataDayCPDC.KHJEXFD, out decimal CPDCxfd);
                    if (cpdc > CPDCsfd)
                    {
                        item.GDCPDC_KHJE = CPDCsfd.ToString();
                    }
                    if (cpdc < CPDCxfd)
                    {
                        item.GDCPDC_KHJE = CPDCxfd.ToString();
                    }

                    //工段整体产量
                    if (n4 >= 0)
                    {
                        item.GDZTCL_KHJE = dataDayZTCL.UnitPrice;
                    }
                    else
                    {
                        decimal.TryParse(dataDayZTCL.UnitPrice, out decimal UnitPrice);
                        decimal.TryParse(dataDayZTCL.GRJSB1, out decimal grjsb);
                        decimal.TryParse(dataDayZTCL.GRJLDJ1, out decimal grjldj);
                        item.GDZTCL_KHJE = (UnitPrice + n4 / grjsb * grjldj).ToString();
                    }
                    decimal.TryParse(item.GDZTCL_KHJE, out decimal ztclkh);
                    decimal.TryParse(dataDayZTCL.KHJESFD, out decimal ZTCLsfd);
                    decimal.TryParse(dataDayZTCL.KHJEXFD, out decimal ZTCLxfd);
                    if (ztclkh > ZTCLsfd)
                    {
                        item.GDZTCL_KHJE = ZTCLsfd.ToString();
                    }
                    if (ztclkh < ZTCLxfd)
                    {
                        item.GDZTCL_KHJE = ZTCLxfd.ToString();
                    }

                    List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("一厂") && t.Dept.Contains("成型")).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

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
                        decimal.TryParse(item.GDCPDC_KHJE, out decimal CPDC_KHJE);
                        decimal.TryParse(item.GDZTCL_KHJE, out decimal ZTCL_KHJE);
                        decimal.TryParse(item.SCQ, out decimal scq);
                        decimal.TryParse(item.YCQ, out decimal ycq);
                        item.HJ = ((ZTZL_KHJE + CPDC_KHJE + ZTCL_KHJE) / ycq * scq).ToString();
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
            var list = new ExcelHelper<DataBase1CX_DNG_01CXDNGKH_02KH>().ReadExcel(fileName, 3, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            var listDCL = new List<DataBase1CX_DNG_01CXDNGKH_01DCL>();
            var listKH = new List<DataBase1CX_DNG_01CXDNGKH_02KH>();

            var tmp = list.Where(t => t.GD.Contains("工段产品达成率")).FirstOrDefault();
            if (tmp == null)
            {
                MessageBox.Show("Excel文件样式与模板不一致。下方的【工段产品达成率】有一整行和考核表隔开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int flag = Convert.ToInt32(tmp.No);
            for (int i = flag + 1; i < list.Count; i++)
            {
                var item = list[i];
                var dcl = new DataBase1CX_DNG_01CXDNGKH_01DCL { Id = item.Id, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), GD = item.GD, UserCode = item.UserCode, UserName = item.UserName, PZ = item.GDZTZL_MB, HGPS = item.GDZTZL_SJWC, SJWC = item.GDCPDC_MB, DCSJ = item.GDCPDCL_SJWC, No = item.No, TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
                listDCL.Add(dcl);
            }
            for (int i = 0; i < flag - 1; i++)
            {
                var item = list[i];
                if (string.IsNullOrEmpty(item.UserName) && string.IsNullOrEmpty(item.UserCode))
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
                listKH.Add(item);
            }
            if (Recount(listKH) && new BaseDal<DataBase1CX_DNG_01CXDNGKH_01DCL>().Add(listDCL) > 0 && new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().Add(listKH) > 0)
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

        #endregion
    }
}