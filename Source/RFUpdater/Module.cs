using System;
using System.Collections.Generic;
using Cyclic.Redundancy.Check;

namespace RFUpdater
{
	public class Module
	{
		private string Name { get; set; }
		private string Version { get; set; }
		private CRC Integrity { get; set; }
		private DateTime RealeaseDate { get; set; }
		private Boolean Mandatory { get; set; }
		private List<String> Files { get; set; }
		private List<Module> Dependancies { get; set; }
		private List<Module> Conflicts { get; set; }

		public Module ()
		{
		}

		public override string ToString(){
			return Name;
		}
	}
}

