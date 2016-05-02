using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Cyclic.Redundancy.Check;


namespace RFUpdater
{
	public class Module
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }
		[XmlAttribute("Version")]
		public int Version { get; set; }
		//[XmlAttribute("Integrity")]
		//public CRC Integrity { get; set; }
		[XmlAttribute("ReleaseDate")]
		public DateTime RealeaseDate { get; set; }
		[XmlAttribute("Mandatory")]
		public Boolean Mandatory { get; set; }
		[XmlArray("Files"), XmlArrayItem("File")]
		public List<String> Files { get; set; }
		[XmlArray("dependancies"), XmlArrayItem("dependancy")]
		public List<Module> Dependancies { get; set; }
		[XmlArray("Conflicts"), XmlArrayItem("Conflict")]
		public List<Module> Conflicts { get; set; }

		public Module ()
		{
		}

		public override string ToString(){
			return Name;
		}
	}
}

