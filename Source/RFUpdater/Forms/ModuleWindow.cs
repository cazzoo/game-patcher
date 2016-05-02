using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Gtk;

namespace RFUpdater
{
	public partial class ModuleWindow : Gtk.Window
	{
		private string Current_module_name;
		private int Current_module_version;

		public ModuleWindow (string ModuleName = null) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			if (ModuleName == null) {
				FormStateNew ();
				in_version.Text = "1";
				UpdateDate ();
			} else {
				FormStateView ();
				LoadModule(ModuleName);
				btn_newVersion.Sensitive = true;
			}
		}

		private void UpdateDate() {
			in_realeaseDate.Text = DateTime.Now.ToString ();
		}

		private void FormStateNew() {
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

		private void FormStateView() {
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

		private void FormStateEdit() {
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

		private Module ParseModuleInputs() {
			Module ParsedModule = new Module ();
			ParsedModule.Name = in_name.Text;
			ParsedModule.Version = Convert.ToInt32(in_version.Text);
			ParsedModule.RealeaseDate = Convert.ToDateTime(in_realeaseDate.Text);
			ParsedModule.Mandatory = chk_mandatory.Active;
			return ParsedModule;
		}

		private void ParseModuleFile(Module FileModule) {
			in_name.Text = FileModule.Name;
			in_version.Text = FileModule.Version.ToString();
			in_realeaseDate.Text = FileModule.RealeaseDate.ToString();
			chk_mandatory.Active = FileModule.Mandatory;
			lbl_moduleDependancies.Text = FileModule.Dependancies.ToString ();
			lbl_moduleConflicts.Text = FileModule.Conflicts.ToString ();
		}

		private void SaveModule() {
			Current_module_name = in_name.Text;
			var serializer = new XmlSerializer(typeof(Module));
			var stream = new FileStream(in_name.Text + ".xml", FileMode.Create);
			Module Input_module = ParseModuleInputs ();
			serializer.Serialize(stream, Input_module);
			stream.Close();
		}

		private Module LoadModule(string ModuleName) {
			var serializer = new XmlSerializer(typeof(Module));
			var stream = new FileStream(ModuleName + ".xml", FileMode.Open);
			var container = serializer.Deserialize(stream) as Module;
			ParseModuleFile (container);
			stream.Close();

			Current_module_name = container.Name;
			Current_module_version = container.Version;

			return container;
		}

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
	}
}

