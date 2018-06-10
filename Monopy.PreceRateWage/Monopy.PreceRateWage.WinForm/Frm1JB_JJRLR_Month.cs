using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
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
    public partial class Frm1JB_JJRLR_Month : Office2007Form
    {
        
        private Thread threadSh;
        private DateTime dateTime;

        public Frm1JB_JJRLR_Month()
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
        private void Frm1JB_JJRLR_Month_Load(object sender, EventArgs e)
        {
            UiControl();
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
            threadSh = new Thread(CheckSh)
            {
                IsBackground = true
            };
            threadSh.Start();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefGrid(dtp.Value);
        }

        /// <summary>
        /// 日期更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dtp_ValueChanged(object sender, EventArgs e)
        {
            dateTime = dtp.Value;
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
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
            sfd.FileName = "梦牌一厂检包车间--线外计件-" + dtp.Value.ToString("yyyy年MM月") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = new ExcelHelper().DataTable2Excel(dt, "线外计件", "一工厂" + dtp.Value.ToString("yyyy年MM月") + "线外计件明细");
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

        /// <summary>
        /// PMC审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPMCCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.PMC_Check);
        }

        /// <summary>
        /// 品管审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPGCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.PG_Check);
        }

        /// <summary>
        /// 开发审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKFCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.KF_Check);
        }

        #endregion

        #region 调用方法

        private void CheckSh()
        {
            Dictionary<object, InvokeData> dic = new Dictionary<object, InvokeData>();
            while (true)
            {
                dic.Clear();
                var theOne = new BaseDal<DataBase1JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
                if (theOne != null)
                {
                    dic.Add(BtnPMCCheck, new InvokeData(this) { Text = theOne.IsPMC_Manager ? "PMC经理已审" : "PMC经理未审", Enable = !theOne.IsPMC_Manager });
                    dic.Add(lblPMC, new InvokeData(this) { Text = theOne.IsPMC_Manager ? "PMC经理已审" : "PMC经理未审", BackColor = theOne.IsPMC_Manager ? Color.Lime : Color.Red });

                    dic.Add(BtnPGCheck, new InvokeData(this) { Text = theOne.IsPG_Manager ? "品管经理已审" : "品管经理未审", Enable = !theOne.IsPG_Manager });
                    dic.Add(lblPG, new InvokeData(this) { Text = theOne.IsPG_Manager ? "品管经理已审" : "品管经理未审", BackColor = theOne.IsPG_Manager ? Color.Lime : Color.Red });

                    dic.Add(BtnKFCheck, new InvokeData(this) { Text = theOne.IsKF_Manager ? "开发经理已审" : "开发经理未审", Enable = !theOne.IsKF_Manager });
                    dic.Add(lblKF, new InvokeData(this) { Text = theOne.IsKF_Manager ? "开发经理已审" : "开发经理未审", BackColor = theOne.IsKF_Manager ? Color.Lime : Color.Red });

                }
                else
                {
                    dic.Add(BtnPMCCheck, new InvokeData(this) { Text = "PMC经理未审", Enable = true });
                    dic.Add(lblPMC, new InvokeData(this) { Text = "PMC经理未审", BackColor = Color.Red });

                    dic.Add(BtnPGCheck, new InvokeData(this) { Text = "品管经理未审", Enable = true });
                    dic.Add(lblPG, new InvokeData(this) { Text = "品管经理未审", BackColor = Color.Red });

                    dic.Add(BtnKFCheck, new InvokeData(this) { Text = "开发经理未审", Enable = true });
                    dic.Add(lblKF, new InvokeData(this) { Text = "开发经理未审", BackColor = Color.Red });
                }
                if (dic.Count > 0)
                {
                    InvokeHelper.SetInvoke(dic);
                }
                Thread.Sleep(1000);
            }
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

        private void RefGrid(DateTime selectTime)
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


            list = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();


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

        private void DoCheck(DateTime dateTime, CheckType checkType)
        {
            if (MessageBox.Show("检查无误，确认审核", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string sql = string.Empty;
                switch (checkType)
                {
                    case CheckType.PMC_Check:
                        sql = "update DataBase1JB_JJRLR set PMC_Manager_Time=getdate(),PMC_Manager_User='" + Program.User.Name + "'";
                        break;

                    case CheckType.PG_Check:
                        sql = "update DataBase1JB_JJRLR set PG_Manager_Time=getdate(),PG_Manager_User='" + Program.User.Name + "'";
                        break;

                    case CheckType.KF_Check:
                        sql = "update DataBase1JB_JJRLR set KF_Manager_Time=getdate(),KF_Manager_User='" + Program.User.Name + "'";
                        break;

                }
                sql += " where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    if (conn.Execute(sql) > 0)
                    {
                        MessageBox.Show("经理审核完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("经理审核失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion
    }
}