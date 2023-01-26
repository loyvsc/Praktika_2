using Microsoft.Win32;
using Praktika_2.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Praktika_2.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            if (Tools.FileCheck.ValidFileName != null)
            {
                OpenFile(Tools.FileCheck.ValidFileName);
            }
        }

        public bool FileSaved = false;

        public ObservableCollection<Models.Item> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                }
                OnPropertyChanged("Items");
            }
        }

        #region Properties

        private ObservableCollection<Models.Item> _items;
        public event PropertyChangedEventHandler PropertyChanged;
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
                return sum;
            }
        }
        public bool FileCreatedOrOpened
        {
            get { return _fileCreated; }
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
        public ICommand Savefile { get { return new RelayCommand(SaveFile); } }
        #endregion

        private void CreateFile(object obj)
        {
            try
            {
                Tools.DB.CreateBD();
                FileCreatedOrOpened = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("При создании файла произошла ошибка:\n" + e.Message, "Создание накладной", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddItem(object obj)
        {
            MessageBox.Show("добавление клиента");
        }

        private void SaveFile(object obj)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = "Расходно-приходная накладная|*.rpnf",
                    Title = "Сохранение накладной"
                };
                if (dialog.ShowDialog() == true)
                {
                    File.Move(Tools.DB.Path, dialog.FileName);
                    FileSaved = true;
                    MessageBox.Show("Сохранено!", "Сохранение накладной", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else throw new System.Exception("Путь сохранения не выбран!");
            }
            catch (Exception e)
            {
                MessageBox.Show("При сохранении накладной возникла ошибка:\n" + e.Message, "Сохранение наклмдной", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenFile(object fileName)
        {
            void Open(string path)
            {
                try
                {
                    Tools.DB.Path = path;
                    Items = Tools.DB.GetClients("SELECT * FROM Items");
                }
                catch (Exception e)
                {
                    MessageBox.Show("При открытии накладной возникла ошибка:\n" + e.Message, "Открытие накладной", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            string check = fileName as string;
            if (check.Length > 0)
            {
                Open(check);
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Filter = "Расходно-приходная накладная|*.rpnf",
                    Title = "Сохранение накладной"
                };
                if (dialog.ShowDialog() != false)
                {
                    Open(dialog.FileName);
                }
            }

        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (FileCreatedOrOpened == true && FileSaved == false)
            {
                if (MessageBox.Show("Выйти из приложения без сохранения?", "Закрытие приложения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    SaveFile(sender);
                    e.Cancel = true;
                }
                else Tools.DB.DeleteBD();
            }
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}