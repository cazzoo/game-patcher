using System;
using System.ComponentModel;
using Gtk;
using System.Net;

namespace RFUpdater.Utils
{
	class Networking
	{
		private static int NetworkAction;
		public static void CheckNetwork (int Action)
		{
			NetworkAction = Action;
			Common.ChangeStatus (Texts.Keys.CONNECTING);

			BackgroundWorker backgroundWorker = new BackgroundWorker ();

			backgroundWorker.DoWork += backgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

			if (backgroundWorker.IsBusy) {
				Common.ShowMessageBox(Texts.Keys.UNKNOWNERROR, "CheckNetwork isBusy");
			} else {
				backgroundWorker.RunWorkerAsync ();
			}
		}

		private static void backgroundWorker_DoWork (object sender, DoWorkEventArgs e)
		{
			try {
				WebClient client = new WebClient ();
				client.OpenRead (Globals.ServerURL);

				e.Result = true;
			} catch {
				e.Result = false;
			}
		}

		private static void backgroundWorker_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			if (!Convert.ToBoolean (e.Result)) {
				Common.ShowMessageBox(Texts.Keys.NONETWORK);
			} else {
				// Launch the next action regarding the action set in parameters
				if (NetworkAction == Globals.ACTION_DOWNLOAD_DEFINITIONS) {
					ModuleDefinitionDownloader.DownloadModuleDefinitions ();
				} else if (NetworkAction == Globals.ACTION_DOWNLOAD_MODULES) {
					ModuleDownloader.DownloadModuleFiles ();
				}
			}
		}
	}
}
