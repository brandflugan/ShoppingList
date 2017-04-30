
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

        public ActionResult MSE()
        {
            var products = crawler.CrawlMSE("grönsaker");

            dataAccess.SaveProducts(products, "mat@gmail.se");

            return RedirectToAction("/index");
        }

        public ActionResult MHS()
        {
            var products = crawler.CrawlMHS("grönsaker");

            dataAccess.SaveProducts(products, "mathem@gmail.se");

            return RedirectToAction("/index");
        }
    }
}