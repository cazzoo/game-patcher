using RFUpdater.Patcher.Source_files;
using System;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    public partial class PackageSelector : Form
    {
        public PackageSelector()
        {
            InitializeComponent();

            Globals.packageSelector = this;
            Globals.toolStrip = this.Status;

            PackageFetch.CheckNetwork();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            Globals.selectedPackage = (sender as ListBox).SelectedItem.ToString();
            synchPackage.Enabled = true;
        }

        private void synchPackage_Click(object sender, EventArgs e)
        {
            Form packageDownload = new PackageDownloader();
            this.Hide();
            packageDownload.Show();
        }

        private void startAMS_Click(object sender, EventArgs e)
        {
            Starter.Start();
            Application.Exit();
        }
    }
}