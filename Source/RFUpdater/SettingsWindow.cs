using System;
using Gtk;

namespace RFUpdater
{
	public partial class SettingsWindow : Gtk.Window
	{
		public SettingsWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}

		protected void Init()
		{
			INIFile set = new INIFile ("RFCUpdater.ini");
			set.SetValue("Section", "Key", "Value");
			string param = set.GetValue ("Section", "Key", "a:");
			SettingRow row = new SettingRow ();
			row.Label = "GamePath";
			row.DefaultValue = param;
			vboxListSettings.PackStart(row, true, true, 2);
		}

		protected void CancelSettings (object sender, EventArgs e)
		{
			this.CloseWindow ();
		}

		protected void SaveSettings (object sender, EventArgs e)
		{
			this.CloseWindow ();
		}

		private void CloseWindow ()
		{
			this.Destroy ();
		}
	}
}