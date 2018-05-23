using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class Frm3SC_10_ZYJJADL : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工号$类别编码$类别名称$员工编码$姓名$工厂$工段编码$工段名称$开窑量$一级品$装走_降级$装走_破损$装粘_降级$装粘_破损$装磕_降级$装磕_破损$装脏_降级$装脏_破损$刮边_降级$刮边_破损$合计_降级$合计_破损".Split('$');
        private string[] headerTB = "创建日期$创建人$年$月$序号$工段$月报开窑量$实际标缺陷$实际缺陷率$线下目标缺陷率$金额$线上目标缺陷率$金额".Split('$');

        public Frm3SC_10_ZYJJADL()
        {
            InitializeComponent();
        }

        private void UiControl()
        {
            if (Program.User.Code == "Admin")
            {
                return;
            }
            System.Reflection.FieldInfo[] fieldInfo = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ContextMenuStrip> contextMenuStrip = new List<ContextMenuStrip>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfo[i].FieldType == typeof(ButtonX))
                {
                    ((ButtonX)fieldInfo[i].GetValue(this)).Enabled = false;
                    //Controls.Find(fieldInfo[i].Name, true)[0].Enabled = false;
                    continue;
                }

                if (fieldInfo[i].FieldType == typeof(ContextMenuStrip))
                {
                    var tmp = (ContextMenuStrip)fieldInfo[i].GetValue(this);
                    contextMenuStrip.Add(tmp);
                    foreach (var item in tmp.Items)
                    {
                        if (item is ToolStripMenuItem t)
                        {
                            t.Enabled = false;
                        }
                    }
                }
            }

            var dic = MenuBarHelper.FrmControlEnable(Program.User.Code);
            foreach (var item in dic.Keys)
            {
                var cons = Controls.Find(item, true);
                if (cons.Length > 0)
                {
                    cons[0].Enabled = dic[item];
                    continue;
                }
                foreach (var contextMs in contextMenuStrip)
                {
                    foreach (var item2 in contextMs.Items)
                    {
                        if (item2 is ToolStripMenuItem x && x.Name == item)
                        {
                            x.Enabled = dic[item];
                        }
                    }
                }
            }
        }

        private void Frm3SC_10_ZYJJADL_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string code)
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
            var datas = new BaseDal<DataBase3SC_10_ZYJJADL>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.GH.Contains(code)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3SC_10_ZYJJADL(datas));
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
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void RefDgvYZ(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ToList();
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < headerTB.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerTB[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, txtUserCode.Text, txtUserName.Text, txtCode.Text);
        }

        private void btnSearchYZ_Click(object sender, EventArgs e)
        {
            RefDgvYZ(dtp.Value);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——烧成——装窑检验按大类.xlsx");
        }

        private List<DataBase3SC_10_ZYJJADL_XXTBKH> Recount(List<DataBase3SC_10_ZYJJADL> list)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算！");
                return null;
            }
            try
            {
                var xxtbkh = new List<DataBase3SC_10_ZYJJADL_XXTBKH>();
                for (int i = 1; i < 5; i++)
                {
                    var child = new DataBase3SC_10_ZYJJADL_XXTBKH
                    {
                        Id = Guid.NewGuid(),
                        CreateTime = Program.NowTime,
                        CreateUser = Program.User.ToString(),
                        No = i.ToString(),
                        TheYear = list.FirstOrDefault().TheYear,
                        TheMonth = list.FirstOrDefault().TheMonth
                    };
                    switch (i)
                    {
                        case 1:
                            child.GD = "一段";
                            break;

                        case 2:
                            child.GD = "二段";
                            break;

                        case 3:
                            child.GD = "三段";
                            break;

                        case 4:
                            child.GD = "四段";
                            break;

                        default:
                            break;
                    }
                    var x = list.Where(t => t.GDMC.Contains(child.GD) && !t.LBMC.Contains("盖")).ToList().Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M);
                    child.YBKYL = x.ToString();
                    var tmp = new BaseDal<DataBase3SC_08_TBSJQXS>().Get(t => t.GD.Contains(child.GD) && t.TheYear == child.TheYear && t.TheMonth == child.TheMonth);
                    if (tmp == null)
                    {
                        MessageBox.Show("贴标实际缺陷数没有数据，无法计算，有数据后请重新【计算】");
                        return null;
                    }
                    child.SJBQX = tmp.QX.ToString();
                    decimal.TryParse(child.SJBQX, out decimal y);
                    var z = y / x;
                    child.SJQXL = z.ToString();
                    var dataDay_TBG_X = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "贴标工线下");
                    decimal.TryParse(dataDay_TBG_X.GRFKDJ1, out decimal d5);
                    child.XXMBQXL = dataDay_TBG_X.GRKHZB1;
                    var dataMonth_TBG_X = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "贴标工线下");
                    decimal.TryParse(dataMonth_TBG_X.MoneyKH, out decimal d500);
                    decimal.TryParse(child.XXMBQXL, out decimal aa);
                    if (y <= 0)
                    {
                        child.JE1 = 0.ToString();
                    }
                    else
                    {
                        if (z <= aa)
                        {
                            child.JE1 = d500.ToString();
                        }
                        else
                        {
                            if ((d500 - (y - aa * x) * d5) <= 0)
                            {
                                child.JE1 = 0.ToString();
                            }
                            else
                            {
                                child.JE1 = (d500 - (y - aa * x) * d5).ToString();
                            }
                        }
                    }
                    var dataDay_TBG_S = new BaseDal<DataBaseDay>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "烧成车间" && t.PostName == "贴标工线上");
                    decimal.TryParse(dataDay_TBG_S.GRKHZB1, out decimal ac);
                    decimal.TryParse(dataDay_TBG_S.GRFKDJ1, out decimal d5_S);
                    decimal.TryParse(dataDay_TBG_S.KHJEXFD, out decimal d300);
                    child.XSMBQXL = ac.ToString();
                    if (y <= 0)
                    {
                        child.JE2 = 0.ToString();
                    }
                    else
                    {
                        if (z <= ac)
                        {
                            child.JE2 = 0.ToString();
                        }
                        else
                        {
                            if ((ac * x - y) * d5_S <= -d300)
                            {
                                child.JE2 = (-d300).ToString();
                            }
                            else
                            {
                                child.JE2 = ((ac * x - y) * d5_S).ToString();
                            }
                        }
                    }
                    xxtbkh.Add(child);
                }

                return xxtbkh;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Filter = "Excel文件|*.xlsx",
            };
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<DataBase3SC_10_ZYJJADL> list = new ExcelHelper<DataBase3SC_10_ZYJJADL>().ReadExcel(openFileDlg.FileName, 2, 6, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (string.IsNullOrEmpty(item.LBBM) || string.IsNullOrEmpty(item.UserCode))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                        if (!string.IsNullOrEmpty(item.UserCode) && item.UserCode.StartsWith("M") && !MyDal.IsUserCodeAndNameOK(item.UserCode, item.UserName, out string userNameERP))
                        {
                            MessageBox.Show("工号：【" + item.UserCode + "】,姓名：【" + item.UserName + "】,与ERP中人员信息不一致" + Environment.NewLine + "ERP姓名为：【" + userNameERP + "】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        item.GH = string.IsNullOrEmpty(item.GH) ? string.Empty : item.GH;
                        item.UserCode = string.IsNullOrEmpty(item.UserCode) ? string.Empty : item.UserCode;
                        item.UserName = string.IsNullOrEmpty(item.UserName) ? string.Empty : item.UserName;

                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (new BaseDal<DataBase3SC_10_ZYJJADL>().Add(list) > 0)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                        MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
                List<DataBase3SC_10_ZYJJADL> list = dgv.DataSource as List<DataBase3SC_10_ZYJJADL>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3SC_10_ZYJJADL>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——装窑检验按大类.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase3SC_10_ZYJJADL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3SC_10_ZYJJADL DataBase3SC_10_ZYJJADL = new DataBase3SC_10_ZYJJADL() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3SC_10_ZYJJADL> frm = new FrmModify<DataBase3SC_10_ZYJJADL>(DataBase3SC_10_ZYJJADL, header, OptionType.Add, Text, 5, 10);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3SC_10_ZYJJADL> list = new BaseDal<DataBase3SC_10_ZYJJADL>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().ExecuteSqlCommand("delete from DataBase3SC_10_ZYJJADL_XXTBKH where theyear = " + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());

            var list_xxtbkh = Recount(list);
            if (list_xxtbkh == null)
            {
                MessageBox.Show("计算失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().Add(list_xxtbkh) > 0)
            {
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("计算失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3SC_10_ZYJJADL DataBase3SC_10_ZYJJADL)
                {
                    if (DataBase3SC_10_ZYJJADL.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3SC_10_ZYJJADL> frm = new FrmModify<DataBase3SC_10_ZYJJADL>(DataBase3SC_10_ZYJJADL, header, OptionType.Modify, Text, 5, 10);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnRecount.PerformClick();
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3SC_10_ZYJJADL = dgv.SelectedRows[0].DataBoundItem as DataBase3SC_10_ZYJJADL;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3SC_10_ZYJJADL != null)
                    {
                        if (DataBase3SC_10_ZYJJADL.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3SC_10_ZYJJADL>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3SC_10_ZYJJADL>().Delete(item);
                                }
                                var list_Child = new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
                                foreach (var item in list_Child)
                                {
                                    new BaseDal<DataBase3SC_10_ZYJJADL_XXTBKH>().Delete(item);
                                }
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3SC_10_ZYJJADL> frm = new FrmModify<DataBase3SC_10_ZYJJADL>(DataBase3SC_10_ZYJJADL, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }

        private void btnExportExcelTBG_Click(object sender, EventArgs e)
        {
            btnSearchYZ.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3SC_10_ZYJJADL_XXTBKH> list = dgv.DataSource as List<DataBase3SC_10_ZYJJADL_XXTBKH>;
                if (new ExcelHelper<DataBase3SC_10_ZYJJADL_XXTBKH>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——烧成——贴标考核.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
    }
}