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
            resources.ApplyResources(this.Quit, "Quit");
            this.Quit.Name = "Quit";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // packageList
            // 
            this.packageList.FormattingEnabled = true;
            resources.ApplyResources(this.packageList, "packageList");
            this.packageList.Name = "packageList";
            this.packageList.SelectedIndexChanged += new System.EventHandler(this.selectionChanged);
            // 
            // synchPackage
            // 
            resources.ApplyResources(this.synchPackage, "synchPackage");
            this.synchPackage.Name = "synchPackage";
            this.synchPackage.UseVisualStyleBackColor = true;
            this.synchPackage.Click += new System.EventHandler(this.synchPackage_Click);
            // 
            // startAMS
            // 
            resources.ApplyResources(this.startAMS, "startAMS");
            this.startAMS.Name = "startAMS";
            this.startAMS.UseVisualStyleBackColor = true;
            this.startAMS.Click += new System.EventHandler(this.startAMS_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar;
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;
            // 
            // Status
            // 
            resources.ApplyResources(this.Status, "Status");
            this.Status.Name = "Status";
            this.Status.Spring = true;
            // 
            // PackageSelector
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.startAMS);
            this.Controls.Add(this.synchPackage);
            this.Controls.Add(this.packageList);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Quit);
            this.MaximizeBox = false;
            this.Name = "PackageSelector";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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