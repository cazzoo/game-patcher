using System;
using System.Collections.Generic;

namespace RFUpdater.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Contact { get; set; }
        public string Icon { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public Version Version { get; set; }
        public String FolderName { get; set; }
        public List<ModFile> Files { get; set; }
        public Boolean Mandatory { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Dictionary<string, Version>> Dependencies { get; set; }
    }
}