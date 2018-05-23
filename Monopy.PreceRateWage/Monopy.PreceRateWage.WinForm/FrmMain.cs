using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmMain : Office2007RibbonForm
    {
        private Thread threadCheckDb = null;

        private List<DataBaseMsg> listMsg;
        private Thread threadMsg = null;

        public Action<string> ActionOpenWin;

        public FrmMain()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            List<BaseGroup> list;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                list = conn.Query<BaseGroup>("select * from BaseGroups").ToList();
            }
            //list = new BaseDal<BaseGroup>().GetList().ToList();
            MenuBarHelper bar = new MenuBarHelper(list, superTabControl1);
            var items = bar.Get(Program.User.Code);
            foreach (var item in items.Keys)
            {
                ribbonControl1.Controls.Add(items[item]);
                ribbonControl1.Items.Add(item);
            }
            threadMsg = new Thread(GetMsg)
            {
                IsBackground = true
            };
            threadMsg.Start();

            threadCheckDb = new Thread(CheckDb)
            {
                IsBackground = true
            };
            threadCheckDb.Start();
            lblUser.Text = Program.User.DepName + ":" + Program.User.ToString();
        }

        private void GetMsg()
        {
            bool tmp = true;
            while (true)
            {
                try
                {
                    listMsg = new BaseDal<DataBaseMsg>().GetList(t => t.UserCode == Program.User.Code && t.IsDone == false).ToList();
                    if (listMsg.Count > 0)
                    {
                        if (tmp)
                        {
                            InvokeHelper.SetInvoke(lblMsg, new InvokeData(this) { Text = "提醒", BackColor = Color.Lime, Symbol = listMsg.Count < 10 ? listMsg.Count.ToString() : "9+", SymbolColor = Color.Red });
                            tmp = false;
                        }
                        else
                        {
                            InvokeHelper.SetInvoke(lblMsg, new InvokeData(this) { Text = "提醒", BackColor = Color.Red, Symbol = "", SymbolColor = Color.Lime });
                            tmp = true;
                        }
                    }
                    else
                    {
                        InvokeHelper.SetInvoke(lblMsg, new InvokeData(this) { Text = string.Empty, Visible = false });
                    }
                }
                catch { }
                Thread.Sleep(2000);
            }
        }

        private void CheckDb()
        {
            while (true)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    try
                    {
                        DateTime dateTime = Convert.ToDateTime(con.ExecuteScalar("select getdate()"));
                        Program.NowTime = dateTime;
                        InvokeHelper.SetInvoke(lblTime, new InvokeData(this) { Text = dateTime.ToString("yyyy年MM月dd日 HH时mm分ss秒") });
                    }
                    catch
                    {
                        InvokeHelper.SetInvoke(lblTime, new InvokeData(this) { Text = "与服务器连接失败！", SymbolColor = Color.Red });
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            new FrmMsg().ShowDialog();
        }
    }
}