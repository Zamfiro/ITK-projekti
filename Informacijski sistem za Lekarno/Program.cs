using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lekarna
{

    enum Enota {μg, mg, ml, g, l, dag, dcg, none}
    //enum Tip_Zdravila {mazilo, tableta, sirup, inhalator, ampula}
    enum Skladisce {hladilnik, polica}

    class Program
    {
        
        static void Main(string[] args)
        {

            Artikel artikelMazilo = new Artikel(1, "Badger", 9.98, "New Hampshire", "Grajski trg 41, 8360 Zuzemberk, Slovenija", DateTime.Now.AddYears(2), Skladisce.polica );
            Artikel artikelRaztopina = new Artikel(2, "ActiMaris Forte", 31.85, "Svica", "Grajski trg 41, 8360 Zuzemberk, Slovenija", DateTime.Now.AddMonths(3), Skladisce.polica);
            Artikel artikelTableta = new Artikel(3, "BlokMax", 5.60, "Makedonija", "Bul. Aleksandar Makedonski 12, 1000 Skopje, Republika Severna Makedonija", DateTime.Now.AddDays(31), Skladisce.polica);
            Artikel artikelInhalator = new Artikel(4, "Wellion Mesh", 79.99, "Slovenija", "Grajski trg 41, 8360 Zuzemberk, Slovenija",  DateTime.MaxValue, Skladisce.polica);
            Artikel artikelAmpula = new Artikel(5, "Dr. Temt Collagen", 20.30, "Avstrija", "Grajski trg 41, 8360 Zuzemberk, Slovenija", DateTime.Now.AddDays(28), Skladisce.hladilnik);

            Sestavine_Zdravila sestavineMazilo = new Sestavine_Zdravila("Rastlinska osnova", "Naravna olja", Enota.g, 20);
            Sestavine_Zdravila sestavineRaztopina = new Sestavine_Zdravila("Ionizirana voda", "Aktivni kisik", Enota.ml, 300);
            Sestavine_Zdravila sestavineTableta = new Sestavine_Zdravila("Ibuprofen", "ibuprofen", Enota.mg, 200);
            Sestavine_Zdravila sestavineInhalator = new Sestavine_Zdravila("/", "/", Enota.none, 0);
            Sestavine_Zdravila sestavineAmpula = new Sestavine_Zdravila("Rastlinski kolagen", "Kolagen", Enota.ml, 5*5);
            

            List<Sestavine_Zdravila> seznamSestavinMazilo = new List<Sestavine_Zdravila>() {sestavineMazilo};
            List<Sestavine_Zdravila> seznamSestavinRaztopina = new List<Sestavine_Zdravila>() {sestavineRaztopina};
            List<Sestavine_Zdravila> seznamSestavinTableta = new List<Sestavine_Zdravila>() {sestavineTableta};
            List<Sestavine_Zdravila> seznamSestavinInhalator = new List<Sestavine_Zdravila>() {sestavineInhalator};
            List<Sestavine_Zdravila> seznamSestavinAmpula = new List<Sestavine_Zdravila>() {sestavineAmpula};

            
            
            Artikel mazilo = new Mazilo(200, Mazilo.povrsinaAplikacije.telo, 25, artikelMazilo, seznamSestavinMazilo, false, new TimeSpan(24));
            
            Artikel raztopina = new Raztopina(0.05, Raztopina.Topilo.voda, 100, 5, artikelRaztopina, seznamSestavinRaztopina, false, TimeSpan.Zero);
            
            Artikel tableta = new Tableta(20, 2, artikelTableta, seznamSestavinTableta, false, new TimeSpan(8)); 
            
            Artikel inhalator = new Inhalator(2, 15, artikelInhalator, seznamSestavinInhalator, true, TimeSpan.Zero);
            
            Artikel ampula = new Ampula(5, artikelAmpula, seznamSestavinAmpula, true, new TimeSpan(24));

            List<Artikel> inventar = new List<Artikel>();
            // string potDoLastneDatoteke = @"C:\Users\mimin\Desktop\FERI\2. semester\VAJE\NP\Naloga 6\DATOTEKE\LastniInventar.csv";
            string potDoUstvarjeneDatoteke = @"C:\Users\mimin\Desktop\FERI\2. semester\VAJE\NP\Naloga 6\DATOTEKE\Inventorij.csv";

            inventar.Add(mazilo);
            inventar.Add(raztopina);
            inventar.Add(tableta);
            inventar.Add(inhalator);
            inventar.Add(ampula);

            // Inventar.VpisiVDatoteko(potDoLastneDatoteke, inventar);
            Inventar.PreberiIzDatoteke(potDoUstvarjeneDatoteke);

            Console.WriteLine();
            
            Func<string,string, string> isciArtikel = (pot, kajIsces) => 
            {
                foreach (var item in File.ReadAllLines(pot))
                {
                    if(item.Contains(kajIsces))
                    {
                        return item;
                    }
                }
                return "Ni takega artikla v inventaru!";
            };

            //Console.WriteLine(isciArtikel(potDoUstvarjeneDatoteke, "Lekadol"));

            Func<string,string, string> izbrisiArtikel = (pot, kajIsces) => 
            {
                string izbrisan = isciArtikel(pot,kajIsces);

                var zadrzi = File.ReadLines(pot).Where( x => x != izbrisan);
                File.AppendAllText(pot, zadrzi.ToString());
                
                return "Artikel " + izbrisan + " je izbrisan iz inventara.\n";
            };

            //Console.WriteLine(izbrisiArtikel(potDoUstvarjeneDatoteke, "Lekadol"));

           
            Action<string> najblizjiRok = (pot) =>
            {   
                DateTime yDatum = new DateTime();
                foreach (var item in File.ReadAllLines(pot))
                {
                    string[] vnos = item.Split(";");
                    
                    if(vnos[6] != "Rok trajanja" && vnos[6] != "ni roka")
                    {
                        string[] arr = {vnos[6]};
                        
                        foreach (var xDatum in arr)
                        {
                            yDatum = DateTime.Parse(xDatum);

                            DateTime[] doRoka = {yDatum};
                        }
                    }               
                }
                Console.WriteLine(isciArtikel(pot, yDatum.ToString("dd/MM/yyyy")));
                //Console.WriteLine(izbrisiArtikel(pot, yDatum.ToString("dd/MM/yyyy")));
            };

            // najblizjiRok(potDoUstvarjeneDatoteke);


            Action<double,double> vmesnaCena = (minCena, maxCena) =>
            {
                foreach(var item in inventar)
                {
                    if(minCena < item.cena && maxCena > item.cena )
                    {
                        Artikel.Izpis_o_artiklu(item);
                    }
                } 
            };

            vmesnaCena(5.00,10.00);

            Action<string> izpisArtiklovPoImenu = niz =>
            {
                foreach(var item in inventar)
                {
                    if(item.ime.Contains(niz))
                    {
                        Artikel.Izpis_o_artiklu(item);
                    }
                }
            };

            izpisArtiklovPoImenu("Forte");

            Action<string> izpisArtiklovPoDrzavi = x =>
            {
                foreach(var item in inventar)
                {
                    if(item.drzava_proizvajalca.Equals(x))
                    {
                        Artikel.Izpis_o_artiklu(item);
                    }
                }
            };
            izpisArtiklovPoDrzavi("Avstrija");

            Oseba Jan = new Oseba("Jan", "Novak", "moški", new DateTime(2002,03,18),null,0,false);
            Racun racun = new Racun("1", inventar, true, Jan);
            
            

            Lekarna lekarna = new Lekarna(new Dictionary<string, Racun>(), inventar, null);

            lekarna.OdstraniArtikel(artikelTableta);

            lekarna.OdstraniArtikel(artikelTableta);
        }
    }
}
