namespace Monopy.PreceRateWage.WinForm
{
    partial class Frm1JB_JJRLR_Month
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblKF = new DevComponents.DotNetBar.LabelX();
            this.lblPG = new DevComponents.DotNetBar.LabelX();
            this.lblPMC = new DevComponents.DotNetBar.LabelX();
            this.btnExportExcel = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.BtnPGCheck = new DevComponents.DotNetBar.ButtonX();
            this.BtnPMCCheck = new DevComponents.DotNetBar.ButtonX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dgv = new CCWin.SkinControl.SkinDataGridView();
            this.BtnKFCheck = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dtp
            // 
            this.dtp.CustomFormat = "yyyy-MM";
            this.dtp.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp.Location = new System.Drawing.Point(72, 5);
            this.dtp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtp.Name = "dtp";
            this.dtp.ShowUpDown = true;
            this.dtp.Size = new System.Drawing.Size(113, 30);
            this.dtp.TabIndex = 1;
            this.dtp.ValueChanged += new System.EventHandler(this.Dtp_ValueChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.groupPanel1.Controls.Add(this.lblKF);
            this.groupPanel1.Controls.Add(this.lblPG);
            this.groupPanel1.Controls.Add(this.lblPMC);
            this.groupPanel1.Controls.Add(this.btnExportExcel);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.BtnPGCheck);
            this.groupPanel1.Controls.Add(this.dtp);
            this.groupPanel1.Controls.Add(this.BtnPMCCheck);
            this.groupPanel1.Controls.Add(this.BtnKFCheck);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(1089, 112);
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
            this.groupPanel1.TabIndex = 4;
            this.groupPanel1.Text = "查询和审核";
            // 
            // lblKF
            // 
            this.lblKF.AutoSize = true;
            this.lblKF.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblKF.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblKF.Location = new System.Drawing.Point(261, 50);
            this.lblKF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblKF.Name = "lblKF";
            this.lblKF.Size = new System.Drawing.Size(38, 21);
            this.lblKF.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblKF.TabIndex = 22;
            this.lblKF.Text = "开发";
            // 
            // lblPG
            // 
            this.lblPG.AutoSize = true;
            this.lblPG.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPG.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPG.Location = new System.Drawing.Point(133, 50);
            this.lblPG.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblPG.Name = "lblPG";
            this.lblPG.Size = new System.Drawing.Size(38, 21);
            this.lblPG.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPG.TabIndex = 21;
            this.lblPG.Text = "品管";
            // 
            // lblPMC
            // 
            this.lblPMC.AutoSize = true;
            this.lblPMC.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPMC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPMC.Location = new System.Drawing.Point(13, 51);
            this.lblPMC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblPMC.Name = "lblPMC";
            this.lblPMC.Size = new System.Drawing.Size(31, 19);
            this.lblPMC.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPMC.TabIndex = 20;
            this.lblPMC.Text = "PMC";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExcel.Location = new System.Drawing.Point(315, 4);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(116, 35);
            this.btnExportExcel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExportExcel.Symbol = "";
            this.btnExportExcel.TabIndex = 17;
            this.btnExportExcel.Text = "导出";
            this.btnExportExcel.Click += new System.EventHandler(this.BtnExportExcel_Click);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.ForeColor = System.Drawing.Color.Red;
            this.labelX2.Location = new System.Drawing.Point(600, 48);
            this.labelX2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(422, 28);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 16;
            this.labelX2.Text = "注意：数据不正确，不要审核！请联系人资。";
            // 
            // BtnPGCheck
            // 
            this.BtnPGCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnPGCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnPGCheck.Location = new System.Drawing.Point(600, 4);
            this.BtnPGCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnPGCheck.Name = "BtnPGCheck";
            this.BtnPGCheck.Size = new System.Drawing.Size(155, 35);
            this.BtnPGCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnPGCheck.Symbol = "";
            this.BtnPGCheck.TabIndex = 15;
            this.BtnPGCheck.Text = "品管经理审核";
            this.BtnPGCheck.Click += new System.EventHandler(this.BtnPGCheck_Click);
            // 
            // BtnPMCCheck
            // 
            this.BtnPMCCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnPMCCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnPMCCheck.Location = new System.Drawing.Point(439, 4);
            this.BtnPMCCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnPMCCheck.Name = "BtnPMCCheck";
            this.BtnPMCCheck.Size = new System.Drawing.Size(155, 35);
            this.BtnPMCCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnPMCCheck.Symbol = "";
            this.BtnPMCCheck.TabIndex = 14;
            this.BtnPMCCheck.Text = "PMC经理审核";
            this.BtnPMCCheck.Click += new System.EventHandler(this.BtnPMCCheck_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.Location = new System.Drawing.Point(187, 4);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(116, 35);
            this.btnSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSearch.Symbol = "";
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 10);
            this.labelX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(54, 21);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "日期：";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(246)))), ((int)(((byte)(253)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv.ColumnFont = null;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv.HeadFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv.HeadSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.Location = new System.Drawing.Point(0, 112);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1089, 429);
            this.dgv.TabIndex = 6;
            this.dgv.TitleBack = null;
            this.dgv.TitleBackColorBegin = System.Drawing.Color.White;
            this.dgv.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(196)))), ((int)(((byte)(242)))));
            // 
            // BtnKFCheck
            // 
            this.BtnKFCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnKFCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnKFCheck.Location = new System.Drawing.Point(761, 4);
            this.BtnKFCheck.Margin = new System.Windows.Forms.Padding(4);
            this.BtnKFCheck.Name = "BtnKFCheck";
            this.BtnKFCheck.Size = new System.Drawing.Size(155, 35);
            this.BtnKFCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnKFCheck.Symbol = "";
            this.BtnKFCheck.TabIndex = 11;
            this.BtnKFCheck.Text = "开发经理审核";
            this.BtnKFCheck.Click += new System.EventHandler(this.BtnKFCheck_Click);
            // 
            // Frm1JB_JJRLR_Month
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 541);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Frm1JB_JJRLR_Month";
            this.Text = "一厂检包计件月汇总审核";
            this.Load += new System.EventHandler(this.Frm1JB_JJRLR_Month_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtp;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX BtnPGCheck;
        private DevComponents.DotNetBar.ButtonX BtnPMCCheck;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private CCWin.SkinControl.SkinDataGridView dgv;
        private DevComponents.DotNetBar.ButtonX btnExportExcel;
        private DevComponents.DotNetBar.LabelX lblKF;
        private DevComponents.DotNetBar.LabelX lblPG;
        private DevComponents.DotNetBar.LabelX lblPMC;
        private DevComponents.DotNetBar.ButtonX BtnKFCheck;
    }
}