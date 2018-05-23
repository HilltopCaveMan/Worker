namespace Monopy.PreceRateWage.WinForm
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.reflectionLabel1 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtUserCode = new CCWin.SkinControl.SkinTextBox();
            this.txtPwd = new CCWin.SkinControl.SkinTextBox();
            this.btnLogin = new CCWin.SkinControl.SkinButton();
            this.skinPictureBox1 = new CCWin.SkinControl.SkinPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // reflectionLabel1
            // 
            this.reflectionLabel1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.reflectionLabel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.reflectionLabel1.Font = new System.Drawing.Font("宋体", 22F);
            this.reflectionLabel1.Location = new System.Drawing.Point(0, 0);
            this.reflectionLabel1.Name = "reflectionLabel1";
            this.reflectionLabel1.Size = new System.Drawing.Size(762, 175);
            this.reflectionLabel1.TabIndex = 0;
            this.reflectionLabel1.TabStop = false;
            this.reflectionLabel1.Text = "<b><font size=\"+10\"><b>唐山梦牌瓷业</b><font color=\"#B02B2C\"><i><u><font color=\"#00B7EF" +
    "\">工资计算软件</font></u></i></font></font></b>";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("宋体", 12F);
            this.labelX1.Location = new System.Drawing.Point(26, 240);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(92, 26);
            this.labelX1.Symbol = "";
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "工号：";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("宋体", 12F);
            this.labelX2.Location = new System.Drawing.Point(305, 240);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(92, 26);
            this.labelX2.Symbol = "";
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "密码：";
            // 
            // txtUserCode
            // 
            this.txtUserCode.BackColor = System.Drawing.Color.Transparent;
            this.txtUserCode.DownBack = null;
            this.txtUserCode.Icon = null;
            this.txtUserCode.IconIsButton = false;
            this.txtUserCode.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUserCode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtUserCode.IsPasswordChat = '\0';
            this.txtUserCode.IsSystemPasswordChar = false;
            this.txtUserCode.Lines = new string[0];
            this.txtUserCode.Location = new System.Drawing.Point(118, 239);
            this.txtUserCode.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserCode.MaxLength = 6;
            this.txtUserCode.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtUserCode.MouseBack = null;
            this.txtUserCode.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUserCode.Multiline = false;
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.NormlBack = null;
            this.txtUserCode.Padding = new System.Windows.Forms.Padding(5);
            this.txtUserCode.ReadOnly = false;
            this.txtUserCode.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUserCode.Size = new System.Drawing.Size(156, 28);
            // 
            // 
            // 
            this.txtUserCode.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserCode.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserCode.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtUserCode.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtUserCode.SkinTxt.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserCode.SkinTxt.MaxLength = 6;
            this.txtUserCode.SkinTxt.Name = "BaseText";
            this.txtUserCode.SkinTxt.Size = new System.Drawing.Size(146, 22);
            this.txtUserCode.SkinTxt.TabIndex = 0;
            this.txtUserCode.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUserCode.SkinTxt.WaterText = "OA工号（需输入“M”）";
            this.txtUserCode.TabIndex = 0;
            this.txtUserCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtUserCode.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUserCode.WaterText = "OA工号（需输入“M”）";
            this.txtUserCode.WordWrap = true;
            // 
            // txtPwd
            // 
            this.txtPwd.BackColor = System.Drawing.Color.Transparent;
            this.txtPwd.DownBack = null;
            this.txtPwd.Icon = null;
            this.txtPwd.IconIsButton = false;
            this.txtPwd.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPwd.IsPasswordChat = '*';
            this.txtPwd.IsSystemPasswordChar = false;
            this.txtPwd.Lines = new string[0];
            this.txtPwd.Location = new System.Drawing.Point(397, 239);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(0);
            this.txtPwd.MaxLength = 32767;
            this.txtPwd.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPwd.MouseBack = null;
            this.txtPwd.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPwd.Multiline = false;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.NormlBack = null;
            this.txtPwd.Padding = new System.Windows.Forms.Padding(5);
            this.txtPwd.ReadOnly = false;
            this.txtPwd.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPwd.Size = new System.Drawing.Size(156, 28);
            // 
            // 
            // 
            this.txtPwd.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPwd.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPwd.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtPwd.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPwd.SkinTxt.Name = "BaseText";
            this.txtPwd.SkinTxt.PasswordChar = '*';
            this.txtPwd.SkinTxt.Size = new System.Drawing.Size(146, 22);
            this.txtPwd.SkinTxt.TabIndex = 0;
            this.txtPwd.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPwd.SkinTxt.WaterText = "OA密码";
            this.txtPwd.TabIndex = 1;
            this.txtPwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPwd.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPwd.WaterText = "OA密码";
            this.txtPwd.WordWrap = true;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLogin.DownBack = null;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.Location = new System.Drawing.Point(582, 227);
            this.btnLogin.MouseBack = null;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.NormlBack = null;
            this.btnLogin.Radius = 50;
            this.btnLogin.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnLogin.Size = new System.Drawing.Size(116, 52);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.TabStop = false;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // skinPictureBox1
            // 
            this.skinPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("skinPictureBox1.Image")));
            this.skinPictureBox1.Location = new System.Drawing.Point(230, 145);
            this.skinPictureBox1.Name = "skinPictureBox1";
            this.skinPictureBox1.Size = new System.Drawing.Size(277, 59);
            this.skinPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.skinPictureBox1.TabIndex = 5;
            this.skinPictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(762, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Copyright © 2018 梦牌信息中心";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 346);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skinPictureBox1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.reflectionLabel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎使用梦牌工资计算软件";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private CCWin.SkinControl.SkinTextBox txtUserCode;
        private CCWin.SkinControl.SkinTextBox txtPwd;
        private CCWin.SkinControl.SkinButton btnLogin;
        private CCWin.SkinControl.SkinPictureBox skinPictureBox1;
        private System.Windows.Forms.Label label1;
    }
}