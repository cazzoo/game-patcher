using Newtonsoft.Json;
using RFUpdater.Models;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace RFUpdater.ModEditor
{
    public static class ModUtility
    {
        public static Mod SaveToFile(Mod mod)
        {
            mod.SetUpdatedDate();
            mod.IncrementVersion();

            string output = JsonConvert.SerializeObject(mod, Formatting.Indented);

            string jsonFile = Path.Combine(mod.Path, string.Format("{0}.json", mod.Name));
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(jsonFile))
                {
                    streamWriter.Write(output);
                }

                MessageBox.Show(String.Format("[{0}] mod has been successfully saved.", mod.Name));
                return mod;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Unable to save [{0}] mod on following path : [{1}].", mod.Name, mod.Path));
                throw;
            }
        }

        public static Mod LoadFile(string fileName)
        {
            Mod mod;
            string stringifiedMod;
            using (StreamReader file = File.OpenText(fileName))
            {
                stringifiedMod = file.ReadToEnd();
            }
            try
            {
                mod = JsonConvert.DeserializeObject<Mod>(stringifiedMod);

                string currentPath = Path.GetDirectoryName(fileName);
                mod.Path = currentPath;
                return mod;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while parsing the mod file. Content is invalid.");
                throw;
            }
        }

        public static Mod LoadOldFile(string fileName)
        {
            string modPath = Path.GetDirectoryName(fileName);
            DateTime creationDate = File.GetCreationTime(fileName);
            Mod loadedMod;
            try
            {
                string _modName = Path.GetFileNameWithoutExtension(fileName);
                List<ModFile> modFiles = new List<ModFile>();

                using (StreamReader reader = File.OpenText(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        char[] lineArray = line.ToCharArray();
                        Array.Reverse(lineArray);
                        string reversedLine = new string(lineArray);
                        string[] lineElements = reversedLine.Split(new char[] { ' ' }, 3);
                        for (int i = 0; i < lineElements.Length; i++)
                        {
                            char[] reversedElement = lineElements[i].ToCharArray();
                            Array.Reverse(reversedElement);
                            lineElements[i] = new string(reversedElement);
                        }

                        Array.Reverse(lineElements);
                        string path = Path.GetDirectoryName(lineElements[0]);
                        string filename = Path.GetFileName(lineElements[0]);
                        ModFile file = new ModFile()
                        {
                            Protected = false,
                            FilePath = path,
                            FileName = filename,
                            FileHash = UInt32.Parse(lineElements[1], System.Globalization.NumberStyles.HexNumber),
                            FileSize = UInt32.Parse(lineElements[2]),
                        };
                        modFiles.Add(file);
                    }
                }

                loadedMod = new Mod()
                {
                    Name = _modName,
                    Path = modPath,
                    Version = new SemVersion(1),
                    CreationDate = creationDate,
                    Files = modFiles
                };

                return loadedMod;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while parsing the mod file. Content is invalid.");
                throw;
            }
        }
    }
}