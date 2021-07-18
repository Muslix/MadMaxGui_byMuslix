using Domain;
using MadMaxGui.Commands;
using MadMaxGui.State.Navigators;
using Ninject;
using System.Windows;


namespace MadMaxGui.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private WindowState _AktuellesFensterFormat;
        public WindowState AktuellesFensterFormat
        {
            get => _AktuellesFensterFormat;
            set
            {
                _AktuellesFensterFormat = value;
                OnPropertyChanged();
            }
        }

        public INavigator Navigator { get; set; }
        public RelayCommand MinimizeCommand { get;set;}
        public RelayCommand MaximizeCommand { get;set;}
        public RelayCommand ShutdownCommand { get;set;}

        private IKernel kernel;
        ILoadSaveXml loadSaveXml;
        public MainViewModel(ILoadSaveXml loadSaveXml, IKernel kernel) 
        {
            MaximizeCommand = new RelayCommand(MaximizeExecute);
            MinimizeCommand = new RelayCommand(MinimizeExecute);
            ShutdownCommand = new RelayCommand(ShutdownExecute);
            this.loadSaveXml = loadSaveXml;
            this.kernel = kernel;
            Navigator = kernel.Get<Navigator>();
        }

        private void MinimizeExecute(object x)
        {
            AktuellesFensterFormat = WindowState.Minimized;
        }

        private void MaximizeExecute(object x)
        {
            AktuellesFensterFormat = AktuellesFensterFormat == WindowState.Normal
                ? WindowState.Maximized
                : WindowState.Normal;
        }

        private void ShutdownExecute(object x)
        {
            Application.Current.Shutdown();
        }
    }
}
