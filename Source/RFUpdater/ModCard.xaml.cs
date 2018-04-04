using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RFUpdater
{
    /// <summary>
    /// Interaction logic for ModCard.xaml
    /// </summary>
    public partial class ModCard : UserControl
    {
        private bool modMandatory = false;

        public string ModName
        {
            get { return (string)GetValue(ModNameProperty); }
            set { SetValue(ModNameProperty, value); }
        }

        public string ModDescription
        {
            get { return (string)GetValue(ModDescriptionProperty); }
            set { SetValue(ModDescriptionProperty, value); }
        }

        public string ModVersion
        {
            get { return (string)GetValue(ModVersionProperty); }
            set { SetValue(ModVersionProperty, $"({value})"); }
        }

        public string ModCreationDate
        {
            get { return (string)GetValue(ModCreationDateProperty); }
            set { SetValue(ModCreationDateProperty, value); }
        }

        public string ModUpdateDate
        {
            get { return (string)GetValue(ModUpdateDateProperty); }
            set { SetValue(ModUpdateDateProperty, value); }
        }

        public bool ModMandatory
        {
            get { return modMandatory; }
            set
            {
                modMandatory = value;
                OnPropertyChanged("ModMandatory");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty ModNameProperty = DependencyProperty.Register("ModName", typeof(string), typeof(ModCard), new PropertyMetadata(null));
        public static readonly DependencyProperty ModDescriptionProperty = DependencyProperty.Register("ModDescription", typeof(string), typeof(ModCard), new PropertyMetadata(null));
        public static readonly DependencyProperty ModVersionProperty = DependencyProperty.Register("ModVersion", typeof(string), typeof(ModCard), new PropertyMetadata(null));
        public static readonly DependencyProperty ModCreationDateProperty = DependencyProperty.Register("ModCreationDate", typeof(string), typeof(ModCard), new PropertyMetadata(null));
        public static readonly DependencyProperty ModUpdateDateProperty = DependencyProperty.Register("ModUpdateDate", typeof(string), typeof(ModCard), new PropertyMetadata(null));

        public ModCard()
        {
            InitializeComponent();

            CardRoot.DataContext = this;
        }
    }
}