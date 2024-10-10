using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Lekarna
{
    class ZdraviloNiNaRacunuException : Exception
    {
        public ZdraviloNiNaRacunuException(string message) : base(message)
        {
            throw new ZdraviloNiNaRacunuException("Ni mogoce odstraniti zdravila, ki ni na računu!");
        }
    }

    public class RacunZeIzdanException : Exception
    {
        public RacunZeIzdanException(string message) : base(message)
        {
            Console.WriteLine("Racun je ze izdan");
        }
    }

    interface ILekarna
    {
        void rezervirajArtikel(Artikel artikel, Oseba oseba);
        void ustvariRacun(Oseba oseba);
        void dodajNaRacun(List<Artikel> seznamArtiklov, string idRacuna);
        void sprostiRezervacijoArtikla(Artikel artikel, Oseba oseba);
        void izdajRacun(Oseba oseba, string idRacuna);
        void sprostiRezervacijoArtikla(RezerviranArtikel rezerviranArtikel);
        int kolicinaArtiklovNaZalogi(Artikel artikel);
        bool artikelNaZalogi(Artikel artikel);
        double IzracunajCeno(string idRacuna);
        
    }

    class Lekarna : ILekarna
    {
        protected Dictionary<string, Racun> IzdaniRacuni { get; set; }
        protected List<Artikel> ZalogaArtiklov { get; set; }
        protected List<RezerviranArtikel> RezerviraniArtikli { get; set; }
    
        public Lekarna(Dictionary<string, Racun> IzdaniRacuni, List<Artikel> ZalogaArtiklov, List<RezerviranArtikel> RezerviraniArtikli)
        {
            this.IzdaniRacuni = IzdaniRacuni;
            this.ZalogaArtiklov = ZalogaArtiklov;
            this.RezerviraniArtikli = RezerviraniArtikli;
        }

        //NALOGA 5

        public void DodajArtikel(Artikel artikel)
        {
            ZalogaArtiklov.Add(artikel);

            Console.WriteLine("Artikel " + artikel.ime + " je dodan v zalogo.\n ");
        }

        public void OdstraniArtikel(Artikel artikel)
        {
            try
            {
                ZalogaArtiklov.Remove(artikel);
                Console.WriteLine("Artikel " + artikel.ime + " je odstranjen iz zaloge.\n");
            }
            catch(ZdraviloNiNaRacunuException ex)
            {
                Console.WriteLine(ex.Message);
                string zapis = ex.Message + DateTime.Now.ToString();
                File.AppendAllText(@"C:\Users\mimin\Desktop\FERI\2. semester\VAJE\NP\Naloga 7\Izjeme.txt", zapis);
            }
        }

        public Artikel VrniZadnjiDodanArtikel()
        {
            return ZalogaArtiklov.Last();
        }

        public void rezervirajArtikel(Artikel artikel, Oseba oseba)
        {
            RezerviraniArtikli.Add(new RezerviranArtikel(artikel, oseba));

            Console.WriteLine("Artikel " + artikel.ime + " je rezerviran na osebo " + oseba.Ime + " " + oseba.Priimek);
        }

        public void ustvariRacun(Oseba oseba)
        {
           Racun racun = new Racun(oseba, false);
           IzdaniRacuni.Add(racun.id, racun);
        
            Console.WriteLine("Prazen racun ustvaren!");
        }

        public void dodajNaRacun(List<Artikel> seznamArtiklov, string idRacuna)
        {
            foreach (var item in IzdaniRacuni)
            {
                if (IzdaniRacuni.ContainsKey(idRacuna))
                {
                    item.Value.SeznamIzdelkov = seznamArtiklov;
                }
            }
            
        }

        public void izdajRacun(Oseba oseba, string idRacuna)
        {
            try
            {
                if(IzdaniRacuni[idRacuna].Izdan == true)
                {
                    throw new RacunZeIzdanException(idRacuna + "je ze izdan.");
                }
                Racun racun = new Racun() { id = idRacuna, Oseba = oseba, Izdan = true };

                IzdaniRacuni.Add(racun.id, racun);
                Console.WriteLine("Racun st. " + racun.id + " je izdan!");
            }
            catch(RacunZeIzdanException ex)
            {
                Console.WriteLine(ex.Message);
                string zapis = ex.Message + DateTime.Now.ToString();
                File.AppendAllText(@"C:\Users\mimin\Desktop\FERI\2. semester\VAJE\NP\Naloga 7\Izjeme.txt", zapis);
            }   
        }

        public void sprostiRezervacijoArtikla(Artikel artikel, Oseba oseba)
        {
            foreach(var item in RezerviraniArtikli)
            {
                if(item.artikel == artikel && item.oseba == oseba)
                {
                    RezerviraniArtikli.Remove(item);
                    ZalogaArtiklov.Add(item.artikel);

                    Console.WriteLine(artikel.ime + " ni vec rezerviran");
                    break;
                }

            }
        }

        public void sprostiRezervacijoArtikla(RezerviranArtikel rezerviranArtikel)
        {
            foreach(var item in RezerviraniArtikli)
            {
                if(rezerviranArtikel == item)
                {
                    ZalogaArtiklov.Add(item.artikel);
                    RezerviraniArtikli.Remove(item);

                    Console.WriteLine(rezerviranArtikel.artikel.ime + " ni vec rezerviran");
                    break;
                }
            }
        }

        public int kolicinaArtiklovNaZalogi(Artikel artikel)
        {
            int stevecArtiklov = 0;

            foreach(var item in ZalogaArtiklov)
            {
                if(item.ime == artikel.ime)
                {
                    stevecArtiklov++;
                    return stevecArtiklov;
                }
                
            }
            return 0;
        }

        public bool artikelNaZalogi(Artikel artikel)
        {
            foreach (var item in ZalogaArtiklov)
            {
                if (item.ime == artikel.ime)
                {
                    return true;
                }
            }
            return false;
        }

        public double IzracunajCeno(string idRacuna)
        {
            double koncnaCena = 0;
            
            foreach(var item in IzdaniRacuni)
            {   
                if(item.Value.Oseba.OsebaImaDodatnoZavarovanje == true) 
                {
                    return 0;
                }
                else
                {
                    foreach(var artikel in ZalogaArtiklov)
                    {
                        koncnaCena += artikel.cena;   
                    }
                    return koncnaCena;
                }
            }
            return koncnaCena;
        }

        public Racun NajdiRacun(string idRacuna)
        {
            foreach (var racun in IzdaniRacuni)
            {
                if(racun.Key == idRacuna)
                {
                    return racun.Value;
                }
            }

            return null;
        }

    }
}