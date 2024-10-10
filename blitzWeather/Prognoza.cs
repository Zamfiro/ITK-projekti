using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blitzWeather
{
    public class Prognoza : INotifyPropertyChanged
    {
        DateTime dateTime;
        string daydate;
        Bitmap slika;
        string dnevnatempmin;
        string dnevnatempmax;

        public DateTime Datum { get { return dateTime; } set { dateTime = value; NotifyPropertyChanged("Datum"); } }
        public string DanVTednu { get { return daydate; } set { daydate = value; NotifyPropertyChanged("DanVTednu"); } }
        public Bitmap DnevnaSlika { get { return slika; } set { slika = value; NotifyPropertyChanged("DnevnaSlika"); } }
        public string DnevnaTemperaturaMin { get { return dnevnatempmin; } set { dnevnatempmin = value; NotifyPropertyChanged("DnevnaTemperaturaMin"); } }
        public string DnevnaTemperaturaMax { get { return dnevnatempmax; } set { dnevnatempmax = value; NotifyPropertyChanged("DnevnaTemperaturaMax"); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
