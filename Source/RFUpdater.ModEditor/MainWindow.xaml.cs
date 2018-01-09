using Microsoft.WindowsAPICodePack.Dialogs;
using ModEditor.Validators;
using Newtonsoft.Json;
using PropertyTools.Wpf;
using RFUpdater.ModEditor;
using RFUpdater.ModEditor.Converters;
using RFUpdater.Models;
using Semver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ModEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mod mod;
        private const string constDateFormat = "dd MM yyyy";

        public Mod Mod { get => mod; set => mod = value; }

        public MainWindow()
        {
            InitializeComponent();
            InitModObject();
            BindObject();
        }

        #region methods

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

            Binding PathBinding = new Binding("Path")
            {
                Mode = BindingMode.OneWay,
                Source = Mod
            };
            modPath.SetBinding(TextBox.TextProperty, PathBinding);

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
            modImage.Source = new BitmapImage(new Uri("Resources/MissingImage.png", UriKind.Relative));
        }

        private bool SelectModPath()
        {
            bool returnStatus = false;
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

            if (result == CommonFileDialogResult.Ok)
            {
                Mod.Path = openFolderDialog.FileName;
                returnStatus = true;
            }
            return returnStatus;
        }

        private void SaveModFile()
        {
            SetUpdatedDate();
            SetVersion();

            string output = JsonConvert.SerializeObject(Mod, Formatting.Indented);

            string jsonFile = Path.Combine(Mod.Path, string.Format("{0}.json", Mod.Name));
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(jsonFile))
                {
                    streamWriter.Write(output);
                }
                BindObject();

                MessageBox.Show(String.Format("[{0}] mod has been successfully saved.", Mod.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Unable to save [{0}] mod on following path : [{1}].", Mod.Name, Mod.Path));
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
                string stringifiedMod;
                using (StreamReader file = File.OpenText(selectedFile))
                {
                    stringifiedMod = file.ReadToEnd();
                }
                try
                {
                    Mod = JsonConvert.DeserializeObject<Mod>(stringifiedMod);

                    string currentPath = Path.GetDirectoryName(selectedFile);
                    Mod.Path = currentPath;

                    modImage.Source = new BitmapImage(new Uri(Path.Combine(currentPath, Mod.Icon)));

                    BindObject();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while parsing the mod file. Content is invalid.");
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
                Title = "Select The Old Mod",
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
                string selectedFile = openModDialog.FileName;
                string modPath = Path.GetDirectoryName(selectedFile);
                DateTime creationDate = File.GetCreationTime(selectedFile);
                Mod loadedMod;
                try
                {
                    string _modName = Path.GetFileNameWithoutExtension(selectedFile);
                    List<ModFile> modFiles = new List<ModFile>();

                    using (StreamReader reader = File.OpenText(selectedFile))
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

                    Mod = loadedMod;

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
            int nextMajor = 1;
            int nextMinor = 0;
            if (null != Mod.Version)
            {
                nextMajor = Mod.Version.Major;
                nextMinor = Mod.Version.Minor;
                if (nextMinor == 9)
                {
                    nextMajor++;
                    nextMinor = 0;
                }
                else
                {
                    nextMinor++;
                }
            }
            Mod.Version = new SemVersion(nextMajor, nextMinor);
        }

        private void QuitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close the application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion methods

        #region Events

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            bool modPathDefined = !String.IsNullOrEmpty(Mod.Path);
            if (!modPathDefined)
            {
                modPathDefined = SelectModPath();
            }
            if (modPathDefined)
            {
                SaveModFile();
            }
        }

        private void Save_As_Button_Click(object sender, RoutedEventArgs e)
        {
            bool modPathDefined = SelectModPath();
            if (modPathDefined)
            {
                SaveModFile();
            }
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenModFile();
        }

        private void Import_Old_Format_Button_Click(object sender, RoutedEventArgs e)
        {
            ImportOldModFileFormat();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            QuitApplication();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
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
            if (string.IsNullOrEmpty(Mod.Path) || !File.Exists(Path.Combine(Mod.Path, String.Format("{0}.json", Mod.Name))))
            {
                MessageBox.Show("Please set mod path and save it before setting the image.");
                return;
            }

            string fileExtension = Path.GetExtension(fileList[0]);
            string modImageFilename = String.Format("{0}{1}", Mod.Name, fileExtension);
            try
            {
                string modImagePath = Path.Combine(Mod.Path, modImageFilename);
                File.Copy(fileList[0], modImagePath, true);

                Mod.Icon = modImageFilename;

                //Update UI element
                modImage.Source = new BitmapImage(new Uri(modImagePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to move file to mod directory [{0}]. Please check path and user permissions.", Mod.Path);
            }
        }

        private void NewMod_Click(object sender, RoutedEventArgs e)
        {
            InitModObject();
            BindObject();
        }

        #endregion Events

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
                Image = new BitmapImage(new Uri(@"pack://application:,,,/Resources/favicon.ico"))
            };
            dlg.ShowDialog();
        }
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