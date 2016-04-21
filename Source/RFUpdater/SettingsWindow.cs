using System;
using Gtk;

namespace RFUpdater
{
	public partial class SettingsWindow : Gtk.Window
	{
		private string settingCategory = "User Settings";

		public SettingsWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}

		protected void Init()
		{
			MainWindow.settings.Refresh ();
			foreach (string PropertyName in MainWindow.settings.getKeys(settingCategory)) {
				string PropertyValue = MainWindow.settings.GetValue (settingCategory, PropertyName, "default");

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
			foreach (SettingRow SettingRowWidget in vboxListSettings.Children) {
				if (SettingRowWidget.Changed()) {
					MainWindow.settings.SetValue (settingCategory, SettingRowWidget.Label, SettingRowWidget.Value);
				}
			}
			MainWindow.settings.Flush ();
			this.CloseWindow ();
		}

		private void CloseWindow ()
		{
			MainWindow.settings.Refresh ();
			this.Destroy ();
		}
	}
}