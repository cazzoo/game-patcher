using RFUpdater.ModEditor.Properties;
using RFUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using WinSCP;
using System.Linq;
using ModEditor;

namespace RFUpdater.ModEditor
{
    /// <summary>
    /// Interaction logic for ModSynchronizationWindow.xaml
    /// </summary>
    public partial class ModSynchronizationWindow : Window
    {
        private Mod Mod { get; }
        private string _lastFileProcessed;
        private int _countFilesToSynchronize;
        private int _countFilesProcessed;
        private Action _selectedAction;

        public enum Action
        {
            DOWNLOAD,
            UPLOAD
        }

        public ModSynchronizationWindow(Action selectedAction, Mod mod)
        {
            Mod = mod;
            InitializeComponent();
            ResetComponents();
            _selectedAction = selectedAction;
        }

        private void ResetComponents()
        {
            _countFilesToSynchronize = 0;
            _countFilesProcessed = 0;
            statusText.Text = null;
            fileName.Content = null;
            filePercentage.Content = "0%";
            fileProgress.Value = 0;
            overallPercentage.Content = "0%";
            overallProgress.Value = 0;
        }

        private void SynchronizeModToRemote()
        {
            try
            {
                // Setup session options
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

                string localModPath = Path.Combine(Mod.ModDirectory);
                string remoteModPath = Path.Combine(Settings.Default.RepositoryStoragePath, Mod.Name);

                using (Session session = new Session())
                {
                    // Will continuously report progress of synchronization
                    session.FileTransferred += FileTransferred;

                    // Will continuously report progress of transfer
                    session.FileTransferProgress += SessionFileTransferProgress;

                    //string fp = "ssh-rsa 2048 iGUY6ftkfgyHQ+Qcz1ntutaiSed8CETlcVb6elUO/Zk=.";
                    //string fingerprint = session.ScanFingerprint(sessionOptions, "ssh-rsa");
                    //sessionOptions.SshHostKeyFingerprint = fp;

                    // Connect
                    session.Open(sessionOptions);

                    if (!session.FileExists(remoteModPath))
                    {
                        session.CreateDirectory(remoteModPath);
                    }

                    HashSet<string> filesToSynchronize = new HashSet<string>(Directory.GetFiles(localModPath, "*.*", SearchOption.AllDirectories).ToList());
                    _countFilesToSynchronize = filesToSynchronize.Count;

                    SynchronizationResult synchronizationResult;
                    synchronizationResult =
                        session.SynchronizeDirectories(SynchronizationMode.Remote, localModPath, remoteModPath, true);

                    // Throw on any error
                    synchronizationResult.Check();

                    StringBuilder failedFiles = new StringBuilder();
                    foreach (TransferEventArgs failedFile in synchronizationResult.Failures)
                    {
                        failedFiles.AppendLine(string.Format("{0}", failedFile.FileName));
                    }

                    if (failedFiles.Length > 0)
                    {
                        MessageBox.Show(failedFiles.ToString(), "Error uploading files.");
                    }

                    if (synchronizationResult.IsSuccess)
                    {
                        overallPercentage.Content = string.Format("{0:P0}", 1);
                        overallProgress.Value = 100;
                        statusText.Text = "Operation succeeded.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
            }
        }

        private void SynchronizeModToLocal()
        {
            try
            {
                // Setup session options
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
                    // Will continuously report progress of synchronization
                    session.FileTransferred += FileTransferred;

                    // Will continuously report progress of transfer
                    session.FileTransferProgress += SessionFileTransferProgress;

                    //string fp = "ssh-rsa 2048 iGUY6ftkfgyHQ+Qcz1ntutaiSed8CETlcVb6elUO/Zk=.";
                    //string fingerprint = session.ScanFingerprint(sessionOptions, "ssh-rsa");
                    //sessionOptions.SshHostKeyFingerprint = fp;

                    // Connect
                    session.Open(sessionOptions);

                    string localModPath = Path.Combine(Mod.ModDirectory);
                    string remoteModPath = Path.Combine(Settings.Default.RepositoryStoragePath, Mod.Name);

                    if (!session.FileExists(remoteModPath))
                    {
                        MessageBox.Show(string.Format("The mod [{0}] doesn't exist in the remote repository [{1}].", Mod.Name, Settings.Default.RepositoryStoragePath));
                        return;
                    }

                    if (!Directory.Exists(localModPath))
                    {
                        Directory.CreateDirectory(localModPath);
                    }
                    RemoteDirectoryInfo remoteDirectoryInfo = session.ListDirectory(remoteModPath);
                    _countFilesToSynchronize = remoteDirectoryInfo.Files.Count;

                    SynchronizationResult synchronizationResult;
                    synchronizationResult =
                        session.SynchronizeDirectories(SynchronizationMode.Local, localModPath, remoteModPath, true);

                    // Throw on any error
                    synchronizationResult.Check();

                    StringBuilder failedFiles = new StringBuilder();
                    foreach (TransferEventArgs failedFile in synchronizationResult.Failures)
                    {
                        failedFiles.AppendLine(string.Format("{0}", failedFile.FileName));
                    }

                    if (failedFiles.Length > 0)
                    {
                        MessageBox.Show(failedFiles.ToString(), "Error downloading files.");
                    }

                    if (synchronizationResult.IsSuccess)
                    {
                        overallPercentage.Content = string.Format("{0:P0}", 1);
                        overallProgress.Value = 100;
                        statusText.Text = "Operation succeeded.";

                        string modName = Path.GetFileName(localModPath);
                        Mod synchedMod = ModUtility.LoadFile(Path.Combine(localModPath, string.Format("{0}.json", modName)));
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(MainWindow))
                            {
                                (window as MainWindow).Mod = synchedMod;
                                (window as MainWindow).BindObject();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
            }
        }

        private void FileTransferred(object sender, TransferEventArgs e)
        {
            string returnMessage = string.Empty;
            if (e.Error == null)
            {
                returnMessage = $"Upload of {e.FileName} succeeded";
            }
            else
            {
                returnMessage = $"Upload of {e.FileName} failed: {e.Error}";
            }

            if (e.Chmod != null)
            {
                if (e.Chmod.Error == null)
                {
                    returnMessage = $"Permissions of {e.Chmod.FileName} set to {e.Chmod.FilePermissions}";
                }
                else
                {
                    returnMessage = $"Setting permissions of {e.Chmod.FileName} failed: {e.Chmod.Error}";
                }
            }
            else
            {
                returnMessage = $"Permissions of {e.Destination} kept with their defaults";
            }

            if (e.Touch != null)
            {
                if (e.Touch.Error == null)
                {
                    returnMessage = string.Format("Timestamp of {0} set to {1}", e.Touch.FileName, e.Touch.LastWriteTime);
                }
                else
                {
                    returnMessage = string.Format("Setting timestamp of {0} failed: {1}", e.Touch.FileName, e.Touch.Error);
                }
            }
            else
            {
                // This should never happen during "local to remote" synchronization
                returnMessage = string.Format("Timestamp of {0} kept with its default (current time)", e.Destination);
            }
            Console.WriteLine(returnMessage);
            statusText.Text = returnMessage;

            UpdateOverallProgress();
        }

        private void UpdateOverallProgress()
        {
            _countFilesProcessed++;
            double currentOverallPercentage = (double)_countFilesProcessed / _countFilesToSynchronize;
            overallPercentage.Content = string.Format("{0:P0}", currentOverallPercentage);
            overallProgress.Value = currentOverallPercentage * 100;
        }

        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            // New line for every new file
            if (_lastFileProcessed != null && _lastFileProcessed != e.FileName)
            {
                fileName.Content = String.Format("File [{0}] processed.", e.FileName);
            }

            // Print transfer progress
            string percentage = String.Format("{0:P1}", e.FileProgress);
            filePercentage.Content = percentage;
            fileProgress.Value = e.FileProgress * 100;

            // Remember a name of the last file reported
            _lastFileProcessed = e.FileName;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            switch (_selectedAction)
            {
                case Action.DOWNLOAD:
                    SynchronizeModToLocal();
                    break;

                case Action.UPLOAD:
                    SynchronizeModToRemote();
                    break;
            }
        }
    }
}