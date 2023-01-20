using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Praktika_2.Tools
{
    public static class DB
    {
        /// <summary>
        /// Путь к БД
        /// </summary>
        public static string Path { get; set; }

        public static void CreateBD()
        {
            Path = System.IO.Path.GetTempFileName();
            SQLiteConnection.CreateFile(Path);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = "+Path))
            {
                connection.Open();
                using (SQLiteCommand Command = new SQLiteCommand("CREATE TABLE \"Items\" ( \"TovarName\" TEXT, \"TovarCode\" TEXT, \"Articul\" TEXT, \"Sort\" TEXT, \"Razmer\" TEXT, \"Polnota\" TEXT, \"Izmirenie_Name\" TEXT, \"OKEICode\" TEXT, \"Price\" REAL, \"OtpushchenoCount\" REAL, \"OtpushcenoPrice\" REAL, \"SdanoCount\" REAL, \"SdanoPrice\" REAL, \"SellSumm\" REAL );", connection))
                {
                    Command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void DeleteBD()
        {
            File.Delete(Path);
        }

        public static ObservableCollection<Models.Item> GetClients(string Quary)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source = {Path}"))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand sql = new SQLiteCommand(Quary, connection))
                    {
                        SQLiteDataReader reader = sql.ExecuteReader();
                        ObservableCollection<Models.Item> list = new ObservableCollection<Models.Item>();
                        while (reader.Read())
                        {
                            list.Add(new Models.Item()
                            {
                                ItemName = reader.GetString(0),
                                ItemCode = reader.GetString(1),
                                Articul = reader.GetString(2),
                                Sort = reader.GetString(3),
                                Razmer = reader.GetString(4),
                                Model = reader.GetString(5),
                                IzmirenieName = reader.GetString(6),
                                OkeiCode = reader.GetString(7),
                                Price = reader.GetDouble(8),
                                OtpuchenoCount = reader.GetDouble(9),
                                OtpuchenoPrice = reader.GetDouble(10),
                                SdanoCount = reader.GetDouble(11),
                                SdanoPrice = reader.GetDouble(12),
                                Selled = reader.GetDouble(13)
                            });
                        }
                        connection.Close();
                        return list;
                    }
                }
                catch
                {
                    MessageBox.Show("Файл повреждён!");
                    Application.Current.Shutdown();
                    return null;
                }
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}