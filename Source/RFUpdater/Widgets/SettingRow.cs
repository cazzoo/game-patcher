using System;
using Gtk;

namespace RFUpdater
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class SettingRow : Gtk.Bin
	{
		Gtk.Widget widget;
		private string defaultValue;

		public SettingRow (string p_Label, string p_DefaultValue, Setting.SettingType p_type)
		{
			this.Build ();
			Type = p_type;
			Label = p_Label;
			DefaultValue = p_DefaultValue;
			switch (Type) {
			case Setting.SettingType.TEXT:
				widget = new Gtk.Entry ();
				break;
			}
			hbox.PackStart (widget, true, true, 1);
		}

		public string Label {
			get { return lbl_setting.Text; }
			set { lbl_setting.Text = value; }
		}

		public string DefaultValue {
			get { 
				return defaultValue;
			}
			set {
				defaultValue = value;
				switch (Type) {
				case Setting.SettingType.TEXT:
					((Entry)widget).Text = value;
					break;
				}
			}
		}

		public string Value {
			get { 
				string widget_value = "";
				switch (Type) {
				case Setting.SettingType.TEXT:
					widget_value = ((Entry)widget).Text;
					break;
				default:
					widget_value = "";
					break;
				}
				return widget_value;
			}
			set { 
				switch (Type) {
				case Setting.SettingType.TEXT:
					((Entry)widget).Text = value;
					break;
				}
			}
		}

		public Boolean Changed ()
		{
			return Value != DefaultValue;
		}

		public Setting.SettingType Type {
			get;
			set;
		}

		protected void OnBtnSettingClicked (object sender, EventArgs e)
		{
			if (Value != DefaultValue) {
				Value = DefaultValue;
			}
		}
	}
}

