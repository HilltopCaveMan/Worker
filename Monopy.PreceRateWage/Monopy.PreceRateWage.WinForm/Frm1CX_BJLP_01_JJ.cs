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
    public partial class Frm1CX_BJLP_01_JJ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$人员编码$人员姓名$职位$应出勤$实出勤$计件金额".Split('$');

        public Frm1CX_BJLP_01_JJ()
        {
            InitializeComponent();

            EnableGlass = false;
        }

        private void RefCmbUserCode(List<DataBase1CX_BJLP_01_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase1CX_BJLP_01_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase1CX_BJLP_01_JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }

        private void Frm1CX_BJLP_01_JJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            Import();
            InitUI();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1CX_BJLP_01_JJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1CX_BJLP_01_JJ(datas));
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
            RefDgv(dtp.Value, CmbUserCode.Text, CmbUserName.Text);
        }

        private void Import()
        {
            var oldData = new BaseDal<DataBase1CX_BJLP_01_JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            foreach (var item in oldData)
            {
                new BaseDal<DataBase1CX_BJLP_01_JJ>().Delete(item);
            }
            List<DataBase1CX_BJLP_01_JJ> list = new List<DataBase1CX_BJLP_01_JJ>();
            var datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory == "G001" && t.Dept == "青白坯库").ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            if (datas == null || datas.Count == 0)
            {
                MessageBox.Show("出勤表里没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的数据！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            decimal totalCount = 0M;

            for (int i = 0; i < datas.Count; i++)
            {
                DataBase1CX_BJLP_01_JJ DataBase1CX_BJLP_01_JJ = new DataBase1CX_BJLP_01_JJ();
                DataBase1CX_BJLP_01_JJ.CreateUser = Program.User.ToString();
                DataBase1CX_BJLP_01_JJ.CreateTime = Program.NowTime;
                DataBase1CX_BJLP_01_JJ.TheYear = dtp.Value.Year;
                DataBase1CX_BJLP_01_JJ.TheMonth = dtp.Value.Month;
                DataBase1CX_BJLP_01_JJ.Id = new Guid();
                DataBase1CX_BJLP_01_JJ.No = (i + 1).ToString();
                DataBase1CX_BJLP_01_JJ.UserCode = datas[i].UserCode;
                DataBase1CX_BJLP_01_JJ.UserName = datas[i].UserName;
                DataBase1CX_BJLP_01_JJ.ZW = datas[i].Position;
                DataBase1CX_BJLP_01_JJ.YCQ = datas[i].DayYcq;
                DataBase1CX_BJLP_01_JJ.SCQ = datas[i].DayScq;
                decimal.TryParse(DataBase1CX_BJLP_01_JJ.YCQ, out decimal ycq);
                totalCount += ycq;
                list.Add(DataBase1CX_BJLP_01_JJ);
            }
            var baseNumber = new BaseDal<DataBase1CX_BJLP_01_LPYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && string.IsNullOrEmpty(t.UserCode)).FirstOrDefault();
            if (baseNumber == null)
            {
                MessageBox.Show("拉坯月报里没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的数据！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            decimal.TryParse(baseNumber.JE, out decimal je);

            for (int i = 0; i < list.Count; i++)
            {
                decimal.TryParse(list[i].YCQ, out decimal ycq);
                list[i].JJJE = (je / totalCount * ycq).ToString();
            }
            new BaseDal<DataBase1CX_BJLP_01_JJ>().Add(list);
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
                List<DataBase1CX_BJLP_01_JJ> list = dgv.DataSource as List<DataBase1CX_BJLP_01_JJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase1CX_BJLP_01_JJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——半检拉坯——计件.xlsx", saveFileDlg.FileName, list, 2, 6))
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

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase1CX_BJLP_01_JJ = dgv.SelectedRows[0].DataBoundItem as DataBase1CX_BJLP_01_JJ;
                if (DataBase1CX_BJLP_01_JJ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1CX_BJLP_01_JJ != null)
                    {
                        FrmModify<DataBase1CX_BJLP_01_JJ> frm = new FrmModify<DataBase1CX_BJLP_01_JJ>(DataBase1CX_BJLP_01_JJ, header, OptionType.Delete, Text, 4);
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
                var list = dgv.DataSource as List<DataBase1CX_BJLP_01_JJ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase1CX_BJLP_01_JJ>().Delete(item);
                    }
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

        private void Frm1CX_BJLP_01_JJ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitUI();
        }
    }
}