using System;

namespace RFUpdater
{
	public partial class ModuleWindow : Gtk.Window
	{
		public ModuleWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

