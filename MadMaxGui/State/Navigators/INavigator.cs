﻿using MadMaxGui.ViewModels;
using System.Windows.Input;

namespace MadMaxGui.State.Navigators
{
    public enum ViewType
    {
        Home,
        Settings,
        Stuff
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
