using System;
using System.Collections.Generic;

namespace Lekarna
{
     class Sestavine_Zdravila
    {
        public string ime_sestavine { get; set; }
        public string aktivna_snov { get; set; }
        public Enota _enota { get; set; }
        public int kolicina_v_zdravilu { get; set; }

        public Sestavine_Zdravila(string ime_sestavine, string aktivna_snov, Enota _enota, int kolicina_v_zdravilu)
        {
            this.ime_sestavine = ime_sestavine;
            this.aktivna_snov = aktivna_snov;
            this._enota = _enota;
            this.kolicina_v_zdravilu = kolicina_v_zdravilu;
        }

        public Sestavine_Zdravila(string ime_sestavine, string aktivna_snov)
        {
            this.ime_sestavine = ime_sestavine;
            this.aktivna_snov = aktivna_snov;
        }

        public Sestavine_Zdravila()
        {

        }

        public static void Izpis_sestavin(Sestavine_Zdravila sestavine)
        {

            Console.WriteLine("Ime sestavine: " + sestavine.ime_sestavine);
            Console.WriteLine("Aktivna snov: " + sestavine.aktivna_snov);
            
            Console.WriteLine("Kolicina v zdravilu: " + sestavine.kolicina_v_zdravilu);

            Console.WriteLine("Enota: " + sestavine._enota);

            Console.WriteLine();
            
        }
    }
}