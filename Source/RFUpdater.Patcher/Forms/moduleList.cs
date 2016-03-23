using RFUpdater.Patcher.Source_files;
using System;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    public partial class moduleList : Form
    {
        public moduleList()
        {
            InitializeComponent();

            Globals.moduleList = this;
            Globals.currentForm = this;
        }

        private void moduleList_Shown(object sender, EventArgs e)
        {
            Networking.CheckNetwork(Globals.NETWORK_DOWNLOAD_LIST);
        }

        private void btn_synchronize_Click(object sender, EventArgs e)
        {
            pForm pForm = new pForm();
            pForm.Show();
        }
    }
}
