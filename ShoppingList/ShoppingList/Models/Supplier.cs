using Crawling;
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
        public decimal TotalCost { get; set; }
        public Product ActiveProduct { get; set; }
        public int Score { get; set; }

        public void CalculateScore()
        {
            int score = 0;

            foreach (var product in Products)
            {
                if (product.MatchType == MatchType.Match)
                {
                    score += 2;
                }
                else if (product.MatchType == MatchType.Replaced)
                {
                    score += 1;
                }
            }

            Score = score;
        }

        public string GetCheckoutURL()
        {
            string checkoutURL = string.Empty;

            switch (Name.ToLower())
            {
                case "mathem.se":
                    checkoutURL = GetMathemCheckoutURL();
                    break;
            }

            return checkoutURL;
        }

        private string GetMathemCheckoutURL()
        {
            string checkoutURL = "https://www.mathem.se/externalService.asmx/AddProductsToShoppingCart?";

            foreach (var product in Products)
            {
                checkoutURL += "productIDs=" + product.Artikelnummer + "&productCounts=" + product.Antal;

                if (Products.IndexOf(product) != Products.IndexOf(Products.Last()))
                {
                    checkoutURL += "&";
                }
            }

            return checkoutURL;
        }

        public double CalculateMatches()
        {
            return Math.Round((GetMatched() / GetTotal()) * 100);
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