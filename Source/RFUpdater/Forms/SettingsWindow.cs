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
			MainWindow.settings.cancelSettings ();
			foreach (Setting setting in MainWindow.settings.applicationSettings) {
				setting.DefaultValue = setting.Value;
				var SettingRowWidget = new SettingRow (setting);
				vboxListSettings.PackEnd(SettingRowWidget, true, true, 6);
			}
			vboxListSettings.ShowAll ();
		}

		protected void CancelSettings (object sender, EventArgs e)
		{
			Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings not saved");
			MainWindow.settings.cancelSettings ();
			this.CloseWindow ();
		}

		protected void SaveSettings (object sender, EventArgs e)
		{
			int NumberPropertiesChanged = 0;
			foreach (SettingRow SettingRowWidget in vboxListSettings.Children) {
				if (SettingRowWidget.Changed()) {
					NumberPropertiesChanged++;
					SettingRowWidget.setting.DefaultValue = SettingRowWidget.setting.Value;
					MainWindow.settings.updateSetting(SettingRowWidget.setting);
				}
			}
			if (NumberPropertiesChanged > 0) {
				Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings saved, " + NumberPropertiesChanged + " property(ies) updated");
			} else {
				Common.ChangeStatus (Texts.Keys.APPLICATION, "Settings identical, not saved");
			}
			MainWindow.settings.saveSettings ();
			this.CloseWindow ();
		}

		private void CloseWindow ()
		{
			this.Destroy ();
		}
	}
}