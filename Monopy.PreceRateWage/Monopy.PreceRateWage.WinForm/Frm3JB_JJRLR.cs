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
using System.Threading;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3JB_JJRLR : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        private Thread threadSh;

        public Frm3JB_JJRLR()
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
                var theOne = new BaseDal<DataBase3JB_JJRLR>().Get(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month);
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

                    if (isBtnWxEnable)
                    {
                        if (theOne.IsWX_Check)
                        {
                            dic.Add(BtnWXCheckYes, new InvokeData(this) { Text = "外销确认", Enable = false });
                            dic.Add(BtnWXCheckNo, new InvokeData(this) { Text = "外销退回", Enable = false });
                        }
                        else
                        {
                            dic.Add(BtnWXCheckYes, new InvokeData(this) { Text = "外销确认", Enable = true });
                            dic.Add(BtnWXCheckNo, new InvokeData(this) { Text = "外销退回", Enable = true });
                        }
                    }
                    if (theOne.WX_Time != null)
                    {
                        dic.Add(lblWX, new InvokeData(this) { Text = theOne.IsWX_Check ? "外销审核" : "外销退回", BackColor = theOne.IsWX_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblWX, new InvokeData(this) { Text = "外销未审核", BackColor = Color.Transparent });
                    }

                    if (isBtnPmcDdEnable)
                    {
                        if (theOne.IsPMCDD_Check)
                        {
                            dic.Add(BtnPMCDDCheckYes, new InvokeData(this) { Text = "PMC调度确认", Enable = false });
                            dic.Add(BtnPMCDDCheckNo, new InvokeData(this) { Text = "PMC调度退回", Enable = false });
                        }
                        else
                        {
                            dic.Add(BtnPMCDDCheckYes, new InvokeData(this) { Text = "PMC调度确认", Enable = true });
                            dic.Add(BtnPMCDDCheckNo, new InvokeData(this) { Text = "PMC调度退回", Enable = true });
                        }
                    }
                    if (theOne.PMCDD_Time != null)
                    {
                        dic.Add(lblPMCDD, new InvokeData(this) { Text = theOne.IsPMCDD_Check ? "PMC调度审核" : "PMC调度退回", BackColor = theOne.IsPMCDD_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblPMCDD, new InvokeData(this) { Text = "PMC调度未审核", BackColor = Color.Transparent });
                    }
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
                isBtnWxEnable = true;
                isBtnPmcDdEnable = true;
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
                    if (item.Contains("BtnWXCheckYes") && dic[item])
                    {
                        isBtnWxEnable = true;
                    }
                    if (item.Contains("BtnPMCDDCheckYes") && dic[item])
                    {
                        isBtnPmcDdEnable = true;
                    }
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
        private bool isBtnWxEnable = false;
        private bool isBtnPmcDdEnable = false;

        #endregion 界面控制变量

        private void Frm3JB_JJRLR_Load(object sender, EventArgs e)
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

        private void RefDgv(int year, int month, string userCode)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode.Contains(userCode)).ToList().GroupBy(t => new { t.Line, t.GW, t.UserCode, t.UserName }).Select(t => new DataBase3JB_JJRLR { IsPMC_Check = t.Max(x => x.IsPMC_Check), PMC_Manager_Time = t.Max(x => x.PMC_Manager_Time), IsPG_Check = t.Max(x => x.IsPG_Check), PG_Manager_Time = t.Max(x => x.PG_Manager_Time), IsKF_Check = t.Max(x => x.IsKF_Check), KF_Manager_Time = t.Max(x => x.KF_Manager_Time), IsWX_Check = t.Max(x => x.IsWX_Check), WX_Manager_Time = t.Max(x => x.WX_Manager_Time), IsPMCDD_Check = t.Max(x => x.IsPMCDD_Check), TheYear = year, TheMonth = month, TheDay = 0, JE = t.Sum(x => string.IsNullOrEmpty(x.JE) ? 0M : Convert.ToDecimal(x.JE)).ToString(), Line = t.Key.Line, GW = t.Key.GW, UserCode = t.Key.UserCode, UserName = t.Key.UserName, F_1 = t.Sum(x => string.IsNullOrEmpty(x.F_1) ? 0M : Convert.ToDecimal(x.F_1)).ToString(), F_2 = t.Sum(x => string.IsNullOrEmpty(x.F_2) ? 0M : Convert.ToDecimal(x.F_2)).ToString(), F_3 = t.Sum(x => string.IsNullOrEmpty(x.F_3) ? 0M : Convert.ToDecimal(x.F_3)).ToString(), F_B_1 = t.Sum(x => string.IsNullOrEmpty(x.F_B_1) ? 0M : Convert.ToDecimal(x.F_B_1)).ToString(), F_B_2 = t.Sum(x => string.IsNullOrEmpty(x.F_B_2) ? 0M : Convert.ToDecimal(x.F_B_2)).ToString(), F_B_3 = t.Sum(x => string.IsNullOrEmpty(x.F_B_3) ? 0M : Convert.ToDecimal(x.F_B_3)).ToString(), F_B_4 = t.Sum(x => string.IsNullOrEmpty(x.F_B_4) ? 0M : Convert.ToDecimal(x.F_B_4)).ToString(), F_B_5 = t.Sum(x => string.IsNullOrEmpty(x.F_B_5) ? 0M : Convert.ToDecimal(x.F_B_5)).ToString(), PMC_1 = t.Sum(x => string.IsNullOrEmpty(x.PMC_1) ? 0M : Convert.ToDecimal(x.PMC_1)).ToString(), PMC_2 = t.Sum(x => string.IsNullOrEmpty(x.PMC_2) ? 0M : Convert.ToDecimal(x.PMC_2)).ToString(), PMC_3 = t.Sum(x => string.IsNullOrEmpty(x.PMC_3) ? 0M : Convert.ToDecimal(x.PMC_3)).ToString(), PMC_4 = t.Sum(x => string.IsNullOrEmpty(x.PMC_4) ? 0M : Convert.ToDecimal(x.PMC_4)).ToString(), PMC_5 = t.Sum(x => string.IsNullOrEmpty(x.PMC_5) ? 0M : Convert.ToDecimal(x.PMC_5)).ToString(), PMC_6 = t.Sum(x => string.IsNullOrEmpty(x.PMC_6) ? 0M : Convert.ToDecimal(x.PMC_6)).ToString(), PMC_7 = t.Sum(x => string.IsNullOrEmpty(x.PMC_7) ? 0M : Convert.ToDecimal(x.PMC_7)).ToString(), PMC_8 = t.Sum(x => string.IsNullOrEmpty(x.PMC_8) ? 0M : Convert.ToDecimal(x.PMC_8)).ToString(), PMC_9 = t.Sum(x => string.IsNullOrEmpty(x.PMC_9) ? 0M : Convert.ToDecimal(x.PMC_9)).ToString(), PMC_10 = t.Sum(x => string.IsNullOrEmpty(x.PMC_10) ? 0M : Convert.ToDecimal(x.PMC_10)).ToString(), PMC_11 = t.Sum(x => string.IsNullOrEmpty(x.PMC_11) ? 0M : Convert.ToDecimal(x.PMC_11)).ToString(), PMC_12 = t.Sum(x => string.IsNullOrEmpty(x.PMC_12) ? 0M : Convert.ToDecimal(x.PMC_12)).ToString(), PMC_13 = t.Sum(x => string.IsNullOrEmpty(x.PMC_13) ? 0M : Convert.ToDecimal(x.PMC_13)).ToString(), PMC_14 = t.Sum(x => string.IsNullOrEmpty(x.PMC_14) ? 0M : Convert.ToDecimal(x.PMC_14)).ToString(), PMC_15 = t.Sum(x => string.IsNullOrEmpty(x.PMC_15) ? 0M : Convert.ToDecimal(x.PMC_15)).ToString(), PMC_16 = t.Sum(x => string.IsNullOrEmpty(x.PMC_16) ? 0M : Convert.ToDecimal(x.PMC_16)).ToString(), PMC_17 = t.Sum(x => string.IsNullOrEmpty(x.PMC_17) ? 0M : Convert.ToDecimal(x.PMC_17)).ToString(), PMC_18 = t.Sum(x => string.IsNullOrEmpty(x.PMC_18) ? 0M : Convert.ToDecimal(x.PMC_18)).ToString(), PMC_19 = t.Sum(x => string.IsNullOrEmpty(x.PMC_19) ? 0M : Convert.ToDecimal(x.PMC_19)).ToString(), PMC_20 = t.Sum(x => string.IsNullOrEmpty(x.PMC_20) ? 0M : Convert.ToDecimal(x.PMC_20)).ToString(), PMC_21 = t.Sum(x => string.IsNullOrEmpty(x.PMC_21) ? 0M : Convert.ToDecimal(x.PMC_21)).ToString(), PMC_22 = t.Sum(x => string.IsNullOrEmpty(x.PMC_22) ? 0M : Convert.ToDecimal(x.PMC_22)).ToString(), PMC_23 = t.Sum(x => string.IsNullOrEmpty(x.PMC_23) ? 0M : Convert.ToDecimal(x.PMC_23)).ToString(), PMC_B_1 = t.Sum(x => string.IsNullOrEmpty(x.PMC_B_1) ? 0M : Convert.ToDecimal(x.PMC_B_1)).ToString(), PMC_B_2 = t.Sum(x => string.IsNullOrEmpty(x.PMC_B_2) ? 0M : Convert.ToDecimal(x.PMC_B_2)).ToString(), PMC_B_3 = t.Sum(x => string.IsNullOrEmpty(x.PMC_B_3) ? 0M : Convert.ToDecimal(x.PMC_B_3)).ToString(), PMC_B_4 = t.Sum(x => string.IsNullOrEmpty(x.PMC_B_4) ? 0M : Convert.ToDecimal(x.PMC_B_4)).ToString(), PMC_B_5 = t.Sum(x => string.IsNullOrEmpty(x.PMC_B_5) ? 0M : Convert.ToDecimal(x.PMC_B_5)).ToString(), PG_1 = t.Sum(x => string.IsNullOrEmpty(x.PG_1) ? 0M : Convert.ToDecimal(x.PG_1)).ToString(), PG_2 = t.Sum(x => string.IsNullOrEmpty(x.PG_2) ? 0M : Convert.ToDecimal(x.PG_2)).ToString(), PG_3 = t.Sum(x => string.IsNullOrEmpty(x.PG_3) ? 0M : Convert.ToDecimal(x.PG_3)).ToString(), PG_4 = t.Sum(x => string.IsNullOrEmpty(x.PG_4) ? 0M : Convert.ToDecimal(x.PG_4)).ToString(), PG_5 = t.Sum(x => string.IsNullOrEmpty(x.PG_5) ? 0M : Convert.ToDecimal(x.PG_5)).ToString(), PG_6 = t.Sum(x => string.IsNullOrEmpty(x.PG_6) ? 0M : Convert.ToDecimal(x.PG_6)).ToString(), PG_7 = t.Sum(x => string.IsNullOrEmpty(x.PG_7) ? 0M : Convert.ToDecimal(x.PG_7)).ToString(), PG_8 = t.Sum(x => string.IsNullOrEmpty(x.PG_8) ? 0M : Convert.ToDecimal(x.PG_8)).ToString(), PG_B_1 = t.Sum(x => string.IsNullOrEmpty(x.PG_B_1) ? 0M : Convert.ToDecimal(x.PG_B_1)).ToString(), PG_B_2 = t.Sum(x => string.IsNullOrEmpty(x.PG_B_2) ? 0M : Convert.ToDecimal(x.PG_B_2)).ToString(), PG_B_3 = t.Sum(x => string.IsNullOrEmpty(x.PG_B_3) ? 0M : Convert.ToDecimal(x.PG_B_3)).ToString(), PG_B_4 = t.Sum(x => string.IsNullOrEmpty(x.PG_B_4) ? 0M : Convert.ToDecimal(x.PG_B_4)).ToString(), PG_B_5 = t.Sum(x => string.IsNullOrEmpty(x.PG_B_5) ? 0M : Convert.ToDecimal(x.PG_B_5)).ToString(), KF_1 = t.Sum(x => string.IsNullOrEmpty(x.KF_1) ? 0M : Convert.ToDecimal(x.KF_1)).ToString(), KF_B_1 = t.Sum(x => string.IsNullOrEmpty(x.KF_B_1) ? 0M : Convert.ToDecimal(x.KF_B_1)).ToString(), KF_B_2 = t.Sum(x => string.IsNullOrEmpty(x.KF_B_2) ? 0M : Convert.ToDecimal(x.KF_B_2)).ToString(), KF_B_3 = t.Sum(x => string.IsNullOrEmpty(x.KF_B_3) ? 0M : Convert.ToDecimal(x.KF_B_3)).ToString(), KF_B_4 = t.Sum(x => string.IsNullOrEmpty(x.KF_B_4) ? 0M : Convert.ToDecimal(x.KF_B_4)).ToString(), KF_B_5 = t.Sum(x => string.IsNullOrEmpty(x.KF_B_5) ? 0M : Convert.ToDecimal(x.KF_B_5)).ToString(), WX_PMCDD_1 = t.Sum(x => string.IsNullOrEmpty(x.WX_PMCDD_1) ? 0M : Convert.ToDecimal(x.WX_PMCDD_1)).ToString(), WX_1 = t.Sum(x => string.IsNullOrEmpty(x.WX_1) ? 0M : Convert.ToDecimal(x.WX_1)).ToString(), WX_B_1 = t.Sum(x => string.IsNullOrEmpty(x.WX_B_1) ? 0M : Convert.ToDecimal(x.WX_B_1)).ToString(), WX_B_2 = t.Sum(x => string.IsNullOrEmpty(x.WX_B_2) ? 0M : Convert.ToDecimal(x.WX_B_2)).ToString(), WX_B_3 = t.Sum(x => string.IsNullOrEmpty(x.WX_B_3) ? 0M : Convert.ToDecimal(x.WX_B_3)).ToString(), WX_B_4 = t.Sum(x => string.IsNullOrEmpty(x.WX_B_4) ? 0M : Convert.ToDecimal(x.WX_B_4)).ToString(), WX_B_5 = t.Sum(x => string.IsNullOrEmpty(x.WX_B_5) ? 0M : Convert.ToDecimal(x.WX_B_5)).ToString() }).OrderBy(t => int.TryParse(t.Line, out int xx) ? xx : 0).ThenBy(t => t.GW).ThenBy(t => t.UserCode).ThenBy(t => t.UserName).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_JJRLR(datas, new DateTime(year, month, 1)));
            dgv.DataSource = datas;
            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 72; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[9].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "年$月$日$金额$序号$线位$岗位$工号$姓名$刷连体$刷水箱$刷大套$工厂备用1$工厂备用2$工厂备用3$工厂备用4$工厂备用5$包水箱盖（含5755S）$裸套袋连体码托$裸套袋大套、水箱、面具码托$装卸车（各厂调货）$验货贴P.0-2（802盆及大套）$贴验货贴P0 (全贴)$贴发货贴$卸配件$裸套袋$码包$人工提裸水箱、面具$人工提裸大套$人工提裸连体$简包（缠膜）$叉车提裸水箱、面具$叉车提裸大套$叉车提裸连体$叉车协助库房倒库$拉包（北厂）$纸返纸大套合包$3445L/3445J/3313/3312/3341/3342/2294/2296/3517A/3517D/3271大套打纸滑托 $S-4188A水箱打纸滑托 $S-7747调水件$纸返纸大套分包、挂便、连体$纸返纸水箱、盆柱$ML-2467打托盘$PMC备用4$PMC备用5$纸返纸大套合包$拉包（北厂）$叉车协助品管验货倒库$非义务复检大套、蹲便类$非义务复检连体类（含柜盆、挂便）$非义务复检水箱（含盖）$非义务复检面具、台盆、柱$打样品$纸返纸大套分包、挂便、连体$纸返纸水箱、盆柱$品管备用3$品管备用4$品管备用5$打样品$开发备用1$开发备用2$开发备用3$开发备用4$开发备用5$售后配件$打样品$外销备用1$外销备用2$外销备用3$外销备用4$外销备用5$操作时间$操作人".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i > 9 && i < 18)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
                else if (i >= 18 && i < 46)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Blue };
                }
                else if (i >= 46 && i < 59)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Tomato };
                }
                else if (i >= 59 && i < 65)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.SteelBlue };
                }
                else if (i == 65)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Magenta };
                }
            }
            dgv.EnableHeadersVisualStyles = false;
        }

        private void RefDgv(DateTime selectTime, string userCode)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day && t.UserCode.Contains(userCode)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3JB_JJRLR(datas, selectTime));
            dgv.DataSource = datas;
            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 72; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Columns[9].Frozen = true;
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "年$月$日$金额$序号$线位$岗位$工号$姓名$刷连体$刷水箱$刷大套$工厂备用1$工厂备用2$工厂备用3$工厂备用4$工厂备用5$包水箱盖（含5755S）$裸套袋连体码托$裸套袋大套、水箱、面具码托$装卸车（各厂调货）$验货贴P.0-2（802盆及大套）$贴验货贴P0 (全贴)$贴发货贴$卸配件$裸套袋$码包$人工提裸水箱、面具$人工提裸大套$人工提裸连体$简包（缠膜）$叉车提裸水箱、面具$叉车提裸大套$叉车提裸连体$叉车协助库房倒库$拉包（北厂）$纸返纸大套合包$3445L/3445J/3313/3312/3341/3342/2294/2296/3517A/3517D/3271大套打纸滑托 $S-4188A水箱打纸滑托 $S-7747调水件$纸返纸大套分包、挂便、连体$纸返纸水箱、盆柱$ML-2467打托盘$PMC备用4$PMC备用5$纸返纸大套合包$拉包（北厂）$叉车协助品管验货倒库$非义务复检大套、蹲便类$非义务复检连体类（含柜盆、挂便）$非义务复检水箱（含盖）$非义务复检面具、台盆、柱$打样品$纸返纸大套分包、挂便、连体$纸返纸水箱、盆柱$品管备用3$品管备用4$品管备用5$打样品$开发备用1$开发备用2$开发备用3$开发备用4$开发备用5$售后配件$打样品$外销备用1$外销备用2$外销备用3$外销备用4$外销备用5$操作时间$操作人".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i > 9 && i < 18)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
                else if (i >= 18 && i < 46)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Blue };
                }
                else if (i >= 46 && i < 59)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Tomato };
                }
                else if (i >= 59 && i < 65)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.SteelBlue };
                }
                else if (i == 65)
                {
                    dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Magenta };
                }
            }
            dgv.EnableHeadersVisualStyles = false;
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
            RefDgv(selectTime, txtUserCode.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——检包——计件日录入.xlsx");
        }

        private bool Recount(List<DataBase3JB_JJRLR> list)
        {
            #region 获取基础信息

            DataBaseDay dataBaseDay_01 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim().Trim() == "刷连体");
            DataBaseDay dataBaseDay_02 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "刷水箱");
            DataBaseDay dataBaseDay_03 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "刷大套");
            DataBaseDay dataBaseDay_04 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "包水箱盖（含5755S）");
            DataBaseDay dataBaseDay_05 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "裸套袋连体码托");
            DataBaseDay dataBaseDay_06 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "裸套袋大套、水箱、面具码托");
            DataBaseDay dataBaseDay_07 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "装卸车（各厂调货）");
            DataBaseDay dataBaseDay_08 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "验货贴P.0-2（802盆及大套）");
            DataBaseDay dataBaseDay_09 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "贴验货贴P0 (全贴)");
            DataBaseDay dataBaseDay_10 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "贴发货贴");
            DataBaseDay dataBaseDay_11 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "卸配件");
            DataBaseDay dataBaseDay_12 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "裸套袋");
            DataBaseDay dataBaseDay_13 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "码包");
            DataBaseDay dataBaseDay_14 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "人工提裸水箱、面具");
            DataBaseDay dataBaseDay_15 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "人工提裸大套");
            DataBaseDay dataBaseDay_16 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "人工提裸连体");
            DataBaseDay dataBaseDay_17 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "简包（缠膜）");
            DataBaseDay dataBaseDay_18 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "叉车提裸水箱、面具");
            DataBaseDay dataBaseDay_19 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "叉车提裸大套");
            DataBaseDay dataBaseDay_20 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "叉车提裸连体");
            DataBaseDay dataBaseDay_21 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "叉车协助库房倒库");
            DataBaseDay dataBaseDay_22 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "拉包（北厂）");
            DataBaseDay dataBaseDay_23 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "纸返纸大套合包");
            DataBaseDay dataBaseDay_24 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim().Trim() == "3445L/3445J/3313/3312/3341/3342/2294/2296/3517A/3517D/3271大套打纸滑托".Trim());
            DataBaseDay dataBaseDay_25 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "S-4188A水箱打纸滑托");
            DataBaseDay dataBaseDay_26 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "S-7747调水件");
            DataBaseDay dataBaseDay_27 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "叉车协助品管验货倒库");
            DataBaseDay dataBaseDay_28 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "非义务复检大套、蹲便类");
            DataBaseDay dataBaseDay_29 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "非义务复检连体类（含柜盆、挂便）");
            DataBaseDay dataBaseDay_30 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "非义务复检水箱（含盖）");
            DataBaseDay dataBaseDay_31 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "非义务复检面具、台盆、柱");
            DataBaseDay dataBaseDay_32 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "打样品");
            DataBaseDay dataBaseDay_33 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "售后配件");
            DataBaseDay dataBaseDay_B01 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "工厂备用1");
            DataBaseDay dataBaseDay_B02 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "工厂备用2");
            DataBaseDay dataBaseDay_B03 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "工厂备用3");
            DataBaseDay dataBaseDay_B04 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "工厂备用4");
            DataBaseDay dataBaseDay_B05 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "工厂备用5");
            DataBaseDay dataBaseDay_B06 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "纸返纸大套分包、挂便、连体");
            DataBaseDay dataBaseDay_B07 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "纸返纸水箱、盆柱");
            DataBaseDay dataBaseDay_B08 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "ML-2467打托盘");
            DataBaseDay dataBaseDay_B09 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "PMC备用4");
            DataBaseDay dataBaseDay_B10 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "PMC备用5");
            //DataBaseDay dataBaseDay_B11 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "品管备用1");
            //DataBaseDay dataBaseDay_B12 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "品管备用2");
            DataBaseDay dataBaseDay_B13 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "品管备用3");
            DataBaseDay dataBaseDay_B14 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "品管备用4");
            DataBaseDay dataBaseDay_B15 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "品管备用5");
            DataBaseDay dataBaseDay_B16 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "开发备用1");
            DataBaseDay dataBaseDay_B17 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "开发备用2");
            DataBaseDay dataBaseDay_B18 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "开发备用3");
            DataBaseDay dataBaseDay_B19 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "开发备用4");
            DataBaseDay dataBaseDay_B20 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "开发备用5");
            DataBaseDay dataBaseDay_B21 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "外销备用1");
            DataBaseDay dataBaseDay_B22 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "外销备用2");
            DataBaseDay dataBaseDay_B23 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "外销备用3");
            DataBaseDay dataBaseDay_B24 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "外销备用4");
            DataBaseDay dataBaseDay_B25 = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "检包车间" && t.TypesName.Trim() == "外销备用5");
            if (dataBaseDay_01 == null || dataBaseDay_02 == null || dataBaseDay_03 == null || dataBaseDay_04 == null || dataBaseDay_05 == null || dataBaseDay_06 == null || dataBaseDay_07 == null || dataBaseDay_08 == null || dataBaseDay_09 == null || dataBaseDay_10 == null || dataBaseDay_11 == null || dataBaseDay_12 == null || dataBaseDay_13 == null || dataBaseDay_14 == null || dataBaseDay_15 == null || dataBaseDay_16 == null || dataBaseDay_17 == null || dataBaseDay_18 == null || dataBaseDay_19 == null || dataBaseDay_20 == null || dataBaseDay_21 == null || dataBaseDay_22 == null || dataBaseDay_23 == null || dataBaseDay_24 == null || dataBaseDay_25 == null || dataBaseDay_26 == null || dataBaseDay_27 == null || dataBaseDay_28 == null || dataBaseDay_29 == null || dataBaseDay_30 == null || dataBaseDay_31 == null || dataBaseDay_32 == null || dataBaseDay_33 == null)
            {
                return false;
            }

            #endregion 获取基础信息

            try
            {
                foreach (var item in list)
                {
                    decimal je = 0;
                    if (!string.IsNullOrEmpty(item.F_1) && dataBaseDay_01 != null)
                    {
                        je += Convert.ToDecimal(item.F_1) * Convert.ToDecimal(dataBaseDay_01.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.F_2) && dataBaseDay_02 != null)
                    {
                        je += Convert.ToDecimal(item.F_2) * Convert.ToDecimal(dataBaseDay_02.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.F_3) && dataBaseDay_03 != null)
                    {
                        je += Convert.ToDecimal(item.F_3) * Convert.ToDecimal(dataBaseDay_03.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.F_B_1) && dataBaseDay_B01 != null)
                    {
                        je += Convert.ToDecimal(item.F_B_1) * Convert.ToDecimal(dataBaseDay_B01.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.F_B_2) && dataBaseDay_B02 != null)
                    {
                        je += Convert.ToDecimal(item.F_B_2) * Convert.ToDecimal(dataBaseDay_B02.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.F_B_3) && dataBaseDay_B03 != null)
                    {
                        je += Convert.ToDecimal(item.F_B_3) * Convert.ToDecimal(dataBaseDay_B03.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.F_B_4) && dataBaseDay_B04 != null)
                    {
                        je += Convert.ToDecimal(item.F_B_4) * Convert.ToDecimal(dataBaseDay_B04.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.F_B_5) && dataBaseDay_B05 != null)
                    {
                        je += Convert.ToDecimal(item.F_B_5) * Convert.ToDecimal(dataBaseDay_B05.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_1) && dataBaseDay_04 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_1) * Convert.ToDecimal(dataBaseDay_04.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_2) && dataBaseDay_05 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_2) * Convert.ToDecimal(dataBaseDay_05.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_3) && dataBaseDay_06 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_3) * Convert.ToDecimal(dataBaseDay_06.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_4) && dataBaseDay_07 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_4) * Convert.ToDecimal(dataBaseDay_07.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_5) && dataBaseDay_08 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_5) * Convert.ToDecimal(dataBaseDay_08.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_6) && dataBaseDay_09 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_6) * Convert.ToDecimal(dataBaseDay_09.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_7) && dataBaseDay_10 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_7) * Convert.ToDecimal(dataBaseDay_10.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_8) && dataBaseDay_11 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_8) * Convert.ToDecimal(dataBaseDay_11.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_9) && dataBaseDay_12 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_9) * Convert.ToDecimal(dataBaseDay_12.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_10) && dataBaseDay_13 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_10) * Convert.ToDecimal(dataBaseDay_13.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_11) && dataBaseDay_14 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_11) * Convert.ToDecimal(dataBaseDay_14.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_12) && dataBaseDay_15 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_12) * Convert.ToDecimal(dataBaseDay_15.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_13) && dataBaseDay_16 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_13) * Convert.ToDecimal(dataBaseDay_16.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_14) && dataBaseDay_17 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_14) * Convert.ToDecimal(dataBaseDay_17.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_15) && dataBaseDay_18 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_15) * Convert.ToDecimal(dataBaseDay_18.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_16) && dataBaseDay_19 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_16) * Convert.ToDecimal(dataBaseDay_19.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_17) && dataBaseDay_20 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_17) * Convert.ToDecimal(dataBaseDay_20.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_18) && dataBaseDay_21 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_18) * Convert.ToDecimal(dataBaseDay_21.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_19) && dataBaseDay_22 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_19) * Convert.ToDecimal(dataBaseDay_22.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_20) && dataBaseDay_23 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_20) * Convert.ToDecimal(dataBaseDay_23.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_21) && dataBaseDay_24 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_21) * Convert.ToDecimal(dataBaseDay_24.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_22) && dataBaseDay_25 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_22) * Convert.ToDecimal(dataBaseDay_25.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_23) && dataBaseDay_26 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_23) * Convert.ToDecimal(dataBaseDay_26.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.PMC_B_1) && dataBaseDay_B06 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_B_1) * Convert.ToDecimal(dataBaseDay_B06.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_B_2) && dataBaseDay_B07 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_B_2) * Convert.ToDecimal(dataBaseDay_B07.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_B_3) && dataBaseDay_B08 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_B_3) * Convert.ToDecimal(dataBaseDay_B08.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_B_4) && dataBaseDay_B09 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_B_4) * Convert.ToDecimal(dataBaseDay_B09.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PMC_B_5) && dataBaseDay_B10 != null)
                    {
                        je += Convert.ToDecimal(item.PMC_B_5) * Convert.ToDecimal(dataBaseDay_B10.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.PG_1) && dataBaseDay_B06 != null)
                    {
                        je += Convert.ToDecimal(item.PG_1) * Convert.ToDecimal(dataBaseDay_B06.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_2) && dataBaseDay_22 != null)
                    {
                        je += Convert.ToDecimal(item.PG_2) * Convert.ToDecimal(dataBaseDay_22.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_3) && dataBaseDay_27 != null)
                    {
                        je += Convert.ToDecimal(item.PG_3) * Convert.ToDecimal(dataBaseDay_27.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_4) && dataBaseDay_28 != null)
                    {
                        je += Convert.ToDecimal(item.PG_4) * Convert.ToDecimal(dataBaseDay_28.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_5) && dataBaseDay_29 != null)
                    {
                        je += Convert.ToDecimal(item.PG_5) * Convert.ToDecimal(dataBaseDay_29.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_6) && dataBaseDay_30 != null)
                    {
                        je += Convert.ToDecimal(item.PG_6) * Convert.ToDecimal(dataBaseDay_30.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_7) && dataBaseDay_31 != null)
                    {
                        je += Convert.ToDecimal(item.PG_7) * Convert.ToDecimal(dataBaseDay_31.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_8) && dataBaseDay_32 != null)
                    {
                        je += Convert.ToDecimal(item.PG_8) * Convert.ToDecimal(dataBaseDay_32.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.PG_B_1) && dataBaseDay_B06 != null)
                    {
                        je += Convert.ToDecimal(item.PG_B_1) * Convert.ToDecimal(dataBaseDay_B06.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_B_2) && dataBaseDay_B07 != null)
                    {
                        je += Convert.ToDecimal(item.PG_B_2) * Convert.ToDecimal(dataBaseDay_B07.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_B_3) && dataBaseDay_B13 != null)
                    {
                        je += Convert.ToDecimal(item.PG_B_3) * Convert.ToDecimal(dataBaseDay_B13.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_B_4) && dataBaseDay_B14 != null)
                    {
                        je += Convert.ToDecimal(item.PG_B_4) * Convert.ToDecimal(dataBaseDay_B14.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.PG_B_5) && dataBaseDay_B15 != null)
                    {
                        je += Convert.ToDecimal(item.PG_B_5) * Convert.ToDecimal(dataBaseDay_B15.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.KF_1) && dataBaseDay_32 != null)
                    {
                        je += Convert.ToDecimal(item.KF_1) * Convert.ToDecimal(dataBaseDay_32.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.KF_B_1) && dataBaseDay_B16 != null)
                    {
                        je += Convert.ToDecimal(item.KF_B_1) * Convert.ToDecimal(dataBaseDay_B16.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.KF_B_2) && dataBaseDay_B17 != null)
                    {
                        je += Convert.ToDecimal(item.KF_B_2) * Convert.ToDecimal(dataBaseDay_B17.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.KF_B_3) && dataBaseDay_B18 != null)
                    {
                        je += Convert.ToDecimal(item.KF_B_3) * Convert.ToDecimal(dataBaseDay_B18.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.KF_B_4) && dataBaseDay_B19 != null)
                    {
                        je += Convert.ToDecimal(item.KF_B_4) * Convert.ToDecimal(dataBaseDay_B19.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.KF_B_5) && dataBaseDay_B20 != null)
                    {
                        je += Convert.ToDecimal(item.KF_B_5) * Convert.ToDecimal(dataBaseDay_B20.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.WX_PMCDD_1) && dataBaseDay_33 != null)
                    {
                        je += Convert.ToDecimal(item.WX_PMCDD_1) * Convert.ToDecimal(dataBaseDay_33.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.WX_1) && dataBaseDay_32 != null)
                    {
                        je += Convert.ToDecimal(item.WX_1) * Convert.ToDecimal(dataBaseDay_32.UnitPrice);
                    }

                    if (!string.IsNullOrEmpty(item.WX_B_1) && dataBaseDay_B21 != null)
                    {
                        je += Convert.ToDecimal(item.WX_B_1) * Convert.ToDecimal(dataBaseDay_B21.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.WX_B_2) && dataBaseDay_B22 != null)
                    {
                        je += Convert.ToDecimal(item.WX_B_2) * Convert.ToDecimal(dataBaseDay_B22.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.WX_B_3) && dataBaseDay_B23 != null)
                    {
                        je += Convert.ToDecimal(item.WX_B_3) * Convert.ToDecimal(dataBaseDay_B23.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.WX_B_4) && dataBaseDay_B24 != null)
                    {
                        je += Convert.ToDecimal(item.WX_B_4) * Convert.ToDecimal(dataBaseDay_B24.UnitPrice);
                    }
                    if (!string.IsNullOrEmpty(item.WX_B_5) && dataBaseDay_B25 != null)
                    {
                        je += Convert.ToDecimal(item.WX_B_5) * Convert.ToDecimal(dataBaseDay_B25.UnitPrice);
                    }

                    item.JE = je == 0 ? string.Empty : je.ToString();
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
            List<DataBase3JB_JJRLR> list = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            if (list.Count > 0 && (list[0].IsKF_Manager || list[0].IsPG_Manager || list[0].IsPMC_Manager || list[0].IsWX_Manager))
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
                Enabled = false;
                list = new ExcelHelper<DataBase3JB_JJRLR>().ReadExcel(openFileDlg.FileName, 2, 5);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (string.IsNullOrEmpty(item.UserCode) || string.IsNullOrEmpty(item.UserName) || item.UserCode == item.UserName)
                    {
                        list.Remove(item);
                        i--;
                        continue;
                    }

                    if (!MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                    {
                        MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }

                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = selectTime.Year;
                    item.TheMonth = selectTime.Month;
                    item.TheDay = selectTime.Day;
                }
                if (Recount(list) && new BaseDal<DataBase3JB_JJRLR>().Add(list) > 0)
                {
                    Enabled = true;
                    txtUserCode.Text = "";
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_JJRLR> list = dgv.DataSource as List<DataBase3JB_JJRLR>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_JJRLR>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——计件日录入.xlsx", saveFileDlg.FileName, list, 2, 4, 33, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            List<DataBase3JB_JJRLR> list = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList();
            if (list.Count > 0 && (list[0].IsKF_Manager || list[0].IsPG_Manager || list[0].IsPMC_Manager || list[0].IsWX_Manager))
            {
                MessageBox.Show("错误：已经送审或有部门审核，不能【新增】！", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DataBase3JB_JJRLR dataBase3JB_JJRLR = new DataBase3JB_JJRLR() { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = selectTime.Year, TheMonth = selectTime.Month, TheDay = selectTime.Day };
            FrmModify<DataBase3JB_JJRLR> frm = new FrmModify<DataBase3JB_JJRLR>(dataBase3JB_JJRLR, header, OptionType.Add, Text, 4, 33);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                Frm3JB_JJRLR_Load(null, null);
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3JB_JJRLR> list = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.TheDay == selectTime.Day).ToList();
            if (list.Count > 0 && (list[0].IsKF_Manager || list[0].IsPG_Manager || list[0].IsPMC_Manager || list[0].IsWX_Manager))
            {
                MessageBox.Show("错误：已经送审或有部门审核，不能【重新计算】？", "禁止操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            Enabled = false;
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase3JB_JJRLR>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var dataBase3JB_JJRLR = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_JJRLR;
                if (dataBase3JB_JJRLR.IsKF_Manager || dataBase3JB_JJRLR.IsPG_Manager || dataBase3JB_JJRLR.IsPMC_Manager || dataBase3JB_JJRLR.IsWX_Manager)
                {
                    MessageBox.Show("错误：已经送审或有部门审核，不能修改？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (dataBase3JB_JJRLR != null)
                {
                    if (dataBase3JB_JJRLR.Line == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3JB_JJRLR> frm = new FrmModify<DataBase3JB_JJRLR>(dataBase3JB_JJRLR, header, OptionType.Modify, Text, 4, 33);
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
                var dataBase3JB_JJRLR = dgv.SelectedRows[0].DataBoundItem as DataBase3JB_JJRLR;
                if (dataBase3JB_JJRLR.IsKF_Manager || dataBase3JB_JJRLR.IsPG_Manager || dataBase3JB_JJRLR.IsPMC_Manager || dataBase3JB_JJRLR.IsWX_Manager)
                {
                    MessageBox.Show("错误：已经送审或有部门审核，不能删除？", "禁止操作", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (dataBase3JB_JJRLR != null)
                    {
                        if (dataBase3JB_JJRLR.Line == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + selectTime.ToString("yyyy年MM月dd日") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                                {
                                    if (conn.Execute("delete from DataBase3JB_JJRLR where TheYear=" + selectTime.Year + " and TheMonth=" + selectTime.Month + " and TheDay=" + selectTime.Day) > 0)
                                    {
                                        Frm3JB_JJRLR_Load(null, null);
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
                        FrmModify<DataBase3JB_JJRLR> frm = new FrmModify<DataBase3JB_JJRLR>(dataBase3JB_JJRLR, header, OptionType.Delete, Text, 5, 34);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
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
            var one = new BaseDal<DataBase3JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
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
                            sql = "update database3jb_jjrlr set isPMC_check=1,pmc_time=getdate(),pmc_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PG_Check:
                            sql = "update database3jb_jjrlr set isPG_check=1,pg_time=getdate(),pg_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.KF_Check:
                            sql = "update database3jb_jjrlr set isKF_check=1,KF_time=getdate(),KF_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.WX_Check:
                            sql = "update database3jb_jjrlr set isWX_check=1,WX_time=getdate(),WX_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PMCDD_Check:
                            sql = "update database3jb_jjrlr set isPMCDD_check=1,PMCDD_time=getdate(),PMCDD_user='" + Program.User.Name + "'";
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
                            sql = "update database3jb_jjrlr set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,pmc_time=getdate(),pmc_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PG_Check:
                            sql = "update database3jb_jjrlr set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,pg_time=getdate(),pg_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.KF_Check:
                            sql = "update database3jb_jjrlr set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,KF_time=getdate(),KF_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.WX_Check:
                            sql = "update database3jb_jjrlr set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,WX_time=getdate(),WX_user='" + Program.User.Name + "'";
                            break;

                        case CheckType.PMCDD_Check:
                            sql = "update database3jb_jjrlr set PMC_Manager_Time=null,PG_Manager_Time=null,KF_Manager_Time=null,WX_Manager_Time=null,isPMC_check=0,isPG_check=0,isKF_check=0,isWX_check=0,isPMCDD_check=0,PMCDD_time=getdate(),PMCDD_user='" + Program.User.Name + "'";
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
            var data = new BaseDal<DataBase3JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.IsPMC_Check && t.IsPG_Check && t.IsKF_Check && t.IsWX_Check && t.IsPMCDD_Check);
            if (data != null)
            {
                string sql = "update database3jb_jjrlr set PMC_Manager_Time=getdate(),PG_Manager_Time=getdate(),KF_Manager_Time=getdate(),WX_Manager_Time=getdate() where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute(sql);
                }
            }
        }

        private void BJ(DateTime dateTime)
        {
            var data = new BaseDal<DataBase3JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
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

        private void BtnWXCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.WX_Check, true);
        }

        private void BtnWXCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.WX_Check, false);
        }

        private void BtnPMCDDCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PMCDD_Check, true);
        }

        private void BtnPMCDDCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(selectTime, CheckType.PMCDD_Check, false);
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

        private void btnSendToCheck_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请确定数据完整无误后再送审，送审后不能修改、删除、新增操作。", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DateTime dateTime = Convert.ToDateTime(dtp.text);
                string sql = "update database3jb_jjrlr set PMC_Manager_Time=getdate() where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();
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
            RefDgv(selectTime.Year, selectTime.Month, txtUserCode.Text);
        }

        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            if (!CheckTime(dtp.text))
            {
                return;
            }
            btnMonthSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_JJRLR> list = dgv.DataSource as List<DataBase3JB_JJRLR>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase3JB_JJRLR>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——计件日录入.xlsx", saveFileDlg.FileName, list, 2, 4, 33, 0, 0, 0, selectTime.ToString("yyyy-MM")))
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