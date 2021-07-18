using MadMaxGui.Commands;
using MadMaxGui.Models;
using MadMaxGui.ViewModels;
using Ninject;
using System.Windows.Input;

namespace MadMaxGui.State.Navigators
{
    public class Navigator : ObservableObject, INavigator
    {
        private ViewModelBase currentViewModel;
        private IKernel kernel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public Navigator(IKernel kernel)
        {

            this.kernel = kernel;
        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this, kernel);
    }
}
