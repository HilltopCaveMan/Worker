﻿using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm2YL_XCYLTJ : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$品种$类别$单位$数量".Split('$');

        public Frm2YL_XCYLTJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2YL_XCYLTJ_Load(object sender, EventArgs e)
        {
            InitUI();
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
            RefDgv(dtp.Value, CmbLB.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——原料——原料统计.xlsx");
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
                    List<DataBase2YL_XCYLTJ> list = new List<DataBase2YL_XCYLTJ>();
                    DateTime dateTime = dtp.Value;
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            IRow rowFirst = sheet.GetRow(1);
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            DataBase2YL_XCYLTJ t = new DataBase2YL_XCYLTJ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
                            t.No = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.PZ = ExcelHelper.GetCellValue(row.GetCell(1));
                            if (string.IsNullOrEmpty(t.PZ))
                            {
                                continue;
                            }
                            t.LB = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.DW = ExcelHelper.GetCellValue(row.GetCell(3));
                            t.SL = ExcelHelper.GetCellValue(row.GetCell(4));
                            list.Add(t);
                        }
                    }
                    if (new BaseDal<DataBase2YL_XCYLTJ>().Add(list) > 0)
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
                List<DataBase2YL_XCYLTJ> list = dgv.DataSource as List<DataBase2YL_XCYLTJ>;
                if (new ExcelHelper<DataBase2YL_XCYLTJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——原料——原料统计.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2YL_XCYLTJ DataBase2YL_XCYLTJ)
                {
                    if (DataBase2YL_XCYLTJ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2YL_XCYLTJ> frm = new FrmModify<DataBase2YL_XCYLTJ>(DataBase2YL_XCYLTJ, header, OptionType.Modify, Text, 5, 0);
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
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {

                var DataBase2YL_XCYLTJ = dgv.SelectedRows[0].DataBoundItem as DataBase2YL_XCYLTJ;
                DateTime dateTime = Convert.ToDateTime(dtp.Value);
                if (DataBase2YL_XCYLTJ.No == "合计")
                {
                    if (MessageBox.Show("要删除" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            conn.Open();
                            //IDbTransaction dbTransaction = conn.BeginTransaction();
                            try
                            {
                                string sqlMain = "delete from DataBase2YL_XCYLTJ where theyear=" + dateTime.Year + " and themonth=" + dateTime.Month + " ";

                                conn.Execute(sqlMain, null, null);

                                //dbTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                //dbTransaction.Rollback();
                                MessageBox.Show("删除失败，请检查网络和操作！详细错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                //dbTransaction.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("要删除选中的记录吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (DataBase2YL_XCYLTJ != null)
                        {
                            FrmModify<DataBase2YL_XCYLTJ> frm = new FrmModify<DataBase2YL_XCYLTJ>(DataBase2YL_XCYLTJ, header, OptionType.Delete, Text, 5);
                            if (frm.ShowDialog() == DialogResult.Yes)
                            {
                                InitUI();

                            }
                        }
                    }
                }
                btnSearch.PerformClick();
            }
        }
        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase2YL_XCYLTJ>().GetList().ToList();

            RefCmbLB(list);
        }

        private void RefCmbLB(List<DataBase2YL_XCYLTJ> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string lb)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2YL_XCYLTJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2YL_XCYLTJ(datas));
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

        #endregion
    }
}
