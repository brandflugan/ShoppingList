using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class Score
    {
        public static List<Supplier> CalculateScore(List<Supplier> suppliers, List<Product> checkoutList)
        {
            foreach (var prod in checkoutList)
            {
                suppliers.ForEach(s => s.ActiveProduct = s.Products.Where(p => p.Artikelnummer == prod.Artikelnummer).FirstOrDefault());

                suppliers.Sort((s1, s2) => s1.ActiveProduct.Pris.CompareTo(s2.ActiveProduct.Pris));

                int score = 1;

                for (int i = 1; i < suppliers.Count + 1; i++)
                {
                    if (i > 1)
                    {
                        if (suppliers[i - 2].ActiveProduct.Pris * 0.02m < suppliers[i - 1].ActiveProduct.Pris)
                        {
                            score++;
                        }
                    }

                    suppliers[i - 1].Score += score;
                }
            }

            suppliers.Sort((s1, s2) => s1.Score.CompareTo(s2.Score));

            return suppliers;
        }
    }
}