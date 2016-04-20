using System;
using Gtk;

namespace RFUpdater
{
	public partial class MainWindow : Gtk.Window
	{
		public static INIFile settings;

		public MainWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}

		protected void Init ()
		{
			settings = new INIFile ("RFCUpdater.ini");
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
	}
}