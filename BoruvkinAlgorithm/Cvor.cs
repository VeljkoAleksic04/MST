using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoruvkinAlgorithm
{
    internal class Cvor
    {
        public int Vrednost { get; set; }
        public List<KeyValuePair<Cvor, int>> Susedi { get; private set; }
        public Cvor()
        {
            Susedi = new List<KeyValuePair<Cvor, int>>();
        }
        public void DodajSuseda(Cvor sused, int tezina)
        {
            if (this.Vrednost == sused.Vrednost)
                throw new ArgumentException("Cvor ne moze sam sebi biti sused...");

            if (Susedi.Contains(new KeyValuePair<Cvor, int>(sused, tezina)))
                throw new ArgumentException("Ovaj cvor vec ima ovog suseda...");

            Susedi.Add(new KeyValuePair<Cvor, int>(sused, tezina));
            sused.DodajSuseda(this, tezina);
        }
    }
}
