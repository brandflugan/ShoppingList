using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingList.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.DataAccess.Tests
{
    [TestClass()]
    public class DataAccessTests
    {
        DataAccess da = new DataAccess();

        [TestMethod()]
        public void FindMatchTest()
        {
            var result = da.FindMatch(new Models.Product { Produktnamn = "Kycklingfärs Kronfågel, 500g"} );

            Assert.Fail();
        }
    }
}