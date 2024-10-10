using System;
using System.IO;
using System.Collections.Generic;

namespace Lekarna
{
    class CSVDatotekaNapacneOblikeException : Exception
    {
        public CSVDatotekaNapacneOblikeException(string message) : base(message)
        {
            throw new CSVDatotekaNapacneOblikeException("Datoteka ni pravilnega formata!");
        }
    }

    class Inventar
    {
        public Artikel artikel {get; set;}
        public Zdravilo zdravilo {get; set;}

        public Inventar (Artikel artikel, Zdravilo zdravilo)
        {
            artikel = this.artikel;
            zdravilo = this.zdravilo;
        }

        public static void VpisiVDatoteko(string pot, List<Artikel> zaloga)
        {

            if (File.Exists(pot))
            {
                File.Delete(pot);
            }
            string prvaVrstica = "ID;Ime;Cena;Skladisce;Rok Trajanja\n";
            File.AppendAllText(pot, prvaVrstica);
            
            
            foreach(var izdelek in zaloga)
            {
                string[] artikel = {izdelek.ID.ToString(), izdelek.ime.ToString(), izdelek.cena.ToString(), izdelek.skladisce.ToString(), izdelek.rok_trajanja.ToString("dd.MM.yyyy")};
                string podatki = String.Join("; ", artikel) + Environment.NewLine;

                File.AppendAllText(pot, podatki);                

            }
            
        }
        public static void PreberiIzDatoteke(string pot)
        {
            try
            {
                if (pot.Contains(".png"))
                {
                    if (File.Exists(pot))
                    {
                        foreach (var item in File.ReadAllLines(pot))
                        {
                            Console.WriteLine(item);
                        }
                    }
                    else throw new CSVDatotekaNapacneOblikeException("Neustrezna datoteka!");
                }
            }
            catch(CSVDatotekaNapacneOblikeException ex)
            {
                Console.WriteLine(ex.Message);
                string zapis = ex.Message + DateTime.Now.ToString();
                File.AppendAllText(@"C:\Users\mimin\Desktop\FERI\2. semester\VAJE\NP\Naloga 7\Izjeme.txt", zapis);
            }
            
        }
    }
}