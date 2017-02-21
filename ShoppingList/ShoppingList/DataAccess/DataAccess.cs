using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShoppingList.DataAccess
{
    public class DataAccess
    {
       // string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=" + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + "QuizDatabase.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework";
        string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog = MatkrisDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void Seed()
        {

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