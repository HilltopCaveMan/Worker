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
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_CX_03JJ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$班组$班组名称$工号$员工编码$姓名$工段$存货名称$类别编码$类别名称$开窑量$一级品$破损数$单价$一级率$交坯数$大裂数量$双锅次数$模型数$注浆次数$模型数2$注浆次数2$实际交坯率$应交坯率$目标一级率$目标一级率2$应交坯率（产量）$产量上浮基数比$产量上浮单价$质量上浮基数比$质量下浮基数比$质量上浮百分点$质量下浮百分点$上封顶单价（%）$下封顶单价（%）$双锅单价$产量考核$质量考核$单价变动最后$双锅金额$计件总额$工资总额".Split('$');
        private string[] header7 = "创建日期$创建人$年$月$序号$月份$班组$班组名称$工厂$工号$员工编码$姓名$工段$存货编码$存货名称$类别编码$类别名称$开窑量$一级品$破损数$单价$一级率$交坯数$大裂数量$双锅次数$模型数$注浆次数$模型数2$注浆次数2$实际交坯率$应交坯率$目标一级率$享受等级$单价变动$残扣率$有无残扣$双锅金额$计件总额$工资总额".Split('$');

        public Frm3CX_CX_03JJ()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbBZName(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.BZName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZName.DataSource = list2;
            CmbBZName.DisplayMember = "BZName";
            CmbBZName.Text = string.Empty;
        }

        private void RefCmbGH(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGH.DataSource = list2;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = string.Empty;
        }

        private void RefCmbUsercode(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = list2;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbGD(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGD.DataSource = list2;
            CmbGD.DisplayMember = "GD";
            CmbGD.Text = string.Empty;
        }

        private void RefCmbCHMC(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.CHMC).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbCHMC.DataSource = list2;
            CmbCHMC.DisplayMember = "CHMC";
            CmbCHMC.Text = string.Empty;
        }

        private void RefCmbLBMC(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.LBMC).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbLBMC.DataSource = list2;
            CmbLBMC.DisplayMember = "LBMC";
            CmbLBMC.Text = string.Empty;
        }

        private void RefCmbYJPL(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.YJPL).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbYJPL.DataSource = list2;
            CmbYJPL.DisplayMember = "YJPL";
            CmbYJPL.Text = string.Empty;
        }

        private void RefCmbMBYJL(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.MBYJL).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbMBYJL.DataSource = list2;
            CmbMBYJL.DisplayMember = "MBYJL";
            CmbMBYJL.Text = string.Empty;
        }

        private void RefCmbMBYJL2(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.MBYJL2).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbMBYJL2.DataSource = list2;
            CmbMBYJL2.DisplayMember = "MBYJL2";
            CmbMBYJL2.Text = string.Empty;
        }

        private void RefCmbYJPL_CL(List<DataBase3CX_CX_03JJ> list)
        {
            var list2 = list.GroupBy(t => t.YJPL_CL).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbYJPL_CL.DataSource = list2;
            CmbYJPL_CL.DisplayMember = "YJPL_CL";
            CmbYJPL_CL.Text = string.Empty;
        }

        private void InitUi()
        {
            var list = new BaseDal<DataBase3CX_CX_03JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbBZName(list);
            RefCmbGH(list);
            RefCmbUsercode(list);
            RefCmbGD(list);
            RefCmbCHMC(list);
            RefCmbLBMC(list);
            RefCmbYJPL(list);
            RefCmbMBYJL(list);
            RefCmbMBYJL2(list);
            RefCmbYJPL_CL(list);
        }

        private void Frm3CX_CX_03JJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUi();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string bzName, string gh, string userCode, string gd, string chmc, string lbmc, string yjpl, string mbyjl, string mbyjl2, string yjpl_cl)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_03JJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZName.Contains(bzName) && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.GD.Contains(gd) && t.CHMC.Contains(chmc) && t.LBMC.Contains(lbmc) && t.YJPL.Contains(yjpl) && t.MBYJL.Contains(mbyjl) && t.MBYJL2.Contains(mbyjl2) && t.YJPL_CL.Contains(yjpl_cl)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_03JJ(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns["DJ"].Frozen = true;

            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbBZName.Text, CmbGH.Text, CmbUserCode.Text, CmbGD.Text, CmbCHMC.Text, CmbLBMC.Text, CmbYJPL.Text, CmbMBYJL.Text, CmbMBYJL2.Text, CmbYJPL_CL.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——成型计件7通.xlsx");
        }

        private List<DataBase3CX_CX_03JJ> Recount(List<DataBase3CX_CX_03JJ_7T> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            try
            {
                var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "注修工" && !string.IsNullOrEmpty(t.TypesName));
                var listResult = new List<DataBase3CX_CX_03JJ>();
                foreach (var item in list)
                {
                    var itemResult = new DataBase3CX_CX_03JJ
                    {
                        Id = item.Id,
                        CreateTime = item.CreateTime,
                        CreateUser = item.CreateUser,
                        TheYear = item.TheYear,
                        TheMonth = item.TheMonth,
                        No = item.No,
                        BZ = item.BZ.Substring(3),
                        BZName = item.BZName,
                        GH = item.GH,
                        UserCode = item.UserCode,
                        UserName = item.UserName,
                        GD = item.GD,
                        CHMC = item.CHMC,
                        LBBM = item.LBBM,
                        LBMC = item.LBMC,
                        KYL = item.KYL,
                        YJP = item.YJP,
                        PSS = item.PSS,
                        DJ = item.DJ,
                        JPS = item.JPS,
                        DLSL = item.DLSL,
                        SGCS = item.SGCS,
                        MXS = item.MXS,
                        ZJCS = item.ZJCS,
                        MXS2 = item.MXS2,
                        ZJCS2 = item.ZJCS2
                    };
                    decimal.TryParse(itemResult.DJ, out decimal mm);
                    decimal.TryParse(itemResult.KYL, out decimal jj);
                    decimal.TryParse(itemResult.YJP, out decimal kk);
                    var nn = jj == 0 ? 0M : (kk / jj);
                    itemResult.YJL = nn.ToString();
                    decimal.TryParse(itemResult.JPS, out decimal oo);

                    decimal.TryParse(itemResult.SGCS, out decimal qq);

                    decimal.TryParse(itemResult.MXS, out decimal rr);
                    decimal.TryParse(itemResult.ZJCS, out decimal ss);
                    decimal.TryParse(itemResult.MXS2, out decimal tt);
                    decimal.TryParse(itemResult.ZJCS2, out decimal uu);
                    var vv = (rr * ss == 0M) ? 0M : (oo / (rr * ss + tt * uu));
                    itemResult.SJJPL = vv.ToString();
                    var itemDay = listDay.Where(t => t.TypesName == itemResult.CHMC).FirstOrDefault();
                    if (itemDay == null)
                    {
                        MessageBox.Show(itemResult.CHMC + ",基础数据中不存在，无法计算！");
                        return null;
                    }
                    itemResult.YJPL = itemDay.GRKHZB1;
                    decimal.TryParse(itemResult.YJPL, out decimal ww);
                    itemResult.MBYJL = itemDay.GRJSB1;
                    decimal.TryParse(itemResult.MBYJL, out decimal xx);
                    itemResult.MBYJL2 = itemDay.GRJLDJ1;
                    decimal.TryParse(itemResult.MBYJL2, out decimal yy);
                    itemResult.YJPL_CL = itemDay.GRFKDJ1;
                    decimal.TryParse(itemResult.YJPL_CL, out decimal zz);

                    itemResult.CLSFJSB = itemDay.GRKHZB2;
                    decimal.TryParse(itemResult.CLSFJSB, out decimal aa);
                    itemResult.CLSFDJ = itemDay.GRJSB2;
                    decimal.TryParse(itemResult.CLSFDJ, out decimal ab);
                    itemResult.ZLSFJSB = itemDay.GRJLDJ2;
                    decimal.TryParse(itemResult.ZLSFJSB, out decimal ac);
                    itemResult.ZLXFJSB = itemDay.GRKHZB3;
                    decimal.TryParse(itemResult.ZLXFJSB, out decimal ad);
                    itemResult.ZLSFBFD = itemDay.GRFKDJ2;
                    decimal.TryParse(itemResult.ZLSFBFD, out decimal ae);
                    itemResult.ZLXFBFD = itemDay.GRJSB3;
                    decimal.TryParse(itemResult.ZLXFBFD, out decimal af);
                    itemResult.SFDDJ = itemDay.GRJLDJ3;
                    decimal.TryParse(itemResult.SFDDJ, out decimal ag);
                    itemResult.XFDDJ = itemDay.GRFKDJ3;
                    decimal.TryParse(itemResult.XFDDJ, out decimal ah);
                    itemResult.SGDJ = itemDay.GRJLDJ4;
                    decimal.TryParse(itemResult.SGDJ, out decimal ai);
                    var aj = 0M;
                    if (zz <= 0)
                    {
                        aj = 0M;
                    }
                    else
                    {
                        if (vv <= zz)
                        {
                            aj = 0M;
                        }
                        else
                        {
                            aj = (vv - zz) / aa * ab;
                        }
                    }
                    itemResult.CLKH = aj.ToString();

                    var ak = 0M;
                    if (xx <= 0)
                    {
                        ak = 0M;
                    }
                    else
                    {
                        if (nn >= xx && vv >= ww)
                        {
                            ak = (nn - xx) / ac * mm * ae;
                        }
                        else
                        {
                            if (nn < xx && vv >= ww)
                            {
                                ak = (nn - xx) / ad * mm * af;
                            }
                            else
                            {
                                if (nn <= xx && vv <= ww)
                                {
                                    ak = (nn - xx) / ad * mm * af;
                                }
                                else
                                {
                                    ak = 0;
                                }
                            }
                        }
                    }
                    itemResult.ZLKH = ak.ToString();
                    var al = 0M;
                    if (ah <= 0 && ag <= 0)
                    {
                        al = aj + ak;
                    }
                    else
                    {
                        if (ak <= -mm * ah && ah != 0)
                        {
                            al = aj - mm * ah;
                        }
                        else
                        {
                            if (ak >= mm * ag && ag != 0)
                            {
                                al = aj + mm * ag;
                            }
                            else
                            {
                                al = aj + ak;
                            }
                        }
                    }
                    itemResult.DJBDZH = al.ToString();
                    var am = 0M;
                    am = qq * ai;
                    itemResult.SGJE = am.ToString();
                    var an = 0M;
                    an = (mm + al) * kk;
                    itemResult.JJZE = an.ToString();
                    var ao = 0M;
                    ao = am + an;
                    itemResult.GZZE = ao.ToString();
                    listResult.Add(itemResult);
                }
                return listResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void Import(string fileName)
        {
            var list7T = new ExcelHelper<DataBase3CX_CX_03JJ_7T>().ReadExcel(fileName, 1, 6, 0, 0, 0, true);
            if (list7T == null || list7T.Count <= 1)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < list7T.Count; i++)
            {
                if (!MyDal.IsUserCodeAndNameOK(list7T[i].UserCode, list7T[i].UserName, out string userNameERP))
                {
                    MessageBox.Show("工号：【" + list7T[i].UserCode + "】,姓名：【" + list7T[i].UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }

                var item = list7T[i];
                if (string.IsNullOrEmpty(item.UserCode))
                {
                    list7T.RemoveAt(i);
                }
            }
            Enabled = false;
            for (int i = 0; i < list7T.Count; i++)
            {
                list7T[i].CreateUser = Program.User.ToString();
                list7T[i].CreateTime = Program.NowTime;
                list7T[i].TheYear = dtp.Value.Year;
                list7T[i].TheMonth = dtp.Value.Month;
            }
            var list = Recount(list7T);
            if (list == null)
            {
                Enabled = true;
                return;
            }
            if (new BaseDal<DataBase3CX_CX_03JJ_7T>().Add(list7T) > 0 && new BaseDal<DataBase3CX_CX_03JJ>().Add(list) > 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitUi();
            }
            Enabled = true;
        }

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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3CX_CX_03JJ> list = dgv.DataSource as List<DataBase3CX_CX_03JJ>;
                if (new ExcelHelper<DataBase3CX_CX_03JJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型——成型计件.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_CX_03JJ_7T> list7T = new BaseDal<DataBase3CX_CX_03JJ_7T>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list7T == null && list7T.Count == 0)
            {
                return;
            }
            var list = Recount(list7T);
            foreach (var item in list)
            {
                new BaseDal<DataBase3CX_CX_03JJ>().Edit(item);
            }
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3CX_CX_03JJ DataBase3CX_CX_03JJ)
                {
                    if (DataBase3CX_CX_03JJ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var item = new BaseDal<DataBase3CX_CX_03JJ_7T>().Get(t => t.Id == DataBase3CX_CX_03JJ.Id);

                    FrmModify<DataBase3CX_CX_03JJ_7T> frm = new FrmModify<DataBase3CX_CX_03JJ_7T>(item, header7, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUi();
                        btnRecount.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3CX_CX_03JJ = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_03JJ;
                if (DataBase3CX_CX_03JJ.No == "合计")
                {
                    if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            if (conn.Execute("delete from DataBase3CX_CX_03JJ where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
                            {
                                conn.Execute("delete from DataBase3CX_CX_03JJ_7T where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString());
                                InitUi();
                                btnSearch.PerformClick();
                                MessageBox.Show("全部删除完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("删除失败，请检查您的操作和网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_CX_03JJ != null)
                    {
                        FrmModify<DataBase3CX_CX_03JJ> frm = new FrmModify<DataBase3CX_CX_03JJ>(DataBase3CX_CX_03JJ, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                conn.Execute("delete from DataBase3CX_CX_03JJ_7T where id='" + DataBase3CX_CX_03JJ.Id + "'");
                            }
                            InitUi();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void Frm3CX_CX_03JJ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_CX_03JJ_DragDrop(object sender, DragEventArgs e)
        {
            var tmp = ((Array)e.Data.GetData(DataFormats.FileDrop));
            if (tmp.Length != 1)
            {
                MessageBox.Show("一次只允许导入一个文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var path = tmp.GetValue(0).ToString();
            if (StringHelper.IsDir(path))
            {
                MessageBox.Show("请选择一个文件而不是文件夹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var extension = FileHelper.GetExtension(path);
            if (!extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show($"请选择一个Excel文件，当前选择的文件扩展名为：【{extension}】。{Environment.NewLine}为了所有用户的兼容性一致，请使用新格式（扩展名为【xlsx】）！{ Environment.NewLine }旧的xls格式，请用excle2007以上版本另存……", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Import(path);
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitUi();
        }
    }
}