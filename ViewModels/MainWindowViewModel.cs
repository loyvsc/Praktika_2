using Praktika_2.Tools;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Praktika_2.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        public bool FileSaved = false;

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (FileCreatedOrOpened == true && FileSaved==false)
            {
                if (MessageBox.Show("Закрытие приложения", "Выйти из приложения бех сохранения?", MessageBoxButton.YesNo, MessageBoxImage.Warning)==MessageBoxResult.No)
                {

                    return;
                }                
            }
        }

        public ObservableCollection<Models.Item> Items
        {
            get => _items;
            set
            {
                if (_items!= value)
                {
                    _items = value;
                }
                OnPropertyChanged("Items");
            }
        }

        #region Properties

        private ObservableCollection<Models.Item> _items;
        private event PropertyChangedEventHandler PropertyChanged;
        private bool _fileCreated = false;
        public string ItemsCount
        {
            get
            {
                if (FileCreatedOrOpened == false)
                {
                    return "Кол-во товаров: 0";
                }
                return "Кол-во товаров: " + Items.Count;
            }
        }
        public string ItemsOtpushcheno
        {
            get
            {
                if (FileCreatedOrOpened == false)
                {
                    return "Отпущено: 0\nОтмущено на сумму: 0";
                }
                return $"Отпущено: {OtpushchenoCount}\nОтмущено на сумму: 0";
            }
        }
        public double OtpushchenoCount
        {
            get
            {
                double sum = 0;
                foreach (var item in Tools.DB.GetClients("SELECT [OtpushchenoCount] FROM [Items]"))
                {
                    sum += item.OtpuchenoCount;
                }
                OnPropertyChanged("OtpushchenoCount");
                return sum;
            }
        }
        public bool FileCreatedOrOpened
        {
            get => _fileCreated;
            set
            {
                _fileCreated = value;
                OnPropertyChanged("FileCreatedOrOpened");
            }
        }
        #endregion

        #region Commands
        public ICommand ADDItem { get { return new RelayCommand(AddItem); } }
        public ICommand CreateNewFile { get { return new RelayCommand(CreateFile); } }
        #endregion

        public void CreateFile(object obj)
        {
            Tools.DB.CreateBD();
            FileCreatedOrOpened = true;
            MessageBox.Show("");
        }

        public void AddItem(object obj)
        {
            MessageBox.Show("добавление клиента");
        }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}