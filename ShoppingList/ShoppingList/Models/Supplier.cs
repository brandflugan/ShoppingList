﻿using System;
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
        public decimal TotalCost { get; set; }
        public Product ActiveProduct { get; set; }

        public int CalculateScore()
        {
            int score = 0;

            foreach (var product in Products)
            {
                if (product.MatchType == MatchType.Match)
                {
                    score += 2;
                } else if (product.MatchType == MatchType.Replaced)
                {
                    score += 1;
                }
            }

            return score;
        }

        public double CalculateMatches()
        {
            return Math.Round((GetMatched()/GetTotal())*100);
        }

        public double GetMatched()
        {
            double matched = 0;

            foreach (var product in Products)
            {

                if (product.MatchType == MatchType.Match)
                {
                    matched += product.Antal;
                }
            }
            return matched;
        }

        public double GetTotal()
        {
            double total = 0;

            foreach (var product in Products)
            {
                total += product.Antal;
            }
            return total;
        }
    }
}