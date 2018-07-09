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
    public partial class Frm2JB_BJSSJJ : Office2007Form
    {
        string[] header = "Time$User$Year$Month$序号$岗位$人员编号$姓名$类别$品种对应类别$数量$单价$计件金额".Split('$');
        public Frm2JB_BJSSJJ()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2JB_BJSSJJ_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbLine.Text, CmbUserCode.Text, CmbUserName.Text, CmbLB.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——检包——补胶、试水计件.xlsx");
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
                List<DataBase2JB_BJSSJJ> list = dgv.DataSource as List<DataBase2JB_BJSSJJ>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2JB_BJSSJJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——检包——补胶、试水计件.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            List<DataBase2JB_BJSSJJ> list = new BaseDal<DataBase2JB_BJSSJJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2JB_BJSSJJ>().Edit(item);
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2JB_BJSSJJ DataBase2JB_BJSSJJ)
                {
                    if (DataBase2JB_BJSSJJ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase2JB_BJSSJJ> frm = new FrmModify<DataBase2JB_BJSSJJ>(DataBase2JB_BJSSJJ, header, OptionType.Modify, Text, 5, 2);
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
                var DataBase2JB_BJSSJJ = dgv.SelectedRows[0].DataBoundItem as DataBase2JB_BJSSJJ;
                if (DataBase2JB_BJSSJJ.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2JB_BJSSJJ != null)
                    {
                        FrmModify<DataBase2JB_BJSSJJ> frm = new FrmModify<DataBase2JB_BJSSJJ>(DataBase2JB_BJSSJJ, header, OptionType.Delete, Text, 5);
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
                var list = dgv.DataSource as List<DataBase2JB_BJSSJJ>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2JB_BJSSJJ>().Delete(item);
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
            var list = new BaseDal<DataBase2JB_BJSSJJ>().GetList().ToList();

            RefCmbLine(list);
            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbLB(list);
        }

        private void RefCmbLine(List<DataBase2JB_BJSSJJ> list)
        {
            var listTmp = list.GroupBy(t => t.GW).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbLine.DataSource = listTmp;
            CmbLine.DisplayMember = "GW";
            CmbLine.Text = "全部";
        }

        private void RefCmbUserCode(List<DataBase2JB_BJSSJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2JB_BJSSJJ> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbLB(List<DataBase2JB_BJSSJJ> list)
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
            var datas = new BaseDal<DataBase2JB_BJSSJJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gw == "全部" ? true : t.GW.Contains(gw)) && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (lb == "全部" ? true : t.LB.Contains(lb))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2JB_BJSSJJ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
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
            List<DataBase2JB_BJSSJJ> list = new ExcelHelper<DataBase2JB_BJSSJJ>().ReadExcel(fileName, 2, 5, 0, 0, 0, true);
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
            }
            var boolOK = Recount(list);
            if (boolOK && new BaseDal<DataBase2JB_BJSSJJ>().Add(list) > 0)
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

        private bool Recount(List<DataBase2JB_BJSSJJ> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var listZY = new BaseDal<DataBase2JB_ZY>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList(); ;

                if (listZY == null || listZY.Count == 0)
                {
                    MessageBox.Show("没有" + dtp.Value.Year + "年" + dtp.Value.Month + "月的大窑月报和回烧月报数据，导入后重新计算！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                int dtl = 0;//大套类
                int gbl = 0;//挂便类
                int ltl = 0;//连体类
                int sxl = 0;//水箱类
                int cz = 0;//槽子
                int gpl = 0;//柜盆类
                int p = 0;//盆类
                int z = 0;//柱类
                int g = 0;//盖
                foreach (var item in listZY)
                {
                    switch (item.CPMC)
                    {
                        case "10-大套类":
                            int.TryParse(item.CPDJ_HG, out int dthg);
                            int.TryParse(item.CPDJ_M, out int dtm);
                            dtl += (dthg + dtm);
                            break;
                        case "10-挂便类":
                            int.TryParse(item.CPDJ_HG, out int gbhg);
                            int.TryParse(item.CPDJ_M, out int gbm);
                            gbl += (gbhg + gbm);
                            break;
                        case "10-连体类":
                            int.TryParse(item.CPDJ_HG, out int lthg);
                            int.TryParse(item.CPDJ_M, out int ltm);
                            ltl += (lthg + ltm);
                            break;
                        case "10-水箱类":
                            int.TryParse(item.CPDJ_HG, out int sxhg);
                            int.TryParse(item.CPDJ_M, out int sxm);
                            sxl += (sxhg + sxm);
                            break;
                        case "10-槽子":
                            int.TryParse(item.CPDJ_HG, out int czhg);
                            int.TryParse(item.CPDJ_M, out int czm);
                            cz += (czhg + czm);
                            break;
                        case "10-柜盆类":
                            int.TryParse(item.CPDJ_HG, out int gphg);
                            int.TryParse(item.CPDJ_M, out int gpm);
                            gpl += (gphg + gpm);
                            break;
                        case "10-盆类":
                            int.TryParse(item.CPDJ_HG, out int phg);
                            int.TryParse(item.CPDJ_M, out int pm);
                            p += (phg + pm);
                            break;
                        case "10-柱类":
                            int.TryParse(item.CPDJ_HG, out int zhg);
                            int.TryParse(item.CPDJ_M, out int zm);
                            z += (zhg + zm);
                            break;
                        case "30-盖":
                            int.TryParse(item.CPDJ_HG, out int ghg);
                            int.TryParse(item.CPDJ_M, out int gm);
                            g += (ghg + gm);
                            break;
                    }
                }


                //补胶对数
                int bjds_bdt = dtl;//补大套
                int bjds_bgb = gbl + cz;//补挂便、蹲便类、槽子
                int bjds_blt = ltl;//补连体
                int bjds_bgp = gpl;//补柜盆
                int bjds_bsx = sxl + g;//补水箱（含盖）
                int bjds_bmj = p + z;// 补面具、台盆、柱
                //试水对数
                int ssds_dt = dtl * 2;//试水大套
                int ssds_lt = ltl * 3;//试水连体
                int ssds_ltysf = ltl;//连体养水封（含内销连体）
                //品种对应类别
                int lb_bdt = 0;//补大套
                int lb_bgb = 0;//补挂便、蹲便类、槽子
                int lb_blt = 0;//补连体
                int lb_bgp = 0;//补柜盆
                int lb_bsx = 0;//补水箱（含盖）
                int lb_bmj = 0;// 补面具、台盆、柱
                int lb_dt = 0;//试水大套
                int lb_lt = 0;//试水连体
                int lb_ltysf = 0;//连体养水封（含内销连体）

                foreach (var item in list)
                {
                    switch (item.PZDYLB)
                    {
                        case "补大套":
                            int.TryParse(item.SL, out int dtsl);
                            lb_bdt += dtsl;
                            break;
                        case "补挂便、蹲便类、槽子":
                            int.TryParse(item.SL, out int gbsl);
                            lb_bgb += gbsl;
                            break;
                        case "补连体":
                            int.TryParse(item.SL, out int ltsl);
                            lb_blt += ltsl;
                            break;
                        case "补柜盆":
                            int.TryParse(item.SL, out int pgsl);
                            lb_bgp += pgsl;
                            break;
                        case "补水箱（含盖）":
                            int.TryParse(item.SL, out int sxsl);
                            lb_bsx += sxsl;
                            break;
                        case "补面具、台盆、柱":
                            int.TryParse(item.SL, out int mjsl);
                            lb_bmj += mjsl;
                            break;
                        case "试水大套":
                            int.TryParse(item.SL, out int ssdtsl);
                            lb_dt += ssdtsl;
                            break;
                        case "试水连体":
                            int.TryParse(item.SL, out int ssltsl);
                            lb_lt += ssltsl;
                            break;
                        case "连体养水封（含内销连体）":
                            int.TryParse(item.SL, out int ysfsl);
                            lb_ltysf += ysfsl;
                            break;
                    }
                }

                if (bjds_bdt > lb_bdt)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补大套对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (bjds_bgb > lb_bgb)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补挂便、蹲便类、槽子对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (bjds_blt > lb_blt)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补连体对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (bjds_bgp > lb_bgp)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补柜盆对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (bjds_bsx > lb_bsx)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补水箱（含盖）对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (bjds_bmj > lb_bmj)
                {
                    MessageBox.Show("类别为补胶的品种对应类别为补面具、台盆、柱对应的数量大于补胶对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (ssds_dt > lb_lt)
                {
                    MessageBox.Show("类别为试水的品种对应类别为试水大套对应的数量大于试水对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (ssds_lt > lb_bdt)
                {
                    MessageBox.Show("类别为试水的品种对应类别为连体养水封（含内销连体）对应的数量大于试水对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (ssds_ltysf > lb_ltysf)
                {
                    MessageBox.Show("类别为试水的品种对应类别为补大套对应的数量大于试水对数对应的合计，导入失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                foreach (var item in list)
                {
                    var baseDay = new BaseDal<DataBaseDay>().Get(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G002" && t.WorkshopName == "检包车间" && t.TypesType == item.PZDYLB && t.Classification == item.LB);
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

        #endregion

    }
}
