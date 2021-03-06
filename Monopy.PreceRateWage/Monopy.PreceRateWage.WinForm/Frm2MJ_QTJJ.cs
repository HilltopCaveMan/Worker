﻿using DevComponents.DotNetBar;
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
    public partial class Frm2MJ_QTJJ : Office2007Form
    {
        string[] header = "创建日期$创建人$年$月$序号$岗位$人员编码$姓名$类别$品种名称$数量$备注$单价$金额合计".Split('$');

        public Frm2MJ_QTJJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2MJ_QTJJ_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbUserCode.Text, CmbUserName.Text, CmbLB.Text, CmbPZMC.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——模具——其他计件.xlsx");
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
                List<DataBase2MJ_QTJJ> list = dgv.DataSource as List<DataBase2MJ_QTJJ>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase2MJ_QTJJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——模具——其他计件.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2MJ_QTJJ> list = dgv.DataSource as List<DataBase2MJ_QTJJ>;
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
                new BaseDal<DataBase2MJ_QTJJ>().Edit(item);
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
                var DataBase2MJ_QTJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2MJ_QTJJ;
                if (DataBase2MJ_QTJJ != null)
                {
                    if (DataBase2MJ_QTJJ.GW == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2MJ_QTJJ> frm = new FrmModify<DataBase2MJ_QTJJ>(DataBase2MJ_QTJJ, header, OptionType.Modify, Text, 4, 2);
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
                var DataBase2MJ_QTJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2MJ_QTJJ;
                if (DataBase2MJ_QTJJ.GW == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2MJ_QTJJ != null)
                    {
                        FrmModify<DataBase2MJ_QTJJ> frm = new FrmModify<DataBase2MJ_QTJJ>(DataBase2MJ_QTJJ, header, OptionType.Delete, Text, 4);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
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
                var list = dgv.DataSource as List<DataBase2MJ_QTJJ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.GW != "合计")
                    {
                        new BaseDal<DataBase2MJ_QTJJ>().Delete(item);
                    }
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

        private void InitUI()
        {
            var list = new BaseDal<DataBase2MJ_QTJJ>().GetList().ToList();

            RefCmbLB(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbPZMC(list);
        }

        private void RefCmbLB(List<DataBase2MJ_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2MJ_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase2MJ_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbPZMC(List<DataBase2MJ_QTJJ> list)
        {
            var listTmp = list.GroupBy(t => t.PZMC).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbPZMC.DataSource = listTmp;
            CmbPZMC.DisplayMember = "PZMC";
            CmbPZMC.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string lb, string pzmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2MJ_QTJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (lb == "全部" ? true : t.LB == lb) && (pzmc == "全部" ? true : t.PZMC == pzmc)).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2MJ_QTJJ(datas));
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
            dgv.ContextMenuStrip = contextMenuStrip1;
            dgv.ClearSelection();

        }

        private void Import(string fileName)
        {
            List<DataBase2MJ_QTJJ> list = new ExcelHelper<DataBase2MJ_QTJJ>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
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

                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);
            if (boolOK && new BaseDal<DataBase2MJ_QTJJ>().Add(list) > 0)
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

        private bool Recount(List<DataBase2MJ_QTJJ> list)
        {
            try
            {
                foreach (var item in list)
                {
                    decimal.TryParse(item.SL, out decimal js);
                    var baseday = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "模具车间" && t.TypesName == item.PZMC && t.Classification == item.LB && t.PostName == item.GW);
                    if (baseday == null)
                    {
                        MessageBox.Show("指标数据没有导入，请联系相关人员先导入数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        item.DJ = 0.ToString();
                        item.JEHJ = 0.ToString();
                    }
                    else
                    {
                        item.DJ = baseday.UnitPrice;
                        decimal.TryParse(item.DJ, out decimal dj);
                        item.JEHJ = (dj * js).ToString();
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
