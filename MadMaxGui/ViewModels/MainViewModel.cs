using Domain;
using MadMaxGui.State.Navigators;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadMaxGui.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; } 

        private IKernel kernel;
        ILoadSaveXml loadSaveXml;
        public MainViewModel(ILoadSaveXml loadSaveXml, IKernel kernel) 
        {
            this.loadSaveXml = loadSaveXml;
            this.kernel = kernel;
            Navigator = kernel.Get<Navigator>();
        }


    }
}
