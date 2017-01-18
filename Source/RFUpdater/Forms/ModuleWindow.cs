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
		private ModuleCollection moduleCollection = new ModuleCollection ();
		private ListStore storePaths;
		private ComboBox comboVersionSelector;
		private List<string> listVersions = new List<string> ();

		public ModuleWindow (string ModuleName = null) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();

			storePaths = CreateModel ();
			storePaths.Clear ();

			comboVersionSelector = ComboBox.NewText ();
			comboVersionSelector.Changed += new EventHandler (OnComboBoxChanged);

			hbox5.PackEnd (comboVersionSelector, true, true, 0);

			if (ModuleName == null) {
				FormStateNew ();
				listVersions.Add ("1");
				UpdateDate ();
			} else {
				FormStateView ();
				LoadModule (ModuleName);
				btn_newVersion.Sensitive = true;
			}

			updateVersionsComboList ();

			treeview_files.Model = storePaths;
			AddColumns (treeview_files);

			ShowAll ();
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
			comboVersionSelector.Sensitive = false;
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
			comboVersionSelector.Sensitive = false;
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
			storePaths.Foreach ((model, path, iter) => {
				files.Add (storePaths.GetValue (iter, 1).ToString ());
				return false;
			});
			parsedModule.Files = files;
			return parsedModule;
		}

		private void ParseModuleFile (Module FileModule)
		{
			in_realeaseDate.Text = FileModule.RealeaseDate.ToString ();
			chk_mandatory.Active = FileModule.Mandatory;
			lbl_moduleDependancies.Text = FileModule.Dependancies.ToString ();
			lbl_moduleConflicts.Text = FileModule.Conflicts.ToString ();

			storePaths.Clear ();
			foreach (string file in FileModule.Files) {
				storePaths.AppendValues (false,
					file);
			}
		}

		private void SaveModule ()
		{
			var serializer = new XmlSerializer (typeof (ModuleCollection));
			if (!Directory.Exists (Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory (Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + in_name.Text + ".xml", FileMode.Create);
			Module Input_module = ParseModuleInputs ();
			moduleCollection.Name = in_name.Text;
			moduleCollection.Modules.Add (Input_module);
			serializer.Serialize (stream, moduleCollection);
			stream.Close ();
		}

		private Module LoadModule (string ModuleName)
		{
			var serializer = new XmlSerializer (typeof (ModuleCollection));
			if (!Directory.Exists (Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory (Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + ModuleName + ".xml", FileMode.Open);
			var container = serializer.Deserialize (stream) as ModuleCollection;
			stream.Close ();

			moduleCollection = container;
			Module last_version_module = moduleCollection.GetLastModuleVersion ();
			in_name.Text = moduleCollection.Name;

			int lastModuleVersion = last_version_module.Version;
			listVersions.Clear ();
			for (int i = 1; i <= lastModuleVersion; i++) {
				listVersions.Add (i.ToString ());
			}

			ParseModuleFile (last_version_module);
			return last_version_module;
		}

		#endregion moduleHandlers

		#region treeview

		private ListStore CreateModel ()
		{
			var store = new ListStore (typeof (bool),
								  typeof (string));
			return store;
		}

		private void SelectedToggled (object o, ToggledArgs args)
		{
			TreeIter iter;
			if (storePaths.GetIterFromString (out iter, args.Path)) {
				var val = (bool)storePaths.GetValue (iter, 0);
				storePaths.SetValue (iter, 0, !val);
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
				foreach (string file in getFiles (filechooser.Filename, true)) {
					TreeIter iter;
					if (!storePaths.GetIter (out iter, new TreePath (file))) {
						storePaths.AppendValues (false,
						file);
					}
				}
			}

			filechooser.Destroy ();
		}

		private List<String> getFiles (string path, bool recursive = false)
		{
			var listPaths = new List<String> ();
			foreach (string file in Directory.GetFiles (path)) {
				listPaths.Add (file);
			}
			if (recursive) {
				foreach (string directory in Directory.GetDirectories (path)) {
					listPaths.AddRange (getFiles (directory, recursive));
				}
			}
			return listPaths;
		}

		protected void OnBtnRemoveFilesSelected (object sender, EventArgs e)
		{
			TreeIter iter;
			var paths = new List<string> ();
			if (storePaths.GetIterFirst (out iter)) {
				do {
					if ((bool)storePaths.GetValue (iter, 0)) {
						paths.Add ((string)storePaths.GetValue (iter, 1));
					}
				} while (storePaths.IterNext (ref iter));
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

			if (storePaths.GetIterFirst (out iter)) {
				do {
					if (((string)storePaths.GetValue (iter, 1)).Equals (path)) {
						storePaths.Remove (ref iter);
						break;
					}
				} while (storePaths.IterNext (ref iter));
			}
		}

		protected void OnBtnSelectNoneClicked (object sender, EventArgs e)
		{
			storePaths.Foreach ((model, path, iter) => {
				storePaths.SetValue (iter, 0, false);
				return false;
			});
		}

		protected void OnBtnSelectAllClicked (object sender, EventArgs e)
		{
			storePaths.Foreach ((model, path, iter) => {
				storePaths.SetValue (iter, 0, true);
				return false;
			});
		}

		#endregion treeview

		#region versionCombo

		private void updateVersionsComboList ()
		{
			for (int i = listVersions.Count - 1 ; i >= 0; i--) {
				comboVersionSelector.AppendText (listVersions[i]);
			}
		}

		protected void OnComboBoxChanged (object o, EventArgs args)
		{
			ComboBox combo = o as ComboBox;
			if (o == null)
				return;

			TreeIter iter;

			if (combo.GetActiveIter (out iter)) {
				string valueVersion = ((string)combo.Model.GetValue (iter, 0));
				Common.ChangeStatus(Texts.Keys.DEVELOP, valueVersion);
				ParseModuleFile (moduleCollection.GetModuleVersion(int.Parse(valueVersion)));
			}
		}

		#endregion versionCombo

		#region buttons
		protected void OnBtnValidateClicked (object sender, EventArgs e)
		{
			SaveModule ();
			this.Destroy ();
		}

		protected void OnBtnNewVersionClicked (object sender, EventArgs e)
		{
			Module Loaded_module = LoadModule (moduleCollection.Name);
			UpdateDate ();
			var nextModuleVersion = (Loaded_module.Version + 1).ToString ();
			listVersions.Add (nextModuleVersion);
			updateVersionsComboList ();
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

