using System;
using System.Collections.Generic;

namespace Lekarna
{
    class RezerviranArtikel
    {
        public Artikel artikel {get; set;}
        public Oseba oseba {get; set;}

        public RezerviranArtikel(Artikel artikel, Oseba oseba)
        {
            this.artikel = artikel;
            this.oseba = oseba;
        }


    }
}