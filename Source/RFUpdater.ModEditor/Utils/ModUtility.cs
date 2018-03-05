using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using RFUpdater.Models;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using WinSCP;
using System.Linq;
using RFUpdater.ModEditor.Properties;

namespace RFUpdater.ModEditor
{
    public static class ModUtility
    {
        public static Mod SaveToFile(Mod mod)
        {
            mod.SetUpdatedDate();
            mod.IncrementVersion();

            mod.Files.ForEach(f => f.FilePath = f.FilePath.Replace(mod.ModStorePath, ""));

            string output = JsonConvert.SerializeObject(mod, Formatting.Indented);

            string jsonFile = Path.Combine(mod.ModDirectory, string.Format("{0}.json", mod.Name));
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
                MessageBox.Show(String.Format("Unable to save [{0}] mod on following path : [{1}].", mod.Name, mod.ModStorePath));
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
                mod.ModStorePath = Directory.GetParent(fileName).Parent.FullName;
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
                    ModStorePath = modPath,
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

        public static string SelectModPath()
        {
            CommonOpenFileDialog selectPathDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = false,
                EnsureFileExists = false,
                Multiselect = false,
                IsFolderPicker = true,
                AllowNonFileSystemItems = false,
                Title = "Select Mod root path"
            };
            var result = selectPathDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                return selectPathDialog.FileName;
            }
            return null;
        }

        public static HashSet<Mod> FetchRemoteModList()
        {
            try
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = Settings.Default.RepositoryUrl,
                    UserName = Settings.Default.RepositoryName,
                    Password = Settings.Default.RepositoryPassword,
                    SshPrivateKeyPath = Settings.Default.RepositoryPrivateKeyPath,
                    PrivateKeyPassphrase = "altER3g0$",
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };

                using (Session session = new Session())
                {
                    //string fp = "ssh-rsa 2048 iGUY6ftkfgyHQ+Qcz1ntutaiSed8CETlcVb6elUO/Zk=.";
                    //string fingerprint = session.ScanFingerprint(sessionOptions, "ssh-rsa");
                    //sessionOptions.SshHostKeyFingerprint = fp;

                    session.Open(sessionOptions);

                    TransferOptions transferOptions = new TransferOptions()
                    {
                        FileMask = "*.json",
                    };
                    TransferOperationResult transferResult;
                    transferResult = session.GetFiles(Settings.Default.RepositoryStoragePath, Path.GetTempPath(), false, transferOptions);
                    transferResult.Check();

                    if (!transferResult.Transfers.Any())
                    {
                        MessageBox.Show(string.Format("No mods found in remote path [{0}]", Settings.Default.RepositoryStoragePath), "Error fetching remote mods.");
                        return new HashSet<Mod>();
                    }
                    HashSet<Mod> mods = new HashSet<Mod>();
                    foreach (TransferEventArgs transferedFile in transferResult.Transfers)
                    {
                        var mod = LoadFile(transferedFile.Destination);
                        mods.Add(mod);
                    }
                    return mods;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                return new HashSet<Mod>();
            }
        }
    }
}