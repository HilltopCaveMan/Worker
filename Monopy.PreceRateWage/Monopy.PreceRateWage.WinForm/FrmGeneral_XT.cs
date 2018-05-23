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
    public partial class FrmGeneral_XT : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工种$人员编码$姓名$第几个月$补助天数$应出勤天数$学徒标准$学徒月工资$实出勤$应出勤$补助金额$以得学徒总额$上限金额$核对$补余额".Split('$');

        public FrmGeneral_XT()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGeneral_XT_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbGZ.Text, CmbUserCode.Text, CmbUserName.Text, checkBox1.Checked);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——仓储——入职表.xlsx");
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
        /// 导入补余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var list = new ExcelHelper<DataBaseGeneral_XT>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                var listOld = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.IsBYE && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                foreach (var item in list)
                {
                    var itemOld = listOld.Where(t => t.UserCode == item.UserCode && t.UserName == item.UserName && t.GZ == item.GZ).FirstOrDefault();
                    if (itemOld == null)
                    {
                        MessageBox.Show($"工号：{item.UserCode},姓名{item.UserName},补余额中存在，但是原始数据中不存在，请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        itemOld.BZJE = item.BZJE;
                        new BaseDal<DataBaseGeneral_XT>().Edit(itemOld);
                    }
                }
                btnRecount.PerformClick();
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
                List<DataBaseGeneral_XT> list = dgv.DataSource as List<DataBaseGeneral_XT>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_XT>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——仓储——入职表 .xlsx", saveFileDlg.FileName, list, 2, 6, 1, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 导出补余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBaseGeneral_XT> list = dgv.DataSource as List<DataBaseGeneral_XT>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_XT>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——仓储——入职表.xlsx", saveFileDlg.FileName, list, 2, 6, 1, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBaseGeneral_XT> list = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ> listTZ);
            foreach (var item in list)
            {
                new BaseDal<DataBaseGeneral_XT>().Edit(item);
            }
            if (boolOK)
            {
                //台账处理
                //先删除台账中这个月的旧数据。
                new BaseDal<DataBase1CC_XTTZ>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);

                if (new BaseDal<DataBase1CC_XTTZ>().Add(listTZ) <= 0)
                {
                    MessageBox.Show("台账保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnYZ_Click(object sender, EventArgs e)
        {
            bool IsOk = true;

            var list = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("一厂") && t.Dept.Contains("仓储")).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            List<DataBaseMonth> moths = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo.Contains("一厂") && t.WorkshopName.Contains("仓储")).ToList();

            foreach (var item in list)
            {
                DateTime dateTime = new DateTime(dtp.Value.Year, dtp.Value.Month, 1);
                var listTZ = new BaseDal<DataBase1CC_XTTZ>().GetList(t => t.UserCode == item.UserCode && t.TimeBZ < dateTime);
                var moth = moths.Where(x => x.PostName == item.GZ && x.MonthData == item.DJGY).FirstOrDefault();
                var data = datas.Where(x => x.Position == item.GZ && x.UserCode == item.UserCode && x.UserName == item.UserName).FirstOrDefault();
                if (data == null)
                {
                    IsOk = false;
                    MessageBox.Show($"缺少出勤记录：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int.TryParse(item.BZTS, out int bzts);
                    int.TryParse(data.DayScq, out int scq);
                    int.TryParse(item.YCQTS, out int ycqts);
                    int.TryParse(data.DayYcq, out int ycq);
                    //验证补助天数
                    if (bzts > scq)
                    {
                        IsOk = false;
                        MessageBox.Show($"补助天数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，补助天数为：{item.BZTS}，出勤记录的实出勤为：{data.DayScq}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //验证应出勤天数
                    if (ycq != ycqts)
                    {
                        IsOk = false;
                        MessageBox.Show($"应出勤天数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，应出勤天数为：{item.YCQTS}，出勤记录的应出勤为：{data.DayYcq}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //验证学徒标准
                if (moth == null)
                {
                    IsOk = false;
                    MessageBox.Show($"缺少月工资记录：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (item.XTBZ != moth.MoneyRZBZ)
                    {
                        IsOk = false;
                        MessageBox.Show($"学徒标准不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，学徒标准为：{item.YCQTS}，月工资的入职补助为：{moth.MoneyRZBZ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                int.TryParse(item.DJGY, out int djgy);
                //验证台账
                if (djgy - 1 != listTZ.Count())
                {
                    IsOk = false;
                    MessageBox.Show($"补助月份不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，提报补助月份：{item.DJGY}，台账中只有：{listTZ.Count()}个月！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                int.TryParse(item.HD, out int hd);
                //验证核对
                if (hd < 0)
                {
                    IsOk = false;
                    MessageBox.Show($"核对数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，核对数为：{item.HD}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_XT DataBaseGeneral_XT)
                {
                    if (DataBaseGeneral_XT.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_XT> frm = new FrmModify<DataBaseGeneral_XT>(DataBaseGeneral_XT, header, OptionType.Modify, Text, 5, 8);
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
                var DataBaseGeneral_XT = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_XT;
                if (DataBaseGeneral_XT.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_XT != null)
                    {
                        FrmModify<DataBaseGeneral_XT> frm = new FrmModify<DataBaseGeneral_XT>(DataBaseGeneral_XT, header, OptionType.Delete, Text, 5, 1);
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
                var list = dgv.DataSource as List<DataBaseGeneral_XT>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        continue;
                    }
                    new BaseDal<DataBaseGeneral_XT>().Delete(item);
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
            var list = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbGZ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
        }

        private void RefCmbGZ(List<DataBaseGeneral_XT> list)
        {
            var listTmp = list.GroupBy(t => t.GZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGZ.DataSource = listTmp;
            CmbGZ.DisplayMember = "GZ";
            CmbGZ.Text = "全部";
        }
        private void RefCmbUserCode(List<DataBaseGeneral_XT> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }
        private void RefCmbUserName(List<DataBaseGeneral_XT> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gz, string userCode, string userName, bool isBye)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new List<DataBaseGeneral_XT>();
            if (isBye)
            {
                datas = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gz == "全部" ? true : t.GZ.Contains(gz)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode)) && t.IsBYE == isBye).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            else
            {
                datas = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gz == "全部" ? true : t.GZ.Contains(gz)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode))).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_XT(datas));

            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
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
            List<DataBaseGeneral_XT> list = new ExcelHelper<DataBaseGeneral_XT>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                //if (!MyDal.IsUserCodeAndNameOK(list[i].UserCode, list[i].UserName, out string userNameERP))
                //{
                //    MessageBox.Show("工号：【" + list[i].UserCode + "】,姓名：【" + list[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    Enabled = true;
                //    return;
                //}
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ> listTZ);
            if (new BaseDal<DataBaseGeneral_XT>().Add(list) > 0)
            {
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase1CC_XTTZ>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);
                    
                    if (new BaseDal<DataBase1CC_XTTZ>().Add(listTZ) <= 0)
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

        private bool Recount(List<DataBaseGeneral_XT> list, out List<DataBase1CC_XTTZ> listTZ)
        {
            listTZ = new List<DataBase1CC_XTTZ>();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var itemTmp = list.FirstOrDefault();
                var listHrCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == itemTmp.TheYear && t.TheMonth == itemTmp.TheMonth && t.Factory == "一厂" && t.Dept == "仓储");
                if (listHrCQ == null || listHrCQ.Count() == 0)
                {
                    MessageBox.Show("没有出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == itemTmp.TheYear && t.CreateMonth == itemTmp.TheMonth && t.FactoryNo == "G001" && t.WorkshopName == "仓储");

                //台账最大No
                var tmpNo = new BaseDal<DataBase1CC_XTTZ>().GetList().ToList().OrderByDescending(t => int.TryParse(t.No, out int ino) ? ino : 0).FirstOrDefault();
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

                    if (!item.IsBYE)
                    {
                        item.SCQ = scq.ToString();

                        item.YCQ = ycq.ToString();

                        var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.MonthData == item.DJGY).FirstOrDefault();
                        if (itemMonth == null)
                        {
                            item.IsBYE = true;
                            continue;
                        }
                        var itemMonthUp = listMonth.Where(t => t.PostName == item.GZ && t.Classification == "学徒上限").FirstOrDefault();
                        item.IsBYE = false;
                        item.XTYGZ = itemMonth.MoneyRZBZ;
                        decimal.TryParse(item.XTYGZ, out decimal xtygz);
                        decimal.TryParse(item.BZTS, out decimal bzts);
                        item.BZJE = (xtygz * bzts / ycq).ToString();
                        item.SXJE = itemMonthUp.MoneyKH;
                        var tmpMoney = new BaseDal<DataBase1CC_XTTZ>().GetList(t => t.CJ == "仓储" && t.GW == item.GZ && t.UserCode == item.UserCode && t.UserName == item.UserName);
                        int totalMoney = 0;
                        foreach (var tmp in tmpMoney)
                        {
                            int.TryParse(tmp.Money, out int money);
                            totalMoney += money;
                        }
                        item.XTZE = totalMoney.ToString();
                    }

                    //生成台账
                    if (!string.IsNullOrEmpty(item.BZJE))
                    {
                        var itemTZ = new DataBase1CC_XTTZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), CJ = "仓储", GW = item.GZ, UserCode = item.UserCode, UserName = item.UserName, Money = item.BZJE };
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




        #endregion

      
    }
}
