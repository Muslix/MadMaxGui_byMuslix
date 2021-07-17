
using Domain;
using MadMaxGui.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        private readonly ILoadSaveXml loadSaveXml;
        public SettingsViewModel(ILoadSaveXml loadSaveXml)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            this.loadSaveXml = loadSaveXml;
        }

        private void LoadCommandExecute(object obj)
        {
            Config config = loadSaveXml.loadData("test.xml");

            MadmaxDir = config.MadmaxDir;
            TempDir = config.TempDir;
            TempDir2 = config.TempDir2;
            FinalDir = config.FinalDir;
            FarmerKey = config.FarmerKey;
            ContractKey = config.ContractKey;
        }

        private void SaveCommandExecute(object obj)
        {

            Config config = new Config();
            config.MadmaxDir = MadmaxDir;
            config.TempDir = TempDir;
            config.TempDir2 = TempDir2;
            config.FinalDir = FinalDir;
            config.FarmerKey = FarmerKey;
            config.ContractKey = ContractKey;

            loadSaveXml.savedata(config, "test.xml");
            //  MadmaxParam = "-n 1 -r 8 -u 512 -v 256 -t " + TempDir + " -d " + FinalDir + " -c " + ContractKey + " -f " + FarmerKey;
        }
    }
}
