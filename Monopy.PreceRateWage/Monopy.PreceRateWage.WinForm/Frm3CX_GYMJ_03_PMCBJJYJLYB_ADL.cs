using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Drawing;
using System.Linq;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3CX_GYMJ_03_PMCBJJYJLYB_ADL : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工号$类别编码$类别名称$员工编码$姓名$工厂$工段编码$工段名称$开窑量$一级品$降级$破损".Split('$');

        public Frm3CX_GYMJ_03_PMCBJJYJLYB_ADL()
        {
            InitializeComponent();
        }

        private void InitCmb(DateTime dateTime)
        {
            var list = new BaseDal<DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).DistinctBy(t => t.LBMC).OrderBy(t => t.LBMC).ToList();
            list.Insert(0, new DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL() { LBMC = "全部" });
            cmbLB.DataSource = list;
            cmbLB.DisplayMember = "LBMC";
            cmbLB.ValueMember = "LBMC";
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            InitCmb(dtp.Value);
        }

        private void Frm3CX_GYMJ_03_PMCBJJYJLYB_ADL_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtGH.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gh, string lbmc)
        {
            var datas = new BaseDal<DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GH.Contains(gh) && (lbmc.Equals("全部") ? true : t.LBMC.Equals(lbmc))).ToList().OrderBy(t => decimal.TryParse(t.No, out decimal d) ? d : 0M).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL(datas));
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtGH.Text, cmbLB.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
        }
    }
}