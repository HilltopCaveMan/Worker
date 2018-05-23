using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Data;
using System.Linq;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmGroup : Office2007Form
    {
        public FrmGroup()
        {
            InitializeComponent();
            EnableGlass = false;
        }

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
                nodeParet.Tag = itemParent;
                nodeParet.NodeClick += node_NodeClick;
                var listT2 = listMain.Where(t => t.ParentID == itemParent.GroupID).OrderBy(t => t.GroupID).ToList();
                foreach (var item in listT2)
                {
                    Node np = new Node();
                    np.Expanded = true;
                    np.Text = item.Name;
                    np.Tag = item;
                    np.NodeClick += node_NodeClick;
                    var listT3 = listMain.Where(t => t.ParentID == item.GroupID).OrderBy(t => t.GroupID).ToList();
                    foreach (var it in listT3)
                    {
                        Node node = new Node();
                        node.Text = it.Name;
                        node.Tag = it;
                        node.NodeClick += node_NodeClick;
                        np.Nodes.Add(node);
                    }
                    nodeParet.Nodes.Add(np);
                }
                advTree1.Nodes.Add(nodeParet);
            }
            advTree1.EndUpdate();
        }

        private void node_NodeClick(object sender, EventArgs e)
        {
            Node node = sender as Node;
            propertyGrid1.SelectedObject = node.Tag as BaseGroup;
        }

        private void FrmGroup_Load(object sender, EventArgs e)
        {
            RefAdvTree();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = new BaseGroup() { CloseButtonVisible = true };
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            BaseGroup bg = propertyGrid1.SelectedObject as BaseGroup;
            if (bg.ID.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                new BaseDal<BaseGroup>().Delete(bg);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BaseGroup bg = propertyGrid1.SelectedObject as BaseGroup;
            if (bg.ID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                bg.ID = Guid.NewGuid();
                new BaseDal<BaseGroup>().Add(bg);
            }
            else
            {
                new BaseDal<BaseGroup>().Edit(bg);
            }
            propertyGrid1.SelectedObject = null;
            RefAdvTree();
        }
    }
}