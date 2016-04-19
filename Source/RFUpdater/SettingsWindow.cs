using System;
using Gtk;

public partial class SettingsWindow : Gtk.Window
{
	public SettingsWindow () :
		base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
	}
}
