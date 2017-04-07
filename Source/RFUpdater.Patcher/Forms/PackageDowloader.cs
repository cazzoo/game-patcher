using RFUpdater.Patcher.Source_files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    public partial class PackageDownloader : Form
    {

        public PackageDownloader()
        {
            InitializeComponent();

            Globals.OldFiles = new List<string>();
            Globals.Files = new List<Globals.File>();

            Globals.packageDownloader = this;
            Globals.toolStrip = this.Status;
        }

        private void pForm_Shown(object sender, EventArgs e)
        {
            Networking.CheckNetwork();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PackageDownloader_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void startAMS_Click(object sender, EventArgs e)
        {
            Starter.Start();
            Application.Exit();
        }

        private void selectMods_Click(object sender, EventArgs e)
        {
            Globals.packageSelector.Show();
            Common.EnableStart();
            this.Dispose();
        }
    }

}
