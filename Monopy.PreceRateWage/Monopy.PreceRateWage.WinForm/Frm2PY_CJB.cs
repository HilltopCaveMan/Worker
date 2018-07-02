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
    public partial class Frm2PY_CJB : Office2007Form
    {

        string[] header = "Time$User$Year$Month$序号$类别$工种$人员编号$姓名$工号$出勤天数$学徒天数$合计$实出勤$应出勤$喷釉工（手工）考核$喷釉工（手工）计件$喷釉工（机械）考核$喷釉工（机械）计件$擦水工（手工）考核$擦水工（手工）计件$擦水工（机械）考核$擦水工（机械）计件".Split('$');
        string[] headerygz = "创建日期$创建人$年$月$类别$工种$人员编码$姓名$工号$出勤天数$学徒天数$实出勤$应出勤$标准$金额".Split('$');
        public Frm2PY_CJB()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm2PY_CJB_Load(object sender, EventArgs e)
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
            RefDgv(dtp.Value, CmbUserCode.Text, CmbUserName.Text, CmbGH.Text);
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 查询手工擦水月工资
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBC_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            foreach (DataGridViewRow item in dgv.Rows)
            {
                item.Frozen = false;
            }
            var datas = new BaseDal<DataBase2PY_CJB_YGZ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerygz.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerygz[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板二厂——喷釉——车间报.xlsx");
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
                List<DataBase2PY_CJB> list = dgv.DataSource as List<DataBase2PY_CJB>;
                var hj = list[0];
                list.RemoveAt(0);
                list.Add(hj);
                if (new ExcelHelper<DataBase2PY_CJB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出二厂——喷釉——车间报.xlsx", saveFileDlg.FileName, list, 2, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase2PY_CJB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase2PY_CJB DataBase2PY_CJB = new DataBase2PY_CJB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase2PY_CJB> frm = new FrmModify<DataBase2PY_CJB>(DataBase2PY_CJB, header, OptionType.Add, Text, 6, 11);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
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
            List<DataBase2PY_CJB> list = new BaseDal<DataBase2PY_CJB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            var boolOK = Recount(list);
            foreach (var item in list)
            {
                new BaseDal<DataBase2PY_CJB>().Edit(item);
            }

            new BaseDal<DataBase2PY_CJB_YGZ>().ExecuteSqlCommand("delete from DataBase2PY_CJB_YGZ where theYear = " + dtp.Value.Year + " and theMonth= " + dtp.Value.Month);

            if (new BaseDal<DataBase2PY_CJB_YGZ>().Add(Recount_YGZ(list)) > 0)
            {
                MessageBox.Show("手工擦水月工资，计算成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 导出手工擦水月工资
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcelXCG_Click(object sender, EventArgs e)
        {
            btnSearchBC.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase2PY_CJB_YGZ> list = dgv.DataSource as List<DataBase2PY_CJB_YGZ>;
                if (new ExcelHelper<DataBase2PY_CJB_YGZ>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——喷釉——手工擦水月工资.xlsx", saveFileDlg.FileName, list, 1, 5, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase2PY_CJB DataBase2PY_CJB)
                {
                    FrmModify<DataBase2PY_CJB> frm = new FrmModify<DataBase2PY_CJB>(DataBase2PY_CJB, header, OptionType.Modify, Text, 6, 11);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
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
                var DataBase2PY_CJB = dgv.SelectedRows[0].DataBoundItem as DataBase2PY_CJB;
                if (DataBase2PY_CJB.No == "合计")
                {
                    MessageBox.Show("【合计】不能删除，要全部删除请点【全部删除】！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2PY_CJB != null)
                    {
                        FrmModify<DataBase2PY_CJB> frm = new FrmModify<DataBase2PY_CJB>(DataBase2PY_CJB, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnRecount.PerformClick();
                            InitUI();
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
                var list = dgv.DataSource as List<DataBase2PY_CJB>;
                dgv.DataSource = null;
                foreach (var item in list)
                {
                    if (item.No != "合计")
                    {
                        new BaseDal<DataBase2PY_CJB>().Delete(item);
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
            var list = new BaseDal<DataBase2PY_CJB>().GetList().ToList();

            RefCmbUserCode(list);
            RefCmbUserName(list);
            RefCmbGH(list);
        }

        private void RefCmbUserCode(List<DataBase2PY_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserCode.DataSource = listTmp;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = "全部";
        }

        private void RefCmbUserName(List<DataBase2PY_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbUserName.DataSource = listTmp;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = "全部";
        }

        private void RefCmbGH(List<DataBase2PY_CJB> list)
        {
            var listTmp = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            listTmp.Insert(0, "全部");
            CmbGH.DataSource = listTmp;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = "全部";
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2PY_CJB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (userCode == "全部" ? true : t.UserCode == userCode) && (userName == "全部" ? true : t.UserName == userName) && (gh == "全部" ? true : t.GH.Contains(gh))).ToList().OrderBy(t => t.No).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2PY_CJB(datas));
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
            List<DataBase2PY_CJB> list = new ExcelHelper<DataBase2PY_CJB>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
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
                list[i].No = (i + 1).ToString();
                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
            }
            var boolOK = Recount(list);
            var check = Check(list);
            if (check && new BaseDal<DataBase2PY_CJB>().Add(list) > 0)
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

        private bool Check(List<DataBase2PY_CJB> list)
        {
            var groupList = list.GroupBy(t => t.GH);
            foreach (var item in groupList)
            {
                decimal pyg = 0M;
                decimal csg = 0M;
                string pygName = string.Empty;
                string csgName = string.Empty;
                List<DataBase2PY_CJB> sl = item.ToList<DataBase2PY_CJB>();//分组后的集合 
                foreach (var gh in sl)
                {
                    if (gh.GZ.Contains("喷釉工"))
                    {
                        pygName = gh.GZ;
                        decimal.TryParse(gh.CQTS, out decimal pycq);
                        pyg += pycq;
                    }
                    if (gh.GZ.Contains("擦水工"))
                    {
                        csgName = gh.GZ;
                        decimal.TryParse(gh.CQTS, out decimal cscq);
                        csg += cscq;
                    }
                }
                if (csg > pyg)
                {
                    MessageBox.Show("工号：【" + item.Key + "】的" + csgName + "对应大于" + pygName + "", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool Recount(List<DataBase2PY_CJB> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                List<DataBaseGeneral_CQ> datas = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.Factory.Contains("一厂") && t.Dept.Contains("喷釉")).ToList().OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
                var listDays = new BaseDal<DataBaseDay>().GetList(h => h.CreateYear == dtp.Value.Year && h.CreateMonth == dtp.Value.Month && h.FactoryNo == "G001" && h.WorkshopName == "喷釉车间").ToList();

                foreach (var item in list)
                {
                    var data = datas.Where(x => x.Position == item.GZ && x.UserCode == item.UserCode && x.UserName == item.UserName).FirstOrDefault();
                    if (data == null)
                    {
                        item.SCQ = "0";
                        item.YCQ = "0";

                        MessageBox.Show("没有【" + item.UserName + "】的出勤数据，无法计算，导入出勤后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        item.SCQ = data.DayScq;
                        item.YCQ = data.DayYcq;
                    }
                    //计算合计
                    decimal.TryParse(item.CQTS, out decimal cqts);
                    decimal.TryParse(item.XTTS, out decimal xtts);
                    item.HJ = (cqts + xtts).ToString();
                    if (item.LB == "手工橱")
                    {
                        DataBase2PY_RJDR rjdr = new BaseDal<DataBase2PY_RJDR>().GetList(t =>  t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && string.IsNullOrEmpty(t.LX_Remark) && t.GH == item.GH).ToList().FirstOrDefault();
                        if (rjdr == null)
                        {
                            MessageBox.Show("没有" + dtp.Value.Year.ToString() + "年" + dtp.Value.Month.ToString() + "月的考核数据，无法计算，导入考核数据后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        switch (item.GZ)
                        {
                            case "喷釉工（手工）":
                                item.PYGSGKH = rjdr.KH;
                                item.PYGSGJJ = rjdr.JJ ;
                                break;
                            case "擦水工（手工）":
                                var listDay = listDays.Where(x => x.PostName == item.GZ).FirstOrDefault();
                                if (listDay == null)
                                {
                                    item.CSGSGKH = "0";
                                    item.CSGSGJJ = "0";

                                    MessageBox.Show("没有" + dtp.Value.Year.ToString() + "年" + dtp.Value.Month.ToString() + "月的指标数据，无法计算，导入指标数据后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                                else
                                {
                                    decimal sum = list.Where(t => t.LB == item.LB && t.GZ == item.GZ && t.GH == item.GH).Sum(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M);
                                    decimal.TryParse(rjdr.JJ, out decimal jjhj);
                                    decimal.TryParse(rjdr.KH, out decimal kh);
                                    decimal.TryParse(listDay.ZB_PY_BZKH, out decimal bzkh);
                                    item.CSGSGKH = ((kh * bzkh) / (sum * cqts)).ToString();
                                    item.CSGSGJJ = ((jjhj * bzkh) / (sum * cqts)).ToString();
                                }
                                break;
                        }
                    }
                    else
                    {
                        DataBase2PY_RJDR rjdr = new BaseDal<DataBase2PY_RJDR>().GetList(t =>  t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month && t.LX_Remark == "机械臂" && t.GH == item.GH).ToList().FirstOrDefault();
                        if (rjdr == null)
                        {
                            MessageBox.Show("没有" + dtp.Value.Year.ToString() + "年" + dtp.Value.Month.ToString() + "月的考核数据，无法计算，导入考核数据后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        decimal.TryParse(rjdr.JJ, out decimal jjhj);
                        decimal.TryParse(rjdr.KH, out decimal kh);
                        var listDay = listDays.Where(x => x.PostName == item.GZ).FirstOrDefault();
                        if (listDay == null)
                        {
                            item.PYGJXKH = "0";
                            item.PYGJXJJ = "0";
                            item.CSGJJKH = "0";
                            item.CSGJXJJ = "0";

                            MessageBox.Show("没有" + dtp.Value.Year.ToString() + "年" + dtp.Value.Month.ToString() + "月的指标数据，无法计算，导入指标数据后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else
                        {
                            decimal.TryParse(listDay.ZB_PY_BZKH, out decimal bzkh);
                            switch (item.GZ)
                            {
                                case "喷釉工（机械）":
                                    item.PYGJXKH = (bzkh * kh).ToString();
                                    item.PYGJXJJ = (bzkh * jjhj).ToString();
                                    break;
                                case "擦水工（机械）":

                                    decimal sum = list.Where(t => t.LB == item.LB && t.GZ == item.GZ && t.GH == item.GH).Sum(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M);

                                    item.CSGJJKH = ((kh * bzkh) / (sum * cqts)).ToString();
                                    item.CSGJXJJ = ((jjhj * bzkh) / (sum * cqts)).ToString();

                                    break;
                            }
                        }

                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<DataBase2PY_CJB_YGZ> Recount_YGZ(List<DataBase2PY_CJB> list)
        {
            List<DataBase2PY_CJB_YGZ> DataBase2PY_CJB_YGZs = new List<DataBase2PY_CJB_YGZ>();
            var listTJ = new BaseDal<DataBaseMonth>().GetList(t => t.CreateYear == dtp.Value.Year && t.CreateMonth == dtp.Value.Month && t.FactoryNo == "G001" && t.WorkshopName == "喷釉车间" && t.Classification == "月工资" && t.PostName == "擦水工（手工）").FirstOrDefault();

            foreach (var item in list)
            {
                DataBase2PY_CJB_YGZ xCGKH = new DataBase2PY_CJB_YGZ { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = list.FirstOrDefault().TheYear, TheMonth = list.FirstOrDefault().TheMonth };
                if (!string.IsNullOrEmpty(item.XTTS) && item.GZ == "擦水工（手工）")
                {
                    xCGKH.LB = item.LB;
                    xCGKH.GZ = item.GZ;
                    xCGKH.UserCode = item.UserCode;
                    xCGKH.UserName = item.UserName;
                    xCGKH.GH = item.GH;
                    xCGKH.CQTS = item.CQTS;
                    xCGKH.XTTS = item.XTTS;
                    xCGKH.SCQ = item.SCQ;
                    xCGKH.YCQ = item.YCQ;
                    if (listTJ == null)
                    {
                        xCGKH.BZ = "0";
                        MessageBox.Show("没有月工资的数据，无法计算，导入月工资后，再重新【计算】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        xCGKH.BZ = listTJ.MoneyRZBZ;
                    }
                    decimal.TryParse(xCGKH.XTTS, out decimal xtts);
                    decimal.TryParse(xCGKH.SCQ, out decimal scq);
                    decimal.TryParse(xCGKH.YCQ, out decimal ycq);
                    decimal.TryParse(xCGKH.CQTS, out decimal cqts);
                    decimal.TryParse(xCGKH.BZ, out decimal bz);
                    if (xtts == scq)
                    {
                        xCGKH.JE = "0";
                    }
                    else if (xtts < scq)
                    {
                        if ((scq - xtts) >= cqts)
                        {
                            xCGKH.JE = (bz / ycq * cqts).ToString();
                        }
                        else
                        {
                            xCGKH.JE = (bz / ycq * (scq - xtts)).ToString();
                        }
                    }
                    DataBase2PY_CJB_YGZs.Add(xCGKH);
                }
            }
            return DataBase2PY_CJB_YGZs;
        }
        #endregion
    }
}
