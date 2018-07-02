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
    public partial class Frm2YL_NYLYLTJ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$釉浆名称$生产数量(吨)$材料出库数量(公斤)$金额（元）".Split('$');
        private string[] header1 = "创建日期$创建人$年$月$序号$泥浆名称$生产数量(吨)$材料出库数量(吨)$金额（元）".Split('$');

        public Frm2YL_NYLYLTJ()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2YL_NYLYLTJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询釉浆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        /// <summary>
        /// 查询泥浆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCS_Click(object sender, EventArgs e)
        {
            RefDgv1(dtp.Value);
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
        /// 查询模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——原料——泥、釉料统计.xlsx");
        }

        /// <summary>
        /// 导出釉浆
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
                var list = new BaseDal<DataBase2YL_NYLYLJJ_01YJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
                if (new ExcelHelper<DataBase2YL_NYLYLJJ_01YJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——原料——釉浆原料统计.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 导出泥浆
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
                var list1 = new BaseDal<DataBase2YL_NYLYLJJ_02NJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();

                if (new ExcelHelper<DataBase2YL_NYLYLJJ_02NJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——原料——泥浆原料统计.xlsx", saveFileDlg.FileName, list1, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 全部删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                new BaseDal<DataBase2YL_NYLYLJJ_02NJ>().ExecuteSqlCommand("delete from DataBase2YL_NYLYLJJ_02NJ where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);
                new BaseDal<DataBase2YL_NYLYLJJ_02NJ>().ExecuteSqlCommand("delete from DataBase2YL_NYLYLJJ_01YJ where theyear=" + dtp.Value.Year + " and themonth=" + dtp.Value.Month);

                btnSearch.PerformClick();
            }
        }

        private void Frm2YL_NYLYLTJ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm2YL_NYLYLTJ_DragDrop(object sender, DragEventArgs e)
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

        private void RefDgv(DateTime selectTime)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2YL_NYLYLJJ_01YJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
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

        private void RefDgv1(DateTime selectTime)
        {
            dgv.DataSource = null;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2YL_NYLYLJJ_02NJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
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

        private void Import(string fileName)
        {
            var list = new ExcelHelper<DataBase2YL_NYLYLJJ_02NJ>().ReadExcel(fileName, 3, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            var listNJ = new List<DataBase2YL_NYLYLJJ_02NJ>();
            var listYJ = new List<DataBase2YL_NYLYLJJ_01YJ>();

            var tmp = list.Where(t => string.IsNullOrEmpty(t.NJMC)).FirstOrDefault();

            int flag = Convert.ToInt32(tmp.No);
            for (int i = 0; i < flag - 1; i++)
            {
                var item = list[i];
                var kh = new DataBase2YL_NYLYLJJ_01YJ { Id = item.Id, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), YJMC = item.NJMC, SCSL = item.SCSL, CKSL = item.CKSL, JE = item.JE, No = item.No, TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
                listYJ.Add(kh);
            }
            for (int i = flag + 1; i < list.Count; i++)
            {
                var item = list[i];

                item.CreateTime = Program.NowTime;
                item.CreateUser = Program.User.ToString();
                item.TheYear = dtp.Value.Year;
                item.TheMonth = dtp.Value.Month;
                listNJ.Add(item);
            }
            if (new BaseDal<DataBase2YL_NYLYLJJ_02NJ>().Add(listNJ) > 0 && new BaseDal<DataBase2YL_NYLYLJJ_01YJ>().Add(listYJ) > 0)
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