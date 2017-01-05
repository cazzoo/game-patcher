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
			//MainWindow.settings.Refresh ();
			foreach (Setting setting in MainWindow.settings.applicationSettings) {
				string PropertyValue = setting.Value;

				SettingRow SettingRowWidget = new SettingRow (setting.Name, setting.Value, Setting.SettingType.TEXT);
				vboxListSettings.PackEnd(SettingRowWidget, true, true, 6);
			}
			vboxListSettings.ShowAll ();
		}

		protected void CancelSettings (object sender, EventArgs e)
		{
			Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings not saved");
			this.CloseWindow ();
		}

		protected void SaveSettings (object sender, EventArgs e)
		{
			int NumberPropertiesChanged = 0;
			foreach (SettingRow SettingRowWidget in vboxListSettings.Children) {
				if (SettingRowWidget.Changed()) {
					NumberPropertiesChanged++;
					//MainWindow.settings.SetValue (settingCategory, SettingRowWidget.Label, SettingRowWidget.Value);
				}
			}
			if (NumberPropertiesChanged > 0) {
				//MainWindow.settings.Flush ();
				Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings saved, " + NumberPropertiesChanged + " property(ies) updated");
			} else {
				Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings identical, not saved");
			}
			this.CloseWindow ();
		}

		private void CloseWindow ()
		{
			//MainWindow.settings.Refresh ();
			this.Destroy ();
		}
	}
}