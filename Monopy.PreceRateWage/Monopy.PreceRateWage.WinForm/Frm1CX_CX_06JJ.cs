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
    public partial class Frm1CX_CX_06JJ : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$班组$班组名称$工号$员工编码$姓名$工段$品种$开窑量$一级品$破损数$单价$一级率$交坯数$大裂$模型数$注浆次数$模型数1$注浆次数1$实际交坯率$目标交坯率$目标一级率$上浮倍数$下浮倍数$目标合格品$考核工资$计件工资$合计$残扣率$是否参与考核$个别调整单价".Split('$');
        private string[] header7 = "创建日期$创建人$年$月$序号$月份$班组$班组名称$工厂$工号$员工编码$姓名$工段$存货编码$存货名称$类别编码$类别名称$开窑量$一级品$破损数$单价$一级率$交坯数$大裂数量$双锅次数$模型数$注浆次数$模型数2$注浆次数2$实际交坯率$应交坯率$目标一级率$享受等级$单价变动$残扣率$有无残扣$双锅金额$计件总额$工资总额".Split('$');

        public Frm1CX_CX_06JJ()
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
        private void Frm1CX_CX_03JJ_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUi();
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbBZName.Text, CmbGH.Text, CmbUserCode.Text, CmbUserName.Text, CmbCHMC.Text);
        }

        /// <summary>
        /// 查看模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——成型计件7通.xlsx");
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
                List<DataBase1CX_CX_06JJ> list = dgv.DataSource as List<DataBase1CX_CX_06JJ>;
                if (new ExcelHelper<DataBase1CX_CX_06JJ>().WriteExcle(Application.StartupPath + "\\Excel\\模板一厂——成型——注修工成型计件.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase1CX_CX_03JJ_7T> list7T = new BaseDal<DataBase1CX_CX_03JJ_7T>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list7T == null && list7T.Count == 0)
            {
                return;
            }
            var list = Recount(list7T);
            foreach (var item in list)
            {
                new BaseDal<DataBase1CX_CX_06JJ>().Edit(item);
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                if (dgv.SelectedRows[0].DataBoundItem is DataBase1CX_CX_06JJ DataBase1CX_CX_03JJ)
                {
                    if (DataBase1CX_CX_03JJ.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var item = new BaseDal<DataBase1CX_CX_03JJ_7T>().Get(t => t.Id == DataBase1CX_CX_03JJ.Id);

                    FrmModify<DataBase1CX_CX_03JJ_7T> frm = new FrmModify<DataBase1CX_CX_03JJ_7T>(item, header7, OptionType.Modify, Text, 5, 0);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        InitUi();
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
                var DataBase1CX_CX_03JJ = dgv.SelectedRows[0].DataBoundItem as DataBase1CX_CX_06JJ;
                if (DataBase1CX_CX_03JJ.No == "合计")
                {
                    if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                        {
                            if (conn.Execute("delete from DataBase1CX_CX_06JJ where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString()) > 0)
                            {
                                conn.Execute("delete from DataBase1CX_CX_03JJ_7T where TheYear=" + dtp.Value.Year.ToString() + " and TheMonth=" + dtp.Value.Month.ToString());
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
                    if (DataBase1CX_CX_03JJ != null)
                    {
                        FrmModify<DataBase1CX_CX_06JJ> frm = new FrmModify<DataBase1CX_CX_06JJ>(DataBase1CX_CX_03JJ, header, OptionType.Delete, Text, 5);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                conn.Execute("delete from DataBase1CX_CX_03JJ_7T where id='" + DataBase1CX_CX_03JJ.Id + "'");
                            }
                            InitUi();
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void Frm1CX_CX_03JJ_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm1CX_CX_03JJ_DragDrop(object sender, DragEventArgs e)
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

        #endregion

        #region 调用方法

        private void InitUi()
        {
            var list = new BaseDal<DataBase1CX_CX_06JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbBZName(list);
            RefCmbGH(list);
            RefCmbUsercode(list);
            RefCmbUserName(list);
            RefCmbCHMC(list);
        }

        private void RefCmbBZName(List<DataBase1CX_CX_06JJ> list)
        {
            var list2 = list.GroupBy(t => t.BZName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZName.DataSource = list2;
            CmbBZName.DisplayMember = "BZName";
            CmbBZName.Text = string.Empty;
        }

        private void RefCmbGH(List<DataBase1CX_CX_06JJ> list)
        {
            var list2 = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGH.DataSource = list2;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = string.Empty;
        }

        private void RefCmbUsercode(List<DataBase1CX_CX_06JJ> list)
        {
            var list2 = list.GroupBy(t => t.UserCode).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserCode.DataSource = list2;
            CmbUserCode.DisplayMember = "UserCode";
            CmbUserCode.Text = string.Empty;
        }

        private void RefCmbUserName(List<DataBase1CX_CX_06JJ> list)
        {
            var list2 = list.GroupBy(t => t.UserName).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbUserName.DataSource = list2;
            CmbUserName.DisplayMember = "UserName";
            CmbUserName.Text = string.Empty;
        }

        private void RefCmbCHMC(List<DataBase1CX_CX_06JJ> list)
        {
            var list2 = list.GroupBy(t => t.CHMC).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbCHMC.DataSource = list2;
            CmbCHMC.DisplayMember = "CHMC";
            CmbCHMC.Text = string.Empty;
        }

        private void RefDgv(DateTime selectTime, string bzName, string gh, string userCode, string userName, string chmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase1CX_CX_06JJ>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZName.Contains(bzName) && t.GH.Contains(gh) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.CHMC.Contains(chmc)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1CX_CX_03JJ(datas));
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

        private List<DataBase1CX_CX_06JJ> Recount(List<DataBase1CX_CX_03JJ_7T> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            try
            {
                var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G001" && t.WorkshopName == "成型车间" && t.PostName == "注修工" && !string.IsNullOrEmpty(t.TypesName));
                var listResult = new List<DataBase1CX_CX_06JJ>();
                foreach (var item in list)
                {
                    var itemResult = new DataBase1CX_CX_06JJ
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
                        KYL = item.KYL,
                        YJP = item.YJP,
                        PSS = item.PSS,
                        DJ = item.DJ,
                        JPS = item.JPS,
                        DLSL = item.DLSL,
                        CKL = item.CKL
                    };

                    var listBKH = new BaseDal<DataBase1CX_CX_04BKH>().Get(t => t.UserCode == itemResult.UserCode);
                    if (listBKH != null)
                    {
                        itemResult.SFCYKH = listBKH.SFCYKH;
                    }

                    var listDLDT = new BaseDal<DataBase1CX_CX_07DLDT>().Get(t => t.UserCode == itemResult.UserCode && t.PZ == itemResult.CHMC);
                    if (listDLDT != null)
                    {
                        itemResult.GBTZDJ = listDLDT.DJ;
                    }
                    var listTB = new BaseDal<DataBase1CX_CX_02JJKHTB>().Get(t => t.PZMC == itemResult.CHMC && t.BZBM == itemResult.BZ);
                    if (listTB != null)
                    {
                        itemResult.MXS = listTB.MXS;
                        itemResult.ZJCS = listTB.ZJCS;
                        itemResult.MXS1 = listTB.MXS2;
                        itemResult.ZJCS1 = listTB.ZJCS2;
                    }

                    decimal.TryParse(itemResult.DJ, out decimal mm);//单价
                    decimal.TryParse(itemResult.KYL, out decimal jj);//开窑量
                    decimal.TryParse(itemResult.YJP, out decimal kk);//一级品
                    var nn = jj == 0 ? 0M : (kk / jj);
                    itemResult.YJL = nn.ToString();

                    decimal.TryParse(itemResult.JPS, out decimal oo);//交坯数
                    decimal.TryParse(itemResult.MXS, out decimal rr);//模型数
                    decimal.TryParse(itemResult.ZJCS, out decimal ss);//注浆次数
                    decimal.TryParse(itemResult.MXS1, out decimal tt);//模型数1
                    decimal.TryParse(itemResult.ZJCS1, out decimal uu);//注浆次数1
                    var vv = (rr * ss == 0M) ? 0M : (oo / (rr * ss + tt * uu));
                    itemResult.SJJPL = vv.ToString();
                    itemResult.MBHGP = (rr * ss + tt * uu).ToString();

                    var itemDay = listDay.Where(t => t.TypesName == itemResult.CHMC).FirstOrDefault();
                    if (itemDay == null)
                    {
                        MessageBox.Show(itemResult.CHMC + ",基础数据中不存在，无法计算！");
                        return null;
                    }
                    itemResult.YJPL = itemDay.GRKHZB1;
                    itemResult.MBYJL = itemDay.GRKHZB3;
                    itemResult.SFBZ = itemDay.GRJLDJ1;
                    itemResult.XFBS = itemDay.GRFKDJ1;

                    decimal.TryParse(itemResult.MBHGP, out decimal mbhgp);//目标合格品
                    //考核工资
                    if (mbhgp == 0 || itemResult.SFCYKH == "否" || string.IsNullOrEmpty(itemResult.MBYJL) || string.IsNullOrEmpty(itemResult.YJPL))
                    {
                        itemResult.KHGZ = "0";
                    }
                    else
                    {
                        decimal.TryParse(itemResult.SFBZ, out decimal sfbz);//上浮倍数
                        decimal.TryParse(itemResult.XFBS, out decimal xfbz);//下浮倍数

                        if (kk >= mbhgp)
                        {
                            itemResult.KHGZ = ((kk - mbhgp) * mm + sfbz).ToString();
                        }
                        if (kk < mbhgp)
                        {
                            itemResult.KHGZ = ((kk - mbhgp) * mm + xfbz).ToString();
                        }
                    }
                    decimal.TryParse(itemResult.PSS, out decimal pss);//破损数
                    decimal.TryParse(itemResult.CKL, out decimal ckl);//残扣率

                    //计件工资
                    if (string.IsNullOrEmpty(itemResult.GBTZDJ))
                    {
                        itemResult.JJGZ = (kk * mm - pss * mm * ckl).ToString();
                    }
                    else
                    {
                        decimal.TryParse(itemResult.GBTZDJ, out decimal gbtzdj);//个别调整单价
                        itemResult.JJGZ = (kk * gbtzdj - pss * gbtzdj * ckl).ToString();
                    }
                    //合计
                    decimal.TryParse(itemResult.KHGZ, out decimal khgz);//考核工资
                    decimal.TryParse(itemResult.JJGZ, out decimal jjgz);//计件工资
                    itemResult.HJ = (khgz + jjgz).ToString();

                    listResult.Add(itemResult);
                }
                return listResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void Import(string fileName)
        {
            var list7T = new ExcelHelper<DataBase1CX_CX_03JJ_7T>().ReadExcel(fileName, 1, 6, 0, 0, 0, true);
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
            if (new BaseDal<DataBase1CX_CX_03JJ_7T>().Add(list7T) > 0 && new BaseDal<DataBase1CX_CX_06JJ>().Add(list) > 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitUi();
            }
            Enabled = true;
        }

        #endregion
    }
}