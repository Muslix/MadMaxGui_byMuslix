
using Domain;
using MadMaxGui.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private void LoadCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Xml Files (*.xml)|*.xml|All files (*.*)|*.*";
            fileDialog.InitialDirectory = s;
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;
            Config = loadSaveXml.loadData(fileDialog.FileName);

            MadmaxDir = Config.MadmaxDir;
            TempDir = Config.TempDir;
            TempDir2 = Config.TempDir2;
            FinalDir = Config.FinalDir;
            FarmerKey = Config.FarmerKey;
            ContractKey = Config.ContractKey;
            Buckets = Config.Buckets;
            BucketsPhaseThreeAndFour = Config.BucketsPhaseThreeAndFour;
            Threads = Config.Threads;
        }

        //Zum Speichern der Settings
        private void SaveCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Xml Files (*.xml)|*.xml|All files (*.*)|*.*";
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
            loadSaveXml.savedata(config, fileDialog.FileName);
            //  MadmaxParam = "-n 1 -r 8 -u 512 -v 256 -t " + TempDir + " -d " + FinalDir + " -c " + ContractKey + " -f " + FarmerKey;
        }

        public override Config GetConfig()
        {
            return Config;
        }
    }
}
