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
    public partial class Frm3CX_CX_01JJKHTB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段$搭伙夫妻$人员编号$姓名$注修工号$品种名称$双锅次数$模型数$注浆次数$模型数2$注浆次数2$备注".Split('$');
        private string[] header2 = "班组编码$班组名称$员工".Split('$');

        public Frm3CX_CX_01JJKHTB()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void Frm3CX_CX_01JJKHTB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string gd, string userCode, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_01JJKHTB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gd == "全部" ? true : t.GD.Equals(gd)) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.GH.Contains(gh)).ToList().OrderBy(t => t.GD).ThenBy(t => int.TryParse(t.No, out int ix) ? ix : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_01JJKHTB(datas));
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

        private void RefDgv2(DateTime selectTime, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase3CX_CX_01JJKHTB_Out>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZMC.Contains(userName) && t.GH.Contains(gh)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ThenBy(t => int.TryParse(t.GH, out int iGh) ? iGh : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3CX_CX_01JJKHTB_Out(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header2.Length; i++)
            {
                dgv.Columns[i + 6].HeaderText = header2[i];
            }
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbGD.Text, txtUserCode.Text, txtUserName.Text, txtGH.Text);
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            RefDgv2(dtp.Value, txtUserName.Text, txtGH.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——成型——计件考核提报.xlsx");
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
            btnSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3CX_CX_01JJKHTB> list = dgv.DataSource as List<DataBase3CX_CX_01JJKHTB>;
                if (new ExcelHelper<DataBase3CX_CX_01JJKHTB>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型——计件考核提报.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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

        private void btnExportExcel2_Click(object sender, EventArgs e)
        {
            btnSearch2.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3CX_CX_01JJKHTB_Out> list = dgv.DataSource as List<DataBase3CX_CX_01JJKHTB_Out>;
                var hj = list[0];
                list.RemoveAt(0);
                if (new ExcelHelper<DataBase3CX_CX_01JJKHTB_Out>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型——计件考核提报Out.xlsx", saveFileDlg.FileName, list, 1, 6))
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
                list.Insert(0, hj);
                Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DataBase3CX_CX_01JJKHTB DataBase3CX_CX_01JJKHTB = new DataBase3CX_CX_01JJKHTB() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month };
            FrmModify<DataBase3CX_CX_01JJKHTB> frm = new FrmModify<DataBase3CX_CX_01JJKHTB>(DataBase3CX_CX_01JJKHTB, header, OptionType.Add, Text, 5, 0);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase3CX_CX_01JJKHTB> list = new BaseDal<DataBase3CX_CX_01JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            Recount(list);
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frm3CX_CX_01JJKHTB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm3CX_CX_01JJKHTB_DragDrop(object sender, DragEventArgs e)
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

        private void Import(string fileName)
        {
            Enabled = false;
            List<DataBase3CX_CX_01JJKHTB> list = new ExcelHelper<DataBase3CX_CX_01JJKHTB>().ReadExcel(fileName, 2, 6, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == "注修工");

            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].GH) || string.IsNullOrEmpty(list[i].UserCode) || string.IsNullOrEmpty(list[i].UserName))
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

                var pzmc = list[i].PZMC;
                if (pzmc == "MS-803箱")
                {
                }
                if (!string.IsNullOrEmpty(pzmc))
                {
                    var itemDay = listDay.Where(t => t.TypesName == pzmc).FirstOrDefault();
                    if (itemDay == null)
                    {
                        MessageBox.Show("品种名称为：【" + list[i].PZMC + "】，基础数据中不存在，请检查或联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        return;
                    }
                }

                list[i].CreateUser = Program.User.ToString();
                list[i].CreateTime = Program.NowTime;
                list[i].TheYear = dtp.Value.Year;
                list[i].TheMonth = dtp.Value.Month;
                var gh = list[i].GH;
                if (gh.Length < 3)
                {
                    gh = gh.PadLeft(3, '0');
                    list[i].GH = gh;
                }
            }
            if (Recount(list) && new BaseDal<DataBase3CX_CX_01JJKHTB>().Add(list) > 0)
            {
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("导入成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，基础数据是否完整！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Enabled = true;
            dtp_ValueChanged(null, null);
        }

        private bool Recount(List<DataBase3CX_CX_01JJKHTB> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                var item = list.FirstOrDefault();
                var xx = new BaseDal<DataBase3CX_CX_01JJKHTB_Out>().ExecuteSqlCommand("delete from DataBase3CX_CX_01JJKHTB_Out where theYear=" + item.TheYear + " and theMonth=" + item.TheMonth);
                //成型月报
                //2018-02-01修改，月报没有的，不生成夫妻
                var list2 = new List<DataBase3CX_CX_01JJKHTB>();
                var listCXYB = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && !string.IsNullOrEmpty(t.UserName)).GroupBy(t => new { t.GH, t.UserName }).Select(t => new { t.Key.GH, t.Key.UserName }).ToList();
                foreach (var itemx in list)
                {
                    if (listCXYB.Where(t => t.GH == itemx.GH).FirstOrDefault() != null)
                    {
                        if (list2.Where(t => t.GH == itemx.GH).FirstOrDefault() == null)
                        {
                            list2.Add(itemx);
                        }
                    }
                }

                var listResult = new List<DataBase3CX_CX_01JJKHTB_Out>();
                foreach (var itemCXYB in listCXYB)
                {
                    if (list2.Where(t => t.GH == itemCXYB.GH).FirstOrDefault() == null)
                    {
                        listResult.Add(new DataBase3CX_CX_01JJKHTB_Out { GH = itemCXYB.GH, BZMC = itemCXYB.UserName });
                    }
                }

                var listTmp = list2.Where(t => !t.DHFQ.Contains("夫妻") && !t.DHFQ.Contains("搭伙"));
                foreach (var itemTmp in listTmp)
                {
                    listResult.Add(new DataBase3CX_CX_01JJKHTB_Out { GH = itemTmp.GH, BZMC = itemTmp.DHFQ });
                }

                listResult = listResult.OrderBy(t => int.TryParse(t.GH, out int iGh) ? iGh : 0).ToList();
                var j = 1;
                foreach (var itemResult in listResult)
                {
                    var jj = "3" + j.ToString().PadLeft(5, '0');
                    itemResult.BZBM = jj;
                    itemResult.CreateTime = Program.NowTime;
                    itemResult.CreateUser = Program.User.ToString();
                    itemResult.No = j.ToString();
                    itemResult.Id = Guid.NewGuid();
                    itemResult.TheYear = item.TheYear;
                    itemResult.TheMonth = item.TheMonth;
                    j++;
                }

                listTmp = list2.Where(t => t.DHFQ.Contains("夫妻") || t.DHFQ.Contains("搭伙")).GroupBy(t => new { t.DHFQ, t.GH }).Select(t => new DataBase3CX_CX_01JJKHTB { GH = t.Key.GH, DHFQ = t.Key.DHFQ });

                var xxtmp = listTmp.GroupBy(t => t.DHFQ).Select(t => new DataBase3CX_CX_01JJKHTB { DHFQ = t.Key, TheYear = t.Count(x => x.DHFQ == t.Key) }).OrderBy(t => t.TheYear).ThenBy(t => t.DHFQ);

                foreach (var itemTmp in xxtmp)
                {
                    var jj = "3" + j.ToString().PadLeft(5, '0');
                    foreach (var itemTmp2 in listTmp.Where(t => t.DHFQ == itemTmp.DHFQ).OrderBy(t => int.TryParse(t.GH, out int iGh) ? iGh : int.MaxValue))
                    {
                        listResult.Add(new DataBase3CX_CX_01JJKHTB_Out { GH = itemTmp2.GH, BZMC = itemTmp2.DHFQ, BZBM = jj, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), No = j.ToString(), Id = Guid.NewGuid(), TheYear = item.TheYear, TheMonth = item.TheMonth });
                    }
                    j++;
                }
                if (new BaseDal<DataBase3CX_CX_01JJKHTB_Out>().Add(listResult) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算错误，错误为：" + ex.Message);
                return false;
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase3CX_CX_01JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            list.Insert(0, "全部");
            cmbGD.DataSource = list;
            cmbGD.DisplayMember = "GD";
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3CX_CX_01JJKHTB = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_01JJKHTB;
                if (DataBase3CX_CX_01JJKHTB == null)
                {
                    MessageBox.Show("只能修改【计件考核提报】，不能修改【夫妻】，【夫妻】重新计算即可！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (DataBase3CX_CX_01JJKHTB.No == "合计")
                {
                    MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FrmModify<DataBase3CX_CX_01JJKHTB> frm = new FrmModify<DataBase3CX_CX_01JJKHTB>(DataBase3CX_CX_01JJKHTB, header, OptionType.Modify, Text, 5, 0);
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    btnRecount.PerformClick();
                    btnSearch.PerformClick();
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase3CX_CX_01JJKHTB = dgv.SelectedRows[0].DataBoundItem as DataBase3CX_CX_01JJKHTB;
                if (DataBase3CX_CX_01JJKHTB == null)
                {
                    MessageBox.Show("只能编辑【计件考核提报】，不能编辑【夫妻】，【夫妻】重新计算即可！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3CX_CX_01JJKHTB.No == "合计")
                    {
                        if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            var list = dgv.DataSource as List<DataBase3CX_CX_01JJKHTB>;
                            dgv.DataSource = null;
                            list.RemoveAt(0);
                            foreach (var item in list)
                            {
                                new BaseDal<DataBase3CX_CX_01JJKHTB>().Delete(item);
                            }
                            btnSearch.PerformClick();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    FrmModify<DataBase3CX_CX_01JJKHTB> frm = new FrmModify<DataBase3CX_CX_01JJKHTB>(DataBase3CX_CX_01JJKHTB, header, OptionType.Delete, Text, 5);
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        btnSearch.PerformClick();
                    }
                }
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

        private void BtnYZ_Click(object sender, EventArgs e)
        {
            var listYB = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var list = new BaseDal<DataBase3CX_CX_01JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            bool result = true;
            foreach (var item in list)
            {
                var itemYB = listYB.Where(t => t.UserCode == item.UserCode).FirstOrDefault();
                if (itemYB == null)
                {
                    MessageBox.Show($"人员编码：{item.UserCode}，姓名：{item.UserName}，工段：{item.GD}，在成型月报中不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
                else
                {
                    if (itemYB.GH != item.GH)
                    {
                        MessageBox.Show($"人员编码：【{item.UserCode}】，姓名：【{item.UserName}】，工号：【{item.GH}】，工段：{item.GD}，；在成型月报中为：人员编码：【{itemYB.UserCode}】，姓名：【{itemYB.UserName}】，工号：【{itemYB.GH}】", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = false;
                    }
                }
            }
            if (result)
            {
                MessageBox.Show("验证通过！", "验证完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("至少有一条记录不满足，验证不成功！", "验证完成", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}