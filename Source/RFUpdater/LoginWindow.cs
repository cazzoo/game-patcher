using System;
using Gtk;

public partial class LoginWindow: Gtk.Window
{
	public LoginWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		status_login.Push(1, "Waiting for login");
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void login (object sender, EventArgs e)
	{
		String return_message = form_validate ();
		if (return_message != null) {
			status_login.Push (1, return_message);
		} else {
			check_login ();
		}
	}

	protected string form_validate()
	{
		if (string.Empty == in_login.Text) {
			return "Field login cannot be empty";
		}
		if (string.Empty == in_password.Text) {
			return "Field password cannot be empty";
		}
		return null;
	}

	protected void check_login() {
		if(in_login.Text == "caz" && in_password.Text == "caz") {
			MainWindow main = new MainWindow ();
			this.Destroy ();
			main.Show ();
		}
	}
}
