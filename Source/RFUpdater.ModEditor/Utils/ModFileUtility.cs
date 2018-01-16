using Crc32C;
using RFUpdater.Models;
using System;
using System.IO;
using System.Windows;

namespace RFUpdater.ModEditor
{
    public static class ModFileUtility
    {
        public static UInt32 GetHash(string Name)
        {
            try
            {
                return Crc32CAlgorithm.Compute(File.ReadAllBytes(Name));
            }
            catch
            {
                MessageBox.Show(Name + " cannot be opened.");
                return new UInt32();
            }
        }

        public static Mod CopyModFilesToModPath(Mod workingMod, Mod loadedMod)
        {
            foreach (ModFile modFile in workingMod.Files)
            {
                string contentDirectory = loadedMod != null ? loadedMod.ContentDirectory: modFile.FilePath;
                string selectedModFile = Path.Combine(contentDirectory, modFile.FileName);
                FileInfo selectedFileInfo = new FileInfo(selectedModFile);
                string newFilePath = Path.Combine(workingMod.ContentDirectory, selectedFileInfo.Name);
                FileInfo newFileInfo = new FileInfo(newFilePath);
                if (!File.Exists(newFilePath) || (File.Exists(newFilePath) && newFileInfo.LastWriteTimeUtc != selectedFileInfo.LastWriteTimeUtc))
                {
                    if (!Directory.Exists(workingMod.ContentDirectory))
                    {
                        Directory.CreateDirectory(workingMod.ContentDirectory);
                    }
                    File.Copy(selectedModFile, newFilePath, true);
                }
                modFile.UpdateFromPath(newFilePath);
            }
            return workingMod;
        }

        public static ModFile GetModFile(FileInfo fileInfo)
        {
            return new ModFile
            {
                FileName = fileInfo.Name,
                FileHash = GetHash(fileInfo.FullName),
                FileSize = fileInfo.Length,
                FilePath = fileInfo.DirectoryName
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}