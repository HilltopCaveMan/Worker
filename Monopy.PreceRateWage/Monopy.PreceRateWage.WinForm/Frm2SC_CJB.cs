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
    public partial class Frm2SC_CJB : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$工种$工号$人员编号$姓名$装窑工（大）计件$装窑工（大）考核$实出勤$应出勤".Split('$');
        public Frm2SC_CJB()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2SC_CJB_Load(object sender, EventArgs e)
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
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——烧成——车间报.xlsx");
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
                List<DataBase2SC_CJB> list = dgv.DataSource as List<DataBase2SC_CJB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2SC_CJB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——烧成——车间报.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2SC_CJB> list = new BaseDal<DataBase2SC_CJB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2SC_CJB>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2SC_CJB DataBase2SC_CJB)
                {
                    if (DataBase2SC_CJB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2SC_CJB> frm = new FrmModify<DataBase2SC_CJB>(DataBase2SC_CJB, header, OptionType.Modify, Text, 5, 2);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUI();
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
                var DataBase2SC_CJB = dgv.SelectedRows[0].DataBoundItem as DataBase2SC_CJB;
                if (DataBase2SC_CJB.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2SC_CJB != null)
                    {
                        FrmModify<DataBase2SC_CJB> frm = new FrmModify<DataBase2SC_CJB>(DataBase2SC_CJB, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnRecount.PerformClick();
                            InitUI();
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
                var list = dgv.DataSource as List<DataBase2SC_CJB>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2SC_CJB>().Delete(item);
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
            var list = new BaseDal<DataBase2SC_CJB>().GetList().ToList();

            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbGH(list);
        }
        private void RefCmbUserCode(List<DataBase2SC_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2SC_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbGH(List<DataBase2SC_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGH.DataSource = listTmp;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2SC_CJB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (gh == "全部" ? true : t.GH.Contains(gh))).ToList().OrderBy(t => Convert.ToInt32(t.No)).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2SC_CJB(datas));
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
            dgv.ClearSelection();
        }

        private void Import(string fileName)
        {
            List<DataBase2SC_CJB> list = new ExcelHelper<DataBase2SC_CJB>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
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

            if (new BaseDal<DataBase2SC_CJB>().Add(list) > 0)
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

        private bool Recount(List<DataBase2SC_CJB> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("二厂") && t.Dept.Contains("烧成")).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

                foreach (var item in list)
                {
                    var data = datas.Where(x => x.Position == item.GZ && x.UserCode == item.UserCode && x.UserName == item.UserName).FirstOrDefault();
                    if (data == null)
                    {
                        item.SCQ = "0";
                        item.YCQ = "0";

                        MessageBox.Show("没有【" + item.UserName + "】的出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        item.SCQ = data.DayScq;
                        item.YCQ = data.DayYcq;
                    }

                    DataBase2SC_ZYJJHKH zygd = new BaseDal<DataBase2SC_ZYJJHKH>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("G002") && t.Code == item.GH);
                    if (zygd == null)
                    {
                        item.ZYGDJJ = "0";
                        item.ZYGDKH = "0";
                        MessageBox.Show("没有【" + item.UserName + "】的装窑计件和考核数据，无法计算，导入数据后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        item.ZYGDJJ = zygd.JJ;
                        item.ZYGDKH = zygd.KH;
                    }

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
