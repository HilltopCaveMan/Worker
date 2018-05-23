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
    public partial class Frm1YL_JJ : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$岗位$人员编号$姓名$类别$数量$单价$计件金额".Split('$');
        public Frm1YL_JJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1YL_JJ_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value,CmbLine.Text,CmbUserCode.Text,CmbUserName.Text,CmbLB.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——原料——计件.xlsx");
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
        #endregion

        #region 调用方法

        private void InitUI()
        {
            var list = new BaseDal<DataBase1YL_JJ>().GetList().ToList();

            RefCmbLine(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private void RefCmbLine(List<DataBase1YL_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.GW).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLine.DataSource = listTmp;
            CmbLine.DisplayMember = "GW";
            CmbLine.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase1YL_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase1YL_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbLB(List<DataBase1YL_JJ> list)
        {
            var listTmp = list.GroupBy(t => t.LB).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLB.DataSource = listTmp;
            CmbLB.DisplayMember = "LB";
            CmbLB.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gw, string userCode, string userName, string lb)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1YL_JJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gw == "全部" ? true : t.GW.Contains(gw)) && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1YL_JJ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
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

        private void Import(string fileName)
        {
            List<DataBase1YL_JJ> list = new ExcelHelper<DataBase1YL_JJ>().ReadExcel(fileName,2, 6, 0, 0, 0, true);
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
            var boolOK = Recount(list);
            if (new BaseDal<DataBase1YL_JJ>().Add(list) > 0)
            {
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

        private bool Recount(List<DataBase1YL_JJ> list)
        {
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
                var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == itemTmp.TheYear && t.CreateMonth == itemTmp.TheMonth && t.FactoryNo == "G001" && t.WorkshopName == "仓储" && t.Classification == "入职补助");

                //台账最大No
                var tmpNo = new BaseDal<DataBase1CC_XTTZ>().GetList().ToList().OrderByDescending(t => int.TryParse(t.No, out int ino) ? ino : 0).FirstOrDefault();
                var no = 1;
                if (tmpNo != null)
                {
                    no = Convert.ToInt32(tmpNo.No) + 1;
                }

                foreach (var item in list)
                {
                    //var itemHrCQ = listHrCQ.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                    //if (itemHrCQ == null)
                    //{
                    //    MessageBox.Show($"工号：{item.UserCode}，姓名：{item.UserName}，没有出勤数据。无法计算，有出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    continue;
                    //}
                    //decimal.TryParse(itemHrCQ.DayTotal, out decimal scq);
                    //decimal.TryParse(itemHrCQ.DayYcq, out decimal ycq);

                    //if (!item.IsBYE)
                    //{
                    //    item.SCQ = scq.ToString();

                    //    item.YCQ = ycq.ToString();

                    //    var itemMonth = listMonth.Where(t => t.PostName == item.GZ && t.MonthData == item.DJGY).FirstOrDefault();
                    //    if (itemMonth == null)
                    //    {
                    //        item.IsBYE = true;
                    //        continue;
                    //    }
                    //    var itemMonthUp = listMonth.Where(t => t.PostName == item.GZ && t.ProductType == "学徒上限").FirstOrDefault();
                    //    item.IsBYE = false;
                    //    item.XTYGZ = itemMonth.MoneyRZBZ;
                    //    decimal.TryParse(item.XTYGZ, out decimal xtygz);
                    //    decimal.TryParse(item.BZTS, out decimal bzts);
                    //    item.BZJE = (xtygz * bzts / ycq).ToString();
                    //    item.SXJE = itemMonthUp.MoneyKH;
                    //    var tmpMoney = new BaseDal<DataBase1CC_XTTZ>().GetList(t => t.CJ == "仓储" && t.GW == item.GZ && t.UserCode == item.UserCode && t.UserName == item.UserName);
                    //    int totalMoney = 0;
                    //    foreach (var tmp in tmpMoney)
                    //    {
                    //        int.TryParse(tmp.Money, out int money);
                    //        totalMoney += money;
                    //    }
                    //    item.XTZE = totalMoney.ToString();
                    //}

                    ////生成台账
                    //if (!string.IsNullOrEmpty(item.BZJE))
                    //{
                    //    var itemTZ = new DataBase1CC_XTTZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = item.TheYear, TheMonth = item.TheMonth, No = no.ToString(), TimeBZ = new DateTime(item.TheYear, item.TheMonth, 1), CJ = "仓储", GW = item.GZ, UserCode = item.UserCode, UserName = item.UserName, Money = item.BZJE };
                    //    listTZ.Add(itemTZ);
                    //}
                    //no++;
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
