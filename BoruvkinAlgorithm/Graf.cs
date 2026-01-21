using System;
using System.Collections.Generic;
using System.Text;

namespace BoruvkinAlgorithm
{
    internal class Graf
    {
        public List<Cvor> ListaCvorova { get; set; }
        public List<Grana> ListaGrana { get; set; }

        public Graf(int n, int k)
        {
            ListaCvorova = new List<Cvor>(n);
            ListaGrana = new List<Grana>(k);
        }
        public Graf(List<Cvor> listaCvorova, List<Grana> listaGrana)
        {
            ListaCvorova = listaCvorova;
            ListaGrana = listaGrana;
        }
    }
}
