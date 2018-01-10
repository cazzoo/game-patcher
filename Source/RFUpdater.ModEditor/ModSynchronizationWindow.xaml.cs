using RFUpdater.ModEditor.Properties;
using RFUpdater.Models;
using System;
using System.IO;
using System.Text;
using System.Windows;
using WinSCP;

namespace RFUpdater.ModEditor
{
    /// <summary>
    /// Interaction logic for ModSynchronizationWindow.xaml
    /// </summary>
    public partial class ModSynchronizationWindow : Window
    {
        private Mod Mod { get; }
        private string _lastFileProcessed;

        public ModSynchronizationWindow(Mod mod)
        {
            Mod = mod;
            InitializeComponent();
            ResetComponents();
        }

        private void ResetComponents()
        {
            statusText.Text = null;
            fileName.Content = null;
            filePercentage.Content = "0%";
            FileProgress.Value = 0;
            OverallPercentage.Content = "0%";
            OverallProgress.Value = 0;
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
                };

                using (Session session = new Session())
                {
                    // Will continuously report progress of synchronization
                    session.FileTransferred += FileTransferred;

                    // Will continuously report progress of transfer
                    session.FileTransferProgress += SessionFileTransferProgress;

                    string fingerprint = session.ScanFingerprint(sessionOptions);

                    sessionOptions.SshHostKeyFingerprint = fingerprint;

                    // Connect
                    session.Open(sessionOptions);

                    string localModPath = Path.Combine(Mod.Path);
                    string remoteModPath = Path.Combine(Settings.Default.RepositoryStoragePath, Mod.Name);

                    if (!session.FileExists(remoteModPath))
                    {
                        session.CreateDirectory(remoteModPath);
                    }

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
                        statusText.Text = "Operation succeeded.";
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
            FileProgress.Value = e.FileProgress * 100;

            // Remember a name of the last file reported
            _lastFileProcessed = e.FileName;
        }

        private void RemoteToLocal_Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LocalToRemote_Button_Click(object sender, RoutedEventArgs e)
        {
            ResetComponents();
            SynchronizeModToRemote();
        }
    }
}