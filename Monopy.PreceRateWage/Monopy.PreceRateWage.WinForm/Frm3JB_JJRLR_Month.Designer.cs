namespace Monopy.PreceRateWage.WinForm
{
    partial class Frm3JB_JJRLR_Month
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblWX = new DevComponents.DotNetBar.LabelX();
            this.lblKF = new DevComponents.DotNetBar.LabelX();
            this.lblPG = new DevComponents.DotNetBar.LabelX();
            this.lblPMC = new DevComponents.DotNetBar.LabelX();
            this.btnExportExcel = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.BtnPGCheck = new DevComponents.DotNetBar.ButtonX();
            this.BtnPMCCheck = new DevComponents.DotNetBar.ButtonX();
            this.BtnWXCheck = new DevComponents.DotNetBar.ButtonX();
            this.BtnKFCheck = new DevComponents.DotNetBar.ButtonX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dgv = new CCWin.SkinControl.SkinDataGridView();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
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
            this.dtp.TabIndex = 1;
            this.dtp.ValueChanged += new System.EventHandler(this.Dtp_ValueChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.groupPanel1.Controls.Add(this.lblWX);
            this.groupPanel1.Controls.Add(this.lblKF);
            this.groupPanel1.Controls.Add(this.lblPG);
            this.groupPanel1.Controls.Add(this.lblPMC);
            this.groupPanel1.Controls.Add(this.btnExportExcel);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.BtnPGCheck);
            this.groupPanel1.Controls.Add(this.dtp);
            this.groupPanel1.Controls.Add(this.BtnPMCCheck);
            this.groupPanel1.Controls.Add(this.BtnWXCheck);
            this.groupPanel1.Controls.Add(this.BtnKFCheck);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(817, 90);
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
            // lblWX
            // 
            this.lblWX.AutoSize = true;
            this.lblWX.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblWX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblWX.Location = new System.Drawing.Point(292, 40);
            this.lblWX.Name = "lblWX";
            this.lblWX.Size = new System.Drawing.Size(31, 18);
            this.lblWX.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblWX.TabIndex = 23;
            this.lblWX.Text = "外销";
            // 
            // lblKF
            // 
            this.lblKF.AutoSize = true;
            this.lblKF.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblKF.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblKF.Location = new System.Drawing.Point(196, 40);
            this.lblKF.Name = "lblKF";
            this.lblKF.Size = new System.Drawing.Size(31, 18);
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
            this.lblPG.Location = new System.Drawing.Point(100, 40);
            this.lblPG.Name = "lblPG";
            this.lblPG.Size = new System.Drawing.Size(31, 18);
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
            this.lblPMC.Location = new System.Drawing.Point(10, 41);
            this.lblPMC.Name = "lblPMC";
            this.lblPMC.Size = new System.Drawing.Size(25, 16);
            this.lblPMC.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblPMC.TabIndex = 20;
            this.lblPMC.Text = "PMC";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExcel.Location = new System.Drawing.Point(236, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(87, 28);
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
            this.labelX2.Location = new System.Drawing.Point(450, 38);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(338, 23);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 16;
            this.labelX2.Text = "注意：数据不正确，不要审核！请联系人资。";
            // 
            // BtnPGCheck
            // 
            this.BtnPGCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnPGCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnPGCheck.Location = new System.Drawing.Point(450, 3);
            this.BtnPGCheck.Name = "BtnPGCheck";
            this.BtnPGCheck.Size = new System.Drawing.Size(116, 28);
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
            this.BtnPMCCheck.Location = new System.Drawing.Point(329, 3);
            this.BtnPMCCheck.Name = "BtnPMCCheck";
            this.BtnPMCCheck.Size = new System.Drawing.Size(116, 28);
            this.BtnPMCCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnPMCCheck.Symbol = "";
            this.BtnPMCCheck.TabIndex = 14;
            this.BtnPMCCheck.Text = "PMC经理审核";
            this.BtnPMCCheck.Click += new System.EventHandler(this.BtnPMCCheck_Click);
            // 
            // BtnWXCheck
            // 
            this.BtnWXCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnWXCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnWXCheck.Location = new System.Drawing.Point(692, 3);
            this.BtnWXCheck.Name = "BtnWXCheck";
            this.BtnWXCheck.Size = new System.Drawing.Size(116, 28);
            this.BtnWXCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnWXCheck.Symbol = "";
            this.BtnWXCheck.TabIndex = 12;
            this.BtnWXCheck.Text = "外销经理审核";
            this.BtnWXCheck.Click += new System.EventHandler(this.BtnWXCheck_Click);
            // 
            // BtnKFCheck
            // 
            this.BtnKFCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnKFCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.BtnKFCheck.Location = new System.Drawing.Point(571, 3);
            this.BtnKFCheck.Name = "BtnKFCheck";
            this.BtnKFCheck.Size = new System.Drawing.Size(116, 28);
            this.BtnKFCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnKFCheck.Symbol = "";
            this.BtnKFCheck.TabIndex = 11;
            this.BtnKFCheck.Text = "开发经理审核";
            this.BtnKFCheck.Click += new System.EventHandler(this.BtnKFCheck_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.Location = new System.Drawing.Point(140, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 28);
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
            this.dgv.Location = new System.Drawing.Point(0, 90);
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
            this.dgv.Size = new System.Drawing.Size(817, 343);
            this.dgv.TabIndex = 6;
            this.dgv.TitleBack = null;
            this.dgv.TitleBackColorBegin = System.Drawing.Color.White;
            this.dgv.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(196)))), ((int)(((byte)(242)))));
            // 
            // Frm3JB_JJRLR_Month
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 433);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "Frm3JB_JJRLR_Month";
            this.Text = "三厂检包计件月汇总审核";
            this.Load += new System.EventHandler(this.Frm3JB_JJRLR_Month_Load);
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
        private DevComponents.DotNetBar.ButtonX BtnWXCheck;
        private DevComponents.DotNetBar.ButtonX BtnKFCheck;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private CCWin.SkinControl.SkinDataGridView dgv;
        private DevComponents.DotNetBar.ButtonX btnExportExcel;
        private DevComponents.DotNetBar.LabelX lblWX;
        private DevComponents.DotNetBar.LabelX lblKF;
        private DevComponents.DotNetBar.LabelX lblPG;
        private DevComponents.DotNetBar.LabelX lblPMC;
    }
}