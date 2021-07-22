using Domain;
using MadMaxGui.State.Navigators;
using MadMaxGui.ViewModels;
using Ninject;
using System;
using System.Windows.Input;

namespace MadMaxGui.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly INavigator navigator;
        private readonly IKernel kernel;
        public UpdateCurrentViewModelCommand(INavigator navigator, IKernel kernel)
        {

            this.navigator = navigator;
            this.kernel = kernel;
            navigator.CurrentViewModel = kernel.Get<HomeViewModel>();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType viewType)
            {
                switch (viewType)
                {
                    case ViewType.Home:
                        Config confTmp = null;
                        if (navigator.CurrentViewModel is SettingsViewModel)
                        {
                            confTmp = navigator.CurrentViewModel.GetConfig();
                        }
                        navigator.CurrentViewModel = kernel.Get<HomeViewModel>();
                        if (confTmp != null)
                        {
                            navigator.CurrentViewModel.SetConfig(confTmp);

                        }
                        break;
                    case ViewType.Settings:
                        var temp = navigator.CurrentViewModel.ProcessId;
                        navigator.CurrentViewModel = kernel.Get<SettingsViewModel>();
                        navigator.CurrentViewModel.ProcessId = temp;
                        break;
                    case ViewType.CheckPlots:
                        navigator.CurrentViewModel = kernel.Get<CheckPlotsViewModel>();
                        break;

                }
            }
        }
    }
}
