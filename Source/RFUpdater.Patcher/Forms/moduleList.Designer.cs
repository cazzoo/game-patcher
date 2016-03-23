namespace RFUpdater.Patcher
{
    partial class moduleList
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.chklist_modules = new System.Windows.Forms.CheckedListBox();
            this.btn_synchronize = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btn_parameters = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_about = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // 
            // chklist_modules
            // 
            this.chklist_modules.FormattingEnabled = true;
            this.chklist_modules.Location = new System.Drawing.Point(12, 38);
            this.chklist_modules.Name = "chklist_modules";
            this.chklist_modules.Size = new System.Drawing.Size(449, 349);
            this.chklist_modules.TabIndex = 0;
            // 
            // btn_synchronize
            // 
            this.btn_synchronize.Location = new System.Drawing.Point(12, 393);
            this.btn_synchronize.Name = "btn_synchronize";
            this.btn_synchronize.Size = new System.Drawing.Size(449, 23);
            this.btn_synchronize.TabIndex = 1;
            this.btn_synchronize.Text = "Synchronize";
            this.btn_synchronize.UseVisualStyleBackColor = true;
            this.btn_synchronize.Click += new System.EventHandler(this.btn_synchronize_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_parameters,
            this.btn_about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(473, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btn_parameters
            // 
            this.btn_parameters.Name = "btn_parameters";
            this.btn_parameters.Size = new System.Drawing.Size(76, 20);
            this.btn_parameters.Text = "parameters";
            // 
            // btn_about
            // 
            this.btn_about.Name = "btn_about";
            this.btn_about.Size = new System.Drawing.Size(50, 20);
            this.btn_about.Text = "about";
            // 
            // moduleList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 425);
            this.Controls.Add(this.btn_synchronize);
            this.Controls.Add(this.chklist_modules);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "moduleList";
            this.Text = "Module selection";
            this.Shown += new System.EventHandler(this.moduleList_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.Button btn_synchronize;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btn_parameters;
        private System.Windows.Forms.ToolStripMenuItem btn_about;
        public System.Windows.Forms.CheckedListBox chklist_modules;
    }
}