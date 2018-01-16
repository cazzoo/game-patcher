using Microsoft.WindowsAPICodePack.Dialogs;
using ModEditor.Validators;
using PropertyTools.Wpf;
using RFUpdater.ModEditor;
using RFUpdater.ModEditor.Converters;
using RFUpdater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Linq;

namespace ModEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mod loadedMod;
        private Mod workingMod;
        private const string constDateFormat = "dd MM yyyy";
        private const string applicationIcon = @"pack://application:,,,/Resources/favicon.ico";
        private const string missingImageIcon = @"pack://application:,,,/Resources/MissingImage.png";
        private BackgroundWorker worker;
        private int addedFiles;

        public Mod Mod { get => workingMod; set => workingMod = value; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorker();
            InitModObject();
            BindObject();
        }

        private void InitializeWorker()
        {
            worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        #region methods

        public void BindObject()
        {
            Binding NameBinding = new Binding("Name")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            NameBinding.ValidationRules.Clear();
            NameBinding.ValidationRules.Add(new StringEmptyValidationError());
            modName.SetBinding(TextBox.TextProperty, NameBinding);

            Binding ModDirectoryBinding = new Binding("ModDirectory")
            {
                Mode = BindingMode.OneWay,
                Source = Mod
            };
            modDirectory.SetBinding(TextBox.TextProperty, ModDirectoryBinding);

            Binding DescriptionBinding = new Binding("Description")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            modDescription.SetBinding(TextBox.TextProperty, DescriptionBinding);

            Binding VersionBinding = new Binding("Version")
            {
                Mode = BindingMode.OneWay,
                Source = Mod
            };
            modVersion.SetBinding(TextBox.TextProperty, VersionBinding);

            Binding PasswordBinding = new Binding("Password")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            modPassword.SetBinding(TextBox.TextProperty, PasswordBinding);

            Binding MandatoryBinding = new Binding("Mandatory")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            modMandatory.SetBinding(ToggleButton.IsCheckedProperty, MandatoryBinding);

            Binding CreatedDateBinding = new Binding("CreationDate")
            {
                Mode = BindingMode.OneWay,
                Source = Mod,
                StringFormat = constDateFormat
            };
            modCreationDate.SetBinding(TextBox.TextProperty, CreatedDateBinding);

            Binding UpdatedDateBinding = new Binding("UpdateDate")
            {
                Mode = BindingMode.OneWay,
                Source = Mod,
                StringFormat = constDateFormat
            };
            modUpdateDate.SetBinding(TextBox.TextProperty, UpdatedDateBinding);

            Binding AuthorBinding = new Binding("Author")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            modAuthor.SetBinding(TextBox.TextProperty, AuthorBinding);

            Binding ContactBinding = new Binding("Contact")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod
            };
            modContact.SetBinding(TextBox.TextProperty, ContactBinding);

            Binding CategoriesBinding = new Binding("Categories")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod,
                Converter = new ListToStringConverter()
            };
            modCategories.SetBinding(TextBox.TextProperty, CategoriesBinding);

            Binding TagsBinding = new Binding("Tags")
            {
                Mode = BindingMode.TwoWay,
                Source = Mod,
                Converter = new ListToStringConverter()
            };
            modTags.SetBinding(TextBox.TextProperty, TagsBinding);

            modFilesGrid.ItemsSource = Mod.Files;
        }

        private void InitModObject()
        {
            Mod = new Mod();
            modImage.Source = new BitmapImage(new Uri(missingImageIcon));
        }

        private void SaveModFile()
        {
            try
            {
                Mod.Files = ModFileUtility.CopyModFilesToModPath(Mod, loadedMod).Files;
                Mod = ModUtility.SaveToFile(Mod);
                BindObject();
                loadedMod = Mod;
            }
            catch
            {
                throw;
            }
        }

        private void OpenModFile()
        {
            CommonOpenFileDialog openModDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                Multiselect = false,
                IsFolderPicker = false,
                AllowNonFileSystemItems = false,
                Title = "Select Mod file",
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
                try
                {
                    Mod = ModUtility.LoadFile(selectedFile);
                    if (!String.IsNullOrEmpty(Mod.Icon))
                    {
                        modImage.Source = new BitmapImage(new Uri(Path.Combine(Mod.ModDirectory, Mod.Icon)));
                    }
                    BindObject();
                    loadedMod = Mod.ShallowCopy();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void ImportOldModFileFormat()
        {
            CommonOpenFileDialog openModDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                Multiselect = false,
                IsFolderPicker = false,
                AllowNonFileSystemItems = false,
                Title = "Select old format Mod file",
                DefaultExtension = ".txt"
            };
            openModDialog.Filters.Add(new CommonFileDialogFilter("Text file(*.txt)", " *.txt"));
            var result = openModDialog.ShowDialog();

            if (result != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("No file selected");
                return;
            }

            if (result == CommonFileDialogResult.Ok)
            {
                try
                {
                    Mod = ModUtility.LoadOldFile(openModDialog.FileName);
                    BindObject();
                    loadedMod = Mod;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void QuitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close the application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void AddFileToModFilesList(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!ModFileUtility.IsFileLocked(fileInfo))
            {
                ModFile currentModFile = ModFileUtility.GetModFile(fileInfo);
                if (!Mod.Files.Contains(currentModFile, new ModFileComparer()))
                {
                    addedFiles++;
                    Mod.Files.Add(currentModFile);
                }
            }
        }

        private void DisableButtons()
        {
            SelectFiles.IsEnabled = false;
            SelectFolder.IsEnabled = false;
        }

        private void EnableButtons()
        {
            SelectFiles.IsEnabled = true;
            SelectFolder.IsEnabled = true;
        }

        #endregion methods

        #region Events

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Mod.IsPathDefined)
            {
                Mod.ModStorePath = Path.Combine(ModUtility.SelectModPath(), Mod.Name);
            }
            if (Mod.IsPathDefined)
            {
                SaveModFile();
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Mod.ModStorePath = Path.Combine(ModUtility.SelectModPath(), Mod.Name);
            if (Mod.IsPathDefined)
            {
                SaveModFile();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenModFile();
        }

        private void ImportOldFormat_Click(object sender, RoutedEventArgs e)
        {
            ImportOldModFileFormat();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            QuitApplication();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            QuitApplication();
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (fileList.Length > 1)
            {
                MessageBox.Show("Please select only one image file.");
                return;
            }
            if (!(sender is Image))
            {
                MessageBox.Show("Incorrect file type. Please select image file.");
                return;
            }
            if (!Mod.IsPathDefined || !File.Exists(Path.Combine(Mod.ModStorePath, String.Format("{0}.json", Mod.Name))))
            {
                MessageBox.Show("Please set mod path and save it before setting the image.");
                return;
            }

            string fileExtension = Path.GetExtension(fileList[0]);
            string modImageFilename = String.Format("{0}{1}", Mod.Name, fileExtension);
            try
            {
                string modImagePath = Path.Combine(Mod.ModStorePath, modImageFilename);
                File.Copy(fileList[0], modImagePath, true);

                Mod.Icon = modImageFilename;

                //Update UI element
                modImage.Source = new BitmapImage(new Uri(modImagePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to move file to mod directory [{0}]. Please check path and user permissions.", Mod.ModStorePath);
            }
        }

        private void NewMod_Click(object sender, RoutedEventArgs e)
        {
            InitModObject();
            BindObject();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog() { Owner = this };
            var options = new OptionsViewModel();

            dlg.DataContext = options;
            dlg.Title = "Options";
            if (dlg.ShowDialog().Value)
                options.Save();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog(this)
            {
                Title = "About the application",
                UpdateStatus = "The application is updated.",
                Image = new BitmapImage(new Uri(applicationIcon))
            };
            dlg.ShowDialog();
        }

        private void ImportFromRemote_Click(object sender, RoutedEventArgs e)
        {
            RemoteModSelectionWindow remoteModSelection = new RemoteModSelectionWindow();
            remoteModSelection.Show();
        }

        private void ExportToRemote_Click(object sender, RoutedEventArgs e)
        {
            ModSynchronizationWindow synchWindow = new ModSynchronizationWindow(ModSynchronizationWindow.Action.UPLOAD, Mod);
            synchWindow.ShowDialog();
        }

        private void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFolderDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = true,
                Multiselect = true,
                IsFolderPicker = false,
                AllowNonFileSystemItems = false,
                Title = "Select Files to add to the list"
            };
            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Dictionary<FileAttributes, List<string>> selectedPaths = new Dictionary<FileAttributes, List<string>>
                {
                    { FileAttributes.Normal, openFolderDialog.FileNames.ToList() }
                };

                if (!worker.IsBusy)
                {
                    DisableButtons();
                    worker.RunWorkerAsync(selectedPaths);
                }
            }
        }

        private void SelectFolders_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFolderDialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                Multiselect = true,
                IsFolderPicker = true,
                AllowNonFileSystemItems = false,
                Title = "Select a Folder add to the list"
            };
            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Dictionary<FileAttributes, List<string>> selectedPaths = new Dictionary<FileAttributes, List<string>>
                {
                    { FileAttributes.Directory, openFolderDialog.FileNames.ToList() }
                };

                if (!worker.IsBusy)
                {
                    DisableButtons();
                    worker.RunWorkerAsync(selectedPaths);
                }
            }
        }

        #endregion Events

        #region worker

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            addedFiles = 0;
            Dictionary<FileAttributes, List<string>> selection = (Dictionary<FileAttributes, List<string>>)e.Argument;
            List<string> paths = selection.FirstOrDefault().Value;
            foreach (string path in paths)
            {
                if (FileAttributes.Directory.Equals(selection.FirstOrDefault().Key))
                {
                    // Importing all files (including sub folders) in case of directories
                    string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    int countFiles = files.Length;

                    for (int i = 0; i < countFiles; i++)
                    {
                        AddFileToModFilesList(files[i]);
                        worker.ReportProgress(100 * (i + 1) / countFiles, files[i]);
                    }
                }
                else
                {
                    // Otherwise, import only selected files
                    int countFiles = paths.Count;
                    AddFileToModFilesList(path);
                    worker.ReportProgress(100 * (paths.IndexOf(path) + 1) / countFiles, path);
                }
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AddFilesProgress.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            modFilesGrid.Items.Refresh();
            EnableButtons();
            MessageBox.Show(String.Format("{0} new item(s) were added to the list.", addedFiles.ToString()));
        }

        #endregion worker
    }

    public class Observable : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}