using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace RFUpdater.Patcher.Source_files
{
    class FileDownloader
    {
        private static int      curFile;
        private static long     lastBytes;
        private static long     currentBytes;

        private Stopwatch stopWatch = new Stopwatch();

        public void DownloadFile()
        {
            if(Globals.OldFiles.Count <= 0)
            {
                Common.ChangeStatus(Texts.Keys.CHECKCOMPLETE);
                Common.EnableStart();
                return;
            }

            if (curFile >= Globals.OldFiles.Count)
            {
                Common.ChangeStatus(Texts.Keys.DOWNLOADCOMPLETE);
                Common.EnableStart();
                return;
            }

            if (Globals.OldFiles[curFile].Contains("/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Globals.OldFiles[curFile]));
            }

            WebClient webClient = new WebClient();

            webClient.DownloadProgressChanged   += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);

            webClient.DownloadFileCompleted     += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            stopWatch.Start();

            webClient.DownloadFileAsync(new Uri(Globals.SelectedPackageFolder + Globals.OldFiles[curFile]), Globals.OldFiles[curFile]);
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentBytes = lastBytes + e.BytesReceived;

            var fileLength = Globals.OldFiles[curFile].Length;
            var maxLength = 50;
            Common.ChangeStatus(Texts.Keys.DOWNLOADFILE, (fileLength <= maxLength) ? Globals.OldFiles[curFile] : "..." + Globals.OldFiles[curFile].Substring(fileLength - maxLength), Computer.ComputeDownloadSize(e.BytesReceived).ToString("0.00"), Computer.ComputeDownloadSize(e.TotalBytesToReceive).ToString("0.00") + " MB");

            Common.UpdateCompleteProgress(Computer.Compute(Globals.CompleteSize + currentBytes));

            Common.UpdateCurrentProgress(e.ProgressPercentage, Computer.ComputeDownloadSpeed(e.BytesReceived, stopWatch));
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lastBytes = currentBytes;

            Common.UpdateCurrentProgress(100, 0);

            curFile++;

            stopWatch.Reset();

            DownloadFile();
        }
    }
}
