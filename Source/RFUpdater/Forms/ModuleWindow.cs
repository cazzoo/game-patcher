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
		private string Current_module_name;
		private int Current_module_version;
		private ListStore store;

		public ModuleWindow (string ModuleName = null) :
			base (Gtk.WindowType.Toplevel)
		{
			store = CreateModel ();
			store.Clear ();

			this.Build ();
			if (ModuleName == null) {
				FormStateNew ();
				in_version.Text = "1";
				UpdateDate ();
			} else {
				FormStateView ();
				LoadModule (ModuleName);
				btn_newVersion.Sensitive = true;
			}

			treeview_files.Model = store;
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
			parsedModule.Name = in_name.Text;
			parsedModule.Version = Convert.ToInt32 (in_version.Text);
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
			in_name.Text = FileModule.Name;
			in_version.Text = FileModule.Version.ToString ();
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
			Current_module_name = in_name.Text;
			var serializer = new XmlSerializer (typeof(Module));
			var stream = new FileStream (in_name.Text + ".xml", FileMode.Create);
			Module Input_module = ParseModuleInputs ();
			serializer.Serialize (stream, Input_module);
			stream.Close ();
		}

		private Module LoadModule (string ModuleName)
		{
			var serializer = new XmlSerializer (typeof(Module));
			var stream = new FileStream (ModuleName + ".xml", FileMode.Open);
			var container = serializer.Deserialize (stream) as Module;
			ParseModuleFile (container);
			stream.Close ();

			Current_module_name = container.Name;
			Current_module_version = container.Version;

			return container;
		}

		#endregion moduleHandlers

		#region treeview

		private ListStore CreateModel ()
		{
			ListStore store = new ListStore (typeof(bool),
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
			TreeViewColumn column = new TreeViewColumn ("Selected", rendererToggle, "active", Column.Selected);

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
			FileChooserDialog filechooser =
				new FileChooserDialog ("Select files",
					this,
					FileChooserAction.SelectFolder,
					"Cancel", ResponseType.Cancel,
					"Open", ResponseType.Ok);

			if (filechooser.Run () == (int)ResponseType.Ok) {
				String[] files = Directory.GetFiles (filechooser.Filename);
				foreach (string file in files) {
					store.AppendValues (false,
						file);
				}
			}

			filechooser.Destroy ();
		}

		protected void OnBtnRemoveFilesClicked (object sender, EventArgs e)
		{
			store.Foreach ((model, path, iter) => {
				bool selected = (bool)store.GetValue (iter, 0);
				if (selected) {
					
				}
				return false;
			});

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

		protected void OnButtonValidateClicked (object sender, EventArgs e)
		{
			SaveModule ();
			this.Destroy ();
		}

		protected void OnBtnCancelClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}

		protected void OnBtnNewVersionClicked (object sender, EventArgs e)
		{
			Module Loaded_module = LoadModule (Current_module_name);
			UpdateDate ();
			in_version.Text = (Loaded_module.Version + 1).ToString ();
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

		#endregion buttons
	}
}

