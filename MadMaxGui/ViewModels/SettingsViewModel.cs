using Domain;
using MadMaxGui.Commands;
using Microsoft.Win32;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Text.Json;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

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
        private Config config = new();

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
        public ICommand TempDirCommand { get; }
        public ICommand TempDir2Command { get; }
        public ICommand FinalDirCommand { get; }
        private readonly ILoadSaveXml loadSaveXml;
        public SettingsViewModel(ILoadSaveXml loadSaveXml)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            TempDirCommand = new RelayCommand(TempDirCommandExecute);
            TempDir2Command = new RelayCommand(TempDir2CommandExecute);
            FinalDirCommand = new RelayCommand(FinalDirCommandExecute);
            this.loadSaveXml = loadSaveXml;
        }

        private void FinalDirCommandExecute(object obj)
        {
            FinalDir = OpenFolder();
        }

        private void TempDir2CommandExecute(object obj)
        {
            TempDir2 = OpenFolder();
        }

        private void TempDirCommandExecute(object obj)
        {
            TempDir = OpenFolder();
        }

        private static string OpenFolder()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = currentFolder
            };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileName.EndsWith(@"\") ? dialog.FileName : dialog.FileName + @"\";
            }
            return "";
        }

        //Zum Laden der Settings 
        private async void LoadCommandExecute(object obj)
        {
            var s = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = s
            };
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName) || !File.Exists(fileDialog.FileName))
                return;



            Config = await loadSaveXml.LoadData(fileDialog.FileName);

            MadmaxDir = Config.MadmaxDir;
            TempDir = Config.TempDir;
            TempDir2 = Config.TempDir2;
            FinalDir = Config.FinalDir;
            FarmerKey = Config.FarmerKey;
            ContractKey = Config.ContractKey;
            Buckets = Config.Buckets;
            BucketsPhaseThreeAndFour = Config.BucketsPhaseThreeAndFour;
            Threads = Config.Threads;
            NumberOfPlots = Config.NumberOfPLots;

        }

        //Zum Speichern der Settings
        private void SaveCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            SaveFileDialog fileDialog = new();
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

            loadSaveXml.Savedata(Config, fileDialog.FileName);
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
