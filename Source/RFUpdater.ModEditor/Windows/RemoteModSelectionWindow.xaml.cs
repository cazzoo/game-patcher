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

        private void FetchRemoteMods()
        {
            HashSet<Mod> modList = ModUtility.FetchRemoteModList();
            if (modList.Any())
            {
                remoteModList.ItemsSource = modList.Select(f => f.Name).ToList();
                Import.IsEnabled = true;
                remoteModList.SelectedIndex = 0;
            }
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
                ModSynchronizationWindow synchronizationWindow = new ModSynchronizationWindow(SynchronizationMode.Local, mod);
                synchronizationWindow.Show();
                Close();
            }
        }
    }
}