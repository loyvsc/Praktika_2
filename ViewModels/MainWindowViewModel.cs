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
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        public bool FileSaved = false;

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (FileCreatedOrOpened == true && FileSaved == false)
            {
                if (MessageBox.Show("Выйти из приложения бех сохранения?", "Закрытие приложения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    SaveFile(sender);
                    e.Cancel = true;
                }
                else Tools.DB.DeleteBD();
            }
        }

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
                OnPropertyChanged("OtpushchenoCount");
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

        public void CreateFile(object obj)
        {
            Tools.DB.CreateBD();
            FileCreatedOrOpened = true;
        }

        public void AddItem(object obj)
        {
            MessageBox.Show("добавление клиента");
        }

        public void SaveFile(object obj)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = "Расходно-приходная накладная|*.rpnf",
                    AddExtension = true,
                    Title = "Сохранение наклодной"
                };
                if (dialog.ShowDialog() == true)
                {
                    File.Move(Tools.DB.Path, dialog.FileName);
                    FileSaved = true;
                    throw new System.Exception("Сохранено!");
                }
                else throw new System.Exception("Путь сохранения не выбран!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Сохранение наклодной", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}