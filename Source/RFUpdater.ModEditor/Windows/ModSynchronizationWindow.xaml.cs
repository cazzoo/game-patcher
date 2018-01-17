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
using System.Windows.Threading;
using System.Threading;

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
        private SynchronizationMode _synchronizationMode;
        private SessionOptions _sessionOptions = GetSessionOptions();
        private string _localModPath;
        private string _remoteModPath;

        public enum WindowAction
        {
            DOWNLOAD,
            UPLOAD
        }

        public ModSynchronizationWindow(SynchronizationMode synchronizationMode, Mod mod)
        {
            Mod = mod;
            InitializeComponent();
            ResetComponents();
            _synchronizationMode = synchronizationMode;
            _localModPath = Path.Combine(Mod.ModDirectory);
            _remoteModPath = Path.Combine(Settings.Default.RepositoryStoragePath, Mod.Name);
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

        private static SessionOptions GetSessionOptions()
        {
            return new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.Default.RepositoryUrl,
                UserName = Settings.Default.RepositoryName,
                Password = Settings.Default.RepositoryPassword,
                SshPrivateKeyPath = Settings.Default.RepositoryPrivateKeyPath,
                PrivateKeyPassphrase = "altER3g0$",
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
        }

        private void SynchronizeModToRemote()
        {
            try
            {
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
                    session.Open(_sessionOptions);

                    if (!session.FileExists(_remoteModPath))
                    {
                        session.CreateDirectory(_remoteModPath);
                    }

                    HashSet<string> filesToSynchronize = new HashSet<string>(Directory.GetFiles(_localModPath, "*.*", SearchOption.AllDirectories).ToList());
                    _countFilesToSynchronize = filesToSynchronize.Count;

                    SynchronizationResult synchronizationResult;
                    synchronizationResult =
                        session.SynchronizeDirectories(SynchronizationMode.Remote, _localModPath, _remoteModPath, true);

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
                        Dispatcher.Invoke(new Action(() =>
                        {
                            overallPercentage.Content = string.Format("{0:P0}", 1);
                            overallProgress.Value = 100;
                            statusText.Text = "Operation succeeded.";
                        }));
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
                    session.Open(_sessionOptions);

                    if (!session.FileExists(_remoteModPath))
                    {
                        MessageBox.Show(string.Format("The mod [{0}] doesn't exist in the remote repository [{1}].", Mod.Name, Settings.Default.RepositoryStoragePath));
                        return;
                    }

                    if (!Directory.Exists(_localModPath))
                    {
                        Directory.CreateDirectory(_localModPath);
                    }
                    RemoteDirectoryInfo remoteDirectoryInfo = session.ListDirectory(_remoteModPath);
                    _countFilesToSynchronize = remoteDirectoryInfo.Files.Count;

                    SynchronizationResult synchronizationResult =
                        session.SynchronizeDirectories(SynchronizationMode.Local, _localModPath, _remoteModPath, true);

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
                        Dispatcher.Invoke(new Action(() =>
                        {
                            overallPercentage.Content = string.Format("{0:P0}", 1);
                            overallProgress.Value = 100;
                            statusText.Text = "Operation succeeded.";

                            string modName = Path.GetFileName(_localModPath);
                            Mod synchedMod = ModUtility.LoadFile(Path.Combine(_localModPath, string.Format("{0}.json", modName)));
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(MainWindow))
                                {
                                    (window as MainWindow).Mod = synchedMod;
                                    (window as MainWindow).BindObject();
                                }
                            }
                        }));
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
            string fileName = e.FileName.Replace(_remoteModPath, "");
            if (e.Error == null)
            {
                returnMessage = $"Upload of {e.FileName} succeeded";
            }
            else
            {
                returnMessage = $"Upload of {e.FileName} failed: {e.Error}";
            }

            _countFilesProcessed++;

            Dispatcher.Invoke(new Action(() =>
            {
                statusText.Text = returnMessage;
                double currentOverallPercentage = (double)_countFilesProcessed / _countFilesToSynchronize;
                overallProgress.Value = currentOverallPercentage * 100;
                overallPercentage.Content = string.Format("{0:P0}", currentOverallPercentage);
            }));
        }

        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (_lastFileProcessed != null && _lastFileProcessed != e.FileName)
                {
                    fileName.Content = String.Format("File [{0}] processed.", e.FileName);
                }
                fileProgress.Value = e.FileProgress * 100;
                filePercentage.Content = String.Format("{0:P0}", e.FileProgress);
            }));

            // Remember a name of the last file reported
            _lastFileProcessed = e.FileName;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Thread scpThread = null;
            switch (_synchronizationMode)
            {
                case SynchronizationMode.Local:
                    scpThread = new Thread(SynchronizeModToLocal);
                    break;

                case SynchronizationMode.Remote:
                    scpThread = new Thread(SynchronizeModToRemote);
                    break;
            }

            if (scpThread != null)
            {
                scpThread.Start();
            }
        }
    }
}