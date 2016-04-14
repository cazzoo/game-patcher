using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	public MainWindow () :
		base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
		Init ();
	}

	protected void Init()
	{
		
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void QuitApplication (object sender, EventArgs e)
	{
		Application.Quit ();
	}
}
