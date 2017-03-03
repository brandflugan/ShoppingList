using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShoppingList.DataAccess
{
    public class DataAccess
    {
        string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog = MatkrisDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void Seed()
        {

        }

        public string ValidateUser(string email, string password)
        {
            var query = "SELECT Foretagsnamn FROM Foretag WHERE Epost = @epost AND Losenord = @losen";
            string businessname = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("epost", email));
                command.Parameters.Add(new SqlParameter("losen", password));

                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    businessname = reader.GetString(0) ?? null;
                }
                conn.Close();
            }

            return businessname;
        }

        public List<Product> GetTopMatchesByName(string term)
        {
            List<Product> topProducts = new List<Product>();

            var query = "select top 5 * from Produkter where Produktnamn LIKE '%" + @term + "%' ORDER BY CASE " +
                        "WHEN Produktnamn LIKE '" + @term + "%' THEN 1 " +
                        "ELSE 2 " +
                        "END ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("term", term));

                conn.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    topProducts.Add(new Product { Artikelnummer = reader.GetInt32(0), Produktnamn = reader.GetString(1), Pris = reader.GetDecimal(2), JFM = reader.GetDecimal(3), Kategori = reader.GetString(4), Typ = reader.GetString(5), BildURL = reader.GetString(6) });
                }

                conn.Close();
            }

            return topProducts;
        }

        public void UpdateProductlist(List<string> productlist, string email)
        {
            productlist.RemoveAt(0);

            foreach (var product in productlist)
            {
                var details = product.Split(';');

                var query = "SELECT COUNT(*) FROM Produkter INNER JOIN Foretag ON Foretagsepost = Epost WHERE Epost = @epost AND Artikelnummer = @artnummer";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("epost", email));
                    command.Parameters.Add(new SqlParameter("artnummer", int.Parse(details[0])));

                    conn.Open();

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        query = "UPDATE Produkter SET Artikelnummer = @artnummer, Produktnamn = @produktnamn, Pris = @pris, Jamforelsepris = @jmf, Kategori = @kategori, Typ = @typ, BildURL = @bildURL " +
                            "WHERE Artikelnummer = @artnummer AND Foretagsepost = @epost";
                    }
                    else
                    {

                        query = "INSERT INTO Produkter(Artikelnummer, Produktnamn, Pris, Jamforelsepris, Kategori, Typ, BildURL, Foretagsepost) " +
                            "VALUES(@artnummer, @produktnamn, @pris, @jmf, @kategori, @typ, @bildURL, @epost)";
                    }

                    command = new SqlCommand(query, conn);

                    decimal price;
                    decimal jmf;
                    try
                    {
                        string replacePrice = details[2].Replace(',', '.');
                        string replaceJmf = details[3].Replace(',', '.');
                        price = decimal.Parse(replacePrice);
                        jmf = decimal.Parse(replaceJmf);
                    }
                    catch (Exception)
                    {
                        price = decimal.Parse(details[2]);
                        jmf = decimal.Parse(details[3]);
                    }

                    command.Parameters.Add(new SqlParameter("artnummer", details[0]));
                    command.Parameters.Add(new SqlParameter("produktnamn", details[1]));

                    price = price - (price % 0.01m);
                    jmf = jmf - (jmf % 0.01m);

                    command.Parameters.Add(new SqlParameter("pris", price));
                    command.Parameters.Add(new SqlParameter("jmf", jmf));
                    command.Parameters.Add(new SqlParameter("kategori", details[4]));
                    command.Parameters.Add(new SqlParameter("typ", details[5]));
                    command.Parameters.Add(new SqlParameter("bildURL", details[6]));
                    command.Parameters.Add(new SqlParameter("epost", email));

                    command.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
    }
}