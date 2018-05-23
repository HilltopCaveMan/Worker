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
    public partial class Frm3CX_CX_06ZXJ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$岗位$工号$人员编码$姓名$品种$数量$单价$金额".Split('$');

        public Frm3CX_CX_06ZXJ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbGH(List<DataBase3CX_CX_06ZXJ> list)
        {
            var listTmp = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGH.DataSource = listTmp;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = string.Empty;
        }

        private void RefCmbUserCode(List<DataBase3CX_CX_06ZXJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase3CX_CX_06ZXJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void RefCmbPZ(List<DataBase3CX_CX_06ZXJ> list)
        {
            var listTmp = list.GroupBy(t => t.PZ).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbPZ.DataSource = listTmp;
            CmbPZ.DisplayMember = "PZ";
            CmbPZ.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase3CX_CX_06ZXJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbGH(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbPZ(list);
        }

        private void Frm3CX_CX_06ZXJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gh, string userCode, string userName, string pz)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_06ZXJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.PZ.Contains(pz)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_06ZXJ(datas));
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
            RefDgv(dtp.Value, CmbGH.Text, CmbUserCode.Text, CmbUserName.Text, CmbPZ.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——注小计.xlsx");
        }

        private bool Recount(List<DataBase3CX_CX_06ZXJ> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "注修工");
                foreach (var item in list)
                {
                    var itemDay = listDay.Where(t => t.TypesName == item.PZ).FirstOrDefault();
                    if (itemDay != null)
                    {
                        item.DJ = itemDay.UnitPrice;
                        decimal.TryParse(item.DJ, out decimal dj);
                        decimal.TryParse(item.SL, out decimal sl);
                        var je = sl * dj;
                        item.JE = je.ToString();
                    }
                    else
                    {
                        MessageBox.Show($"产品{item.PZ}，在基础数据库中不存在，请检查或联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
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
            List<DataBase3CX_CX_06ZXJ> list = new ExcelHelper<DataBase3CX_CX_06ZXJ>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
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
                var gh = list[i].GH;
                if (gh.Length < 3)
                {
                    gh = gh.PadLeft(3, '0');
                }
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            if (Recount(list) && new BaseDal<DataBase3CX_CX_06ZXJ>().Add(list) > 0)
            {
                Enabled = true;
                InitUI();
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
                List<DataBase3CX_CX_06ZXJ> list = dgv.DataSource as List<DataBase3CX_CX_06ZXJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3CX_CX_06ZXJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——成型——注小计.xlsx", saveFileDlg.FileName, list, 2, 6))
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
            DataBase3CX_CX_06ZXJ DataBase3CX_CX_06ZXJ = new DataBase3CX_CX_06ZXJ() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3CX_CX_06ZXJ> frm = new FrmModify<DataBase3CX_CX_06ZXJ>(DataBase3CX_CX_06ZXJ, header, OptionType.Add, Text, 5, 2);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_CX_06ZXJ> list = new BaseDal<DataBase3CX_CX_06ZXJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_CX_06ZXJ>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3CX_CX_06ZXJ DataBase3CX_CX_06ZXJ)
                {
                    if (DataBase3CX_CX_06ZXJ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3CX_CX_06ZXJ> frm = new FrmModify<DataBase3CX_CX_06ZXJ>(DataBase3CX_CX_06ZXJ, header, OptionType.Modify, Text, 5, 2);
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
                var DataBase3CX_CX_06ZXJ = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_06ZXJ;
                if (DataBase3CX_CX_06ZXJ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_CX_06ZXJ != null)
                    {
                        FrmModify<DataBase3CX_CX_06ZXJ> frm = new FrmModify<DataBase3CX_CX_06ZXJ>(DataBase3CX_CX_06ZXJ, header, OptionType.Delete, Text, 5);
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
                var list = dgv.DataSource as List<DataBase3CX_CX_06ZXJ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        continue;
                    }
                    new BaseDal<DataBase3CX_CX_06ZXJ>().Delete(item);
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

        private void Frm3CX_CX_06ZXJ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_CX_06ZXJ_DragDrop(object sender, DragEventArgs e)
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
            bool isOK = true;
            var list = new BaseDal<DataBase3CX_CX_06ZXJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            foreach (var item in list)
            {
                var jpb = new BaseDal<DataBase3CX_GYMJ_06_JPB>().GetList(t => t.UserCode == item.UserCode && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).FirstOrDefault();
                if (jpb == null)
                {
                    isOK = false;
                    MessageBox.Show($"交坯表中没有人员编号为{item.UserCode}，姓名为{item.UserName},的交坯数据！验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var tmpI = jpb.Childs.Where(t => t.Name == item.PZ).ToList().Sum(t => int.TryParse(t.Count, out int ii) ? ii : 0m);
                    int.TryParse(item.SL, out int sl);
                    if (sl != tmpI)
                    {
                        isOK = false;
                        MessageBox.Show($"数量错误，工号{item.GH},人员编码{item.UserCode},人员姓名{item.UserName},品种{item.PZ},注小件数量为【{item.SL}】,交坯表数量：【{tmpI}】，数量不相等！验证失败！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (isOK)
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