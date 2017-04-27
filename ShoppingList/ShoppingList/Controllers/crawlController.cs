
using Crawling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingList.Controllers
{
    public class crawlController : Controller
    {
        private Crawler crawler = new Crawler();
        private DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

        // GET: crawl
        public ActionResult Index()
        {
            return View();
        }

        public void MSE()
        {
            var products = crawler.CrawlMSE();

            dataAccess.SaveProducts(products, "mat@gmail.se");
        }

        public void MHS()
        {
            var products = crawler.CrawlMHS();

            dataAccess.SaveProducts(products, "mathem@gmail.se");
        }
    }
}