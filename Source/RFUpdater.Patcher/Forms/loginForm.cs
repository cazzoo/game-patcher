using RFUpdater.Patcher.Source_files;
using System;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();

            Globals.loginForm = this;
            Globals.currentForm = this;
        }

        private void ActionLogin(object sender, EventArgs e)
        {
            if(this.input_login.Text.Equals("caz"))
            {
                Form moduleList = new moduleList();
                this.Hide();
                moduleList.Show();
            } else
            {
                lbl_login_status.Text = "Unable to login";
            }
        }
    }
}