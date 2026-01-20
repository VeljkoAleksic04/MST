using System;
using System.Collections.Generic;
using System.Text;

namespace BoruvkinAlgorithm
{
    internal class Grana
    {
        public Cvor Iz { get; set; }
        public Cvor Ka { get; set; }
        public uint Tezina { get; set; }

        public Grana(Cvor iz, Cvor ka, uint tezina)
        {
            Iz = iz;
            Ka = ka;
            Tezina = tezina;
        }
    }
}
