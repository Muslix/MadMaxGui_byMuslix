using DataAccessLayer;
using Domain;
using MadMaxGui.Interfaces;
using MadMaxGui.ViewModels;
using MadMaxGui.Views;
using Ninject;
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
            kernel.Bind<IHomeView>().To<HomeView>().InSingletonScope();
            kernel.Bind<HomeViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
        }

    }
}
