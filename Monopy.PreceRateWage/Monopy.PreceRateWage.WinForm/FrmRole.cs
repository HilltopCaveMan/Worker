using Dapper;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmRole : Office2007Form
    {
        public FrmRole()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefDgv()
        {
            var list = new BaseDal<BaseRole>().GetList().OrderBy(t => t.RoleName).ToList();
            dgv.DataSource = list;
            dgv.Columns[0].Visible = false;
            string[] header = "编号$权限名$是否有效$创建时间$创建人".Split('$');
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].HeaderText = header[i];
            }
            dgv.ClearSelection();
            txtRole.Clear();
            txtRole.Tag = null;
        }

        private void FrmRole_Load(object sender, EventArgs e)
        {
            RefDgv();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtRole.Tag = null;
            txtRole.Text = string.Empty;
            ckbUsed.Checked = true;
            txtRole.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count != 1)
            {
                MessageBoxEx.Show("请选中要删除的角色！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var item = dgv.SelectedRows[0].DataBoundItem as BaseRole;
            if (MessageBoxEx.Show("确定要删除选中的角色【" + item.RoleName + "】吗？删除不可逆，并且角色下的所有用户都将失去权限！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    string sql1 = "delete from BaseRoleUsers where role_id='" + item.Id + "'";
                    string sql2 = "delete from BaseGroupRoles where role_id='" + item.Id + "'";
                    //string sql3 = "delete from BaseRoles where id='" + item.Id + "'";
                    conn.Execute(sql1);
                    conn.Execute(sql2);
                    //conn.Execute(sql3);
                }

                if (new BaseDal<BaseRole>().Delete(item) > 0)
                {
                    MessageBoxEx.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefDgv();
                }
                else
                {
                    MessageBoxEx.Show("删除失败，请检查网络和操作是否正常！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int result;
            if (txtRole.Tag == null)
            {
                BaseRole item = new BaseRole() { Id = Guid.NewGuid(), RoleName = txtRole.Text, IsUsed = ckbUsed.Checked, CreateUser = Program.User.ToString(), CreateTime = Program.NowTime };
                result = new BaseDal<BaseRole>().Add(item);
            }
            else
            {
                BaseRole item = new BaseRole() { Id = (Guid)txtRole.Tag, RoleName = txtRole.Text, IsUsed = ckbUsed.Checked };
                result = new BaseDal<BaseRole>().Edit(item);
            }
            if (result > 0)
            {
                MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefDgv();
            }
            else
            {
                MessageBoxEx.Show("操作失败，请检查网络和操作是否正常！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.SelectedRows.Count != 1)
            {
                return;
            }
            var item = dgv.SelectedRows[0].DataBoundItem as BaseRole;
            txtRole.Tag = item.Id;
            txtRole.Text = item.RoleName;
            ckbUsed.Checked = item.IsUsed;
        }
    }
}