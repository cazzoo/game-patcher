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
		private Module module = new Module ();
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

		private ModuleVersion ParseModuleInputs ()
		{
			var parsedModule = new ModuleVersion ();
			var moduleVersion = listVersions.Count;
			parsedModule.Version = moduleVersion;
			parsedModule.RealeaseDate = Convert.ToDateTime (in_realeaseDate.Text);
			parsedModule.Mandatory = chk_mandatory.Active;
			var files = new List<ModuleFile> ();
			storePaths.Foreach ((model, path, iter) => {
				var moduleFile = new ModuleFile (storePaths.GetValue (iter, (int)Column.FileWithPath).ToString ());
				moduleFile.FileCRC = storePaths.GetValue (iter, (int)Column.CRC).ToString ();
				files.Add (moduleFile);
				return false;
			});
			parsedModule.ModuleFiles = files;
			return parsedModule;
		}

		private void ParseModuleFile (ModuleVersion FileModule)
		{
			in_realeaseDate.Text = FileModule.RealeaseDate.ToString ();
			chk_mandatory.Active = FileModule.Mandatory;
			lbl_moduleDependancies.Text = FileModule.Dependancies.ToString ();
			lbl_moduleConflicts.Text = FileModule.Conflicts.ToString ();

			storePaths.Clear ();
			foreach (ModuleFile file in FileModule.ModuleFiles) {
				storePaths.AppendValues (false, file.FileCRC, file.FileName);
			}
		}

		private void SaveModule ()
		{
			var serializer = new XmlSerializer (typeof (Module));
			if (!Directory.Exists (Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory (Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + in_name.Text + ".xml", FileMode.Create);
			ModuleVersion Input_module = ParseModuleInputs ();
			module.Name = in_name.Text;
			module.ModuleVersions.Add (Input_module);
			serializer.Serialize (stream, module);
			stream.Close ();
		}

		private ModuleVersion LoadModule (string ModuleName)
		{
			var serializer = new XmlSerializer (typeof (Module));
			if (!Directory.Exists (Globals.LocalModuleDefinitionFolder)) {
				Directory.CreateDirectory (Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream (Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + ModuleName + ".xml", FileMode.Open);
			module = serializer.Deserialize (stream) as Module;
			stream.Close ();

			ModuleVersion last_version_module = module.GetLastModuleVersion ();
			in_name.Text = module.Name;

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
			                           typeof (string), typeof (string));
			return store;
		}

		private void SelectedToggled (object o, ToggledArgs args)
		{
			TreeIter iter;
			if (storePaths.GetIterFromString (out iter, args.Path)) {
				var val = (bool)storePaths.GetValue (iter, (int)Column.Selected);
				storePaths.SetValue (iter, (int)Column.Selected, !val);
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

			// column for CRC
			CellRendererText rendererCRCText = new CellRendererText ();
			column = new TreeViewColumn ("CRC", rendererCRCText, "text", Column.CRC);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 60;
			treeView.AppendColumn (column);

			// column for paths
			CellRendererText rendererText = new CellRendererText ();
			column = new TreeViewColumn ("Filepath", rendererText, "text", Column.FileWithPath);
			column.SortColumnId = (int)Column.FileWithPath;
			column.Sizing = TreeViewColumnSizing.Autosize;
			treeView.AppendColumn (column);
		}

		private enum Column
		{
			Selected,
			CRC,
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
				labelIgnoredPath.Text = filechooser.Filename;
				foreach (string file in getFiles (filechooser.Filename, true)) {
					TreeIter iter;
					if (!storePaths.GetIter (out iter, new TreePath (file))) {
						storePaths.AppendValues (false, Common.GetHash(file), file);
					}
				}
			}

			filechooser.Destroy ();
		}

		protected void OnBtnSelectFolderIgnoredPath (object sender, EventArgs e)
		{
			var folderChooser =
				new FileChooserDialog ("Select IgnoredFolder Path",
					this,
					FileChooserAction.SelectFolder,
					"Cancel", ResponseType.Cancel,
					"Open", ResponseType.Ok);

			if (folderChooser.Run () == (int)ResponseType.Ok) {
				labelIgnoredPath.Text = folderChooser.Filename;
			}

			folderChooser.Destroy ();
		}

		private List<string> getFiles (string path, bool recursive = false)
		{
			var listPaths = new List<string> ();
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
					if ((bool)storePaths.GetValue (iter, (int)Column.Selected)) {
						paths.Add ((string)storePaths.GetValue (iter, (int)Column.FileWithPath));
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
					if (((string)storePaths.GetValue (iter, (int)Column.FileWithPath)).Equals (path)) {
						storePaths.Remove (ref iter);
						break;
					}
				} while (storePaths.IterNext (ref iter));
			}
		}

		protected void OnBtnSelectNoneClicked (object sender, EventArgs e)
		{
			storePaths.Foreach ((model, path, iter) => {
				storePaths.SetValue (iter, (int)Column.Selected, false);
				return false;
			});
		}

		protected void OnBtnSelectAllClicked (object sender, EventArgs e)
		{
			storePaths.Foreach ((model, path, iter) => {
				storePaths.SetValue (iter, (int)Column.Selected, true);
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
			var combo = o as ComboBox;
			if (o == null)
				return;

			TreeIter iter;

			if (combo.GetActiveIter (out iter)) {
				string valueVersion = ((string)combo.Model.GetValue (iter, (int)Column.Selected));
				Common.ChangeStatus(Texts.Keys.DEVELOP, valueVersion);
				ParseModuleFile (module.GetModuleVersion(int.Parse(valueVersion)));
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
			ModuleVersion LoadedModuleVersion = LoadModule (module.Name);
			UpdateDate ();
			var nextModuleVersion = (LoadedModuleVersion.Version + 1).ToString ();
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

		protected void OnButtonApplyIgnoreReleased (object sender, EventArgs e)
		{
			TreeIter iter;
			var paths = new List<string> ();
			if (storePaths.GetIterFirst (out iter)) {
				do {
					var val = new GLib.Value ();
					storePaths.GetValue (iter, (int)Column.FileWithPath, ref val);
					if (((string)val.Val).Contains (labelIgnoredPath.Text)) {
						val.Val = ((string)val.Val).Substring (labelIgnoredPath.Text.Length);
						storePaths.SetValue (iter, (int)Column.FileWithPath, val);
					}
				} while (storePaths.IterNext (ref iter));
			}

			foreach (string path in paths) {
				removeRow (path);
			}
		}

		#endregion buttons
	}
}

