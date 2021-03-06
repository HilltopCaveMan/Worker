﻿using Dapper;
using DevComponents.DotNetBar;
using HH.CS.Com;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3JB_CJJJ : Office2007Form
    {
        public Frm3JB_CJJJ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                return;
            }
            System.Reflection.FieldInfo[] fieldInfo = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ContextMenuStrip> contextMenuStrip = new List<ContextMenuStrip>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfo[i].FieldType == typeof(ButtonX))
                {
                    ((ButtonX)fieldInfo[i].GetValue(this)).Enabled = false;
                    //Controls.Find(fieldInfo[i].Name, true)[0].Enabled = false;
                    continue;
                }

                if (fieldInfo[i].FieldType == typeof(ContextMenuStrip))
                {
                    var tmp = (ContextMenuStrip)fieldInfo[i].GetValue(this);
                    contextMenuStrip.Add(tmp);
                    foreach (var item in tmp.Items)
                    {
                        if (item is ToolStripMenuItem t)
                        {
                            t.Enabled = false;
                        }
                    }
                }
            }

            var dic = MenuBarHelper.FrmControlEnable(Program.User.Code);
            foreach (var item in dic.Keys)
            {
                var cons = Controls.Find(item, true);
                if (cons.Length > 0)
                {
                    cons[0].Enabled = dic[item];
                    continue;
                }
                foreach (var contextMs in contextMenuStrip)
                {
                    foreach (var item2 in contextMs.Items)
                    {
                        if (item2 is ToolStripMenuItem x && x.Name == item)
                        {
                            x.Enabled = dic[item];
                        }
                    }
                }
            }
        }

        private void Frm3JB_CJJJ_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private string GetSumValue(DataTable dt, int column)
        {
            double t = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i][column].ToString()))
                {
                    if (dt.Rows[i][column].GetType() == Guid.NewGuid().GetType())
                    {
                        continue;
                    }
                    t += Convert.ToDouble(dt.Rows[i][column].ToString());
                }
            }
            return t.ToString();
        }

        private void RefDgv(DateTime selectTime)
        {
            SqlParameter[] paras = {
            new SqlParameter("@TheYear", selectTime.Year.ToString()),
            new SqlParameter("@TheMonth", selectTime.Month.ToString()) };
            DataTable dt;
            using (DataHelper dh = new DataHelper(DataHelper.DataType.Sqlclient, ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                dt = dh.ExecuteQuery("SelectCJJJ", paras, CommandType.StoredProcedure);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = Guid.NewGuid();
                dr[1] = Program.NowTime;
                dr[3] = dt.Rows[0][3];
                dr[4] = dt.Rows[0][4];
                dr[5] = dt.Rows[0][5];
                dr[8] = "合计";
                //dr[11] = dt.Columns[11].Expression.Sum(t => string.IsNullOrEmpty(t.ToString()) ? 0 : Convert.ToDecimal(t)).ToString();
                for (int i = 10; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[0][i].GetType() == Guid.NewGuid().GetType())
                    {
                        continue;
                    }

                    dr[i] = GetSumValue(dt, i);
                }
                dt.Rows.InsertAt(dr, 0);
                dgv.DataSource = dt;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[1][i].GetType() == Guid.NewGuid().GetType())
                    {
                        dgv.Columns[i].Visible = false;
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    dgv.Columns[i].Visible = false;
                }
                //dgv.Columns[2].Visible = false;
                //dgv.Columns[3].HeaderText = "年";
                //dgv.Columns[4].HeaderText = "月";
                //dgv.Columns[5].HeaderText = "工厂";
                //dgv.Columns[6].HeaderText = "工号";
                //dgv.Columns[7].HeaderText = "员工编码";
                //dgv.Columns[8].HeaderText = "姓名";
            }
            else
            {
                dgv.DataSource = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

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
                    List<DataBaseDay> listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.PostName == "成检").ToList();
                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        List<string> titles = new List<string>();//顺序
                        List<string> headers = new List<string>();//表头
                        IRow irow = sheet.GetRow(2);
                        for (int i = 4; i < irow.LastCellNum; i++)
                        {
                            string tmp = ExcelHelper.GetCellValue(sheet.GetRow(1).GetCell(i));
                            if (tmp == string.Empty)
                            {
                                tmp = titles.Last();
                            }
                            titles.Add(tmp);
                            string tmpHeader = ExcelHelper.GetCellValue(irow.GetCell(i));
                            if (tmp == "配盖")
                            {
                                if (tmpHeader == "连体类")
                                {
                                    tmpHeader = "连体盖";
                                }
                                if (tmpHeader == "水箱类")
                                {
                                    tmpHeader = "盖";
                                }
                            }
                            headers.Add(tmpHeader);
                        }
                        List<DataBase3JB_CJJJ> list = new List<DataBase3JB_CJJJ>();
                        for (int i = 3; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            string emp = ExcelHelper.GetCellValue(row.GetCell(0));
                            if (string.IsNullOrEmpty(emp))
                            {
                                continue;
                            }
                            else
                            {
                                DataBase3JB_CJJJ cjjj = new DataBase3JB_CJJJ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, FactoryNo = ExcelHelper.GetCellValue(row.GetCell(0)), Code = ExcelHelper.GetCellValue(row.GetCell(1)), UserCode = ExcelHelper.GetCellValue(row.GetCell(2)), UserName = ExcelHelper.GetCellValue(row.GetCell(3)), Childs = new List<DataBase3JB_CJJJ_Child>(), Results = new List<DataBase3JB_CJJJ_Result>() };
                                if (cjjj.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(cjjj.UserCode, cjjj.UserName, out string userNameERP))
                                {
                                    MessageBox.Show("工号：【" + cjjj.UserCode + "】,姓名：【" + cjjj.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                for (int j = 4; j < row.LastCellNum; j++)
                                {
                                    string childCount = ExcelHelper.GetCellValue(row.GetCell(j));
                                    if (!string.IsNullOrEmpty(childCount))
                                    {
                                        var cf = cjjj.Childs.FirstOrDefault(t => t.ChildName == titles[j - 4] + "_" + headers[j - 4]);
                                        if (cf == null)
                                        {
                                            DataBase3JB_CJJJ_Child child = new DataBase3JB_CJJJ_Child
                                            {
                                                Id = Guid.NewGuid(),
                                                ChildName = titles[j - 4] + "_" + headers[j - 4],
                                                ChildCount = Convert.ToDouble(childCount)
                                            };
                                            cjjj.Childs.Add(child);
                                        }
                                        else
                                        {
                                            cf.ChildCount += Convert.ToDouble(childCount);
                                        }
                                    }
                                }
                                foreach (var item in cjjj.Childs)
                                {
                                    var h0 = cjjj.Results.FirstOrDefault(t => t.Name == item.ChildName.Split('_')[1]);
                                    if (h0 == null)
                                    {
                                        //insert
                                        var h1 = new DataBase3JB_CJJJ_Result { Id = Guid.NewGuid(), Name = item.ChildName.Split('_')[1], TheCount = cjjj.Childs.Where(t => t.ChildName == item.ChildName).Sum(t => t.ChildCount) };
                                        var hh1 = listDay.FirstOrDefault(t => t.TypesType == h1.Name);
                                        h1.Price = hh1 == null ? 0 : Convert.ToDouble(hh1.UnitPrice);
                                        h1.Money = h1.Price * h1.TheCount;
                                        cjjj.Results.Add(h1);
                                    }
                                    else
                                    {
                                        //update
                                        h0.TheCount += cjjj.Childs.Where(t => t.ChildName == item.ChildName).Sum(t => t.ChildCount);
                                        h0.Money = h0.Price * h0.TheCount;
                                    }
                                }
                                //连体盖-连体
                                var ltg = cjjj.Results.FirstOrDefault(t => t.Name == "连体盖");
                                var lt = cjjj.Results.FirstOrDefault(t => t.Name == "连体类");
                                var ltg_lt = new DataBase3JB_CJJJ_Result { Id = Guid.NewGuid(), Name = "连体盖-连体", Price = 0, Money = 0, TheCount = (ltg == null ? 0 : ltg.TheCount) - (lt == null ? 0 : lt.TheCount) };
                                cjjj.Results.Add(ltg_lt);
                                var sxl = cjjj.Results.FirstOrDefault(t => t.Name == "水箱类");
                                var g = cjjj.Results.FirstOrDefault(t => t.Name == "盖");
                                var hh = listDay.FirstOrDefault(t => t.TypesType == "水箱含盖类");
                                var sxhgl = new DataBase3JB_CJJJ_Result { Id = Guid.NewGuid(), Name = "水箱含盖类", Price = hh == null ? 0 : Convert.ToDouble(hh.UnitPrice), TheCount = ((sxl == null ? 0 : sxl.TheCount) + (g == null ? 0 : g.TheCount) + (ltg_lt == null ? 0 : ltg_lt.TheCount)) / 2 };
                                sxhgl.Money = sxhgl.Price * sxhgl.TheCount;
                                cjjj.Results.Add(sxhgl);

                                list.Add(cjjj);
                            }
                        }
                        new BaseDal<DataBase3JB_CJJJ>().Add(list);
                        btnSearch.PerformClick();
                        MessageBox.Show("导入完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导入失败!错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType == Guid.NewGuid().GetType())
                {
                    dt.Columns.Remove(dt.Columns[i]);
                }
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "梦牌三厂检包--成检计件" + dtp.Value.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "成检计件");
                try
                {
                    if (sfd.FileName.Substring(sfd.FileName.Length - 4) != ".xls")
                    {
                        sfd.FileName += ".xls";
                    }
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                    {
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        if (MessageBox.Show("马上打开？", "导出成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
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

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除" + dtp.Value.ToString("yyyy年MM月") + "的所有成检计件数据吗？删除不可恢复，请谨慎操作！！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    var listMain = conn.Query<DataBase3JB_CJJJ>("select * from DataBase3JB_CJJJ WHERE theyear=" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var transaction = conn.BeginTransaction();
                    foreach (var item in listMain)
                    {
                        string sql1 = "delete from DataBase3JB_CJJJ_Child where DataBase3JB_CJJJ_Id='" + item.Id + "'";
                        string sql2 = "delete from DataBase3JB_CJJJ_Result where DataBase3JB_CJJJ_Id='" + item.Id + "'";
                        string sql3 = "delete from DataBase3JB_CJJJ where id='" + item.Id + "'";
                        conn.Execute(sql1, null, transaction);
                        conn.Execute(sql2, null, transaction);
                        conn.Execute(sql3, null, transaction);
                    }
                    transaction.Commit();
                }
                RefDgv(dtp.Value);
            }
        }
    }
}