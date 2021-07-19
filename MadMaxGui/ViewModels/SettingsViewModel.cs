using Domain;
using MadMaxGui.Commands;
using Microsoft.Win32;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Text.Json;
using System.IO;

namespace MadMaxGui.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        private string madmaxDir;
        public string MadmaxDir
        {
            get => madmaxDir;
            set
            {
                madmaxDir = value;
                OnPropertyChanged();
            }

        }
        private string contractKey;
        public string ContractKey
        {
            get => contractKey;
            set
            {
                contractKey = value;
                OnPropertyChanged();
            }

        }
        private string farmerKey;
        public string FarmerKey
        {
            get => farmerKey;
            set
            {
                farmerKey = value;
                OnPropertyChanged();
            }

        }
        private string tempDir;
        public string TempDir
        {
            get => tempDir;
            set
            {
                tempDir = value;
                OnPropertyChanged();
            }

        }
        private string tempDir2;
        public string TempDir2
        {
            get => tempDir2;
            set
            {
                tempDir2 = value;
                OnPropertyChanged();
            }

        }
        private string finalDir;
        public string FinalDir
        {
            get => finalDir;
            set
            {
                finalDir = value;
                OnPropertyChanged();
            }

        }
        private string bucketsPhaseThreeAndFour;
        public string BucketsPhaseThreeAndFour
        {
            get => bucketsPhaseThreeAndFour;
            set
            {
                bucketsPhaseThreeAndFour = value;
                OnPropertyChanged();
            }

        }
        private string buckets = "256";
        public string Buckets
        {
            get => buckets;
            set
            {
                buckets = value;
                OnPropertyChanged();
            }

        }
        private string threads = "8";
        public string Threads
        {
            get => threads;
            set
            {
                threads = value;
                OnPropertyChanged();
            }

        }
        private string numberOfPlots = "1";
        public string NumberOfPlots
        {
            get => numberOfPlots;
            set
            {
                numberOfPlots = value;
                OnPropertyChanged();
            }

        }
        private Config config = new ();

        public Config Config 
        {
            get => config;
            set
            {
                config = value;
                OnPropertyChanged();
            }

        }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        private readonly ILoadSaveXml loadSaveXml;
        public SettingsViewModel(ILoadSaveXml loadSaveXml)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            this.loadSaveXml = loadSaveXml;
        }

        //Zum Laden der Settings 
        private async void LoadCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*";
            fileDialog.InitialDirectory = s;
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName) || File.Exists(fileDialog.FileName))
                return;

            await using var fs = new FileStream(fileDialog.FileName, FileMode.Open);
            Config = await JsonSerializer.DeserializeAsync<Config>(fs);

        }

        //Zum Speichern der Settings
        private async void SaveCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*";
            fileDialog.InitialDirectory = s;
            fileDialog.ShowDialog();
            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;
            Config.MadmaxDir = MadmaxDir;
            Config.TempDir = TempDir;
            Config.TempDir2 = TempDir2;
            Config.FinalDir = FinalDir;
            Config.FarmerKey = FarmerKey;
            Config.ContractKey = ContractKey;
            Config.Buckets = Buckets;
            Config.BucketsPhaseThreeAndFour = BucketsPhaseThreeAndFour;
            Config.Threads = Threads;
            Config.NumberOfPLots = NumberOfPlots;
            
            var options = new JsonSerializerOptions() { WriteIndented = true };
            await using var fs = new FileStream(fileDialog.FileName, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, Config, options);
        }

        public override Config GetConfig()
        {
            if (Config.GetType()
                 .GetProperties() //get all properties on object
                 .Select(pi => pi.GetValue(Config)) //get value for the property
                 .Any(value => value != null))
                return Config;
            return null;
        }
    }
}
