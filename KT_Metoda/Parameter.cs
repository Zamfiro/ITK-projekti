using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolić_KT_Metoda
{
    public class Parameter
    {
        public string Ime { get; set; }
        public int Utez { get; set; }

        public Parameter(string ime, int utez)
        {
            Ime = ime;
            Utez = utez;
        }

        public Parameter() { }

    }
}
