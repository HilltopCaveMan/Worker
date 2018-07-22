using DevComponents.DotNetBar;
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
    public partial class Frm2MJ_SCXTDay : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工种$人员编码$姓名$补助天数$应出勤天数$学徒标准$学徒月工资$实出勤$应出勤$补助金额$累计已得学徒天数".Split('$');

        public Frm2MJ_SCXTDay()
        {
            InitializeComponent();
        }

        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2MJ_SCXTDay_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbGZ.Text, CmbUserCode.Text, CmbUserName.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板四厂——学徒——入职表.xlsx");
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
                List<DataBase2MJ_SCXTDay> list = dgv.DataSource as List<DataBase2MJ_SCXTDay>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2MJ_SCXTDay>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出四厂——学徒——入职表.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2MJ_SCXTDay> list = new BaseDal<DataBase2MJ_SCXTDay>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list, out List<DataBase2MJ_SCXTTZ> listTZ);

            if (Check(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBase2MJ_SCXTDay>().Edit(item);
                }
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase2MJ_SCXTTZ>().ExecuteSqlCommand("delete from DataBase2MJ_SCXTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);

                    if (new BaseDal<DataBase2MJ_SCXTTZ>().Add(listTZ) <= 0)
                    {
                        MessageBox.Show("台账保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnYZ_Click(object sender, EventArgs e)
        {
            bool IsOk = true;

            var list = new BaseDal<DataBase2MJ_SCXTDay>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList(); ;
            IsOk = Check(list);
            if (IsOk)
            {
                MessageBox.Show("验证通过", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("至少有一条数据验收失败！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2MJ_SCXTDay DataBase2MJ_SCXTDay)
                {
                    if (DataBase2MJ_SCXTDay.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2MJ_SCXTDay> frm = new FrmModify<DataBase2MJ_SCXTDay>(DataBase2MJ_SCXTDay, header, OptionType.Modify, Text, 5, 5);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUI();
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
                var DataBase2MJ_SCXTDay = dgv.SelectedRows[0].DataBoundItem as DataBase2MJ_SCXTDay;
                if (DataBase2MJ_SCXTDay.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2MJ_SCXTDay != null)
                    {
                        FrmModify<DataBase2MJ_SCXTDay> frm = new FrmModify<DataBase2MJ_SCXTDay>(DataBase2MJ_SCXTDay, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnRecount.PerformClick();
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
                var list = dgv.DataSource as List<DataBase2MJ_SCXTDay>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2MJ_SCXTDay>().Delete(item);
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
            var list = new BaseDal<DataBase2MJ_SCXTDay>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbGZ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
        }

        private void RefCmbGZ(List<DataBase2MJ_SCXTDay> list)
        {
            var listTmp = list.GroupBy(t => t.GZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGZ.DataSource = listTmp;
            CmbGZ.DisplayMember = "GZ";
            CmbGZ.Text = "全部";
        }
        private void RefCmbUserCode(List<DataBase2MJ_SCXTDay> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }
        private void RefCmbUserName(List<DataBase2MJ_SCXTDay> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gz, string userCode, string userName)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new List<DataBase2MJ_SCXTDay>();

            datas = new BaseDal<DataBase2MJ_SCXTDay>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode))).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            datas.Insert(0, MyDal.GetTotalDataBase2MJ_SCXTDay(datas));

            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgv.ClearSelection();
        }

        private void Import(string fileName)
        {
            List<DataBase2MJ_SCXTDay> list = new ExcelHelper<DataBase2MJ_SCXTDay>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
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
            var boolOK = Recount(list, out List<DataBase2MJ_SCXTTZ> listTZ);
            if (Check(list) && new BaseDal<DataBase2MJ_SCXTDay>().Add(list) > 0)
            {
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase2MJ_SCXTTZ>().ExecuteSqlCommand("delete from DataBase2MJ_SCXTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);

                    if (new BaseDal<DataBase2MJ_SCXTTZ>().Add(listTZ) <= 0)
                    {
                        MessageBox.Show("台账保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

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

        private bool Recount(List<DataBase2MJ_SCXTDay> list, out List<DataBase2MJ_SCXTTZ> listTZ)
        {
            listTZ = new List<DataBase2MJ_SCXTTZ>();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var itemTmp = list.FirstOrDefault();
                var listHrCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == itemTmp.TheYear && t.TheMonth == itemTmp.TheMonth && t.Factory == "二厂");
                if (listHrCQ == null || listHrCQ.Count() == 0)
                {
                    MessageBox.Show("没有出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == itemTmp.TheYear && t.CreateMonth == itemTmp.TheMonth && t.FactoryNo == "G002" && t.WorkshopName.Contains("模具车间"));
                if (listMonth == null || listMonth.Count() == 0)
                {
                    MessageBox.Show("没有月工资数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                //台账最大No
                var tmpNo = new BaseDal<DataBase2MJ_SCXTTZ>().GetList().ToList().OrderByDescending(t => int.TryParse(t.No, out int ino) ? ino : 0).FirstOrDefault();
                var no = 1;
                if (tmpNo != null)
                {
                    no = Convert.ToInt32(tmpNo.No) + 1;
                }

                foreach (var item in list)
                {
                    var itemHrCQ = listHrCQ.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                    if (itemHrCQ == null)
                    {
                        MessageBox.Show($"工号：{item.UserCode}，姓名：{item.UserName}，没有出勤数据。无法计算，有出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    decimal.TryParse(itemHrCQ.DayTotal, out decimal scq);
                    decimal.TryParse(itemHrCQ.DayYcq, out decimal ycq);


                    item.SCQ = scq.ToString();

                    item.YCQ = ycq.ToString();

                    var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.Classification == "四厂学徒").FirstOrDefault();
                    item.XTYGZ = itemMonth.MoneyRZBZ;
                    decimal.TryParse(item.XTBZ, out decimal xtbz);
                    decimal.TryParse(item.YCQTS, out decimal ycqts);
                    decimal.TryParse(item.BZTS, out decimal bzts);
                    item.BZJE = (xtbz / ycqts * bzts).ToString();

                    var tmpMoney = new BaseDal<DataBase2MJ_SCXTTZ>().GetList(t => t.GW == item.GZ && t.UserCode == item.UserCode && t.UserName == item.UserName);
                    int totalMoney = 0;
                    foreach (var tmp in tmpMoney)
                    {
                        int.TryParse(tmp.Money, out int money);
                        totalMoney += money;
                    }
                    item.LJXTTS = totalMoney.ToString();

                    //生成台账
                    if (!string.IsNullOrEmpty(item.BZJE))
                    {
                        var itemTZ = new DataBase2MJ_SCXTTZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), CJ = "模具", TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), GW = item.GZ, UserCode = item.UserCode, UserName = item.UserName, Money = item.BZTS };
                        listTZ.Add(itemTZ);
                    }
                    no++;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool Check(List<DataBase2MJ_SCXTDay> list)
        {
            bool IsOk = true;

            var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName.Contains("模具车间"));
            if (listMonth == null || listMonth.Count() == 0)
            {
                MessageBox.Show("没有月工资数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            foreach (var item in list)
            {

                int.TryParse(item.BZTS, out int bzts);
                int.TryParse(item.SCQ, out int scq);
                int.TryParse(item.YCQTS, out int ycqts);
                int.TryParse(item.YCQ, out int ycq);
                //验证补助天数
                if (bzts > scq)
                {
                    IsOk = false;
                    MessageBox.Show($"补助天数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，补助天数为：{item.BZTS}，出勤记录的实出勤为：{item.SCQ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //验证应出勤天数
                if (ycqts != ycq)
                {
                    IsOk = false;
                    MessageBox.Show($"应出勤天数：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，应出勤天数：{item.YCQTS}，出勤记录的应出勤为：{item.YCQ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                int.TryParse(item.XTBZ, out int xtbz);
                int.TryParse(item.XTYGZ, out int xtygz);
                //学徒标准
                if (xtbz != xtygz)
                {
                    IsOk = false;
                    MessageBox.Show($"学徒标准：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，学徒标准：{item.XTBZ}，月工资的入职补助为：{item.XTYGZ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.Classification == "四厂学徒").FirstOrDefault();

                int.TryParse(item.LJXTTS, out int lj);
                int.TryParse(itemMonth.MoneyKH, out int kh);
                //累计已得学徒天数
                if (lj > kh)
                {
                    IsOk = false;
                    MessageBox.Show($"累计已得学徒天数：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，累计已得学徒天数：{item.LJXTTS}，月工资的考核金额为：{itemMonth.MoneyKH}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            return IsOk;
        }


        #endregion
    }
}
