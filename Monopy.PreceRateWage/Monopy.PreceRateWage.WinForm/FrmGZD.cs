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
    public partial class FrmGZD : Office2007Form
    {
        private string[] header;
        private DateTime selectTime;

        public FrmGZD()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void InitCmbFactory(DateTime dateTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                // List<string> list = conn.Query<string>("select distinct Factory from gzds where theYear=" + dateTime.Year.ToString() + " and theMonth=" + dateTime.Month.ToString()).ToList();
                List<string> list = new List<string>();
                list.Insert(0, "一厂");
                list.Insert(1, "二厂");
                list.Insert(2, "三厂");
                cmbFactory.DataSource = list;
            }
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct dept from gzds where theYear=" + selectTime.Year.ToString() + " and theMonth=" + selectTime.Month.ToString() + " and factory like '%" + factory + "%'").ToList();
                list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private void FrmGZD_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            selectTime = dtp.Value;
            txtUserCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime, string userCode, string userName, string factory, string detp, string zw, bool isEnd)
        {
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.Frozen = false;
                item.Visible = true;
            }
            if (factory == "三厂")
            {
                //factory = factory == "全部" ? string.Empty : factory;
                detp = detp == "全部" ? string.Empty : detp;
                zw = zw == "全部" ? string.Empty : zw;
                var datas = new BaseDal<GZD>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.Factory.Contains(factory) && t.Dept.Contains(detp) && t.Position.Contains(zw) && t.IsEnd == isEnd).OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => t.Position).ThenBy(t => t.Code).ThenBy(t => t.UserCode).ToList();
                datas.Insert(0, MyDal.GetTotalGZD(datas, selectTime));
                dgv.DataSource = datas;
                for (int i = 0; i < 10; i++)
                {
                    dgv.Columns[i].Visible = false;
                }
                dgv.Columns[17].Frozen = true;
                dgv.Rows[0].Frozen = true;
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
                dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
                header = "厂区$部门名称$工段$工号$人员类别$职位$人员编码$姓名$弹性假天数$加班出勤$实出勤$应出勤$基本工资$技能工资$加班工资$工龄工资$基本工资额$工龄额$技能工资额$加班工资额1$加班工资额2$全勤奖$日工$弹性假补助$补发工资$师傅计件$计件工资1$计件工资2$计件工资3$产量考核工资$质量考核工资$个人考核工资$班组考核工资$品管验货$替班工资$班长费$入职补助$变产补助$返坯奖罚$开发试烧补助$工厂试烧补助$退模补助$计件补贴$交通补助$租房补助$幼儿园补助$电话补贴$体检费报销$介绍费$车票报销$五星补助$奖励$打卡/惩戒$成型辅料奖罚$破损补助$破损罚款$成检罚款$品管罚款$雇主责任险扣款$离职扣款$公司奖励$公司惩戒$应发合计$工会互助金$代扣公积金$代扣保险费$个税$税后扣款$实发合计".Split('$');
                for (int i = 0; i < header.Length; i++)
                {
                    dgv.Columns[i + 10].HeaderText = header[i];
                }
                for (int i = 1; i < datas.Count; i++)
                {
                    if (!datas[i].IsCheckCQDayOk)
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                dgv.ClearSelection();
            }
            else if(factory == "一厂")
            {
                detp = detp == "全部" ? string.Empty : detp;
                zw = zw == "全部" ? string.Empty : zw;
                var datas = new BaseDal<DataBase1GZD>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.Factory.Contains(factory) && t.Dept.Contains(detp) && t.GWMC.Contains(zw)).OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => t.GWMC).ThenBy(t => t.UserCode).ThenBy(t => t.UserName).ToList();
                datas.Insert(0, MyDal.GetTotalDataBase1GZD(datas));
                dgv.DataSource = datas;
                for (int i = 0; i < 6; i++)
                {
                    dgv.Columns[i].Visible = false;
                }
                //dgv.Columns[17].Frozen = true;
                dgv.Rows[0].Frozen = true;
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
                dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
                header = "工厂$车间$工段$岗位名称$人员编码$姓名$入职补助$变产补助$补压坯$计件工资1$计件工资2$计件工资3$计件工资4$考核工资1$考核工资2$考核工资3$考核工资4$辅助验货$日工$试烧补助$破损补助$车间奖金$车间罚款$辅料奖罚$破损罚款$其他加项$其他减项".Split('$');
                for (int i = 0; i < header.Length; i++)
                {
                    dgv.Columns[i + 6].HeaderText = header[i];
                }
                //for (int i = 1; i < datas.Count; i++)
                //{
                //    if (!datas[i].IsCheckCQDayOk)
                //    {
                //        dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                //    }
                //}
                dgv.ClearSelection();
            }
            else if (factory == "二厂")
            {
                detp = detp == "全部" ? string.Empty : detp;
                zw = zw == "全部" ? string.Empty : zw;
                var datas = new BaseDal<DataBase2GZD>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.UserCode.Contains(userCode) && t.UserName.Contains(userName) && t.Factory.Contains(factory) && t.Dept.Contains(detp) && t.GWMC.Contains(zw)).OrderBy(t => t.Factory).ThenBy(t => t.Dept).ThenBy(t => t.GWMC).ThenBy(t => t.UserCode).ThenBy(t => t.UserName).ToList();
                datas.Insert(0, MyDal.GetTotalDataBase2GZD(datas));
                dgv.DataSource = datas;
                for (int i = 0; i < 6; i++)
                {
                    dgv.Columns[i].Visible = false;
                }
                //dgv.Columns[17].Frozen = true;
                dgv.Rows[0].Frozen = true;
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
                dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
                header = "工厂$车间$工段$岗位名称$人员编码$姓名$入职补助$变产补助$补压坯$计件工资1$计件工资2$计件工资3$计件工资4$考核工资1$考核工资2$考核工资3$考核工资4$辅助验货$日工$试烧补助$破损补助$车间奖金$车间罚款$辅料奖罚$破损罚款$其他加项$其他减项".Split('$');
                for (int i = 0; i < header.Length; i++)
                {
                    dgv.Columns[i + 6].HeaderText = header[i];
                }
                //for (int i = 1; i < datas.Count; i++)
                //{
                //    if (!datas[i].IsCheckCQDayOk)
                //    {
                //        dgv.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                //    }
                //}
                dgv.ClearSelection();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            RefDgv(selectTime, txtUserCode.Text, txtUserName.Text, cmbFactory.Text, cmbDept.Text, cmbZW.Text, checkBox1.Checked);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (cmbFactory.Text == "三厂")
                {
                    Enabled = false;
                    List<GZD> list = dgv.DataSource as List<GZD>;
                    var hj = list[0];
                    list.RemoveAt(0);
                    list.Add(hj);
                    if (new ExcelHelper<GZD>().WriteExcle(Application.StartupPath + "\\Excel\\工资单导出格式.xlsx", saveFileDlg.FileName, list, 3, 10, 0, 0, 1, 0, dtp.Value.ToString("yyyy-MM")))
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
                else if (cmbFactory.Text == "一厂")
                {
                    Enabled = false;
                    List<DataBase1GZD> list = dgv.DataSource as List<DataBase1GZD>;
                    var hj = list[0];
                    list.RemoveAt(0);
                    list.Add(hj);
                    if (new ExcelHelper<DataBase1GZD>().WriteExcle(Application.StartupPath + "\\Excel\\一厂二厂工资单导出格式.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                else if (cmbFactory.Text == "二厂")
                {
                    Enabled = false;
                    List<DataBase2GZD> list = dgv.DataSource as List<DataBase2GZD>;
                    var hj = list[0];
                    list.RemoveAt(0);
                    list.Add(hj);
                    if (new ExcelHelper<DataBase2GZD>().WriteExcle(Application.StartupPath + "\\Excel\\一厂二厂工资单导出格式.xlsx", saveFileDlg.FileName, list, 2, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "最终结果";
            }
            else
            {
                checkBox1.Text = "计算过程";
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            selectTime = dtp.Value;
            InitCmbFactory(selectTime);
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbZW.DataSource = null;
            string factory = cmbFactory.Text == "全部" ? string.Empty : cmbFactory.Text;
            string dept = cmbDept.Text == "全部" ? string.Empty : cmbDept.Text;
            List<string> list = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                list = conn.Query<string>("select distinct Position from gzds where Position is not null and theYear=" + selectTime.Year.ToString() + " and theMonth=" + selectTime.Month.ToString() + " and factory like '%" + factory + "%' and dept like '%" + dept + "%' order by Position").ToList();
            }
            list.Insert(0, "全部");
            cmbZW.DataSource = list;
            cmbZW.SelectedIndex = 0;
        }
    }
}