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
    public partial class Frm3JB_JJRLR_Month : Office2007Form
    {
        private ExcelHelper<DataBase3JB_JJRLR> excelHelper = new ExcelHelper<DataBase3JB_JJRLR>();
        private string[] header;
        private Thread threadSh;
        private DateTime dateTime;

        public Frm3JB_JJRLR_Month()
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
                var theOne = new BaseDal<DataBase3JB_JJRLR>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
                if (theOne != null)
                {
                    dic.Add(BtnPMCCheck, new InvokeData(this) { Text = theOne.IsPMC_Manager ? "PMC经理已审" : "PMC经理未审", Enable = !theOne.IsPMC_Manager });
                    dic.Add(lblPMC, new InvokeData(this) { Text = theOne.IsPMC_Manager ? "PMC经理已审" : "PMC经理未审", BackColor = theOne.IsPMC_Manager ? Color.Lime : Color.Red });

                    dic.Add(BtnPGCheck, new InvokeData(this) { Text = theOne.IsPG_Manager ? "品管经理已审" : "品管经理未审", Enable = !theOne.IsPG_Manager });
                    dic.Add(lblPG, new InvokeData(this) { Text = theOne.IsPG_Manager ? "品管经理已审" : "品管经理未审", BackColor = theOne.IsPG_Manager ? Color.Lime : Color.Red });

                    dic.Add(BtnKFCheck, new InvokeData(this) { Text = theOne.IsKF_Manager ? "开发经理已审" : "开发经理未审", Enable = !theOne.IsKF_Manager });
                    dic.Add(lblKF, new InvokeData(this) { Text = theOne.IsKF_Manager ? "开发经理已审" : "开发经理未审", BackColor = theOne.IsKF_Manager ? Color.Lime : Color.Red });

                    dic.Add(BtnWXCheck, new InvokeData(this) { Text = theOne.IsWX_Manager ? "外销经理已审" : "外销经理未审", Enable = !theOne.IsWX_Manager });
                    dic.Add(lblWX, new InvokeData(this) { Text = theOne.IsWX_Manager ? "外销经理已审" : "外销经理未审", BackColor = theOne.IsWX_Manager ? Color.Lime : Color.Red });
                }
                else
                {
                    dic.Add(BtnPMCCheck, new InvokeData(this) { Text = "PMC经理未审", Enable = true });
                    dic.Add(lblPMC, new InvokeData(this) { Text = "PMC经理未审", BackColor = Color.Red });

                    dic.Add(BtnPGCheck, new InvokeData(this) { Text = "品管经理未审", Enable = true });
                    dic.Add(lblPG, new InvokeData(this) { Text = "品管经理未审", BackColor = Color.Red });

                    dic.Add(BtnKFCheck, new InvokeData(this) { Text = "开发经理未审", Enable = true });
                    dic.Add(lblKF, new InvokeData(this) { Text = "开发经理未审", BackColor = Color.Red });

                    dic.Add(BtnWXCheck, new InvokeData(this) { Text = "外销经理未审", Enable = true });
                    dic.Add(lblWX, new InvokeData(this) { Text = "外销经理未审", BackColor = Color.Red });
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

        private void Frm3JB_JJRLR_Month_Load(object sender, EventArgs e)
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

        private void RefGdv(DateTime selectTime)
        {
            string sql = @"SELECT TheYear,TheMonth,TheDay
,SUM(case when isnumeric(JE)=1  then CAST(JE as numeric(18,4)) else 0 end) as JE
,SUM(case when isnumeric(f_1)=1  then CAST(f_1 as numeric(18,4)) else 0 end) as F_1
,SUM(case when isnumeric(f_2)=1  then CAST(f_2 as numeric(18,4)) else 0 end) as F_2
,SUM(case when isnumeric(f_3)=1  then CAST(f_3 as numeric(18,4)) else 0 end) as F_3
,SUM(case when isnumeric(f_b_1)=1  then CAST(f_b_1 as numeric(18,4)) else 0 end) as f_b_1
,SUM(case when isnumeric(f_b_2)=1  then CAST(f_b_2 as numeric(18,4)) else 0 end) as f_b_2
,SUM(case when isnumeric(f_b_3)=1  then CAST(f_b_3 as numeric(18,4)) else 0 end) as f_b_3
,SUM(case when isnumeric(f_b_4)=1  then CAST(f_b_4 as numeric(18,4)) else 0 end) as f_b_4
,SUM(case when isnumeric(f_b_5)=1  then CAST(f_b_5 as numeric(18,4)) else 0 end) as f_b_5
,SUM(case when isnumeric(pmc_1)=1  then CAST(pmc_1 as numeric(18,4)) else 0 end) as pmc_1
,SUM(case when isnumeric(pmc_2)=1  then CAST(pmc_2 as numeric(18,4)) else 0 end) as pmc_2
,SUM(case when isnumeric(pmc_3)=1  then CAST(pmc_3 as numeric(18,4)) else 0 end) as pmc_3
,SUM(case when isnumeric(pmc_4)=1  then CAST(pmc_4 as numeric(18,4)) else 0 end) as pmc_4
,SUM(case when isnumeric(pmc_5)=1  then CAST(pmc_5 as numeric(18,4)) else 0 end) as pmc_5
,SUM(case when isnumeric(pmc_6)=1  then CAST(pmc_6 as numeric(18,4)) else 0 end) as pmc_6
,SUM(case when isnumeric(pmc_7)=1  then CAST(pmc_7 as numeric(18,4)) else 0 end) as pmc_7
,SUM(case when isnumeric(pmc_8)=1  then CAST(pmc_8 as numeric(18,4)) else 0 end) as pmc_8
,SUM(case when isnumeric(pmc_9)=1  then CAST(pmc_9 as numeric(18,4)) else 0 end) as pmc_9
,SUM(case when isnumeric(pmc_10)=1  then CAST(pmc_10 as numeric(18,4)) else 0 end) as pmc_10
,SUM(case when isnumeric(pmc_11)=1  then CAST(pmc_11 as numeric(18,4)) else 0 end) as pmc_11
,SUM(case when isnumeric(pmc_12)=1  then CAST(pmc_12 as numeric(18,4)) else 0 end) as pmc_12
,SUM(case when isnumeric(pmc_13)=1  then CAST(pmc_13 as numeric(18,4)) else 0 end) as pmc_13
,SUM(case when isnumeric(pmc_14)=1  then CAST(pmc_14 as numeric(18,4)) else 0 end) as pmc_14
,SUM(case when isnumeric(pmc_15)=1  then CAST(pmc_15 as numeric(18,4)) else 0 end) as pmc_15
,SUM(case when isnumeric(pmc_16)=1  then CAST(pmc_16 as numeric(18,4)) else 0 end) as pmc_16
,SUM(case when isnumeric(pmc_17)=1  then CAST(pmc_17 as numeric(18,4)) else 0 end) as pmc_17
,SUM(case when isnumeric(pmc_18)=1  then CAST(pmc_18 as numeric(18,4)) else 0 end) as pmc_18
,SUM(case when isnumeric(pmc_19)=1  then CAST(pmc_19 as numeric(18,4)) else 0 end) as pmc_19
,SUM(case when isnumeric(pmc_20)=1  then CAST(pmc_20 as numeric(18,4)) else 0 end) as pmc_20
,SUM(case when isnumeric(pmc_21)=1  then CAST(pmc_21 as numeric(18,4)) else 0 end) as pmc_21
,SUM(case when isnumeric(pmc_22)=1  then CAST(pmc_22 as numeric(18,4)) else 0 end) as pmc_22
,SUM(case when isnumeric(pmc_23)=1  then CAST(pmc_23 as numeric(18,4)) else 0 end) as pmc_23
,SUM(case when isnumeric(PMC_B_1)=1  then CAST(PMC_B_1 as numeric(18,4)) else 0 end) as PMC_B_1
,SUM(case when isnumeric(PMC_B_2)=1  then CAST(PMC_B_2 as numeric(18,4)) else 0 end) as PMC_B_2
,SUM(case when isnumeric(PMC_B_3)=1  then CAST(PMC_B_3 as numeric(18,4)) else 0 end) as PMC_B_3
,SUM(case when isnumeric(PMC_B_4)=1  then CAST(PMC_B_4 as numeric(18,4)) else 0 end) as PMC_B_4
,SUM(case when isnumeric(PMC_B_5)=1  then CAST(PMC_B_5 as numeric(18,4)) else 0 end) as PMC_B_5
,SUM(case when isnumeric(PG_1)=1  then CAST(PG_1 as numeric(18,4)) else 0 end) as PG_1
,SUM(case when isnumeric(PG_2)=1  then CAST(PG_2 as numeric(18,4)) else 0 end) as PG_2
,SUM(case when isnumeric(PG_3)=1  then CAST(PG_3 as numeric(18,4)) else 0 end) as PG_3
,SUM(case when isnumeric(PG_4)=1  then CAST(PG_4 as numeric(18,4)) else 0 end) as PG_4
,SUM(case when isnumeric(PG_5)=1  then CAST(PG_5 as numeric(18,4)) else 0 end) as PG_5
,SUM(case when isnumeric(PG_6)=1  then CAST(PG_6 as numeric(18,4)) else 0 end) as PG_6
,SUM(case when isnumeric(PG_7)=1  then CAST(PG_7 as numeric(18,4)) else 0 end) as PG_7
,SUM(case when isnumeric(PG_8)=1  then CAST(PG_8 as numeric(18,4)) else 0 end) as PG_8
,SUM(case when isnumeric(PG_B_1)=1  then CAST(PG_B_1 as numeric(18,4)) else 0 end) as PG_B_1
,SUM(case when isnumeric(PG_B_2)=1  then CAST(PG_B_2 as numeric(18,4)) else 0 end) as PG_B_2
,SUM(case when isnumeric(PG_B_3)=1  then CAST(PG_B_3 as numeric(18,4)) else 0 end) as PG_B_3
,SUM(case when isnumeric(PG_B_4)=1  then CAST(PG_B_4 as numeric(18,4)) else 0 end) as PG_B_4
,SUM(case when isnumeric(PG_B_5)=1  then CAST(PG_B_5 as numeric(18,4)) else 0 end) as PG_B_5
,SUM(case when isnumeric(KF_1)=1  then CAST(KF_1 as numeric(18,4)) else 0 end) as KF_1
,SUM(case when isnumeric(KF_B_1)=1  then CAST(KF_B_1 as numeric(18,4)) else 0 end) as KF_B_1
,SUM(case when isnumeric(KF_B_2)=1  then CAST(KF_B_2 as numeric(18,4)) else 0 end) as KF_B_2
,SUM(case when isnumeric(KF_B_3)=1  then CAST(KF_B_3 as numeric(18,4)) else 0 end) as KF_B_3
,SUM(case when isnumeric(KF_B_4)=1  then CAST(KF_B_4 as numeric(18,4)) else 0 end) as KF_B_4
,SUM(case when isnumeric(KF_B_5)=1  then CAST(KF_B_5 as numeric(18,4)) else 0 end) as KF_B_5
,SUM(case when isnumeric(WX_PMCDD_1)=1  then CAST(WX_PMCDD_1 as numeric(18,4)) else 0 end) as WX_PMCDD_1
,SUM(case when isnumeric(WX_1)=1  then CAST(WX_1 as numeric(18,4)) else 0 end) as WX_1
,SUM(case when isnumeric(WX_B_1)=1  then CAST(WX_B_1 as numeric(18,4)) else 0 end) as WX_B_1
,SUM(case when isnumeric(WX_B_2)=1  then CAST(WX_B_2 as numeric(18,4)) else 0 end) as WX_B_2
,SUM(case when isnumeric(WX_B_3)=1  then CAST(WX_B_3 as numeric(18,4)) else 0 end) as WX_B_3
,SUM(case when isnumeric(WX_B_4)=1  then CAST(WX_B_4 as numeric(18,4)) else 0 end) as WX_B_4
,SUM(case when isnumeric(WX_B_5)=1  then CAST(WX_B_5 as numeric(18,4)) else 0 end) as WX_B_5
FROM [DataBase3JB_JJRLR]
  WHERE TheYear=" + selectTime.Year + @" and TheMonth=" + selectTime.Month + @"
  group by TheYear,TheMonth,TheDay
  order by TheDay";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                var list = conn.Query<DataBase3JB_JJRLR>(sql).ToList();
                var sum = MyDal.GetTotalDataBase3JB_JJRLR(list, selectTime);
                sum.TheDay = 0;
                list.Insert(0, sum);
                foreach (DataGridViewColumn item in dgv.Columns)
                {
                    item.Frozen = false;
                }
                foreach (DataGridViewRow item in dgv.Rows)
                {
                    item.Frozen = false;
                }
                dgv.DataSource = list;
                dgv.Columns[0].Visible = false;
                dgv.Columns[5].Visible = false;
                dgv.Columns[6].Visible = false;
                dgv.Columns[7].Visible = false;
                dgv.Columns[8].Visible = false;
                dgv.Columns[4].Frozen = true;
                dgv.Rows[0].Frozen = true;
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
                dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
                header = "年$月$日$金额$线位$岗位$工号$姓名$刷连体$刷水箱$刷大套$工厂备用1$工厂备用2$工厂备用3$工厂备用4$工厂备用5$包水箱盖（含5755S）$裸套袋连体码托$裸套袋大套、水箱、面具码托$装卸车（各厂调货）$验货贴P.0-2（802盆及大套）$贴验货贴P0 (全贴)$贴发货贴$卸配件$裸套袋$码包$人工提裸水箱、面具$人工提裸大套$人工提裸连体$简包（缠膜）$叉车提裸水箱、面具$叉车提裸大套$叉车提裸连体$叉车协助库房倒库$拉包（北厂）$纸返纸$3517A/3517D大套打纸滑托 $S-4188A水箱打纸滑托 $S-7747调水件$PMC备用1$PMC备用2$PMC备用3$PMC备用4$PMC备用5$纸返纸$拉包（北厂）$叉车协助品管验货倒库$非义务复检大套、蹲便类$非义务复检连体类（含柜盆、挂便）$非义务复检水箱（含盖）$非义务复检面具、台盆、柱$打样品$品管备用1$品管备用2$品管备用3$品管备用4$品管备用5$打样品$开发备用1$开发备用2$开发备用3$开发备用4$开发备用5$售后配件$打样品$外销备用1$外销备用2$外销备用3$外销备用4$外销备用5$操作时间$操作人".Split('$');
                for (int i = 71; i < dgv.Columns.Count; i++)
                {
                    dgv.Columns[i].Visible = false;
                }
                for (int i = 0; i < header.Length; i++)
                {
                    dgv.Columns[i + 1].HeaderText = header[i];
                }
                dgv.ClearSelection();
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (i > 8 && i < 17)
                    {
                        dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    }
                    else if (i >= 17 && i < 45)
                    {
                        dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Blue };
                    }
                    else if (i >= 45 && i < 58)
                    {
                        dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Tomato };
                    }
                    else if (i >= 58 && i < 64)
                    {
                        dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.SteelBlue };
                    }
                    else if (i == 64)
                    {
                        dgv.Columns[i].HeaderCell.Style = new DataGridViewCellStyle { ForeColor = Color.Magenta };
                    }
                }
                dgv.EnableHeadersVisualStyles = false;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefGdv(dateTime);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3JB_JJRLR> list = dgv.DataSource as List<DataBase3JB_JJRLR>;
                if (excelHelper.WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——检包——计件日录入.xlsx", saveFileDlg.FileName, list, 1, 1, 33))
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

        private void DoCheck(DateTime dateTime, CheckType checkType)
        {
            if (MessageBox.Show("检查无误，确认审核", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string sql = string.Empty;
                switch (checkType)
                {
                    case CheckType.PMC_Check:
                        sql = "update database3jb_jjrlr set PMC_Manager_Time=getdate(),PMC_Manager_User='" + Program.User.Name + "'";
                        break;

                    case CheckType.PG_Check:
                        sql = "update database3jb_jjrlr set PG_Manager_Time=getdate(),PG_Manager_User='" + Program.User.Name + "'";
                        break;

                    case CheckType.KF_Check:
                        sql = "update database3jb_jjrlr set KF_Manager_Time=getdate(),KF_Manager_User='" + Program.User.Name + "'";
                        break;

                    case CheckType.WX_Check:
                        sql = "update database3jb_jjrlr set WX_Manager_Time=getdate(),WX_Manager_User='" + Program.User.Name + "'";
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

        private void BtnPMCCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.PMC_Check);
        }

        private void BtnPGCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.PG_Check);
        }

        private void BtnKFCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.KF_Check);
        }

        private void BtnWXCheck_Click(object sender, EventArgs e)
        {
            DoCheck(dateTime, CheckType.WX_Check);
        }

        private void Dtp_ValueChanged(object sender, EventArgs e)
        {
            dateTime = dtp.Value;
            btnSearch.PerformClick();
        }
    }
}