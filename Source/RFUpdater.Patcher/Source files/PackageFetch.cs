using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections;

namespace RFUpdater.Patcher.Source_files
{
    class PackageFetch
    {
        public static void CheckNetwork()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            Common.ChangeStatus(Texts.Keys.LISTDOWNLOAD);

            backgroundWorker.DoWork              += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted  += backgroundWorker_RunWorkerCompleted;

            if(backgroundWorker.IsBusy)
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
                IList listPackages = new List<string>();
                HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(Globals.ServerURL);
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]")) {
                    if(link.InnerText != "../") {
                        listPackages.Add(link.InnerText.Remove(link.InnerText.Length -1));
                    }
                }
                Globals.packageSelector.packageList.DataSource = listPackages;
            }
            Common.ChangeStatus(Texts.Keys.LISTDOWNLOADCOMPLETED);
        }
    }
}
