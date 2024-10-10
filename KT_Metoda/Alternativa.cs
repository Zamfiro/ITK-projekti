using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Nikolić_KT_Metoda
{
    internal class Alternativa
    {
        public string Ime { get; set; }

        public List<int> Ocene { get; set; }

        public Alternativa(string ime, List<int> ocene)
        {
            Ime = ime;

            Ocene = ocene;
        }

        public Alternativa() { }
    }
}
