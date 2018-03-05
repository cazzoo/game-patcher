using Newtonsoft.Json;
using Semver;
using System;
using System.Collections.Generic;

namespace RFUpdater.Models
{
    public class Mod : IEquatable<Mod>
    {
        private const string _contentDirectoryName = "content";

        [JsonIgnore]
        public string ContentDirectory => System.IO.Path.Combine(ModDirectory, _contentDirectoryName);

        [JsonIgnore]
        public string ModDirectory => System.IO.Path.Combine(ModStorePath, Name);

        [JsonIgnore]
        public bool IsPathDefined => !string.IsNullOrEmpty(ModStorePath);

        [JsonIgnore]
        public string ModStorePath { get; set; }

        public Mod()
        {
            ModStorePath = string.Empty;
            Name = string.Empty;
            Categories = new List<string>();
            Tags = new List<string>();
            Files = new List<ModFile>();
            Dependencies = new HashSet<ModDependency>();
            CreationDate = DateTime.Now;
        }

        public string Name { get; set; }
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

        public HashSet<ModDependency> Dependencies { get; set; }

        public void IncrementVersion()
        {
            int nextMajor = 1;
            int nextMinor = 0;
            if (null != Version)
            {
                nextMajor = Version.Major;
                nextMinor = Version.Minor;
                if (nextMinor == 9)
                {
                    nextMajor++;
                    nextMinor = 0;
                }
                else
                {
                    nextMinor++;
                }
            }
            Version = new SemVersion(nextMajor, nextMinor);
        }

        public void SetUpdatedDate()
        {
            if (null != Version)
            {
                UpdateDate = DateTime.Now;
            }
        }

        public Mod ShallowCopy()
        {
            return (Mod)this.MemberwiseClone();
        }

        public bool Equals(Mod other)
        {
            return Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}