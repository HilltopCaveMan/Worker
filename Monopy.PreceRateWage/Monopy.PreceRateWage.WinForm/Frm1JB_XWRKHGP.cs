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
    public partial class Frm1JB_XWRKHGP : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public Frm1JB_XWRKHGP()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_JB_XWRKHGP_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.text = Program.NowTime.ToString("yyyy-M-d");
            txtTypesName.Text = string.Empty;
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 日查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            RefDgv(selectTime, txtTypesName.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 前一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(-1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 后一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 月查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            DateTime dateTime = DateTime.Parse(dtp.text);
            RefDgv(dateTime.Year, dateTime.Month, txtTypesName.Text);
            dgv.ContextMenuStrip = null;
        }

        /// <summary>
        /// 日查询模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——检包——线位入库合格品日录入.xlsx");
        }

        /// <summary>
        /// 日导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var list = new ExcelHelper<DataBase1JB_XWRKHGP>().ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.TypesName) || string.IsNullOrEmpty(item.No))
                    {
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = selectTime.Year;
                    item.TheMonth = selectTime.Month;
                    item.TheDay = selectTime.Day;
                }
                if (!Recount(list))
                {
                    MessageBox.Show("注意：有【品种】没有设置【指标】基础数据，工资无法计算，请检查【品种】是否录入正确或联系人资！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Enabled = true;
                    return;
                }
                if (new BaseDal<DataBase1JB_XWRKHGP>().Add(list) > 0)
                {
                    Enabled = true;
                    txtTypesName.Text = "";
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Enabled = true;
            }
        }

        /// <summary>
        /// 日导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
            ExportExcel();
        }

        /// <summary>
        /// 日新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            DataBase1JB_XWRKHGP DataBase1JB_XWRKHGP = new DataBase1JB_XWRKHGP() { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month, TheDay = selectTime.Day };
            FrmModify<DataBase1JB_XWRKHGP> frm = new FrmModify<DataBase1JB_XWRKHGP>(DataBase1JB_XWRKHGP, header, OptionType.Add, Text, 5, 13);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                Frm_JB_XWRKHGP_Load(null, null);
            }
        }

        /// <summary>
        /// 日重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase1JB_XWRKHGP> list = dgv.DataSource as List<DataBase1JB_XWRKHGP>;
            dgv.DataSource = null;
            Enabled = false;
            list.RemoveAt(0);
            if (!Recount(list))
            {
                MessageBox.Show("注意：有【品种】没有设置【指标】基础数据，工资无法计算，请检查【品种】是否录入正确或联系人资！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Enabled = true;
                return;
            }
            foreach (var item in list)
            {
                new BaseDal<DataBase1JB_XWRKHGP>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 月导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            btnMonthSearch.PerformClick();
            ExportExcel();
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
                var DataBase1JB_XWRKHGP = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_XWRKHGP;
                if (DataBase1JB_XWRKHGP != null)
                {
                    if (DataBase1JB_XWRKHGP.TypesName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1JB_XWRKHGP> frm = new FrmModify<DataBase1JB_XWRKHGP>(DataBase1JB_XWRKHGP, header, OptionType.Modify, Text, 5, 13);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
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
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var DataBase1JB_XWRKHGP = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_XWRKHGP;
                    if (DataBase1JB_XWRKHGP != null)
                    {
                        if (DataBase1JB_XWRKHGP.TypesName == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                List<DataBase1JB_XWRKHGP> list = dgv.DataSource as List<DataBase1JB_XWRKHGP>;
                                dgv.DataSource = null;
                                Enabled = false;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase1JB_XWRKHGP>().Delete(item);
                                }
                                Enabled = true;
                                btnSearch.PerformClick();
                                MessageBox.Show("全部删除完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase1JB_XWRKHGP> frm = new FrmModify<DataBase1JB_XWRKHGP>(DataBase1JB_XWRKHGP, header, OptionType.Delete, Text, 5, 13);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
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
                    if (item == "btnSearch")
                    {
                        buttonX1.Enabled = dic[item];
                        buttonX2.Enabled = dic[item];
                    }
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

        private void RefDgv(int year, int month, string typesName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1JB_XWRKHGP>().GetList(t => t.TheYear == year && t.TheMonth == month && t.TypesName.Contains(typesName)).ToList().GroupBy(t => new { t.TypesName, t.Unit }).Select(t => new DataBase1JB_XWRKHGP { TypesName = t.Key.TypesName, Unit = t.Key.Unit, X1 = t.Sum(x => string.IsNullOrEmpty(x.X1) ? 0M : Convert.ToDecimal(x.X1)).ToString(), X2 = t.Sum(x => string.IsNullOrEmpty(x.X2) ? 0M : Convert.ToDecimal(x.X2)).ToString(), X3 = t.Sum(x => string.IsNullOrEmpty(x.X3) ? 0M : Convert.ToDecimal(x.X3)).ToString(), X4 = t.Sum(x => string.IsNullOrEmpty(x.X4) ? 0M : Convert.ToDecimal(x.X4)).ToString(), X5 = t.Sum(x => string.IsNullOrEmpty(x.X5) ? 0M : Convert.ToDecimal(x.X5)).ToString(), X6 = t.Sum(x => string.IsNullOrEmpty(x.X6) ? 0M : Convert.ToDecimal(x.X6)).ToString(), X7 = t.Sum(x => string.IsNullOrEmpty(x.X7) ? 0M : Convert.ToDecimal(x.X7)).ToString(), X8 = t.Sum(x => string.IsNullOrEmpty(x.X8) ? 0M : Convert.ToDecimal(x.X8)).ToString(), X9 = t.Sum(x => string.IsNullOrEmpty(x.X9) ? 0M : Convert.ToDecimal(x.X9)).ToString(), X10 = t.Sum(x => string.IsNullOrEmpty(x.X10) ? 0M : Convert.ToDecimal(x.X10)).ToString(), X11 = t.Sum(x => string.IsNullOrEmpty(x.X11) ? 0M : Convert.ToDecimal(x.X11)).ToString(), UnitPrice = t.Max(x => x.UnitPrice), L1 = t.Sum(x => string.IsNullOrEmpty(x.L1) ? 0M : Convert.ToDecimal(x.L1)).ToString(), L2 = t.Sum(x => string.IsNullOrEmpty(x.L2) ? 0M : Convert.ToDecimal(x.L2)).ToString(), L3 = t.Sum(x => string.IsNullOrEmpty(x.L3) ? 0M : Convert.ToDecimal(x.L3)).ToString(), L4 = t.Sum(x => string.IsNullOrEmpty(x.L4) ? 0M : Convert.ToDecimal(x.L4)).ToString(), L5 = t.Sum(x => string.IsNullOrEmpty(x.L5) ? 0M : Convert.ToDecimal(x.L5)).ToString(), L6 = t.Sum(x => string.IsNullOrEmpty(x.L6) ? 0M : Convert.ToDecimal(x.L6)).ToString(), L7 = t.Sum(x => string.IsNullOrEmpty(x.L7) ? 0M : Convert.ToDecimal(x.L7)).ToString(), L8 = t.Sum(x => string.IsNullOrEmpty(x.L8) ? 0M : Convert.ToDecimal(x.L8)).ToString(), L9 = t.Sum(x => string.IsNullOrEmpty(x.L9) ? 0M : Convert.ToDecimal(x.L9)).ToString(), L10 = t.Sum(x => string.IsNullOrEmpty(x.L10) ? 0M : Convert.ToDecimal(x.L10)).ToString(), L11 = t.Sum(x => string.IsNullOrEmpty(x.L11) ? 0M : Convert.ToDecimal(x.L11)).ToString() }).OrderBy(t => t.TypesName).ToList();
            var hj = MyDal.GetTotalDataBase1JB_XWRKHGP(datas, new DateTime(year, month, 1));
            hj.Unit = "月";
            datas.Insert(0, hj);
            dgv.DataSource = datas;
            for (int i = 0; i < 7; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间	操作人	年	月	日	序号	品种	单位	1	2	3	4	5	6	7	8	9	10	11	合计	单价	1	2	3	4	5	6	7	8	9	10	11	1-8.11".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void RefDgv(DateTime selectTime, string typesName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1JB_XWRKHGP>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day && t.TypesName.Contains(typesName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            var hj = MyDal.GetTotalDataBase1JB_XWRKHGP(datas, selectTime);
            hj.Unit = "日";
            datas.Insert(0, hj);
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "操作时间	操作人	年	月	日	序号	品种	单位	1	2	3	4	5	6	7	8	9	10	11	合计	单价	1	2	3	4	5	6	7	8	9	10	11	1-8.11".Split('	');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private bool CheckTime(string strTime)
        {
            if (string.IsNullOrEmpty(strTime) || !DateTime.TryParse(strTime, out selectTime))
            {
                MessageBox.Show("请选择日期，或日期错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp.Focus();
                return false;
            }
            return true;
        }

        private bool Recount(List<DataBase1JB_XWRKHGP> list)
        {
            foreach (var item in list)
            {
                DataBaseDay dataBaseDay = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G001" && t.WorkshopName == "检包车间" && t.TypesName == item.TypesName && t.TypesUnit == item.Unit);
                if (dataBaseDay == null)
                {
                    MessageBox.Show("没有基础数据：" + item.TypesName + "," + item.Unit + "请检查是否名称写错，或单位写错！！！");
                    return false;
                }
                item.UnitPrice = dataBaseDay.UnitPrice;
                item.L1 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X1) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X1)).ToString();
                item.L2 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X2) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X2)).ToString();
                item.L3 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X3) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X3)).ToString();
                item.L4 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X4) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X4)).ToString();
                item.L5 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X5) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X5)).ToString();
                item.L6 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X6) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X6)).ToString();
                item.L7 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X7) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X7)).ToString();
                item.L8 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X8) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X8)).ToString();
                item.L9 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X9) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X9)).ToString();
                item.L10 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X10) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X10)).ToString();
                item.L11 = string.IsNullOrEmpty(item.UnitPrice) || string.IsNullOrEmpty(item.X11) ? string.Empty : (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.X11)).ToString();
            }
            return true;
        }

        private void ExportExcel()
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1JB_XWRKHGP> list = dgv.DataSource as List<DataBase1JB_XWRKHGP>;
                var hj = list[0];
                list.RemoveAt(0);
                string tm = hj.Unit;
                hj.Unit = string.Empty;
                list.Add(hj);
                List<int> propertysNos = new List<int>
                {
                    6,
                    7,
                    8,9,10,11,12,13,14,15,16,17,18,19,
                    20,
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27,
                    28,
                    29,
                    30,
                    31,32,33
                };
                if (new ExcelHelper<DataBase1JB_XWRKHGP>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——检包——线位入库合格品日录入和计算.xlsx", saveFileDlg.FileName, list, 2, propertysNos, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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
                list.Remove(hj);
                hj.Unit = tm;
                list.Insert(0, hj);
                //btnSearch.PerformClick();
            }
        }

        #endregion
        
    }
}