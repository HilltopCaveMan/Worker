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
    public partial class Frm3CX_CX_12SFBZ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$师傅人员编码$师傅注修工号$师傅所在工段$师傅姓名$徒弟注修工号$徒弟所在工段$徒弟姓名$补贴月份（第几个月）$补贴标准（%）$备注$徒弟计件$师傅补助".Split('$');

        public Frm3CX_CX_12SFBZ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbTdGH(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.TDGH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbTdGH.DataSource = listTmp;
            CmbTdGH.DisplayMember = "TDGH";
            CmbTdGH.Text = string.Empty;
        }

        private void RefCmbTdGD(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.TDGD).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbTdGD.DataSource = listTmp;
            CmbTdGD.DisplayMember = "TDGD";
            CmbTdGD.Text = string.Empty;
        }

        private void RefCmbTdName(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.TDName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbTdName.DataSource = listTmp;
            CmbTdName.DisplayMember = "SFName";
            CmbTdName.Text = string.Empty;
        }

        private void RefCmbSfCode(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.SFCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbSFCode.DataSource = listTmp;
            CmbSFCode.DisplayMember = "SFCode";
            CmbSFCode.Text = string.Empty;
        }

        private void RefCmbSfGH(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.SFGH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbSFGH.DataSource = listTmp;
            CmbSFGH.DisplayMember = "SFGH";
            CmbSFGH.Text = string.Empty;
        }

        private void RefCmbSfGD(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.SFGD).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbSFGD.DataSource = listTmp;
            CmbSFGD.DisplayMember = "SFGD";
            CmbSFGD.Text = string.Empty;
        }

        private void RefCmbSfName(List<DataBase3CX_CX_12SFBZ> list)
        {
            var listTmp = list.GroupBy(t => t.SFName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbSFName.DataSource = listTmp;
            CmbSFName.DisplayMember = "SFName";
            CmbSFName.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase3CX_CX_12SFBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbTdGD(list);
            RefCmbTdGH(list);
            RefCmbTdName(list);
            RefCmbSfCode(list);
            RefCmbSfGD(list);
            RefCmbSfGH(list);
            RefCmbSfName(list);
        }

        private void Frm3CX_CX_12SFBZ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string tdGh, string tdGd, string tdName, string sfCode, string sfGh, string sfGd, string sfName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_12SFBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TDGH.Contains(tdGh) && t.TDGD.Contains(tdGd) && t.TDName.Contains(tdName) && t.SFCode.Contains(sfCode) && t.SFGH.Contains(sfGh) && t.SFGD.Contains(sfGd) && sfName.Contains(sfName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_12SFBZ(datas));
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbTdGH.Text, CmbTdGD.Text, CmbTdName.Text, CmbSFCode.Text, CmbSFGH.Text, CmbSFGD.Text, CmbSFName.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——师傅补助.xlsx");
        }

        private bool Recount(List<DataBase3CX_CX_12SFBZ> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "师傅补助");
                if (listMonth == null || listMonth.Count() == 0)
                {
                    MessageBox.Show("没有基础数据，请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listGr = new BaseDal<DataBase3CX_CX_03JJ_GR>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                if (listGr == null || listGr.Count() == 0)
                {
                    MessageBox.Show("没有个人工资，请先完成计件，再计算！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                foreach (var item in list)
                {
                    var tmpGr = listGr.Where(t => t.GH == item.TDGH).ToList();
                    if (tmpGr != null && tmpGr.Count() > 0)
                    {
                        item.TDJJ = tmpGr.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();
                    }
                    decimal.TryParse(item.TDJJ, out decimal tdjj);
                    var itemMonth = listMonth.Where(t => t.MonthData == item.BTYF_DJGY).FirstOrDefault();
                    if (itemMonth != null)
                    {
                        item.BTBZ = itemMonth.SFbackTD;
                    }
                    decimal.TryParse(item.BTBZ, out decimal btbz);
                    var l = tdjj * btbz;
                    item.SFBZ = l.ToString();
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
            List<DataBase3CX_CX_12SFBZ> list = new ExcelHelper<DataBase3CX_CX_12SFBZ>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].SFCode != null && list[i].SFCode.StartsWith("M"))
                {
                    if (!MyDal.IsUserCodeAndNameOK(list[i].SFCode, list[i].SFName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + list[i].SFCode + "】,姓名：【" + list[i].SFName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }

                    list[i].CreateUser = Program.User.ToString();
                    list[i].CreateTime = Program.NowTime;
                    list[i].TheYear = dtp.Value.Year;
                    list[i].TheMonth = dtp.Value.Month;
                }
                else
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
            if (!Recount(list))
            {
                MessageBox.Show("缺少数据，计算失败！数据完整后，请重新计算！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (new BaseDal<DataBase3CX_CX_12SFBZ>().Add(list) > 0)
            {
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
                List<DataBase3CX_CX_12SFBZ> list = dgv.DataSource as List<DataBase3CX_CX_12SFBZ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3CX_CX_12SFBZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型——师傅补助.xlsx", saveFileDlg.FileName, list, 2, 6))
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
            DataBase3CX_CX_12SFBZ DataBase3CX_CX_12SFBZ = new DataBase3CX_CX_12SFBZ() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3CX_CX_12SFBZ> frm = new FrmModify<DataBase3CX_CX_12SFBZ>(DataBase3CX_CX_12SFBZ, header, OptionType.Add, Text, 5, 2);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_CX_12SFBZ> list = new BaseDal<DataBase3CX_CX_12SFBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_CX_12SFBZ>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3CX_CX_12SFBZ DataBase3CX_CX_12SFBZ)
                {
                    if (DataBase3CX_CX_12SFBZ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3CX_CX_12SFBZ> frm = new FrmModify<DataBase3CX_CX_12SFBZ>(DataBase3CX_CX_12SFBZ, header, OptionType.Modify, Text, 5, 2);
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
                var DataBase3CX_CX_12SFBZ = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_12SFBZ;
                if (DataBase3CX_CX_12SFBZ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_CX_12SFBZ != null)
                    {
                        FrmModify<DataBase3CX_CX_12SFBZ> frm = new FrmModify<DataBase3CX_CX_12SFBZ>(DataBase3CX_CX_12SFBZ, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
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
                var list = dgv.DataSource as List<DataBase3CX_CX_12SFBZ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        continue;
                    }
                    new BaseDal<DataBase3CX_CX_12SFBZ>().Delete(item);
                }
                InitUI();
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }

        private void Frm3CX_CX_12SFBZ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_CX_12SFBZ_DragDrop(object sender, DragEventArgs e)
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

        private void BtnYZ_Click(object sender, EventArgs e)
        {
            bool IsOk = true;
            var listYB = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var list = new BaseDal<DataBase3CX_CX_12SFBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            foreach (var item in list)
            {
                var itemYB = listYB.Where(t => t.UserCode == item.SFCode).FirstOrDefault();
                if (itemYB != null)
                {
                    if (itemYB.GH != item.SFGH)
                    {
                        IsOk = false;
                        MessageBox.Show($"工号不一致，师傅姓名：{item.SFName}，人员编号：{item.SFCode}，成型月报中的工号为：{itemYB.GH},工号为：{item.SFGH}，工号一不一致，验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    IsOk = false;
                    MessageBox.Show($"工号不一致，师傅姓名：{item.SFName}，人员编号：{item.SFCode}，工号为：{item.SFGH}，成型月报中查无此人！验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var itemYBTD = listYB.Where(t => t.GH == item.TDGH).FirstOrDefault();
                if (itemYBTD != null)
                {
                    if (itemYBTD.UserName != item.TDName)
                    {
                        IsOk = false;
                        MessageBox.Show($"工号不一致，徒弟工号：【{item.TDGH}】，姓名：{item.TDName}，成型月报中的此工号的姓名为：{itemYBTD.UserName}，一不一致，验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    IsOk = false;
                    MessageBox.Show($"工号不一致，徒弟工号：【{item.TDGH}】，姓名：{item.TDName}，成型月报中查无此人！验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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