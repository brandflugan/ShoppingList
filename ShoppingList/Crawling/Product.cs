using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crawling
{
    public class Product
    {
        public int Artikelnummer { get; set; }
        public string Produktnamn { get; set; }
        public decimal Pris { get; set; }
        public decimal Jmf { get; set; }
        public string Kategori { get; set; }
        public string Typ { get; set; }
        public string BildURL { get; set; }
        public MatchType MatchType { get; set; }
        public int MatchScore { get; set; }
        public int Antal { get; set; } = 0;
        public string Replaced { get; set; }
        public double Mangd { get; set; }
        public string MangdUnit { get; set; }
    }

    public enum MatchType
    {
        Match,
        Replaced,
        Unavailable
    }
}