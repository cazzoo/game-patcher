using System;
using Gtk;

namespace RFUpdater
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class SettingRow : Gtk.Bin
	{
		Gtk.Widget widget;
		public Setting setting;

		public SettingRow (Setting pSetting)
		{
			this.Build ();
			setting = pSetting;

			// Label
			hbox.PackStart (new Label (setting.Name), true, true, 0);

			// Widget
			switch (setting.Type) {
			case Setting.SettingType.TEXT:
				widget = new Entry ();

				((Entry)widget).Text = setting.Value;
				break;
			}
			((Entry)widget).KeyReleaseEvent += OnValueChanged;
			hbox.PackStart (widget, true, true, 1);

			// Reset button
			var resetButton = new global::Gtk.Button ();
			resetButton.CanFocus = true;
			resetButton.Name = "btn_setting";
			resetButton.UseUnderline = true;
			var w2 = new Image ();
			w2.Pixbuf = Stetic.IconLoader.LoadIcon (this, "gtk-undo", IconSize.Menu);
			resetButton.Image = w2;
			hbox.PackStart (resetButton, true, true, 2);
			resetButton.Clicked += OnBtnSettingClicked;
		}

		public Boolean Changed ()
		{
			return setting.Value != setting.DefaultValue;
		}

		protected void OnBtnSettingClicked (object sender, EventArgs e)
		{
			if (setting.Value != setting.DefaultValue) {
				setting.Value = setting.DefaultValue;
				updateWidgetValue ();
			}
		}

		protected void OnValueChanged (object sender, EventArgs e)
		{
			switch (setting.Type) {
			case Setting.SettingType.TEXT:
				setting.Value = ((Entry)widget).Text;
				break;
			}
		}

		private void updateWidgetValue ()
		{
			switch (setting.Type) {
			case Setting.SettingType.TEXT:
				((Entry)widget).Text = setting.Value;
				break;
			}
		}
	}
}

