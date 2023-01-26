using System;
using System.Collections.ObjectModel;
using System.Data;
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
        /// <summary>
        /// Создание БД
        /// </summary>
        public static void CreateBD()
        {
            Path = System.IO.Path.GetTempFileName();
            SQLiteConnection.CreateFile(Path);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + Path))
            {
                connection.Open();
                using (SQLiteCommand Command = new SQLiteCommand("CREATE TABLE \"Items\" ( \"TovarName\" TEXT, \"TovarCode\" TEXT, \"Articul\" TEXT, \"Sort\" TEXT, \"Razmer\" TEXT, \"Polnota\" TEXT, \"Izmirenie_Name\" TEXT, \"OKEICode\" TEXT, \"Price\" REAL, \"OtpushchenoCount\" REAL, \"OtpushcenoPrice\" REAL, \"SdanoCount\" REAL, \"SdanoPrice\" REAL, \"SellSumm\" REAL );", connection))
                {
                    Command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// Удаленние БД
        /// </summary>
        public static void DeleteBD()
        {
            File.Delete(Path);
        }

        public static void AddItem(Models.Item item)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data source = "+Path))
            {
                connection.Open();
                using (SQLiteCommand insertSQL = new SQLiteCommand($"INSERT INTO Items (Name, Surname, Pathnetic," +
                    $" Dolgnost, HourPay, Hours) VALUES" +
                    $" (\"{Client.Name}\",\"{Client.SurName}\",\"{Client.Pathnetic}\"," +
                    $"\"{Client.Dolgnost}\",:hourpay,{Client.Hours})", connection))
                {
                    try
                    {
                        insertSQL.Parameters.Add("hourpay", DbType.Double).Value = Client.HourPay;
                        insertSQL.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("")
                    }
                }
                connection.Close();
            }
        }


        public static ObservableCollection<Models.Item> GetClients(string Quary)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source = " + Path))
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
                    throw new Exception("Файл поврежден");
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

    public static class FileCheck
    {
        public static string ValidFileName { get; set; } = null;

        public static bool isFileNameValid(string fileName)
        {
            if ((fileName == null) || (fileName.IndexOfAny(Path.GetInvalidPathChars()) != -1))
                return false;
            try
            {
                new FileInfo(fileName);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}