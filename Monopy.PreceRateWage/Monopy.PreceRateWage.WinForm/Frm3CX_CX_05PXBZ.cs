using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_CX_05PXBZ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$所注产品$类别$第几月$金额$备注$金额$实出勤$应出勤$补助金额$补余额".Split('$');

        public Frm3CX_CX_05PXBZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbGD(List<DataBase3CX_CX_05PXBZ> list)
        {
            var listTmp = list.GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGD.DataSource = listTmp;
            CmbGD.DisplayMember = "GD";
            CmbGD.Text = string.Empty;
        }

        private void RefCmbPZ(List<DataBase3CX_CX_05PXBZ> list)
        {
            var listTmp = list.GroupBy(t => t.SZPZ).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbPZ.DataSource = listTmp;
            CmbPZ.DisplayMember = "SZPZ";
            CmbPZ.Text = string.Empty;
        }

        private void RefCmbUserCode(List<DataBase3CX_CX_05PXBZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase3CX_CX_05PXBZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void RefCmbLB(List<DataBase3CX_CX_05PXBZ> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbGD(list);
            RefCmbPZ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private void Frm3CX_CX_05PXBZ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gd, string pz, string userCode, string userName, string lb, bool isBye)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new List<DataBase3CX_CX_05PXBZ>();
            if (isBye)
            {
                datas = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GD.Contains(gd) && t.SZPZ.Contains(pz) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.LB.Contains(lb) && t.IsBYE == isBye).ToList().OrderBy(t => t.GD).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            else
            {
                datas = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GD.Contains(gd) && t.SZPZ.Contains(pz) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.LB.Contains(lb)).ToList().OrderBy(t => t.GD).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_05PXBZ(datas));

            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbGD.Text, CmbPZ.Text, CmbUserCode.Text, CmbUserName.Text, CmbLB.Text, checkBox1.Checked);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——入职表.xlsx");
        }

        private bool Recount(List<DataBase3CX_CX_05PXBZ> list, out List<DataBase3CX_CX_05PX_TZ> listTZ)
        {
            listTZ = new List<DataBase3CX_CX_05PX_TZ>();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var itemTmp = list.FirstOrDefault();
                var listHrCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == itemTmp.TheYear && t.TheMonth == itemTmp.TheMonth && t.Factory == "三厂" && t.Dept == "成型车间");
                if (listHrCQ == null || listHrCQ.Count() == 0)
                {
                    MessageBox.Show("没有出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "注修工" && t.Classification == "入职补助");

                //台账最大No
                var tmpNo = new BaseDal<DataBase3CX_CX_05PX_TZ>().GetList().ToList().OrderByDescending(t => int.TryParse(t.No, out int ino) ? ino : 0).FirstOrDefault();
                var no = 1;
                if (tmpNo != null)
                {
                    no = Convert.ToInt32(tmpNo.No) + 1;
                }

                foreach (var item in list)
                {
                    var itemHrCQ = listHrCQ.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                    if (itemHrCQ == null)
                    {
                        MessageBox.Show($"工号：{item.UserCode}，姓名：{item.UserName}，没有出勤数据。无法计算，有出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    decimal.TryParse(itemHrCQ.DayTotal, out decimal scq);
                    decimal.TryParse(itemHrCQ.DayYcq, out decimal ycq);

                    if (!item.IsBYE)
                    {
                        item.SCQ = scq.ToString();

                        item.YCQ = ycq.ToString();

                        var itemMonth = listMonth.Where(t => t.ProductType == item.LB && t.MonthData == item.DJY).FirstOrDefault();
                        if (itemMonth == null)
                        {
                            item.IsBYE = true;
                            continue;
                        }
                        item.IsBYE = false;
                        item.JE = itemMonth.MoneyRZBZ;
                        decimal.TryParse(item.JE, out decimal je);

                        //item.BZJE = (je * scq / ycq).ToString();
                        //20180203补助金额计算发生变化
                        if (ycq < 26)
                        {
                            if (scq < ycq)
                            {
                                item.BZJE = (je * scq / 26).ToString();
                            }
                            else
                            {
                                item.BZJE = je.ToString();
                            }
                        }
                        else
                        {
                            if (scq < ycq)
                            {
                                item.BZJE = (je * scq / 26).ToString();
                            }
                            else
                            {
                                item.BZJE = je.ToString();
                            }
                        }
                    }

                    //生成台账
                    if (!string.IsNullOrEmpty(item.BZJE))
                    {
                        var itemTZ = new DataBase3CX_CX_05PX_TZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), GD = item.GD, PZ = item.LB, UserCode = item.UserCode, UserName = item.UserName, Money = item.BZJE };
                        listTZ.Add(itemTZ);
                    }
                    no++;
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
            List<DataBase3CX_CX_05PXBZ> list = new ExcelHelper<DataBase3CX_CX_05PXBZ>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                {
                    MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }

                if (string.IsNullOrEmpty(list[i].GD) || int.TryParse(list[i].GD.Replace("段", ""), out int intTmp))
                {
                    MessageBox.Show("工段不能为空并且必须格式正确，请检查！(一段、二段、三段……或一、二、三……)" + (i + 3).ToString() + "行【" + list[i].GD + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                list[i].GD = list[i].GD.Replace("段", "") + "段";
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list, out List<DataBase3CX_CX_05PX_TZ> listTZ);
            if (new BaseDal<DataBase3CX_CX_05PXBZ>().Add(list) > 0)
            {
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase3CX_CX_05PX_TZ>().ExecuteSqlCommand("delete from DataBase3CX_CX_05PX_TZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);
                    //现有台账数据补全，主要是初始数据部分列缺失。
                    //var listOldTZ = new BaseDal<DataBase3CX_CX_05PX_TZ>().GetList();
                    //foreach (var item in listTZ)
                    //{
                    //    var itemOldTZ = listOldTZ.Where(t => t.GD == item.GD && t.XW == item.XW && t.UserCode == item.UserCode && t.BGCPMC == item.BGCPMC && t.LB == item.LB).FirstOrDefault();
                    //    if (itemOldTZ != null && (string.IsNullOrEmpty(itemOldTZ.YCPMC) || string.IsNullOrEmpty(itemOldTZ.BCSJ)))
                    //    {
                    //        itemOldTZ.YCPMC = item.YCPMC;
                    //        itemOldTZ.BCSJ = item.BCSJ;
                    //    }
                    //    new BaseDal<DataBase3CX_CX_05PX_TZ>().Edit(itemOldTZ);
                    //}
                    if (new BaseDal<DataBase3CX_CX_05PX_TZ>().Add(listTZ) <= 0)
                    {
                        MessageBox.Show("台账保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            InitUI();
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
                List<DataBase3CX_CX_05PXBZ> list = dgv.DataSource as List<DataBase3CX_CX_05PXBZ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3CX_CX_05PXBZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型——入职表.xlsx", saveFileDlg.FileName, list, 2, 6, 1, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_CX_05PXBZ> list = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list, out List<DataBase3CX_CX_05PX_TZ> listTZ);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_CX_05PXBZ>().Edit(item);
            }
            if (boolOK)
            {
                //台账处理
                //先删除台账中这个月的旧数据。
                new BaseDal<DataBase3CX_CX_05PX_TZ>().ExecuteSqlCommand("delete from DataBase3CX_CX_05PX_TZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);
                //现有台账数据补全，主要是初始数据部分列缺失。
                //var listOldTZ = new BaseDal<DataBase3CX_CX_05PX_TZ>().GetList();
                //foreach (var item in listTZ)
                //{
                //    var itemOldTZ = listOldTZ.Where(t => t.GD == item.GD && t.XW == item.XW && t.UserCode == item.UserCode && t.BGCPMC == item.BGCPMC && t.LB == item.LB).FirstOrDefault();
                //    if (itemOldTZ != null && (string.IsNullOrEmpty(itemOldTZ.YCPMC) || string.IsNullOrEmpty(itemOldTZ.BCSJ)))
                //    {
                //        itemOldTZ.YCPMC = item.YCPMC;
                //        itemOldTZ.BCSJ = item.BCSJ;
                //    }
                //    new BaseDal<DataBase3CX_CX_05PX_TZ>().Edit(itemOldTZ);
                //}
                if (new BaseDal<DataBase3CX_CX_05PX_TZ>().Add(listTZ) <= 0)
                {
                    MessageBox.Show("台账保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            Enabled = true;
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3CX_CX_05PXBZ DataBase3CX_CX_05PXBZ)
                {
                    if (DataBase3CX_CX_05PXBZ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3CX_CX_05PXBZ> frm = new FrmModify<DataBase3CX_CX_05PXBZ>(DataBase3CX_CX_05PXBZ, header, OptionType.Modify, Text, 5, 5);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUI();
                        btnRecount.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3CX_CX_05PXBZ = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_05PXBZ;
                if (DataBase3CX_CX_05PXBZ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_CX_05PXBZ != null)
                    {
                        FrmModify<DataBase3CX_CX_05PXBZ> frm = new FrmModify<DataBase3CX_CX_05PXBZ>(DataBase3CX_CX_05PXBZ, header, OptionType.Delete, Text, 5, 1);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnRecount.PerformClick();
                        }
                    }
                }
            }
        }

        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase3CX_CX_05PXBZ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        continue;
                    }
                    new BaseDal<DataBase3CX_CX_05PXBZ>().Delete(item);
                }
                btnRecount.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }

        private void Frm3CX_CX_05PXBZ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_CX_05PXBZ_DragDrop(object sender, DragEventArgs e)
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

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitUI();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var list = new ExcelHelper<DataBase3CX_CX_05PXBZ>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                var listOld = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.IsBYE && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                foreach (var item in list)
                {
                    var itemOld = listOld.Where(t => t.UserCode == item.UserCode && t.UserName == item.UserName && t.GD == item.GD && t.SZPZ == item.SZPZ && t.LB == item.LB).FirstOrDefault();
                    if (itemOld == null)
                    {
                        MessageBox.Show($"工号：{item.UserCode},姓名{item.UserName}，所注品种：{item.SZPZ},补余额中存在，但是原始数据中不存在，请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        itemOld.JE = item.JE;
                        itemOld.BZJE = item.BZJE;
                        new BaseDal<DataBase3CX_CX_05PXBZ>().Edit(itemOld);
                    }
                }
                btnRecount.PerformClick();
            }
        }

        private void BtnYZ_Click(object sender, EventArgs e)
        {
            //验证金额
            var list = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && !t.IsBYE);
            bool IsOk = true;
            foreach (var item in list)
            {
                decimal.TryParse(item.JE_NotUsed, out decimal je_cj);
                decimal.TryParse(item.JE, out decimal je);
                if (Math.Abs(je_cj - je) > 0M)
                {
                    IsOk = false;
                    MessageBox.Show($"车间金额和计算金额不一致：工段：{item.GD}，人员编码：{item.UserCode}，姓名：{item.UserName}，车间提报金额：【{item.JE_NotUsed}】，计算金额：【{item.JE}】，不一致！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //与台账验证。
            list = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            foreach (var item in list)
            {
                DateTime dateTime = new DateTime(dtp.Value.Year, dtp.Value.Month, 1);
                var listTZ = new BaseDal<DataBase3CX_CX_05PX_TZ>().GetList(t => t.UserCode == item.UserCode && t.TimeBZ < dateTime);
                int.TryParse(item.DJY, out int djy);

                if (djy - 1 != listTZ.Count())
                {
                    IsOk = false;
                    MessageBox.Show($"补助月份不对：工段：{item.GD}，人员编码：{item.UserCode}，姓名：{item.UserName}，提报补助月份：{item.DJY}，台账中只有：{listTZ.Count()}个月！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (IsOk)
            {
                MessageBox.Show("验证通过", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("至少有一条数据验收失败！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}