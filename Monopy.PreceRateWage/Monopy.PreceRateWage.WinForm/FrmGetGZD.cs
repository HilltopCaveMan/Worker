using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmGetGZD : Office2007Form
    {
        public FrmGetGZD()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefCmbFactory(DateTime dateTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct Factory from DataBaseGeneral_CQ where theYear=" + dateTime.Year.ToString() + " and theMonth=" + dateTime.Month.ToString()).ToList();
                cmbFactory.DataSource = list;
            }
        }

        private void Dtp_ValueChanged(object sender, EventArgs e)
        {
            RefCmbFactory(dtp.Value);
        }

        private void FrmGetGZD_Load(object sender, EventArgs e)
        {
            dtp.Value = new DateTime(Program.NowTime.Year, Program.NowTime.Month, 1);
            RefCmbFactory(dtp.Value);
        }

        private void CmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                List<string> list = conn.Query<string>("select distinct dept from DataBaseGeneral_CQ where theYear=" + dtp.Value.Year.ToString() + " and theMonth=" + dtp.Value.Month.ToString() + " and factory = '" + cmbFactory.Text + "'").ToList();
                //list.Insert(0, "全部");
                cmbDept.DataSource = list;
            }
        }

        private StringBuilder Detection(DateTime dateTime, string factory, string dept)
        {
            StringBuilder msg = new StringBuilder();
            //出勤
            if (string.IsNullOrEmpty(factory) || string.IsNullOrEmpty(dept))
            {
                msg.Append("无【出勤】数据！");
            }
            switch (factory)
            {
                case "三厂":
                    switch (dept)
                    {
                        case "检包车间":
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                int count = 0;
                                //成检计件
                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_cjjj where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【成检计件】数据！");
                                }
                                else
                                {
                                    msg.Append("【成检计件】:" + count.ToString());
                                }
                                msg.Append(Environment.NewLine);

                                //工龄
                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databasegeneral_gl where theyear=" + dateTime.Year.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【工龄工资】数据！");
                                }
                                else
                                {
                                    msg.Append("【工龄工资】:" + count.ToString());
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_xcsj_wx where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【叉车/外协】数据！");
                                }
                                else
                                {
                                    msg.Append("【叉车/外协】:" + count.ToString());
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_xwrycq where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【线位人员出勤日录入】数据！");
                                }
                                else
                                {
                                    msg.Append("【线位人员出勤日录入】:" + count.ToString());
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_jjrlr where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【计件日录入月确认】数据！");
                                }
                                else
                                {
                                    msg.Append("【计件日录入月确认】:" + count.ToString());
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_fzyh where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【辅助验货】数据！");
                                }
                                else
                                {
                                    msg.Append($"【辅助验货】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from database3jb_xzf where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【线长费】数据！");
                                }
                                else
                                {
                                    msg.Append($"【线长费】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_JT where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【交通补助】数据！");
                                }
                                else
                                {
                                    msg.Append($"【交通补助】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_zf where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【租房补助】数据！");
                                }
                                else
                                {
                                    msg.Append($"【租房补助】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_YEY where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【幼儿园补助】数据！");
                                }
                                else
                                {
                                    msg.Append($"【幼儿园补助】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_tjf where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【体检费】数据！");
                                }
                                else
                                {
                                    msg.Append($"【体检费】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_jsf where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【介绍费】数据！");
                                }
                                else
                                {
                                    msg.Append($"【介绍费】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_cp where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【车票】数据！");
                                }
                                else
                                {
                                    msg.Append($"【车票】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_wx where theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【五星补助】数据！");
                                }
                                else
                                {
                                    msg.Append($"【五星补助】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_jc_dept where factoryno='3' and dept ='jb' and thetype='workshop' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【车间奖惩】数据！");
                                }
                                else
                                {
                                    msg.Append($"【车间奖惩】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_jc_dept where factoryno='3' and dept ='jb' and thetype='Cj' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【成检奖惩】数据！");
                                }
                                else
                                {
                                    msg.Append($"【成检奖惩】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_jc_dept where factoryno='3' and dept ='jb' and thetype='Pg' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【品管奖惩】数据！");
                                }
                                else
                                {
                                    msg.Append($"【品管奖惩】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_gzzrx where factory='三工厂' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【雇主责任险扣款】数据！");
                                }
                                else
                                {
                                    msg.Append($"【雇主责任险扣款】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_jc_factory where factory='三厂' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【公司奖罚】数据！");
                                }
                                else
                                {
                                    msg.Append($"【公司奖罚】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_hzjj_noMoney where factory='三工厂' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【互助基金不扣款】数据！");
                                }
                                else
                                {
                                    msg.Append($"【互助基金不扣款】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);

                                count = Convert.ToInt32(conn.ExecuteScalar("select count(id) from databaseGeneral_bx where factory='3' and theyear=" + dateTime.Year.ToString() + " and themonth=" + dateTime.Month.ToString()));
                                if (count == 0)
                                {
                                    msg.Append("无【代扣公积金、保险】数据！");
                                }
                                else
                                {
                                    msg.Append($"【代扣公积金、保险】:{count.ToString()}");
                                }
                                msg.Append(Environment.NewLine);
                            }
                            break;

                        case "模具车间":
                            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            //{
                            //}
                            break;

                        case "原料车间":

                            break;
                    }
                    break;
            }
            return msg;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            txtMsg.Clear();
            txtMsg.Text = "开始检查，请稍后……" + Environment.NewLine;
            txtMsg.Text += Detection(dtp.Value, cmbFactory.Text, cmbDept.Text).ToString();
            txtMsg.Text += "检查结束。";
        }

        private bool CheckCQDay(string userCode, string userName, int year, int month)
        {
            return MyDal.YZ3JB_CQ(userCode, userName, year, month, Program.NowTime, Program.HrCode);
        }

        private void Calculation_New(object obj)
        {
            var gzd_work = obj as GZD_Work;
            DateTime dateTime = gzd_work.Time;

            #region 原料验证

            var listYlJJtjb = new BaseDal<DataBase3YL_JJTJB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            var d28 = listYlJJtjb.Sum(t => decimal.TryParse(t.JJGZ, out decimal d_jjgz) ? d_jjgz : 0M);
            var jjSum = new BaseDal<DataBase3YL_JJHS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList().Sum(t => decimal.TryParse(t.JJGZ, out decimal d_jjgz) ? d_jjgz : 0M);
            if (d28 > jjSum)
            {
                //报警
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "原料计件工资合计未通过验证", Msg = "计件工资：" + d28.ToString() + ",计件核算计件工资合计：" + jjSum.ToString() + ",验证失败", MsgClass = "原料报警", UserCode = listYlJJtjb.FirstOrDefault().CreateUser.Split('_')[0] }, true);
            }
            var e28 = listYlJJtjb.Sum(t => decimal.TryParse(t.JYG, out decimal d_jyg) ? d_jyg : 0M);
            var kh = new BaseDal<DataBase3YL_JYGKHB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList().Sum(t => decimal.TryParse(t.KHZE, out decimal khze) ? khze : 0M);
            var yz_month = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间" && t.PostName == "加釉工").ToList().Sum(t => (decimal.TryParse(t.MoneyBase, out decimal b_jbgz) ? b_jbgz : 0M) + (decimal.TryParse(t.MoneyJN, out decimal b_jn) ? b_jn : 0M) + (decimal.TryParse(t.MoneyJB, out decimal b_jb) ? b_jb : 0M));
            if (e28 > kh + yz_month)
            {
                //bj
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "原料加釉工合计未通过验证", Msg = "加釉工工资合计：" + e28.ToString() + ",月基础数据合计：" + yz_month.ToString() + ",考核合计：" + kh.ToString() + ",验证失败！", MsgClass = "原料报警", UserCode = listYlJJtjb.FirstOrDefault().CreateUser.Split('_')[0] }, true);
            }
            var f28 = listYlJJtjb.Sum(t => decimal.TryParse(t.QYG, out decimal d_qyg) ? d_qyg : 0M);
            var baseMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间" && t.PostName == "清釉工").ToList().Max(t => decimal.TryParse(t.MoneyBase, out decimal d_baseMoney) ? d_baseMoney : 0M);
            if (f28 > baseMonth)
            {
                //bj
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "原料清釉工合计未通过验证", Msg = "清釉工工资合计：" + f28.ToString() + ",月基础数据，清釉工的基本工资：" + baseMonth.ToString() + ",验证失败！", MsgClass = "原料报警", UserCode = listYlJJtjb.FirstOrDefault().CreateUser.Split('_')[0] }, true);
            }

            #endregion 原料验证

            #region 喷釉验证

            List<string> listGH = new List<string> { "B201", "B202", "B203", "B301", "B302", "B303" };
            foreach (var item in listGH)
            {
                var cjps = new BaseDal<DataBase3PY_PS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.GH.Equals(item)).ToList();
                var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G003") && t.Dept.Equals("喷釉") && t.GH.Equals(item)).ToList();
                if (cjps.Count == 0)
                {
                    if (pmcps.Count > 0)
                    {
                        BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "喷釉破损和PMC破损不一致", Msg = $"工号：{item}，车间无数据，PMC有数据", MsgClass = "喷釉报警", UserCode = Program.HrCode }, false);
                    }
                    continue;
                }

                var py_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
                var py_cjps = cjps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M);

                if (py_pmcps != py_cjps)
                {
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "喷釉破损和PMC破损不一致", Msg = $"工号：{item}，PMC数量：{py_pmcps.ToString()}，车间数量：{py_cjps.ToString()}", MsgClass = "喷釉报警", UserCode = cjps.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
            }

            #endregion 喷釉验证

            List<GZD> list = new List<GZD>();
            List<GZD> listSave = new List<GZD>();
            var list_user = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).GroupBy(t => t.UserCode).Select(t => new { usercode = t.Key });
            foreach (var item in list_user)
            {
                list.Clear();
                List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item.usercode).ToList().OrderByDescending(t => decimal.TryParse(t.DayScq, out decimal d_scq) ? d_scq : 0M).ToList();
                //var item_cq = list_CQ[0];
                foreach (var item_cq in list_CQ)
                {
                    if (item_cq.Factory == "三厂")
                    {
                        switch (item_cq.Dept)
                        {
                            case "检包车间":
                                GZD gzd3jb = Calculation3JB(list, item_cq);
                                if (gzd3jb == null)
                                {
                                    continue;
                                }
                                gzd3jb.IsEnd = false;
                                list.Add(gzd3jb);
                                listSave.Add(gzd3jb);
                                break;

                            case "模具车间":
                                GZD gzd3mj = Calculation3MJ(list, item_cq);
                                if (gzd3mj == null)
                                {
                                    continue;
                                }
                                gzd3mj.IsEnd = false;
                                list.Add(gzd3mj);
                                listSave.Add(gzd3mj);
                                break;

                            case "原料车间":
                                GZD gzd3yl = Calculation3YL(list, item_cq);
                                if (gzd3yl == null)
                                {
                                    continue;
                                }
                                gzd3yl.IsEnd = false;
                                list.Add(gzd3yl);
                                listSave.Add(gzd3yl);
                                break;

                            case "喷釉车间":
                                GZD gzd3py = Calculation3PY(list, item_cq);
                                if (gzd3py == null)
                                {
                                    continue;
                                }
                                gzd3py.IsEnd = false;
                                list.Add(gzd3py);
                                listSave.Add(gzd3py);
                                break;

                            case "烧成车间":
                                GZD gzd3sc = Calculation3SC(list, item_cq);
                                if (gzd3sc == null)
                                {
                                    continue;
                                }
                                gzd3sc.IsEnd = false;
                                list.Add(gzd3sc);
                                listSave.Add(gzd3sc);
                                break;

                            case "成型车间":
                                if (item_cq.Remark.Contains("面具"))
                                {
                                    GZD gzd3cx_mj = Calculation3CX_GYMJ(list, item_cq);
                                    if (gzd3cx_mj == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_mj.IsEnd = false;
                                    list.Add(gzd3cx_mj);
                                    listSave.Add(gzd3cx_mj);
                                }
                                else if (item_cq.Remark.Contains("水箱"))
                                {
                                    GZD gzd3cx_sx = Calculation3CX_GYSX(list, item_cq);
                                    if (gzd3cx_sx == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_sx.IsEnd = false;
                                    list.Add(gzd3cx_sx);
                                    listSave.Add(gzd3cx_sx);
                                }
                                else if (item_cq.Remark.Contains("大套"))
                                {
                                    GZD gzd3cx_dt = Calculation3CX_GYDT(list, item_cq);
                                    if (gzd3cx_dt == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_dt.IsEnd = false;
                                    list.Add(gzd3cx_dt);
                                    listSave.Add(gzd3cx_dt);
                                }
                                else if (item_cq.Position.Contains("技术员"))
                                {
                                    GZD gzd3cx_jsy = Calculation3CX_JSY(list, item_cq);
                                    if (gzd3cx_jsy == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_jsy.IsEnd = false;
                                    list.Add(gzd3cx_jsy);
                                    listSave.Add(gzd3cx_jsy);
                                }
                                else
                                {
                                    //成形其他
                                    GZD gzd3cx_cx = Calculation3CX_CX(list, item_cq);
                                    if (gzd3cx_cx == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_cx.IsEnd = false;
                                    list.Add(gzd3cx_cx);
                                    listSave.Add(gzd3cx_cx);
                                }
                                break;

                            case "青白坯库":
                                if (item_cq.Position.Contains("成型拉坯工"))
                                {
                                    GZD gzd3cx_lp = Calculation3CX_BJLP(list, item_cq);
                                    if (gzd3cx_lp == null)
                                    {
                                        continue;
                                    }
                                    gzd3cx_lp.IsEnd = false;
                                    list.Add(gzd3cx_lp);
                                    listSave.Add(gzd3cx_lp);
                                }
                                break;
                        }
                    }

                }
                GZD gzdMain = Calculation3Pub(list, list_CQ);
                if (gzdMain != null)
                {
                    gzdMain.IsEnd = true;
                    listSave.Add(gzdMain);
                }
            }

            #region 成型高压面具验证

            //定员12	*（基本工资+技能+加班）
            var dy = new BaseDal<BaseHeadcount>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Factory == "三厂" && t.DeptName == "成型车间" && t.Name == "高压面具");
            if (dy == null)
            {
                //没有定员，无法验证.
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压面具，无【定员】，无法验证", Msg = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压面具，无【定员】，无法验证", MsgClass = "成型高压面具报警", UserCode = Program.HrCode }, false);
            }
            decimal.TryParse(dy.UserCount, out decimal dyCount);
            var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName.Contains("面具"));
            if (dataMonth == null)
            {
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压面具，无【月基础数据】，无法验证", Msg = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压面具，无【月基础数据】，无法验证", MsgClass = "成型高压面具报警", UserCode = Program.HrCode }, false);
            }
            decimal.TryParse(dataMonth.MoneyBase, out decimal basegz);
            decimal.TryParse(dataMonth.MoneyJN, out decimal jngz);
            decimal.TryParse(dataMonth.MoneyJB, out decimal jbgz);
            var sum1 = dyCount * (basegz + jngz + jbgz);
            //工资核算表每一个总金额*班长费的个数
            var bzf = new BaseDal<DataBase3CX_GYMJ_07_BZFGY>().GetList(t => t.TheType.Contains("面具") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).Count();
            var khb = new BaseDal<DataBase3CX_GYMJ_081_ZLWCHZB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList().GroupBy(t => new { t.GH, t.ZJE }).Select(t => new DataBase3CX_GYMJ_081_ZLWCHZB { GH = t.Key.GH, ZJE = t.Key.ZJE }).ToList();
            var sum2 = 0M;
            foreach (var item in khb)
            {
                sum2 += (decimal.TryParse(item.ZJE, out decimal d_zje) ? d_zje : 0M) * ((decimal.TryParse(dy.UserCount, out decimal d_Dy) ? d_Dy : 0M) / bzf);
            }
            var sum3 = 0M;
            //每个人的基本工资+技能+加班+质量考核+【加班】的金额
            foreach (var item in listSave.Where(t => t.IsEnd == true && t.Factory == "三厂" && t.Dept == "成型车间" && t.Position == "面具"))
            {
                decimal.TryParse(item.JBGZE, out decimal s1);
                decimal.TryParse(item.JNGZE, out decimal s2);
                decimal.TryParse(item.JBGZE2, out decimal s3);
                decimal.TryParse(item.ZLKHGZ, out decimal s4);
                decimal.TryParse(item.TBGZ, out decimal s5);
                sum3 += s1 + s2 + s3 + s4 + s5;
            }
            if (sum1 + sum2 < sum3)
            {
                foreach (var item in listSave.Where(t => t.IsEnd == true && t.Factory == "三厂" && t.Dept == "成型车间" && t.Position == "面具"))
                {
                    item.IsCheckCQDayOk = false;
                }
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.ToString("yyyy年MM月")}成型车间--高压面具工资超总额", Msg = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压面具，工资总数验证未通过{sum1}+{sum2}不能小于{sum3}", MsgClass = "成型高压面具报警", UserCode = "Admin" }, true);
            }

            #endregion 成型高压面具验证

            #region 成型高压水箱验证

            var yz1 = listSave.Where(t => t.IsEnd && t.Factory == "三厂" && t.Dept == "成型车间" && t.Position == "水箱").ToList().Sum(t => (decimal.TryParse(t.JJGZ1, out decimal d1) ? d1 : 0M) + (decimal.TryParse(t.TBGZ, out decimal d2) ? d2 : 0M));
            var yz2 = new BaseDal<DataBase3CX_GYSX_02_GYSX>().GetList(t => t.TheYear == dtp.Value.Year && t.TheMonth == dtp.Value.Month).ToList().Sum(t => decimal.TryParse(t.GZ, out decimal d) ? d : 0M);
            if (Math.Round(yz1, 5) > Math.Round(yz2, 5))
            {
                foreach (var item in listSave.Where(t => t.IsEnd == true && t.Factory == "三厂" && t.Dept == "成型车间" && t.Position == "水箱"))
                {
                    item.IsCheckCQDayOk = false;
                }

                //bj
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.ToString("yyyy年MM月")}成型车间--高压水箱工资超总额", Msg = $"{dateTime.ToString("yyyy年MM月")}成型车间-高压水箱，工资总数验证未通过{yz1}(工资单的计件工资+替班工资)不能大于等于{yz2}(工资核算表的金额合计)", MsgClass = "成型高压水箱报警", UserCode = "Admin" }, true);
            }

            #endregion 成型高压水箱验证

            new BaseDal<GZD>().Add(listSave);
            MethodInvoker invoke = () =>
            {
                btnRun.Enabled = true;
                MessageBox.Show("计算完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            if (InvokeRequired)
            {
                Invoke(invoke);
            }
            else
            {
                invoke();
            }
        }

        private void Calculation1Factory_New(object obj)
        {
            var gzd_work = obj as GZD_Work;
            DateTime dateTime = gzd_work.Time;

            List<DataBase1GZD> list = new List<DataBase1GZD>();
            List<DataBase1GZD> listSave = new List<DataBase1GZD>();
            var list_user = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).GroupBy(t => t.UserCode).Select(t => new { usercode = t.Key });
            foreach (var item in list_user)
            {
                list.Clear();
                List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item.usercode).ToList().OrderByDescending(t => decimal.TryParse(t.DayScq, out decimal d_scq) ? d_scq : 0M).ToList();
                //var item_cq = list_CQ[0];
                foreach (var item_cq in list_CQ)
                {
                    if (item_cq.Factory == "一厂")
                    {
                        switch (item_cq.Dept)
                        {
                            case "仓储":
                                DataBase1GZD gzd1cc = Calculation1CC(list, item_cq);
                                if (gzd1cc == null)
                                {
                                    continue;
                                }
                                list.Add(gzd1cc);
                                listSave.Add(gzd1cc);
                                break;

                            case "检包车间":
                                DataBase1GZD gzd1jb = Calculation1JB(list, item_cq);
                                if (gzd1jb == null)
                                {
                                    continue;
                                }
                                list.Add(gzd1jb);
                                listSave.Add(gzd1jb);
                                break;

                            case "模具车间":
                                DataBase1GZD gzd1mj = Calculation1MJ(list, item_cq);
                                if (gzd1mj == null)
                                {
                                    continue;
                                }

                                list.Add(gzd1mj);
                                listSave.Add(gzd1mj);
                                break;

                            case "原料车间":
                                DataBase1GZD gzd1yl = Calculation1YL(list, item_cq);
                                if (gzd1yl == null)
                                {
                                    continue;
                                }

                                list.Add(gzd1yl);
                                listSave.Add(gzd1yl);
                                break;

                            case "喷釉车间":
                                DataBase1GZD gzd1py = Calculation1PY(list, item_cq);
                                if (gzd1py == null)
                                {
                                    continue;
                                }

                                list.Add(gzd1py);
                                listSave.Add(gzd1py);
                                break;

                            case "烧成车间":
                                DataBase1GZD gzd1sc = Calculation1SC(list, item_cq);
                                if (gzd1sc == null)
                                {
                                    continue;
                                }

                                list.Add(gzd1sc);
                                listSave.Add(gzd1sc);
                                break;

                            case "修检线":
                                DataBase1GZD gzd1xjx = Calculation1XJX(list, item_cq);
                                if (gzd1xjx == null)
                                {
                                    continue;
                                }

                                list.Add(gzd1xjx);
                                listSave.Add(gzd1xjx);
                                break;

                            case "成型车间":

                                if (item_cq.Position.Contains("多能工"))
                                {
                                    DataBase1GZD gzd1cx_dng = Calculation1CX_DNG(list, item_cq);
                                    if (gzd1cx_dng == null)
                                    {
                                        continue;
                                    }

                                    list.Add(gzd1cx_dng);
                                    listSave.Add(gzd1cx_dng);
                                }
                                else if (item_cq.Position.Contains("成型拉坯工"))
                                {
                                    DataBase1GZD gzd1cx_lp = Calculation1CX_BJLP(list, item_cq);
                                    if (gzd1cx_lp == null)
                                    {
                                        continue;
                                    }

                                    list.Add(gzd1cx_lp);
                                    listSave.Add(gzd1cx_lp);
                                }
                                else
                                {
                                    DataBase1GZD gzd1cx = Calculation1CX(list, item_cq);
                                    if (gzd1cx == null)
                                    {
                                        continue;
                                    }

                                    list.Add(gzd1cx);
                                    listSave.Add(gzd1cx);
                                }

                                break;
                           
                        }
                    }
                }
            }

            new BaseDal<DataBase1GZD>().Add(listSave);
            MethodInvoker invoke = () =>
            {
                btnRun.Enabled = true;
                MessageBox.Show("计算完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            if (InvokeRequired)
            {
                Invoke(invoke);
            }
            else
            {
                invoke();
            }
        }

        private void Calculation2Factory_New(object obj)
        {
            var gzd_work = obj as GZD_Work;
            DateTime dateTime = gzd_work.Time;

            List<DataBase2GZD> list = new List<DataBase2GZD>();
            List<DataBase2GZD> listSave = new List<DataBase2GZD>();
            var list_user = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).GroupBy(t => t.UserCode).Select(t => new { usercode = t.Key });
            foreach (var item in list_user)
            {
                list.Clear();
                List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item.usercode).ToList().OrderByDescending(t => decimal.TryParse(t.DayScq, out decimal d_scq) ? d_scq : 0M).ToList();
                //var item_cq = list_CQ[0];
                foreach (var item_cq in list_CQ)
                {
                    if (item_cq.Factory == "二厂")
                    {
                        switch (item_cq.Dept)
                        {
                            case "仓储":
                                DataBase2GZD gzd2cc = Calculation2CC(list, item_cq);
                                if (gzd2cc == null)
                                {
                                    continue;
                                }
                                list.Add(gzd2cc);
                                listSave.Add(gzd2cc);
                                break;

                            case "检包车间":
                                DataBase2GZD gzd2jb = Calculation2JB(list, item_cq);
                                if (gzd2jb == null)
                                {
                                    continue;
                                }
                                list.Add(gzd2jb);
                                listSave.Add(gzd2jb);
                                break;

                            case "模具车间":
                                DataBase2GZD gzd2mj = Calculation2MJ(list, item_cq);
                                if (gzd2mj == null)
                                {
                                    continue;
                                }

                                list.Add(gzd2mj);
                                listSave.Add(gzd2mj);
                                break;

                            case "原料车间":
                                DataBase2GZD gzd2yl = Calculation2YL(list, item_cq);
                                if (gzd2yl == null)
                                {
                                    continue;
                                }

                                list.Add(gzd2yl);
                                listSave.Add(gzd2yl);
                                break;

                            case "喷釉车间":
                                DataBase2GZD gzd2py = Calculation2PY(list, item_cq);
                                if (gzd2py == null)
                                {
                                    continue;
                                }

                                list.Add(gzd2py);
                                listSave.Add(gzd2py);
                                break;

                            case "烧成车间":
                                DataBase2GZD gzd2sc = Calculation2SC(list, item_cq);
                                if (gzd2sc == null)
                                {
                                    continue;
                                }

                                list.Add(gzd2sc);
                                listSave.Add(gzd2sc);
                                break;

                            case "成型车间":

                                if (item_cq.Position.Contains("半检工"))
                                {
                                    DataBase2GZD gzd2cx_lp = Calculation2CX_BJLP(list, item_cq);
                                    if (gzd2cx_lp == null)
                                    {
                                        continue;
                                    }

                                    list.Add(gzd2cx_lp);
                                    listSave.Add(gzd2cx_lp);
                                }
                                else
                                {
                                    DataBase2GZD gzd2cx = Calculation2CX(list, item_cq);
                                    if (gzd2cx == null)
                                    {
                                        continue;
                                    }

                                    list.Add(gzd2cx);
                                    listSave.Add(gzd2cx);
                                }
                               
                                break;
                          
                        }
                    }
                }
            }

            new BaseDal<DataBase2GZD>().Add(listSave);
            MethodInvoker invoke = () =>
            {
                btnRun.Enabled = true;
                MessageBox.Show("计算完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            if (InvokeRequired)
            {
                Invoke(invoke);
            }
            else
            {
                invoke();
            }
        }

        /// <summary>
        /// 检包车间工资计算
        /// </summary>
        /// <param name="item_CQ">出勤</param>
        /// <returns></returns>
        private GZD Calculation3JB(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3JB").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            bool checkCQDay = CheckCQDay(item_CQ.UserCode, item_CQ.UserName, dateTime.Year, dateTime.Month);
            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, IsCheckCQDayOk = checkCQDay, Factory = item_CQ.Factory, Dept = item_CQ.Dept, Position = item_CQ.Position, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayDx, out decimal d) ? d : 0m).ToString() /*item_CQ.DayDx*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString()/*item_CQ.DayYcq*/, CalculationType = "3JB", UserType = "3JB" };

            var jbgze = new BaseDal<DataBase3JB_XCSJ_WX>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (jbgze != null)
            {
                gzd.JBGZE = jbgze.Money;
            }
            if (gzd.UserCode == "M16291")
            {
            }
            var cjjj = new BaseDal<DataBase3JB_CJJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (cjjj != null && cjjj.Count > 0)
            {
                gzd.Code = cjjj.Max(t => t.Code);

                gzd.JJGZ1 = string.IsNullOrEmpty(gzd.JJGZ1) ? cjjj.Sum(t => t.Results.Sum(x => x.Money)).ToString() : (Convert.ToDouble(gzd.JJGZ1) + cjjj.Sum(t => t.Results.Sum(x => x.Money))).ToString();
            }
            var mclb = new BaseDal<DataBase3JB_MCLBJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.IsOkLb1 && t.IsOkLb2 && t.IsOkMc1 && t.IsOkMc2 && t.IsOkYkcpg).ToList();
            if (mclb != null && mclb.Count > 0)
            {
                gzd.JJGZ1 = string.IsNullOrEmpty(gzd.JJGZ1) ? mclb.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() : (Convert.ToDecimal(gzd.JJGZ1) + mclb.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M)).ToString();
            }

            var xsmclb = new BaseDal<DataBase3JB_XSMCLBJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (xsmclb != null && xsmclb.Count > 0)
            {
                //gzd.JJGZ1 = string.IsNullOrEmpty(gzd.JJGZ1) ? xsmclb.Money : (Convert.ToDouble(gzd.JJGZ1) + Convert.ToDouble(xsmclb.Money)).ToString();
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (xsmclb.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M) + jjgz1).ToString();
            }

            var xwrycq = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JJGZ2 = xwrycq.Sum(t => string.IsNullOrEmpty(t.DGWGZ) ? 0M : Convert.ToDecimal(t.DGWGZ)).ToString();
            gzd.TBGZ = xwrycq.Sum(t => string.IsNullOrEmpty(t.TBGZE) ? 0M : Convert.ToDecimal(t.TBGZE)).ToString();
            var d_rzbz = xwrycq.Sum(t => string.IsNullOrEmpty(t.RZBZGZ) ? 0M : Convert.ToDecimal(t.RZBZGZ));
            //入职补助需加上入职补助窗体的数据。
            var d2_rzbz = new BaseDal<DataBaseGeneral_RZBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.Money, out decimal d_m) ? d_m : 0M);
            var m_rzbz = new BaseDal<DataBaseGeneral_RZBZ_Month>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M);
            gzd.RZBZ = (d_rzbz + d2_rzbz + m_rzbz).ToString();

            var jj = new BaseDal<DataBase3JB_JJRLR>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            jj = jj.Where(t => t.IsKF_Manager && t.IsPG_Manager && t.IsPMC_Manager && t.IsWX_Manager && t.IsWX_PMC_Manager).ToList();
            gzd.JJGZ3 = jj.Sum(t => string.IsNullOrEmpty(t.JE) ? 0M : Convert.ToDecimal(t.JE)).ToString();

            var fzyh = new BaseDal<DataBase3JB_FZYH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && !string.IsNullOrEmpty(t.Money)).ToList();
            gzd.PGYH = fzyh.Sum(t => Convert.ToDecimal(t.Money)).ToString();

            var xzf = new BaseDal<DataBase3JB_XZF>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.BZF = xzf.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString();
            bool checkXzf = MyDal.YZ3JB_XZF(item_CQ.UserCode, item_CQ.UserName, dateTime.Year, dateTime.Month, Program.NowTime, xzf.Sum(t => decimal.TryParse(t.DayCq, out decimal d_cq) ? d_cq : 0M), decimal.TryParse(item_CQ.DayScq, out decimal d_scq) ? d_scq : 0M, Program.HrCode);
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "jb" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        private GZD Calculation3MJ(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3MJ").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, IsCheckCQDayOk = true, Factory = item_CQ.Factory, Dept = item_CQ.Dept, Position = item_CQ.Position, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayDx, out decimal d) ? d : 0m).ToString()/*item_CQ.DayDx*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString()/*item_CQ.DayYcq*/, CalculationType = "3MJ", UserType = "3MJ" };
            var jbgz = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "模具车间" && t.PostName == item_CQ.Position);
            if (jbgz != null)
            {
                gzd.JiBenGZ = jbgz.MoneyBase;
            }
            gzd.JBGZE = ((decimal.TryParse(gzd.JiBenGZ, out decimal d_jbgz) ? d_jbgz : 0M) * (decimal.TryParse(item_CQ.DayScq, out decimal d_scq) ? d_scq : 0M) / (decimal.TryParse(item_CQ.DayYcq, out decimal d_ycq) ? d_ycq : 0M)).ToString();
            gzd.JBGZE1 = (d_jbgz * (decimal.TryParse(item_CQ.DayDx, out decimal d_jbcq) ? d_jbcq : 0M) / d_ycq).ToString();
            var xsg = new BaseDal<DataBase3MJ_XSGJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (xsg.Count > 0)
            {
                gzd.JJGZ1 = xsg.Sum(t => decimal.TryParse(t.Money, out decimal m_xsg) ? m_xsg : 0M).ToString();
            }
            var djSum = new BaseDal<DataBase3MJ_PMCDJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().GroupBy(t => t.UserCode).Select(t => new { UserCode = t.Key }).ToList();
            decimal d_djmoney = 0M;//大件金额
            if (djSum.Count == 1)
            {
                var itemSum = djSum[0];
                var listDJ = new BaseDal<DataBase3MJ_PMCDJ>().GetList(t => t.UserCode == itemSum.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
                foreach (var itemMain in listDJ)
                {
                    foreach (var item in itemMain.Childs)
                    {
                        d_djmoney += (decimal.TryParse(item.Money, out decimal d_tp) ? d_tp : 0M);
                    }
                }
            }

            var xjSum = new BaseDal<DataBase3MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().GroupBy(t => t.UserCode).Select(t => new { UserCode = t.Key }).ToList();
            decimal d_xjmoney = 0M;//x件金额
            if (xjSum.Count == 1)
            {
                var itemSum = xjSum[0];
                var listDJ = new BaseDal<DataBase3MJ_PMCXJ>().GetList(t => t.UserCode == itemSum.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
                foreach (var itemMain in listDJ)
                {
                    foreach (var item in itemMain.Childs)
                    {
                        d_xjmoney += (decimal.TryParse(item.Money, out decimal d_tp) ? d_tp : 0M);
                    }
                }
            }

            var sxSum = new BaseDal<DataBase3MJ_PMCSX>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().GroupBy(t => t.UserCode).Select(t => new { UserCode = t.Key }).ToList();
            decimal d_sxmoney = 0M;//sx金额
            if (sxSum.Count == 1)
            {
                var itemSum = sxSum[0];
                var listDJ = new BaseDal<DataBase3MJ_PMCSX>().GetList(t => t.UserCode == itemSum.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
                foreach (var itemMain in listDJ)
                {
                    foreach (var item in itemMain.Childs)
                    {
                        d_sxmoney += (decimal.TryParse(item.Money, out decimal d_tp) ? d_tp : 0M);
                    }
                }
            }

            gzd.JJGZ2 = (d_djmoney + d_xjmoney + d_sxmoney).ToString();

            var grkh = new BaseDal<DataBase3MJ_CJKH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (grkh.Count > 0)
            {
                gzd.GRKHGZ = (grkh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M) * d_scq / d_ycq).ToString();
            }

            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "模具").ToList();
            if (fzyh.Count > 0)
            {
                gzd.PGYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            var rzbz = new BaseDal<DataBaseGeneral_RZBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("模具")).ToList();
            if (rzbz.Count > 0)
            {
                //需验证补助天数和出勤天数，助天数和公用的出勤天数验证。如果补助天数大于实出勤，报警。
                var bzts = rzbz.Sum(t => decimal.TryParse(t.Day_BZ, out decimal d_ts) ? d_ts : 0M);
                decimal.TryParse(gzd.DaySCQ, out decimal scq);
                if (bzts > scq)
                {
                    //bj
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "Admin", IsDone = false, IsRead = false, UserCode = "Admin", MsgClass = "模具入职补助错误", MsgTitle = "模具车间入职补助天数【大于】实出勤天数！", Msg = $"{dtp.Value.ToString("yyyy年MM月")}模具车间入职补助天数【大于】实出勤天数，工号：{rzbz[0].UserCode}，姓名：{rzbz[0].UserName}，入职补助天数：{bzts}，实出勤天数：{scq}" }, true);
                }
                gzd.RZBZ = rzbz.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "mj" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        private void BJ(DataBaseMsg dataBaseMsg, bool isToHr)
        {
            new BaseDal<DataBaseMsg>().Add(dataBaseMsg);
            if (isToHr)
            {
                dataBaseMsg.ID = Guid.NewGuid();
                dataBaseMsg.UserCode = Program.HrCode;
                new BaseDal<DataBaseMsg>().Add(dataBaseMsg);
            }
        }

        /// <summary>
        /// 原料车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3YL(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3YL").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, IsCheckCQDayOk = true, Factory = item_CQ.Factory, Dept = item_CQ.Dept, Position = item_CQ.Position, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayDx, out decimal d) ? d : 0m).ToString()/*item_CQ.DayDx*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString()/*item_CQ.DayYcq*/, CalculationType = "3YL", UserType = "3YL" };
            DataBaseMonth jbgz = null;
            if (item_CQ.Position.Contains("供浆工"))
            {
                jbgz = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间" && t.PostName == item_CQ.Position && t.Classification.Contains(item_CQ.Remark));
            }
            else
            {
                jbgz = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "原料车间" && t.PostName == item_CQ.Position);
            }
            if (jbgz != null && !item_CQ.Position.Contains("加釉工"))
            {
                gzd.JiBenGZ = jbgz.MoneyBase;
            }
            gzd.JBGZE = ((decimal.TryParse(gzd.JiBenGZ, out decimal d_jbgz) ? d_jbgz : 0M) * (decimal.TryParse(item_CQ.DayScq, out decimal d_scq) ? d_scq : 0M) / (decimal.TryParse(item_CQ.DayYcq, out decimal d_ycq) ? d_ycq : 0M)).ToString();
            gzd.JBGZE1 = (d_jbgz * (decimal.TryParse(item_CQ.DayDx, out decimal d_jbcq) ? d_jbcq : 0M) / d_ycq).ToString();
            var jjtjb = new BaseDal<DataBase3YL_JJTJB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.QQJ = jjtjb.Sum(t => decimal.TryParse(t.QQJ, out decimal qqj) ? qqj : 0M).ToString();
            if (jjtjb.Count > 0)
            {
                gzd.JJGZ1 = jjtjb.Sum(t => (decimal.TryParse(t.JJGZ, out decimal jjgz) ? jjgz : 0M) + (decimal.TryParse(t.JYG, out decimal jyg) ? jyg : 0M) + (decimal.TryParse(t.QYG, out decimal qyg) ? qyg : 0M)).ToString();
            }

            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "原料").ToList();
            if (fzyh.Count > 0)
            {
                gzd.PGYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }
            var jbspb = new BaseDal<DataBaseGeneral_JBSPB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "原料车间").ToList();
            gzd.TBGZ = jbspb.Sum(t => decimal.TryParse(t.Money, out decimal d_jbspb) ? d_jbspb : 0M).ToString();
            gzd.BZF = (jjtjb.Sum(t => decimal.TryParse(t.BZF, out decimal d_bzf) ? d_bzf : 0M) * d_scq / d_ycq).ToString();

            var m_rzbz = new BaseDal<DataBaseGeneral_RZBZ_Month>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("原料")).ToList().Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M);
            gzd.RZBZ = m_rzbz.ToString();

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "yl" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        /// <summary>
        /// 三厂喷釉工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3PY(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3PY").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayDx, out decimal d) ? d : 0m).ToString() /*item_CQ.DayDx*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3PY", UserType = "3PY", Position = string.Empty };

            var listBG = new BaseDal<DataBase3PY_BG>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().OrderByDescending(t => decimal.TryParse(t.SCQTS, out decimal scq)).ToList();
            var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo.Equals("G003") && t.WorkshopName.Equals(item_CQ.Dept)).ToList();
            if (listBG != null && listBG.Count > 0)
            {
                gzd.IsCheckCQDayOk = true;
                var a1 = listBG.Sum(t => decimal.TryParse(t.SCQTS, out decimal scq) ? scq : 0M);
                var a2 = list_CQ.Sum(t => decimal.TryParse(t.DayTotal, out decimal d) ? d : 0M); /* decimal.TryParse(item_CQ.DayScq, out decimal dscq) ? dscq : 0M;*/
                var jbspb = new BaseDal<DataBaseGeneral_JBSPB>().GetList(t => t.FactoryNo == "G003" && t.Dept == "喷釉车间" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().Sum(t => decimal.TryParse(t.JBTS, out decimal d) ? d : 0M);
                a2 += jbspb;
                if (a1 < a2)
                {
                    //是否少计工了！
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "喷釉车间报工实出勤（熟练工+学徒）小于人资出勤合计", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间实出勤（熟练工+学徒）天数：{a1.ToString()},人资出勤合计：{a2},请核对是否少报工！", MsgClass = "喷釉报警", UserCode = listBG.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
                if (a1 > a2)
                {
                    gzd.IsCheckCQDayOk = false;
                    //记工多了，报警
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "喷釉车间报工实出勤（熟练工+学徒）【大于】人资出勤合计", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间实出勤（熟练工+学徒）天数：{a1.ToString()},人资出勤合计：{a2},报工天数大于实出勤天数！", MsgClass = "喷釉报警", UserCode = listBG.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
                var a3 = listBG.Average(t => decimal.TryParse(t.YCQTS, out decimal ycq) ? ycq : 0M);
                var a4 = (decimal.TryParse(item_CQ.DayYcq, out decimal dycq) ? dycq : 0M);
                if (a3 != a4)
                {
                    //应出勤不相等，报警
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "喷釉车间报工应出勤【不等于】人资应出勤", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间应出勤天数：{a3.ToString()},人资应出勤：{a4},应出勤【不等于】人资应出勤！", MsgClass = "喷釉报警", UserCode = listBG.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }

                gzd.GD = listBG[0].GD;
                gzd.Code = listBG[0].GH;
                gzd.Position = listBG[0].GZ;
                gzd.JBGZE = listBG.Sum(t => decimal.TryParse(t.JBGZE, out decimal d) ? d : 0M).ToString();
                gzd.JNGZE = listBG.Sum(t => decimal.TryParse(t.JNGZE, out decimal d) ? d : 0M).ToString();
                gzd.JBGZE1 = listBG.Sum(t => decimal.TryParse(t.JiaBGZE, out decimal d) ? d : 0M).ToString();
                var tmpBg = listBG.Where(t => t.GZ.Contains("手工橱喷釉工"));
                if (tmpBg.Count() == 0)
                {
                    if (listBG.Sum(t => decimal.TryParse(t.SCQTS, out decimal scq) ? scq : 0M) >= (decimal.TryParse(listBG[0].YCQTS, out decimal ycq) ? ycq : 0M))
                    {
                        var tmpMonth = listMonth.Where(t => t.PostName.Equals(listBG[0].GZ)).FirstOrDefault();
                        if (tmpMonth != null)
                        {
                            gzd.QQJ = tmpMonth.MoneyQQJ;
                        }
                    }
                }
                gzd.JJGZ1 = listBG.Sum(t => decimal.TryParse(t.JXJJGZ, out decimal d) ? d : 0M).ToString();
                gzd.JJGZ2 = listBG.Sum(t => decimal.TryParse(t.PYJJ, out decimal d) ? d : 0M).ToString();
                gzd.JJGZ3 = listBG.Sum(t => decimal.TryParse(t.CSJJ, out decimal d) ? d : 0M).ToString();

                var khBgTmp = listBG.Where(t => t.GZ == "修检工").ToList();
                if (khBgTmp.Count > 0)
                {
                    gzd.CLKHGZ = new BaseDal<DataBase3PY_JXG_KH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode).ToList().Sum(t => decimal.TryParse(t.KHJE2, out decimal d) ? d : 0M).ToString();
                }
                else
                {
                    gzd.CLKHGZ = string.Empty;
                }
                gzd.ZLKHGZ = listBG.Sum(t => (decimal.TryParse(t.PYKH, out decimal d) ? d : 0M) + (decimal.TryParse(t.CSKH, out decimal dd) ? dd : 0M)).ToString();
                gzd.GRKHGZ = listBG.Sum(t => (decimal.TryParse(t.JXGRKHGZ, out decimal d) ? d : 0M) + (decimal.TryParse(t.CLGGRKH, out decimal dd) ? dd : 0M)).ToString();
                gzd.BZKHGZ = listBG.Sum(t => decimal.TryParse(t.BZKHGZ, out decimal d) ? d : 0M).ToString();
            }

            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "喷釉").ToList();
            if (fzyh.Count > 0)
            {
                gzd.PGYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            var rzbz = new BaseDal<DataBaseGeneral_RZBZ_Month>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("喷釉")).ToList();
            if (rzbz.Count > 0)
            {
                gzd.RZBZ = rzbz.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("喷釉")).ToList();
            if (kfss.Count > 0)
            {
                gzd.KFSSBZ = kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m).ToString();
            }

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "py" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            var cjps = new BaseDal<DataBase3PY_PS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_cjps = 0M;
            if (cjps.Count > 0)
            {
                d_cjps = cjps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G003") && t.Dept.Equals("喷釉") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = ((d_cjps + d_pmcps) > 0M) ? (d_cjps + d_pmcps).ToString() : string.Empty;
            return gzd;
        }

        /// <summary>
        /// 三厂烧成工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3SC(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3SC").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayDx, out decimal d) ? d : 0m).ToString() /*item_CQ.DayDx*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3SC", UserType = "3SC" };

            var listBG = new BaseDal<DataBase3SC_13_BG>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().OrderByDescending(t => decimal.TryParse(t.YCQTS, out decimal d) ? d : 0M).ToList();
            var listMonth = new BaseDal<DataBaseMonth>().GetList(t => t.FactoryNo.Equals("G003") && t.WorkshopName.Equals(item_CQ.Dept) && string.IsNullOrEmpty(t.MonthData) && !string.IsNullOrEmpty(t.MoneyQQJ)).ToList();
            gzd.IsCheckCQDayOk = true;
            //if (listBG != null && listBG.Count > 0)
            //{
            if (listBG.Where(t => t.GD.Equals("三段") && t.GZ.Equals("开窑工")).Count() == 0)
            {
                var a1 = listBG.Sum(t => decimal.TryParse(t.HJ_SCQTS, out decimal scq) ? scq : 0M);
                var a2 = list_CQ.Sum(t => decimal.TryParse(t.DayTotal, out decimal d) ? d : 0m); /* decimal.TryParse(item_CQ.DayScq, out decimal dscq) ? dscq : 0M;*/
                var jbspb = new BaseDal<DataBaseGeneral_RZBZ_Month>().GetList(t => t.FactoryNo == "G003" && t.Dept.Contains("烧成") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();

                if (jbspb != null)
                {
                    a1 += jbspb.Sum(t => decimal.TryParse(t.BZTS, out decimal d) ? d : 0m);
                }
                if (a1 < a2)
                {
                    //是否少计工了！
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "烧成车间报工实出勤（熟练工+学徒+入职）小于人资出勤合计", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间实出勤（熟练工+学徒）+入职补助天数：{a1.ToString()},人资出勤合计：{a2.ToString()},请核对是否少报工！", MsgClass = "烧成报警", UserCode = (listBG.Count != 0 ? listBG.FirstOrDefault().CreateUser.Split('_')[0] : item_CQ.UserCode) }, true);
                }
                if (a1 > a2)
                {
                    gzd.IsCheckCQDayOk = false;
                    //记工多了，报警
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "烧成车间报工实出勤（熟练工+学徒+入职）【大于】人资出勤合计", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间实出勤（熟练工+学徒）+入职补助天数：{a1.ToString()},人资出勤合计：{a2.ToString()},报工天数大于实出勤天数！", MsgClass = "烧成报警", UserCode = (listBG.Count != 0 ? listBG.FirstOrDefault().CreateUser.Split('_')[0] : item_CQ.UserCode) }, true);
                }

                var a3 = listBG.Count > 0 ? listBG.Sum(t => decimal.TryParse(t.YCQTS, out decimal ycq1) ? ycq1 : 0M) : 0M;
                if (jbspb != null)
                {
                    a3 += jbspb.Sum(t => decimal.TryParse(t.YCQTS, out decimal d) ? d : 0m);
                }
                if (((listBG.Count > 0 ? listBG.Count : 0) + (jbspb.Count > 0 ? jbspb.Count : 0)) != 0)
                {
                    a3 = a3 / ((listBG.Count > 0 ? listBG.Count : 0) + (jbspb.Count > 0 ? jbspb.Count : 0));
                }
                else
                {
                    a3 = 0;
                }
                var a4 = list_CQ.Min(t => decimal.TryParse(t.DayYcq, out decimal ycq2) ? ycq2 : 0m);/* (decimal.TryParse(item_CQ.DayYcq, out decimal dycq) ? dycq : 0M);*/
                if (a3 != a4)
                {
                    //应出勤不相等，报警
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = "烧成车间报工应出勤【不等于】人资应出勤", Msg = $"工号：{item_CQ.UserCode}，姓名：{item_CQ.UserName}，车间应出勤天数：{a3.ToString()},人资应出勤：{item_CQ.DayYcq},应出勤【不等于】人资应出勤！", MsgClass = "烧成报警", UserCode = (listBG.Count != 0 ? listBG.FirstOrDefault().CreateUser.Split('_')[0] : item_CQ.UserCode) }, true);
                }
            }
            if (listBG.Count > 0)
            {
                gzd.GD = listBG[0].GD;
                gzd.Code = listBG[0].GH;
                gzd.Position = listBG[0].GZ;
                gzd.JiBenGZ = listBG.Max(t => decimal.TryParse(t.JBGZ, out decimal d) ? d : 0M).ToString();
                gzd.JBGZE = listBG.Sum(t => decimal.TryParse(t.JBGZE, out decimal d) ? d : 0M).ToString();
            }
            //1、报工一条记录，应出勤《=实出勤，并且基础数据库有全勤奖，那么这个人有全勤奖。
            //2、报工多条记录，检查这个人的报工每一条记录是否在基础数据中有全勤奖，
            //情况一如果有，那么实出勤 = 所有记录的实出勤的合计，应出勤 = 所有记录中最大。
            //情况二，一个一条记录没有全勤奖，那么没有全勤奖。

            var listQQj = new List<decimal>();
            var isqqj1 = false;
            if (listBG.Count > 0)
            {
                for (int i = 0; i < listBG.Count; i++)
                {
                    var tmp = listMonth.Where(t => t.PostName.Equals(listBG[i].GZ)).FirstOrDefault();
                    if (tmp != null)
                    {
                        if (!decimal.TryParse(tmp.MoneyQQJ, out decimal dqqj))
                        {
                            isqqj1 = false;
                            break;
                        }
                        isqqj1 = true;
                    }
                    else
                    {
                        isqqj1 = false;
                    }
                }
                if (isqqj1)
                {
                    if (listBG.Count == 1)
                    {
                        if ((decimal.TryParse(listBG[0].YCQTS, out decimal ycq) ? ycq : 0M) <= listBG.Sum(t => decimal.TryParse(t.HJ_SCQTS, out decimal d) ? d : 0M))
                        {
                            var tmp = listMonth.Where(t => t.PostName.Equals(gzd.Position)).FirstOrDefault();
                            if (tmp != null)
                            {
                                listQQj.Add(decimal.TryParse(tmp.MoneyQQJ, out decimal ddd1) ? ddd1 : 0m);
                            }
                            else
                            {
                                listQQj.Clear();
                            }
                        }
                        else { listQQj.Clear(); }
                    }
                    else
                    {
                        for (int i = 0; i < listBG.Count; i++)
                        {
                            if (listBG.Max(t => decimal.TryParse(t.YCQTS, out decimal d) ? d : 0M) <= listBG.Sum(t => decimal.TryParse(t.HJ_SCQTS, out decimal d) ? d : 0M))
                            {
                                var tmp = listMonth.Where(t => t.PostName.Equals(gzd.Position)).FirstOrDefault();
                                if (tmp != null)
                                {
                                    listQQj.Add(decimal.TryParse(tmp.MoneyQQJ, out decimal ddd1) ? ddd1 : 0m);
                                }
                                else
                                {
                                    listQQj.Clear();
                                }
                            }
                            else
                            {
                                listQQj.Clear();
                            }
                        }
                    }
                }
            }
            if (listQQj.Count > 0)
            {
                gzd.QQJ = listQQj.Min(t => t).ToString();
            }

            //if ((decimal.TryParse(listBG[0].YCQTS, out decimal ycq) ? ycq : 0M) <= listBG.Sum(t => decimal.TryParse(t.HJ_SCQTS, out decimal d) ? d : 0M))
            //{
            //    var tmp = listMonth.Where(t => t.PostName.Equals(gzd.Position)).FirstOrDefault();
            //    if (tmp != null)
            //    {
            //        gzd.QQJ = tmp.MoneyQQJ;
            //    }
            //}

            gzd.JJGZ1 = listBG.Sum(t => (decimal.TryParse(t.HSJJHKH, out decimal d) ? d : 0M) + (decimal.TryParse(t.BCJJHKH, out decimal d2) ? d2 : 0M)).ToString();
            gzd.JJGZ2 = listBG.Sum(t => decimal.TryParse(t.ZYJJHKH, out decimal d) ? d : 0M).ToString();
            gzd.JJGZ3 = listBG.Sum(t => decimal.TryParse(t.KYJJ, out decimal d) ? d : 0M).ToString();
            gzd.CLKHGZ = listBG.Sum(t => (decimal.TryParse(t.JSYKHGZ, out decimal d) ? d : 0M) + (decimal.TryParse(t.QZKH, out decimal d2) ? d2 : 0M)).ToString();
            gzd.ZLKHGZ = listBG.Sum(t => (decimal.TryParse(t.KYKH, out decimal d) ? d : 0M) + (decimal.TryParse(t.SYGX, out decimal d2) ? d2 : 0M)).ToString();
            gzd.GRKHGZ = listBG.Sum(t => (decimal.TryParse(t.TBGXSKH, out decimal d) ? d : 0M) + (decimal.TryParse(t.TBGXXKH, out decimal d2) ? d2 : 0M)).ToString();
            gzd.BZKHGZ = listBG.Sum(t => decimal.TryParse(t.XCGKH, out decimal d) ? d : 0M).ToString();
            //}

            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "烧成").ToList();
            if (fzyh.Count > 0)
            {
                gzd.PGYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //替班工资
            var tbgz = new BaseDal<DataBaseGeneral_JBSPB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "烧成车间").ToList();
            gzd.TBGZ = tbgz.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();

            var rzbz = new BaseDal<DataBaseGeneral_RZBZ_Month>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("烧成")).ToList();
            if (rzbz.Count > 0)
            {
                gzd.Position = gzd.Position ?? rzbz[0].GZ;
                gzd.RZBZ = rzbz.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept.Contains("烧成")).ToList();
            if (kfss.Count > 0)
            {
                gzd.KFSSBZ = kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m).ToString();
            }

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "sc" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G003") && t.Dept.Equals("烧成") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            if (pmcps.Count > 0)
            {
                gzd.PSFK = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m).ToString();
            }
            if (string.IsNullOrEmpty(gzd.Position))
            {
                gzd.Position = string.Empty;
            }
            return gzd;
        }

        /// <summary>
        /// 成型高压面具
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_GYMJ(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_GYMJ").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Remark.Contains("面具")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3CX_GYMJ", UserType = "3CX_GYMJ" };
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);
            decimal.TryParse(gzd.DaySCQ, out decimal scq);

            var rydy = new BaseDal<DataBase3CX_General_RYDY>().GetList(t => t.TheType.Contains("面具") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().OrderByDescending(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M).ToList();
            if (rydy != null && rydy.Count > 0)
            {
                gzd.Code = rydy.FirstOrDefault().GH;
                gzd.ZLKHGZ = rydy.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();

                var mycq = rydy.Average(t => decimal.TryParse(t.MQTS, out decimal d) ? d : 0M);
                if (ycq != mycq)
                {
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压面具人员对应与出勤【应出勤】不一致", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压面具人员对应与出勤【应出勤】不一致,工号:{gzd.UserCode}，姓名:{gzd.UserName}，考勤【应出勤】：{gzd.DayYCQ}，与人员对应满勤天数：{mycq}不相等！", MsgClass = "高压面具报警", UserCode = rydy.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
                var mscq = rydy.Sum(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M);
                if (scq != mscq)
                {
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压面具人员对应与出勤【实出勤】不一致", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压面具人员对应与出勤【实出勤】不一致,工号:{gzd.UserCode}，姓名:{gzd.UserName}，考勤【实出勤】：{gzd.DaySCQ}，与人员对应出勤天数：{mscq}不相等！", MsgClass = "高压面具报警", UserCode = rydy.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
            }
            gzd.Position = item_CQ.Remark;
            var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo.Equals("G003") && t.WorkshopName.Contains(item_CQ.Remark));
            if (dataMonth != null)
            {
                gzd.JiBenGZ = dataMonth.MoneyBase;
                gzd.JiNengGZ = dataMonth.MoneyJN;
                gzd.JiaBanGZ = dataMonth.MoneyJB;
            }
            decimal.TryParse(gzd.JiBenGZ, out decimal jbgz_base);
            gzd.JBGZE = (jbgz_base * scq / ycq).ToString();
            decimal.TryParse(gzd.JiNengGZ, out decimal jngz);
            gzd.JNGZE = (jngz * scq / ycq).ToString();
            decimal.TryParse(gzd.JiaBanGZ, out decimal jbgz_jb);
            gzd.JBGZE2 = (jbgz_jb * scq / ycq).ToString();
            var jb = new BaseDal<DataBase3CX_General_JB>().Get(t => t.TheType.Contains("面具") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode);
            if (jb != null)
            {
                gzd.TBGZ = jb.Money;
            }
            var bz = new BaseDal<DataBase3CX_GYMJ_07_BZFGY>().Get(t => t.TheType.Contains("面具") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode);
            if (bz != null)
            {
                decimal.TryParse(bz.BFZ, out decimal bzf);
                gzd.BZF = (bzf * scq / ycq).ToString();
            }

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "高压面具" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        /// <summary>
        /// 成型水箱
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_GYSX(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_GYSX").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Remark.Contains("水箱")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3CX_GYSX", UserType = "3CX_GYSX" };
            decimal.TryParse(gzd.DaySCQ, out decimal scq);
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);

            var rydy = new BaseDal<DataBase3CX_General_RYDY>().GetList(t => t.TheType.Contains("水箱") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList().OrderByDescending(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M).ToList();
            if (rydy != null && rydy.Count > 0)
            {
                gzd.Code = rydy.FirstOrDefault().GH;
                gzd.JJGZ1 = rydy.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();

                var mycq = rydy.Sum(t => decimal.TryParse(t.MQTS, out decimal d) ? d : 0M);
                if (ycq != mycq)
                {
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压水箱人员对应与出勤【应出勤】不一致", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压水箱人员对应与出勤【应出勤】不一致,工号:{gzd.UserCode}，姓名:{gzd.UserName}，考勤【应出勤】：{gzd.DayYCQ}，与人员对应应出勤天数：{mycq}不相等！", MsgClass = "高压水箱报警", UserCode = rydy.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
                var mscq = rydy.Sum(t => decimal.TryParse(t.CQTS, out decimal d) ? d : 0M);
                if (scq != mscq)
                {
                    gzd.IsCheckCQDayOk = false;
                    BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压水箱人员对应与出勤【实出勤】不一致", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，高压水箱人员对应与出勤【实出勤】不一致,工号:{gzd.UserCode}，姓名:{gzd.UserName}，考勤【实出勤】：{gzd.DaySCQ}，与人员对应应出勤天数：{mscq}不相等！", MsgClass = "高压水箱报警", UserCode = rydy.FirstOrDefault().CreateUser.Split('_')[0] }, true);
                }
            }
            gzd.Position = item_CQ.Remark;
            //var gysx = new BaseDal<DataBase3CX_GYSX_02_GYSX>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.GH1 == gzd.Code);
            //if (gysx != null)
            //{
            //    decimal.TryParse(gysx.DRGZ, out decimal drgz);
            //    gzd.JJGZ1 = (drgz * scq / ycq).ToString();
            //}

            var bz = new BaseDal<DataBase3CX_GYMJ_07_BZFGY>().Get(t => t.TheType.Contains("水箱") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode);
            if (bz != null)
            {
                decimal.TryParse(bz.BFZ, out decimal bzf);
                gzd.BZF = (bzf * scq / ycq).ToString();
            }

            var jb = new BaseDal<DataBase3CX_General_JB>().Get(t => t.TheType.Contains("水箱") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode);
            if (jb != null)
            {
                gzd.TBGZ = jb.Money;
            }
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "高压水箱" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        /// <summary>
        /// 成型大套
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_GYDT(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_GYDT").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Remark.Contains("大套")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3CX_GYDT", UserType = "3CX_GYDT" };
            decimal.TryParse(gzd.DaySCQ, out decimal scq);
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);
            var rydy = new BaseDal<DataBase3CX_General_RYDY>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode);
            if (rydy != null)
            {
                gzd.Code = rydy.GH;
            }
            gzd.Position = item_CQ.Remark;

            var dataMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo.Equals("G003") && t.WorkshopName.Contains(item_CQ.Remark));
            if (dataMonth != null)
            {
                gzd.JiBenGZ = dataMonth.MoneyBase;
                if (scq >= ycq)
                {
                    gzd.QQJ = dataMonth.MoneyQQJ;
                }
            }
            decimal.TryParse(gzd.JiBenGZ, out decimal jbgz_base);
            gzd.JBGZE = (jbgz_base * scq / ycq).ToString();

            var gysx = new BaseDal<DataBase3CX_GYSX_02_GYSX>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.GH1 == gzd.Code);
            if (gysx != null)
            {
                decimal.TryParse(gysx.DRGZ, out decimal drgz);
                gzd.JJGZ1 = (drgz * scq / ycq).ToString();
            }

            var jbspb = new BaseDal<DataBaseGeneral_JBSPB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G003" && t.Dept == "成型车间高压大套").ToList();
            gzd.TBGZ = jbspb.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();

            var jb = new BaseDal<DataBase3CX_General_JB>().Get(t => t.TheType.Contains("水箱") && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == gzd.UserCode);
            if (jb != null)
            {
                gzd.TBGZ = jb.Money;
            }

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "高压大套" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        /// <summary>
        /// 半检拉坯
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_BJLP(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_BJLP").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Position.Contains("成型拉坯工")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayScq*/, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString() /*item_CQ.DayTxj*/, DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString() /*item_CQ.DayYcq*/, CalculationType = "3CX_BJLP", UserType = "3CX_BJLP" };
            gzd.Position = item_CQ.Position;
            decimal.TryParse(gzd.DaySCQ, out decimal scq);
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);
            var lpyb = new BaseDal<DataBase3CX_BJLP_01_LPYB>().Get(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode);
            if (lpyb != null)
            {
                gzd.Code = lpyb.GH;
                gzd.JJGZ1 = lpyb.JE;
            }

            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "半检拉坯" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = cjjc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G003") && t.Dept.Equals("半检拉坯") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            if (pmcps.Count > 0)
            {
                gzd.PSFK = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m).ToString();
            }

            return gzd;
        }

        /// <summary>
        /// 成型技术员
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_JSY(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_JSY").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Position.Contains("技术员")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, Position = item_CQ.Position, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString(), DayJBCQ = list_CQ.Sum(t => decimal.TryParse(t.DayJbjx, out decimal d) ? d : 0M).ToString(), DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString(), DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString(), CalculationType = "3CX_JSY", UserType = "3CX_JSY" };

            decimal.TryParse(gzd.DaySCQ, out decimal scq);
            decimal.TryParse(gzd.DayJBCQ, out decimal jbcq);
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);
            var itemkh = new BaseDal<DataBase3CX_JSY_01CXJSYKH_02KH>().Get(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode);
            if (itemkh != null)
            {
                gzd.GD = itemkh.GD;
                decimal.TryParse(itemkh.HJ, out decimal hj);
                gzd.GRKHGZ = (hj * scq / ycq).ToString();
            }
            else
            {
                var itemMonth_TMP = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName.Contains("技术员") && t.ProductType == gzd.UserCode && t.Classification.Contains("面具"));
                if (itemMonth_TMP != null)
                {
                    var itemmjkh = new BaseDal<DataBase3CX_JSY_02MJJSYKH>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth).ToList().Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M);
                    gzd.GRKHGZ = (itemmjkh * scq / ycq).ToString();
                }
            }
            var itemMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName.Contains("技术员") && t.ProductType == gzd.UserCode);
            if (itemMonth != null)
            {
                gzd.JiBenGZ = itemMonth.MoneyBase;
                decimal.TryParse(gzd.JiBenGZ, out decimal jbgz);
                gzd.JBGZE = (jbgz * scq / ycq).ToString();
                gzd.JBGZE1 = (jbgz * jbcq / ycq).ToString();
            }

            var jc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "成型技术员" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = jc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = jc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            return gzd;
        }

        /// <summary>
        /// 成型其他
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private GZD Calculation3CX_CX(List<GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "3CX_CX").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && !t.Remark.Contains("面具") && !t.Remark.Contains("水箱") && !t.Remark.Contains("大套") && !t.Position.Contains("技术员")).ToList();

            GZD gzd = new GZD { Id = Guid.NewGuid(), IsCheckCQDayOk = true, CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, Position = item_CQ.Position, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, DayTxj = list_CQ.Sum(t => decimal.TryParse(t.DayTxj, out decimal d) ? d : 0m).ToString(), DaySCQ = list_CQ.Sum(t => decimal.TryParse(t.DayScq, out decimal d) ? d : 0m).ToString(), DayYCQ = list_CQ.Average(t => decimal.TryParse(t.DayYcq, out decimal d) ? d : 0m).ToString(), CalculationType = "3CX_CX", UserType = "3CX_CX" };

            decimal.TryParse(gzd.DaySCQ, out decimal scq);
            decimal.TryParse(gzd.DayYCQ, out decimal ycq);

            //计算工段
            var item03 = new BaseDal<DataBase3CX_CX_03JJ>().GetList(t => t.UserCode == gzd.UserCode && !string.IsNullOrEmpty(t.GD)).FirstOrDefault();
            if (item03 != null)
            {
                gzd.GD = item03.GD;
            }
            else
            {
                var item05 = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.UserCode == gzd.UserCode && !string.IsNullOrEmpty(t.GD)).FirstOrDefault();
                if (item05 != null)
                {
                    gzd.GD = item05.GD;
                }
            }

            //计算短工号
            var itemYb = new BaseDal<DataBase3CX_General_CXYB>().GetList(t => t.UserCode == gzd.UserCode && !string.IsNullOrEmpty(t.GH) && t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth).FirstOrDefault();
            if (itemYb != null)
            {
                gzd.Code = itemYb.GH;
            }
            var itemMonth = new BaseDal<DataBaseMonth>().Get(t => t.FactoryNo == "G003" && t.WorkshopName == "成型车间" && t.PostName == gzd.Position);
            if (itemMonth != null)
            {
                gzd.JiBenGZ = itemMonth.MoneyBase;
                decimal.TryParse(gzd.JiBenGZ, out decimal jbgz);
                gzd.JBGZE = (jbgz * scq / ycq).ToString();

                gzd.JiNengGZ = itemMonth.MoneyJN;
                decimal.TryParse(gzd.JiNengGZ, out decimal jngz);
                gzd.JNGZE = (jngz * scq / ycq).ToString();
            }

            var listSFBZ = new BaseDal<DataBase3CX_CX_12SFBZ>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.SFCode == gzd.UserCode).ToList();
            gzd.SFJJ = listSFBZ.Sum(t => decimal.TryParse(t.SFBZ, out decimal d) ? d : 0M).ToString();
            if (!string.IsNullOrEmpty(gzd.Code))
            {
                var list0303 = new BaseDal<DataBase3CX_CX_03JJ_GR>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.GH == gzd.Code).ToList();
                gzd.JJGZ1 = list0303.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString();
            }

            var listZXJ = new BaseDal<DataBase3CX_CX_06ZXJ>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.JJGZ2 = listZXJ.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString();

            var listRZBZ = new BaseDal<DataBase3CX_CX_05PXBZ>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.RZBZ = listRZBZ.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0m).ToString();

            var listBCBZ = new BaseDal<DataBase3CX_CX_04SXBZ>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.BCBZ = listBCBZ.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();

            var listSS = new BaseDal<DataBase3CX_CX_09SS>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.KFSSBZ = listSS.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString();

            var jc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "成型" && t.TheType == "workshop" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.JL = jc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
            //车间奖惩=车间奖惩+打卡扣款
            decimal kk = jc.Sum(t => (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)));
            if (!string.IsNullOrEmpty(item_CQ.CountCdzt))
            {
                var baseM = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "迟到早退罚款");
                kk += Convert.ToDecimal(baseM.MoneyBase) * Convert.ToDecimal(item_CQ.CountCdzt);
            }
            gzd.CJ = kk.ToString();

            var flfk = new BaseDal<DataBase3CX_CX_07FLFK>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            gzd.CXFLJF = flfk.Sum(t => decimal.TryParse(t.FLFK, out decimal d) ? d : 0M).ToString();

            if (!string.IsNullOrEmpty(gzd.Code))
            {
                var psbz = new BaseDal<DataBase3CX_CX_11PSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.GH == gzd.Code).ToList();
                gzd.PSBZ = psbz.Sum(t => decimal.TryParse(t.HJJE, out decimal d) ? d : 0M).ToString();
            }

            var psfk = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.FactoryNo == "G003" && t.Dept == "成型" && t.UserCode == item_CQ.UserCode).ToList();
            gzd.PSFK = psfk.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m).ToString();
            return gzd;
        }

        /// <summary>
        /// 工资单公共部分，最后计算
        /// </summary>
        /// <param name="gzd"></param>
        private GZD Calculation3Pub(List<GZD> listGzd, List<DataBaseGeneral_CQ> listCQ)
        {
            if (listGzd == null || listGzd.Count == 0)
            {
                return null;
            }
            string userCode = listGzd[0].UserCode;
            string userName = listGzd[0].UserName;
            int theYear = listGzd[0].TheYear;
            int theMonth = listGzd[0].TheMonth;

            GZD gzd = new GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = theYear, TheMonth = theMonth, IsCheckCQDayOk = listGzd.OrderBy(t => t.IsCheckCQDayOk).LastOrDefault().IsCheckCQDayOk, Factory = listGzd.OrderBy(t => decimal.TryParse(t.DaySCQ, out decimal d) ? d : 0M).LastOrDefault().Factory, Dept = listGzd.OrderBy(t => decimal.TryParse(t.DaySCQ, out decimal d) ? d : 0M).LastOrDefault().Dept, Position = listGzd.OrderBy(t => decimal.TryParse(t.DaySCQ, out decimal d) ? d : 0M).LastOrDefault().Position, GD = listGzd.OrderBy(t => decimal.TryParse(t.DaySCQ, out decimal d) ? d : 0M).LastOrDefault().GD, UserCode = userCode, UserName = userName, Code = listGzd.OrderBy(t => decimal.TryParse(t.DaySCQ, out decimal d) ? d : 0M).LastOrDefault().Code, JiBenGZ = listGzd.Sum(t => decimal.TryParse(t.JiBenGZ, out decimal d) ? d : 0M).ToString(), CXFLJF = listGzd.Sum(t => decimal.TryParse(t.CXFLJF, out decimal d) ? d : 0m).ToString(), SFJJ = listGzd.Sum(t => decimal.TryParse(t.SFJJ, out decimal d) ? d : 0m).ToString(), BCBZ = listGzd.Sum(t => decimal.TryParse(t.BCBZ, out decimal d) ? d : 0m).ToString(), JBGZE = listGzd.Sum(t => decimal.TryParse(t.JBGZE, out decimal jbgze) ? jbgze : 0m).ToString(), JNGZE = listGzd.Sum(t => decimal.TryParse(t.JNGZE, out decimal jbgze) ? jbgze : 0m).ToString(), JBGZE1 = listGzd.Sum(t => decimal.TryParse(t.JBGZE1, out decimal jbgze) ? jbgze : 0m).ToString(), JBGZE2 = listGzd.Sum(t => decimal.TryParse(t.JBGZE2, out decimal jbgze2) ? jbgze2 : 0m).ToString(), JJGZ1 = listGzd.Sum(t => decimal.TryParse(t.JJGZ1, out decimal jjgz1) ? jjgz1 : 0M).ToString(), JJGZ2 = listGzd.Sum(t => decimal.TryParse(t.JJGZ2, out decimal jjgz2) ? jjgz2 : 0M).ToString(), JJGZ3 = listGzd.Sum(t => decimal.TryParse(t.JJGZ3, out decimal jjgz3) ? jjgz3 : 0M).ToString(), CLKHGZ = listGzd.Sum(t => decimal.TryParse(t.CLKHGZ, out decimal jjgz3) ? jjgz3 : 0M).ToString(), ZLKHGZ = listGzd.Sum(t => decimal.TryParse(t.ZLKHGZ, out decimal jjgz3) ? jjgz3 : 0M).ToString(), GRKHGZ = listGzd.Sum(t => decimal.TryParse(t.GRKHGZ, out decimal grkhgz) ? grkhgz : 0M).ToString(), BZKHGZ = listGzd.Sum(t => decimal.TryParse(t.BZKHGZ, out decimal jjgz3) ? jjgz3 : 0M).ToString(), KFSSBZ = listGzd.Sum(t => decimal.TryParse(t.KFSSBZ, out decimal jjgz3) ? jjgz3 : 0M).ToString(), TBGZ = listGzd.Sum(t => decimal.TryParse(t.TBGZ, out decimal tbgz) ? tbgz : 0M).ToString(), RZBZ = listGzd.Sum(t => decimal.TryParse(t.RZBZ, out decimal rzbz) ? rzbz : 0M).ToString(), PGYH = listGzd.Sum(t => decimal.TryParse(t.PGYH, out decimal pgyh) ? pgyh : 0M).ToString(), BZF = listGzd.Sum(t => decimal.TryParse(t.BZF, out decimal bzf) ? bzf : 0M).ToString(), JL = listGzd.Sum(t => decimal.TryParse(t.JL, out decimal jl) ? jl : 0M).ToString(), CJ = listGzd.Sum(t => decimal.TryParse(t.CJ, out decimal cj) ? cj : 0M).ToString(), QQJ = listGzd.Sum(t => decimal.TryParse(t.QQJ, out decimal qqj) ? qqj : 0m).ToString(), PSFK = listGzd.Sum(t => decimal.TryParse(t.PSFK, out decimal cj) ? cj : 0M).ToString(), PSBZ = listGzd.Sum(t => decimal.TryParse(t.PSBZ, out decimal psbz) ? psbz : 0M).ToString() };
            gzd.DayTxj = listCQ.Sum(t => string.IsNullOrEmpty(t.DayTxj) ? 0M : Convert.ToDecimal(t.DayTxj)).ToString();
            gzd.DayJBCQ = listCQ.Sum(t => string.IsNullOrEmpty(t.DayDx) ? 0M : Convert.ToDecimal(t.DayDx)).ToString();
            decimal scq = listCQ.Sum(t => string.IsNullOrEmpty(t.DayScq) ? 0M : Convert.ToDecimal(t.DayScq));
            gzd.DaySCQ = scq.ToString();

            decimal ycq = listCQ.Average(t => string.IsNullOrEmpty(t.DayYcq) ? 0M : Convert.ToDecimal(t.DayYcq));
            gzd.DayYCQ = ycq.ToString();
            var gl = new BaseDal<DataBaseGeneral_GL>().Get(t => t.UserCode == userCode && t.TheYear == theYear);
            if (gl != null)
            {
                decimal glgz = decimal.TryParse(gl.Money, out decimal d_gl) ? d_gl : 0M;  //Convert.ToDecimal(gl.Money);
                gzd.GLGZ = glgz.ToString();
                //if (scq > 10M || (scq <= 10M && scq >= 6.67M && ycq < 23M))//20180117修改不用了。
                if (scq >= 5M)
                {
                    //都计算工龄工资和工龄额
                    foreach (var item in listGzd)
                    {
                        item.GLGZ = gl.Money;
                        if (ycq > 0m)
                        {
                            decimal excel_q = decimal.TryParse(item.GLGZ, out decimal d_glgz) ? d_glgz : 0M; // Convert.ToDecimal(item.GLGZ);
                            decimal excel_l = decimal.TryParse(item.DaySCQ, out decimal d_dayscq) ? d_dayscq : 0M; //string.IsNullOrEmpty(item.DaySCQ) ? 0M : Convert.ToDecimal(item.DaySCQ);
                            decimal excel_k = decimal.TryParse(item.DayJBCQ, out decimal d_dayjbcq) ? d_dayjbcq : 0M;// string.IsNullOrEmpty(item.DayJBCQ) ? 0M : Convert.ToDecimal(item.DayJBCQ);
                            decimal excel_j = decimal.TryParse(item.DayTxj, out decimal d_daytxj) ? d_daytxj : 0M;//string.IsNullOrEmpty(item.DayTxj) ? 0M : Convert.ToDecimal(item.DayTxj);
                            decimal excel_m = decimal.TryParse(item.DayYCQ, out decimal d_dayycq) ? d_dayycq : 0M;//string.IsNullOrEmpty(item.DayYCQ) ? 0M : Convert.ToDecimal(item.DayYCQ);
                            if (excel_l + excel_k + excel_j >= excel_m)
                            {
                                item.GLE = item.GLGZ;
                            }
                            else
                            {
                                //if ((excel_m >= 24m && excel_l + excel_k + excel_j > 10m) || (excel_m >= 14m && excel_m <= 23m && (excel_l + excel_k + excel_j) >= 6.7m))
                                {
                                    item.GLE = (excel_q / excel_m * (excel_l + excel_k + excel_j)).ToString();
                                }
                            }
                        }
                    }
                    decimal gle = listGzd.Sum(t => string.IsNullOrEmpty(t.GLE) ? 0M : Convert.ToDecimal(t.GLE));
                    gzd.GLE = gle > glgz ? glgz.ToString() : gle.ToString();
                }
                else
                {
                    gzd.GLE = 0.ToString();
                }
            }
            var bfgz = new BaseDal<DataBaseGeneral_BF>().Get(t => t.TheYear == theYear && t.TheMonth == theMonth && t.UserCode == userCode);
            if (bfgz != null)
            {
                gzd.BFGZ = bfgz.Money;
            }
            var jtbz = new BaseDal<DataBaseGeneral_JT>().GetList(t => t.TheYear == theYear && t.TheMonth == theMonth && t.UserCode == userCode).ToList();
            if (jtbz.Count > 0)
            {
                decimal.TryParse(jtbz[0].Money, out decimal jtbzBase);
                var jtNow = jtbz.Sum(t => Convert.ToDecimal(t.Money)) / Convert.ToDecimal(gzd.DayYCQ) * Convert.ToDecimal(gzd.DaySCQ);
                if (jtNow > jtbzBase)
                {
                    gzd.JTBZ = jtbzBase.ToString();
                }
                else
                {
                    gzd.JTBZ = jtNow.ToString();
                }
            }

            var zfbz = new BaseDal<DataBaseGeneral_ZF>().GetList(t => t.TheYear == theYear && t.TheMonth == theMonth && t.UserCode == userCode).ToList();
            if (zfbz.Count > 0 && Convert.ToDecimal(gzd.DaySCQ) >= Convert.ToDecimal(gzd.DayYCQ) / 2)
            {
                decimal.TryParse(zfbz[0].Money, out decimal zfbzBase);
                var zfbzNow = zfbz.Sum(t => Convert.ToDecimal(t.Money)) / Convert.ToDecimal(gzd.DayYCQ) * Convert.ToDecimal(gzd.DaySCQ);
                if (zfbzNow > zfbzBase)
                {
                    gzd.ZFBZ = zfbzBase.ToString();
                }
                else
                {
                    gzd.ZFBZ = zfbzNow.ToString();
                }
            }

            var yey_F = 0m;
            var yey_M = 0M;
            var yey = new BaseDal<DataBaseGeneral_YEY>().Get(t => t.TheYear == theYear && t.TheMonth == theMonth && t.FUserCode == userCode);
            if (yey != null)
            {
                var cq_m = new BaseDal<DataBaseGeneral_CQ>().Get(t => t.TheYear == theYear && t.TheMonth == theMonth && t.UserCode == yey.MUserCode);
                if ((string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDouble(gzd.DaySCQ)) >= (string.IsNullOrEmpty(gzd.DayYCQ) ? 0 : Convert.ToDouble(gzd.DayYCQ)) / 2 && (string.IsNullOrEmpty(cq_m.DayScq) ? 0 : Convert.ToDouble(cq_m.DayScq)) >= (string.IsNullOrEmpty(cq_m.DayYcq) ? 0 : Convert.ToDouble(cq_m.DayYcq)) / 2)
                {
                    //if ((string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDouble(gzd.DaySCQ)) >= (string.IsNullOrEmpty(cq_m.DayScq) ? 0 : Convert.ToDouble(cq_m.DayScq)))
                    //{
                    //算在父亲上
                    yey_F = (string.IsNullOrEmpty(yey.Money) ? 0m : Convert.ToDecimal(yey.Money)) / (string.IsNullOrEmpty(gzd.DayYCQ) ? 0M : Convert.ToDecimal(gzd.DayYCQ)) * (string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDecimal(gzd.DaySCQ));
                    yey_M = (string.IsNullOrEmpty(yey.Money) ? 0m : Convert.ToDecimal(yey.Money)) / (string.IsNullOrEmpty(cq_m.DayYcq) ? 0M : Convert.ToDecimal(cq_m.DayYcq)) * (string.IsNullOrEmpty(cq_m.DayScq) ? 0 : Convert.ToDecimal(cq_m.DayScq));

                    if (yey_F > yey_M)
                    {
                        gzd.YEYBZ = yey_F.ToString();
                    }

                    //}
                }
            }
            else
            {
                yey = new BaseDal<DataBaseGeneral_YEY>().Get(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.MUserCode == gzd.UserCode);
                if (yey != null)
                {
                    var cq_f = new BaseDal<DataBaseGeneral_CQ>().Get(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == yey.FUserCode);
                    if (cq_f != null)
                    {
                        if ((string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDouble(gzd.DaySCQ)) >= (string.IsNullOrEmpty(gzd.DayYCQ) ? 0 : Convert.ToDouble(gzd.DayYCQ)) / 2 && (string.IsNullOrEmpty(cq_f.DayScq) ? 0 : Convert.ToDouble(cq_f.DayScq)) >= (string.IsNullOrEmpty(cq_f.DayYcq) ? 0 : Convert.ToDouble(cq_f.DayYcq)) / 2)
                        {
                            //if ((string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDouble(gzd.DaySCQ)) > (string.IsNullOrEmpty(cq_f.DayScq) ? 0 : Convert.ToDouble(cq_f.DayScq)))
                            //{
                            //算在母亲上
                            yey_M = (string.IsNullOrEmpty(yey.Money) ? 0m : Convert.ToDecimal(yey.Money)) / (string.IsNullOrEmpty(gzd.DayYCQ) ? 0M : Convert.ToDecimal(gzd.DayYCQ)) * (string.IsNullOrEmpty(gzd.DaySCQ) ? 0 : Convert.ToDecimal(gzd.DaySCQ));
                            yey_F = (string.IsNullOrEmpty(yey.Money) ? 0m : Convert.ToDecimal(yey.Money)) / (string.IsNullOrEmpty(cq_f.DayYcq) ? 0M : Convert.ToDecimal(cq_f.DayYcq)) * (string.IsNullOrEmpty(cq_f.DayScq) ? 0 : Convert.ToDecimal(cq_f.DayScq));
                            if (yey_M >= yey_F)
                            {
                                gzd.YEYBZ = yey_M.ToString();
                            }

                            //}
                        }
                    }
                }
            }

            var tjf = new BaseDal<DataBaseGeneral_TJF>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.TJFBX = tjf.Sum(t => Convert.ToDecimal(t.Money)).ToString();

            var jsf = new BaseDal<DataBaseGeneral_JSF>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.Js_UserCode == gzd.UserCode).ToList();
            gzd.JSF = jsf.Sum(t => Convert.ToDecimal(t.Jsf)).ToString();

            var cp = new BaseDal<DataBaseGeneral_CP>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.CPBX = cp.Sum(t => Convert.ToDecimal(t.Money)).ToString();

            var wx = new BaseDal<DataBaseGeneral_WX>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.WXBZ = wx.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString();

            var cjfk = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "jb" && t.TheType == "Cj" && t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.CJFK = cjfk.Sum(t => (string.IsNullOrEmpty(t.J) ? 0m : Convert.ToDecimal(t.J)) - (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C))).ToString();

            var pgfk = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.FactoryNo == "3" && t.Dept == "jb" && t.TheType == "Pg" && t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.PGFK = pgfk.Sum(t => (string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)) - (string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C))).ToString();

            var gzzrx = new BaseDal<DataBaseGeneral_GZZRX>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.GZZRXKK = gzzrx.Sum(t => Convert.ToDecimal(decimal.TryParse(t.MoneyPlus, out decimal moneyPlus) ? moneyPlus : 0M) - Convert.ToDecimal(decimal.TryParse(t.MoneyMinus, out decimal moneyMinus) ? moneyMinus : 0M)).ToString();

            var bx = new BaseDal<DataBaseGeneral_BX>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.DKGJJ = bx.Sum(t => string.IsNullOrEmpty(t.GJJ) ? 0M : Convert.ToDecimal(t.GJJ)).ToString();
            gzd.DKBXF = bx.Sum(t => string.IsNullOrEmpty(t.BX) ? 0M : Convert.ToDecimal(t.BX)).ToString();
            string bx_DW = bx.Sum(t => string.IsNullOrEmpty(t.BX_DW) ? 0M : Convert.ToDecimal(t.BX_DW)).ToString();
            string gjj_DW = bx.Sum(t => string.IsNullOrEmpty(t.GJJ_DW) ? 0M : Convert.ToDecimal(t.GJJ_DW)).ToString();

            var dk_lz = new BaseDal<DataBaseGeneral_LZ>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.DK_LZKK = dk_lz.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString();

            var jc_gs = new BaseDal<DataBaseGeneral_JC_Factory>().GetList(t => t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth && t.UserCode == gzd.UserCode).ToList();
            gzd.GSJL = jc_gs.Where(t => !t.Money.Contains("-")).Sum(t => Convert.ToDecimal(t.Money)).ToString();
            string gscj = jc_gs.Where(t => t.Money.Contains("-")).Sum(t => Convert.ToDecimal(t.Money)).ToString();
            gzd.GSCJ = ((decimal.TryParse(gscj, out decimal d_gscj) ? d_gscj : 0M) - (decimal.TryParse(bx_DW, out decimal d_bx_dw) ? d_bx_dw : 0M) - (decimal.TryParse(gjj_DW, out decimal d_gjj_DW) ? d_gjj_DW : 0M)).ToString();

            var tmp_Yfhj = (string.IsNullOrEmpty(gzd.JBGZE) ? 0m : Convert.ToDecimal(gzd.JBGZE)) +
                (string.IsNullOrEmpty(gzd.GLE) ? 0m : Convert.ToDecimal(gzd.GLE)) +
                (string.IsNullOrEmpty(gzd.JNGZE) ? 0m : Convert.ToDecimal(gzd.JNGZE)) +
                (string.IsNullOrEmpty(gzd.JBGZE1) ? 0m : Convert.ToDecimal(gzd.JBGZE1)) +
                (string.IsNullOrEmpty(gzd.JBGZE2) ? 0m : Convert.ToDecimal(gzd.JBGZE2)) +
                (string.IsNullOrEmpty(gzd.QQJ) ? 0m : Convert.ToDecimal(gzd.QQJ)) +
                (string.IsNullOrEmpty(gzd.RG) ? 0m : Convert.ToDecimal(gzd.RG)) +
                (string.IsNullOrEmpty(gzd.TXJBZ) ? 0m : Convert.ToDecimal(gzd.TXJBZ)) +
                (string.IsNullOrEmpty(gzd.BFGZ) ? 0m : Convert.ToDecimal(gzd.BFGZ)) +
                (string.IsNullOrEmpty(gzd.SFJJ) ? 0m : Convert.ToDecimal(gzd.SFJJ)) +
                (string.IsNullOrEmpty(gzd.JJGZ1) ? 0m : Convert.ToDecimal(gzd.JJGZ1)) +
                (string.IsNullOrEmpty(gzd.JJGZ2) ? 0m : Convert.ToDecimal(gzd.JJGZ2)) +
                (string.IsNullOrEmpty(gzd.JJGZ3) ? 0m : Convert.ToDecimal(gzd.JJGZ3)) +
                (string.IsNullOrEmpty(gzd.CLKHGZ) ? 0m : Convert.ToDecimal(gzd.CLKHGZ)) +
                (string.IsNullOrEmpty(gzd.ZLKHGZ) ? 0m : Convert.ToDecimal(gzd.ZLKHGZ)) +
                (string.IsNullOrEmpty(gzd.GRKHGZ) ? 0m : Convert.ToDecimal(gzd.GRKHGZ)) +
                (string.IsNullOrEmpty(gzd.BZKHGZ) ? 0m : Convert.ToDecimal(gzd.BZKHGZ)) +
                (string.IsNullOrEmpty(gzd.PGYH) ? 0m : Convert.ToDecimal(gzd.PGYH)) +
                (string.IsNullOrEmpty(gzd.TBGZ) ? 0m : Convert.ToDecimal(gzd.TBGZ)) +
                (string.IsNullOrEmpty(gzd.BZF) ? 0m : Convert.ToDecimal(gzd.BZF)) +
                (string.IsNullOrEmpty(gzd.RZBZ) ? 0m : Convert.ToDecimal(gzd.RZBZ)) +
                (string.IsNullOrEmpty(gzd.BCBZ) ? 0m : Convert.ToDecimal(gzd.BCBZ)) +
                (string.IsNullOrEmpty(gzd.FPJF) ? 0m : Convert.ToDecimal(gzd.FPJF)) +
                (string.IsNullOrEmpty(gzd.KFSSBZ) ? 0m : Convert.ToDecimal(gzd.KFSSBZ)) +
                (string.IsNullOrEmpty(gzd.GCSSBZ) ? 0m : Convert.ToDecimal(gzd.GCSSBZ)) +
                (string.IsNullOrEmpty(gzd.TMBZ) ? 0m : Convert.ToDecimal(gzd.TMBZ)) +
                (string.IsNullOrEmpty(gzd.JJBT) ? 0m : Convert.ToDecimal(gzd.JJBT)) +
                (string.IsNullOrEmpty(gzd.JTBZ) ? 0m : Convert.ToDecimal(gzd.JTBZ)) +
                (string.IsNullOrEmpty(gzd.ZFBZ) ? 0m : Convert.ToDecimal(gzd.ZFBZ)) +
                (string.IsNullOrEmpty(gzd.YEYBZ) ? 0m : Convert.ToDecimal(gzd.YEYBZ)) +
                (string.IsNullOrEmpty(gzd.DHBT) ? 0m : Convert.ToDecimal(gzd.DHBT)) +
                (string.IsNullOrEmpty(gzd.TJFBX) ? 0m : Convert.ToDecimal(gzd.TJFBX)) +
                (string.IsNullOrEmpty(gzd.JSF) ? 0m : Convert.ToDecimal(gzd.JSF)) +
                (string.IsNullOrEmpty(gzd.CPBX) ? 0m : Convert.ToDecimal(gzd.CPBX)) +
                (string.IsNullOrEmpty(gzd.WXBZ) ? 0m : Convert.ToDecimal(gzd.WXBZ)) +
                (string.IsNullOrEmpty(gzd.JL) ? 0m : Convert.ToDecimal(gzd.JL)) -
                (string.IsNullOrEmpty(gzd.CJ) ? 0m : Convert.ToDecimal(gzd.CJ)) +
                (string.IsNullOrEmpty(gzd.CXFLJF) ? 0m : Convert.ToDecimal(gzd.CXFLJF)) +
                (string.IsNullOrEmpty(gzd.PSBZ) ? 0m : Convert.ToDecimal(gzd.PSBZ)) -
                (string.IsNullOrEmpty(gzd.PSFK) ? 0m : Convert.ToDecimal(gzd.PSFK)) +
                (string.IsNullOrEmpty(gzd.CJFK) ? 0m : Convert.ToDecimal(gzd.CJFK)) +
                (string.IsNullOrEmpty(gzd.PGFK) ? 0m : Convert.ToDecimal(gzd.PGFK)) +
                (string.IsNullOrEmpty(gzd.GZZRXKK) ? 0m : Convert.ToDecimal(gzd.GZZRXKK)) -
                (string.IsNullOrEmpty(gzd.DK_LZKK) ? 0m : Convert.ToDecimal(gzd.DK_LZKK)) +
                (string.IsNullOrEmpty(gzd.GSJL) ? 0m : Convert.ToDecimal(gzd.GSJL)) +
                (string.IsNullOrEmpty(gzd.GSCJ) ? 0m : Convert.ToDecimal(gzd.GSCJ));
            //应发合计
            gzd.YFHJ = Math.Round(tmp_Yfhj, 2, MidpointRounding.AwayFromZero).ToString();
            //工会互助金???
            var hzj_kk = new BaseDal<DataBaseGeneral_HZJJ_NoMoney>().Get(t => t.UserCode == gzd.UserCode && t.TheYear == gzd.TheYear && t.TheMonth == gzd.TheMonth);
            if (hzj_kk == null)
            {
                var hzj_high = new BaseDal<DataBaseGeneral_HZJJ_High>().Get(t => t.UserCode == gzd.UserCode);
                if (hzj_high == null)
                {
                    var baseMonth = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.PostName == "互助金");
                    gzd.GHHZJ = baseMonth.MoneyBase;
                }
                else
                {
                    gzd.GHHZJ = hzj_high.HZJJ;
                }
            }
            //个税
            decimal excel_bl = string.IsNullOrEmpty(gzd.YFHJ) ? 0M : Convert.ToDecimal(gzd.YFHJ);
            decimal excel_bn = string.IsNullOrEmpty(gzd.DKGJJ) ? 0M : Convert.ToDecimal(gzd.DKGJJ);
            decimal excel_bo = string.IsNullOrEmpty(gzd.DKBXF) ? 0M : Convert.ToDecimal(gzd.DKBXF);
            decimal excel_bm = string.IsNullOrEmpty(gzd.GHHZJ) ? 0M : Convert.ToDecimal(gzd.GHHZJ);
            if (excel_bl - excel_bo - excel_bn - excel_bm > 3500)
            {
                decimal[] d1 = { 0.03M, 0.1M, 0.2M, 0.25M, 0.3M, 0.35M, 0.45M };
                decimal[] d2 = { 0M, 105M, 555M, 1005M, 2755M, 5505M, 13505M };
                List<decimal> d3 = new List<decimal>();
                for (int i = 0; i < d1.Length; i++)
                {
                    d3.Add((excel_bl - excel_bo - excel_bn - excel_bm - 3500) * d1[i] - d2[i]);
                }
                gzd.GS = Math.Round(d3.Max(), 2, MidpointRounding.AwayFromZero).ToString();
            }
            decimal excel_bp = string.IsNullOrEmpty(gzd.GS) ? 0M : Convert.ToDecimal(gzd.GS);
            decimal excel_bq = string.IsNullOrEmpty(gzd.SHKK) ? 0M : Convert.ToDecimal(gzd.SHKK);
            gzd.SFHJ = Math.Round(excel_bl - excel_bo - excel_bn - excel_bm - excel_bp - excel_bq, 2, MidpointRounding.AwayFromZero).ToString();

            return gzd;
        }

        private Thread trCalculation;

        private void BtnRun_Click(object sender, EventArgs e)
        {
            object obj;
            switch (cmbFactory.Text)
            {
                case "一厂":
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        obj = conn.ExecuteScalar("select count(id) from DataBase1GZDS where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        if (Convert.ToInt32(obj) > 0)
                        {
                            if (MessageBox.Show("已经存在【" + cmbFactory.Text + "】数据，重新计算将会删除已存在的数据，确定继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            {
                                return;
                            }
                            conn.Execute("delete DataBase1GZDS where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        }
                    }
                    trCalculation = new Thread(new ParameterizedThreadStart(Calculation1Factory_New));
                    break;
                case "二厂":
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        obj = conn.ExecuteScalar("select count(id) from DataBase2GZDS where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        if (Convert.ToInt32(obj) > 0)
                        {
                            if (MessageBox.Show("已经存在【" + cmbFactory.Text + "】数据，重新计算将会删除已存在的数据，确定继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            {
                                return;
                            }
                            conn.Execute("delete DataBase2GZDS where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        }
                    }
                    trCalculation = new Thread(new ParameterizedThreadStart(Calculation2Factory_New));
                    break;
                case "三厂":
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                    {
                        obj = conn.ExecuteScalar("select count(id) from gzds where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        if (Convert.ToInt32(obj) > 0)
                        {
                            if (MessageBox.Show("已经存在【" + cmbFactory.Text + "】数据，重新计算将会删除已存在的数据，确定继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            {
                                return;
                            }
                            conn.Execute("delete gzds where theyear =" + dtp.Value.Year.ToString() + " and themonth=" + dtp.Value.Month.ToString());
                        }
                    }
                    trCalculation = new Thread(new ParameterizedThreadStart(Calculation_New));
                    break;
            }

            GZD_Work gzd_work = new GZD_Work { Time = dtp.Value/*, Factory = cmbFactory.Text, Dept = cmbDept.Text */};
            txtMsg.Text = "开始计算，计算时间较长，计算过程中不要关闭软件，否则计算失败！计算完成后【有提示】。" + Environment.NewLine + "计算中，请稍后……";
            btnRun.Enabled = false;
            trCalculation.Start(gzd_work);

        }

        private void FrmGetGZD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (trCalculation != null && trCalculation.IsAlive)
            {
                trCalculation.Abort();
            }
        }

        #region 一厂工资计算

        /// <summary>
        /// 仓储车间工资计算
        /// </summary>
        /// <param name="item_CQ">出勤</param>
        /// <returns></returns>
        private DataBase1GZD Calculation1CC(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1CC").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1CC" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XT>().Get(t => t.FactoryNo == "G001" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "仓储");
            if (xt != null)
            {
                gzd.RZBZ = xt.BZJE;
            }

            //计件工资1
            var jjgz = new BaseDal<DataBase1CC_CJB>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (jjgz != null)
            {
                gzd.JJGZ1 = jjgz.HJ;
            }

            //辅助验货
            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "仓储" && t.FactoryNo == "G001").ToList();
            if (fzyh != null && fzyh.Count > 0)
            {
                gzd.FZYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //车间奖金/罚款
            var cjjj = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "仓储" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjj != null)
            {
                gzd.CJJJ = cjjj.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjj.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 成型多能工
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1CX_DNG(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1CX_DNG").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept && t.Position.Contains("多能工")).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1CX_DNG" };


            //多能工考核
            var kh = new BaseDal<DataBase1CX_DNG_01CXDNGKH_02KH>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (kh != null)
            {
                gzd.KHGZ1 = kh.HJ;
            }

            return gzd;
        }

        /// <summary>
        /// 检包车间工资计算
        /// </summary>
        /// <param name="item_CQ">出勤</param>
        /// <returns></returns>
        private DataBase1GZD Calculation1JB(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1JB").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1JB" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            var m_rzbz = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M);
            gzd.RZBZ = (d2_rzbz + m_rzbz).ToString();

            //计件工资1
            var ywrtcq = new BaseDal<DataBase1JB_XWRYCQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (ywrtcq != null && ywrtcq.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (ywrtcq.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M) + jjgz1).ToString();
            }

            //计件工资2
            var jjrlr = new BaseDal<DataBase1JB_JJRLR>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            jjrlr = jjrlr.Where(t => t.IsKF_Manager && t.IsPG_Manager && t.IsPMC_Manager).ToList();
            if (jjrlr != null && jjrlr.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (jjrlr.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var cjjj = new BaseDal<DataBase1JB_CJJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var mclbjj = new BaseDal<DataBase1JB_MCLBJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();

            if (cjjj != null && cjjj.Count > 0 && mclbjj != null && mclbjj.Count > 0)
            {
                double.TryParse(gzd.JJGZ3, out double jjgz3);
                gzd.JJGZ3 = (cjjj.Sum(t => t.Results.Sum(x => x.Money)) + mclbjj.Sum(t => double.TryParse(t.Money, out double d) ? d : 0) + jjgz3).ToString();
            }

            //辅助验货
            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "检包" && t.FactoryNo == "G001").ToList();
            if (fzyh != null && fzyh.Count > 0)
            {
                gzd.FZYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //开发试烧补助
            var kfss = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.Factory == "G001" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (kfss != null && kfss.Count > 0)
            {
                gzd.SSBZ = kfss.Sum(t => decimal.TryParse(t.JE, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "检包" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 模具车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1MJ(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1MJ").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1MJ" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G001" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "模具").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //计件工资1
            var pmcdj = new BaseDal<DataBase1MJ_PMCDJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var pmcxj = new BaseDal<DataBase1MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (pmcdj != null && pmcdj.Count > 0 && pmcxj != null && pmcxj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (pmcdj.Sum(t => t.Childs.Sum(x => decimal.TryParse(x.Money, out decimal d_tp) ? d_tp : 0M)) + pmcxj.Sum(t => t.Childs.Sum(x => decimal.TryParse(x.Money, out decimal d) ? d : 0M)) + jjgz1).ToString();
            }

            //计件工资2
            var ymjj = new BaseDal<DataBase1MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (ymjj != null && ymjj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (ymjj.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var xsgjj = new BaseDal<DataBase1MJ_XSGJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (xsgjj != null && xsgjj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ3, out decimal jjgz3);
                gzd.JJGZ3 = (xsgjj.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M) + jjgz3).ToString();
            }

            //日工
            var rgtb = new BaseDal<DataBase1MJ_RGTB>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (rgtb != null && rgtb.Count > 0)
            {

                decimal.TryParse(gzd.RG, out decimal rg);
                gzd.RG = (rgtb.Sum(t => decimal.TryParse(t.RGHJ, out decimal d) ? d : 0M) + rg).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "模具" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 原料车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1YL(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1YL").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1YL" };

            //计件工资1
            var jjgz = new BaseDal<DataBase1YL_JJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (jjgz != null)
            {
                gzd.JJGZ1 = jjgz.JJJE;
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "原料" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 喷釉工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1PY(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1PY").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1PY" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("喷釉")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            var m_rzbz = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("喷釉")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M);
            gzd.RZBZ = (d2_rzbz + m_rzbz).ToString();

            //计件工资1
            var pygjj = new BaseDal<DataBase1PY_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "喷釉工（手工）" || t.GZ == "喷釉工（机械）")).ToList();
            if (pygjj != null && pygjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (pygjj.Sum(t => decimal.TryParse(t.PYGSGJJ, out decimal d_tp) ? d_tp : 0M) + pygjj.Sum(t => decimal.TryParse(t.PYGJXJJ, out decimal d) ? d : 0M) + jjgz1).ToString();
            }

            //计件工资2
            var csgjj = new BaseDal<DataBase1PY_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "擦水工（手工）" || t.GZ == "擦水工（机械）")).ToList();
            if (csgjj != null && csgjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (csgjj.Sum(t => decimal.TryParse(t.CSGJXJJ, out decimal d_tp) ? d_tp : 0M) + csgjj.Sum(t => decimal.TryParse(t.CSGSGJJ, out decimal d) ? d : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var lpjj = new BaseDal<DataBase1PY_LPJJ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (lpjj != null && lpjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ3, out decimal jjgz3);
                gzd.JJGZ3 = (lpjj.Sum(t => decimal.TryParse(t.JJJE, out decimal d_tp) ? d_tp : 0M) + jjgz3).ToString();
            }

            //考核工资1
            var pygygz = new BaseDal<DataBase1PY_CJB_YGZ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (pygygz != null && pygygz.Count > 0)
            {
                decimal.TryParse(gzd.KHGZ1, out decimal khgz1);
                gzd.KHGZ1 = (pygjj.Sum(t => decimal.TryParse(t.PYGSGKH, out decimal d_tp) ? d_tp : 0M) + pygjj.Sum(t => decimal.TryParse(t.PYGJXKH, out decimal d) ? d : 0M) + pygygz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + khgz1).ToString();
            }

            //考核工资2
            decimal.TryParse(gzd.KHGZ2, out decimal khgz2);
            gzd.JJGZ2 = (csgjj.Sum(t => decimal.TryParse(t.CSGJJKH, out decimal d_tp) ? d_tp : 0M) + csgjj.Sum(t => decimal.TryParse(t.CSGSGKH, out decimal d) ? d : 0M) + khgz2).ToString();


            //试烧补助
            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.Dept.Contains("喷釉")).ToList();
            var jsbz = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.CJ.Contains("喷釉")).ToList();

            if (kfss != null && kfss.Count > 0 && jsbz != null && jsbz.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m) + jsbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "喷釉" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G001") && t.Dept.Equals("喷釉") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 烧成工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1SC(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1SC").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1SC" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("烧成")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            gzd.RZBZ = d2_rzbz.ToString();

            //计件工资1
            var zygjj = new BaseDal<DataBase1SC_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "装窑工（大）" || t.GZ == "装窑工（替补）")).ToList();
            if (zygjj != null && zygjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (zygjj.Sum(t => decimal.TryParse(t.ZYGDJJ, out decimal d_tp) ? d_tp : 0M) + zygjj.Sum(t => decimal.TryParse(t.ZYGTBJJ, out decimal d) ? d : 0M) + jjgz1).ToString();
            }

            //计件工资2
            var qtjj = new BaseDal<DataBase1SC_QTJJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (qtjj != null)
            {
                gzd.JJGZ2 = qtjj.JJJE;
            }

            //考核工资1
            var zygkh = new BaseDal<DataBase1SC_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "装窑工（大）" || t.GZ == "补瓷工")).ToList();
            if (zygkh != null && zygkh.Count > 0)
            {
                decimal.TryParse(gzd.KHGZ1, out decimal khjj1);
                gzd.KHGZ1 = (zygkh.Sum(t => decimal.TryParse(t.ZYGDKH, out decimal d_tp) ? d_tp : 0M) + zygjj.Sum(t => decimal.TryParse(t.BCKH, out decimal d) ? d : 0M) + khjj1).ToString();
            }

            //试烧补助
            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.Dept.Contains("烧成")).ToList();
            var jsbz = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.CJ.Contains("烧成")).ToList();

            if (kfss != null && kfss.Count > 0 && jsbz != null && jsbz.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m) + jsbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "烧成" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G001") && t.Dept.Equals("烧成") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 修检线工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1XJX(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1XJX").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1XJX" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G001" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("修检线")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            gzd.RZBZ = d2_rzbz.ToString();

            //计件工资1
            var xjdybg = new BaseDal<DataBase1XJ_XJDYBG>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xjdybg != null)
            {
                gzd.JJGZ1 = xjdybg.XJJJ;
            }

            //考核工资1
            var xckh = new BaseDal<DataBase1XJ_XCKH>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xckh != null)
            {
                gzd.KHGZ1 = xckh.KHJE;
            }

            //试烧补助
            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.Dept.Contains("修检线")).ToList();
            var jsbz = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G001" && t.CJ.Contains("修检线")).ToList();

            if (kfss != null && kfss.Count > 0 && jsbz != null && jsbz.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m) + jsbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "修检线" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G001") && t.Dept.Equals("修检线") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 半检拉坯工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1CX_BJLP(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1CX_BJLP").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1CX_BJLP" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G001" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "半检拉坯").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //计件工资1
            var xjdybg = new BaseDal<DataBase1CX_BJLP_01_JJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xjdybg != null)
            {
                gzd.JJGZ1 = xjdybg.JJJE;
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "半检拉坯" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G001") && t.Dept.Equals("半检拉坯") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 成型车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase1GZD Calculation1CX(List<DataBase1GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "1CX").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase1GZD gzd = new DataBase1GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "1CX" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G001" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "成型").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //变产补助还未做
            var bcbz = new BaseDal<DataBase1CX_BCBZ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (bcbz != null && bcbz.Count > 0)
            {
                gzd.BCBZ = bcbz.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            if (!string.IsNullOrEmpty(gzd.RZBZ) && !string.IsNullOrEmpty(gzd.BCBZ))
            {
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，入职补助和变产补助同一个人都有数据", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，入职补助和变产补助同一个人都有数据,工号:{gzd.UserCode}，姓名:{gzd.UserName}", MsgClass = "成型车间报警", UserCode = xt.FirstOrDefault().CreateUser.Split('_')[0] }, true);
            }

            //计件工资1
            var xjdybg = new BaseDal<DataBase1CX_CX_06JJ_GR>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xjdybg != null)
            {
                gzd.JJGZ1 = xjdybg.GRHJ;
            }

            //计件工资2
            var xjbg = new BaseDal<DataBase1CX_XJBG>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (xjbg != null || xjbg.Count != 0)
            {
                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (xjbg.Sum(t => decimal.TryParse(t.JE, out decimal d_tp) ? d_tp : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var qtjj = new BaseDal<DataBase1CX_QTJJ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (qtjj != null || qtjj.Count != 0)
            {
                decimal.TryParse(gzd.JJGZ3, out decimal jjgz3);
                gzd.JJGZ3 = (qtjj.Sum(t => decimal.TryParse(t.JE, out decimal d_tp) ? d_tp : 0M) + jjgz3).ToString();
            }

            //试烧补助
            var kfss = new BaseDal<DataBase1CX_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (kfss != null && kfss.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //破损补助
            var psbz = new BaseDal<DataBase1CX_PSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (psbz != null && psbz.Count > 0)
            {
                gzd.PSBZ = (psbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "成型" && t.TheType == "workshop" && t.FactoryNo == "1");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //辅料奖罚
            var fljf = new BaseDal<DataBase1CX_FLJF>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.UserCode.Equals(item_CQ.UserCode)).ToList();

            if (fljf.Count > 0)
            {
                gzd.FLFK = fljf.Sum(t => string.IsNullOrEmpty(t.FLFK) ? 0M : Convert.ToDecimal(t.FLFK)).ToString();
            }


            return gzd;
        }

        #endregion

        #region 二厂工资计算

        /// <summary>
        /// 仓储车间工资计算
        /// </summary>
        /// <param name="item_CQ">出勤</param>
        /// <returns></returns>
        private DataBase2GZD Calculation2CC(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2CC").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2CC" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XT>().Get(t => t.FactoryNo == "G002" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "仓储");
            if (xt != null)
            {
                gzd.RZBZ = xt.BZJE;
            }

            //计件工资1
            var jjgz = new BaseDal<DataBase2CC_CJB>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (jjgz != null)
            {
                gzd.JJGZ1 = jjgz.HJ;
            }

            //辅助验货
            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "仓储" && t.FactoryNo == "G002").ToList();
            if (fzyh != null && fzyh.Count > 0)
            {
                gzd.FZYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //车间奖金/罚款
            var cjjj = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "仓储" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjj != null)
            {
                gzd.CJJJ = cjjj.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjj.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 模具车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2MJ(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2MJ").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2MJ" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G002" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "模具").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //计件工资1
            var pmcdj = new BaseDal<DataBase2MJ_PMCDJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var pmcxj = new BaseDal<DataBase2MJ_PMCXJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (pmcdj != null && pmcdj.Count > 0 && pmcxj != null && pmcxj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (pmcdj.Sum(t => t.Childs.Sum(x => decimal.TryParse(x.Money, out decimal d_tp) ? d_tp : 0M)) + pmcxj.Sum(t => t.Childs.Sum(x => decimal.TryParse(x.Money, out decimal d) ? d : 0M)) + jjgz1).ToString();
            }

            //计件工资2
            var ymjj = new BaseDal<DataBase2MJ_YMJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (ymjj != null && ymjj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (ymjj.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var xsgjj = new BaseDal<DataBase2MJ_QTJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (xsgjj != null && xsgjj.Count > 0)
            {

                decimal.TryParse(gzd.JJGZ3, out decimal jjgz3);
                gzd.JJGZ3 = (xsgjj.Sum(t => decimal.TryParse(t.JEHJ, out decimal d) ? d : 0M) + jjgz3).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "模具" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 喷釉工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2PY(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2PY").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2PY" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G002" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("喷釉")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            var m_rzbz = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G002" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("喷釉")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M);
            gzd.RZBZ = (d2_rzbz + m_rzbz).ToString();

            //计件工资1
            var pygjj = new BaseDal<DataBase2PY_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "喷釉工（手工）" || t.GZ == "喷釉工（机械）")).ToList();
            if (pygjj != null && pygjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (pygjj.Sum(t => decimal.TryParse(t.PYGSGJJ, out decimal d_tp) ? d_tp : 0M) + pygjj.Sum(t => decimal.TryParse(t.PYGJXJJ, out decimal d) ? d : 0M) + jjgz1).ToString();
            }

            //计件工资2
            var csgjj = new BaseDal<DataBase2PY_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && (t.GZ == "擦水工（手工）" || t.GZ == "擦水工（机械）")).ToList();
            if (csgjj != null && csgjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (csgjj.Sum(t => decimal.TryParse(t.CSGJXJJ, out decimal d_tp) ? d_tp : 0M) + csgjj.Sum(t => decimal.TryParse(t.CSGSGJJ, out decimal d) ? d : 0M) + jjgz2).ToString();
            }

            //考核工资1
            var pygygz = new BaseDal<DataBase2PY_CJB_YGZ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (pygygz != null && pygygz.Count > 0)
            {
                decimal.TryParse(gzd.KHGZ1, out decimal khgz1);
                gzd.KHGZ1 = (pygjj.Sum(t => decimal.TryParse(t.PYGSGKH, out decimal d_tp) ? d_tp : 0M) + pygjj.Sum(t => decimal.TryParse(t.PYGJXKH, out decimal d) ? d : 0M) + pygygz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + khgz1).ToString();
            }

            //考核工资2
            decimal.TryParse(gzd.KHGZ2, out decimal khgz2);
            gzd.JJGZ2 = (csgjj.Sum(t => decimal.TryParse(t.CSGJJKH, out decimal d_tp) ? d_tp : 0M) + csgjj.Sum(t => decimal.TryParse(t.CSGSGKH, out decimal d) ? d : 0M) + khgz2).ToString();


            //试烧补助
            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G002" && t.Dept.Contains("喷釉")).ToList();
            var jsbz = new BaseDal<DataBase2PY_JSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();

            if (kfss != null && kfss.Count > 0 && jsbz != null && jsbz.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m) + jsbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "喷釉" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G002") && t.Dept.Equals("喷釉") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 烧成工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2SC(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2SC").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2SC" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G002" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("烧成")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            gzd.RZBZ = d2_rzbz.ToString();

            //计件工资1
            var zygjj = new BaseDal<DataBase2SC_CJB>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (zygjj != null && zygjj.Count > 0)
            {
                decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
                gzd.JJGZ1 = (zygjj.Sum(t => decimal.TryParse(t.ZYGDJJ, out decimal d_tp) ? d_tp : 0M) + jjgz1).ToString();
            }

            //考核工资1
            if (zygjj != null && zygjj.Count > 0)
            {
                decimal.TryParse(gzd.KHGZ1, out decimal khjj1);
                gzd.KHGZ1 = (zygjj.Sum(t => decimal.TryParse(t.ZYGDKH, out decimal d_tp) ? d_tp : 0M) + khjj1).ToString();
            }

            //试烧补助
            var kfss = new BaseDal<DataBaseGeneral_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G002" && t.Dept.Contains("烧成")).ToList();
            var jsbz = new BaseDal<DataBase1SC_JSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.FactoryNo == "G002" && t.CJ.Contains("烧成")).ToList();

            if (kfss != null && kfss.Count > 0 && jsbz != null && jsbz.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m) + jsbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "烧成" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G002") && t.Dept.Equals("烧成") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 原料车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2YL(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2YL").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2YL" };

            //计件工资1
            var jjgz = new BaseDal<DataBase2YL_XCJJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (jjgz != null)
            {
                gzd.JJGZ1 = jjgz.JJJE;
            }

            //计件工资2
            var grjj = new BaseDal<DataBase2YL_GRJJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (grjj != null)
            {
                gzd.JJGZ2 = grjj.JJGZ;
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "原料" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        /// <summary>
        /// 半检拉坯工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2CX_BJLP(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2CX_BJLP").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2CX_BJLP" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G002" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "半检拉坯").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //计件工资1、考核金额1
            var xjdybg = new BaseDal<DataBase2CX_BJLP_CJTB>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xjdybg != null)
            {
                gzd.JJGZ1 = xjdybg.JJJE;
                gzd.KHGZ1 = xjdybg.KHJE;
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "半检拉坯" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //破损罚款
            var pmcps = new BaseDal<DataBaseGeneral_PMCPS>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.FactoryNo.Equals("G002") && t.Dept.Equals("半检拉坯") && t.UserCode.Equals(item_CQ.UserCode)).ToList();
            decimal d_pmcps = 0m;
            if (pmcps.Count > 0)
            {
                d_pmcps = pmcps.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0m);
            }
            gzd.PSFK = d_pmcps > 0M ? d_pmcps.ToString() : string.Empty;

            return gzd;
        }

        /// <summary>
        /// 成型车间工资计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item_CQ"></param>
        /// <returns></returns>
        private DataBase2GZD Calculation2CX(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2CX").Count() > 0)
            {
                return null;
            }
            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);
            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2CX" };

            //入职补助
            var xt = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G002" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.CJ == "成型").ToList();
            if (xt != null && xt.Count > 0)
            {
                gzd.RZBZ = xt.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }

            //变产补助还未做
            var bcbz = new BaseDal<DataBase2CX_BCBZ>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (bcbz != null && bcbz.Count > 0)
            {
                gzd.BCBZ = bcbz.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString();
            }
            if (!string.IsNullOrEmpty(gzd.RZBZ) && !string.IsNullOrEmpty(gzd.BCBZ))
            {
                BJ(new DataBaseMsg { ID = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = "系统报警", IsDone = false, IsRead = false, MsgTitle = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，入职补助和变产补助同一个人都有数据", Msg = $"{dateTime.Year.ToString()}年{dateTime.Month.ToString()}月，入职补助和变产补助同一个人都有数据,工号:{gzd.UserCode}，姓名:{gzd.UserName}", MsgClass = "成型车间报警", UserCode = xt.FirstOrDefault().CreateUser.Split('_')[0] }, true);
            }

            //计件工资1
            var xjdybg = new BaseDal<DataBase2CX_CXJJ>().Get(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month);
            if (xjdybg != null)
            {
                gzd.JJGZ1 = xjdybg.GZE;
            }

            //计件工资2
            var xjbg = new BaseDal<DataBase2CX_XJBG>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (xjbg != null || xjbg.Count != 0)
            {
                decimal.TryParse(gzd.JJGZ2, out decimal jjgz2);
                gzd.JJGZ2 = (xjbg.Sum(t => decimal.TryParse(t.JE, out decimal d_tp) ? d_tp : 0M) + jjgz2).ToString();
            }

            //计件工资3
            var sfbz = new BaseDal<DataBase2CX_SFBZ>().GetList(t => t.SFCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (sfbz != null || sfbz.Count != 0)
            {
                decimal.TryParse(gzd.JJGZ3, out decimal jjgz3);
                gzd.JJGZ3 = (sfbz.Sum(t => decimal.TryParse(t.SFBZ, out decimal d_tp) ? d_tp : 0M) + jjgz3).ToString();
            }

            //试烧补助
            var kfss = new BaseDal<DataBase2CX_KFSS>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (kfss != null && kfss.Count > 0)
            {
                gzd.SSBZ = (kfss.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //破损补助
            var psbz = new BaseDal<DataBase2CX_PSBZ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            if (psbz != null && psbz.Count > 0)
            {
                gzd.PSBZ = (psbz.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0m)).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "成型" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            //辅料奖罚
            var fljf = new BaseDal<DataBase2CX_FLJF>().GetList(t => t.TheYear.Equals(dateTime.Year) && t.TheMonth.Equals(dateTime.Month) && t.UserCode.Equals(item_CQ.UserCode)).ToList();

            if (fljf.Count > 0)
            {
                gzd.FLFK = fljf.Sum(t => string.IsNullOrEmpty(t.FLFK) ? 0M : Convert.ToDecimal(t.FLFK)).ToString();
            }


            return gzd;
        }

        /// <summary>
        /// 检包车间工资计算
        /// </summary>
        /// <param name="item_CQ">出勤</param>
        /// <returns></returns>
        private DataBase2GZD Calculation2JB(List<DataBase2GZD> list, DataBaseGeneral_CQ item_CQ)
        {
            if (list.Count > 0 && list.Where(t => t.CalculationType == "2JB").Count() > 0)
            {
                return null;
            }

            List<DataBaseGeneral_CQ> list_CQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == item_CQ.TheYear && t.TheMonth == item_CQ.TheMonth && t.UserCode == item_CQ.UserCode && t.Factory == item_CQ.Factory && t.Dept == item_CQ.Dept).ToList();

            DateTime dateTime = new DateTime(item_CQ.TheYear, item_CQ.TheMonth, 1);

            DataBase2GZD gzd = new DataBase2GZD { Id = Guid.NewGuid(), CreateTime = Program.NowTime, CreateUser = Program.User.ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month, Factory = item_CQ.Factory, Dept = item_CQ.Dept, UserCode = item_CQ.UserCode, UserName = item_CQ.UserName, CalculationType = "2JB" };

            //入职补助。
            var d2_rzbz = new BaseDal<DataBaseGeneral_XT>().GetList(t => t.FactoryNo == "G002" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d_m) ? d_m : 0M);
            var m_rzbz = new BaseDal<DataBaseGeneral_XTDay>().GetList(t => t.FactoryNo == "G002" && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.CJ.Contains("检包")).ToList().Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M);
            gzd.RZBZ = (d2_rzbz + m_rzbz).ToString();

            //计件工资1
            var bzcq = new BaseDal<DataBase2JB_BZCQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var zzcq = new BaseDal<DataBase2JB_ZZCQ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var rkgjj = new BaseDal<DataBase2JB_RKGJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();

            decimal.TryParse(gzd.JJGZ1, out decimal jjgz1);
            gzd.JJGZ1 = (bzcq.Sum(t => decimal.TryParse(t.GRJJ, out decimal d) ? d : 0M) + zzcq.Sum(t => decimal.TryParse(t.GRJJ, out decimal d) ? d : 0M) + rkgjj.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M) + jjgz1).ToString();


            //计件工资2
            var fjjj = new BaseDal<DataBase2JB_FJJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode && t.IsPG_Check == true).ToList();
            var bjssjj = new BaseDal<DataBase2JB_BJSSJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var mcjj = new BaseDal<DataBase2JB_MCJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();
            var cjjj = new BaseDal<DataBase2JB_CJJJ>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.UserCode == item_CQ.UserCode).ToList();

            double.TryParse(gzd.JJGZ2, out double jjgz2);
            gzd.JJGZ2 = (fjjj.Sum(t => double.TryParse(t.JJJE, out double d) ? d : 0) + bjssjj.Sum(t => double.TryParse(t.JJJE, out double d) ? d : 0) + mcjj.Sum(t => double.TryParse(t.Money, out double d) ? d : 0) + cjjj.Sum(t => t.Results.Sum(x => x.Money)) + jjgz2).ToString();


            //辅助验货
            var fzyh = new BaseDal<DataBaseGeneral_FZYH>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "检包" && t.FactoryNo == "G002").ToList();
            if (fzyh != null && fzyh.Count > 0)
            {
                gzd.FZYH = fzyh.Sum(t => decimal.TryParse(t.Money, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //开发试烧补助
            var kfss = new BaseDal<DataBase1JB_KFSS>().GetList(t => t.Factory == "G002" && t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            if (kfss != null && kfss.Count > 0)
            {
                gzd.SSBZ = kfss.Sum(t => decimal.TryParse(t.JE, out decimal d_tp) ? d_tp : 0M).ToString();
            }

            //车间奖金/罚款
            var cjjc = new BaseDal<DataBaseGeneral_JC_Dept>().GetList(t => t.UserCode == item_CQ.UserCode && t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month && t.Dept == "检包" && t.TheType == "workshop" && t.FactoryNo == "2");
            if (cjjc != null)
            {
                gzd.CJJJ = cjjc.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString();
                gzd.CJFK = cjjc.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.J)).ToString();

            }

            return gzd;
        }

        #endregion
    }
}