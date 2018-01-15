using System;
using System.IO;

namespace RFUpdater.Models
{
    public class ModFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public UInt32 FileHash { get; set; }
        public long FileSize { get; set; }
        public Boolean Protected { get; set; }

        public string GetFullPath()
        {
            return Path.Combine(FilePath, FileName);
        }

        public void UpdateFromPath(string newFilePath)
        {
            string fileName = Path.GetFileName(newFilePath);
            string filePath = Path.GetDirectoryName(newFilePath);
            FileName = fileName;
            FilePath = filePath;
        }
    }
}