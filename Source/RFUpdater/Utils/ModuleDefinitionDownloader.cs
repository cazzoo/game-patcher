using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using Gtk;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace RFUpdater.Utils
{
	class ModuleDefinitionDownloader
	{
		public static void DownloadModuleDefinitions()
		{
			BackgroundWorker backgroundWorker = new BackgroundWorker();

			Common.ChangeStatus(Texts.Keys.DEFINITIONSDOWNLOAD);

			backgroundWorker.DoWork              += backgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted  += backgroundWorker_RunWorkerCompleted;

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
			// Retrieving module definition files
			string url = Globals.ServerURL + Globals.RemoteModuleDefinitionFolder;
			var web = new HtmlWeb();
			HtmlDocument doc = web.Load(url);

			var links = doc.DocumentNode.Descendants("a");
			var listModuleNames = new List<string>();

			// Ignoring the first 5 links that are related to apache directory actions
			int ignoredLinks = 5;
			foreach(var link in links)
			{
				if (ignoredLinks > 0)
				{
					ignoredLinks--;
				}
				else {
					listModuleNames.Add(link.InnerText);
				}
			}

			var webClient = new WebClient();

			foreach(string ModuleName in listModuleNames)
            {
				ListProcessor.AddFile(ModuleName);
            }
		}

		private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			FileChecker.CheckFiles(Globals.ACTION_DOWNLOAD_DEFINITIONS);
		}
	}
}
