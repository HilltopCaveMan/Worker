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
    public partial class Frm2JB_FJJJ : Office2007Form
    {
        private string[] header = "Time$User$Year$Month$序号$岗位名称$人员编码$姓名$类别$品种对应类别$数量$单价$计件金额$IsPG_Check$品管审核时间$品管审核人".Split('$');

        private Thread threadSh;

        private void CheckSh()
        {
            Dictionary<object, InvokeData> dic = new Dictionary<object, InvokeData>();
            while (true)
            {
                dic.Clear();
                var theOne = new BaseDal<DataBase2JB_FJJJ>().Get(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                if (theOne != null)
                {

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
                        dic.Add(lblPG, new InvokeData(this) { Text = theOne.IsPG_Check ? "品管审核确认" : "品管退回", BackColor = theOne.IsPG_Check ? Color.Lime : Color.Red });
                    }
                    else
                    {
                        dic.Add(lblPG, new InvokeData(this) { Text = "品管未审核", BackColor = Color.Transparent });
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
                isBtnPgEnable = true;
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

                    if (item.Contains("BtnPGCheckYes") && dic[item])
                    {
                        isBtnPgEnable = true;
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


        private bool isBtnPgEnable = false;

        #endregion 界面控制变量

        public Frm2JB_FJJJ()
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
        private void Frm2JB_FJJJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            UiControl();
            btnMonthSearch.PerformClick();
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
        private void btnMonthSearch_Click(object sender, EventArgs e)
        {
            RefGrid(dtp.Value, CmbUserCode.Text, CmbUserName.Text, CmbPZDYLB.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——检包——复检计件.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase2JB_FJJJ> list = new BaseDal<DataBase2JB_FJJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();

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
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase2JB_FJJJ> list = new BaseDal<DataBase2JB_FJJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();

            Enabled = false;
            Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2JB_FJJJ>().Edit(item);
            }
            Enabled = true;
            btnMonthSearch.PerformClick();
            MessageBox.Show("重新计算完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonthExportExcel_Click(object sender, EventArgs e)
        {
            btnMonthSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase2JB_FJJJ> list = dgv.DataSource as List<DataBase2JB_FJJJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2JB_FJJJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——检包——复检计件.xlsx", saveFileDlg.FileName, list, 2, 5, 3, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase2JB_FJJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_FJJJ;
                if (DataBase2JB_FJJJ.No == "合计")
                {
                    if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        var list = dgv.DataSource as List<DataBase2JB_FJJJ>;
                        dgv.DataSource = null;
                        foreach (var item in list)
                        {
                            if (item.No != "合计")
                            {
                                new BaseDal<DataBase2JB_FJJJ>().Delete(item);
                            }
                        }
                        InitUI();
                        btnMonthSearch.PerformClick();
                        return;
                    }
                }
                else if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2JB_FJJJ != null)
                    {
                        FrmModify<DataBase2JB_FJJJ> frm = new FrmModify<DataBase2JB_FJJJ>(DataBase2JB_FJJJ, header, OptionType.Delete, Text, 5, 3);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnMonthSearch.PerformClick();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 品管确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPGCheckYes_Click(object sender, EventArgs e)
        {
            DoCheck(dtp.Value, true);
        }

        /// <summary>
        /// 品管退回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPGCheckNo_Click(object sender, EventArgs e)
        {
            DoCheck(dtp.Value, false);
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

        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase2JB_FJJJ>().GetList().ToList();
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbPZDYLB(list);
        }

        private void RefCmbUserCode(List<DataBase2JB_FJJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2JB_FJJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbPZDYLB(List<DataBase2JB_FJJJ> list)
        {
            var listTmp = list.GroupBy(t => t.PZDYLB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbPZDYLB.DataSource = listTmp;
            CmbPZDYLB.DisplayMember = "PZDYLB";
            CmbPZDYLB.Text = "全部";
        }

        private void RefGrid(DateTime selectTime, string userCode, string userName, string pzdylb)
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
            List<DataBase2JB_FJJJ> datas = new List<DataBase2JB_FJJJ>();


            datas = new BaseDal<DataBase2JB_FJJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (pzdylb == "全部" ? true : t.PZDYLB == pzdylb)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();

            datas.Insert(0, MyDal.GetTotalDataBase2JB_FJJJ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < header.Length + 1; i++)
            {
                if (i < 5 || i > 13)
                {
                    dgv.Columns[i].Visible = false;
                }
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

        private bool Recount(List<DataBase2JB_FJJJ> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var baseDay = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "检包车间" && t.Classification == item.LB && t.TypesType == item.PZDYLB);
                    if (baseDay == null)
                    {
                        item.DJ = string.Empty;
                        MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月类别：【" + item.LB + "】的指标数据，导入后重新计算！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        item.DJ = baseDay.UnitPrice;
                    }

                    decimal.TryParse(item.DJ, out decimal dj);
                    decimal.TryParse(item.SL, out decimal sl);
                    item.JJJE = (dj * sl).ToString();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Import(string fileName)
        {
            var list = new ExcelHelper<DataBase2JB_FJJJ>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
            if (list == null || list.Count <= 1)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                {
                    MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }

                var item = list[i];
                if (string.IsNullOrEmpty(item.UserCode))
                {
                    list.RemoveAt(i);
                }
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);

            if (new BaseDal<DataBase2JB_FJJJ>().Add(list) > 0)
            {
                Enabled = true;
                InitUI();
                btnMonthSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            Enabled = true;
        }

        private void DoCheck(DateTime dateTime, bool isCheckYesOrNo)
        {
            var one = new BaseDal<DataBase2JB_FJJJ>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (one == null)
            {
                MessageBox.Show("当前月份没有数据，不能审核，请审核的时候仔细检查！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (isCheckYesOrNo)
            {
                if (MessageBox.Show(string.Format("检查无误，确认审核：{0}年{1}月的所有数据吗？", dateTime.Year, dateTime.Month), "审核确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string sql = string.Empty;

                    sql = "update DataBase2JB_FJJJ set isPG_check=1,pg_time=getdate(),pg_user='" + Program.User.Name + "'where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();

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
                }
            }
            else
            {
                if (MessageBox.Show(string.Format("发现错误，确认退回：{0}年{1}月的所有数据吗？" + Environment.NewLine + "注意：退回时如果其他部门已审核通过需要重新审核！", dateTime.Year, dateTime.Month), "退回确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string sql = string.Empty;
                    sql = "update DataBase2JB_FJJJ set isPG_check=0,pg_time=getdate(),pg_user='" + Program.User.Name + "' where TheYear=" + dateTime.Year.ToString() + " and TheMonth=" + dateTime.Month.ToString();

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

        private void BJ(DateTime dateTime)
        {
            var data = new BaseDal<DataBase2JB_FJJJ>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (data != null)
            {
                var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = data.CreateUser.Split('_')[0], MsgTitle = "审核退回", IsDone = false, IsRead = false, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), MsgClass = "审核退回", Msg = $"{dateTime.Year}年{dateTime.Month}月一厂复检计件被审核退回，退回人：{Program.User.Code}_{Program.User.Name}" };
                new BaseDal<DataBaseMsg>().Add(msg);
            }
        }

        #endregion






    }
}