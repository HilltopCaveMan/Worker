using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using System;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmLogin : Office2007Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUserCode.Focus();

            //txtUserCode.Text = "M16624";
            //txtPwd.Text = "112233";
            //btnLogin.PerformClick();
            txtUserCode.Text = "Admin";
            txtPwd.Text = "ADMIN";
            //btnLogin.PerformClick();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (MyDal.IsCanLogin(txtUserCode.Text, txtPwd.Text, out Program.User))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBoxEx.Show("工号或密码错误，请输入OA工号和密码！", "登陆失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}