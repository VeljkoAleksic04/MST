using System.Diagnostics;

namespace BoruvkinAlgorithm
{
    internal class Program
    {
        Graf GenerisiPovezanNasumicniGraf(int n, int k)
        {
            Graf graf = new(n, k*n);
            Random rnd = new();
            try
            {
                //if (graf.ListaCvorova == null)
                //    graf.ListaCvorova = [];
                //if (graf.ListaGrana == null)
                //    graf.ListaGrana = [];

                // Dodavanje cvorova u listu cvorova u grafu
                for(int i = 0; i < n; i++)
                {
                    int vr = rnd.Next(-50, 50);
                    int nadjen = graf.ListaCvorova.FindIndex(c => c.Vrednost == vr);
                    if (nadjen == -1)
                        graf.ListaCvorova.Add(new Cvor(vr));
                    else i--;
                }

                // povezivanje random cvora sa svim ostalim cvorovima u listi
                int rndIndex = rnd.Next(0, graf.ListaCvorova.Count);
                Cvor rndCvor = graf.ListaCvorova[rndIndex];
                uint tezina = 0;
                foreach (Cvor c in graf.ListaCvorova)
                {
                    if (c.Vrednost == rndCvor.Vrednost)
                        continue;

                    tezina = (uint)rnd.Next(0, 20);
                    rndCvor.DodajSuseda(c, tezina);
                    // Ovde se dodaje 99 grana (ako je u listi 100 cvorova), pa moramo da ih dodamo u listu grana
                    graf.ListaGrana.Add(new Grana(rndCvor, c, tezina));
                }

                // Odredjujemo broj slobodnih mesta za grane
                int cnt = graf.ListaGrana.Capacity - graf.ListaGrana.Count;
                for(int i = 0; i < cnt; i++)
                {
                    if (graf.ListaCvorova[graf.ListaCvorova.Capacity - 1].Susedi.Count == 99)
                        break;

                    tezina = (uint)rnd.Next(0, 20);

                    int c1 = rnd.Next(0, graf.ListaCvorova.Capacity);
                    int c2 = rnd.Next(0, graf.ListaCvorova.Capacity);

                    // Uzimamo dva random cvora
                    Cvor rndC1 = graf.ListaCvorova[c1];
                    Cvor rndC2 = graf.ListaCvorova[c2];

                    // Ispitujemo da li su mozda vec povezani
                    KeyValuePair<Cvor, uint> povezani = rndC1.Susedi.Find(sused => sused.Key == rndC2);
                    if(povezani.Key == null && c1 != c2)
                    {
                        graf.ListaCvorova[graf.ListaCvorova.FindIndex(c => c.Vrednost == rndC1.Vrednost)].DodajSuseda(rndC2, tezina);
                        graf.ListaGrana.Add(new Grana(rndC1, rndC2, tezina));
                    } 
                    else
                    {
                        i--;
                    }
                }
                if(graf.ListaGrana.Capacity == graf.ListaGrana.Count)
                    return graf;
                else
                {
                    Console.WriteLine("Svi cvorovi u random grafu su povezani i nije moguce dalje povezivanje...\n");
                    return new Graf(0, 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        Graf GenerisiDasyNasumicniGraf(int n, int k)
        {
            Graf graf = new Graf(n, k*n);
            Random rnd = new();
            try
            {
                // Dodavanje cvorova u listu cvorova u grafu
                for (int i = 0; i < n; i++)
                {
                    int vr = rnd.Next(-50, 50);
                    int nadjen = graf.ListaCvorova.FindIndex(c => c.Vrednost == vr);
                    if (nadjen == -1)
                        graf.ListaCvorova.Add(new Cvor(vr));
                    else i--;
                }

                uint tezina = 0;
                for(int i = 0; i < graf.ListaCvorova.Count - 1; i++)
                {
                    tezina = (uint)rnd.Next(0, 20);
                    graf.ListaCvorova[i].DodajSuseda(graf.ListaCvorova[i + 1], tezina);
                    graf.ListaGrana.Add(new Grana(graf.ListaCvorova[i], graf.ListaCvorova[i + 1], tezina));
                }
                int cnt = graf.ListaGrana.Capacity - graf.ListaGrana.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (graf.ListaCvorova[graf.ListaCvorova.Capacity - 1].Susedi.Count == 99)
                        break;

                    tezina = (uint)rnd.Next(0, 20);

                    int c1 = rnd.Next(0, graf.ListaCvorova.Capacity);
                    int c2 = rnd.Next(0, graf.ListaCvorova.Capacity);

                    // Uzimamo dva random cvora
                    Cvor rndC1 = graf.ListaCvorova[c1];
                    Cvor rndC2 = graf.ListaCvorova[c2];

                    // Ispitujemo da li su mozda vec povezani
                    KeyValuePair<Cvor, uint> povezani = rndC1.Susedi.Find(sused => sused.Key == rndC2);
                    if (povezani.Key == null && c1 != c2)
                    {
                        graf.ListaCvorova[graf.ListaCvorova.FindIndex(c => c.Vrednost == rndC1.Vrednost)].DodajSuseda(rndC2, tezina);
                        graf.ListaGrana.Add(new Grana(rndC1, rndC2, tezina));
                    }
                    else
                    {
                        i--;
                    }
                }
                if (graf.ListaGrana.Capacity == graf.ListaGrana.Count)
                    return graf;
                else
                {
                    Console.WriteLine("Svi cvorovi u Dasy grafu su povezani i nije moguce dalje povezivanje...\n");
                    return new Graf(0, 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PokreniBenchmark()
        {
            int[] N = { 100, 1000, 10000, 100000 };
            int[] K = { 2, 5, 10, 20, 33, 50 };

            for (int i = 0; i < N.Length; i++)
            {
                int n = N[i];
                for(int j = 0; j < K.Length; j++)
                {
                    int k = K[j];
                    int brPotega = (int)(n * k);

                    // 1. Generiši graf
                    Graf graf = GenerisiPovezanNasumicniGraf(n, k);
                    Graf dasyGraf = GenerisiDasyNasumicniGraf(n, k);

                    // 2. Pokreni i izmeri
                    Boruvka algoritam = new Boruvka();
                    Stopwatch sw = Stopwatch.StartNew();
                    List<Grana> mstRandom = algoritam.PronadjiMST(graf);
                    List<Grana> mstDasy = algoritam.PronadjiMST(dasyGraf);
                    sw.Stop();

                    if(dasyGraf.ListaGrana.Count == graf.ListaGrana.Capacity || graf.ListaGrana.Count == graf.ListaGrana.Capacity)
                        Console.WriteLine($"N={n}, K={k} | Vreme: {sw.ElapsedMilliseconds} ms");
                }
            }
        }
        static void Main()
        {
            Program program = new Program();
            program.PokreniBenchmark();
        }
    }
}
