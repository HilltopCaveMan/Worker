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
    public partial class Frm2CX_CX_06JJ_GR : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$班组编码$班组名称$工号$员工编码$姓名$产量考核$质量考核$计件工资$产量考核个人$质量考核个人$计件工资个人$个人合计".Split('$');

        public Frm2CX_CX_06JJ_GR()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbBZ(List<DataBase2CX_CX_06JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.BZBM).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZ.DataSource = list2;
            CmbBZ.DisplayMember = "BZBM";
            CmbBZ.Text = string.Empty;
        }

        private void RefCmbBZName(List<DataBase2CX_CX_06JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.BZMC).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbBZName.DataSource = list2;
            CmbBZName.DisplayMember = "BZMC";
            CmbBZName.Text = string.Empty;
        }

        private void RefCmbGH(List<DataBase2CX_CX_06JJ_GR> list)
        {
            var list2 = list.GroupBy(t => t.GH).Select(t => t.Key).OrderBy(t => t).ToList();
            CmbGH.DataSource = list2;
            CmbGH.DisplayMember = "GH";
            CmbGH.Text = string.Empty;
        }

        private void InitUI()
        {
            var list = new BaseDal<DataBase2CX_CX_06JJ_GR>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
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
            var datas = new BaseDal<DataBase2CX_CX_06JJ_GR>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZBM.Contains(bzbm) && t.BZMC.Contains(bzmc) && t.GH.Contains(gh)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2CX_CX_06JJ_GR(datas));
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

        private List<DataBase2CX_CX_06JJ_GR> Recount(List<DataBase2CX_CX_06JJ> list, List<DataBase2CX_CX_02JJKHTB_Out> listJJKHTB)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            try
            {
                var listCLKH = list.GroupBy(t => new { t.BZ, t.BZName }).Select(t => new { t.Key.BZ, t.Key.BZName, CLKH = t.Sum(x => decimal.TryParse(x.CLKH, out decimal d) ? d : 0m), ZLKH = t.Sum(x => decimal.TryParse(x.ZLKH, out decimal d) ? d : 0m) });
                var listJJ = list.GroupBy(t => new { t.BZ, t.BZName }).Select(t => new { t.Key.BZ, t.Key.BZName, JE = t.Sum(x => decimal.TryParse(x.JJGZ, out decimal d) ? d : 0m) });
                var baseCLKH = new BaseDal<DataBaseDay>().Get(h => h.FactoryNo == "G002" && h.WorkshopName == "成型车间" && h.PostName == "注修工" && h.Classification == "产量考核");
                var baseZLKH = new BaseDal<DataBaseDay>().Get(h => h.FactoryNo == "G002" && h.WorkshopName == "成型车间" && h.PostName == "注修工" && h.Classification == "质量考核");
                var baseXTCLKH = new BaseDal<DataBaseDay>().Get(h => h.FactoryNo == "G002" && h.WorkshopName == "成型车间" && h.PostName == "注修工" && h.Classification == "学徒期产量考核");
                var baseXTZLKH = new BaseDal<DataBaseDay>().Get(h => h.FactoryNo == "G002" && h.WorkshopName == "成型车间" && h.PostName == "注修工" && h.Classification == "学徒期质量考核");

                var listGr = new List<DataBase2CX_CX_06JJ_GR>();
                var no = 1;
                foreach (var item in listJJKHTB)
                {
                    var itemGr = new DataBase2CX_CX_06JJ_GR();
                    itemGr.Id = Guid.NewGuid();
                    itemGr.CreateTime = Program.NowTime;
                    itemGr.CreateUser = Program.User.ToString();
                    itemGr.TheYear = dtp.Value.Year;
                    itemGr.TheMonth = dtp.Value.Month;
                    itemGr.No = no.ToString();
                    itemGr.BZBM = item.BZBM;
                    itemGr.BZMC = item.BZMC;
                    itemGr.GH = item.GH;
                    itemGr.UserCode = item.UserCode;
                    itemGr.UserName = item.UserName;
                    var xx = listCLKH.Where(t => t.BZ == itemGr.BZBM);
                    if (xx.Count() == 0)
                    {
                        MessageBox.Show("计件中找不到，班组编号：" + itemGr.BZBM + ",班组名称为：" + itemGr.BZMC + ",无法计算！！！");
                        return null;
                    }

                    var jj = listJJ.Where(t => t.BZ == itemGr.BZBM);
                    if (jj.Count() == 0)
                    {
                        MessageBox.Show("计件中找不到，班组编号：" + itemGr.BZBM + ",班组名称为：" + itemGr.BZMC + ",无法计算！！！");
                        return null;
                    }

                    itemGr.CLKH = xx.FirstOrDefault().CLKH.ToString();
                    itemGr.ZLKH = xx.FirstOrDefault().ZLKH.ToString();
                    itemGr.JJGZ = jj.FirstOrDefault().JE.ToString();
                    var tmp = list.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                    var yy = listJJKHTB.Where(t => t.BZBM == itemGr.BZBM).Count();
                    itemGr.ZLKHGR = (xx.FirstOrDefault().ZLKH / yy).ToString();
                    itemGr.CLKHGR = (xx.FirstOrDefault().CLKH / yy).ToString();

                    if (tmp.SFXTQ == "是" && yy == 1)
                    {
                        if (baseXTCLKH == null || baseXTZLKH == null)
                        {
                            MessageBox.Show("指标数据库中没有学徒期产量考核或者学徒期质量考核的数据，请先导入在【重新计算】");
                            itemGr.ZLKHGR = "0";
                            itemGr.CLKHGR = "0";
                        }
                        else
                        {
                            decimal.TryParse(baseXTCLKH.KHJEXFD, out decimal xtclkhkhxfd);//学徒期产量考核
                            decimal.TryParse(baseXTZLKH.KHJEXFD, out decimal xtzlkhkhxfd);//学徒期质量考核
                            decimal.TryParse(itemGr.ZLKHGR, out decimal zlkhgr);
                            decimal.TryParse(itemGr.CLKHGR, out decimal clkhgr);
                            if (clkhgr < xtclkhkhxfd)
                            {
                                itemGr.CLKHGR = xtclkhkhxfd.ToString();
                            }
                            if (zlkhgr < xtzlkhkhxfd)
                            {
                                itemGr.ZLKHGR = xtzlkhkhxfd.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (baseCLKH == null || baseZLKH == null)
                        {
                            MessageBox.Show("指标数据库中没有产量考核或者质量考核的数据，请先导入在【重新计算】");
                            itemGr.ZLKHGR = "0";
                            itemGr.CLKHGR = "0";
                        }
                        else
                        {
                            decimal.TryParse(baseCLKH.KHJEXFD, out decimal clkhkhxfd);//产量考核
                            decimal.TryParse(baseZLKH.KHJEXFD, out decimal zlkhkhxfd);//质量考核
                            decimal.TryParse(itemGr.ZLKHGR, out decimal zlkhgr);
                            decimal.TryParse(itemGr.CLKHGR, out decimal clkhgr);
                            if (clkhgr < clkhkhxfd)
                            {
                                itemGr.CLKHGR = clkhkhxfd.ToString();
                            }
                            if (zlkhgr < zlkhkhxfd)
                            {
                                itemGr.ZLKHGR = zlkhkhxfd.ToString();
                            }
                        }
                    }

                    itemGr.JJGZGR = (jj.FirstOrDefault().JE / yy).ToString();

                    decimal.TryParse(itemGr.ZLKHGR, out decimal zlkhgr1);//质量考核个人
                    decimal.TryParse(itemGr.CLKHGR, out decimal clkhgr1);//产量考核个人
                    decimal.TryParse(itemGr.JJGZGR, out decimal jjgzgr);//计件工资个人

                    itemGr.GRHJ = (zlkhgr1 + clkhgr1 + jjgzgr).ToString();
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
            var list = new BaseDal<DataBase2CX_CX_06JJ>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (list.Count == 0)
            {
                MessageBox.Show("没有成型计件数据，无法计算！");
                Enabled = true;
                return;
            }
            var listJJKHTB = new BaseDal<DataBase2CX_CX_02JJKHTB_Out>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().OrderBy(t => t.BZBM).ThenBy(t => int.TryParse(t.GH, out int gh) ? gh : int.MaxValue).ToList();
            if (listJJKHTB.Count == 0)
            {
                MessageBox.Show("没有成型计件考核提报数据，无法计算！");
                Enabled = true;
                return;
            }

            var listGr = Recount(list, listJJKHTB);
            if (listGr != null)
            {
                new BaseDal<DataBase2CX_CX_06JJ_GR>().ExecuteSqlCommand("delete from DataBase2CX_CX_06JJ_GR where TheYear=" + dtp.Value.Year + " and TheMonth=" + dtp.Value.Month);
                new BaseDal<DataBase2CX_CX_06JJ_GR>().Add(listGr);
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
                List<DataBase2CX_CX_06JJ_GR> list = dgv.DataSource as List<DataBase2CX_CX_06JJ_GR>;
                if (new ExcelHelper<DataBase2CX_CX_06JJ_GR>().WriteExcle(Application.StartupPath + "\\Excel\\模板二厂——成型——成型计件个人.xlsx", saveFileDlg.FileName, list, 1, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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