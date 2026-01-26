using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace BoruvkinAlgorithm
{
    internal class Program
    {
        Graf GenerisiPovezanNasumicniGraf(int n, int k)
        {
            int brojGrana = k * n;

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

                //int centarInd = rnd.Next(0, n);
                //Cvor centar = graf.ListaCvorova[centarInd];

                int cnt = 0;

                while (cnt < brojGrana)
                {
                    Cvor c1 = graf.ListaCvorova[rnd.Next(0, n)];
                    Cvor c2 = graf.ListaCvorova[rnd.Next(0, n)];

                    if (c1.Vrednost == c2.Vrednost)
                        continue;

                    uint tezina = (uint)rnd.Next(1, 21);
                    c1.DodajSuseda(c2, tezina);
                    c2.DodajSuseda(c1, tezina);
                    graf.ListaGrana.Add(new Grana(c1, c2, tezina));

                    cnt++;
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
            int brojGrana = k * n;
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

                int cnt = 0;
                while(cnt < brojGrana)
                {
                    uint tezina = (uint)rnd.Next(1, 21);
                    Cvor c1 = graf.ListaCvorova[rnd.Next(0, n)];
                    Cvor c2 = graf.ListaCvorova[rnd.Next(0, n)];
                    if (c1.Vrednost == c2.Vrednost)
                        continue;

                    c1.DodajSuseda(c2, tezina);
                    c2.DodajSuseda(c1, tezina);
                    graf.ListaGrana.Add(new Grana(c1, c2, tezina));
                    postojeceGrane.Add((graf.ListaCvorova.FindIndex(c => c.Vrednost == c1.Vrednost), graf.ListaCvorova.FindIndex(c => c.Vrednost == c2.Vrednost)));
                    cnt++;
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