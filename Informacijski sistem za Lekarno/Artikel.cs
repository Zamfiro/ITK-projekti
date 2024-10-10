using System;
using System.Collections.Generic;

namespace Lekarna
{
    class Artikel
    {
       public string ime { get; set; }
       public double cena { get; set; }
       public string drzava_proizvajalca { get; set; }
       public string naslov_distributerja { get; set; }
       public DateTime rok_trajanja { get; set; }
       public Skladisce skladisce { get; set; }
       public int ID { get; set; }
       
 

       public Artikel(int ID, string ime, double cena, string drzava_proizvajalca, string naslov_distributerja, DateTime rok_trajanja, Skladisce skladisce)
       {
           this.ID = ID;
           this.ime = ime;
           this.cena = cena;
           this.drzava_proizvajalca = drzava_proizvajalca;
           this.naslov_distributerja = naslov_distributerja;
           this.rok_trajanja = rok_trajanja;
           this.skladisce = skladisce;
       }

       public Artikel(string ime, double cena, DateTime rok_trajanja)
       {
           this.ime = ime;
           this.cena = cena;
           this.rok_trajanja = rok_trajanja;
       }

       public Artikel()
       {

       }

       public static int Do_Roka(DateTime rok_trajanja) 
       {
           DateTime trenutno = DateTime.Now;
           TimeSpan doRoka = rok_trajanja - trenutno;
            
           return doRoka.Days;
       }

        public static double Do_Roka(DateTime rok_trajanja, Skladisce skladisce)
        {
            double steviloDni = 0;
            
            if(skladisce == Skladisce.polica)
            {
                steviloDni = (rok_trajanja - DateTime.Now).TotalDays;
                return Math.Round(steviloDni, 1);
            }
            if (skladisce == Skladisce.hladilnik)
            {
                steviloDni = (rok_trajanja - DateTime.Now).TotalDays;
                return Math.Round(steviloDni * 1.5, 1);
            }

            steviloDni = (rok_trajanja - DateTime.Now).TotalDays;
            return  Math.Round(steviloDni,1);
        }
            
       public static void Izpis_o_artiklu(Artikel artikel)
       {
            Console.WriteLine("Ime: " + artikel.ime);
            Console.WriteLine("Cena: " + artikel.cena);
            Console.WriteLine("Država proizvajalca: " + artikel.drzava_proizvajalca);
            Console.WriteLine("Naslov distributerja: " + artikel.naslov_distributerja);
            Console.WriteLine("Rok trajanja: " + artikel.rok_trajanja.ToString("dd.MM.yyyy"));
            Console.WriteLine("Tip skladistenja: " + artikel.skladisce);
            Console.WriteLine("Do roka: " + Do_Roka(artikel.rok_trajanja, artikel.skladisce) + " dni");


            Console.WriteLine();
       }
    }
}