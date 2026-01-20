using System.Diagnostics;

namespace BoruvkinAlgorithm
{
    internal class Program
    {
        List<Cvor> GenerisiNasumicanPovezanGraf(int n, int k)
        {
            List<Cvor> graf = new List<Cvor>();

            return graf;
        }

        public void PokreniBenchmark()
        {
            int[] N = { 100, 1000, 10000, 100000 };
            int[] K = { 2, 5, 10, 20, 33, 50 };

            foreach (int n in N)
            {
                foreach (int k in K)
                {
                    int brPotega = (int)(n * k);

                    // 1. Generiši graf
                    List<Cvor> graf = GenerisiNasumicanPovezanGraf(n, k);

                    // 2. Pokreni i izmeri
                    Boruvka algoritam = new Boruvka();
                    Stopwatch sw = Stopwatch.StartNew();

                    var mst = algoritam.PronadjiMST(graf);

                    sw.Stop();
                    Console.WriteLine($"N={n}, K={k} | Vreme: {sw.ElapsedMilliseconds} ms");
                }
            }
        }
        static void Main(string[] args)
        {
            
        }
    }
}
