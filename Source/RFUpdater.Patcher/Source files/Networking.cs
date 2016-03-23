using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace RFUpdater.Patcher.Source_files
{
    class Networking
    {
        public static void CheckNetwork(String action)
        {
            Common.ChangeStatus(Texts.Keys.CONNECTING);

            BackgroundWorker backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork              += backgroundWorker_DoWork;
            if(action == Globals.NETWORK_DOWNLOAD_LIST) { 
                backgroundWorker.RunWorkerCompleted  += backgroundWorker_RunWorkerCompleted;
            } else {
                backgroundWorker.RunWorkerCompleted += backgroundWorker_files_RunWorkerCompleted;
            }

            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(Texts.GetText(Texts.Keys.UNKNOWNERROR, "CheckNetwork isBusy"));
                Application.Exit();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                client.OpenRead(Globals.ServerURL);

                e.Result = true;
            }
            catch
            {
                e.Result = false;
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!Convert.ToBoolean(e.Result))
            {
                MessageBox.Show(Texts.GetText(Texts.Keys.NONETWORK));
                Application.Exit();
            }
            else
            {
                ListDownloader.DownloadList();
            }
        }

        private static void backgroundWorker_files_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Convert.ToBoolean(e.Result))
            {
                MessageBox.Show(Texts.GetText(Texts.Keys.NONETWORK));
                Application.Exit();
            }
            else
            {
                ListDownloader.DownloadFiles();
            }
        }
    }
}
