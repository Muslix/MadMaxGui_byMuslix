using Domain;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MadMaxGui.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Config GetConfig()
        {
            return null;
        }
        public virtual void SetConfig(Config config) { }

    }
}
