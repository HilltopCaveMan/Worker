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
    public partial class Frm2CX_QTJJ : Office2007Form
    {
        string[] header = "创建日期$创建人$年$月$序号$岗位$工号$人员编码$姓名$类别$产品名称$件数$单价$金额".Split('$');

        public Frm2CX_QTJJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2CX_QTJJ_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbUserCode.Text, CmbUserName.Text, CmbGH.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——成型——其他计件.xlsx");
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
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase2CX_QTJJ> list = dgv.DataSource as List<DataBase2CX_QTJJ>;
                if (new ExcelHelper<DataBase2CX_QTJJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——成型——其他计件.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2CX_QTJJ> list = dgv.DataSource as List<DataBase2CX_QTJJ>;
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
                new BaseDal<DataBase2CX_QTJJ>().Edit(item);
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
                var DataBase2CX_QTJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_QTJJ;
                if (DataBase2CX_QTJJ != null)
                {
                    if (DataBase2CX_QTJJ.UserName == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2CX_QTJJ> frm = new FrmModify<DataBase2CX_QTJJ>(DataBase2CX_QTJJ, header, OptionType.Modify, Text, 4, 2);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
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
                var DataBase2CX_QTJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_QTJJ;
                if (DataBase2CX_QTJJ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2CX_QTJJ != null)
                    {
                        FrmModify<DataBase2CX_QTJJ> frm = new FrmModify<DataBase2CX_QTJJ>(DataBase2CX_QTJJ, header, OptionType.Delete, Text, 4, 2);
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
                var list = dgv.DataSource as List<DataBase2CX_QTJJ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2CX_QTJJ>().Delete(item);
                    }
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
            var list = new BaseDal<DataBase2CX_QTJJ>().GetList().ToList();

            RefCmbGH(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);

        }

        private void RefCmbGH(List<DataBase2CX_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGH.DataSource = listTmp;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2CX_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase2CX_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2CX_QTJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (gh == "全部" ? true : t.GH == gh)).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2CX_QTJJ(datas));
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
            dgv.ContextMenuStrip = contextMenuStrip1;
            dgv.ClearSelection();

        }

        private void Import(string fileName)
        {
            List<DataBase2CX_QTJJ> list = new ExcelHelper<DataBase2CX_QTJJ>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
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
                list[i].No = (i + 1).ToString();
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);
            if (boolOK && new BaseDal<DataBase2CX_QTJJ>().Add(list) > 0)
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

        private bool Recount(List<DataBase2CX_QTJJ> list)
        {
            try
            {
                foreach (var item in list)
                {
                    decimal.TryParse(item.JS, out decimal js);
                    var baseday = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G001" && t.WorkshopName == "成型车间" && t.TypesName == item.CPMC && t.Classification == item.LB);
                    if (baseday == null)
                    {
                        item.DJ = 0.ToString();
                        item.JE = 0.ToString();
                    }
                    else
                    {
                        item.DJ = baseday.UnitPrice;
                        decimal.TryParse(item.DJ, out decimal dj);
                        item.JE = (dj * js).ToString();
                    }

                    var pmcjp = new BaseDal<DataBase2CX_PMCJP>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.UserCode == item.UserCode);
                    if (pmcjp == null)
                    {
                        MessageBox.Show("请先导入精坯月报数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    bool sign = false;
                    if (item.LB == "青坯计件")
                    {
                        foreach (var chiid in pmcjp.Childs)
                        {
                            if (chiid.CPMC == item.CPMC)
                            {
                                sign = true;
                                decimal.TryParse(chiid.Count, out decimal count);

                                if (js != count)
                                {
                                    MessageBox.Show("姓名：【" + item.UserName + "】,品种名称：【" + item.CPMC + "】的数量为【" + item.JS + "】超过精坯月报对应的数量，精坯月报中【" + item.CPMC + "】的数量为【" + chiid.Count + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                            }
                        }
                        if (sign)
                        {
                            MessageBox.Show("精坯月报中没有姓名：【" + item.UserName + "】，品种名称：【" + item.CPMC + "】的记录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
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
