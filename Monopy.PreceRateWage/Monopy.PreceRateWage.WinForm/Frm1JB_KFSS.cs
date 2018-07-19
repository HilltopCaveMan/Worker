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
    public partial class Frm1JB_KFSS : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;
        private string _factoryNo;
        public Frm1JB_KFSS()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        public Frm1JB_KFSS(string args) : this()
        {
            try
            {
                _factoryNo = args;

            }
            catch (Exception ex)
            {
                MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
                return;
            }
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1JB_KFSS_Load(object sender, EventArgs e)
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
            RefDgv(selectTime, CmbUserCode.Text, CmbUserName.Text, CmbLB.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——检包——开发试烧.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase1JB_KFSS> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                list = new ExcelHelper<DataBase1JB_KFSS>().ReadExcel(openFileDlg.FileName, 2, 6);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Enabled = false;
                for (int i = 0; i < list.Count; i++)
                {

                    if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                    {
                        list.RemoveAt(i);
                        if (i > 0)
                        {
                            i--;
                        }
                        else
                        {
                            i = -1;
                        }
                        continue;

                    }


                    if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }
                    list[i].Factory = _factoryNo;
                    list[i].CreateUser = Program.User.ToString();
                    list[i].CreateTime = Program.NowTime;
                    list[i].TheYear = selectTime.Year;
                    list[i].TheMonth = selectTime.Month;
                }
                if (Recount(list) && new BaseDal<DataBase1JB_KFSS>().Add(list) > 0)
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
                List<DataBase1JB_KFSS> list = dgv.DataSource as List<DataBase1JB_KFSS>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase1JB_KFSS>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——检包——开发试烧.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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
            DataBase1JB_KFSS DataBase1JB_KFSS = new DataBase1JB_KFSS() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month, Factory = _factoryNo };
            FrmModify<DataBase1JB_KFSS> frm = new FrmModify<DataBase1JB_KFSS>(DataBase1JB_KFSS, header, OptionType.Add, Text, 5, 2);
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
            List<DataBase1JB_KFSS> list = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase1JB_KFSS>().Edit(item);
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
                var DataBase1JB_KFSS = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_KFSS;
                if (DataBase1JB_KFSS != null)
                {
                    if (DataBase1JB_KFSS.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1JB_KFSS> frm = new FrmModify<DataBase1JB_KFSS>(DataBase1JB_KFSS, header, OptionType.Modify, Text, 5, 2);
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
                var DataBase1JB_KFSS = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_KFSS;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1JB_KFSS != null)
                    {
                        if (DataBase1JB_KFSS.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase1JB_KFSS where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString() + " and Factory='" + _factoryNo + "'") > 0)
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
                        FrmModify<DataBase1JB_KFSS> frm = new FrmModify<DataBase1JB_KFSS>(DataBase1JB_KFSS, header, OptionType.Delete, Text, 5);
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

        private void RefCmbUserCode(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void RefCmbLB(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.Factory == _factoryNo).ToList();
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string lb)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.Factory == _factoryNo && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode.Contains(userCode)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1JB_KFSS(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[10].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "cTime$cUser$年$月$工厂$序号$车间$人员编码$姓名$类别$数量（件）$单价$金额".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private bool Recount(List<DataBase1JB_KFSS> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var bDayMc = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.WorkshopName == "检包车间" && t.Classification == "磨瓷" && t.TypesType == item.LB);

                    item.DJ = bDayMc == null ? "0" : bDayMc.UnitPrice;

                    item.JE = string.IsNullOrEmpty(item.DJ) ? string.Empty : (Convert.ToDecimal(item.DJ) * Convert.ToDecimal(item.Count)).ToString();

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