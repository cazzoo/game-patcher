using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RFUpdater
{
	[XmlRoot("Module")]
	public class Module
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }
		[XmlArray("Versions"), XmlArrayItem("Version")]
		public List<ModuleVersion> ModuleVersions { get; set; }

		public Module ()
		{
			ModuleVersions = new List<ModuleVersion> ();
		}

		public override string ToString(){
			return Name;
		}

		public ModuleVersion GetModuleVersion (int version)
		{
			foreach (ModuleVersion module in ModuleVersions) {
				if (module != null && module.Version.Equals(version)) {
					return module;
				}
			}
			return null;
		}

		public ModuleVersion GetLastModuleVersion()
		{
			ModuleVersion last_module = null;
			foreach (ModuleVersion module in ModuleVersions) {
				if (last_module == null || (last_module != null && last_module.Version < module.Version)) {
					last_module = module;
				}
			}
			return last_module;
		}
	}
}

