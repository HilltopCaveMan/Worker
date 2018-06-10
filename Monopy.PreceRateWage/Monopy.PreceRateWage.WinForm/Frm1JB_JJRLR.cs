using Dapper;
using DevComponents.DotNetBar;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm1JB_JJRLR : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        private Thread threadSh;

        public Frm1JB_JJRLR()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void CheckSh()
        {
            Dictionary<object, InvokeData> dic = new Dictionary<object, InvokeData>();
            while (true)
            {
                dic.Clear();
                var theOne = new BaseDal<DataBase1JB_JJRLR>().Get(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month);
                if (theOne != null)
                {
                    if (isBtnPmcEnable)
                    {
                        if (theOne.IsPMC_Check)
                        {
                            //已审核
                            dic.Add(BtnPMCCheckYes, new InvokeData(this) { Text = "PMC确认", Enable = false });
                            dic.Add(BtnPMCCheckNo, new InvokeData(this) { Text = "PMC退回", Enable = false });
                        }
                        else
                        {
                            dic.Add(BtnPMCCheckYes, new InvokeData(this) { Text = "PMC确认", Enable = true });
                            dic.Add(BtnPMCCheckNo, new InvokeData(this) { Text = "PMC退回", Enable = true });
                        }
                    }
                    if (theOne.PMC_Time != null)
                    {
                        dic.Add(lblPMC, new InvokeData(this) { Text = theOne.IsPMC_Check ? "PMC审核" : "PMC退回", BackColor = theOne.IsPMC_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblPMC, new InvokeData(this) { Text = "PMC未审核", BackColor = Color.Transparent });
                    }

                    if (isBtnPgEnable)
                    {
                        if (theOne.IsPG_Check)
                        {
                            dic.Add(BtnPGCheckYes, new InvokeData(this) { Text = "品管确认", Enable = false });
                            dic.Add(BtnPGCheckNo, new InvokeData(this) { Text = "品管退回", Enable = false });
                        }
                        else
                        {
                            dic.Add(BtnPGCheckYes, new InvokeData(this) { Text = "品管确认", Enable = true });
                            dic.Add(BtnPGCheckNo, new InvokeData(this) { Text = "品管退回", Enable = true });
                        }
                    }
                    if (theOne.PG_Time != null)
                    {
                        dic.Add(lblPG, new InvokeData(this) { Text = theOne.IsPG_Check ? "品管审核" : "品管退回", BackColor = theOne.IsPG_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblPG, new InvokeData(this) { Text = "品管未审核", BackColor = Color.Transparent });
                    }

                    if (isBtnKfEnable)
                    {
                        if (theOne.IsKF_Check)
                        {
                            dic.Add(BtnKFCheckYes, new InvokeData(this) { Text = "开发确认", Enable = false });
                            dic.Add(BtnKFCheckNo, new InvokeData(this) { Text = "开发退回", Enable = false });
                        }
                        else
                        {
                            dic.Add(BtnKFCheckYes, new InvokeData(this) { Text = "开发确认", Enable = true });
                            dic.Add(BtnKFCheckNo, new InvokeData(this) { Text = "开发退回", Enable = true });
                        }
                    }
                    if (theOne.KF_Time != null)
                    {
                        dic.Add(lblKF, new InvokeData(this) { Text = theOne.IsKF_Check ? "开发审核" : "开发退回", BackColor = theOne.IsKF_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblKF, new InvokeData(this) { Text = "开发未审核", BackColor = Color.Transparent });
                    }

                    //if (isBtnWxEnable)
                    //{
                    //    if (theOne.IsWX_Check)
                    //    {
                    //        dic.Add(BtnWXCheckYes, new InvokeData(this) { Text = "外销确认", Enable = false });
                    //        dic.Add(BtnWXCheckNo, new InvokeData(this) { Text = "外销退回", Enable = false });
                    //    }
                    //    else
                    //    {
                    //        dic.Add(BtnWXCheckYes, new InvokeData(this) { Text = "外销确认", Enable = true });
                    //        dic.Add(BtnWXCheckNo, new InvokeData(this) { Text = "外销退回", Enable = true });
                    //    }
                    //}
                    //if (theOne.WX_Time != null)
                    //{
                    //    dic.Add(lblWX, new InvokeData(this) { Text = theOne.IsWX_Check ? "外销审核" : "外销退回", BackColor = theOne.IsWX_Check ? Color.Lime : Color.Red });
                    //}
                    //else
                    //{
                    //    dic.Add(lblWX, new InvokeData(this) { Text = "外销未审核", BackColor = Color.Transparent });
                    //}

                    //if (isBtnPmcDdEnable)
                    //{
                    //    if (theOne.IsPMCDD_Check)
                    //    {
                    //        dic.Add(BtnPMCDDCheckYes, new InvokeData(this) { Text = "PMC调度确认", Enable = false });
                    //        dic.Add(BtnPMCDDCheckNo, new InvokeData(this) { Text = "PMC调度退回", Enable = false });
                    //    }
                    //    else
                    //    {
                    //        dic.Add(BtnPMCDDCheckYes, new InvokeData(this) { Text = "PMC调度确认", Enable = true });
                    //        dic.Add(BtnPMCDDCheckNo, new InvokeData(this) { Text = "PMC调度退回", Enable = true });
                    //    }
                    //}
                    //if (theOne.PMCDD_Time != null)
                    //{
                    //    dic.Add(lblPMCDD, new InvokeData(this) { Text = theOne.IsPMCDD_Check ? "PMC调度审核" : "PMC调度退回", BackColor = theOne.IsPMCDD_Check ? Color.Lime : Color.Red });
                    //}
                    //else
                    //{
                    //    dic.Add(lblPMCDD, new InvokeData(this) { Text = "PMC调度未审核", BackColor = Color.Transparent });
                    //}
                }
                if (dic.Count > 0)
                {
                    InvokeHelper.SetInvoke(dic);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 界面控制/数据导入，修改；审核；审核数据修改；
        /// 放到后面做和权限控制一起做吧！
        /// </summary>
        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                isBtnPmcEnable = true;
                isBtnPgEnable = true;
                isBtnKfEnable = true;
                //isBtnWxEnable = true;
                //isBtnPmcDdEnable = true;
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
                    if (item.Contains("BtnPMCCheckYes") && dic[item])
                    {
                        isBtnPmcEnable = true;
                    }
                    if (item.Contains("BtnPGCheckYes") && dic[item])
                    {
                        isBtnPgEnable = true;
                    }
                    if (item.Contains("BtnKFCheckYes") && dic[item])
                    {
                        isBtnKfEnable = true;
                    }
                    //if (item.Contains("BtnWXCheckYes") && dic[item])
                    //{
                    //    isBtnWxEnable = true;
                    //}
                    //if (item.Contains("BtnPMCDDCheckYes") && dic[item])
                    //{
                    //    isBtnPmcDdEnable = true;
                    //}
                    if (item == "btnSearch")
                    {
                        buttonX1.Enabled = dic[item];
                        buttonX2.Enabled = dic[item];
                    }
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

        #region 界面控制变量

        private bool isBtnPmcEnable = false;
        private bool isBtnPgEnable = false;
        private bool isBtnKfEnable = false;
        //private bool isBtnWxEnable = false;
        //private bool isBtnPmcDdEnable = false;

        #endregion 界面控制变量

        private void Frm1JB_JJRLR_Load(object sender, EventArgs e)
        {
            dtp.text = Program.NowTime.ToString("yyyy-M-d");
            txtUserCode.Text = string.Empty;
            btnSearch.PerformClick();
            UiControl();
            threadSh = new Thread(CheckSh)
            {
                IsBackground = true
            };
            threadSh.Start();
        }

        private void RefGrid(DateTime selectTime, string userCode, string userName, bool sign)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            dgv.DataSource = null;
            List<DataBase1JB_JJRLR> list = new List<DataBase1JB_JJRLR>();

            if (sign)
            {//日查询
                list = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            }
            else
            {//月查询
                list = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            }

            if (list.Count == 0)
            {
                return;
            }
            DataTable dt = DataTableHelper.GetDataTable(list);
            dt.Columns.Remove("Childs");
            dt.Columns.Remove("CreateTime");
            dt.Columns.Remove("CreateUser");
            dt.Columns.Remove("ModifyTime");
            dt.Columns.Remove("ModifyUser");
            dt.Columns.Remove("IsPMC_Check");
            dt.Columns.Remove("PMC_Time");
            dt.Columns.Remove("PMC_User");
            dt.Columns.Remove("PMC_Manager_Time");
            dt.Columns.Remove("PMC_Manager_User");
            dt.Columns.Remove("IsPMC_Manager");
            dt.Columns.Remove("IsPG_Check");
            dt.Columns.Remove("PG_Time");
            dt.Columns.Remove("PG_User");
            dt.Columns.Remove("PG_Manager_Time");
            dt.Columns.Remove("PG_Manager_User");
            dt.Columns.Remove("IsPG_Manager");
            dt.Columns.Remove("IsKF_Check");
            dt.Columns.Remove("KF_Time");
            dt.Columns.Remove("KF_User");
            dt.Columns.Remove("KF_Manager_Time");
            dt.Columns.Remove("KF_Manager_User");
            dt.Columns.Remove("IsKF_Manager");

            DataRow dj = dt.NewRow();
            dj["UserName"] = "单价";
            dt.Rows.InsertAt(dj, 0);
            for (int i = 0; i < list.Count; i++)
            {
                foreach (var item in list[i].Childs.OrderBy(t => t.No))
                {
                    string cpmc = item.CPMC;
                    if (!dt.Columns.Contains(cpmc))
                    {
                        dt.Columns.Add(cpmc, typeof(string));
                    }
                    dt.Rows[i + 1][cpmc] = item.Count;
                    dt.Rows[0][cpmc] = item.Price;
                }
            }
            DataRow dr_sum = dt.NewRow();

            decimal[] tp = new decimal[dt.Columns.Count - 5];
            bool result = true;
            decimal totalCount = 0;
            for (int j = 5; j < dt.Columns.Count; j++)
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    if (result)
                    {
                        totalCount += (decimal.TryParse(dt.Rows[i][4].ToString(), out decimal t) ? t : 0M);
                    }
                    tp[j - 5] += (decimal.TryParse(dt.Rows[i][j].ToString(), out decimal m) ? m : 0M);
                }
                result = false;
                dr_sum[j] = tp[j - 5];
            }
            dr_sum[4] = totalCount;
            dr_sum["No"] = string.Empty;
            dr_sum["GW"] = string.Empty;
            dr_sum["UserCode"] = string.Empty;
            dr_sum["UserName"] = "合计";
            dt.Rows.InsertAt(dr_sum, 1);
            string[] header = "ID$TheYear$TheMonth$TheDay$金额$序号$岗位$工号$姓名".Split('$');
            dgv.DataSource = dt;
            for (int i = 0; i < header.Length; i++)
            {
                if (i < 4)
                {
                    dgv.Columns[i].Visible = false;
                }
                else
                {
                    dgv.Columns[i].HeaderText = header[i];
                }
            }
            dgv.Columns[4].Frozen = true;
            for (int i = 0; i < 2; i++)
            {
                dgv.Rows[i].Frozen = true;
                if (i == 1)
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                }
                else
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.LimeGreen;
                    dgv.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Blue;
                }
            }
            dgv.ClearSelection();
        }

        private bool CheckTime(string strTime)
        {
            if (string.IsNullOrEmpty(strTime) || !DateTime.TryParse(strTime, out selectTime))
            {
                MessageBox.Show("请选择日期，或日期错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp.Focus();
                return false;
            }
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            RefGrid(selectTime, txtUserCode.Text, txtUserName.Text, true);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——检包——计件日录入.xlsx");
        }

        private bool Recount(List<DataBase1JB_JJRLR> list)
        {
            try
            {
                foreach (var item in list)
                {
                    decimal totalCount = 0;
                    foreach (var child in item.Childs)
                    {
                        DataBaseDay dataBaseDay_01 = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == item.TheYear && t.CreateMonth == item.TheMonth && t.FactoryNo == "G001" && t.WorkshopName == "检包车间" && t.Classification == "线外计件" && t.TypesName == child.CPMC);
                        decimal.TryParse(dataBaseDay_01.UnitPrice, out decimal price);
                        decimal.TryParse(child.Count, out decimal count);
                        totalCount += price * count;
                        child.Price = dataBaseDay_01.UnitPrice;
                    }
                    item.JE = totalCount.ToString();
                }
            }
            catch
            {
                MessageBox.Show("请检查导入的Excel文件，有数量但是基础数据没有单价！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            List<DataBase1JB_JJRLR> list = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            if (list.Count > 0 && (list[0].IsKF_Manager || list[0].IsPG_Manager || list[0].IsPMC_Manager))
            {
                MessageBox.Show("错误：已经送审或有部门审核，不能【导入】！", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<DataBase1JB_JJRLR> listCount = new List<DataBase1JB_JJRLR>();

                    using (FileStream fs = new FileStream(openFileDlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;
                        workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int i, j;
                        for (i = 2; i <= sheet.LastRowNum; i++)
                        {
                            IRow rowFirst = sheet.GetRow(1);
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }
                            DataBase1JB_JJRLR t = new DataBase1JB_JJRLR { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month, TheDay = selectTime.Day };
                            t.No = ExcelHelper.GetCellValue(row.GetCell(0));
                            t.GW = ExcelHelper.GetCellValue(row.GetCell(1));
                            t.UserCode = ExcelHelper.GetCellValue(row.GetCell(2));
                            t.UserName = ExcelHelper.GetCellValue(row.GetCell(3));
                            if (string.IsNullOrEmpty(t.UserCode) || string.IsNullOrEmpty(t.UserName))
                            {
                                continue;
                            }
                            if (!MyDal.IsUserCodeAndNameOK(t.UserCode, t.UserName, out string userNameERP))
                            {
                                MessageBox.Show("工号：【" + t.UserCode + "】,姓名：【" + t.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            t.Childs = new List<DataBase1JB_JJRLR_Child>();
                            for (j = 4; j < row.LastCellNum; j++)
                            {
                                var x = new DataBase1JB_JJRLR_Child { Id = Guid.NewGuid() };
                                string cpmc = ExcelHelper.GetCellValue(rowFirst.GetCell(j));
                                x.No = j - 4;
                                x.CPMC = cpmc;
                                x.Count = ExcelHelper.GetCellValue(row.GetCell(j));
                                if (!string.IsNullOrEmpty(x.Count) && !string.IsNullOrEmpty(cpmc))
                                {
                                    t.Childs.Add(x);
                                }
                            }
                            listCount.Add(t);
                        }
                    }
                    if (Recount(listCount) && new BaseDal<DataBase1JB_JJRLR>().Add(listCount) > 0)
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            btnSearch.PerformClick();
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //                  0      1    2          3    4     5    6     7   8     9   
            string[] header = "ID$TheYear$TheMonth$TheDay$金额$序号$岗位$工号$姓名".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }
            
            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);
            dt.Columns.RemoveAt(0);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "梦牌一厂检包车间--线外计件-" + selectTime.ToLongDateString().ToString() + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "线外计件", "一工厂" + selectTime.ToLongDateString().ToString() + "线外计件明细");
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


        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase1JB_JJRLR> list = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day).ToList();
            if (list.Count > 0 && (list[0].IsKF_Manager || list[0].IsPG_Manager || list[0].IsPMC_Manager))
            {
                MessageBox.Show("错误：已经送审或有部门审核，不能【重新计算】？", "禁止操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            Enabled = false;
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase1JB_JJRLR>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var id = dgv.SelectedRows[0].Cells["id"].Value.ToString();

               
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {

                    if (string.IsNullOrEmpty(id))
                    {
                        if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                conn.Open();
                                var list = conn.Query<DataBase1JB_JJRLR>("select * from DataBase1JB_JJRLR where theyear=" + selectTime.Year + " and themonth=" + selectTime.Month + " and TheDay=" + selectTime.Day);
                                IDbTransaction dbTransaction = conn.BeginTransaction();
                                foreach (var item in list)
                                {
                                    if (item.IsKF_Manager || item.IsPG_Manager || item.IsPMC_Manager)
                                    {
                                        MessageBox.Show("错误：已经送审或有部门审核，不能删除？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                }
                                try
                                {
                                    string sqlMain = "delete from DataBase1JB_JJRLR where id=@id";
                                    string sqlChild = "delete from DataBase1JB_JJRLR_Child where DataBase1JB_JJRLR_Id=@id";
                                    foreach (var item in list)
                                    {
                                        conn.Execute(sqlChild, new { id = item.ID.ToString() }, dbTransaction, null, null);
                                        conn.Execute(sqlMain, new { id = item.ID.ToString() }, dbTransaction, null, null);

                                    }
                                    dbTransaction.Commit();
                                    Frm1JB_JJRLR_Load(null, null);
                                }
                                catch (Exception ex)
                                {
                                    dbTransaction.Rollback();
                                    MessageBox.Show("删除失败，请检查网络和操作！详细错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    dbTransaction.Dispose();
                                }

                            }
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("要删除选中的记录吗？删除后不可恢复！！", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                conn.Open();

                                string sql = "select * from DataBase1JB_JJRLR where id='" + id+"'";
                                var list = conn.Query<DataBase1JB_JJRLR>(sql).FirstOrDefault();
                                if (list.IsKF_Manager || list.IsPG_Manager || list.IsPMC_Manager)
                                {
                                    MessageBox.Show("错误：已经送审或有部门审核，不能删除？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                                IDbTransaction dbTransaction = conn.BeginTransaction();
                                try
                                {
                                   
                                    string sqlMain = "delete from DataBase1JB_JJRLR where id=@id";
                                    string sqlChild = "delete from DataBase1JB_JJRLR_Child where DataBase1JB_JJRLR_ID=@id";
                                    conn.Execute(sqlChild, new { id = id }, dbTransaction, null, null);
                                    conn.Execute(sqlMain, new { id = id }, dbTransaction, null, null);

                                    dbTransaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    dbTransaction.Rollback();
                                    MessageBox.Show("删除失败，请检查网络和操作！详细错误为：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    Frm1JB_JJRLR_Load(null, null);
                                    dbTransaction.Dispose();
                                }
                            }
                        }
                    }

                }
            }
        }

        private void DoCheck(DateTime dateTime, CheckType checkType, bool isCheckYesOrNo)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            var one = new BaseDal<DataBase1JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (one == null)
            {
                MessageBox.Show("当前月份没有数据，不能审核，请审核的时候仔细检查！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!one.IsPMC_Manager)
            {
                MessageBox.Show("当前月份没有送审，请待送审后，再审核！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (isCheckYesOrNo)
            {
                if (MessageBox.Show(string.Format("检查无误，确认审核：{0}年{1}月的所有数据吗？", dateTime.Year, dateTime.Month), "审核确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string sql = string.Empty;
                    switch (checkType)
                    {
                        case CheckType.PMC_Check:
                            sql = "update DataBase1JB_JJRLR set isPMC_check=1,pmc_time=getdate(),pmc_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PG_Check:
                            sql = "update DataBase1JB_JJRLR set isPG_check=1,pg_time=getdate(),pg_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.KF_Check:
                            sql = "update DataBase1JB_JJRLR set isKF_check=1,KF_time=getdate(),KF_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.WX_Check:
                            sql = "update DataBase1JB_JJRLR set isWX_check=1,WX_time=getdate(),WX_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PMCDD_Check:
                            sql = "update DataBase1JB_JJRLR set isPMCDD_check=1,PMCDD_time=getdate(),PMCDD_user='" + Program.User.Name + "'";
                            break;
                    }
                    sql += " where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString() /*+ " and TheDay=" + selectTime.Day.ToString()*/;
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        if (conn.Execute(sql) > 0)
                        {
                            MessageBox.Show("审核完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("审核失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    DoCheckManager(dateTime);
                }
            }
            else
            {
                if (MessageBox.Show(string.Format("发现错误，确认退回：{0}年{1}月的所有数据吗？" + Environment.NewLine + "注意：退回时如果其他部门已审核通过需要重新审核！", dateTime.Year, dateTime.Month), "退回确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string sql = string.Empty;
                    switch (checkType)
                    {
                        case CheckType.PMC_Check:
                            sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,pmc_time=getdate(),pmc_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PG_Check:
                            sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,pg_time=getdate(),pg_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.KF_Check:
                            sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,KF_time=getdate(),KF_user='" + Program.User.Name + "'";
                            break;
                    }
                    sql += " where TheYear=" + selectTime.Year.ToString() + " and TheMonth=" + selectTime.Month.ToString() /*+ " and TheDay=" + selectTime.Day.ToString()*/;
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        if (conn.Execute(sql) > 0)
                        {
                            BJ(dateTime);
                            MessageBox.Show("退回完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("退回失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DoCheckManager(DateTime dateTime)
        {
            var data = new BaseDal<DataBase1JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.IsPMC_Check && t.IsPG_Check && t.IsKF_Check);
            if (data != null)
            {
                string sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=getdate(),PG_Manager_Time=getdate(),KF_Manager_Time=getdate(),WX_Manager_Time=getdate() where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute(sql);
                }
            }
        }

        private void BJ(DateTime dateTime)
        {
            var data = new BaseDal<DataBase1JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (data != null)
            {
                var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = data.CreateUser.Split('_')[0], MsgTitle = "审核退回", IsDone = false, IsRead = false, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), MsgClass = "审核退回", Msg = $"{dateTime.Year}年{dateTime.Month}月三厂检包计件被审核退回，退回人：{Program.User.Code}_{Program.User.Name}" };
                new BaseDal<DataBaseMsg>().Add(msg);
            }
        }

        private void BtnPMCCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PMC_Check, true);
        }

        private void BtnPMCCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PMC_Check, false);
        }

        private void BtnPGCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PG_Check, true);
        }

        private void BtnPGCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PG_Check, false);
        }

        private void BtnKFCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.KF_Check, true);
        }

        private void BtnKFCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.KF_Check, false);
        }

        //private void BtnWXCheckYes_Click(object sender, EventArgs e)
        //{
        //    DoCheck(selectTime, CheckType.WX_Check, true);
        //}

        //private void BtnWXCheckNo_Click(object sender, EventArgs e)
        //{
        //    DoCheck(selectTime, CheckType.WX_Check, false);
        //}

        //private void BtnPMCDDCheckYes_Click(object sender, EventArgs e)
        //{
        //    DoCheck(selectTime, CheckType.PMCDD_Check, true);
        //}

        //private void BtnPMCDDCheckNo_Click(object sender, EventArgs e)
        //{
        //    DoCheck(selectTime, CheckType.PMCDD_Check, false);
        //}

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

        private void btnSendToCheck_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请确定数据完整无误后再送审，送审后不能修改、删除、新增操作。", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DateTime dateTime = Convert.ToDateTime(dtp.text);
                string sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=getdate() where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    if (conn.Execute(sql) > 0)
                    {
                        MessageBox.Show("送审成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("送审失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnMonthSearch_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            RefGrid(selectTime, txtUserCode.Text, txtUserName.Text, false);
        }

        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            btnMonthSearch.PerformClick();
            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 1)
            {
                MessageBox.Show("没有数据，无法导出！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //                  0      1    2          3    4     5    6     7   8     9   
            string[] header = "ID$TheYear$TheMonth$TheDay$金额$序号$岗位$工号$姓名".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dt.Columns[i].Caption = header[i];
            }

            dt.Columns.RemoveAt(3);
            dt.Columns.RemoveAt(2);
            dt.Columns.RemoveAt(1);
            dt.Columns.RemoveAt(0);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "梦牌一厂检包车间--线外计件-" + selectTime.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "线外计件", "一工厂" + selectTime.ToString("yyyy年MM月") + "线外计件明细");
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

        private void buttonX1_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(-1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(dtp.text);
            time = time.AddDays(1);
            dtp.text = time.ToString("yyyy-M-d");
            btnSearch.PerformClick();
        }
    }
}