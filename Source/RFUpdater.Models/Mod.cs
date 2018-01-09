using Semver;
using System;
using System.Collections.Generic;

namespace RFUpdater.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Contact { get; set; }
        public string Icon { get; set; }
        public string Password { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public SemVersion Version { get; set; }
        public Boolean Mandatory { get; set; }
        public List<ModFile> Files { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<Dictionary<string, string>> Dependencies { get; set; }
    }
}