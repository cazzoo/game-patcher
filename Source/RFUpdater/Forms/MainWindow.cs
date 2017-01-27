using System;
using System.IO;
using System.Xml.Serialization;
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
		private ListStore storeAvailableModules;
		private ListStore storeInstalledModules;

		public MainWindow() :
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			Init();
		}

		protected void Init()
		{
			settings = new SettingsList();
			statusbar = status_main;
			progressFile = progress_file;
			progressOverall = progress_overall;

			storeAvailableModules = CreateModel();
			storeAvailableModules.Clear();

			storeInstalledModules = CreateModel();
			storeInstalledModules.Clear();

			updateModuleWidgetList();

			treeviewAvailablesModules.Model = storeAvailableModules;
			AddColumns(treeviewAvailablesModules);

			treeviewInstalledModules.Model = storeInstalledModules;
			AddColumns(treeviewInstalledModules);

			ShowAll();
		}

		public void updateModuleWidgetList()
		{
			if (Directory.Exists(Globals.LocalModuleDefinitionFolder))
			{
				foreach (string FilePath in Directory.GetFiles(Globals.LocalModuleDefinitionFolder))
				{
					if (System.IO.Path.GetExtension(FilePath) == ".xml")
					{
						var module = LoadModule(FilePath);
						Globals.ModuleNameList.Add(module.Name);
						var lastModuleName = module.GetLastModuleVersion();
						storeAvailableModules.AppendValues(module.Name, lastModuleName.Version.ToString(), lastModuleName.Mandatory.ToString());

					}
				}
			}
		}

		private Module LoadModule(string ModuleName)
		{
			var serializer = new XmlSerializer(typeof(Module));
			if (!Directory.Exists(Globals.LocalModuleDefinitionFolder))
			{
				Directory.CreateDirectory(Globals.LocalModuleDefinitionFolder);
			}
			var stream = new FileStream(ModuleName, FileMode.Open);
			var container = serializer.Deserialize(stream) as Module;
			stream.Close();

			return container;
		}

		#region treeview

		private ListStore CreateModel()
		{
			return new ListStore(typeof(string), typeof(string), typeof(string));
		}

		private void AddColumns(TreeView treeView)
		{
			TreeViewColumn column;

			// column for module name
			CellRendererText rendererText = new CellRendererText();
			column = new TreeViewColumn("Module", rendererText, "text", Column.ModuleName);
			column.SortColumnId = (int)Column.ModuleName;
			treeView.AppendColumn(column);

			// column for module version
			column = new TreeViewColumn("Version", rendererText, "text", Column.Version);
			column.SortColumnId = (int)Column.Version;
			treeView.AppendColumn(column);

			// column for info button
			column = new TreeViewColumn("Mandatory", rendererText, "text", Column.Mandatory);
			column.SortColumnId = (int)Column.Mandatory;
			treeView.AppendColumn(column);
		}

		private enum Column
		{
			ModuleName,
			Version,
			Mandatory
		}

		#endregion treeview

		#region windowButtons

		protected void OnBtnActivateClicked(object sender, EventArgs e)
		{
			Common.ChangeStatus(Texts.Keys.DEVELOP, "CLICKED ON ACTIVATE");
		}

		protected void OnBtnDeactivateClicked(object sender, EventArgs e)
		{
			Common.ChangeStatus(Texts.Keys.DEVELOP, "CLICKED ON DEACTIVATE");
		}

		protected void OnBtnSynchStartClicked(object sender, EventArgs e)
		{
			Networking.CheckNetwork(Globals.ACTION_DOWNLOAD_DEFINITIONS);
			Common.ShowMessageBox(MessageType.Warning, Texts.Keys.UNKNOWNERROR, "NOT IMPLEMENTED YET");
		}

		#endregion windowButtons

		#region menuBarButtons

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void QuitApplication(object sender, EventArgs e)
		{
			Application.Quit();
		}

		protected void openSettingsWindow(object sender, EventArgs e)
		{
			var sw = new SettingsWindow();
			sw.Show();
		}

		protected void OnEditModuleActionActivated(object sender, EventArgs e)
		{
			if (treeviewAvailablesModules.Selection.GetSelectedRows().Length > 0)
			{
				TreeIter iter;
				storeAvailableModules.GetIter(out iter, treeviewAvailablesModules.Selection.GetSelectedRows()[0]);
				var selectedModule = storeAvailableModules.GetValue(iter, (int)Column.ModuleName).ToString();
				var moduleWindow = new ModuleWindow(selectedModule);
				moduleWindow.Show();
			}
			else {
				Common.ShowMessageBox(MessageType.Info, Texts.Keys.APPLICATION, "Please select a module");
			}
		}

		protected void OnCreateModuleActionActivated(object sender, EventArgs e)
		{
			var moduleWindow = new ModuleWindow();
			moduleWindow.Show();
		}

		protected void OnDeleteModuleActionActivated(object sender, EventArgs e)
		{
			if (treeviewAvailablesModules.Selection.GetSelectedRows().Length > 0)
			{
				TreeIter iter;
				storeAvailableModules.GetIter(out iter, treeviewAvailablesModules.Selection.GetSelectedRows()[0]);
				var selectedModule = storeAvailableModules.GetValue(iter, (int)Column.ModuleName).ToString();
				var completeFilePath = Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + selectedModule + ".xml"; 
				if (File.Exists(completeFilePath))
				{
					RemoveModule(selectedModule);
				}
			}
			else {
				Common.ShowMessageBox(MessageType.Info, Texts.Keys.APPLICATION, "Please select a module");
			}

		}

		#endregion menuBarButtons

		protected void RemoveModule(string moduleName)
		{
			var completeFilePath = Globals.LocalModuleDefinitionFolder + System.IO.Path.DirectorySeparatorChar + moduleName + ".xml";
			TreeIter iter;
			var found = false;
			storeAvailableModules.GetIterFirst(out iter);
			do
			{
				if (((string)storeAvailableModules.GetValue(iter, (int)Column.ModuleName)).Equals(moduleName))
				{
					found = true;
				}
			} while (storeAvailableModules.IterNext(ref iter) && !found);

			if (found)
			{
				storeAvailableModules.Remove(ref iter);
				File.Delete(completeFilePath);
			}
		}

	}
}