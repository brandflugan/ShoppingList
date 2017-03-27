using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
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
        public bool Match { get; set; }
    }
}