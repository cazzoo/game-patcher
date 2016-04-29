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

		public ModuleWindow (Module LoadedModule = null) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			if (LoadedModule == null) {
				EnableModifications ();
			} else {
				Current_module_name = LoadedModule.Name;
			}
		}

		private void EnableModifications() {
			in_name.Sensitive = true;
		}

		private Module ParseModuleInputs() {
			Module ParsedModule = new Module ();
			ParsedModule.Name = in_name.Text;
			return ParsedModule;
		}

		private void ParseModuleFile(Module FileModule) {
			in_name.Text = FileModule.Name;
		}

		private void SaveModule() {
			Current_module_name = in_name.Text;
			var serializer = new XmlSerializer(typeof(Module));
			var stream = new FileStream(in_name.Text + ".xml", FileMode.Create);
			Module Input_module = ParseModuleInputs ();
			serializer.Serialize(stream, Input_module);
			stream.Close();
		}

		private void LoadModule(string ModuleName) {
			var serializer = new XmlSerializer(typeof(Module));
			var stream = new FileStream(ModuleName + ".xml", FileMode.Open);
			var container = serializer.Deserialize(stream) as Module;
			ParseModuleFile (container);
			stream.Close();
		}

		protected void OnButtonValidateClicked (object sender, EventArgs e)
		{
			SaveModule ();
		}

		protected void OnBtnCancelClicked (object sender, EventArgs e)
		{
			LoadModule (Current_module_name);
		}
	}
}

