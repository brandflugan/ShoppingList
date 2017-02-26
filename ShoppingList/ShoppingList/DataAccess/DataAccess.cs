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

        public bool ValidateUser(string email, string password)
        {
            var query = "SELECT COUNT(*) FROM Foretag WHERE Epost = @epost AND Losenord = @losen";
            int count = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("epost", email));
                command.Parameters.Add(new SqlParameter("losen", password));

                conn.Open();
                count = (int)command.ExecuteScalar();
                conn.Close();
            }

            if (count > 0)
                return true;
            else
                return false;
        }

        public string GetBusinessname(string email)
        {
            var query = "SELECT Foretagsnamn FROM Foretag WHERE Epost = @email";
            string businessname = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("email", email));

                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    businessname = reader.GetString(0);
                }
                conn.Close();
            }

            return businessname;
        }

        public void UpdateProductlist(List<string> productlist, string foretagsnamn)
        {
            foreach (var product in productlist)
            {
                var attributes = product.Split(';');

                var query = "SELECT COUNT FROM Products WHERE Foretagsepost = @epost AND Produktnamn = @produktnamn";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("epost", foretagsnamn));
                    command.Parameters.Add(new SqlParameter("produktnamn", attributes[1]));

                    int count = command.ExecuteNonQuery();
                }

                return;
            }
        }
    }
}