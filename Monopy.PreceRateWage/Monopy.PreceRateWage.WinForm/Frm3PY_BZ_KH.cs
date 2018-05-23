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
    public partial class Frm3PY_BZ_KH : Office2007Form
    {
        private string[] header = "创建日期$创建人$年$月$序号$工段_喷釉段$工段_修检段$类别$喷釉_开窑量$喷釉_一级品$修检线_开窑量$修检线_一级品$修检线_滚釉$修检线_爆釉$修检线_坯渣$班组_实际完成$多功能喷釉工_实际完成$机器人维护工_实际完成$擦绺找枪_实际完成$班组_班组考核指标$班组_班组基数比$班组_班组奖励单价$机器人维护工_班组考核指标$机器人维护工_班组基数比$机器人维护工_班组奖励单价$多功能喷釉工_班组考核指标$多功能喷釉工_班组基数比$多功能喷釉工_班组奖励单价$擦绺找枪班组_班组考核指标$修检班组_班组考核指标$修检班组_班组基数比$修检班组_班组奖励单价$擦水班组_班组考核指标$擦水班组_班组基数比$擦水班组_班组奖励单价$擦绺工_喷釉班组考核占比$上坯工_喷釉班组考核占比".Split('$');
        private string[] headerSum = "年$月$序号$类别$工段$工种$考核金额".Split('$');

        public Frm3PY_BZ_KH()
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

        private void Frm3PY_BZ_KH_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            UiControl();
            btnSearch.PerformClick();
        }

        private void RefDgv(DateTime selectTime)
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
            var datas = new BaseDal<DataBase3PY_BZ_KH>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_BZ_KH(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 6; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dgv.Columns[8].Frozen = true;
            for (int i = 0; i < header.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = header[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = contextMenuStrip1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefDgv(dtp.Value);
        }

        private void RefDgvSum(DateTime selectTime, string gd, string gz)
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
            var datas = new BaseDal<DataBase3PY_BZ_KH_Sum>().GetList(t => t.TheYear == selectTime.Year && t.TheMonth == selectTime.Month && t.GD.Contains(gd) && t.GZ.Contains(gz)).ToList().OrderBy(t => int.TryParse(t.No, out int no) ? no : int.MaxValue).ToList();
            datas.Insert(0, MyDal.GetTotalDataBase3PY_BZ_KH_Sum(datas));
            dgv.DataSource = datas;
            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].Visible = false;
            }
            dgv.Rows[0].Frozen = true;
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
            dgv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            for (int i = 0; i < headerSum.Length; i++)
            {
                dgv.Columns[i + 1].HeaderText = headerSum[i];
            }
            dgv.ClearSelection();
            dgv.ContextMenuStrip = null;
        }

        private void btnSumSearch_Click(object sender, EventArgs e)
        {
            RefDgvSum(dtp.Value, txtGD.Text, txtGZ.Text);
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Excel\\模板三厂——喷釉——喷釉班组考核.xlsx");
        }

        private List<DataBase3PY_BZ_KH_Sum> GetSumBaseList()
        {
            //没有规律，手写了。。。。。
            List<DataBase3PY_BZ_KH_Sum> list = new List<DataBase3PY_BZ_KH_Sum>();
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum1 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 1.ToString(), LB = "一期", GD = "一段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum1);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum2 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 2.ToString(), LB = "一期", GD = "二段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum2);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum3 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 3.ToString(), LB = "一期", GD = "三段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum3);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum4 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 4.ToString(), LB = "一期", GD = "一段", GZ = "修挡板工" };
            list.Add(dataBase3PY_BZ_KH_Sum4);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum5 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 5.ToString(), LB = "一期", GD = "二段", GZ = "修挡板工" };
            list.Add(dataBase3PY_BZ_KH_Sum5);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum6 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 6.ToString(), LB = "一期", GD = "三段", GZ = "修挡板工" };
            list.Add(dataBase3PY_BZ_KH_Sum6);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum7 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 7.ToString(), LB = "一期", GD = "一段", GZ = "干补工" };
            list.Add(dataBase3PY_BZ_KH_Sum7);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum8 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 8.ToString(), LB = "一期", GD = "二段", GZ = "干补工" };
            list.Add(dataBase3PY_BZ_KH_Sum8);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum9 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 9.ToString(), LB = "一期", GD = "三段", GZ = "干补工" };
            list.Add(dataBase3PY_BZ_KH_Sum9);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum10 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 10.ToString(), LB = "一期", GD = "一段", GZ = "吹风工" };
            list.Add(dataBase3PY_BZ_KH_Sum10);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum11 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 11.ToString(), LB = "一期", GD = "二段", GZ = "吹风工" };
            list.Add(dataBase3PY_BZ_KH_Sum11);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum12 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 12.ToString(), LB = "一期", GD = "三段", GZ = "吹风工" };
            list.Add(dataBase3PY_BZ_KH_Sum12);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum13 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 13.ToString(), LB = "一期", GD = "一段", GZ = "灌釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum13);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum14 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 14.ToString(), LB = "一期", GD = "二段", GZ = "灌釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum14);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum15 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 15.ToString(), LB = "一期", GD = "三段", GZ = "灌釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum15);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum16 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 16.ToString(), LB = "一期", GD = "一段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum16);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum17 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 17.ToString(), LB = "一期", GD = "二段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum17);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum18 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 18.ToString(), LB = "一期", GD = "三段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum18);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum19 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 19.ToString(), LB = "一期", GD = "一段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum19);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum20 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 20.ToString(), LB = "一期", GD = "二段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum20);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum21 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 21.ToString(), LB = "一期", GD = "三段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum21);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum22 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 22.ToString(), LB = "一期", GD = "一段", GZ = "多功能喷釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum22);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum23 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 23.ToString(), LB = "一期", GD = "二段", GZ = "多功能喷釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum23);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum24 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 24.ToString(), LB = "一期", GD = "三段", GZ = "多功能喷釉工" };
            list.Add(dataBase3PY_BZ_KH_Sum24);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum25 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 25.ToString(), LB = "一期", GD = "一段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum25);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum26 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 26.ToString(), LB = "一期", GD = "二段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum26);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum27 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 27.ToString(), LB = "一期", GD = "三段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum27);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum28 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 28.ToString(), LB = "一期", GD = "一段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum28);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum29 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 29.ToString(), LB = "一期", GD = "二段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum29);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum30 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 30.ToString(), LB = "一期", GD = "三段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum30);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum31 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 31.ToString(), LB = "一期", GD = "一段", GZ = "修检工" };
            list.Add(dataBase3PY_BZ_KH_Sum31);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum32 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 32.ToString(), LB = "一期", GD = "二段", GZ = "修检工" };
            list.Add(dataBase3PY_BZ_KH_Sum32);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum33 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 33.ToString(), LB = "一期", GD = "三段", GZ = "修检工" };
            list.Add(dataBase3PY_BZ_KH_Sum33);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum34 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 34.ToString(), LB = "二期", GD = "一段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum34);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum35 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 35.ToString(), LB = "二期", GD = "二段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum35);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum36 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 36.ToString(), LB = "二期", GD = "三段", GZ = "上坯工" };
            list.Add(dataBase3PY_BZ_KH_Sum36);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum37 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 37.ToString(), LB = "二期", GD = "一段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum37);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum38 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 38.ToString(), LB = "二期", GD = "二段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum38);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum39 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 39.ToString(), LB = "二期", GD = "三段", GZ = "擦水工" };
            list.Add(dataBase3PY_BZ_KH_Sum39);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum40 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 40.ToString(), LB = "二期", GD = "一段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum40);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum41 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 41.ToString(), LB = "二期", GD = "二段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum41);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum42 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 42.ToString(), LB = "二期", GD = "三段", GZ = "找枪工" };
            list.Add(dataBase3PY_BZ_KH_Sum42);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum43 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 43.ToString(), LB = "二期", GD = "一段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum43);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum44 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 44.ToString(), LB = "二期", GD = "二段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum44);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum45 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 45.ToString(), LB = "二期", GD = "三段", GZ = "擦绺工" };
            list.Add(dataBase3PY_BZ_KH_Sum45);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum46 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 46.ToString(), LB = "二期", GD = "一段", GZ = "刮底工" };
            list.Add(dataBase3PY_BZ_KH_Sum46);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum47 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 47.ToString(), LB = "二期", GD = "二段", GZ = "刮底工" };
            list.Add(dataBase3PY_BZ_KH_Sum47);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum48 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 48.ToString(), LB = "二期", GD = "三段", GZ = "刮底工" };
            list.Add(dataBase3PY_BZ_KH_Sum48);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum49 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 49.ToString(), LB = "二期", GD = "一段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum49);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum50 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 50.ToString(), LB = "二期", GD = "二段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum50);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum51 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 51.ToString(), LB = "二期", GD = "三段", GZ = "机器人维护工" };
            list.Add(dataBase3PY_BZ_KH_Sum51);

            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum52 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 52.ToString(), LB = "二期", GD = "一段", GZ = "下板工" };
            list.Add(dataBase3PY_BZ_KH_Sum52);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum53 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 53.ToString(), LB = "二期", GD = "二段", GZ = "下板工" };
            list.Add(dataBase3PY_BZ_KH_Sum53);
            DataBase3PY_BZ_KH_Sum dataBase3PY_BZ_KH_Sum54 = new DataBase3PY_BZ_KH_Sum { Id = Guid.NewGuid(), No = 54.ToString(), LB = "二期", GD = "三段", GZ = "下板工" };
            list.Add(dataBase3PY_BZ_KH_Sum54);

            return list;
        }

        private bool Recount(List<DataBase3PY_BZ_KH> list, out List<DataBase3PY_BZ_KH_Sum> listSum)
        {
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据，无法计算！");
                listSum = null;
                return false;
            }
            listSum = GetSumBaseList();
            try
            {
                var dataBaseDays = new BaseDal<DataBaseDay>().GetList(h => h.FactoryNo == "G003" && h.WorkshopName == "喷釉车间").ToList();
                foreach (var item in list)
                {
                    #region 班组

                    var dbd_BZ = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("班组")).FirstOrDefault();
                    if (dbd_BZ != null)
                    {
                        item.BZ_BZKHZB = dbd_BZ.BZKHZB;
                        item.BZ_BZJSB = dbd_BZ.BZJSB;
                        item.BZ_BZJLDJ = dbd_BZ.BZJLDJ;
                    }

                    #endregion 班组

                    #region 机器人维护工

                    var dbd_jqr = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("机器人维护工")).FirstOrDefault();
                    if (dbd_jqr != null)
                    {
                        item.JQRWHG_BZKHZB = dbd_jqr.BZKHZB;
                        item.JQRWHG_BZJSB = dbd_jqr.BZJSB;
                        item.JQRWHG_BZJLDJ = dbd_jqr.BZJLDJ;
                    }

                    #endregion 机器人维护工

                    #region 多功能喷釉工

                    var dbd_dgn = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("多功能喷釉工")).FirstOrDefault();
                    if (dbd_dgn != null)
                    {
                        item.DGNPYG_BZKHZB = dbd_dgn.BZKHZB;
                        item.DGNPYG_BZJSB = dbd_dgn.BZJSB;
                        item.DGNPYG_BZJLDJ = dbd_dgn.BZJLDJ;
                    }

                    #endregion 多功能喷釉工

                    #region 擦绺找枪班组

                    var dbd_clzqbz = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("擦绺找枪班组")).FirstOrDefault();
                    if (dbd_clzqbz != null)
                    {
                        item.CLZQBZ_BZKHZB = dbd_clzqbz.BZKHZB;
                    }

                    #endregion 擦绺找枪班组

                    #region 修检班组

                    var dbd_xjbz = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("修检班组")).FirstOrDefault();
                    if (dbd_xjbz != null)
                    {
                        item.JXBZ_BZKHZB = dbd_xjbz.BZKHZB;
                        item.JXBZ_BZJSB = dbd_xjbz.BZJSB;
                        item.JXBZ_BZJLDJ = dbd_xjbz.BZJLDJ;
                    }

                    #endregion 修检班组

                    #region 擦水班组

                    var dbd_csbz = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("擦水班组")).FirstOrDefault();
                    if (dbd_csbz != null)
                    {
                        item.CSBZ_BZKHZB = dbd_csbz.BZKHZB;
                        item.CSBZ_BZJSB = dbd_csbz.BZJSB;
                        item.CSBZ_BZJLDJ = dbd_csbz.BZJLDJ;
                    }

                    #endregion 擦水班组

                    #region 擦绺工

                    var dbd_clg = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("擦绺工")).FirstOrDefault();
                    if (dbd_clg != null)
                    {
                        item.CLG_PYBZKHZB = dbd_clg.ZB_PY_BZKH;
                    }

                    #endregion 擦绺工

                    #region 上坯工

                    var dbd_spg = dataBaseDays.Where(t => t.Classification.Equals(item.LB) && t.PostName.Equals("上坯工")).FirstOrDefault();
                    if (dbd_spg != null)
                    {
                        item.SPG_PYBZKHZB = dbd_spg.ZB_PY_BZKH;
                    }

                    #endregion 上坯工
                }

                foreach (var item in listSum)
                {
                    item.TheYear = dtp.Value.Year;
                    item.TheMonth = dtp.Value.Month;
                    var dbdTmp = list.Where(tt => tt.GD_PYD.Contains(item.GD) && tt.GD_XJD.Contains(item.GD) && tt.LB.Equals(item.LB)).FirstOrDefault();
                    decimal.TryParse(dbdTmp.BZ_SJWC, out decimal k);
                    decimal.TryParse(dbdTmp.BZ_BZKHZB, out decimal o);
                    decimal.TryParse(dbdTmp.BZ_BZJSB, out decimal p);
                    decimal.TryParse(dbdTmp.BZ_BZJLDJ, out decimal q);
                    decimal.TryParse(dbdTmp.SPG_PYBZKHZB, out decimal af);
                    decimal.TryParse(dbdTmp.CLZQBZ_BZKHZB, out decimal x);
                    decimal.TryParse(dbdTmp.CLZQ_SJWC, out decimal n);
                    decimal.TryParse(dbdTmp.CLG_PYBZKHZB, out decimal ae);
                    decimal.TryParse(dbdTmp.DGNPYG_SJWC, out decimal l);
                    decimal.TryParse(dbdTmp.DGNPYG_BZKHZB, out decimal u);
                    decimal.TryParse(dbdTmp.DGNPYG_BZJSB, out decimal v);
                    decimal.TryParse(dbdTmp.DGNPYG_BZJLDJ, out decimal w);
                    decimal.TryParse(dbdTmp.JQRWHG_SJWC, out decimal m);
                    decimal.TryParse(dbdTmp.JQRWHG_BZKHZB, out decimal r);
                    decimal.TryParse(dbdTmp.JQRWHG_BZJSB, out decimal s);
                    decimal.TryParse(dbdTmp.JQRWHG_BZJLDJ, out decimal t);
                    decimal.TryParse(dbdTmp.CSBZ_BZKHZB, out decimal ab);
                    decimal.TryParse(dbdTmp.CSBZ_BZJSB, out decimal ac);
                    decimal.TryParse(dbdTmp.CSBZ_BZJLDJ, out decimal ad);
                    decimal.TryParse(dbdTmp.JXBZ_BZKHZB, out decimal y);
                    decimal.TryParse(dbdTmp.JXBZ_BZJSB, out decimal z);
                    decimal.TryParse(dbdTmp.JXBZ_BZJLDJ, out decimal aa);

                    switch (item.GZ)
                    {
                        case "上坯工":
                            if (k >= o)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((o - k) / p * q * af).ToString();
                            }

                            break;

                        case "找枪工":
                            if (item.LB == "一期")
                            {
                                try
                                {
                                    item.KHJE = (1000 * (1 + (x - n) / x)).ToString();
                                }
                                catch
                                {
                                    item.KHJE = string.Empty;
                                }
                            }
                            if (item.LB == "二期")
                            {
                                if (k >= o)
                                {
                                    item.KHJE = 0.ToString();
                                }
                                else
                                {
                                    item.KHJE = ((o - k) / p * q).ToString();
                                }
                            }
                            break;

                        case "修挡板工":
                        case "干补工":
                        case "吹风工":
                        case "灌釉工":
                            if (k >= o)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((o - k) / p * q).ToString();
                            }
                            break;

                        case "擦绺工":
                            if (item.LB == "一期")
                            {
                                try
                                {
                                    item.KHJE = (1000 * (1 + (x - n) / x) * ae).ToString();
                                }
                                catch
                                {
                                    item.KHJE = string.Empty;
                                }
                            }
                            if (item.LB == "二期")
                            {
                                if (k >= o)
                                {
                                    item.KHJE = 0.ToString();
                                }
                                else
                                {
                                    item.KHJE = ((o - k) / p * q).ToString();
                                }
                            }

                            break;

                        case "刮底工":
                            if (k >= o)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((o - k) / p * q).ToString();
                            }
                            break;

                        case "多功能喷釉工":
                            if (l >= u)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((u - l) / v * w).ToString();
                            }
                            break;

                        case "机器人维护工":
                            if (m >= r)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((r - m) / s * t).ToString();
                            }
                            break;

                        case "擦水工":
                            if (item.LB == "一期")
                            {
                                if (k >= ab)
                                {
                                    item.KHJE = 0.ToString();
                                }
                                else
                                {
                                    item.KHJE = ((ab - k) / ac * ad).ToString();
                                }
                            }
                            if (item.LB == "二期")
                            {
                                if (k >= o)
                                {
                                    item.KHJE = 0.ToString();
                                }
                                else
                                {
                                    item.KHJE = ((o - k) / p * q).ToString();
                                }
                            }
                            break;

                        case "修检工":
                            if (k >= y)
                            {
                                item.KHJE = 0.ToString();
                            }
                            else
                            {
                                item.KHJE = ((y - k) / z * aa).ToString();
                            }
                            break;

                        default:
                            item.KHJE = string.Empty;
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入错误，详细错误为：{ex.Message}", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                    List<DataBase3PY_BZ_KH> list = new ExcelHelper<DataBase3PY_BZ_KH>().ReadExcel(openFileDlg.FileName, 3, 6, 0, 0, 0, true);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("导入失败，请检查Excel数据是否正确或者网络是否正确，没有数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (string.IsNullOrEmpty(item.GD_PYD) || string.IsNullOrEmpty(item.GD_XJD) || string.IsNullOrEmpty(item.LB))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                        item.CreateTime = Program.NowTime;
                        item.CreateUser = Program.User.ToString();
                        item.TheYear = dtp.Value.Year;
                        item.TheMonth = dtp.Value.Month;
                    }
                    if (Recount(list, out List<DataBase3PY_BZ_KH_Sum> listSum) && new BaseDal<DataBase3PY_BZ_KH_Sum>().ExecuteSqlCommand($"delete from DataBase3PY_BZ_KH_Sum where TheYear={dtp.Value.Year} and TheMonth={dtp.Value.Month}") >= 0 && new BaseDal<DataBase3PY_BZ_KH>().Add(list) > 0 && new BaseDal<DataBase3PY_BZ_KH_Sum>().Add(listSum) > 0)
                    {
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
                List<DataBase3PY_BZ_KH> list = dgv.DataSource as List<DataBase3PY_BZ_KH>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_BZ_KH>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——喷釉班组考核.xlsx", saveFileDlg.FileName, list, 3, 6, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
            var list = new BaseDal<DataBase3PY_BZ_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            decimal maxNo = 1M;
            if (list != null && list.Count > 0)
            {
                maxNo = list.Max(t => decimal.TryParse(t.No, out decimal no) ? no : 0);
                maxNo++;
            }
            DataBase3PY_BZ_KH DataBase3PY_BZ_KH = new DataBase3PY_BZ_KH() { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dtp.Value.Year, TheMonth = dtp.Value.Month, No = maxNo.ToString() };
            FrmModify<DataBase3PY_BZ_KH> frm = new FrmModify<DataBase3PY_BZ_KH>(DataBase3PY_BZ_KH, header, OptionType.Add, Text, 5, 18);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                btnRecount.PerformClick();
                btnSearch.PerformClick();
            }
        }

        private void btnRecount_Click(object sender, EventArgs e)
        {
            List<DataBase3PY_BZ_KH> list = new BaseDal<DataBase3PY_BZ_KH>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList();
            if (Recount(list, out List<DataBase3PY_BZ_KH_Sum> listSum))
            {
                Enabled = false;
                foreach (var item in list)
                {
                    new BaseDal<DataBase3PY_BZ_KH>().Edit(item);
                }
                new BaseDal<DataBase3PY_BZ_KH_Sum>().ExecuteSqlCommand($"delete from DataBase3PY_BZ_KH_Sum where TheYear={dtp.Value.Year} and TheMonth={dtp.Value.Month}");
                new BaseDal<DataBase3PY_BZ_KH_Sum>().Add(listSum);
                Enabled = true;
                btnSearch.PerformClick();
                MessageBox.Show("操作成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSumExportExcel_Click(object sender, EventArgs e)
        {
            btnSumSearch.PerformClick();
            SaveFileDialog saveFileDlg = new SaveFileDialog()
            {
                Filter = "Excle2007文件|*.xlsx"
            };
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                List<DataBase3PY_BZ_KH_Sum> list = dgv.DataSource as List<DataBase3PY_BZ_KH_Sum>;
                var hj = list[0];
                list.Remove(hj);
                list.Add(hj);
                if (new ExcelHelper<DataBase3PY_BZ_KH_Sum>().WriteExcle(Application.StartupPath + "\\Excel\\模板导出三厂——喷釉——喷釉班组考核结果.xlsx", saveFileDlg.FileName, list, 2, 4, 0, 0, 0, 0, dtp.Value.ToString("yyyy-MM")))
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
                if (dgv.SelectedRows[0].DataBoundItem is DataBase3PY_BZ_KH DataBase3PY_BZ_KH)
                {
                    if (DataBase3PY_BZ_KH.No == "合计")
                    {
                        MessageBox.Show("【合计】不能修改！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    FrmModify<DataBase3PY_BZ_KH> frm = new FrmModify<DataBase3PY_BZ_KH>(DataBase3PY_BZ_KH, header, OptionType.Modify, Text, 5, 18);
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
                var DataBase3PY_BZ_KH = dgv.SelectedRows[0].DataBoundItem as DataBase3PY_BZ_KH;
                if (MessageBox.Show("警告：数据删除后不能恢复，确定要删除？", "删除警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (DataBase3PY_BZ_KH != null)
                    {
                        if (DataBase3PY_BZ_KH.No == "合计")
                        {
                            if (MessageBox.Show("要删除，日期为：" + dtp.Value.ToString("yyyy年MM月") + "所有数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                var list = dgv.DataSource as List<DataBase3PY_BZ_KH>;
                                list.RemoveAt(0);
                                foreach (var item in list)
                                {
                                    new BaseDal<DataBase3PY_BZ_KH>().Delete(item);
                                }
                                new BaseDal<DataBase3PY_BZ_KH_Sum>().ExecuteSqlCommand($"delete from DataBase3PY_BZ_KH_Sum where TheYear={dtp.Value.Year} and TheMonth={dtp.Value.Month}");
                                btnSearch.PerformClick();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                        FrmModify<DataBase3PY_BZ_KH> frm = new FrmModify<DataBase3PY_BZ_KH>(DataBase3PY_BZ_KH, header, OptionType.Delete, Text, 5, 0);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            btnSearch.PerformClick();
                        }
                    }
                }
            }
        }
    }
}