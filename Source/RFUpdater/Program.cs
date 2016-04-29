using System;
using Gtk;

namespace RFUpdater
{
	class Program
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			//LoginWindow win = new LoginWindow ();
			MainWindow win = new MainWindow();
			win.Show ();
			Application.Run ();
		}
	}
}
