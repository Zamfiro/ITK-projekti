using System;
using System.Collections.Generic;


namespace Lekarna
{
    public class Oseba
    {
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string Spol { get; set; }
        public DateTime DatumRojstva { get; set; }
        public string Email { get; set; }
        public int StevilkaZdravstvenegaZavarovanja { get; set; }
        public bool OsebaImaDodatnoZavarovanje { get; set; }

        public Oseba(string Ime, string Priimek, string Spol, DateTime DatumRojstva, string Email, int StevilkaZdravstvenegaZavarovanja, bool OsebaImaDodatnoZavarovanje)
        {
            this.Ime = Ime;
            this.Priimek = Priimek;
            this.Spol = Spol;
            this.DatumRojstva = DatumRojstva;
            this.Email = Email;
            this.StevilkaZdravstvenegaZavarovanja = StevilkaZdravstvenegaZavarovanja;
            this.OsebaImaDodatnoZavarovanje = OsebaImaDodatnoZavarovanje;
        }

        public Oseba(string Ime)
        {
            this.Ime = Ime;
        }

        public void istaOseba(int stevilkaZZ)
        {
            
            if(StevilkaZdravstvenegaZavarovanja == stevilkaZZ)
            {
                Console.WriteLine("Oseba ze obstaja!");
            }
        }
    }
}