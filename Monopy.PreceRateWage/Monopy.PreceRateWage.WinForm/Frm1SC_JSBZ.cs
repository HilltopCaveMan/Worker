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
    public partial class Frm1SC_JSBZ : Office2007Form
    {
        string[] header = "Time$User$Year$Month$工厂$序号$车间$岗位$工号$人员编号$姓名$品种$件数$单价$占比$金额".Split('$');
        private string _factoryNo;
        private string _dept;

        public Frm1SC_JSBZ()
        {
            InitializeComponent();
        }

        public Frm1SC_JSBZ(string args) : this()
        {
            try
            {
                _factoryNo = args.Split('-')[0];
                _dept = args.Split('-')[1];
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
        private void Frm1SC_JSBZ_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbGW.Text, CmbGH.Text, CmbUserCode.Text, CmbUserName.Text, CmbPZ.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——烧成——技术部实验补助.xlsx");
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
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase1SC_JSBZ> list = dgv.DataSource as List<DataBase1SC_JSBZ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase1SC_JSBZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——烧成——技术部实验补助.xlsx", saveFileDlg.FileName, list, 2, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase1SC_JSBZ> list = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.CJ == _dept).ToList();
            var boolOK = Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase1SC_JSBZ>().Edit(item);
            }

            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1SC_JSBZ DataBase1SC_JSBZ)
                {
                    if (DataBase1SC_JSBZ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase1SC_JSBZ> frm = new FrmModify<DataBase1SC_JSBZ>(DataBase1SC_JSBZ, header, OptionType.Modify, Text, 6, 3);
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
                var DataBase1SC_JSBZ = dgv.SelectedRows[0].DataBoundItem as DataBase1SC_JSBZ;
                if (DataBase1SC_JSBZ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase1SC_JSBZ != null)
                    {
                        FrmModify<DataBase1SC_JSBZ> frm = new FrmModify<DataBase1SC_JSBZ>(DataBase1SC_JSBZ, header, OptionType.Delete, Text, 6);
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
                var list = dgv.DataSource as List<DataBase1SC_JSBZ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No == "合计")
                    {
                        list.Remove(item);
                        continue;
                    }
                    new BaseDal<DataBase1SC_JSBZ>().Delete(item);
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
            var list = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.FactoryNo == _factoryNo && t.CJ == _dept).ToList();
            RefCmbGW(list);
            RefCmbGH(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbPZ(list);
        }

        private void RefCmbGW(List<DataBase1SC_JSBZ> list)
        {
            var listTmp = list.GroupBy(t => t.GW).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGW.DataSource = listTmp;
            CmbGW.DisplayMember = "GW";
            CmbGW.Text = "全部";
        }

        private void RefCmbGH(List<DataBase1SC_JSBZ> list)
        {
            var listTmp = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGH.DataSource = listTmp;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase1SC_JSBZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase1SC_JSBZ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbPZ(List<DataBase1SC_JSBZ> list)
        {
            var listTmp = list.GroupBy(t => t.PZ).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbPZ.DataSource = listTmp;
            CmbPZ.DisplayMember = "PZ";
            CmbPZ.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string gw, string gh, string userCode, string userName, string pz)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.FactoryNo == _factoryNo && t.CJ == _dept && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (gw == "全部" ? true : t.GW.Contains(gw)) && (gh == "全部" ? true : t.GH.Contains(gh)) && (pz == "全部" ? true : t.PZ.Contains(pz))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1SC_JSBZ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 7; i++)
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
            List<DataBase1SC_JSBZ> list = new ExcelHelper<DataBase1SC_JSBZ>().ReadExcel(fileName, 2, 7, 0, 0, 0, true);
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
                list[i].FactoryNo = _factoryNo;
                list[i].No = (i + 1).ToString();
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);
            var check = Check(list);
            if (boolOK && check && new BaseDal<DataBase1SC_JSBZ>().Add(list) > 0)
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

        private bool Check(List<DataBase1SC_JSBZ> list)
        {
            foreach (var item in list)
            {
                var datas = new BaseDal<DataBase1SC_SY>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.CPMC == item.PZ && t.FactoryNo == _factoryNo && t.CJ.Contains(_dept)).FirstOrDefault();
                if (datas == null)
                {
                    MessageBox.Show("总窑产品质量分析汇总表(实验)数据没有导入，请联系相关人员先导入数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                decimal.TryParse(datas.KYL, out decimal kyl);
                decimal.TryParse(item.JS, out decimal js);
                if (js > kyl)
                {
                    MessageBox.Show("【" + item.PZ + "】的件数超出了开窑量件数，开窑量件数为：【" + (kyl).ToString() + "】,【" + item.PZ + "】的件数为：【" + js.ToString() + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool Recount(List<DataBase1SC_JSBZ> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var listTJ = new BaseDal<DataBaseDay>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == _factoryNo && t.WorkshopName == _dept);

                foreach (var item in list)
                {
                    var type = listTJ.Where(t => t.TypesName == item.PZ && t.PostName == item.GW).FirstOrDefault();
                    var baseZb = listTJ.Where(t => t.Classification == "技术部实验").FirstOrDefault();
                    item.DJ = type.UnitPrice;
                    item.ZB = baseZb.ZB_JB_JJGZ;
                    decimal.TryParse(item.DJ, out decimal dj);
                    decimal.TryParse(item.JS, out decimal sl);
                    decimal.TryParse(item.ZB, out decimal zb);
                    item.JE = (dj * sl * zb).ToString();
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
