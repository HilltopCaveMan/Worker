using Dapper;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmGroupRole : Office2007Form
    {
        private List<BaseUser> _listUser = new List<BaseUser>();

        public FrmGroupRole()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void RefRoleDgv()
        {
            var list = new BaseDal<BaseRole>().GetList(t => t.IsUsed).OrderBy(t => t.RoleName).ToList();
            dgvRole.DataSource = list;
            string[] header = "编号$权限名$是否有效$创建时间$创建人".Split('$');
            for (int i = 0; i < dgvRole.Columns.Count; i++)
            {
                dgvRole.Columns[i].HeaderText = header[i];
                dgvRole.Columns[i].Visible = i == 1 ? true : false;
            }
            dgvRole.ClearSelection();
        }

        private void RefUserDep()
        {
            List<BaseDep> list;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["OAContext"].ConnectionString))
            {
                list = con.Query<BaseDep>("select hr_dept_id,dept_name,dept_order from hr_dept where dept_parent_id='ROOT' or dept_parent_id is null order by dept_order").ToList();
            }
            list.Insert(0, new BaseDep() { dept_name = "全部", hr_dept_id = "HH", dept_order = 0 });
            cmbDep.DataSource = list;
            cmbDep.DisplayMember = "dept_name";
            cmbDep.ValueMember = "hr_dept_id";
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefAdvTree()
        {
            var listMain = new BaseDal<BaseGroup>().GetList().ToList();
            var listParent = listMain.Where(t => string.IsNullOrEmpty(t.ParentID)).OrderBy(t => t.GroupID).ToList();
            advTree1.BeginUpdate();
            advTree1.Nodes.Clear();
            foreach (var itemParent in listParent)
            {
                Node nodeParet = new Node();
                nodeParet.Expanded = true;
                nodeParet.Text = itemParent.Name;
                nodeParet.Tag = new RGSM { Group = itemParent };
                nodeParet.CheckBoxStyle = eCheckBoxStyle.CheckBox;
                nodeParet.CheckBoxVisible = true;
                nodeParet.CheckBoxThreeState = false;
                nodeParet.NodeClick += Node_NodeClick;
                var listT2 = listMain.Where(t => t.ParentID == itemParent.GroupID).OrderBy(t => t.GroupID).ToList();
                foreach (var item in listT2)
                {
                    Node np = new Node();
                    np.Expanded = true;
                    np.Text = item.Name;
                    np.Tag = new RGSM { Group = item };
                    np.CheckBoxStyle = eCheckBoxStyle.CheckBox;
                    np.CheckBoxVisible = true;
                    np.CheckBoxThreeState = false;
                    np.NodeClick += Node_NodeClick;
                    var listT3 = listMain.Where(t => t.ParentID == item.GroupID).OrderBy(t => t.GroupID).ToList();
                    foreach (var it in listT3)
                    {
                        Node node = new Node();
                        node.Text = it.Name;
                        node.Tag = new RGSM { Group = it };
                        node.CheckBoxStyle = eCheckBoxStyle.CheckBox;
                        node.CheckBoxVisible = true;
                        node.CheckBoxThreeState = false;
                        node.NodeClick += Node_NodeClick;
                        np.Nodes.Add(node);
                    }
                    nodeParet.Nodes.Add(np);
                }
                advTree1.Nodes.Add(nodeParet);
            }
            advTree1.EndUpdate();
        }

        private bool IsCkbChecked(BaseGroup group, string ckbTag)
        {
            if (listBaseGroupRole.Count == 0)
            {
                return false;
            }

            return true;
        }

        private void Node_NodeClick(object sender, EventArgs e)
        {
            gpGroupParas.Controls.Clear();
            Node node = sender as Node;
            SetChildNodes(node);
            if (node.Parent != null)
            {
                if (node.Checked)
                {
                    node.Parent.Checked = true;
                }
                else
                {
                    if (node.Parent.Nodes.Count == 1)
                    {
                        node.Parent.Checked = false;
                    }
                    else
                    {
                        bool isParentNeedCheck = false;
                        foreach (Node item in node.Parent.Nodes)
                        {
                            if (item.Checked)
                            {
                                isParentNeedCheck = true;
                                continue;
                            }
                        }
                        node.Parent.Checked = isParentNeedCheck;
                    }
                }
            }
            if (node.Checked)
            {
                gpGroupParas.Controls.Clear();
                BaseGroup bGroup = (node.Tag as RGSM).Group;
                if (!string.IsNullOrEmpty(bGroup.Paras) && bGroup.Paras.IndexOf("c@") >= 0)
                {
                    //c@BtnPMCCheckYes$PMC确认;BtnPMCCheckNo$PMC退回;BtnPGCheckYes$品管确认;BtnPGCheckNo$品管退回~a@3-jb-Pg
                    string[] paras = bGroup.Paras.Replace("c@", "").Split('~')[0].TrimEnd(';').Split(';');
                    //c@BtnPMCCheckYes$true;BtnPMCCheckNo$true;BtnPGCheckYes$true;BtnPGCheckNo$true~a@3-jb-Pg
                    string[] setParas = null;
                    if (!string.IsNullOrEmpty((node.Tag as RGSM).Paras))
                    {
                        setParas = (node.Tag as RGSM).Paras.Replace("c@", "").TrimEnd(';').Split('~')[0].Split(';');
                    }
                    if (setParas != null && setParas.Length != paras.Length)
                    {
                        setParas = null;
                        (node.Tag as RGSM).Paras = null;
                    }
                    if (paras.Length > 0)
                    {
                        TableLayoutPanel tlp = new TableLayoutPanel();
                        tlp.RowCount = paras.Length - 1;
                        tlp.ColumnCount = 1;
                        tlp.Dock = DockStyle.Fill;
                        tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                        tlp.Margin = new System.Windows.Forms.Padding(0);
                        tlp.Padding = new System.Windows.Forms.Padding(0);
                        for (int i = 0; i < tlp.RowCount; i++)
                        {
                            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                        }
                        for (int i = 0; i < tlp.ColumnCount; i++)
                        {
                            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F));
                        }

                        for (int i = 0; i < paras.Length; i++)
                        {
                            try
                            {
                                DevComponents.DotNetBar.Controls.CheckBoxX ckb = new DevComponents.DotNetBar.Controls.CheckBoxX();
                                ckb.Text = paras[i].Split('$')[1];
                                ckb.Name = "ckb_" + paras[i].Split('$')[0];
                                ckb.Tag = node.Tag;
                                ckb.Visible = true;
                                ckb.Checked = setParas == null ? false : (setParas.Length > i ? Convert.ToBoolean(setParas[i].Split('$')[1]) : false);
                                ckb.AutoSize = true;
                                ckb.CheckedChanged += Ckb_CheckedChanged;
                                tlp.Controls.Add(ckb, 0, i);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
                        }
                        gpGroupParas.Controls.Add(tlp);
                    }
                }
            }
        }

        private void Ckb_CheckedChanged(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Controls.CheckBoxX ckb = sender as DevComponents.DotNetBar.Controls.CheckBoxX;
            RGSM rGSM = ckb.Tag as RGSM;
            if (string.IsNullOrEmpty(rGSM.Paras))
            {
                var tmp = rGSM.Group.Paras;
                var r = new Regex(@"\$\w+");
                if (r.IsMatch(tmp))
                {
                    tmp = r.Replace(tmp, "$" + false.ToString());
                }
                rGSM.Paras = tmp;
            }
            string[] paras = rGSM.Paras.Replace("c@", "").TrimEnd(';').Split('~')[0].Split(';');
            rGSM.Paras = string.Empty;
            for (int i = 0; i < paras.Length; i++)
            {
                if (ckb.Name.Split('_')[1] == paras[i].Split('$')[0])
                {
                    paras[i] = paras[i].Split('$')[0] + "$" + ckb.Checked.ToString();
                }
                rGSM.Paras += paras[i] + ";";
            }
        }

        private void SetChildNodes(Node ParetNode)
        {
            foreach (Node item in ParetNode.Nodes)
            {
                item.Checked = ParetNode.Checked;
                SetChildNodes(item);
            }
        }

        private void FrmGroupRole_Load(object sender, EventArgs e)
        {
            RefRoleDgv();
            RefUserDep();
            RefAdvTree();
            dgvRole.ClearSelection();
        }

        private void RefUserSearch(string depId, string userID, string userName)
        {
            string sql = "select t1.user_name as 'Name', t1.user_show_id as 'Code', t2.dept_name as 'DepName' from user_user t1 left join hr_dept t2 on t1.dept_id=t2.hr_dept_id where t1.active_flag=1";
            if (depId != "HH")
            {
                sql += " and t2.hr_dept_id ='{0}' ";
                sql = string.Format(sql, depId);
            }
            if (!string.IsNullOrEmpty(userID))
            {
                sql += " and t1.user_show_id like '%{0}%'";
                sql = string.Format(sql, userID);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                sql += " and t1.user_name like '%{0}%'";
                sql = string.Format(sql, userName);
            }
            sql += " order by t1.user_show_id";
            List<BaseUser> list;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["OAContext"].ConnectionString))
            {
                list = con.Query<BaseUser>(sql).ToList();
            }
            dgvUser.DataSource = list;
            string[] heaer = "编号￥工号￥姓名￥部门￥备注￥最后登陆时间￥创建时间￥创建人".Split('￥');
            for (int i = 0; i < dgvUser.Columns.Count; i++)
            {
                dgvUser.Columns[i].HeaderText = heaer[i];
                dgvUser.Columns[i].Visible = false;
            }
            dgvUser.Columns[1].Visible = true;
            dgvUser.Columns[2].Visible = true;
            dgvUser.Columns[3].Visible = true;
            dgvUser.ClearSelection();
        }

        private void cmbDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefUserSearch((cmbDep.SelectedItem as BaseDep).hr_dept_id, txtUserCode.Text, txtUserName.Text);
        }

        private void txtUserCode_TextChanged(object sender, EventArgs e)
        {
            RefUserSearch((cmbDep.SelectedItem as BaseDep).hr_dept_id, txtUserCode.Text, txtUserName.Text);
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            RefUserSearch((cmbDep.SelectedItem as BaseDep).hr_dept_id, txtUserCode.Text, txtUserName.Text);
        }

        private void dgvRole_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var role = dgvRole.Rows[e.RowIndex].DataBoundItem as BaseRole;
            //string sql = "";
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            //{
            //}
            var tmp = new BaseDal<BaseRoleUser>().GetList(t => t.Role.Id == role.Id).ToList();
            _listUser = tmp.Select(t => t.User).ToList();
            RefdgvSelectUser();
            listBaseGroupRole = new BaseDal<BaseGroupRole>().GetList(t => t.Role.Id == role.Id).ToList();
            advTree1.SelectedIndex = 0;
            SetNodeTagClear(advTree1.Nodes);
            SetNodeChecked(advTree1.Nodes);
        }

        private void RefdgvSelectUser()
        {
            //BaseUser tmp = new BaseUser { Code = "M16624", Name = "黄昊" };
            //_listUser.Add(tmp);
            _listUser = _listUser.DistinctBy(t => t.Code).ToList();
            dgvSelectUser.DataSource = _listUser;
            dgvSelectUser.Columns[0].Visible = false;
            dgvSelectUser.Columns[1].HeaderText = "工号";
            dgvSelectUser.Columns[2].HeaderText = "姓名";
            dgvSelectUser.Columns[3].HeaderText = "部门";
            for (int i = 4; i < dgvSelectUser.Columns.Count; i++)
            {
                dgvSelectUser.Columns[i].Visible = false;
            }
            foreach (DataGridViewRow item in dgvSelectUser.Rows)
            {
                if (item.Cells["Id"].Value.ToString().Substring(0, 4) == "0000")
                {
                    item.DefaultCellStyle.BackColor = Color.Yellow;
                    item.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            dgvSelectUser.ClearSelection();
        }

        private void dgvUser_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var user = dgvUser.Rows[e.RowIndex].DataBoundItem as BaseUser;
            _listUser.Add(user);
            RefdgvSelectUser();
        }

        private void dgvSelectUser_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var user = dgvSelectUser.Rows[e.RowIndex].DataBoundItem as BaseUser;
            _listUser.Remove(user);
            RefdgvSelectUser();
        }

        private List<RGSM> listRGSM = new List<RGSM>();

        private void GetAllSelectNode(NodeCollection Nodes)
        {
            foreach (Node item in Nodes)
            {
                if (item.Nodes.Count != 0)
                {
                    GetAllSelectNode(item.Nodes);
                }
                else
                {
                    if (item.Checked)
                    {
                        listRGSM.Add(item.Tag as RGSM);
                    }
                }
            }
        }

        private List<BaseGroupRole> listBaseGroupRole = new List<BaseGroupRole>();

        private void SetNodeTagClear(NodeCollection nodes)
        {
            foreach (Node item in nodes)
            {
                if (item.Nodes.Count != 0)
                {
                    SetNodeTagClear(item.Nodes);
                }
                (item.Tag as RGSM).Paras = null;
            }
        }

        /// <summary>
        /// 根据角色，获取角色所拥有的权限，刷新AdvTree
        /// </summary>
        /// <param name="role"></param>
        private void SetNodeChecked(NodeCollection Nodes)
        {
            foreach (Node item in Nodes)
            {
                item.Checked = false;
                item.RaiseClick();
                if (item.Nodes.Count != 0)
                {
                    SetNodeChecked(item.Nodes);
                }
                else
                {
                    foreach (var groupRole in listBaseGroupRole)
                    {
                        if (item.Tag == null)
                        {
                            continue;
                        }
                        if (groupRole.Group.ID == ((item.Tag as RGSM).Group).ID)
                        {
                            (item.Tag as RGSM).Paras = groupRole.Paras;
                            item.Checked = true;
                            gpGroupParas.Controls.Clear();
                            item.RaiseClick();
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvSelectUser.Rows.Count == 0)
            {
                if (MessageBox.Show("没有用户，确定要保存吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            listRGSM.Clear();
            GetAllSelectNode(advTree1.Nodes);
            if (listRGSM.Count == 0)
            {
                if (MessageBox.Show("没有设置权限，确定要保存吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }
            var role = dgvRole.SelectedRows[0].DataBoundItem as BaseRole;
            var roleUser = new BaseDal<BaseRoleUser>().GetList(t => t.Role.Id == role.Id).ToList();
            foreach (var item in roleUser)
            {
                new BaseDal<BaseRoleUser>().Delete(item);
            }
            var groupRole = new BaseDal<BaseGroupRole>().GetList(t => t.Role.Id == role.Id).ToList();
            foreach (var item in groupRole)
            {
                new BaseDal<BaseGroupRole>().Delete(item);
            }
            foreach (DataGridViewRow item in dgvSelectUser.Rows)
            {
                var user = item.DataBoundItem as BaseUser;
                if (new BaseDal<BaseUser>().Get(t => t.Code == user.Code) == null)
                {
                    user.Id = Guid.NewGuid();
                    new BaseDal<BaseUser>().Add(user);
                }
                user = new BaseDal<BaseUser>().Get(t => t.Code == user.Code);
                string sql = "insert into BaseRoleUsers (id,Role_Id,User_Id) values (@id,@Role_Id,@User_Id)";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute(sql, new { id = Guid.NewGuid(), Role_Id = role.Id, User_Id = user.Id });
                }
            }
            foreach (var item in listRGSM)
            {
                string sql = "insert into BaseGroupRoles ([Id],[Paras],[Group_ID],[Role_Id]) values (@id,@paras,@group_id,@Role_Id)";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                {
                    conn.Execute(sql, new { id = Guid.NewGuid(), paras = item.Paras, group_id = item.Group.ID, Role_Id = role.Id });
                }
            }
            MessageBox.Show("保存成功！");
            FrmGroupRole_Load(null, null);
            dgvSelectUser.DataSource = null;
        }
    }
}