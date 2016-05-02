using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RFUpdater
{
	[XmlRoot("Module")]
	public class ModuleCollection
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }
		[XmlArray("Versions"), XmlArrayItem("Version")]
		public List<Module> Modules { get; set; }

		public ModuleCollection ()
		{
			Modules = new List<Module> ();
		}

		public override string ToString(){
			return Name;
		}

		public Module GetLastModuleVersion() {
			Module last_module = null;
			foreach (Module module in Modules) {
				if (last_module == null || (last_module != null && last_module.Version < module.Version)) {
					last_module = module;
				}
			}
			return last_module;
		}
	}
}

