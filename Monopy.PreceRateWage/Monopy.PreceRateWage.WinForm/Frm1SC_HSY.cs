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
    public partial class Frm1SC_HSY : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$类型$序号$产品名称$颜色$开窑量$产品等级_合格$产品等级_等外$产品等级_残$产品等级_冷补修$产品等级_回烧$产品等级_磨$产品等级_磨修$合格率$原料缺陷_铜脏$原料缺陷_铁脏$原料缺陷_泥脏$原料缺陷_小计$缺陷率$成型缺陷_成走$成型缺陷_内裂$成型缺陷_裂底$成型缺陷_裂体$成型缺陷_裂眼$成型缺陷_修糙$成型缺陷_棕眼$成型缺陷_成脏$成型缺陷_泥绺$成型缺陷_坯泡$成型缺陷_崩渣$成型缺陷_冲刷$成型缺陷_卡球$成型缺陷_吹脏$成型缺陷_小计$缺陷率$修挡板$修检_裂体$修检_修糙$修检_棕眼$修检_成脏$修检_卡球$修检_滚釉$修检_爆釉$修检_坯渣$修检_小计$缺陷率$喷釉缺陷_滚釉$喷釉缺陷_滚釉釉薄$喷釉缺陷_滚釉釉厚$喷釉缺陷_滚釉釉缕$喷釉缺陷_滚釉爆釉$喷釉缺陷_滚釉坯渣$喷釉缺陷_滚釉坯磕$喷釉缺陷_滚釉釉脏$喷釉缺陷_滚釉刮边$喷釉缺陷_滚釉釉粘$喷釉缺陷_滚釉坯脏$喷釉缺陷_滚釉小计$缺陷率$回烧缺陷_修补不合格$回烧缺陷_漏补$回烧缺陷_缺陷$回烧缺陷_小计$缺陷率$装窑缺陷_装走$装窑缺陷_装粘$装窑缺陷_装磕$装窑缺陷_装脏$装窑缺陷_刮边$装窑缺陷_小计$缺陷率$开窑缺陷_出磕$开窑缺陷_划釉$开窑缺陷_小计$缺陷率$烧窑缺陷_桔釉$烧窑缺陷_氧化$烧窑缺陷_烧泡$烧窑缺陷_烧裂$烧窑缺陷_烧生$烧窑缺陷_窑脏$烧窑缺陷_风惊$烧窑缺陷_小计$缺陷率$标污$烧成缺陷率$吹脏$缺陷率$崩脏$缺陷率".Split('$');
        private string[] headerBC = "创建日期$创建人$年$月$开窑量$修补不合格数$修补率$目标$考核单价$奖罚$考核金额".Split('$');


        public Frm1SC_HSY()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1SC_HSY_Load(object sender, EventArgs e)
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
        /// 补瓷考核查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBC_Click(object sender, EventArgs e)
        {
            RefDgvBC(dtp.Value);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx");
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
                    List<DataBase1SC_HSY> list = new ExcelHelper<DataBase1SC_HSY>().ReadExcel(openFileDlg.FileName, 4, 7, 0, 0, 0, true);
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
                    if (new BaseDal<DataBase1SC_HSY>().Add(list) > 0)
                    {
                        btnRecount.PerformClick();
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
                List<DataBase1SC_HSY> list = dgv.DataSource as List<DataBase1SC_HSY>;
                if (new ExcelHelper<DataBase1SC_HSY>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——总窑回烧.xlsx", saveFileDlg.FileName, list, 4, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase1SC_HSY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase1SC_HSY DataBase1SC_HSY = new DataBase1SC_HSY() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString(), TheType = 1 };
            FrmModify<DataBase1SC_HSY> frm = new FrmModify<DataBase1SC_HSY>(DataBase1SC_HSY, header, OptionType.Add, Text, 6, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        /// <summary>
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase1SC_HSY> list = new BaseDal<DataBase1SC_HSY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.TheType == 1).ToList();

            new BaseDal<DataBase1SC_HSY_BCKH>().ExecuteSqlCommand("delete from DataBase1SC_HSY_BCKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);


            if (new BaseDal<DataBase1SC_HSY_BCKH>().Add(Recount_XCG(list)) > 0)
            {
                MessageBox.Show("补瓷考核，计算成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 导出补瓷考核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcelXCG_Click(object sender, EventArgs e)
        {
            btnSearchBC.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1SC_HSY_BCKH> list = dgv.DataSource as List<DataBase1SC_HSY_BCKH>;
                if (new ExcelHelper<DataBase1SC_HSY_BCKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板一厂——烧成——补瓷考核.xlsx", saveFileDlg.FileName, list, 1, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1SC_HSY DataBase1SC_HSY)
                {
                    FrmModify<DataBase1SC_HSY> frm = new FrmModify<DataBase1SC_HSY>(DataBase1SC_HSY, header, OptionType.Modify, Text, 6, 0);
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
                var DataBase1SC_HSY = dgv.SelectedRows[0].DataBoundItem as DataBase1SC_HSY;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1SC_HSY != null)
                    {
                        FrmModify<DataBase1SC_HSY> frm = new FrmModify<DataBase1SC_HSY>(DataBase1SC_HSY, header, OptionType.Delete, Text, 6, 0);
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
                var list = dgv.DataSource as List<DataBase1SC_HSY>;
                foreach (var item in list)
                {
                    new BaseDal<DataBase1SC_HSY>().Delete(item);
                }
                new BaseDal<DataBase1SC_HSY_BCKH>().ExecuteSqlCommand("delete from DataBase1SC_HSY_BCKH where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);

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
            var datas = new BaseDal<DataBase1SC_HSY>().GetList(t => t.TheType == 1 && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
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

        private void RefDgvBC(DateTime selectTime)
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
            var datas = new BaseDal<DataBase1SC_HSY_BCKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerBC.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerBC[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private DataBase1SC_HSY_BCKH Recount_XCG(List<DataBase1SC_HSY> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算【补瓷考核】！");
                return null;
            }
            DataBase1SC_HSY_BCKH xCGKH = new DataBase1SC_HSY_BCKH { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };
            var listTJ = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G001" && t.WorkshopName == "烧成车间" && t.PostName == "补瓷工").FirstOrDefault();
            try
            {
                var zj = list.Where(t => t.CPMC == "总计").OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                var xj = list.Where(t => t.CPMC == "30-盖").OrderByDescending(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).FirstOrDefault();
                decimal.TryParse(zj.KYL, out decimal zjkyl);// 总计开窑量
                decimal.TryParse(xj.KYL, out decimal xjkyl);//30-盖开窑量
                decimal.TryParse(zj.HSQX_01, out decimal zjbhg);//总计修补不合格
                decimal.TryParse(xj.HSQX_01, out decimal xjbhg);//30-盖修补不合格
                decimal.TryParse(listTJ.GRJSB1, out decimal grjsb);//个人基数比
                decimal.TryParse(listTJ.GRFKDJ1, out decimal grfkdj);//个人罚款单价1
                decimal.TryParse(listTJ.KHJESFD, out decimal sfd);//考核金额上封顶
                decimal.TryParse(listTJ.KHJEXFD, out decimal xfd);//考核金额下封顶
                xCGKH.KYL = (zjkyl - xjkyl).ToString();
                xCGKH.XBBHGS = (zjbhg - xjbhg).ToString();
                xCGKH.XBL = ((zjbhg - xjbhg) / (zjkyl - xjkyl)).ToString();
                xCGKH.MB = listTJ.GRKHZB1;
                xCGKH.KHDJ = listTJ.UnitPrice;
                decimal.TryParse(xCGKH.XBL, out decimal xbl);//修补率
                decimal.TryParse(xCGKH.MB, out decimal mb);//目标
                decimal.TryParse(xCGKH.KHDJ, out decimal khdj);//考核单价
                if (mb - xbl >= 0)
                {
                    xCGKH.JF = listTJ.UnitPrice;
                }
                else
                {
                    xCGKH.JF = (khdj - (mb - xbl) / (grjsb * grfkdj)).ToString();
                }
                decimal.TryParse(xCGKH.JF, out decimal jf);//奖罚
                if (jf >= sfd)
                {
                    xCGKH.KHJJ = sfd.ToString();
                }
                else
                {
                    xCGKH.KHJJ = xfd.ToString();
                }
                return xCGKH;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        #endregion

    }
}