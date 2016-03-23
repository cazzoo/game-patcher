using RFUpdater.Patcher.Source_files;
using System;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    public partial class pForm : Form
    {
        public pForm()
        {
            InitializeComponent();

            Globals.pForm = this;
            Globals.currentForm = this;
        }

        private void pForm_Shown(object sender, EventArgs e)
        {
            Networking.CheckNetwork(Globals.NETWORK_DOWNLOAD_FILES);
        }
    }
}
