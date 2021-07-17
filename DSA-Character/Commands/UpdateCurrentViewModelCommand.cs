using MadMaxGui.State.Navigators;
using MadMaxGui.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MadMaxGui.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private INavigator navigator;
        IKernel kernel;
        public UpdateCurrentViewModelCommand(INavigator navigator, IKernel kernel)
        {
            this.navigator = navigator;
            this.kernel = kernel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        navigator.CurrentViewModel = kernel.Get<HomeViewModel>();
                        break;
                    case ViewType.Charakter:
                        navigator.CurrentViewModel = kernel.Get <SettingsViewModel>();
                        break;
                    case ViewType.Stuff:
                        break;
                   
                }
            }
        }
    }
}
