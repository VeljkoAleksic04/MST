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
        public static Cvor PronadjiKorenGrupe(Cvor c)
        {
            if (c.Roditelj == c)
                return c;

            return c.Roditelj = PronadjiKorenGrupe(c.Roditelj);
        }
        public static void SpojiGrupe(Cvor c1, Cvor c2)
        {
            Cvor koren1 = PronadjiKorenGrupe(c1);
            Cvor koren2 = PronadjiKorenGrupe(c2);

            if (koren1 != koren2)
            {
                if (koren1.Rang < koren2.Rang)
                    koren1.Roditelj = koren2;
                else if (koren1.Rang > koren2.Rang)
                    koren2.Roditelj = koren1;
                else
                {
                    koren2.Roditelj = koren1;
                    koren1.Rang++;
                }
            }
        }
    }
}
