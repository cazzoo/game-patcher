using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RFUpdater
{
	public class ModuleVersion
	{
		[XmlAttribute("Id")]
		public string Id { get; set; }
		[XmlAttribute("Version")]
		public int Version { get; set; }
		[XmlAttribute("ReleaseDate")]
		public DateTime RealeaseDate { get; set; }
		[XmlAttribute("Mandatory")]
		public Boolean Mandatory { get; set; }
		[XmlArray("Files"), XmlArrayItem("File")]
		public List<ModuleFile> ModuleFiles { get; set; }
		[XmlArray("Dependancies"), XmlArrayItem("Dependancy")]
		public List<ModuleVersion> Dependancies { get; set; }
		[XmlArray("Conflicts"), XmlArrayItem("Conflict")]
		public List<ModuleVersion> Conflicts { get; set; }

		public ModuleVersion ()
		{
			ModuleFiles = new List<ModuleFile> ();
		}

		public override string ToString(){
			return Id;
		}
	}
}

