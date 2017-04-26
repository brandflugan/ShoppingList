using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.PhantomJS;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace Crawling
{
    class Crawl
    {
        public void CrawlMSE()
        {
            List<Product> products = new List<Product>();

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.mat.se/search.html?q=gr%C3%B6nsaker");
            driver.Manage().Window.Maximize();

            var jse = (IJavaScriptExecutor)driver; ;

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 6));

            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);

            Actions actions = new Actions(driver);

            actions.KeyDown(Keys.Control).SendKeys("a").Perform();

            Thread.Sleep(2000);

            IReadOnlyCollection<IWebElement> productDivs = driver.FindElements(By.CssSelector(".outer"));

            foreach (IWebElement div in productDivs)
            {
                var prod = new Product();

                string text = div.Text;
                text = text.Replace("\r\n", ";");

                var details = text.Split(';');

                details[0] = details[0].Replace(" :-", string.Empty);

                try
                {
                    prod.Pris = decimal.Parse(details[0].Replace('.', ','));
                }
                catch
                {
                    var pricedetails = details[4].Split(' ');

                    prod.Pris = decimal.Parse(pricedetails[2]);
                }

                prod.Produktnamn = details[1];
                prod.Produktnamn += " " + details[2];

                products.Add(prod);
            }

            return;
        }

        public void CrawlMHS()
        {
            List<Product> products = new List<Product>();

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.mathem.se/sok?q=gr%C3%B6nsaker");
            driver.Manage().Window.Maximize();

            var jse = (IJavaScriptExecutor)driver; ;

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 6));

            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);
            jse.ExecuteScript("window.scrollBy(0,600);");
            Thread.Sleep(2000);

            Actions actions = new Actions(driver);

            actions.KeyDown(Keys.Control).SendKeys("a").Perform();

            Thread.Sleep(2000);

            List<IWebElement> productDivs = driver.FindElements(By.XPath(("//div[@class='product prod-info       ']"))).ToList();

            foreach (IWebElement div in productDivs)
            {
                Product prod = new Product();

                var id = div.GetAttribute("data-product-div-id");
                prod.Artikelnummer = int.Parse(id);

                var price = div.GetAttribute("data-price").Replace('.', ',');
                prod.Pris = decimal.Parse(price);

                var productLink = div.FindElement(By.CssSelector("a.fancybox-product"));

                var name = productLink.GetAttribute("title");
                prod.Produktnamn = name;

                products.Add(prod);
            }

            products.Remove(products.First());
            products.Remove(products.First());

            return;
        }
    }
}
