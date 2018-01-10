using ModEditor;
using RFUpdater.ModEditor.Properties;
using System.ComponentModel;

namespace RFUpdater.ModEditor
{
    public class OptionsViewModel : Observable
    {
        private static Settings Settings
        {
            get { return Settings.Default; }
        }

        [Category("Application|Repository")]
        public string RepositoryUrl
        {
            get { return Settings.RepositoryUrl; }
            set
            {
                Settings.RepositoryUrl = value;
                OnPropertyChanged("RepositoryUrl");
            }
        }

        [Category("Application|Repository")]
        public string RepositoryStoragePath
        {
            get { return Settings.RepositoryStoragePath; }
            set
            {
                Settings.RepositoryStoragePath = value;
                OnPropertyChanged("RepositoryStoragePath");
            }
        }

        [Category("Application|Repository")]
        public string RepositoryName
        {
            get { return Settings.RepositoryName; }
            set
            {
                Settings.RepositoryName = value;
                OnPropertyChanged("RepositoryName");
            }
        }

        [Category("Application|Repository")]
        public string RepositoryPassword
        {
            get { return Settings.RepositoryPassword; }
            set
            {
                Settings.RepositoryPassword = value;
                OnPropertyChanged("RepositoryPassword");
            }
        }

        [Category("Application|Repository")]
        public string RepositoryPrivateKeyPath
        {
            get { return Settings.RepositoryPrivateKeyPath; }
            set
            {
                Settings.RepositoryPrivateKeyPath = value;
                OnPropertyChanged("RepositoryPrivateKeyPath");
            }
        }

        public void Reset()
        {
            Settings.Reset();
        }

        public void Save()
        {
            Settings.Save();
        }
    }
}