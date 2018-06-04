using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm1JB_KFSS : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$车间$人员编号$姓名$类别$数量（件）$单价$金额".Split('$');
        private string _dept;
        public Frm1JB_KFSS()
        {
            InitializeComponent();
        }
        //public Frm1JB_KFSS(string args) : this()
        //{
        //    try
        //    {

        //        _dept = args.Split('-')[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
        //        return;
        //    }
        //}

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
            RefDgv(dtp.Value, CmbLine.Text, CmbUserCode.Text, CmbUserName.Text, CmbLB.Text);
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
                List<DataBase1JB_KFSS> list = dgv.DataSource as List<DataBase1JB_KFSS>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase1JB_KFSS>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——原料——计件明细表.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase1JB_KFSS> list = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase1JB_KFSS>().Edit(item);
            }

            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1JB_KFSS DataBase1JB_KFSS)
                {
                    if (DataBase1JB_KFSS.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1JB_KFSS> frm = new FrmModify<DataBase1JB_KFSS>(DataBase1JB_KFSS, header, OptionType.Modify, Text, 5, 3);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUI();
                        btnRecount.PerformClick();
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
                var DataBase1JB_KFSS = dgv.SelectedRows[0].DataBoundItem as DataBase1JB_KFSS;
                if (DataBase1JB_KFSS.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1JB_KFSS != null)
                    {
                        FrmModify<DataBase1JB_KFSS> frm = new FrmModify<DataBase1JB_KFSS>(DataBase1JB_KFSS, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnRecount.PerformClick();
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
                var list = dgv.DataSource as List<DataBase1JB_KFSS>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        continue;
                    }
                    new BaseDal<DataBase1JB_KFSS>().Delete(item);
                }
                btnRecount.PerformClick();
                return;
            }
            else
            {
                return;
            }
        }

        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase1JB_KFSS>().GetList().ToList();

            RefCmbLine(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private void RefCmbLine(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.CJ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLine.DataSource = listTmp;
            CmbLine.DisplayMember = "CJ";
            CmbLine.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbLB(List<DataBase1JB_KFSS> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string cj, string userCode, string userName, string lb)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (cj == "全部" ? true : t.CJ.Contains(cj)) && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1JB_KFSS(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
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

        private void Import(string fileName)
        {
            List<DataBase1JB_KFSS> list = new ExcelHelper<DataBase1JB_KFSS>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
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
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);
            if (boolOK && new BaseDal<DataBase1JB_KFSS>().Add(list) > 0)
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

        private bool Recount(List<DataBase1JB_KFSS> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var listTJ = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G001" && t.WorkshopName == "检包车间" && t.Classification == "磨瓷");

                foreach (var item in list)
                {
                    var type = listTJ.Where(t => t.TypesType == item.LB).FirstOrDefault();
                    item.DJ = type.UnitPrice;
                    decimal.TryParse(item.DJ, out decimal dj);
                    decimal.TryParse(item.SL, out decimal sl);
                    item.JJJE = (dj * sl).ToString();
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
