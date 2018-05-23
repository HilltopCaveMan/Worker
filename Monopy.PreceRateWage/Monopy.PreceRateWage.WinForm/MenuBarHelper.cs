using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public class MenuBarHelper
    {
        private List<BaseGroup> _baseGroupInfos;
        private SuperTabControl _superTabControl;

        public MenuBarHelper(List<BaseGroup> baseGroupInfos, SuperTabControl superTabControl)
        {
            _baseGroupInfos = baseGroupInfos;
            _superTabControl = superTabControl;
            _superTabControl.CloseButtonOnTabsVisible = true;
        }

        private static string groupId;

        public static Dictionary<string, bool> FrmControlEnable(string userCode)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            var roles = new BaseDal<BaseRoleUser>().GetList(t => t.User.Code == userCode).ToList();
            foreach (var role in roles)
            {
                var groupRoles = new BaseDal<BaseGroupRole>().GetList(t => t.Role.Id == role.Role.Id).ToList();
                foreach (var groupRole in groupRoles)
                {
                    if (groupRole == null)
                    {
                        continue;
                    }
                    if (groupRole.Group.ID.ToString() == groupId)
                    {
                        //BtnPMCCheckYes$True;BtnPMCCheckNo$True;BtnPGCheckYes$False;BtnPGCheckNo$False;
                        if (string.IsNullOrEmpty(groupRole.Paras))
                        {
                            continue;
                        }
                        var strs = groupRole.Paras.TrimEnd(';').Split(';');
                        for (int i = 0; i < strs.Length; i++)
                        {
                            string key = strs[i].Split('$')[0];
                            bool value = Convert.ToBoolean(strs[i].Split('$')[1]);
                            if (!result.ContainsKey(key))
                            {
                                result.Add(key, value);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<BaseRoleUser> listRoleUsers;

        private void GetListRoles(string userCode)
        {
            listRoleUsers = /*conn.Query<BaseRoleUser>("select a.* from BaseRoleUsers a INNER JOIN BaseUsers b on a.user_id=b.Id and b.Code=@code", new { code = userCode });*/ new BaseDal<BaseRoleUser>().GetList(t => t.User.Code == userCode).ToList();
        }

        public bool MenuEnable(string groupId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
            {
                foreach (var role in listRoleUsers)
                {
                    var groupRoles = /*conn.Query<BaseGroupRole>("select * from BaseGroupRoles where role_id=@role_id", new { role_id = role.Id });*/ new BaseDal<BaseGroupRole>().GetList(t => t.Role.Id == role.Role.Id && t.Group.ID.ToString() == groupId).ToList();
                    if (groupRoles.Count > 0)
                    {
                        return true;
                    }
                    //if (conn.ExecuteScalar("select id from BaseGroupRoles where role_id = @role_id and group_id=@group_id", new { role_id = role.Id, group_id = new Guid(groupId) }) != null)
                    //{
                    //    return true;
                    //}
                    //foreach (var groupRole in groupRoles)
                    //{
                    //    if (groupRole.Group.ID.ToString() == groupId)
                    //    {
                    //        return true;
                    //    }
                    //}
                }
                return false;
            }
        }

        public Dictionary<RibbonTabItem, RibbonPanel> Get(string userCode)
        {
            GetListRoles(userCode);
            Dictionary<RibbonTabItem, RibbonPanel> dicResult = new Dictionary<RibbonTabItem, RibbonPanel>();
            List<BaseGroup> parentGroupInfos = _baseGroupInfos.Where(t => string.IsNullOrEmpty(t.ParentID)).OrderBy(t => t.GroupID).ToList();
            foreach (BaseGroup parentItem in parentGroupInfos)
            {
                RibbonBar ribbonBar = new RibbonBar()
                {
                    TitleVisible = false
                };
                RibbonPanel ribbonPanel = new RibbonPanel()
                {
                    Dock = DockStyle.Fill,
                    Text = parentItem.Name
                };
                ribbonPanel.Controls.Add(ribbonBar);
                RibbonTabItem ribbonTabItem = new RibbonTabItem()
                {
                    Text = parentItem.Name,
                    BeginGroup = true,
                    Symbol = parentItem.Symbol,
                    ButtonStyle = eButtonStyle.ImageAndText,
                    Panel = ribbonPanel,
                    Checked = parentItem.IsChecked
                };
                List<BaseGroup> baseGroupInfos = _baseGroupInfos.Where(t => t.ParentID == parentItem.GroupID).OrderBy(t => t.GroupID).ToList();
                foreach (BaseGroup item in baseGroupInfos)
                {
                    if (item.IsOnRibbonBar)
                    {
                        GalleryContainer gr = new GalleryContainer()
                        {
                            PopupUsesStandardScrollbars = false,
                            CanCustomize = false
                        };
                        ButtonItem buttonItemT2 = new ButtonItem()
                        {
                            Text = item.Name,
                            Tag = item,
                            BeginGroup = item.IsBeginGroup,
                            Symbol = item.Symbol,
                            Tooltip = item.Tooltip,
                            ButtonStyle = eButtonStyle.ImageAndText,
                            //ImagePosition = eImagePosition.Top,
                            AutoExpandOnClick = true
                        };
                        buttonItemT2.SubItems.AddRange(new BaseItem[] { gr });
                        var gi = _baseGroupInfos.Where(t => t.ParentID == item.GroupID).OrderBy(t => t.GroupID).ToList();
                        int oneHeight = 28;
                        gr.MinimumSize = new System.Drawing.Size(240, oneHeight * gi.Count + 20);
                        foreach (var it in gi)
                        {
                            ButtonItem buttonItem = new ButtonItem()
                            {
                                Text = it.Name,
                                Tag = it,
                                BeginGroup = it.IsBeginGroup,
                                Symbol = it.Symbol,
                                Tooltip = it.Tooltip,
                                ButtonStyle = eButtonStyle.ImageAndText,
                                Enabled = userCode == "Admin" ? true : MenuEnable(it.ID.ToString()),
                                FixedSize = new System.Drawing.Size(240, oneHeight)
                            };
                            buttonItem.Click += ButtonItemClick;
                            gr.SubItems.Add(buttonItem);
                        }
                        ribbonBar.Items.Add(buttonItemT2);
                    }
                    else
                    {
                        ButtonItem buttonItem = new ButtonItem()
                        {
                            Text = item.Name,
                            Tag = item,
                            BeginGroup = item.IsBeginGroup,
                            Symbol = item.Symbol,
                            Tooltip = item.Tooltip,
                            ImagePosition = eImagePosition.Top,
                            Enabled = userCode == "Admin" ? true : MenuEnable(item.ID.ToString()),
                            ButtonStyle = eButtonStyle.ImageAndText
                        };
                        buttonItem.Click += ButtonItemClick;
                        ribbonBar.Items.Add(buttonItem);
                    }
                }
                dicResult.Add(ribbonTabItem, ribbonPanel);
            }
            return dicResult;
        }

        private void ButtonItemClick(object sender, EventArgs e)
        {
            ButtonItem buttonItem = sender as ButtonItem;
            if (buttonItem == null)
            {
                return;
            }
            BaseGroup baseGroupInfo = buttonItem.Tag as BaseGroup;
            ShowForm(baseGroupInfo);
        }

        private void ShowForm(BaseGroup baseGroupInfo)
        {
            if (string.IsNullOrEmpty(baseGroupInfo.GroupClass))
            {
                MessageBox.Show("没有配置，请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Type type = Assembly.GetEntryAssembly().GetType(baseGroupInfo.GroupClass);
            object[] objArgs = null;
            if (baseGroupInfo.Paras != null && baseGroupInfo.Paras.Contains("a@"))
            {
                objArgs = baseGroupInfo.Paras.Substring(baseGroupInfo.Paras.IndexOf("a@") + 2).Split('$');
            }
            if (type == null)
            {
                MessageBox.Show("配置错误，请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form form = null;
            object obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, objArgs);
            form = obj as Form;
            if (baseGroupInfo.Showdialog)
            {
                if (form != null)
                {
                    form.ShowDialog();
                }
            }
            else
            {
                foreach (SuperTabItem item in _superTabControl.Tabs)
                {
                    if (item.Tag != null && item.Tag.ToString() == baseGroupInfo.GroupID)
                    {
                        _superTabControl.SelectedTab = item;
                        groupId = item.Tag.ToString();
                        return;
                    }
                }
                if (form != null)
                {
                    groupId = baseGroupInfo.ID.ToString();
                    SuperTabItem superTabItem = _superTabControl.CreateTab(baseGroupInfo.Name);
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.TopLevel = false;
                    form.Visible = true;
                    form.Dock = DockStyle.Fill;
                    superTabItem.AttachedControl.Controls.Add(form);
                    superTabItem.CloseButtonVisible = baseGroupInfo.CloseButtonVisible;
                    superTabItem.Tag = baseGroupInfo.GroupID;
                    _superTabControl.SelectedTab = superTabItem;
                }
            }
        }
    }
}