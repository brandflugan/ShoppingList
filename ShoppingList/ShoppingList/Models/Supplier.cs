using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class Supplier
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public int Score { get; set; }
        public Product ActiveProduct { get; set; }
    }
}