using Monopy.PreceRateWage.Model;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    internal static class Program
    {
        public static System.Threading.Mutex run;

        public static BaseUser User;
        public static DateTime NowTime;
        public static string HrCode = "M11936";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            run = new System.Threading.Mutex(true, Process.GetCurrentProcess().ProcessName, out bool runone);
            if (runone)
            {
                run.ReleaseMutex();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                FrmLogin lg = new FrmLogin();
                FrmMain fm = new FrmMain();
                lg.ShowDialog();
                if (lg.DialogResult == DialogResult.OK) Application.Run(fm);
            }
            else
            {
                MessageBox.Show("程序已经运行，如果非正常关闭，并且不能再次运行，请检查【任务管理器】，或重启计算机！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}