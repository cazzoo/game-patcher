using System;
using Gtk;
using RFUpdater.Utils;

namespace RFUpdater
{
	public partial class MainWindow : Gtk.Window
	{
		public static INIFile settings;
		public static Statusbar statusbar;
		public static ProgressBar progressFile;
		public static ProgressBar progressOverall;

		public MainWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}

		protected void Init ()
		{
			settings = new INIFile ("RFCUpdater.ini", true, false);
			statusbar = status_main;
			progressFile = progress_file;
			progressOverall = progress_overall;
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void QuitApplication (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		protected void openSettingsWindow (object sender, EventArgs e)
		{
			SettingsWindow sw = new SettingsWindow ();
			sw.Show ();
		}

		protected void OnBtnActivateClicked (object sender, EventArgs e)
		{
			Common.ChangeStatus(Texts.Keys.DEVELOP, "CLICKED ON ACTIVATE");
		}

		protected void OnBtnDeactivateClicked (object sender, EventArgs e)
		{
			Common.ChangeStatus(Texts.Keys.DEVELOP, "CLICKED ON DEACTIVATE");
		}

		protected void OnBtnSynchStartClicked (object sender, EventArgs e)
		{
			Common.ShowMessageBox (Texts.Keys.UNKNOWNERROR, "NOT IMPLEMENTED YET");
		}

		protected void OnEditModuleActionActivated (object sender, EventArgs e)
		{
			ModuleWindow moduleWindow = new ModuleWindow();
			moduleWindow.Show();
		}
	}
}