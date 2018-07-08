using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm2JB_ZZCQ : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public Frm2JB_ZZCQ()
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
        private void Frm2JB_ZZCQ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, CmbUserCode.Text, CmbUserName.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——检包——组装工出勤.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase2JB_ZZCQ> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBase2JB_ZZCQ>().ReadExcel(openFileDlg.FileName, 2, 5);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].CreateUser = Program.User.ToString();
                    list[i].CreateTime = Program.NowTime;
                    list[i].TheYear = selectTime.Year;
                    list[i].TheMonth = selectTime.Month;
                }
                if (Recount(list) && new BaseDal<DataBase2JB_ZZCQ>().Add(list) > 0)
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
                List<DataBase2JB_ZZCQ> list = dgv.DataSource as List<DataBase2JB_ZZCQ>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase2JB_ZZCQ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——检包——组装工出勤.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase2JB_ZZCQ DataBase2JB_ZZCQ = new DataBase2JB_ZZCQ() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month };
            FrmModify<DataBase2JB_ZZCQ> frm = new FrmModify<DataBase2JB_ZZCQ>(DataBase2JB_ZZCQ, header, OptionType.Add, Text, 5, 1);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                InitUI();
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
            Enabled = false;
            List<DataBase2JB_ZZCQ> list = new BaseDal<DataBase2JB_ZZCQ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            Recount(list);

            foreach (var item in list)
            {
                new BaseDal<DataBase2JB_ZZCQ>().Edit(item);
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase2JB_ZZCQ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_ZZCQ;
                if (DataBase2JB_ZZCQ != null)
                {
                    if (DataBase2JB_ZZCQ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2JB_ZZCQ> frm = new FrmModify<DataBase2JB_ZZCQ>(DataBase2JB_ZZCQ, header, OptionType.Modify, Text, 5, 1);
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
                var DataBase2JB_ZZCQ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_ZZCQ;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2JB_ZZCQ != null)
                    {
                        if (DataBase2JB_ZZCQ.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase2JB_ZZCQ where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString()) > 0)
                                    {
                                        btnSearch.PerformClick();
                                        MessageBox.Show("全部删除完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("删除失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase2JB_ZZCQ> frm = new FrmModify<DataBase2JB_ZZCQ>(DataBase2JB_ZZCQ, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }
        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase2JB_ZZCQ>().GetList().ToList();

            RefCmbUserCode(list);
            RefCmbUserName(list);
        }

        private void RefCmbUserCode(List<DataBase2JB_ZZCQ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2JB_ZZCQ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2JB_ZZCQ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2JB_ZZCQ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[10].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "cTime$cUser$年$月$序号$岗位名称$人员编码$姓名$天数$个人计件".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private bool Recount(List<DataBase2JB_ZZCQ> list)
        {
            try
            {
                var bzjj = new BaseDal<DataBase2JB_BZJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
                DataBase2JB_BZJJ sum = new DataBase2JB_BZJJ();
                if (bzjj == null || bzjj.Count == 0)
                {
                    MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的组装计件提报数据，请先导入数据后，【重新计算】！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
                else
                {
                    sum = new DataBase2JB_BZJJ
                    {
                        JJJE = bzjj.Sum(t => decimal.TryParse(t.JJJE, out decimal d) ? d : 0M).ToString()
                    };
                }
                DataBase2JB_ZZCQ sumCQ = new DataBase2JB_ZZCQ
                {
                    CQTS = list.Sum(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M).ToString()
                };
                foreach (var item in list)
                {
                    decimal.TryParse(sum.JJJE, out decimal jjje);
                    decimal.TryParse(sumCQ.CQTS, out decimal cqhj);
                    decimal.TryParse(item.CQTS, out decimal cq);
                    item.GRJJ = (jjje / cqhj * cq).ToString();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}