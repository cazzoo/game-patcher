using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Gtk;
using System.Collections.Generic;

namespace RFUpdater
{
	public partial class ModuleWindow : Gtk.Window
	{
		private ModuleCollection module = new ModuleCollection ();
		private ListStore store;
		private ComboBox combo;

		public ModuleWindow (string ModuleName = null) :
			base (Gtk.WindowType.Toplevel)
		{
			store = CreateModel ();
			store.Clear ();

			this.Build ();

			combo = ComboBox.NewText ();
			combo.Changed += new EventHandler (OnComboBoxChanged);

			hbox5.PackEnd (combo, true, true, 0);

			if (ModuleName == null) {
				FormStateNew ();
				combo.AppendText ("1");
				UpdateDate ();
			} else {
				FormStateView ();
				LoadModule (ModuleName);
				btn_newVersion.Sensitive = true;
			}

			combo.Active=0;

			AddColumns (treeview_files);

			ShowAll ();
		}

		void OnComboBoxChanged (object o, EventArgs args)
		{
			ComboBox combo = o as ComboBox;
			if (o == null)
				return;

			TreeIter iter;

			if (combo.GetActiveIter (out iter))
				Console.WriteLine ((string) combo.Model.GetValue (iter, 0));
		}

		private void UpdateDate ()
		{
			in_realeaseDate.Text = DateTime.Now.ToString ();
		}

		private void FormStateNew ()
		{
			in_name.Sensitive = true;
			btn_selectDependancies.Sensitive = true;
			btn_selectConflicts.Sensitive = true;
			btn_validate.Sensitive = true;
			chk_mandatory.Sensitive = true;

			btn_select_none.Sensitive = true;
			btn_select_all.Sensitive = true;
			btn_remove_files.Sensitive = true;
			btn_select_files.Sensitive = true;

			btn_newVersion.Sensitive = false;
		}

		private void FormStateView ()
		{
			btn_validate.Sensitive = false;
			chk_mandatory.Sensitive = false;
			in_name.Sensitive = false;
			btn_newVersion.Sensitive = false;
			btn_selectDependancies.Sensitive = false;
			btn_selectConflicts.Sensitive = false;

			btn_select_none.Sensitive = false;
			btn_select_all.Sensitive = false;
			btn_remove_files.Sensitive = false;
			btn_select_files.Sensitive = false;
		}

		private void FormStateEdit ()
		{
			btn_selectDependancies.Sensitive = true;
			btn_selectConflicts.Sensitive = true;
			btn_validate.Sensitive = true;
			chk_mandatory.Sensitive = true;

			btn_select_none.Sensitive = true;
			btn_select_all.Sensitive = true;
			btn_remove_files.Sensitive = true;
			btn_select_files.Sensitive = true;

			in_name.Sensitive = false;
			btn_newVersion.Sensitive = false;
		}

		#region moduleHandlers

		private Module ParseModuleInputs ()
		{
			Module parsedModule = new Module ();
			var moduleVersion = Convert.ToInt32 ("1");
			parsedModule.Version = moduleVersion;
			parsedModule.RealeaseDate = Convert.ToDateTime (in_realeaseDate.Text);
			parsedModule.Mandatory = chk_mandatory.Active;
			List<String> files = new List<string> ();
			store.Foreach ((model, path, iter) => {
				files.Add (store.GetValue (iter, 1).ToString ());
				return false;
			});
			parsedModule.Files = files;
			return parsedModule;
		}

		private void ParseModuleFile (Module FileModule)
		{
			var moduleVersion = FileModule.Version.ToString ();
			//in_version.Text = moduleVersion;
			in_realeaseDate.Text = FileModule.RealeaseDate.ToString ();
			chk_mandatory.Active = FileModule.Mandatory;
			lbl_moduleDependancies.Text = FileModule.Dependancies.ToString ();
			lbl_moduleConflicts.Text = FileModule.Conflicts.ToString ();

			store.Clear ();
			foreach (string file in FileModule.Files) {
				store.AppendValues (false,
					file);
			}
		}

		private void SaveModule ()
		{
			var serializer = new XmlSerializer (typeof(ModuleCollection));
			if(!Directory.Exists(Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory(Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + in_name.Text + ".xml", FileMode.Create);
			Module Input_module = ParseModuleInputs ();
			module.Name = in_name.Text;
			module.Modules.Add (Input_module);
			serializer.Serialize (stream, module);
			stream.Close ();
		}

		private Module LoadModule (string ModuleName)
		{
			var serializer = new XmlSerializer (typeof(ModuleCollection));
			if(!Directory.Exists(Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory(Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + ModuleName + ".xml", FileMode.Open);
			var container = serializer.Deserialize (stream) as ModuleCollection;
			stream.Close ();

			module = container;
			Module last_version_module = module.GetLastModuleVersion ();
			in_name.Text = module.Name;

			int lastModuleVersion = last_version_module.Version;

			//combo.Clear ();
			for (int i = lastModuleVersion; i >= 1; i--) {
				combo.AppendText (i.ToString());
			}

			ParseModuleFile (last_version_module);
			return last_version_module;
		}

		#endregion moduleHandlers

		#region treeview

		private ListStore CreateModel ()
		{
			var store = new ListStore (typeof(bool),
				                  typeof(string));
			return store;
		}

		private void SelectedToggled (object o, ToggledArgs args)
		{
			TreeIter iter;
			if (store.GetIterFromString (out iter, args.Path)) {
				var val = (bool)store.GetValue (iter, 0);
				store.SetValue (iter, 0, !val);
			}
		}

		private void AddColumns (TreeView treeView)
		{
			// column for selected toggles
			var rendererToggle = new CellRendererToggle ();
			rendererToggle.Toggled += new ToggledHandler (SelectedToggled);
			var column = new TreeViewColumn ("Selected", rendererToggle, "active", Column.Selected);

			// set this column to a fixed sizing (of 50 pixels)
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 60;
			treeView.AppendColumn (column);

			// column for bug numbers
			CellRendererText rendererText = new CellRendererText ();
			column = new TreeViewColumn ("Filepath", rendererText, "text", Column.FileWithPath);
			column.SortColumnId = (int)Column.FileWithPath;
			treeView.AppendColumn (column);
		}

		private enum Column
		{
			Selected,
			FileWithPath
		}

		protected void OnBtnSelectFilesClicked (object sender, EventArgs e)
		{
			var filechooser =
				new FileChooserDialog ("Select files",
					this,
					FileChooserAction.SelectFolder,
					"Cancel", ResponseType.Cancel,
					"Open", ResponseType.Ok);

			if (filechooser.Run () == (int)ResponseType.Ok) {
				foreach (string file in getFiles(filechooser.Filename, true)) {
					TreeIter iter;
					if (!store. GetIter (out iter, new TreePath(file))) {
						store.AppendValues (false,
						file);
					}
				}
			}

			filechooser.Destroy ();
		}

		private List<String> getFiles (string path, bool recursive = false)
		{
			var listPaths = new List<String>();
			foreach (string file in Directory.GetFiles (path)) {
				listPaths.Add(file);
			}
			if (recursive) {
				foreach (string directory in Directory.GetDirectories (path)) {
					listPaths.AddRange (getFiles(directory, recursive));
				}
			}
			return listPaths;
		}

		protected void OnBtnRemoveFilesSelected (object sender, EventArgs e)
		{
			TreeIter iter;
			var paths = new List<string> ();
			if (store.GetIterFirst (out iter)) {
				do {
					if ((bool)store.GetValue (iter, 0)) {
						paths.Add ((string)store.GetValue (iter, 1));
					}
				} while (store.IterNext (ref iter));
			}

			foreach (string path in paths) {
				removeRow (path);
			}
		}

		/**
		 * Removes a single row
		 */
		private void removeRow (string path)
		{
			TreeIter iter;

			if (store.GetIterFirst (out iter)) {
				do {
					if (((string)store.GetValue (iter, 1)).Equals(path)) {
						store.Remove (ref iter);
						break;
					}
				} while (store.IterNext (ref iter));
			}
		}

		protected void OnBtnSelectNoneClicked (object sender, EventArgs e)
		{
			store.Foreach ((model, path, iter) => {
				store.SetValue (iter, 0, false);
				return false;
			});
		}

		protected void OnBtnSelectAllClicked (object sender, EventArgs e)
		{
			store.Foreach ((model, path, iter) => {
				store.SetValue (iter, 0, true);
				return false;
			});
		}

		#endregion treeview

		#region buttons

		protected void OnBtnValidateClicked (object sender, EventArgs e)
		{
			SaveModule ();
			this.Destroy ();
		}

		protected void OnBtnNewVersionClicked (object sender, EventArgs e)
		{
			Module Loaded_module = LoadModule (module.Name);
			UpdateDate ();
			var nextModuleVersion = (Loaded_module.Version + 1).ToString ();
			combo.AppendText (nextModuleVersion);
			FormStateEdit ();
		}

		protected void OnBtnSelectDependanciesClicked (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		protected void OnBtnSelectConflictsClicked (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		protected void OnBtnCancelClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}

		#endregion buttons
	}
}

