using System;
using System.Collections.Generic;

namespace Lekarna
{
    class Racun
    {
        public string id {get; set;}
        public List<Artikel> SeznamIzdelkov { get; set; } 
        public bool Izdan { get; set; }
        public Oseba Oseba { get; set; }

        public Racun(string id, List<Artikel> SeznamIzdelkov, bool Izdan, Oseba Oseba)
        {
            this.id = id;
            this.SeznamIzdelkov = SeznamIzdelkov;
            this.Izdan = Izdan;
            this.Oseba = Oseba;
        }

        public Racun(List<Artikel> SeznamIzdelkov)
        {
            this.SeznamIzdelkov = SeznamIzdelkov;
        }
        public Racun(Oseba Oseba, bool Izdan)
        {
            this.Oseba = Oseba;
            this.Izdan = Izdan;
        }
        public Racun() {}

        public static void izpisiRacun(Racun racun)
        {
            Console.Write(racun.id + " ");
            foreach (var item in racun.SeznamIzdelkov)
            {
               Console.Write(item.ime + ", ");
            }
            if(racun.Izdan)
            {
                Console.WriteLine("Je izdan.");
            }
            else Console.WriteLine("Ni izdan.");
        }        
    }
}