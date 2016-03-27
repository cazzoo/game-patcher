using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RFUpdater.Patcher.Source_files
{
    class ListDownloader
    {
        public static void DownloadList()
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

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Retrieving module files
            WebRequest request = WebRequest.Create(Globals.ServerURL + Globals.ModulesListFolder);
            WebResponse response = request.GetResponse();
            Regex regex = new Regex("<a href=\"[^\"]+\">(?<name>(?!../).*?)</a>");
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();

                MatchCollection matches = regex.Matches(result);
                if (matches.Count == 0)
                {
                    MessageBox.Show(Texts.GetText(Texts.Keys.UNKNOWNERROR, "DownloadList isBusy"));
                    return;
                }

                foreach (Match match in matches)
                {
                    if (!match.Success) { continue; }
                    string name = match.Groups["name"].Value;
					Globals.Patchlist.Add(System.IO.Path.GetFileNameWithoutExtension(name));
                }
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Globals.moduleList.chklist_modules.DataSource = Globals.Patchlist;
            // Adding modules to list
            // Checking previously selected modules
        }

        public static void DownloadFiles()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            Common.ChangeStatus(Texts.Keys.LISTDOWNLOAD);

            backgroundWorker.DoWork += backgroundWorker_files_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_files_RunWorkerCompleted;

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

        private static void backgroundWorker_files_DoWork(object sender, DoWorkEventArgs e)
        {
            // Processing each files

            WebClient webClient = new WebClient();

            foreach(object checkedItem in Globals.moduleList.chklist_modules.CheckedItems)
            {
                Stream stream = webClient.OpenRead(Globals.ServerURL + checkedItem.ToString() + ".txt");
                StreamReader streamReader = new StreamReader(stream);

                while (!streamReader.EndOfStream)
                {
                    ListProcessor.AddFile(streamReader.ReadLine());
                }

            }
        }

        private static void backgroundWorker_files_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileChecker.CheckFiles();
        }
    }
}
