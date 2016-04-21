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
			foreach (string PropertyName in MainWindow.settings.getKeys("Section")) {
				string PropertyValue = MainWindow.settings.GetValue ("Section", PropertyName, "default");

				SettingRow SettingRowWidget = new SettingRow (PropertyName, PropertyValue);
				vboxListSettings.PackEnd(SettingRowWidget, true, true, 6);
			}
			vboxListSettings.ShowAll ();
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