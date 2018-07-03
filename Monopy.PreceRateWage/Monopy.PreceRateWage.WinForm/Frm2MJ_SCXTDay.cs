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
        private string[] header = "创建日期$创建人$年$月$工厂$序号$车间$工种$人员编码$姓名$补助天数$学徒日工$实出勤$应出勤$补助金额$以得学徒总额$上限金额$核对".Split('$');
      

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
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——学徒——入职表（日）.xlsx");
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
                List<DataBaseGeneral_XTDay> list = dgv.DataSource as List<DataBaseGeneral_XTDay>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBaseGeneral_XTDay>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——学徒——入职表（日） .xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBaseGeneral_XTDay> list = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == _factoryNo && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.CJ == _workShop).ToList();
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ> listTZ);
            foreach (var item in list)
            {
                new BaseDal<DataBaseGeneral_XTDay>().Edit(item);
            }
            if (boolOK)
            {
                //台账处理
                //先删除台账中这个月的旧数据。
                new BaseDal<DataBase1CC_XTTZ>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + "and CJ = '" + _workShop + "'" + "and FactoryNo = '" + _factoryNo + "'");

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

            var list = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == _factoryNo && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.CJ == _workShop);
            List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains((_factoryNo == "G001" ? "一厂" : "二厂")) && t.Dept.Contains(_workShop)).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            foreach (var item in list)
            {
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

                    //验证补助天数
                    if (bzts > scq)
                    {
                        IsOk = false;
                        MessageBox.Show($"补助天数不对：工种：{item.GZ}，人员编码：{item.UserCode}，姓名：{item.UserName}，补助天数为：{item.BZTS}，出勤记录的实出勤为：{data.DayScq}！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBaseGeneral_XTDay DataBaseGeneral_XTDay)
                {
                    if (DataBaseGeneral_XTDay.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBaseGeneral_XTDay> frm = new FrmModify<DataBaseGeneral_XTDay>(DataBaseGeneral_XTDay, header, OptionType.Modify, Text, 7, 8);
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
                var DataBaseGeneral_XTDay = dgv.SelectedRows[0].DataBoundItem as DataBaseGeneral_XTDay;
                if (DataBaseGeneral_XTDay.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBaseGeneral_XTDay != null)
                    {
                        FrmModify<DataBaseGeneral_XTDay> frm = new FrmModify<DataBaseGeneral_XTDay>(DataBaseGeneral_XTDay, header, OptionType.Delete, Text, 7, 1);
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
                var list = dgv.DataSource as List<DataBaseGeneral_XTDay>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        list.Remove(item);
                        continue;
                    }
                    new BaseDal<DataBaseGeneral_XTDay>().Delete(item);
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
            var list = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == _factoryNo && t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.CJ == _workShop).ToList();
            RefCmbGZ(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
        }

        private void RefCmbGZ(List<DataBaseGeneral_XTDay> list)
        {
            var listTmp = list.GroupBy(t => t.GZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGZ.DataSource = listTmp;
            CmbGZ.DisplayMember = "GZ";
            CmbGZ.Text = "全部";
        }
        private void RefCmbUserCode(List<DataBaseGeneral_XTDay> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }
        private void RefCmbUserName(List<DataBaseGeneral_XTDay> list)
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
            var datas = new List<DataBaseGeneral_XTDay>();

            datas = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == _factoryNo && t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CJ == _workShop && (gz == "全部" ? true : t.GZ.Contains(gz)) && (userName == "全部" ? true : t.UserName.Contains(userName)) && (userCode == "全部" ? true : t.UserCode.Contains(userCode))).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();

            datas.Insert(0, MyDal.GetTotalDataBaseGeneral_XTDay(datas));

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
            List<DataBaseGeneral_XTDay> list = new ExcelHelper<DataBaseGeneral_XTDay>().ReadExcel(fileName, 2, 8, 0, 0, 0, true);
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
                list[i].CJ = _workShop;
                list[i].FactoryNo = _factoryNo;
            }
            var boolOK = Recount(list, out List<DataBase1CC_XTTZ> listTZ);
            if (new BaseDal<DataBaseGeneral_XTDay>().Add(list) > 0)
            {
                if (boolOK)
                {
                    //台账处理
                    //先删除台账中这个月的旧数据。
                    new BaseDal<DataBase1CC_XTTZ>().ExecuteSqlCommand("delete from DataBase1CC_XTTZ where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month + "and CJ ='" + _workShop + "'" + "and FactoryNo ='" + _factoryNo + "'");

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

        private bool Recount(List<DataBaseGeneral_XTDay> list, out List<DataBase1CC_XTTZ> listTZ)
        {
            listTZ = new List<DataBase1CC_XTTZ>();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var itemTmp = list.FirstOrDefault();
                var listHrCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == itemTmp.TheYear && t.TheMonth == itemTmp.TheMonth && t.Factory == (_factoryNo == "G001" ? "一厂" : "二厂") && t.Dept.Contains(_workShop));
                if (listHrCQ == null || listHrCQ.Count() == 0)
                {
                    MessageBox.Show("没有出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == itemTmp.TheYear && t.CreateMonth == itemTmp.TheMonth && t.FactoryNo == _factoryNo && t.WorkshopName.Contains(_workShop));

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


                    item.SCQ = scq.ToString();

                    item.YCQ = ycq.ToString();

                    var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.ProductType == "入职补助").FirstOrDefault();

                    var itemMonthUp = listMonth.Where(t => t.PostName == item.GZ && t.ProductType == "学徒上限").FirstOrDefault();

                    item.XTRGZ = itemMonth.DayWork_XT;
                    decimal.TryParse(item.XTRGZ, out decimal xtrgz);
                    decimal.TryParse(item.BZTS, out decimal bzts);
                    item.BZJE = (xtrgz * bzts).ToString();
                    item.SXJE = itemMonthUp.MoneyKH;
                    var tmpMoney = new BaseDal<DataBase1CC_XTTZ>().GetList(t => t.FactoryNo == _factoryNo && t.CJ == _workShop && t.GW == item.GZ && t.UserCode == item.UserCode && t.UserName == item.UserName);
                    int totalMoney = 0;
                    foreach (var tmp in tmpMoney)
                    {
                        int.TryParse(tmp.Money, out int money);
                        totalMoney += money;
                    }
                    item.XTZE = totalMoney.ToString();
                    decimal.TryParse(item.SXJE, out decimal sxje);
                    item.HD = (totalMoney - sxje).ToString();

                    //生成台账
                    if (!string.IsNullOrEmpty(item.BZJE))
                    {
                        var itemTZ = new DataBase1CC_XTTZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), FactoryNo = _factoryNo, CJ = _workShop, GW = item.GZ, UserCode = item.UserCode, UserName = item.UserName, Money = item.BZJE };
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
