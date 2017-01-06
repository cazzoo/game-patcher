using System;
using System.ComponentModel;
using System.Net;
using Gtk;
using System.Text.RegularExpressions;
using System.IO;

namespace RFUpdater.Utils
{
    class ModuleDownloader
    {
        public static void DownloadModuleFiles()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            Common.ChangeStatus(Texts.Keys.DEFINITIONSDOWNLOAD);

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            if (backgroundWorker.IsBusy)
            {
				Common.ShowMessageBox(MessageType.Warning, Texts.Keys.UNKNOWNERROR, "DownloadList isBusy");
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Processing each files

            WebClient webClient = new WebClient();

            /*foreach(object checkedItem in Globals.moduleList.chklist_modules.CheckedItems)
            {
                Stream stream = webClient.OpenRead(Globals.ServerURL + checkedItem.ToString() + ".txt");
                StreamReader streamReader = new StreamReader(stream);

                while (!streamReader.EndOfStream)
                {
                    ListProcessor.AddFile(streamReader.ReadLine());
                }

            }*/
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileChecker.CheckFiles();
        }
    }
}
