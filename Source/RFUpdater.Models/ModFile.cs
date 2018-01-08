using System;
using System.IO;

namespace RFUpdater.Models
{
    public class ModFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public UInt32 FileHash { get; set; }
        public UInt32 FileSize { get; set; }
        public Boolean Deletable { get; set; }

        public string GetFullPath()
        {
            return Path.Combine(FilePath, FileName);
        }
    }
}