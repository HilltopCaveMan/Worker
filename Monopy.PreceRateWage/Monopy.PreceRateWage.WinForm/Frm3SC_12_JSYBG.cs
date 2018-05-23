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
    public partial class Frm3SC_12_JSYBG : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$人员编码$姓名$开窑量$装窑缺陷数$标污缺陷$开窑缺陷$缺陷率$目标缺陷率$金额".Split('$');

        public Frm3SC_12_JSYBG()
        {
            InitializeComponent();
        }

        private void Frm3SC_12_JSYBG_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_12_JSYBG>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_12_JSYBG(datas));
            datas.Insert(1, MyDal.GetTotalDataBase3SC_12_JSYBG(dtp.Value));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgv.Rows[1].Frozen = true;
            dgv.Rows[1].DefaultCellStyle.BackColor = Color.Green;
            dgv.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Lime;

            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——技术员报工.xlsx");
        }

        private bool Recount(List<DataBase3SC_12_JSYBG> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算");
                return false;
            }

            #region 核对数据

            var old = MyDal.GetTotalDataBase3SC_12_JSYBG(dtp.Value);
            var now = MyDal.GetTotalDataBase3SC_12_JSYBG(list);
            if (old.KYL != now.KYL || old.ZYQXS != now.ZYQXS || old.BWQX != now.BWQX || old.KYQX != now.KYQX)
            {
                MessageBox.Show($"【技术员考核】的数据与导入的数据不一致：开窑量：导入数据：{now.KYL},技术员考核：{old.KYL},装窑缺陷数：导入数据：{now.ZYQXS},技术员考核：{old.ZYQXS},标污缺陷：导入数据：{now.BWQX},技术员考核：{old.BWQX},开窑缺陷：导入数据：{now.KYQX},技术员考核：{old.KYQX}");
                return false;
            }

            #endregion 核对数据

            try
            {
                var dataDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "技术员");
                decimal.TryParse(dataDay.GRKHZB1, out decimal i);
                decimal.TryParse(dataDay.GRJSB1, out decimal d01);
                decimal.TryParse(dataDay.GRFKDJ1, out decimal d50);

                var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "技术员");
                decimal.TryParse(dataMonth.MoneyKH, out decimal d1500);
                foreach (var item in list)
                {
                    decimal.TryParse(item.KYL, out decimal d);
                    decimal.TryParse(item.ZYQXS, out decimal e);
                    decimal.TryParse(item.BWQX, out decimal f);
                    decimal.TryParse(item.KYQX, out decimal g);
                    var h = (e + f + g) / d;

                    item.QXL = h.ToString();
                    item.MBQXL = i.ToString();
                    if (d <= 0)
                    {
                        item.JE = 0.ToString();
                    }
                    else
                    {
                        if (h <= i)
                        {
                            item.JE = d1500.ToString();
                        }
                        else
                        {
                            if (d1500 - (h - i) / d01 * d50 < 0)
                            {
                                item.JE = 0.ToString();
                            }
                            else
                            {
                                item.JE = (d1500 - (h - i) / d01 * d50).ToString();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                    List<DataBase3SC_12_JSYBG> list = new ExcelHelper<DataBase3SC_12_JSYBG>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (item.GD == item.UserCode && item.GD == item.UserName)
                        {
                            list.RemoveAt(i);
                            i--;
                            continue;
                        }
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                        if (item.UserName == null)
                        {
                            item.UserName = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;
                    }
                    if (Recount(list) && new BaseDal<DataBase3SC_12_JSYBG>().Add(list) > 0)
                    {
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
                List<DataBase3SC_12_JSYBG> list = dgv.DataSource as List<DataBase3SC_12_JSYBG>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3SC_12_JSYBG>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——烧成——技术员报工.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3SC_12_JSYBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_12_JSYBG DataBase3SC_12_JSYBG = new DataBase3SC_12_JSYBG() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3SC_12_JSYBG> frm = new FrmModify<DataBase3SC_12_JSYBG>(DataBase3SC_12_JSYBG, header, OptionType.Add, Text, 5, 5);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3SC_12_JSYBG> list = new BaseDal<DataBase3SC_12_JSYBG>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (Recount(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase3SC_12_JSYBG>().Edit(item);
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("重新计算失败，请检查网络和操作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_12_JSYBG DataBase3SC_12_JSYBG)
                {
                    if (DataBase3SC_12_JSYBG.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3SC_12_JSYBG> frm = new FrmModify<DataBase3SC_12_JSYBG>(DataBase3SC_12_JSYBG, header, OptionType.Modify, Text, 5, 5);
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
                var DataBase3SC_12_JSYBG = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_12_JSYBG;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_12_JSYBG != null)
                    {
                        if (DataBase3SC_12_JSYBG.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3SC_12_JSYBG>;
                                list.RemoveAt(0);
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3SC_12_JSYBG>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3SC_12_JSYBG> frm = new FrmModify<DataBase3SC_12_JSYBG>(DataBase3SC_12_JSYBG, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }
    }
}