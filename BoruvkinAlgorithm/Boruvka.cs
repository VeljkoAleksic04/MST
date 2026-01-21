using System;
using System.Collections.Generic;
using System.Text;

namespace BoruvkinAlgorithm
{
    internal class Boruvka
    {
        private Cvor PronadjiRoditelja(Cvor cvor)
        {
            if (cvor.Roditelj == cvor)
                return cvor;
            return cvor.Roditelj = PronadjiRoditelja(cvor.Roditelj);
        }
        private void UnijaOstrva(Cvor koren1, Cvor koren2)
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

        public List<Grana> PronadjiMST(Graf graf)
        {
            List<Grana> mstGrane = new List<Grana>();

            if (graf.ListaGrana == null || graf.ListaCvorova == null)
                throw new NullReferenceException("Lista grana ili lista cvorova je null.\n");

            int brojOstrva = graf.ListaCvorova.Count;

            while (brojOstrva > 1)
            {
                var najjeftinijaZaOstrvo = new Dictionary<Cvor, Grana>();

                // EFIKASNO: Iteracija kroz graf.ListaGrana (svaka grana samo jednom)
                foreach (var grana in graf.ListaGrana)
                {
                    Cvor korenU = PronadjiRoditelja(grana.Iz);
                    Cvor korenV = PronadjiRoditelja(grana.Ka);

                    if (korenU != korenV)
                    {
                        // Proveri da li je ova grana najjeftinija za ostrvo U
                        if (!najjeftinijaZaOstrvo.ContainsKey(korenU) ||
                            grana.Tezina < najjeftinijaZaOstrvo[korenU].Tezina)
                        {
                            najjeftinijaZaOstrvo[korenU] = grana;
                        }

                        // Proveri da li je ova grana najjeftinija za ostrvo V
                        if (!najjeftinijaZaOstrvo.ContainsKey(korenV) ||
                            grana.Tezina < najjeftinijaZaOstrvo[korenV].Tezina)
                        {
                            najjeftinijaZaOstrvo[korenV] = grana;
                        }
                    }
                }

                // Spoji ostrva pomoću pronađenih najjeftinijih grana
                bool biloSpajanja = false;
                HashSet<Grana> dodateGrane = new HashSet<Grana>(); // Spreči duplikate

                foreach (var grana in najjeftinijaZaOstrvo.Values)
                {
                    // Proveri da li smo već dodali ovu granu
                    if (dodateGrane.Contains(grana))
                        continue;

                    Cvor korenA = PronadjiRoditelja(grana.Iz);
                    Cvor korenB = PronadjiRoditelja(grana.Ka);

                    if (korenA != korenB)
                    {
                        mstGrane.Add(grana);
                        dodateGrane.Add(grana);
                        UnijaOstrva(korenA, korenB);
                        brojOstrva--;
                        biloSpajanja = true;
                    }
                }

                if (!biloSpajanja)
                    break;
            }

            return mstGrane;
        }
    }
}
