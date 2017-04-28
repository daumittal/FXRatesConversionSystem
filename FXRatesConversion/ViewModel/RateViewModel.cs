using System;
using System.ComponentModel;


namespace FXRatesConversion.ViewModel
{
    public class RateViewModel : INotifyPropertyChanged
    {
        private string fromCurrency { get; set; }
        private string toCurrency { get; set; }
        private double? rate { get; set; }
        private DateTime updateTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string FromCurrency
        {
            get { return fromCurrency; }

            set
            {
                if (value != fromCurrency)
                {
                    fromCurrency = value;
                    NotifyPropertyChanged("FromCurrency");
                }
            }
        }
        public string ToCurrency
        {
            get { return toCurrency; }

            set
            {
                if (value != toCurrency)
                {
                    toCurrency = value;
                    NotifyPropertyChanged("ToCurrency");
                }
            }
        }
        public double? Rate
        {
            get { return rate; }

            set
            {
                if (value != rate)
                {
                    rate = value;
                    NotifyPropertyChanged("Rate");
                }
            }
        }
        public DateTime UpdateTime
        {
            get { return updateTime; }

            set
            {
                if (value != updateTime)
                {
                    updateTime = value;
                    NotifyPropertyChanged("UpdateTime");
                }
            }
        }

        public int UpdateCount { get; set; }
    }
}
