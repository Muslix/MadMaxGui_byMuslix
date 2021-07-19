using Domain;
using MadMaxGui.Helper;
using MadMaxGui.Commands;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using MadMaxGui.Interfaces;
using System.Linq;

namespace MadMaxGui.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string madMaxOutput;
        public string MadMaxOutput
        {
            get => madMaxOutput;
            set
            {
                madMaxOutput = value;
                homeView.ScrollToEnd();
                OnPropertyChanged();
            }
                 
        }     
    
        private string madmaxParam;
        public string MadmaxParam
        {
            get => madmaxParam;
            set
            {
                madmaxParam = value;
                OnPropertyChanged();
            }
                 
        }
        private Config config;

        public Config Config
        {
            get => config;
            set
            {
                config = value;
                OnPropertyChanged();
            }

        }
        private string fileName;

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                OnPropertyChanged();
            }

        }

        public ParamCreator Creator = new ParamCreator();

        private Process myProcess;

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand LoadXmlCommand { get; }
        private ILoadSaveXml loadSaveXml;
        private readonly IHomeView homeView;
        public HomeViewModel(ILoadSaveXml loadSaveXml, IHomeView homeView)
        {
            StartCommand = new RelayCommand(StartCommandExecute);     
            StopCommand = new RelayCommand(StopCommandExecute);
            LoadXmlCommand = new RelayCommand(LoadXmlCommandExecute);
            this.loadSaveXml = loadSaveXml;
            this.homeView = homeView;
        }


        //Setter
        public override void SetConfig(Config config) 
        {
            Config = config;
            MadmaxParam = Creator.Create(Config);
        }

        //Commands
        private void LoadXmlCommandExecute(object obj)
        {
            var s = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Xml Files (*.xml)|*.xml|All files (*.*)|*.*";
            fileDialog.InitialDirectory = s;
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;
            Config = loadSaveXml.loadData(fileDialog.FileName);
            var strcut = fileDialog.FileName.Split(@"\").Last(); ;
            FileName = strcut;
            MadmaxParam = Creator.Create(Config);       
        }

        private void StopCommandExecute(object obj)
        {
            try
            {
                if (myProcess is null)
                    return;
                myProcess.Kill();
            }
            catch (Exception ew)
            {

                throw new Exception(ew.Message);
            }
        }

        public void StartCommandExecute(object obj)
        {
            if (MadmaxParam is null)
                return;
            myProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.MadmaxDir,
                    Arguments = MadmaxParam,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.OutputDataReceived += p_OutputDataReceived;
                myProcess.Start();
                myProcess.BeginOutputReadLine();
                ProcessId = myProcess.Id;             
               
            }
            catch (Exception ew)
            {
                myProcess.Kill();
                throw new(ew.Message);
            }
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            MadMaxOutput += e.Data + "\n";
        }

        public override void ContinueProcess(int id) 
        {
            myProcess = Process.GetProcessById(id);
            myProcess.OutputDataReceived += p_OutputDataReceived;
            myProcess.BeginOutputReadLine();
        }
    }
}
