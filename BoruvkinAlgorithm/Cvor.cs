using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoruvkinAlgorithm
{
    internal class Cvor
    {
        public int Vrednost { get; set; }
        public List<KeyValuePair<Cvor, uint>> Susedi { get; private set; }

        public int Rang { get; set; }
        // Roditelj grupe
        public Cvor Roditelj { get; set; }

        public Cvor(int vrednost)
        {
            Vrednost = vrednost;
            Susedi = new List<KeyValuePair<Cvor, uint>>();
            Roditelj = this;
            Rang = 0;
        }
        public void DodajSuseda(Cvor sused, uint tezina)
        {
            if (this.Vrednost == sused.Vrednost)
                throw new ArgumentException("Cvor ne moze sam sebi biti sused...");

            Susedi.Add(new KeyValuePair<Cvor, uint>(sused, tezina));
            sused.Susedi.Add(new KeyValuePair<Cvor, uint>(this, tezina));
        }
    }
}
