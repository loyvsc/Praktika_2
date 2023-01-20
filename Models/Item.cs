using System.ComponentModel;

namespace Praktika_2.Models
{
    public class Item
    {
        #region PrivatePropertys
        public event PropertyChangedEventHandler PropertyChanged;
        private string _itemName;
        private string _itemCode;
        private string _articul;
        private string _sort;
        private string _size;
        private string _model;
        private string _izmirenieName;
        private string _okeiCode;
        private double _price;
        private double _otpuchneCount;
        private double _otpuchnePrice;
        private double _sdanoCount;
        private double _sdanoPrice;
        private double _sellPrice;
        #endregion

        #region PublicPropertys
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged("ItemName");
            }
        }
        public string ItemCode
        {
            get => _itemCode;
            set
            {
                _itemCode = value;
                OnPropertyChanged("ItemCode");
            }
        }
        public string Articul
        {
            get => _articul;
            set
            {
                _articul = value;
                OnPropertyChanged("Articul");
            }
        }
        public string Sort
        {
            get => _sort;
            set
            {
                _sort = value;
                OnPropertyChanged("Sort");
            }
        }
        public string Razmer
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged("Razmer");
            }
        }
        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }
        public string IzmirenieName
        {
            get => _izmirenieName;
            set
            {
                _izmirenieName = value;
                OnPropertyChanged("IzmirenieName");
            }
        }
        public string OkeiCode
        {
            get => _okeiCode;
            set
            {
                _okeiCode = value;
                OnPropertyChanged("OkeiCode");
            }
        }
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
        public double OtpuchenoCount
        {
            get => _otpuchneCount;
            set
            {
                _otpuchneCount = value;
                OnPropertyChanged("OtpuchenoCount");
            }
        }
        public double OtpuchenoPrice
        {
            get => _otpuchnePrice;
            set
            {
                _otpuchnePrice = value;
                OnPropertyChanged("OtpuchenoPrice");
            }
        }
        public double SdanoCount
        {
            get => _sdanoCount;
            set
            {
                _sdanoCount = value;
                OnPropertyChanged("SdanoCount");
            }
        }
        public double SdanoPrice
        {
            get => _sdanoPrice;
            set
            {
                _sdanoPrice = value;
                OnPropertyChanged("SdanoPrice");
            }
        }   
        public double Selled
        {
            get => _sellPrice;
            set
            {
                _sellPrice = value;
                OnPropertyChanged("Selled");
            }
        }
        #endregion

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}