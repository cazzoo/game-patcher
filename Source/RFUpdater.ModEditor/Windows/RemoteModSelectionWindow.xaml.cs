using RFUpdater.ModEditor.Properties;
using System;
using System.Collections.Generic;
using System.Windows;
using WinSCP;
using System.Linq;
using RFUpdater.Models;

namespace RFUpdater.ModEditor
{
    /// <summary>
    /// Interaction logic for RemoteModSelectionWindow.xaml
    /// </summary>
    public partial class RemoteModSelectionWindow : Window
    {
        public RemoteModSelectionWindow()
        {
            InitializeComponent();
            FetchRemoteMods();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Mod mod = new Mod()
            {
                Name = remoteModList.SelectedValue.ToString(),
                ModStorePath = ModUtility.SelectModPath()
            };

            if (mod.IsPathDefined)
            {
                ModSynchronizationWindow synchronizationWindow = new ModSynchronizationWindow(ModSynchronizationWindow.Action.DOWNLOAD, mod);
                synchronizationWindow.Show();
                Close();
            }
        }

        private void FetchRemoteMods()
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
                    //string fp = "ssh-rsa 2048 iGUY6ftkfgyHQ+Qcz1ntutaiSed8CETlcVb6elUO/Zk=.";
                    //string fingerprint = session.ScanFingerprint(sessionOptions, "ssh-rsa");
                    //sessionOptions.SshHostKeyFingerprint = fp;

                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo remoteDirectoryInfo = session.ListDirectory(Settings.Default.RepositoryStoragePath);

                    List<RemoteFileInfo> modList = remoteDirectoryInfo.Files.Where(f => f.IsDirectory && !f.IsThisDirectory && !f.IsParentDirectory).ToList();

                    if (modList.Any())
                    {
                        remoteModList.ItemsSource = modList.Select(f => f.Name).ToList();
                        Import.IsEnabled = true;
                        remoteModList.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("No mods found in remote path [{0}]", Settings.Default.RepositoryStoragePath), "Error fetching remote mods.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
            }
        }
    }
}