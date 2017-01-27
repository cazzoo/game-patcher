using System;
using Gtk;
using System.ComponentModel;
using System.IO;

namespace RFUpdater.Utils
{
    class FileChecker
    {
        enum State
        {
            REPORT_NAME     = 0,
            REPORT_PROGRESS = 1
        }

        private static BackgroundWorker backgroundWorker = new BackgroundWorker();
		private static int NetworkAction;

        public static void CheckFiles(int Action)
        {
            backgroundWorker.WorkerReportsProgress = true;
			NetworkAction = Action;

            backgroundWorker.DoWork              += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged     += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted  += backgroundWorker_RunWorkerCompleted;

            if (backgroundWorker.IsBusy)
            {
                Application.Quit();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (Globals.File file in Globals.Files)
            {
                Globals.FullSize += file.Size;

                backgroundWorker.ReportProgress((int)State.REPORT_NAME, Path.GetFileName(file.Name));

				var basePath = String.Empty;
				if (Globals.ACTION_DOWNLOAD_DEFINITIONS == NetworkAction)
				{
					basePath = Globals.LocalModuleDefinitionFolder;
				}
				else {
					basePath = Globals.GameBasePath;
				}

                if (!File.Exists(basePath + file.Name) || Common.GetHash(basePath + file.Name) != file.Hash)
                {
                    Globals.OldFiles.Add(file.Name);
                }
                else
                {
                    Globals.CompleteSize += file.Size;
                    backgroundWorker.ReportProgress((int)State.REPORT_PROGRESS);
                }
            }
        }

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == (int)State.REPORT_NAME)
            {
                Common.ChangeStatus(Texts.Keys.CHECKFILE, e.UserState.ToString());
            }
            else
            {
                Common.UpdateCompleteProgress(Computer.Compute(Globals.CompleteSize));
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileDownloader.DownloadFile(NetworkAction);
        }
    }
}
