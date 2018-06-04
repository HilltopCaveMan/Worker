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
    public partial class Frm1SC_SY : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$类型$序号$产品名称$颜色$开窑量$产品等级_合格$产品等级_等外$产品等级_残$产品等级_冷补修$产品等级_回烧$产品等级_磨$产品等级_磨修$合格率$原料缺陷_铜脏$原料缺陷_铁脏$原料缺陷_泥脏$原料缺陷_小计$缺陷率$成型缺陷_成走$成型缺陷_内裂$成型缺陷_裂底$成型缺陷_裂体$成型缺陷_裂眼$成型缺陷_修糙$成型缺陷_棕眼$成型缺陷_成脏$成型缺陷_泥绺$成型缺陷_坯泡$成型缺陷_崩渣$成型缺陷_冲刷$成型缺陷_卡球$成型缺陷_吹脏$成型缺陷_小计$缺陷率$修挡板$修检_裂体$修检_修糙$修检_棕眼$修检_成脏$修检_卡球$修检_滚釉$修检_爆釉$修检_坯渣$修检_小计$缺陷率$喷釉缺陷_滚釉$喷釉缺陷_滚釉釉薄$喷釉缺陷_滚釉釉厚$喷釉缺陷_滚釉釉缕$喷釉缺陷_滚釉爆釉$喷釉缺陷_滚釉坯渣$喷釉缺陷_滚釉坯磕$喷釉缺陷_滚釉釉脏$喷釉缺陷_滚釉刮边$喷釉缺陷_滚釉釉粘$喷釉缺陷_滚釉坯脏$喷釉缺陷_滚釉小计$缺陷率$回烧缺陷_修补不合格$回烧缺陷_漏补$回烧缺陷_缺陷$回烧缺陷_小计$缺陷率$装窑缺陷_装走$装窑缺陷_装粘$装窑缺陷_装磕$装窑缺陷_装脏$装窑缺陷_刮边$装窑缺陷_小计$缺陷率$开窑缺陷_出磕$开窑缺陷_划釉$开窑缺陷_小计$缺陷率$烧窑缺陷_桔釉$烧窑缺陷_氧化$烧窑缺陷_烧泡$烧窑缺陷_烧裂$烧窑缺陷_烧生$烧窑缺陷_窑脏$烧窑缺陷_风惊$烧窑缺陷_小计$缺陷率$标污$烧成缺陷率$吹脏$缺陷率$崩脏$缺陷率".Split('$');

        public Frm1SC_SY()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1SC_SY_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——烧成——总窑.xlsx");
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
                try
                {
                    List<DataBase1SC_SY> list = new ExcelHelper<DataBase1SC_SY>().ReadExcel(openFileDlg.FileName, 4, 7, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        item.TheType = 1;
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase1SC_SY>().Add(list) > 0)
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                List<DataBase1SC_SY> list = dgv.DataSource as List<DataBase1SC_SY>;
                if (new ExcelHelper<DataBase1SC_SY>().WriteExcle(Application.StartupPath + "\\Excel\\模板一厂——烧成——总窑.xlsx", saveFileDlg.FileName, list, 4, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase1SC_SY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase1SC_SY DataBase1SC_SY = new DataBase1SC_SY() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString(), TheType = 1 };
            FrmModify<DataBase1SC_SY> frm = new FrmModify<DataBase1SC_SY>(DataBase1SC_SY, header, OptionType.Add, Text, 6, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {

                btnSearch.PerformClick();
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1SC_SY DataBase1SC_SY)
                {
                    FrmModify<DataBase1SC_SY> frm = new FrmModify<DataBase1SC_SY>(DataBase1SC_SY, header, OptionType.Modify, Text, 6, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {

                        btnSearch.PerformClick();
                    }
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase1SC_SY = dgv.SelectedRows[0].DataBoundItem as DataBase1SC_SY;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1SC_SY != null)
                    {
                        FrmModify<DataBase1SC_SY> frm = new FrmModify<DataBase1SC_SY>(DataBase1SC_SY, header, OptionType.Delete, Text, 6, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var list = dgv.DataSource as List<DataBase1SC_SY>;
                foreach (var item in list)
                {
                    new BaseDal<DataBase1SC_SY>().Delete(item);
                }
                btnSearch.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }

        #endregion

        #region 调用方法

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
            var datas = new BaseDal<DataBase1SC_SY>().GetList(t => t.TheType == 1 && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            dgv.DataSource = null;
            dgv.DataSource = datas;
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

        #endregion


    }
}