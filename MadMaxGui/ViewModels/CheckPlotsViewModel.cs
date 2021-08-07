using Domain;
using MadMaxGui.Commands;
using MadMaxGui.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MadMaxGui.ViewModels
{
    public class CheckPlotsViewModel : ViewModelBase
    {
        private string daemonFolder;
        public string DaemonFolder
        {
            get => daemonFolder;
            set
            {
                daemonFolder = value;
                OnPropertyChanged();               
            }

        }
        private string plotsCheckOutput;
        public string PlotsCheckOutput
        {
            get => plotsCheckOutput;
            set
            {
                plotsCheckOutput = value;
                OnPropertyChanged();
            }

        }



        private Process myProcess;
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand DaemonPathCommand { get; }

        private readonly ILoadSaveXml loadSaveXml;
        public CheckPlotsViewModel(ILoadSaveXml loadSaveXml)
        {
            StartCommand = new RelayCommand(StartCommandExecute);
            StopCommand = new RelayCommand(StopCommandExecute);
            DaemonPathCommand = new RelayCommand(DaemonPathCommandExecute);
            this.loadSaveXml = loadSaveXml;
        }

        private void DaemonPathCommandExecute(object obj)
        {
            DaemonFolder = OpenFolder();
        }

        private void StartCommandExecute(object obj)
        {
            var path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                , "chia-blockchain"
                , "app-1.2.3"
                , "resources"
                , "app.asar.unpacked"
                , "daemon");

            if (!Directory.Exists(path))
                return;

            try
            {
                myProcess = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = @"cmd",
                        WorkingDirectory = path + @"\",
                        //   Arguments = "cd "+ path,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                );

                myProcess.StandardInput.WriteLine("chia plots check");
                myProcess.OutputDataReceived += P_OutputDataReceived;
                myProcess.ErrorDataReceived += P_ErrorDataReceived;

                myProcess.BeginOutputReadLine();
                myProcess.BeginErrorReadLine();
            }
            catch (Exception ew)
            {
                myProcess.Kill();
                throw new Exception(ew.Message);
            }
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            PlotsCheckOutput += e.Data + "\n";
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

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            PlotsCheckOutput += e.Data + "\n";
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
                return dialog.FileName.EndsWith(@"\") ? dialog.FileName : dialog.FileName.TrimEnd('\\');
            }
            return "";
        }
    }
}
