﻿using Microsoft.WindowsAPICodePack.Dialogs;
using ModEditor.Validators;
using Newtonsoft.Json;
using RFUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mod mod = new Mod();

        public Mod Mod { get => mod; set => mod = value; }

        public MainWindow()
        {
            InitializeComponent();
            InitModObject();
            BindObject();
        }

        private void BindObject()
        {
            Binding NameBinding = new Binding("Name")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            NameBinding.ValidationRules.Clear();
            NameBinding.ValidationRules.Add(new StringEmptyValidationError());
            modName.SetBinding(TextBox.TextProperty, NameBinding);

            Binding VersionBinding = new Binding("Version")
            {
                Mode = BindingMode.OneWay,
                Source = Mod
            };
            modVersion.SetBinding(TextBox.TextProperty, VersionBinding);

            Binding CreationDateBinding = new Binding("CreationDate")
            {
                Mode = BindingMode.OneWay,
                Source = Mod
            };
            modCreationDate.SetBinding(TextBox.TextProperty, CreationDateBinding);
        }

        private void InitModObject()
        {
            Mod.Name = "";
            Mod.CreationDate = DateTime.Now;
            Mod.Files = new List<ModFile>() {
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

            dataGrid.ItemsSource = Mod.Files;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFolderDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                Multiselect = false,
                IsFolderPicker = true,
                AllowNonFileSystemItems = false,
                Title = "Select The Folder To Process"
            };
            var result = openFolderDialog.ShowDialog();

            if (result != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("No folder selected");
                return;
            }

            if (result == CommonFileDialogResult.Ok)
            {
                SetUpdatedDate();
                SetVersion();

                string selectedFolder = openFolderDialog.FileName;
                string output = JsonConvert.SerializeObject(Mod, Formatting.Indented);

                string jsonFile = Path.Combine(selectedFolder, string.Format("{0}.json", Mod.Name));
                using (StreamWriter streamWriter = new StreamWriter(jsonFile))
                {
                    streamWriter.Write(output);
                }

                MessageBox.Show(String.Format("{0} has been successfully saved.", Mod.Name));
            }
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openModDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                Multiselect = false,
                IsFolderPicker = false,
                AllowNonFileSystemItems = false,
                Title = "Select The Mod",
                DefaultExtension = ".json"
            };
            openModDialog.Filters.Add(new CommonFileDialogFilter("Mod file (*.json)", "*.json"));
            var result = openModDialog.ShowDialog();

            if (result != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("No file selected");
                return;
            }

            if (result == CommonFileDialogResult.Ok)
            {
                string selectedFile = openModDialog.FileName;
                Mod loadedMod;
                string stringifiedMod;
                using (StreamReader file = File.OpenText(selectedFile))
                {
                    stringifiedMod = file.ReadToEnd();
                }
                try
                {
                    Mod = JsonConvert.DeserializeObject<Mod>(stringifiedMod);
                    BindObject();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while parsing the mod file. Content is invalid.");
                }
            }
        }

        private void SetUpdatedDate()
        {
            if (null != Mod.Version)
            {
                Mod.UpdateDate = DateTime.Now;
            }
        }

        private void SetVersion()
        {
            int nextBuild = 1;
            int nextMajor = 0;
            if (null != Mod.Version)
            {
                nextBuild = Mod.Version.Build;
                nextMajor = Mod.Version.Major;
                if (nextMajor == 9)
                {
                    nextBuild++;
                    nextMajor = 0;
                }
                else
                {
                    nextMajor++;
                }
                Mod.Version = new Version(nextBuild, nextMajor);
            }
            Mod.Version = new Version(nextBuild, nextMajor, 0, 0);
        }

        private void QuitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close the application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            QuitApplication();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            QuitApplication();
        }
    }
}