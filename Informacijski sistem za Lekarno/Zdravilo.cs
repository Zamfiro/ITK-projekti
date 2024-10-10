using System;
using System.Collections.Generic;

namespace Lekarna
{
     abstract class Zdravilo : Artikel
    {
        
        protected Artikel artikel {get; set;}
        protected List<Sestavine_Zdravila> sestavnineZdravila { get; set; }

        protected bool na_recept {get; set;}

        protected TimeSpan casMedOdmerki {get; set;}


        public Zdravilo(Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel.ID, artikel.ime,artikel.cena,artikel.drzava_proizvajalca,artikel.naslov_distributerja,artikel.rok_trajanja, artikel.skladisce)
        {
            this.artikel = artikel;
            this.sestavnineZdravila = sestavnineZdravila;
            this.na_recept = na_recept;
            this.casMedOdmerki = casMedOdmerki;
        }

        public Zdravilo(List<Sestavine_Zdravila> sestavnineZdravila)
        {
            this.sestavnineZdravila = sestavnineZdravila;
        }

        public Zdravilo()
        {

        }

        public abstract double SteviloPreostalihDoziranj();
         
        public virtual void Izpis_o_zdravilu()
        {
            Console.Write("Seznam sestavin: ");
            foreach (var item in sestavnineZdravila)
            {
                Console.Write(item.ime_sestavine + " " + item.kolicina_v_zdravilu + item._enota);
            }
            Console.WriteLine("\n");

            if(na_recept)
            {
                Console.WriteLine("Zdravilo je na recept!");
            }
            else
            {
                Console.WriteLine("Zdravilo ni na recept!");
            }
            
            Console.WriteLine("Stevilo preostalih doziranj: " + SteviloPreostalihDoziranj());

            Console.WriteLine();
            
        }
        
    }
    //PODRAZREDI

    class Mazilo : Zdravilo
    {
        private int kolicinaMazila {get; set;}
        public enum povrsinaAplikacije {obraz, roka, telo, lokalno}
        private povrsinaAplikacije povrsina {get; set;}
        private int odmerek {get; set;}

        public Mazilo(int kolicinaMazila, povrsinaAplikacije povrsina, int odmerek, Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel, sestavnineZdravila, na_recept, casMedOdmerki)
        {
            this.kolicinaMazila = kolicinaMazila;
            this.povrsina = povrsina;
            this.odmerek = odmerek;
        }

        public override double SteviloPreostalihDoziranj()
        {
            double stDoziranj;

            if(povrsina == povrsinaAplikacije.obraz)
            {
                stDoziranj = kolicinaMazila / odmerek * 5;
                return Math.Round(stDoziranj, 2);
            }
            else if(povrsina == povrsinaAplikacije.roka)
            {
                stDoziranj = kolicinaMazila / odmerek * 3;
                return Math.Round(stDoziranj, 2);
            }
            else if(povrsina == povrsinaAplikacije.telo)
            {
                stDoziranj = kolicinaMazila / odmerek * 0.5;
                return Math.Round(stDoziranj, 2);
            }
            else if(povrsina == povrsinaAplikacije.lokalno)
            {
                stDoziranj = kolicinaMazila / odmerek * 10;
                return Math.Round(stDoziranj, 2);
            }
            else return 0;
        }
    }

    class Raztopina : Zdravilo
    {
        private double koncentracijaRaztopine {get; set;} //procentualna vrednost
        public enum Topilo {natrijev_klorid, glukoza, sterofundin, lonolyte, voda};
        private Topilo topilo;
        private int skupnaKolicinaZdravila {get; set;}
        private int odmerek {get; set;}

        public Raztopina(double koncentracijaRaztopine, Topilo topilo, int skupnaKolicinaZdravila, int odmerek, Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel, sestavnineZdravila, na_recept, casMedOdmerki)
        {
            this.koncentracijaRaztopine = koncentracijaRaztopine;
            this.topilo = topilo;
            this.skupnaKolicinaZdravila = skupnaKolicinaZdravila;
            this.odmerek = odmerek;
        }

        public override double SteviloPreostalihDoziranj() 
        {
            double stDoziranj = skupnaKolicinaZdravila / odmerek * koncentracijaRaztopine;
            
            return Math.Round(stDoziranj, 2);
        }
    }

    class Tableta : Zdravilo
    {
        private int steviloTabletVPakiranju {get; set;}
        private int steviloTabletNaAplikacijo {get; set;}

        public Tableta(int steviloTabletVPakiranju, int steviloTabletNaAplikacijo, Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel, sestavnineZdravila, na_recept, casMedOdmerki)
        {
            this.steviloTabletVPakiranju = steviloTabletVPakiranju;
            this.steviloTabletNaAplikacijo = steviloTabletNaAplikacijo; 
        }

        public override void Izpis_o_zdravilu()
        {
            Console.WriteLine("Stevilo tablet v pakiranju: " + steviloTabletVPakiranju);
            Console.WriteLine("Stevilo tablet na aplikacijo: " + steviloTabletNaAplikacijo);

            Console.WriteLine("Stevilo preostalih doziranj: " + SteviloPreostalihDoziranj() + "\n");
        }

        public override double SteviloPreostalihDoziranj() 
        { 
            double stDoziranj = steviloTabletVPakiranju / steviloTabletNaAplikacijo;
            
            return Math.Round(stDoziranj, 2);
        }
    }

    class Inhalator : Zdravilo
    {
        private int steviloVpihovZaAplikacijo {get; set;}
        private int kolicinaVpihovVPakiranju {get; set;}

        public Inhalator(int steviloVpihovZaAplikacijo, int kolicinaVpihovVPakiranju, Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel, sestavnineZdravila, na_recept, casMedOdmerki)
        {
            this.steviloVpihovZaAplikacijo = steviloVpihovZaAplikacijo;
            this.kolicinaVpihovVPakiranju = kolicinaVpihovVPakiranju;
        }

        public override double SteviloPreostalihDoziranj() 
        {
            double stDoziranj = kolicinaVpihovVPakiranju / steviloVpihovZaAplikacijo;

            return stDoziranj;
        }
    }

    class Ampula : Zdravilo
    {
        private int kolicinaAmpulVPakiranju { get; set; }

        public Ampula(int kolicinaAmpulVPakiranju, Artikel artikel, List<Sestavine_Zdravila> sestavnineZdravila, bool na_recept, TimeSpan casMedOdmerki) : base(artikel, sestavnineZdravila, na_recept, casMedOdmerki)
        {
            this.kolicinaAmpulVPakiranju = kolicinaAmpulVPakiranju;
        }

        public override double SteviloPreostalihDoziranj() { return kolicinaAmpulVPakiranju; }
    }

}