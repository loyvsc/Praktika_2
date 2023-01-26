using System.Windows;

namespace Praktika_2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1 && Tools.FileCheck.isFileNameValid(e.Args[0]))
            {
                Tools.FileCheck.ValidFileName = e.Args[0];
            }
        }
    }
}