using System;

namespace RFUpdater
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class SettingRow : Gtk.Bin
	{
		string defaultValue;

		public SettingRow ()
		{
			this.Build ();
		}

		public string Label {
			get { return lbl_setting.Text; }
			set { lbl_setting.Text = value; }
		}

		public string DefaultValue {
			get { return defaultValue; }
			set {
				defaultValue = value;
				in_setting.Text = value;
			}
		}

		public string Value {
			get { return in_setting.Text; }
			set { in_setting.Text = value; }
		}

		protected void OnBtnSettingClicked (object sender, EventArgs e)
		{
			if (Value != DefaultValue) {
				Value = DefaultValue;
			}
		}
	}
}

