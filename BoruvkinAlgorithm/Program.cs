using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace BoruvkinAlgorithm
{
    internal class Program
    {
        Graf GenerisiPovezanNasumicniGraf(int n, int k)
        {
            int brojGrana = (n - 1) + (k * n);

            int maxGrana = n * (n - 1) / 2;
            if (brojGrana > maxGrana)
            {
                Console.WriteLine($"GREŠKA: Potrebno {brojGrana} grana, max={maxGrana} za n={n}");
                return new Graf(0, 0);
            }

            Graf graf = new(n, brojGrana);
            Random rnd = new();

            HashSet<(int, int)> postojeceGrane = new HashSet<(int, int)>();

            try
            {
                for (int i = 0; i < n; i++)
                {
                    graf.ListaCvorova.Add(new Cvor(i));
                }

                int centarInd = rnd.Next(0, n);
                Cvor centar = graf.ListaCvorova[centarInd];

                for (int i = 0; i < n; i++)
                {
                    if (i == centarInd)
                        continue;

                    uint tezina = (uint)rnd.Next(1, 21);
                    centar.DodajSuseda(graf.ListaCvorova[i], tezina);
                    graf.ListaGrana.Add(new Grana(centar, graf.ListaCvorova[i], tezina));

                    int min = Math.Min(centarInd, i);
                    int max = Math.Max(centarInd, i);
                    postojeceGrane.Add((min, max));
                }

                int potrebnoGrana = k * n;
                int dodato = 0;

                if (potrebnoGrana > maxGrana / 2)
                {
                    List<(int, int)> sveGrane = new List<(int, int)>(maxGrana);

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            if (!postojeceGrane.Contains((i, j)))
                            {
                                sveGrane.Add((i, j));
                            }
                        }
                    }

                    int granica = Math.Min(potrebnoGrana, sveGrane.Count);
                    for (int i = 0; i < granica; i++)
                    {
                        int j = rnd.Next(i, sveGrane.Count);
                        var temp = sveGrane[i];
                        sveGrane[i] = sveGrane[j];
                        sveGrane[j] = temp;

                        var (c1Idx, c2Idx) = sveGrane[i];
                        uint tezina = (uint)rnd.Next(1, 21);

                        graf.ListaCvorova[c1Idx].DodajSuseda(graf.ListaCvorova[c2Idx], tezina);
                        graf.ListaGrana.Add(new Grana(graf.ListaCvorova[c1Idx], graf.ListaCvorova[c2Idx], tezina));
                        dodato++;
                    }
                }
                else
                {
                    while (dodato < potrebnoGrana)
                    {
                        int c1 = rnd.Next(0, n);
                        int c2 = rnd.Next(0, n);

                        if (c1 == c2)
                            continue;

                        int min = Math.Min(c1, c2);
                        int max = Math.Max(c1, c2);

                        if (postojeceGrane.Add((min, max)))
                        {
                            uint tezina = (uint)rnd.Next(1, 21);
                            graf.ListaCvorova[c1].DodajSuseda(graf.ListaCvorova[c2], tezina);
                            graf.ListaGrana.Add(new Grana(graf.ListaCvorova[c1], graf.ListaCvorova[c2], tezina));
                            dodato++;

                        }
                    }
                }

                return graf;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        Graf GenerisiDasyNasumicniGraf(int n, int k)
        {
            int brojGrana = (n - 1) + (k * n);
            Graf graf = new Graf(n, brojGrana);

            int maxGrana = n * (n - 1) / 2;
            if (brojGrana > maxGrana)
            {
                Console.WriteLine($"GREŠKA: Potrebno {brojGrana} grana, max={maxGrana} za n={n}");
                return new Graf(0, 0);
            }

            Random rnd = new();

            HashSet<(int, int)> postojeceGrane = new HashSet<(int, int)>();

            try
            {
                for (int i = 0; i < n; i++)
                {
                    graf.ListaCvorova.Add(new Cvor(i));
                }

                for (int i = 0; i < n - 1; i++)
                {
                    uint tezina = (uint)rnd.Next(1, 21);
                    graf.ListaCvorova[i].DodajSuseda(graf.ListaCvorova[i + 1], tezina);
                    graf.ListaGrana.Add(new Grana(graf.ListaCvorova[i], graf.ListaCvorova[i + 1], tezina));
                    postojeceGrane.Add((i, i + 1));
                }

                int potrebnoGrana = k * n;
                int dodato = 0;

                if (potrebnoGrana > maxGrana / 2)
                {
                    List<(int, int)> sveGrane = new List<(int, int)>(maxGrana);

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            if (!postojeceGrane.Contains((i, j)))
                            {
                                sveGrane.Add((i, j));
                            }
                        }
                    }

                    int granica = Math.Min(potrebnoGrana, sveGrane.Count);
                    for (int i = 0; i < granica; i++)
                    {
                        int j = rnd.Next(i, sveGrane.Count);
                        var temp = sveGrane[i];
                        sveGrane[i] = sveGrane[j];
                        sveGrane[j] = temp;

                        var (c1Ind, c2Ind) = sveGrane[i];
                        uint tezina = (uint)rnd.Next(1, 21);

                        graf.ListaCvorova[c1Ind].DodajSuseda(graf.ListaCvorova[c2Ind], tezina);
                        graf.ListaGrana.Add(new Grana(graf.ListaCvorova[c1Ind], graf.ListaCvorova[c2Ind], tezina));
                        dodato++;
                    }
                }
                else
                {
                    // Za manji broj grana, random sampling
                    while (dodato < potrebnoGrana)
                    {
                        int c1 = rnd.Next(0, n);
                        int c2 = rnd.Next(0, n);

                        if (c1 == c2)
                            continue;

                        int min = Math.Min(c1, c2);
                        int max = Math.Max(c1, c2);

                        if (postojeceGrane.Add((min, max)))
                        {
                            uint tezina = (uint)rnd.Next(1, 21);
                            graf.ListaCvorova[c1].DodajSuseda(graf.ListaCvorova[c2], tezina);
                            graf.ListaGrana.Add(new Grana(graf.ListaCvorova[c1], graf.ListaCvorova[c2], tezina));
                            dodato++;
                        }
                    }
                }

                return graf;
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

            Console.WriteLine("===== BORUVKA - BENCHMARK =====\n");

            for (int i = 0; i < N.Length; i++)
            {
                int n = N[i];
                for (int j = 0; j < K.Length; j++)
                {
                    int k = K[j];
                    int ukupnoGrana = (n - 1) + (k * n);
                    Stopwatch swGen = Stopwatch.StartNew();
                    Graf graf = GenerisiPovezanNasumicniGraf(n, k);
                    swGen.Stop();

                    swGen.Restart();
                    Graf dasyGraf = GenerisiDasyNasumicniGraf(n, k);
                    swGen.Stop();

                    Console.WriteLine($"\n========== N={n}, k={k} ==========");

                    
                    if (graf.ListaCvorova.Count == 0 || dasyGraf.ListaCvorova.Count == 0)
                    {
                        Console.WriteLine("!!! GREŠKA !!!\n");
                        continue;
                    }

                    Boruvka alg1 = new Boruvka();
                    Stopwatch sw1 = Stopwatch.StartNew();
                    List<Grana> mstRandom = alg1.PronadjiMST(graf);
                    sw1.Stop();

                    Boruvka alg2 = new Boruvka();
                    Stopwatch sw2 = Stopwatch.StartNew();
                    List<Grana> mstDasy = alg2.PronadjiMST(dasyGraf);
                    sw2.Stop();

                    Console.WriteLine($"\nBORUVKA:");
                    Console.WriteLine($"Random: {sw1.ElapsedMilliseconds,8} ms | MST grana: {mstRandom.Count}");
                    Console.WriteLine($"Daisy:  {sw2.ElapsedMilliseconds,8} ms | MST grana: {mstDasy.Count}");
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