namespace RFUpdater.Patcher
{
    partial class loginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 148);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(460, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Status";
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(0, 17);

            this.btn_login = new System.Windows.Forms.Button();
            this.input_login = new System.Windows.Forms.TextBox();
            this.lbl_login = new System.Windows.Forms.Label();
            this.input_password = new System.Windows.Forms.TextBox();
            this.lbl_password = new System.Windows.Forms.Label();
            this.lbl_login_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(71, 64);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 0;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.ActionLogin);
            // 
            // input_login
            // 
            this.input_login.Location = new System.Drawing.Point(71, 12);
            this.input_login.Name = "input_login";
            this.input_login.Size = new System.Drawing.Size(170, 20);
            this.input_login.TabIndex = 1;
            // 
            // lbl_login
            // 
            this.lbl_login.AutoSize = true;
            this.lbl_login.Location = new System.Drawing.Point(13, 12);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(33, 13);
            this.lbl_login.TabIndex = 2;
            this.lbl_login.Text = "Login";
            // 
            // input_password
            // 
            this.input_password.Location = new System.Drawing.Point(71, 38);
            this.input_password.Name = "input_password";
            this.input_password.Size = new System.Drawing.Size(170, 20);
            this.input_password.TabIndex = 3;
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(12, 38);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(53, 13);
            this.lbl_password.TabIndex = 4;
            this.lbl_password.Text = "Password";
            // 
            // lbl_login_status
            // 
            this.lbl_login_status.Location = new System.Drawing.Point(13, 99);
            this.lbl_login_status.Name = "lbl_login_status";
            this.lbl_login_status.Size = new System.Drawing.Size(225, 19);
            this.lbl_login_status.TabIndex = 5;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 136);
            this.Controls.Add(this.lbl_login_status);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.input_password);
            this.Controls.Add(this.lbl_login);
            this.Controls.Add(this.input_login);
            this.Controls.Add(this.btn_login);
            this.Name = "Login";
            this.Text = "RF-Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox input_login;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.TextBox input_password;
        private System.Windows.Forms.Label lbl_password;
        private System.Windows.Forms.Label lbl_login_status;
    }
}