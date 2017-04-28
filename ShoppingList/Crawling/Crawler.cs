
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.PhantomJS;
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
    public class Crawler
    {
        public List<Product> CrawlMSE(string category)
        {
            List<Product> products = new List<Product>();

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.mat.se/search.html?q=" + category);
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
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);

            Actions actions = new Actions(driver);

            actions.KeyDown(Keys.Control).SendKeys("a").Perform();

            Thread.Sleep(2000);

            IWebElement productList = driver.FindElement(By.CssSelector("#categoryProductResults"));
            IReadOnlyCollection<IWebElement> productDivs = productList.FindElements(By.CssSelector("li"));

            foreach (IWebElement li in productDivs)
            {
                products.Add(SetProductDetails(li, category));
            }

            driver.Quit();

            return products;
        }

        private Product SetProductDetails(IWebElement li, string category)
        {
            var prod = new Product();

            prod.Artikelnummer = int.Parse(li.GetAttribute("data-product"));

            IWebElement div = li.FindElement(By.CssSelector(".outer"));

            string text = div.Text;
            text = text.Replace("\r\n", ";");

            var details = text.Split(';');

            string price = details[0];

            if (price.Split(' ').Length == 4)
                price = details[4].Split(' ')[2];
            else if (price.Contains(" :-"))
                price = details[0].Replace(" :-", string.Empty);
            else
                price = price.Insert(price.Length - 2, ",");

            prod.Pris = decimal.Parse(price);

            try
            {
                prod.Jmf = decimal.Parse(details[3].Split(' ')[2]);
            }
            catch { }

            prod.Produktnamn = details[1];
            prod.Produktnamn += " " + details[2];

            var img = div.FindElement(By.CssSelector("img"));
            prod.BildURL = img.GetAttribute("src");

            prod.Kategori = category;

            return prod;
        }

        public List<Product> CrawlMHS(string category)
        {
            List<Product> products = new List<Product>();

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.mathem.se/sok?q=" + category);
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
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);
            //jse.ExecuteScript("window.scrollBy(0,600);");
            //Thread.Sleep(2000);

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

                var jmf = div.FindElement(By.CssSelector("span#spnPricePerUnit")).Text;
                jmf = jmf.Replace("ca ", "");
                prod.Jmf = decimal.Parse(jmf.Split(' ')[0]);

                var img = div.FindElement(By.CssSelector("img"));
                prod.BildURL = img.GetAttribute("src");

                prod.Kategori = category;

                products.Add(prod);
            }

            driver.Quit();

            return products;
        }
    }
}
