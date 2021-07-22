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
using System.IO;
using System.Text.Json;
using System.Windows.Threading;

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
        private PerformanceCounter cpuCounter;
        public PerformanceCounter CpuCounter
        {
            get => cpuCounter;
            set
            {
                cpuCounter = value;
               
                OnPropertyChanged();
            }

        }
        private PerformanceCounter ramCounter;
        public PerformanceCounter RamCounter
        {
            get => ramCounter;
            set
            {
                ramCounter = value;
               
                OnPropertyChanged();
            }

        }
        private string cpuCounterString;
        public string CpuCounterString
        {
            get => cpuCounterString;
            set
            {
                cpuCounterString = value;
                OnPropertyChanged();
            }

        }
        private string ramCounterString;
        public string RamCounterString
        {
            get => ramCounterString;
            set
            {
                ramCounterString = value;
                OnPropertyChanged();
            }

        }

        public ParamCreator Creator = new ();

        private Process myProcess;

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand LoadXmlCommand { get; }
        private readonly ILoadSaveXml loadSaveXml;
        private readonly IHomeView homeView;
        public HomeViewModel(ILoadSaveXml loadSaveXml, IHomeView homeView)
        {
            StartCommand = new RelayCommand(StartCommandExecute);
            StopCommand = new RelayCommand(StopCommandExecute);
            LoadXmlCommand = new RelayCommand(LoadXmlCommandExecute);
            this.loadSaveXml = loadSaveXml;
            this.homeView = homeView;

            CpuCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
            DispatcherTimerInit();
        }

        private void DispatcherTimerInit()
        {
            DispatcherTimer timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_tick;
            timer.Start();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            CpuCounterString = "CPU: " + (int)cpuCounter.NextValue() + " %";
            RamCounterString = "Ram: " + Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024  + " MB";
            
        }

        //Setter
        public override void SetConfig(Config config)
        {
            Config = config;
            MadmaxParam = Creator.Create(Config);
        }

        //Commands
        private async void LoadXmlCommandExecute(object obj)
        {
            var s = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = s
            };
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;         
            Config = await loadSaveXml.LoadData(fileDialog.FileName);
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
            try
            {
                myProcess = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = Config.MadmaxDir,
                        Arguments = MadmaxParam,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                );
                myProcess.OutputDataReceived += P_OutputDataReceived;            
                myProcess.BeginOutputReadLine();
                ProcessId = myProcess.Id;

            }
            catch (Exception ew)
            {
                myProcess.Kill();
                throw new Exception(ew.Message);
            }
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            MadMaxOutput += e.Data + "\n";
        }

        //public override void ContinueProcess(int id) 
        //{
        //    myProcess = Process.GetProcessById(id);
        //    myProcess.OutputDataReceived += p_OutputDataReceived;
        //    myProcess.BeginOutputReadLine();
        //}
    }
}
