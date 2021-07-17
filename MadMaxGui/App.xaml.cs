using DataAccessLayer;
using Domain;
using MadMaxGui.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MadMaxGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel kernel;
        protected override void OnStartup(StartupEventArgs e)
        {
           
            ConfigureContainer();
            Window window = new MainWindow();
            window.DataContext = kernel.Get<MainViewModel>();
            window.Show();

            base.OnStartup(e);
        }
        private void ConfigureContainer()
        {
            this.kernel = new StandardKernel();
            kernel.Bind<ILoadSaveXml>().To<LoadSaveXml>().InSingletonScope();
        }

    }
}
