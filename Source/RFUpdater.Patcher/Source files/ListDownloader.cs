using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RFUpdater.Patcher.Source_files
{
    class WebClientWithTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = 5000;
            return wr;
        }
    }

    class ListDownloader
    {
        public void DownloadList()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            Common.ChangeStatus(Texts.Keys.LISTDOWNLOAD);

            backgroundWorker.DoWork              += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted  += backgroundWorker_RunWorkerCompleted;

            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(Texts.GetText(Texts.Keys.UNKNOWNERROR, "DownloadList isBusy"));
                Application.Exit();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 25;
            Globals.Files = new List<Globals.File>();
            Globals.OldFiles = new List<String>();

            Globals.SelectedPackageFolder = Globals.ServerURL + Globals.selectedPackage + Path.AltDirectorySeparatorChar;
            String patchFile = Globals.SelectedPackageFolder + Globals.selectedPackage + ".txt";

            try
            {
                WebClient webClient = new WebClientWithTimeout();
                Stream stream = webClient.OpenRead(Uri.EscapeUriString(patchFile));

                StreamReader streamReader = new StreamReader(stream);

                while (!streamReader.EndOfStream)
                {
                    ListProcessor.AddFile(streamReader.ReadLine());
                }
            }
            catch
            {
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileChecker fchecker = new FileChecker();
            fchecker.CheckFiles();
        }
    }
}
