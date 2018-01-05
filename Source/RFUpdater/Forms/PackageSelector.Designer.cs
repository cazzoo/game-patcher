namespace RFUpdater.Patcher
{
    partial class PackageSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageSelector));
            this.Quit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.packageList = new System.Windows.Forms.ListBox();
            this.synchPackage = new System.Windows.Forms.Button();
            this.startAMS = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Location = new System.Drawing.Point(532, 393);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(80, 23);
            this.Quit.TabIndex = 6;
            this.Quit.Text = "Quit";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 83);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // packageList
            // 
            this.packageList.FormattingEnabled = true;
            this.packageList.Location = new System.Drawing.Point(12, 101);
            this.packageList.Name = "packageList";
            this.packageList.Size = new System.Drawing.Size(600, 277);
            this.packageList.TabIndex = 8;
            this.packageList.SelectedIndexChanged += new System.EventHandler(this.selectionChanged);
            // 
            // synchPackage
            // 
            this.synchPackage.Enabled = false;
            this.synchPackage.Location = new System.Drawing.Point(446, 393);
            this.synchPackage.Name = "synchPackage";
            this.synchPackage.Size = new System.Drawing.Size(80, 23);
            this.synchPackage.TabIndex = 9;
            this.synchPackage.Text = "Synchronize";
            this.synchPackage.UseVisualStyleBackColor = true;
            this.synchPackage.Click += new System.EventHandler(this.synchPackage_Click);
            // 
            // startAMS
            // 
            this.startAMS.Location = new System.Drawing.Point(360, 393);
            this.startAMS.Name = "startAMS";
            this.startAMS.Size = new System.Drawing.Size(80, 23);
            this.startAMS.TabIndex = 10;
            this.startAMS.Text = "Start AMS";
            this.startAMS.UseVisualStyleBackColor = true;
            this.startAMS.Click += new System.EventHandler(this.startAMS_Click);
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
            // PackageSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.startAMS);
            this.Controls.Add(this.synchPackage);
            this.Controls.Add(this.packageList);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Quit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PackageSelector";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Racing-France AutoUpdater (v1.3)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button Quit;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.ListBox packageList;
        public System.Windows.Forms.Button synchPackage;
        public System.Windows.Forms.Button startAMS;
        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel Status;
    }
}