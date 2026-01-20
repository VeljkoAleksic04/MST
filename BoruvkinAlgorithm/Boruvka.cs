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

        public List<Grana> PronadjiMST(List<Cvor> sviCvorovi)
        {
            List<Grana> mstGrane = new List<Grana>();
            int brojOstrva = sviCvorovi.Count;

            while (brojOstrva > 1)
            {
                var najjeftinijaZaOstrvo = new Dictionary<Cvor, Grana>();

                foreach (var cvor in sviCvorovi)
                {
                    Cvor korenU = PronadjiRoditelja(cvor);

                    foreach (var susedstvo in cvor.Susedi)
                    {
                        Cvor sused = susedstvo.Key;
                        Cvor korenV = PronadjiRoditelja(sused);

                        if (korenU != korenV)
                        {
                            uint tezina = susedstvo.Value;

                            if (!najjeftinijaZaOstrvo.ContainsKey(korenU) || tezina < najjeftinijaZaOstrvo[korenU].Tezina)
                            {
                                najjeftinijaZaOstrvo[korenU] = new Grana(cvor, sused, tezina);
                            }
                        }
                    }
                }

                // 2. Spoji ostrva pomoću pronađenih najjeftinijih grana
                bool biloSpajanja = false;
                foreach (var grana in najjeftinijaZaOstrvo.Values)
                {
                    Cvor korenA = PronadjiRoditelja(grana.Iz);
                    Cvor korenB = PronadjiRoditelja(grana.Ka);

                    if (korenA != korenB)
                    {
                        mstGrane.Add(grana);
                        UnijaOstrva(korenA, korenB);
                        brojOstrva--;
                        biloSpajanja = true;
                    }
                }

                // Ako u celom krugu nismo našli nijednu granu za spajanje, graf nije povezan
                if (!biloSpajanja) break;
            }

            return mstGrane;
        }
    }
}
