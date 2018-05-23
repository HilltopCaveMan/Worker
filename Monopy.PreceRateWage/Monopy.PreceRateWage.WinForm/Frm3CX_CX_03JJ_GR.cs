using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_CX_03JJ_GR : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$班组编号$班组名称$员工$工资总额$个人金额".Split('$');

        public Frm3CX_CX_03JJ_GR()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbBZ(List<DataBase3CX_CX_03JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.BZBM).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZ.DataSource = list2;
            CmbBZ.DisplayMember = "BZBM";
            CmbBZ.Text = string.Empty;
        }

        private void RefCmbBZName(List<DataBase3CX_CX_03JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.BZMC).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZName.DataSource = list2;
            CmbBZName.DisplayMember = "BZMC";
            CmbBZName.Text = string.Empty;
        }

        private void RefCmbGH(List<DataBase3CX_CX_03JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGH.DataSource = list2;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase3CX_CX_03JJ_GR>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            RefCmbBZ(list);
            RefCmbBZName(list);
            RefCmbGH(list);
        }

        private void FrmCX_CX_03JJ_GR_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            InitUI();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string bzbm, string bzmc, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_03JJ_GR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZBM.Contains(bzbm) && t.BZMC.Contains(bzmc) && t.GH.Contains(gh)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_03JJ_GR(datas));
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, CmbBZ.Text, CmbBZName.Text, CmbGH.Text);
        }

        private List<DataBase3CX_CX_03JJ_GR> Recount(List<DataBase3CX_CX_03JJ> list, List<DataBase3CX_CX_01JJKHTB_Out> listJJKHTB)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            try
            {
                var listTmp = list.GroupBy(t => new { t.BZ, t.BZName }).Select(t => new { t.Key.BZ, t.Key.BZName, Je = t.Sum(x => decimal.TryParse(x.GZZE, out decimal d) ? d : 0m) });
                var listGr = new List<DataBase3CX_CX_03JJ_GR>();
                var no = 1;
                foreach (var item in listJJKHTB)
                {
                    var itemGr = new DataBase3CX_CX_03JJ_GR();
                    itemGr.Id = Guid.NewGuid();
                    itemGr.CreateTime = Program.NowTime;
                    itemGr.CreateUser = Program.User.ToString();
                    itemGr.TheYear = dtp.Value.Year;
                    itemGr.TheMonth = dtp.Value.Month;
                    itemGr.No = no.ToString();
                    itemGr.BZBM = item.BZBM;
                    itemGr.BZMC = item.BZMC;
                    itemGr.GH = item.GH;
                    var xx = listTmp.Where(t => t.BZ == itemGr.BZBM);
                    if (xx.Count() == 0)
                    {
                        MessageBox.Show("计件中找不到，班组编号：" + itemGr.BZBM + ",班组名称为：" + itemGr.BZMC + ",无法计算！！！");
                        return null;
                    }
                    itemGr.SumMoney = xx.FirstOrDefault().Je.ToString();
                    var yy = listJJKHTB.Where(t => t.BZBM == itemGr.BZBM).Count();
                    itemGr.Money = (xx.FirstOrDefault().Je / yy).ToString();
                    listGr.Add(itemGr);
                    no++;
                }

                return listGr;
            }
            catch
            {
                return null;
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var list = new BaseDal<DataBase3CX_CX_03JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list.Count == 0)
            {
                MessageBox.Show("没有成型计件数据，无法计算！");
                Enabled = true;
                return;
            }
            var listJJKHTB = new BaseDal<DataBase3CX_CX_01JJKHTB_Out>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => t.BZBM).ThenBy(t => int.TryParse(t.GH, out int gh) ? gh : int.MaxValue).ToList();
            if (listJJKHTB.Count == 0)
            {
                MessageBox.Show("没有成型计件考核提报数据，无法计算！");
                Enabled = true;
                return;
            }

            var listGr = Recount(list, listJJKHTB);
            if (listGr != null)
            {
                new BaseDal<DataBase3CX_CX_03JJ_GR>().ExecuteSqlCommand("delete from DataBase3CX_CX_03JJ_GR where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);
                new BaseDal<DataBase3CX_CX_03JJ_GR>().Add(listGr);
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Enabled = true;
                MessageBox.Show("操作失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                List<DataBase3CX_CX_03JJ_GR> list = dgv.DataSource as List<DataBase3CX_CX_03JJ_GR>;
                if (new ExcelHelper<DataBase3CX_CX_03JJ_GR>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型——成型计件个人.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitUI();
        }
    }
}