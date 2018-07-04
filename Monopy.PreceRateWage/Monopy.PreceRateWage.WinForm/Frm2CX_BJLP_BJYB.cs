using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm2CX_BJLP_BJYB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工号$类别编码$类别名称$员工编码$姓名$工厂$工段编码$工段名称$开窑量$一级品$降级$破损$指标$奖励单价$罚款单价$考核金额".Split('$');

        public Frm2CX_BJLP_BJYB()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2CX_BJLP_BJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtGH.Text, txtLBMC.Text, txtUserName.Text);
        }

        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——半检拉坯——半检月报.xlsx");
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
                SaveImportData(openFileDlg.FileName);
            }
        }

        /// <summary>
        /// 导出
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
                List<DataBase2CX_BJLP_BJYB> list = dgv.DataSource as List<DataBase2CX_BJLP_BJYB>;
                if (new ExcelHelper<DataBase2CX_BJLP_BJYB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——半检拉坯——半检月报.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2CX_BJLP_BJYB> list = dgv.DataSource as List<DataBase2CX_BJLP_BJYB>;
            dgv.DataSource = null;
            Enabled = false;
            if (list == null || list.Count == 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                return;
            }
            list.RemoveAt(0);
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2CX_BJLP_BJYB>().Edit(item);
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

        private void Frm2CX_BJLP_BJYB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm2CX_BJLP_BJYB_DragDrop(object sender, DragEventArgs e)
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
            SaveImportData(path);
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
                var DataBase2CX_BJLP_BJYB = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_BJLP_BJYB;
                if (DataBase2CX_BJLP_BJYB != null)
                {
                    FrmModify<DataBase2CX_BJLP_BJYB> frm = new FrmModify<DataBase2CX_BJLP_BJYB>(DataBase2CX_BJLP_BJYB, header, OptionType.Modify, Text, 6, 0);
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
                var item = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_BJLP_BJYB;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (item != null)
                    {
                        if (item.LBBM.Equals("合计"))
                        {
                            if (MessageBox.Show("确定要全部删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase2CX_BJLP_BJYB>;
                                list.RemoveAt(0);
                                foreach (var i in list)
                                {
                                    new BaseDal<DataBase2CX_BJLP_BJYB>().Delete(i);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase2CX_BJLP_BJYB> frm = new FrmModify<DataBase2CX_BJLP_BJYB>(item, header, OptionType.Delete, Text, 5, 0);
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

        private void RefDgv(DateTime selectTime, string gh, string lbmc, string userName)
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
            var datas = new BaseDal<DataBase2CX_BJLP_BJYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && t.LBMC.Contains(lbmc) && t.UserName.Contains(userName)).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2CX_BJLP_BJYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }

            dgv.ClearSelection();
        }

        private void Import(string filePath)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<DataBase2CX_BJLP_BJYB> list = new List<DataBase2CX_BJLP_BJYB>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i;
                        string lb = string.Empty;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            bool sign = true;
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            if (ExcelHelper.GetCellValue(row.GetCell(0)).Contains("工号") && lb != ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1])
                            {
                                sign = false;
                                lb = ExcelHelper.GetCellValue(row.GetCell(0)).Split(':')[1];
                            }
                            if (sign)
                            {
                                DataBase2CX_BJLP_BJYB t = new DataBase2CX_BJLP_BJYB { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                                t.No = (i - 2).ToString();
                                t.GH = lb;

                                t.LBBM = ExcelHelper.GetCellValue(row.GetCell(1));
                                t.LBMC = ExcelHelper.GetCellValue(row.GetCell(2));
                                t.UserCode = ExcelHelper.GetCellValue(row.GetCell(3));
                                t.UserName = ExcelHelper.GetCellValue(row.GetCell(4));
                                t.GC = ExcelHelper.GetCellValue(row.GetCell(5));
                                t.GDBM = ExcelHelper.GetCellValue(row.GetCell(6));
                                t.GDMC = ExcelHelper.GetCellValue(row.GetCell(7));
                                t.KYL = ExcelHelper.GetCellValue(row.GetCell(8));
                                t.YJP = ExcelHelper.GetCellValue(row.GetCell(9));
                                t.JJ = ExcelHelper.GetCellValue(row.GetCell(10));
                                t.PS = ExcelHelper.GetCellValue(row.GetCell(11));
                                t.ZB = ExcelHelper.GetCellValue(row.GetCell(12));
                                t.JLDJ = ExcelHelper.GetCellValue(row.GetCell(13));
                                t.FKDJ = ExcelHelper.GetCellValue(row.GetCell(14));
                                t.KHJE = ExcelHelper.GetCellValue(row.GetCell(15));

                                list.Add(t);
                            }
                        }
                    }
                    Recount(list);
                    if (new BaseDal<DataBase2CX_BJLP_BJYB>().Add(list) > 0)
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

        private void SaveImportData(string filePath)
        {
            Import(filePath);
        }

        private bool Recount(List<DataBase2CX_BJLP_BJYB> list)
        {
            try
            {
                var listBase = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dtp.Value.Year && h.CreateMonth == dtp.Value.Month && h.FactoryNo == "G002" && h.WorkshopName == "成型车间" && h.PostName == "半检工").ToList();

                if (listBase == null || listBase.Count == 0)
                {
                    MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.UserCode))
                    {
                        var tmp = listBase.Where(t => t.TypesType == item.LBMC).FirstOrDefault();
                        if (tmp == null)
                        {
                            MessageBox.Show("指标数据中没有" + item.LBMC + "的数据，无法计算，导入指标后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            item.ZB = tmp.GRKHZB1;
                            item.JLDJ = tmp.GRJLDJ1;
                            item.FKDJ = tmp.GRFKDJ1;
                            decimal.TryParse(item.YJP, out decimal yjp);//一级品
                            decimal.TryParse(item.KYL, out decimal kyl);//开窑量
                            decimal.TryParse(item.ZB, out decimal zb);//指标
                            decimal.TryParse(item.JLDJ, out decimal jldj);//奖励单价
                            decimal.TryParse(item.FKDJ, out decimal fkdj);//罚款单价
                            if (yjp / kyl >= zb)
                            {
                                item.KHJE = ((yjp - kyl * zb) * jldj).ToString();
                            }
                            else
                            {
                                item.KHJE = ((yjp - kyl * zb) * fkdj).ToString();
                            }
                            decimal.TryParse(item.KHJE, out decimal khje);
                            decimal.TryParse(tmp.KHJEXFD, out decimal xfd);
                            if (khje < xfd)
                            {
                                item.KHJE = tmp.KHJEXFD;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        #endregion









    }
}