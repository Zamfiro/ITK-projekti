using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frekvenčna_analiza
{
    internal class Besedilo
    {
        public string sifriranoBesedilo { get; set; }
        public string referencnoBesedilo { get; set; }

        public Besedilo(string sifriranoBesedilo, string desifriranoBesedilo)
        {
            this.sifriranoBesedilo = sifriranoBesedilo;
            this.referencnoBesedilo = desifriranoBesedilo;
        }
        public Besedilo()
        {

        }
    }
}
