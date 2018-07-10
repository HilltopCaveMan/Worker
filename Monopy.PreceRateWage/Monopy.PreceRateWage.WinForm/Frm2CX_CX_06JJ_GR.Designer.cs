namespace Monopy.PreceRateWage.WinForm
{
    partial class Frm2CX_CX_06JJ_GR
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnRecount = new DevComponents.DotNetBar.ButtonX();
            this.btnExportExcel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.CmbGH = new System.Windows.Forms.ComboBox();
            this.CmbBZName = new System.Windows.Forms.ComboBox();
            this.CmbBZ = new System.Windows.Forms.ComboBox();
            this.lblPg = new DevComponents.DotNetBar.LabelX();
            this.lblLb2 = new DevComponents.DotNetBar.LabelX();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.lblLb1 = new DevComponents.DotNetBar.LabelX();
            this.lblMc2 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dgv = new CCWin.SkinControl.SkinDataGridView();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRecount
            // 
            this.btnRecount.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRecount.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.btnRecount.Location = new System.Drawing.Point(793, 3);
            this.btnRecount.Name = "btnRecount";
            this.btnRecount.Size = new System.Drawing.Size(101, 28);
            this.btnRecount.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRecount.Symbol = "";
            this.btnRecount.TabIndex = 7;
            this.btnRecount.Text = "重新计算";
            this.btnRecount.Click += new System.EventHandler(this.btnRecount_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExcel.Location = new System.Drawing.Point(897, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(87, 28);
            this.btnExportExcel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExportExcel.Symbol = "";
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "导出";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.groupPanel1.Controls.Add(this.btnExportExcel);
            this.groupPanel1.Controls.Add(this.btnRecount);
            this.groupPanel1.Controls.Add(this.CmbGH);
            this.groupPanel1.Controls.Add(this.CmbBZName);
            this.groupPanel1.Controls.Add(this.CmbBZ);
            this.groupPanel1.Controls.Add(this.lblPg);
            this.groupPanel1.Controls.Add(this.lblLb2);
            this.groupPanel1.Controls.Add(this.dtp);
            this.groupPanel1.Controls.Add(this.lblLb1);
            this.groupPanel1.Controls.Add(this.lblMc2);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(1087, 59);
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
            this.groupPanel1.TabIndex = 14;
            this.groupPanel1.Text = "查询";
            // 
            // CmbGH
            // 
            this.CmbGH.Font = new System.Drawing.Font("宋体", 12F);
            this.CmbGH.FormattingEnabled = true;
            this.CmbGH.Location = new System.Drawing.Point(577, 5);
            this.CmbGH.Name = "CmbGH";
            this.CmbGH.Size = new System.Drawing.Size(123, 24);
            this.CmbGH.TabIndex = 17;
            // 
            // CmbBZName
            // 
            this.CmbBZName.Font = new System.Drawing.Font("宋体", 12F);
            this.CmbBZName.FormattingEnabled = true;
            this.CmbBZName.Location = new System.Drawing.Point(410, 5);
            this.CmbBZName.Name = "CmbBZName";
            this.CmbBZName.Size = new System.Drawing.Size(123, 24);
            this.CmbBZName.TabIndex = 17;
            // 
            // CmbBZ
            // 
            this.CmbBZ.Font = new System.Drawing.Font("宋体", 12F);
            this.CmbBZ.FormattingEnabled = true;
            this.CmbBZ.Location = new System.Drawing.Point(208, 5);
            this.CmbBZ.Name = "CmbBZ";
            this.CmbBZ.Size = new System.Drawing.Size(134, 24);
            this.CmbBZ.TabIndex = 17;
            // 
            // lblPg
            // 
            this.lblPg.AutoSize = true;
            this.lblPg.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPg.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPg.Location = new System.Drawing.Point(975, 17);
            this.lblPg.Name = "lblPg";
            this.lblPg.Size = new System.Drawing.Size(0, 0);
            this.lblPg.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPg.TabIndex = 14;
            // 
            // lblLb2
            // 
            this.lblLb2.AutoSize = true;
            this.lblLb2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLb2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLb2.Location = new System.Drawing.Point(1236, 8);
            this.lblLb2.Name = "lblLb2";
            this.lblLb2.Size = new System.Drawing.Size(0, 0);
            this.lblLb2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblLb2.TabIndex = 13;
            // 
            // dtp
            // 
            this.dtp.CustomFormat = "yyyy-MM";
            this.dtp.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp.Location = new System.Drawing.Point(54, 4);
            this.dtp.Name = "dtp";
            this.dtp.ShowUpDown = true;
            this.dtp.Size = new System.Drawing.Size(86, 26);
            this.dtp.TabIndex = 12;
            this.dtp.ValueChanged += new System.EventHandler(this.dtp_ValueChanged);
            // 
            // lblLb1
            // 
            this.lblLb1.AutoSize = true;
            this.lblLb1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblLb1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLb1.Location = new System.Drawing.Point(975, 17);
            this.lblLb1.Name = "lblLb1";
            this.lblLb1.Size = new System.Drawing.Size(0, 0);
            this.lblLb1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblLb1.TabIndex = 11;
            // 
            // lblMc2
            // 
            this.lblMc2.AutoSize = true;
            this.lblMc2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMc2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMc2.Location = new System.Drawing.Point(975, 17);
            this.lblMc2.Name = "lblMc2";
            this.lblMc2.Size = new System.Drawing.Size(0, 0);
            this.lblMc2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblMc2.TabIndex = 10;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(533, 8);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 18);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "工号：";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(342, 8);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 18);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "班组名称：";
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.Location = new System.Drawing.Point(703, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 28);
            this.btnSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSearch.Symbol = "";
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(140, 8);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 18);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "班组编号：";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(10, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 18);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "日期：";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(246)))), ((int)(((byte)(253)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv.ColumnFont = null;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle8;
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1087, 251);
            this.dgv.TabIndex = 16;
            this.dgv.TitleBack = null;
            this.dgv.TitleBackColorBegin = System.Drawing.Color.White;
            this.dgv.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(196)))), ((int)(((byte)(242)))));
            // 
            // Frm2CX_CX_06JJ_GR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 310);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "Frm2CX_CX_06JJ_GR";
            this.Text = "成型计件——个人";
            this.Load += new System.EventHandler(this.FrmCX_CX_03JJ_GR_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX btnRecount;
        private DevComponents.DotNetBar.ButtonX btnExportExcel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.ComboBox CmbBZName;
        private System.Windows.Forms.ComboBox CmbBZ;
        private DevComponents.DotNetBar.LabelX lblPg;
        private DevComponents.DotNetBar.LabelX lblLb2;
        private System.Windows.Forms.DateTimePicker dtp;
        private DevComponents.DotNetBar.LabelX lblLb1;
        private DevComponents.DotNetBar.LabelX lblMc2;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.ComboBox CmbGH;
        private DevComponents.DotNetBar.LabelX labelX2;
        private CCWin.SkinControl.SkinDataGridView dgv;
    }
}