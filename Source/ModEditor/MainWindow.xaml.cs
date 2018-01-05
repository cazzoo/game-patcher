using Microsoft.Win32;
using Newtonsoft.Json;
using RFUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ModEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ModFile> modFiles = new List<ModFile>();

        public MainWindow()
        {
            InitializeComponent();

            FillTable();
        }

        private void FillTable()
        {
            modFiles = new List<ModFile>() {
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521},
            new ModFile() {FileName = "1", FilePath = "p+/", Deletable = true, FileHash = 216, FileSize= 23 },
            new ModFile() {FileName = "2", FilePath = "erfdgodf/egsdfg15/ergsdf", Deletable = false, FileHash = 2785716, FileSize= 236521}
            };

            dataGrid.ItemsSource = modFiles;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateModForm())
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = String.Format("{0}.json", modName.Text),
                Filter = "Json file (*.json)|*.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string modNameWithoutExtenstion = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                Mod mod = new Mod()
                {
                    Name = modNameWithoutExtenstion,
                    Files = modFiles,
                    FolderName = modNameWithoutExtenstion
                };

                string output = JsonConvert.SerializeObject(mod, Formatting.Indented);

                string jsonFile = Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), string.Format("{0}.json", modNameWithoutExtenstion));
                using (StreamWriter streamWriter = new StreamWriter(jsonFile))
                {
                    streamWriter.Write(output);
                }
            }
        }

        private bool ValidateModForm()
        {
            if (string.IsNullOrEmpty(modName.Text))
            {
                MessageBoxResult result = MessageBox.Show("Name is empty, please fill name", "Validation error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close the application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}