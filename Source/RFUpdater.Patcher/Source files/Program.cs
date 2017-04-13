using RFUpdater.Patcher.Source_files;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RFUpdater.Patcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (checkPassword())
                Application.Run(new PackageSelector());
            else
                Application.Exit();
        }

        private static Boolean checkPassword()
        {
            string password = "//RF//simulation";
            string inputPassword = string.Empty;
            DialogResult result = Dialogs.InputBox("Password", "Enter password", ref inputPassword);
            if (result == DialogResult.OK)
            {
                if (!password.Equals(inputPassword))
                {
                    Dialogs.ShowDialog("Wrong password entered.");
                    checkPassword();
                }
                else
                {
                    return true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return false;
            }
            else
            {
                checkPassword();
            }
            return false;
        }
    }
}
