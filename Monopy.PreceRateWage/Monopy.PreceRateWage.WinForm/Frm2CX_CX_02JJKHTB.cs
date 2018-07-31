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
    public partial class Frm2CX_CX_02JJKHTB : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$颜色$序号$班组编码$人员编码班组$工段$搭伙夫妻$人员编号$姓名$注修工号$品种名称$模型数$注浆次数$模型数2$注浆次数2$备注".Split('$');
        private string[] header2 = "班组编码$班组名称$员工".Split('$');
        private string[] header3 = "班组编码$班组名称$员工$员工编码$姓名$人员编码班组".Split('$');

        public Frm2CX_CX_02JJKHTB()
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
        private void Frm2CX_CX_02JJKHTB_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            dtp_ValueChanged(null, null);
            btnSearch.PerformClick();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value, cmbGD.Text, txtDHFQ.Text, txtUserCode.Text, txtUserName.Text, txtGH.Text, txtPZMC.Text);
        }

        /// <summary>
        /// 查询班组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            RefDgv2(dtp.Value, txtUserName.Text, txtGH.Text);
        }

        /// <summary>
        /// 查询班组明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            RefDgv3(dtp.Value, txtUserName.Text, txtGH.Text);

        }

        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板一厂——成型——计件考核提报.xlsx");
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
                List<DataBase2CX_CX_02JJKHTB> list = dgv.DataSource as List<DataBase2CX_CX_02JJKHTB>;
                if (new ExcelHelper<DataBase2CX_CX_02JJKHTB>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出一厂——成型——计件考核提报.xlsx", saveFileDlg.FileName, list, 1, 7, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        /// 导出七通
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                List<DataBase2CX_CX_02JJKHTB_Out> list = dgv.DataSource as List<DataBase2CX_CX_02JJKHTB_Out>;
                foreach (var item in list)
                {
                    item.UserCode = string.Empty;
                    item.UserName = string.Empty;
                    item.RYBMBZ = string.Empty;
                }
                //var hj = list[0];
                //list.RemoveAt(0);
                if (new ExcelHelper<DataBase2CX_CX_02JJKHTB_Out>().WriteExcle(Application.StartupPath + "\\Excel\\模板三厂——成型——计件考核提报Out.xlsx", saveFileDlg.FileName, list, 1, 6))
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
                //list.Insert(0, hj);
                Enabled = true;
            }
        }

        /// <summary>
        /// 导出班组明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            buttonX1.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase2CX_CX_02JJKHTB_Out> list = dgv.DataSource as List<DataBase2CX_CX_02JJKHTB_Out>;
                //var hj = list[0];
                //list.RemoveAt(0);
                if (new ExcelHelper<DataBase2CX_CX_02JJKHTB_Out>().WriteExcle(Application.StartupPath + "\\Excel\\模板一厂——成型——计件考核提报OutDetail.xlsx", saveFileDlg.FileName, list, 1, 6))
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
                //list.Insert(0, hj);
                Enabled = true;
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            var list = new BaseDal<DataBase2CX_CX_02JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).GroupBy(t => t.GD).Select(t => t.Key).OrderBy(t => t).ToList();
            list.Insert(0, "全部");
            cmbGD.DataSource = list;
            cmbGD.DisplayMember = "GD";
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                var DataBase2CX_CX_02JJKHTB = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_CX_02JJKHTB;
                if (DataBase2CX_CX_02JJKHTB == null)
                {
                    MessageBox.Show("只能修改【计件考核提报】，不能修改【班组】，【班组】重新计算即可！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (DataBase2CX_CX_02JJKHTB.No == "合计")
                {
                    MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FrmModify<DataBase2CX_CX_02JJKHTB> frm = new FrmModify<DataBase2CX_CX_02JJKHTB>(DataBase2CX_CX_02JJKHTB, header, OptionType.Modify, Text, 6, 0);
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    btnRecount.PerformClick();
                    btnSearch.PerformClick();
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
                var DataBase2CX_CX_02JJKHTB = dgv.SelectedRows[0].DataBoundItem as DataBase2CX_CX_02JJKHTB;
                if (DataBase2CX_CX_02JJKHTB == null)
                {
                    MessageBox.Show("只能编辑【计件考核提报】，不能编辑【班组】，【班组】重新计算即可！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase2CX_CX_02JJKHTB.No == "合计")
                    {
                        if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            var list = dgv.DataSource as List<DataBase2CX_CX_02JJKHTB>;
                            dgv.DataSource = null;
                            list.RemoveAt(0);
                            foreach (var item in list)
                            {
                                new BaseDal<DataBase2CX_CX_02JJKHTB>().Delete(item);
                            }
                            btnSearch.PerformClick();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    FrmModify<DataBase2CX_CX_02JJKHTB> frm = new FrmModify<DataBase2CX_CX_02JJKHTB>(DataBase2CX_CX_02JJKHTB, header, OptionType.Delete, Text, 6);
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

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnYZ_Click(object sender, EventArgs e)
        {
            var listYB = new BaseDal<DataBase2CX_CX_01CXYB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
            var list = new BaseDal<DataBase2CX_CX_02JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month);
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

        /// <summary>
        /// 重新计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecount_Click(object sender, EventArgs e)
        {
            Enabled = false;
            List<DataBase2CX_CX_02JJKHTB> list = new BaseDal<DataBase2CX_CX_02JJKHTB>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).OrderBy(t => t.No).ToList();
            Recount(list);
            Enabled = true;
            btnSearch.PerformClick();
            MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frm2CX_CX_02JJKHTB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Frm2CX_CX_02JJKHTB_DragDrop(object sender, DragEventArgs e)
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

        #endregion

        #region 调用方法

        private void RefDgv(DateTime selectTime, string gd, string dhfq, string userCode, string userName, string gh, string pzmc)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2CX_CX_02JJKHTB>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && (gd == "全部" ? true : t.GD.Equals(gd)) && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.GH.Contains(gh) && t.DHFQ.Contains(dhfq) && t.PZMC.Contains(pzmc)).ToList().OrderBy(t => t.GD).ThenBy(t => int.TryParse(t.No, out int ix) ? ix : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase2CX_CX_02JJKHTB(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 7; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            for (int i = 0; i < datas.Count; i++)
            {
                if (!string.IsNullOrEmpty(datas[i].Color))
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
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
            var datas = new BaseDal<DataBase2CX_CX_02JJKHTB_Out>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZMC.Contains(userName) && t.GH.Contains(gh)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ThenBy(t => int.TryParse(t.GH, out int iGh) ? iGh : int.MaxValue).ToList();
            //datas.Insert(0, MyDal.GetTotalDataBase2CX_CX_02JJKHTB_Out(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 12; i++)
            {
                if (i < 6 || i > 9)
                {
                    dgv.Columns[i].Visible = false;
                }
            }
            //dgv.Rows[0].Frozen = true;
            //dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            //dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header2.Length; i++)
            {
                dgv.Columns[i + 6].HeaderText = header2[i];
            }
            dgv.ClearSelection();
        }

        private void RefDgv3(DateTime selectTime, string userName, string gh)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            var datas = new BaseDal<DataBase2CX_CX_02JJKHTB_Out>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.BZMC.Contains(userName) && t.GH.Contains(gh)).ToList().OrderBy(t => int.TryParse(t.No, out int i) ? i : int.MaxValue).ThenBy(t => int.TryParse(t.GH, out int iGh) ? iGh : int.MaxValue).ToList();
            //datas.Insert(0, MyDal.GetTotalDataBase2CX_CX_02JJKHTB_Out(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            //dgv.Rows[0].Frozen = true;
            //dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            //dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < header3.Length; i++)
            {
                dgv.Columns[i + 6].HeaderText = header3[i];
            }
            dgv.ClearSelection();
        }

        private void Import(string fileName)
        {
            Enabled = false;
            List<DataBase2CX_CX_02JJKHTB> list = new ExcelHelper<DataBase2CX_CX_02JJKHTB>().ReadExcel(fileName, 1, 9, 0, 0, 0, true);
            if (list == null)
            {
                MessageBox.Show("Excel文件错误（请用Excle2007或以上打开文件，另存，再试），或者文件正在打开（关闭Excel），或者文件没有数据（请检查！）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var listDay = new BaseDal<DataBaseDay>().GetList(t => t.FactoryNo == "G002" && t.WorkshopName == "成型车间" && t.PostName == "注修工");

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

                if (!string.IsNullOrEmpty(pzmc))
                {
                    var itemDay = listDay.Where(t => t.TypesName == pzmc).FirstOrDefault();
                    if (itemDay == null)
                    {
                        MessageBox.Show("品种名称为：【" + list[i].PZMC + "】，基础数据中不存在，请检查或联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Enabled = true;
                        list[i].Color = "Sign";
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
            if (Recount(list) && new BaseDal<DataBase2CX_CX_02JJKHTB>().Add(list) > 0)
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

        private bool Recount(List<DataBase2CX_CX_02JJKHTB> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].DHFQ))
                    {
                        list[i].RYBMBZ = list[i].UserCode;
                    }
                    if (list[i].DHFQ.Contains("夫妻") || list[i].DHFQ.Contains("搭伙"))
                    {
                        if (list[i].DHFQ.Contains(list[i].UserName))
                        {
                            list[i].RYBMBZ = list[i].UserCode;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                list[i].RYBMBZ = list[i + 1].UserCode;
                            }
                            else if (i == list.Count - 1)
                            {
                                list[i].RYBMBZ = list[i - 1].UserCode;
                            }
                            else
                            {
                                if (list[i].DHFQ.Contains(list[i - 1].UserName))
                                {
                                    list[i].RYBMBZ = list[i - 1].UserCode;
                                }
                                else if (list[i].DHFQ.Contains(list[i + 1].UserName))
                                {
                                    list[i].RYBMBZ = list[i + 1].UserCode;
                                }
                                else
                                {
                                    list[i].RYBMBZ = list[i - 1].RYBMBZ;
                                }
                            }
                        }
                    }
                    else
                    {
                        list[i].RYBMBZ = list[i].UserCode;
                    }

                }
                var item = list.FirstOrDefault();
                var xx = new BaseDal<DataBase2CX_CX_02JJKHTB_Out>().ExecuteSqlCommand("delete from DataBase2CX_CX_02JJKHTB_Out where theYear=" + item.TheYear + " and theMonth=" + item.TheMonth);
                //成型月报
                //2018-02-01修改，月报没有的，不生成夫妻
                var list2 = new List<DataBase2CX_CX_02JJKHTB>();
                var listCXYB = new BaseDal<DataBase2CX_CX_01CXYB>().GetList(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && !string.IsNullOrEmpty(t.UserName)).GroupBy(t => new { t.GH, t.UserName }).Select(t => new { t.Key.GH, t.Key.UserName }).ToList();
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

                var listResult = new List<DataBase2CX_CX_02JJKHTB_Out>();
                foreach (var itemCXYB in listCXYB)
                {
                    if (list2.Where(t => t.GH == itemCXYB.GH).FirstOrDefault() == null)
                    {
                        listResult.Add(new DataBase2CX_CX_02JJKHTB_Out { GH = itemCXYB.GH, BZMC = itemCXYB.UserName });
                    }
                }

                var listTmp = list2.Where(t => !t.DHFQ.Contains("夫妻") && !t.DHFQ.Contains("搭伙"));
                foreach (var itemTmp in listTmp)
                {
                    listResult.Add(new DataBase2CX_CX_02JJKHTB_Out { GH = itemTmp.GH, BZMC = itemTmp.DHFQ });
                }

                listResult = listResult.OrderBy(t => int.TryParse(t.GH, out int iGh) ? iGh : 0).ToList();
                var j = 1;
                foreach (var itemResult in listResult)
                {
                    var jj = "2" + j.ToString().PadLeft(5, '0');
                    itemResult.BZBM = jj;
                    itemResult.CreateTime = Program.NowTime;
                    itemResult.CreateUser = Program.User.ToString();
                    itemResult.No = j.ToString();
                    itemResult.Id = Guid.NewGuid();
                    itemResult.TheYear = item.TheYear;
                    itemResult.TheMonth = item.TheMonth;
                    j++;
                }

                listTmp = list2.Where(t => t.DHFQ.Contains("夫妻") || t.DHFQ.Contains("搭伙")).GroupBy(t => new { t.DHFQ, t.GH }).Select(t => new DataBase2CX_CX_02JJKHTB { GH = t.Key.GH, DHFQ = t.Key.DHFQ });

                var xxtmp = listTmp.GroupBy(t => t.DHFQ).Select(t => new DataBase2CX_CX_02JJKHTB { DHFQ = t.Key, TheYear = t.Count(x => x.DHFQ == t.Key) }).OrderBy(t => t.TheYear).ThenBy(t => t.DHFQ);

                foreach (var itemTmp in xxtmp)
                {
                    var jj = "2" + j.ToString().PadLeft(5, '0');
                    foreach (var itemTmp2 in listTmp.Where(t => t.DHFQ == itemTmp.DHFQ).OrderBy(t => int.TryParse(t.GH, out int iGh) ? iGh : int.MaxValue))
                    {
                        listResult.Add(new DataBase2CX_CX_02JJKHTB_Out { GH = itemTmp2.GH, BZMC = itemTmp2.DHFQ, BZBM = jj, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), No = j.ToString(), Id = Guid.NewGuid(), TheYear = item.TheYear, TheMonth = item.TheMonth });
                    }
                    j++;
                }

                foreach (var itemResult in listResult)
                {
                    var listResultCXYB = new BaseDal<DataBase2CX_CX_01CXYB>().Get(t => t.TheYear == item.TheYear && t.TheMonth == item.TheMonth && t.GH == itemResult.GH);
                    if (listResultCXYB != null)
                    {
                        itemResult.UserCode = listResultCXYB.UserCode;
                        itemResult.UserName = listResultCXYB.UserName;
                    }
                    var listRes = list.Where(t => t.UserCode == itemResult.UserCode).FirstOrDefault();
                    if (listRes != null)
                    {
                        itemResult.RYBMBZ = listRes.RYBMBZ;
                    }
                    else
                    {
                        itemResult.RYBMBZ = itemResult.UserCode;
                    }
                }

                foreach (var itemListResult in list)
                {
                    var result = listResult.Where(t => t.UserCode == itemListResult.UserCode).FirstOrDefault();
                    if (result != null)
                    {
                        itemListResult.BZBM = result.BZBM;
                    }
                }
                if (new BaseDal<DataBase2CX_CX_02JJKHTB_Out>().Add(listResult) > 0)
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

        #endregion


    }
}