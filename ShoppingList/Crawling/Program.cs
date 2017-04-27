using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawling
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawl = new Crawler();

            crawl.CrawlMHS();
        }
    }
}
