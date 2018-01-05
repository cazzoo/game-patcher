using System;

namespace RFUpdater.Patcher.Source_files
{
    internal class ListProcessor
    {
        public static void AddFile(string File)
        {
            Globals.File file = new Globals.File();

            var currentFileString = File;
            var lastSpaceIndex = currentFileString.LastIndexOf(' ');
            file.Size = Convert.ToInt64(currentFileString.Substring(lastSpaceIndex).Trim());
            currentFileString = currentFileString.Substring(0, lastSpaceIndex);

            lastSpaceIndex = currentFileString.LastIndexOf(' ');
            file.Hash = currentFileString.Substring(lastSpaceIndex).Trim();
            currentFileString = currentFileString.Substring(0, lastSpaceIndex);

            file.Name = currentFileString.Trim();

            Globals.Files.Add(file);
        }
    }
}