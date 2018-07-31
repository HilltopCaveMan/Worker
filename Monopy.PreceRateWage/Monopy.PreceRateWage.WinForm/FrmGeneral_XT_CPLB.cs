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
    public partial class FrmGeneral_XT_CPLB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$工厂$车间$序号$工种$人员编码$姓名$产品类别$第几个月$补助天数$学徒标准$学徒月工资$实出勤$应出勤$补助金额$已得学徒总额$上限金额$核对$是否补差额".Split('$');
        private string _factoryNo;
        private string _workShop;
        public FrmGeneral_XT_CPLB()
        {
            InitializeComponent();
        }

        public FrmGeneral_XT_CPLB(string args) : this()
        {
            try
            {
                _factoryNo = args.Split('-')[0];
                _workShop = args.Split('-')[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置错误，无法运行此界面，请联系管理员！系统错误信息为：" + ex.Message);
                return;
            }
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGeneral_XT_CPLB_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbGZ.Text, CmbUserCode.Text, CmbUserName.Text, CmbCPLB.Text, checkBox1.Checked);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板通用——入职表.xlsx");
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
                var list = new ExcelHelper<DataBaseGeneral_XT_CPLB>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                var listOld = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.IsBYE && t.FactoryNo == _factoryNo && t.CJ == _workShop && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
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
                        new BaseDal<DataBaseGeneral_XT_CPLB>().Edit(itemOld);
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
                List<DataBaseGeneral_XT_CPLB> list = dgv.DataSource as List<DataBaseGeneral_XT_CPLB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_XT_CPLB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出通用——入职表.xlsx", saveFileDlg.FileName, list, 2, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                List<DataBaseGeneral_XT_CPLB> list = dgv.DataSource as List<DataBaseGeneral_XT_CPLB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_XT_CPLB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出通用——入职表.xlsx", saveFileDlg.FileName, list, 2, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBaseGeneral_XT_CPLB> list = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.CJ == _workShop).ToList();
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ_CPLB> listTZ);
            if (Check(list))
            {
                foreach (var item in list)
                {
                    new BaseDal<DataBaseGeneral_XT_CPLB>().Edit(item);
                }
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase1CC_XTTZ_CPLB>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ_CPLB where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + "and CJ = '" + _workShop + "'" + "and FactoryNo = '" + _factoryNo + "'");

                    if (new BaseDal<DataBase1CC_XTTZ_CPLB>().Add(listTZ) <= 0)
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

            var list = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.CJ == _workShop).ToList();

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
                if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_XT_CPLB DataBaseGeneral_XT_CPLB)
                {
                    if (DataBaseGeneral_XT_CPLB.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_XT_CPLB> frm = new FrmModify<DataBaseGeneral_XT_CPLB>(DataBaseGeneral_XT_CPLB, header, OptionType.Modify, Text, 7, 8);
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
                var DataBaseGeneral_XT_CPLB = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_XT_CPLB;
                if (DataBaseGeneral_XT_CPLB.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_XT_CPLB != null)
                    {
                        FrmModify<DataBaseGeneral_XT_CPLB> frm = new FrmModify<DataBaseGeneral_XT_CPLB>(DataBaseGeneral_XT_CPLB, header, OptionType.Delete, Text, 7, 1);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            InitUI();
                            btnSearch.PerformClick();
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
                var list = dgv.DataSource as List<DataBaseGeneral_XT_CPLB>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBaseGeneral_XT_CPLB>().Delete(item);
                    }

                }
                new BaseDal<DataBase1CC_XTTZ_CPLB>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ_CPLB where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + "and CJ = '" + _workShop + "'" + "and FactoryNo = '" + _factoryNo + "'");
                btnSearch.PerformClick();
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
            var list = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.CJ == _workShop).ToList();
            RefCmbGZ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbCPLB(list);
        }

        private void RefCmbGZ(List<DataBaseGeneral_XT_CPLB> list)
        {
            var listTmp = list.GroupBy(t => t.GZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGZ.DataSource = listTmp;
            CmbGZ.DisplayMember = "GZ";
            CmbGZ.Text = "全部";
        }
        private void RefCmbUserCode(List<DataBaseGeneral_XT_CPLB> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }
        private void RefCmbUserName(List<DataBaseGeneral_XT_CPLB> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbCPLB(List<DataBaseGeneral_XT_CPLB> list)
        {
            var listTmp = list.GroupBy(t => t.CPLB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbCPLB.DataSource = listTmp;
            CmbCPLB.DisplayMember = "CPLB";
            CmbCPLB.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gz, string userCode, string userName, string cplb, bool isBye)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new List<DataBaseGeneral_XT_CPLB>();
            if (isBye)
            {
                datas = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.FactoryNo == _factoryNo && t.CJ == _workShop && (gz == "全部" ? true : t.GZ.Contains(gz)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode)) && (cplb == "全部" ? true : t.CPLB.Contains(cplb)) && t.IsBYE == isBye).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            else
            {
                datas = new BaseDal<DataBaseGeneral_XT_CPLB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.FactoryNo == _factoryNo && t.CJ == _workShop && (gz == "全部" ? true : t.GZ.Contains(gz)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode)) && (cplb == "全部" ? true : t.CPLB.Contains(cplb))).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            }
            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_XT_CPLB(datas));

            dgv.DataSource = datas;
            for (int i = 0; i < 7; i++)
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
            List<DataBaseGeneral_XT_CPLB> list = new ExcelHelper<DataBaseGeneral_XT_CPLB>().ReadExcel(fileName, 2, 7, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Enabled = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
                {
                    list.RemoveAt(i);
                    if (i > 0)
                    {
                        i--;
                    }
                    else
                    {
                        i = -1;
                    }
                    continue;

                }

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
                list[i].CJ = _workShop;
                list[i].FactoryNo = _factoryNo;
            }
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ_CPLB> listTZ);
            if (Check(list) && new BaseDal<DataBaseGeneral_XT_CPLB>().Add(list) > 0)
            {
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    string sql = "delete from DataBase1CC_XTTZ_CPLB where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + "and CJ ='" + _workShop + "'" + "and FactoryNo = '" + _factoryNo + "'";
                    new BaseDal<DataBase1CC_XTTZ_CPLB>().ExecuteSqlCommand(sql);

                    if (new BaseDal<DataBase1CC_XTTZ_CPLB>().Add(listTZ) <= 0)
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

        private bool Recount(List<DataBaseGeneral_XT_CPLB> list, out List<DataBase1CC_XTTZ_CPLB> listTZ)
        {
            listTZ = new List<DataBase1CC_XTTZ_CPLB>();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var itemTmp = list.FirstOrDefault();
                List<DataBaseGeneral_CQ> listHrCQ = new List<DataBaseGeneral_CQ>();
                List<DataBaseMonth> listMonth = new List<DataBaseMonth>();


                listHrCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == itemTmp.TheYear && t.TheMonth == itemTmp.TheMonth && t.Factory == (_factoryNo == "G001" ? "一厂" : "二厂") && t.Dept.Contains(_workShop)).ToList();
                listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == itemTmp.TheYear && t.CreateMonth == itemTmp.TheMonth && t.FactoryNo == _factoryNo && t.WorkshopName.Contains(_workShop)).ToList();

                if (listHrCQ == null || listHrCQ.Count == 0)
                {
                    MessageBox.Show("没有出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                //台账最大No
                var tmpNo = new BaseDal<DataBase1CC_XTTZ_CPLB>().GetList().ToList().OrderByDescending(t => int.TryParse(t.No, out int ino) ? ino : 0).FirstOrDefault();
                var no = 1;
                if (tmpNo != null)
                {
                    no = Convert.ToInt32(tmpNo.No) + 1;
                }

                foreach (var item in list)
                {
                    var itemHrCQ = listHrCQ.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                    decimal ycq = 0M;

                    if (itemHrCQ == null)
                    {
                        MessageBox.Show($"工号：{item.UserCode}，姓名：{item.UserName}，没有出勤数据。无法计算，有出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        item.SCQ = "0";
                        item.YCQ = "0";
                    }
                    else
                    {
                        decimal.TryParse(itemHrCQ.DayTotal, out decimal scq);
                        decimal.TryParse(itemHrCQ.DayYcq, out ycq);
                        item.SCQ = scq.ToString();
                        item.YCQ = ycq.ToString();
                    }
                    if (!item.IsBYE)
                    {

                        var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.MonthData == item.DJGY && t.ProductType == item.CPLB).FirstOrDefault();
                        if (itemMonth == null)
                        {
                            item.IsBYE = true;
                            continue;
                        }
                        var itemMonthUp = listMonth.Where(t => t.PostName == item.GZ && t.Classification == "学徒上限" && t.ProductType == item.CPLB).FirstOrDefault();
                        item.IsBYE = false;
                        item.XTYGZ = itemMonth.MoneyRZBZ;
                        decimal.TryParse(item.XTYGZ, out decimal xtygz);
                        decimal.TryParse(item.BZTS, out decimal bzts);
                        item.BZJE = ycq == 0 ? "0" : (xtygz * bzts / ycq).ToString();
                        item.SXJE = itemMonthUp.MoneyKH;
                        var tmpMoney = new BaseDal<DataBase1CC_XTTZ_CPLB>().GetList(t => t.FactoryNo == _factoryNo && t.CJ == _workShop && t.GW == item.GZ && t.UserCode == item.UserCode && t.UserName == item.UserName && t.LB == item.CPLB);
                        int totalMoney = 0;
                        foreach (var tmp in tmpMoney)
                        {
                            int.TryParse(tmp.Money, out int money);
                            totalMoney += money;
                        }
                        item.XTZE = totalMoney.ToString();
                        decimal.TryParse(item.SXJE, out decimal sxje);
                        item.HD = (sxje - totalMoney).ToString();
                    }


                    //生成台账
                    if (!string.IsNullOrEmpty(item.BZJE))
                    {
                        var itemTZ = new DataBase1CC_XTTZ_CPLB { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), FactoryNo = _factoryNo, CJ = _workShop, GW = item.GZ, UserCode = item.UserCode, UserName = item.UserName, LB = item.CPLB, Money = item.BZJE };
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

        private bool Check(List<DataBaseGeneral_XT_CPLB> list)
        {
            bool IsOk = true;

            foreach (var item in list)
            {
                DateTime dateTime = new DateTime(dtp.Value.Year, dtp.Value.Month, 1);
                var listTZ = new BaseDal<DataBase1CC_XTTZ_CPLB>().GetList(t => t.FactoryNo == _factoryNo && t.CJ == _workShop && t.UserCode == item.UserCode && t.TimeBZ < dateTime);

                var tmp = list.Where(t => t.UserCode == item.UserCode).ToList();
                if (tmp.Count > 1)
                {
                    MessageBox.Show($"工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}！有多条记录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                var totalCount = tmp.Sum(t => string.IsNullOrEmpty(t.BZTS) ? 0 : Convert.ToDecimal(t.BZTS));

                decimal.TryParse(item.SCQ, out decimal scq);
                decimal.TryParse(item.YCQ, out decimal ycq);
                //验证补助天数
                if (totalCount > scq)
                {
                    IsOk = false;
                    MessageBox.Show($"补助天数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，补助天数为：{item.BZTS}，出勤记录的实出勤为：{item.SCQ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //验证学徒标准
                int.TryParse(item.XTBZ, out int xtbz);
                int.TryParse(item.XTYGZ, out int xtygz);

                if (xtbz != xtygz)
                {
                    IsOk = false;
                    MessageBox.Show($"学徒标准不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，学徒标准为：{item.XTBZ}，月工资的入职补助为：{item.XTYGZ}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"学徒总额超上限金额：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，核对数为：{item.HD}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return IsOk;
        }


        #endregion
    }
}
