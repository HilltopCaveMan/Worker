﻿namespace Monopy.PreceRateWage.WinForm
{
    partial class FrmBaseHeadcount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbFactory = new CCWin.SkinControl.SkinComboBox();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbDept = new CCWin.SkinControl.SkinComboBox();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtName = new CCWin.SkinControl.SkinTextBox();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.cmbF = new CCWin.SkinControl.SkinComboBox();
            this.btnViewExcel = new DevComponents.DotNetBar.ButtonX();
            this.btnImportExcel = new DevComponents.DotNetBar.ButtonX();
            this.btnExportExcel = new DevComponents.DotNetBar.ButtonX();
            this.btnNew = new DevComponents.DotNetBar.ButtonX();
            this.dgv = new CCWin.SkinControl.SkinDataGridView();
            this.groupPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.groupPanel1.Controls.Add(this.dtp);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.cmbFactory);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.cmbDept);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.txtName);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(922, 59);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 11;
            this.groupPanel1.Text = "查询";
            // 
            // dtp
            // 
            this.dtp.CustomFormat = "yyyy-MM";
            this.dtp.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp.Location = new System.Drawing.Point(50, 4);
            this.dtp.Name = "dtp";
            this.dtp.ShowUpDown = true;
            this.dtp.Size = new System.Drawing.Size(86, 26);
            this.dtp.TabIndex = 18;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(6, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 18);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 17;
            this.labelX1.Text = "日期：";
            // 
            // cmbFactory
            // 
            this.cmbFactory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFactory.Font = new System.Drawing.Font("宋体", 12F);
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(180, 4);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(75, 27);
            this.cmbFactory.TabIndex = 16;
            this.cmbFactory.WaterText = "";
            this.cmbFactory.SelectedIndexChanged += new System.EventHandler(this.cmbFactory_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(136, 8);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(44, 18);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX4.TabIndex = 15;
            this.labelX4.Text = "厂区：";
            // 
            // cmbDept
            // 
            this.cmbDept.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDept.Font = new System.Drawing.Font("宋体", 12F);
            this.cmbDept.FormattingEnabled = true;
            this.cmbDept.Location = new System.Drawing.Point(299, 4);
            this.cmbDept.Name = "cmbDept";
            this.cmbDept.Size = new System.Drawing.Size(121, 27);
            this.cmbDept.TabIndex = 14;
            this.cmbDept.WaterText = "";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(255, 8);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 18);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 13;
            this.labelX2.Text = "单位：";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.Transparent;
            this.txtName.DownBack = null;
            this.txtName.Icon = null;
            this.txtName.IconIsButton = false;
            this.txtName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtName.IsPasswordChat = '\0';
            this.txtName.IsSystemPasswordChar = false;
            this.txtName.Lines = new string[0];
            this.txtName.Location = new System.Drawing.Point(488, 3);
            this.txtName.Margin = new System.Windows.Forms.Padding(0);
            this.txtName.MaxLength = 32767;
            this.txtName.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtName.MouseBack = null;
            this.txtName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.NormlBack = null;
            this.txtName.Padding = new System.Windows.Forms.Padding(5);
            this.txtName.ReadOnly = false;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.Size = new System.Drawing.Size(86, 28);
            // 
            // 
            // 
            this.txtName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtName.SkinTxt.Name = "BaseText";
            this.txtName.SkinTxt.Size = new System.Drawing.Size(76, 18);
            this.txtName.SkinTxt.TabIndex = 0;
            this.txtName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtName.SkinTxt.WaterText = "";
            this.txtName.TabIndex = 8;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtName.WaterText = "";
            this.txtName.WordWrap = true;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(420, 8);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 18);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "岗位名称：";
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.Location = new System.Drawing.Point(579, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 28);
            this.btnSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSearch.Symbol = "";
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 58);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(103, 6);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.groupPanel2.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(0, 208);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(922, 64);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 13;
            this.groupPanel2.Text = "操作";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Controls.Add(this.dtpTo, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonX1, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbF, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnViewExcel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnImportExcel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExportExcel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNew, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(824, 40);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy-MM";
            this.dtpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTo.Font = new System.Drawing.Font("宋体", 12F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(588, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(111, 26);
            this.dtpTo.TabIndex = 19;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.buttonX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonX1.Location = new System.Drawing.Point(705, 3);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(116, 34);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.Symbol = "";
            this.buttonX1.TabIndex = 18;
            this.buttonX1.Text = "导入查询数据";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // cmbF
            // 
            this.cmbF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbF.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbF.Font = new System.Drawing.Font("宋体", 12F);
            this.cmbF.FormattingEnabled = true;
            this.cmbF.Items.AddRange(new object[] {
            "一厂",
            "二厂",
            "三厂"});
            this.cmbF.Location = new System.Drawing.Point(120, 3);
            this.cmbF.Name = "cmbF";
            this.cmbF.Size = new System.Drawing.Size(111, 27);
            this.cmbF.TabIndex = 17;
            this.cmbF.WaterText = "";
            // 
            // btnViewExcel
            // 
            this.btnViewExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnViewExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnViewExcel.Location = new System.Drawing.Point(3, 3);
            this.btnViewExcel.Name = "btnViewExcel";
            this.btnViewExcel.Size = new System.Drawing.Size(111, 34);
            this.btnViewExcel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnViewExcel.Symbol = "";
            this.btnViewExcel.TabIndex = 1;
            this.btnViewExcel.Text = "查看模板";
            this.btnViewExcel.Click += new System.EventHandler(this.btnViewExcel_Click);
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.btnImportExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportExcel.Location = new System.Drawing.Point(237, 3);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(111, 34);
            this.btnImportExcel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImportExcel.Symbol = "";
            this.btnImportExcel.TabIndex = 0;
            this.btnImportExcel.Text = "导入";
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportExcel.Location = new System.Drawing.Point(354, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(111, 34);
            this.btnExportExcel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExportExcel.Symbol = "";
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "导出";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnNew
            // 
            this.btnNew.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNew.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNew.Location = new System.Drawing.Point(471, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(111, 34);
            this.btnNew.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnNew.Symbol = "";
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "新增";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(246)))), ((int)(((byte)(253)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv.ColumnFont = null;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv.HeadFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv.HeadSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.Location = new System.Drawing.Point(0, 59);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(922, 149);
            this.dgv.TabIndex = 14;
            this.dgv.TitleBack = null;
            this.dgv.TitleBackColorBegin = System.Drawing.Color.White;
            this.dgv.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(196)))), ((int)(((byte)(242)))));
            this.dgv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseDown);
            // 
            // FrmBaseHeadcount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 272);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "FrmBaseHeadcount";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "编制";
            this.Load += new System.EventHandler(this.FrmBaseHeadcount_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private CCWin.SkinControl.SkinComboBox cmbFactory;
        private DevComponents.DotNetBar.LabelX labelX4;
        private CCWin.SkinControl.SkinComboBox cmbDept;
        private DevComponents.DotNetBar.LabelX labelX2;
        private CCWin.SkinControl.SkinTextBox txtName;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CCWin.SkinControl.SkinComboBox cmbF;
        private DevComponents.DotNetBar.ButtonX btnViewExcel;
        private DevComponents.DotNetBar.ButtonX btnImportExcel;
        private DevComponents.DotNetBar.ButtonX btnExportExcel;
        private DevComponents.DotNetBar.ButtonX btnNew;
        private CCWin.SkinControl.SkinDataGridView dgv;
        private System.Windows.Forms.DateTimePicker dtp;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}