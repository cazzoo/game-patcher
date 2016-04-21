using System;
using System.Collections.Generic;
using Cyclic.Redundancy.Check;

namespace RFUpdater
{
	public class Module
	{
		private string name;
		private string version;
		private CRC integrity;
		private DateTime realeaseDate;
		private Boolean mandatory;
		private List<String> files;
		private List<Module> dependancies;

		public Module ()
		{
		}

		#region "Properties"

		public string Version {
			get {
				return version;
			}
			set {
				version = value;
			}
		}

		public CRC Integrity {
			get {
				return integrity;
			}
			set {
				integrity = value;
			}
		}

		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		public DateTime RealeaseDate {
			get {
				return realeaseDate;
			}
			set {
				realeaseDate = value;
			}
		}

		public Boolean Mandatory {
			get {
				return mandatory;
			}
			set {
				mandatory = value;
			}
		}

		public List<String> Files {
			get {
				return files;
			}
			set {
				files = value;
			}
		}

		public List<Module> Dependancies {
			get {
				return dependancies;
			}
			set {
				dependancies = value;
			}
		}

		#endregion
	}
}

