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
    public partial class Frm1MJ_DJCJYB : Office2007Form
    {
        private string[] header;

        public Frm1MJ_DJCJYB()
        {
            InitializeComponent();
        }
        #region 按钮事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm1MJ_DJCJYB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            txtCPMC.Text = string.Empty;
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtCPMC.Text);
        }

        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——模具——大件车间月报.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            List<DataBase1MJ_DJCJYB> list = null;
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                list = new ExcelHelper<DataBase1MJ_DJCJYB>().ReadExcel(openFileDlg.FileName, 5, 6, 1, 0, 0, true);
                if (list == null)
                {
                    MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    return;
                }
                var listBase = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G001" && t.WorkshopName == "模具车间").ToList();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    //if (new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间" && t.TypesName == item.CPMC) == null)
                    if (listBase.Where(t => t.TypesName == item.CPMC).Count() == 0)
                    {
                        sb.Append("【" + item.CPMC + "】,");
                        list.RemoveAt(i);
                        i--;
                        continue;
                    }
                    item.CreateUser = Program.User.ToString();
                    item.CreateTime = Program.NowTime;
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                    item.No = (i + 1).ToString();
                }
                if (sb.Length > 0)
                {
                    MessageBox.Show("基础数据库中没有信息，导入时自动过滤的有：" + Environment.NewLine + sb.ToString().TrimEnd(','), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (new BaseDal<DataBase1MJ_DJCJYB>().Add(list) > 0)
                {
                    Enabled = true;
                    txtCPMC.Text = "";
                    btnSearch.PerformClick();
                    MessageBox.Show("导入成功！" + Environment.NewLine + sb.ToString().TrimEnd(','), "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Enabled = true;
            }
        }

        #endregion

        #region 调用方法

        private void RefDgv(DateTime selectTime, string cpmc)
        {
            var datas = new BaseDal<DataBase1MJ_DJCJYB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.CPMC.Contains(cpmc)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase1MJ_DJCJYB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            header = "创建日期$创建人$年$月$序号$产品名称$单位$上期结存$当日生产$生产累计$调拨入库$当日调拨$拨出累计$调拨出库$当日破损$破损累计$期末结存$备注".Split('$');
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
        }




        #endregion

        private void btnRecount_Click(object sender, EventArgs e)
        {

        }
    }
}
