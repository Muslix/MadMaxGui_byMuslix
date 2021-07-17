using Domain;
using MadMaxGui.Helper;
using MadMaxGui.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                MadmaxParam = "-n 1 -r " + Config.Threads + " -u " + Config.Buckets + " -v " + Config.BucketsPhaseThreeAndFour + " -t " + Config.TempDir + " -d " + Config.FinalDir + " -c " + Config.ContractKey + " -f " + Config.FarmerKey;
                OnPropertyChanged();
            }

        }

        //private string programmDir = @"C:\Users\Muslix\Desktop\chia neu\chia_plot.exe";
       // private string param = @"-n 1 -r 8 -u 512 -v 256 -t E:\ -d I:\ -c xch1clr8dvslj78hd2ed8jp7nvz9xnv3x7k2scyxk9w7ac2lt5ykrfjshzfxau -f 82057077ba9e2a7bd1b62bcf5e49d95d5f6d91a49ff0c94b8fb642506a312d4642cc2b89d646dcc9088483daf965ac8f";
        private Process myProcess;

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand LoadXmlCommand { get; }
        private ILoadSaveXml loadSaveXml;
        public HomeViewModel(ILoadSaveXml loadSaveXml) 
        {
            StartCommand = new RelayCommand(StartCommandExecute);     
            StopCommand = new RelayCommand(StopCommandExecute);
            LoadXmlCommand = new RelayCommand(LoadXmlCommandExecute);
            this.loadSaveXml = loadSaveXml;
        }


        //Setter
        public override void SetConfig(Config config) 
        {
            Config = config;
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
            ParamCreator creator = new ParamCreator(Config);
            MadmaxParam = creator.Create();
                //"-n 1 -r " + Config.Threads + " -u " + Config.Buckets + " -v " + Config.BucketsPhaseThreeAndFour + " -t " + Config.TempDir + " -d " + Config.FinalDir + " -c " + Config.ContractKey + " -f " + Config.FarmerKey;
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


                //var line = myProcess.StandardOutput.ReadLine();
                //    MadMaxOutput += line;
                    
                
               
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
    }
}
