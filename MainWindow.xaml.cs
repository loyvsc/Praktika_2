using System.Windows;

namespace Praktika_2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModels.MainWindowViewModel();
            InitializeComponent();
        }
    }
}