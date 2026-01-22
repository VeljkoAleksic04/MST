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

                foreach (var grana in graf.ListaGrana)
                {
                    Cvor korenLevo = PronadjiRoditelja(grana.Iz);
                    Cvor korenDesno = PronadjiRoditelja(grana.Ka);

                    if (korenLevo != korenDesno)
                    {
                        if (!najjeftinijaZaOstrvo.ContainsKey(korenLevo) ||
                            grana.Tezina < najjeftinijaZaOstrvo[korenLevo].Tezina)
                        {
                            najjeftinijaZaOstrvo[korenLevo] = grana;
                        }

                        if (!najjeftinijaZaOstrvo.ContainsKey(korenDesno) ||
                            grana.Tezina < najjeftinijaZaOstrvo[korenDesno].Tezina)
                        {
                            najjeftinijaZaOstrvo[korenDesno] = grana;
                        }
                    }
                }

                bool biloSpajanja = false;
                HashSet<Grana> dodateGrane = new HashSet<Grana>();

                foreach (var grana in najjeftinijaZaOstrvo.Values)
                {
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
