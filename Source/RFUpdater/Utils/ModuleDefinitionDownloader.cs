using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

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
				Common.ShowMessageBox(Texts.Keys.UNKNOWNERROR, "DownloadList isBusy");
			}
			else
			{
				backgroundWorker.RunWorkerAsync();
			}
		}

		private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Retrieving module definition files
			WebRequest request = WebRequest.Create(Globals.ServerURL + Globals.ModuleDefinitionFolder);
			WebResponse response = request.GetResponse();
			Regex regex = new Regex("<a href=\"[^\"]+\">(?<name>(?!../).*?)</a>");
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				string result = reader.ReadToEnd();

				MatchCollection matches = regex.Matches(result);
				if (matches.Count == 0)
				{
					Common.ChangeStatus (Texts.Keys.UNKNOWNERROR, "DownloadList isBusy");
					return;
				}

				foreach (Match match in matches)
				{
					if (!match.Success) { continue; }
					string name = match.Groups["name"].Value;
					Globals.ModuleNameList.Add(System.IO.Path.GetFileNameWithoutExtension(name));
				}
			}
		}

		private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//Globals.moduleList.chklist_modules.DataSource = Globals.Patchlist;
			// Adding modules to list
			// Checking previously selected modules
		}
	}
}
