namespace RFUpdater.Patcher
{
    partial class PackageDownloader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageDownloader));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.completeProgress = new System.Windows.Forms.ProgressBar();
            this.currentProgress = new System.Windows.Forms.ProgressBar();
            this.completeProgressText = new System.Windows.Forms.Label();
            this.currentProgressText = new System.Windows.Forms.Label();
            this.quit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.startAMS = new System.Windows.Forms.Button();
            this.selectMods = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar;
            this.statusStrip.AutoSize = false;
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 419);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(624, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Status";
            // 
            // Status
            // 
            this.Status.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(609, 17);
            this.Status.Spring = true;
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // completeProgress
            // 
            this.completeProgress.BackColor = System.Drawing.SystemColors.Control;
            this.completeProgress.Location = new System.Drawing.Point(12, 175);
            this.completeProgress.Name = "completeProgress";
            this.completeProgress.Size = new System.Drawing.Size(600, 50);
            this.completeProgress.TabIndex = 1;
            // 
            // currentProgress
            // 
            this.currentProgress.ForeColor = System.Drawing.Color.YellowGreen;
            this.currentProgress.Location = new System.Drawing.Point(12, 293);
            this.currentProgress.Name = "currentProgress";
            this.currentProgress.Size = new System.Drawing.Size(600, 50);
            this.currentProgress.TabIndex = 2;
            // 
            // completeProgressText
            // 
            this.completeProgressText.AutoSize = true;
            this.completeProgressText.Location = new System.Drawing.Point(529, 159);
            this.completeProgressText.Name = "completeProgressText";
            this.completeProgressText.Size = new System.Drawing.Size(83, 13);
            this.completeProgressText.TabIndex = 3;
            this.completeProgressText.Text = "Full process: 0%";
            // 
            // currentProgressText
            // 
            this.currentProgressText.AutoSize = true;
            this.currentProgressText.Location = new System.Drawing.Point(453, 277);
            this.currentProgressText.Name = "currentProgressText";
            this.currentProgressText.Size = new System.Drawing.Size(159, 13);
            this.currentProgressText.TabIndex = 4;
            this.currentProgressText.Text = "Per file process: 0%  |  0.00 kb/s";
            // 
            // quit
            // 
            this.quit.Enabled = false;
            this.quit.Location = new System.Drawing.Point(532, 393);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(80, 23);
            this.quit.TabIndex = 5;
            this.quit.Text = "Quit";
            this.quit.UseVisualStyleBackColor = true;
            this.quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 83);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // startAMS
            // 
            this.startAMS.Enabled = false;
            this.startAMS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.startAMS.Location = new System.Drawing.Point(360, 393);
            this.startAMS.Name = "startAMS";
            this.startAMS.Size = new System.Drawing.Size(80, 23);
            this.startAMS.TabIndex = 11;
            this.startAMS.Text = "Start AMS";
            this.startAMS.UseVisualStyleBackColor = true;
            this.startAMS.Click += new System.EventHandler(this.startAMS_Click);
            // 
            // selectMods
            // 
            this.selectMods.Enabled = false;
            this.selectMods.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.selectMods.Location = new System.Drawing.Point(446, 393);
            this.selectMods.Name = "selectMods";
            this.selectMods.Size = new System.Drawing.Size(80, 23);
            this.selectMods.TabIndex = 12;
            this.selectMods.Text = "Select mods";
            this.selectMods.UseVisualStyleBackColor = true;
            this.selectMods.Click += new System.EventHandler(this.selectMods_Click);
            // 
            // PackageDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.selectMods);
            this.Controls.Add(this.startAMS);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.currentProgressText);
            this.Controls.Add(this.completeProgressText);
            this.Controls.Add(this.currentProgress);
            this.Controls.Add(this.completeProgress);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 480);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "PackageDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFUpdater";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PackageDownloader_FormClosed);
            this.Shown += new System.EventHandler(this.pForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip            statusStrip;
        public  System.Windows.Forms.ToolStripStatusLabel   Status;
        public  System.Windows.Forms.ProgressBar            completeProgress;
        public  System.Windows.Forms.ProgressBar            currentProgress;
        public  System.Windows.Forms.Label                  completeProgressText;
        public  System.Windows.Forms.Label                  currentProgressText;
        public  System.Windows.Forms.Button                 quit;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button startAMS;
        public System.Windows.Forms.Button selectMods;
    }
}

