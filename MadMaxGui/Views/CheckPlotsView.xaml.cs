using MadMaxGui.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MadMaxGui.Views
{
    /// <summary>
    /// Interaktionslogik für CheckPlotsView.xaml
    /// </summary>
    public partial class CheckPlotsView : UserControl, IHomeView
    {
        public CheckPlotsView()
        {
            InitializeComponent();
        }
        public void ScrollToEnd()
        {
            UITraceScroll.ScrollToBottom();
        }
    }
}
