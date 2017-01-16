using System;
using System.IO;
using Gtk;
using RFUpdater.Utils;

namespace RFUpdater
{
	public partial class MainWindow : Gtk.Window
	{
		public static SettingsList settings;
		public static Statusbar statusbar;
		public static ProgressBar progressFile;
		public static ProgressBar progressOverall;
		private ListStore store;

		public MainWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}

		protected void Init ()
		{
			settings = new SettingsList ();
			statusbar = status_main;
			progressFile = progress_file;
			progressOverall = progress_overall;
			store = CreateModel ();
			store.Clear ();
			updateModuleWidgetList ();
			treeview_modules.Model = store;
			AddColumns (treeview_modules);

			ShowAll ();
		}

		public void updateModuleWidgetList ()
		{
			if(Directory.Exists(Globals.LocalModuleDefinitionFolder)) {
				foreach (string FilePath in Directory.GetFiles(Globals.LocalModuleDefinitionFolder)) {
					if (System.IO.Path.GetExtension (FilePath) == ".xml") {
						string ModuleName = System.IO.Path.GetFileNameWithoutExtension (FilePath);
						Globals.ModuleNameList.Add (ModuleName);
						store.AppendValues (false, ModuleName);
					}
				}
			}
		}

		#region treeview

		private ListStore CreateModel ()
		{
			store = new ListStore (typeof(bool),
				                  typeof(string));
			return store;
		}

		private void SelectedToggled (object o, ToggledArgs args)
		{
			TreeIter iter;
			if (store.GetIterFromString (out iter, args.Path)) {
				bool val = (bool)store.GetValue (iter, 0);
				store.SetValue (iter, 0, !val);
			}
		}

		private void AddColumns (TreeView treeView)
		{
			// column for selected toggles
			CellRendererToggle rendererToggle = new CellRendererToggle ();
			rendererToggle.Toggled += new ToggledHandler (SelectedToggled);
			TreeViewColumn column = new TreeViewColumn ("Installed", rendererToggle, "active", Column.Installed);

			// set this column to a fixed sizing (of 50 pixels)
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 60;
			treeView.AppendColumn (column);

			// column for module name
			CellRendererText rendererText = new CellRendererText ();
			column = new TreeViewColumn ("Module", rendererText, "text", Column.Module);
			column.SortColumnId = (int)Column.Module;
			treeView.AppendColumn (column);

			// column for module version
			column = new TreeViewColumn ("Version", rendererText, "text", Column.Version);
			column.SortColumnId = (int)Column.Version;
			treeView.AppendColumn (column);

			// column for info button
			column = new TreeViewColumn ("Mandatory", rendererText, "text", Column.Mandatory);
			column.SortColumnId = (int)Column.Mandatory;
			treeView.AppendColumn (column);
		}

		private enum Column
		{
			Installed,
			Module,
			Version,
			Mandatory
		}

		#endregion treeview

		#region windowButtons

		protected void OnBtnActivateClicked (object sender, EventArgs e)
		{
			Common.ChangeStatus (Texts.Keys.DEVELOP, "CLICKED ON ACTIVATE");
		}

		protected void OnBtnDeactivateClicked (object sender, EventArgs e)
		{
			Common.ChangeStatus (Texts.Keys.DEVELOP, "CLICKED ON DEACTIVATE");
		}

		protected void OnBtnSynchStartClicked (object sender, EventArgs e)
		{
			Common.ShowMessageBox (MessageType.Warning, Texts.Keys.UNKNOWNERROR, "NOT IMPLEMENTED YET");
		}

		#endregion windowButtons

		#region menuBarButtons

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void QuitApplication (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		protected void openSettingsWindow (object sender, EventArgs e)
		{
			var sw = new SettingsWindow ();
			sw.Show ();
		}

		protected void OnEditModuleActionActivated (object sender, EventArgs e)
		{
			if (treeview_modules.Selection.GetSelectedRows ().Length > 0) {
				TreeIter iter;
				store.GetIter (out iter, treeview_modules.Selection.GetSelectedRows () [0]);
				var selectedModule = store.GetValue (iter, 1).ToString ();
				var moduleWindow = new ModuleWindow (selectedModule);
				moduleWindow.Show ();
			} else {
				Common.ShowMessageBox (MessageType.Info, Texts.Keys.APPLICATION, "Please select a module");
			}
		}

		protected void OnCreateModuleActionActivated (object sender, EventArgs e)
		{
			var moduleWindow = new ModuleWindow ();
			moduleWindow.Show ();
		}

		protected void OnDeleteModuleActionActivated (object sender, EventArgs e)
		{
			if (File.Exists ("new" + ".xml")) {
				File.Delete ("new" + ".xml");
			}
		}

		#endregion menuBarButtons

	}
}