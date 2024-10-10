using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blitzWeather
{
    public class Mesto : INotifyPropertyChanged
    {
        string ime;
        string drzava;
        string temperatura;
        string vlagaZraka;
        Bitmap ikonica;
        string stanje;
        List<Prognoza> prognozaList;
        Prognoza prognoza;
        

        public string Ime { get { return ime; } set { ime = value; NotifyPropertyChanged("Ime"); } }
        public string Drzava { get { return drzava; } set { drzava = value; NotifyPropertyChanged("Drzava"); } }
        public string Temperatura { get { return temperatura; } set { temperatura = value; NotifyPropertyChanged("Temperatura"); } }
        public string VlagaZraka { get { return vlagaZraka; } set { vlagaZraka = value; NotifyPropertyChanged("VlagaZraka"); } }
        public Bitmap Ikonica { get { return ikonica; } set { ikonica = value; NotifyPropertyChanged("Ikonica"); } }
        public string Stanje { get { return stanje; } set { stanje = value; NotifyPropertyChanged("Stanje"); } }
        public Prognoza Prognoza { get { return prognoza; } set { prognoza = value; NotifyPropertyChanged("Prognoza"); } }
        public List<Prognoza> SeznamPrognoz { get { return prognozaList; } set { prognozaList = value; NotifyPropertyChanged("SeznamPrognoz"); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        
        public Mesto() { }

    }
}
