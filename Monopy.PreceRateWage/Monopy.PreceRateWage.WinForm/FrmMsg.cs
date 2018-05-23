using DevComponents.DotNetBar;
using Monopy.PreceRateWage.Dal;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopy.PreceRateWage.WinForm
{
    public partial class FrmMsg : Office2007Form
    {
        public FrmMsg()
        {
            InitializeComponent();
            EnableGlass = false;
        }

        private void FrmMsg_Load(object sender, EventArgs e)
        {
            RefDgv();
        }

        private void RefDgv()
        {
            var list = new BaseDal<DataBaseMsg>().GetList(t => t.UserCode == Program.User.Code && !t.IsDone).OrderByDescending(t => t.CreateTime).ToList();
            string[] heaer = "编号￥用户￥标题￥类￥消息内容￥是否已读￥是否完成￥消息时间￥消息创建人".Split('￥');
            dgv.DataSource = list;
            for (int i = 0; i < heaer.Length; i++)
            {
                dgv.Columns[i].HeaderText = heaer[i];
            }
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[5].Visible = false;
            dgv.Columns[6].Visible = false;
            dgv.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var list = dgv.DataSource as List<DataBaseMsg>;
            foreach (var item in list)
            {
                item.IsDone = true;
                item.IsRead = true;
                new BaseDal<DataBaseMsg>().Edit(item);
            }
            RefDgv();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count != 1)
            {
                return;
            }
            var item = dgv.SelectedRows[0].DataBoundItem as DataBaseMsg;
            item.IsDone = true;
            item.IsRead = true;
            new BaseDal<DataBaseMsg>().Edit(item);
            RefDgv();
        }
    }
}