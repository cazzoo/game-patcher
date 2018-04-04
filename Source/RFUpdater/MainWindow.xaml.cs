using HtmlAgilityPack;
using RFUpdater.Converters;
using RFUpdater.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Diagnostics;
using NLog;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RFUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mod workingMod;
        private const string constDateFormat = "dd MM yyyy HH:mm";
        private const string applicationIcon = @"pack://application:,,,/Resources/favicon.ico";
        private const string missingImageIcon = @"pack://application:,,,/Resources/MissingImage.png";
        private string remoteRepositoryUrl = "http://mods.racing-france.fr/";
        private string applicationTempPath = Path.Combine(Path.GetTempPath(), Process.GetCurrentProcess().ProcessName);

        private Logger logger = LogManager.GetCurrentClassLogger();

        public Mod Mod { get => workingMod; set => workingMod = value; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SynchronizeMods_Click(object sender, RoutedEventArgs e)
        {
            synchronizeMods.IsEnabled = false;
            applicationActionName.Text = "Fetching mods from remote...";
            AvailableMods.ItemsSource = null;
            List<string> remoteMods = GetModListFromRemoteUrl();

            List<Action> actions = new List<Action>();
            foreach (string remoteModName in remoteMods)
            {
                actions.Add(() => DownloadModDefinitionFile(remoteModName));
            }
            Parallel.Invoke(actions.ToArray());

            synchronizeMods.IsEnabled = true;
            applicationActionName.Text = "Task Finished!";

            AvailableMods.ItemsSource = remoteMods;
            AvailableMods.SelectionChanged += AvailableMods_SelectionChanged;
        }

        private void AvailableMods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMod = (AvailableMods.SelectedItem as string);
            if (selectedMod != null)
            {
                // Load mod from temp file
                applicationActionName.Text = $"selectedMod : {selectedMod}";
                Mod = Utils.ParseModFile(Path.Combine(applicationTempPath, $"{selectedMod}.json"));
                if (Mod.Icon != null)
                {
                    string modIcon = $"{remoteRepositoryUrl}{Mod.Name}/{Mod.Icon}";
                    DownloadFile(modIcon, 1);
                }
                BindObject();
            }
        }

        private List<string> GetModListFromRemoteUrl()
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(remoteRepositoryUrl);

                List<String> remoteMods = doc.DocumentNode.Descendants("a")
                    .Where(x => !x.InnerHtml.StartsWith(".."))
                    .Select(y => y.InnerHtml.Substring(0, y.InnerHtml.Length - 1).Trim())
                    .ToList();

                if (Directory.Exists(applicationTempPath))
                {
                    if (new DirectoryInfo(applicationTempPath).LastWriteTime < DateTime.Now.AddDays(-2))
                    {
                        Directory.Delete(applicationTempPath);
                    }
                }
                else
                {
                    Directory.CreateDirectory(applicationTempPath);
                }

                return remoteMods;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No mods found in the remote server.");
                logger.Warn(ex.Message);
                return new List<string>();
            }
        }

        private void DownloadModDefinitionFile(string remoteModName)
        {
            string modDefinition = $"{remoteRepositoryUrl}{remoteModName}/{remoteModName}.json";
            DownloadFile(modDefinition);
        }

        /// <summary>
        /// Download a file on a remote path if not existing on the system or existing but is not fresh.
        /// Default freshness is 12 hours.
        /// </summary>
        /// <param name="filePath">Remote file to download</param>
        /// <param name="freshenessInHours">Freshness of the file to check in hours (default : 12).</param>
        private void DownloadFile(string filePath, int freshenessInHours = 12)
        {
            try
            {
                Uri ur = new Uri(filePath);
                string filename = new FileInfo(ur.LocalPath).Name;
                string localFile = Path.Combine(applicationTempPath, filename);

                if (!File.Exists(localFile) || (File.Exists(localFile) && new DirectoryInfo(applicationTempPath).LastWriteTime < DateTime.Now.AddHours(-1 * freshenessInHours)))
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += WebClientDownloadProgressChanged;
                        wc.DownloadDataCompleted += WebClientDownloadCompleted;

                        wc.DownloadFile(ur, localFile);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                applicationActionName.Text = $"Download status: {e.ProgressPercentage}%.";
                applicationActionProgress.Value = e.ProgressPercentage;
            }));
        }

        private void WebClientDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            applicationActionName.Text = "Download finished!";
            applicationActionProgress.Value = 100;
        }

        private void BindObject()
        {
            Binding NameBinding = new Binding("Name")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modName.SetBinding(TextBox.TextProperty, NameBinding);

            Binding DescriptionBinding = new Binding("Description")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modDescription.SetBinding(TextBox.TextProperty, DescriptionBinding);

            Binding VersionBinding = new Binding("Version")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modVersion.SetBinding(TextBox.TextProperty, VersionBinding);

            Binding MandatoryBinding = new Binding("Mandatory")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modMandatory.SetBinding(ToggleButton.IsCheckedProperty, MandatoryBinding);

            Binding CreatedDateBinding = new Binding("CreationDate")
            {
                Mode = BindingMode.OneTime,
                Source = Mod,
                StringFormat = constDateFormat
            };
            modCreationDate.SetBinding(TextBox.TextProperty, CreatedDateBinding);

            Binding UpdatedDateBinding = new Binding("UpdateDate")
            {
                Mode = BindingMode.OneTime,
                Source = Mod,
                StringFormat = constDateFormat
            };
            modUpdateDate.SetBinding(TextBox.TextProperty, UpdatedDateBinding);

            Binding AuthorBinding = new Binding("Author")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modAuthor.SetBinding(TextBox.TextProperty, AuthorBinding);

            Binding ContactBinding = new Binding("Contact")
            {
                Mode = BindingMode.OneTime,
                Source = Mod
            };
            modContact.SetBinding(TextBox.TextProperty, ContactBinding);

            Binding CategoriesBinding = new Binding("Categories")
            {
                Mode = BindingMode.OneTime,
                Source = Mod,
                Converter = new ListToStringConverter()
            };
            modCategories.SetBinding(TextBox.TextProperty, CategoriesBinding);

            Binding TagsBinding = new Binding("Tags")
            {
                Mode = BindingMode.OneTime,
                Source = Mod,
                Converter = new ListToStringConverter()
            };
            modTags.SetBinding(TextBox.TextProperty, TagsBinding);

            if (Mod.Icon != null)
            {
                modImage.Source = new BitmapImage(new Uri($"{applicationTempPath}{Path.DirectorySeparatorChar}{Mod.Icon}"));
            }
            else
            {
                modImage.Source = new BitmapImage(new Uri(missingImageIcon));
            }
        }
    }
}