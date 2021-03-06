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
    public partial class Frm2YL_JJTB : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$品种$类别$数量$单价$计件工资".Split('$');
        public Frm2YL_JJTB()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2YL_JJTB_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbPZ.Text, CmbLB.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——原料——计件提报.xlsx");
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
                List<DataBase2YL_JJTB> list = dgv.DataSource as List<DataBase2YL_JJTB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2YL_JJTB>().WriteExcle(Application.StartupPath + "\\Excel\\模板二厂——原料——计件提报.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2YL_JJTB DataBase2YL_JJTB)
                {
                    if (DataBase2YL_JJTB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2YL_JJTB> frm = new FrmModify<DataBase2YL_JJTB>(DataBase2YL_JJTB, header, OptionType.Modify, Text, 5);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUI();
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
                var DataBase2YL_JJTB = dgv.SelectedRows[0].DataBoundItem as DataBase2YL_JJTB;
                if (DataBase2YL_JJTB.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2YL_JJTB != null)
                    {
                        FrmModify<DataBase2YL_JJTB> frm = new FrmModify<DataBase2YL_JJTB>(DataBase2YL_JJTB, header, OptionType.Delete, Text, 5);
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
                var list = dgv.DataSource as List<DataBase2YL_JJTB>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2YL_JJTB>().Delete(item);
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
            var list = new BaseDal<DataBase2YL_JJTB>().GetList().ToList();

            RefCmbPZ(list);
            RefCmbLB(list);
        }
        private void RefCmbPZ(List<DataBase2YL_JJTB> list)
        {
            var listTmp = list.GroupBy(t => t.PZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbPZ.DataSource = listTmp;
            CmbPZ.DisplayMember = "PZ";
            CmbPZ.Text = "全部";
        }

        private void RefCmbLB(List<DataBase2YL_JJTB> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string pz, string lb)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2YL_JJTB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (pz == "全部" ? true : t.PZ == pz) && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2YL_JJTB(datas));
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
            List<DataBase2YL_JJTB> list = new ExcelHelper<DataBase2YL_JJTB>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].PZ))
                {
                    list.Remove(list[i]);
                    i--;
                    continue;
                }
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var check = Check(list);
            if (check && new BaseDal<DataBase2YL_JJTB>().Add(list) > 0)
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

        private bool Check(List<DataBase2YL_JJTB> list)
        {

            var listTJ = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "原料车间").ToList();
            if (listTJ == null || listTJ.Count == 0)
            {
                MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的指标数据，无法计算", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            foreach (var item in list)
            {
                var tmp = listTJ.Where(t => t.TypesName == item.PZ && t.TypesType == item.LB).FirstOrDefault();
                if (tmp == null)
                {
                    MessageBox.Show("没有" + item.PZ + "的指标数据，无法计算", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                decimal.TryParse(tmp.UnitPrice, out decimal price);
                decimal.TryParse(item.DJ, out decimal dj);
                if (price != dj)
                {
                    MessageBox.Show(item.PZ + "的单价:" + item.DJ + "，指标数据的单价:" + tmp.UnitPrice + "，单价不相等，导入失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
