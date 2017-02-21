using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShoppingList.Controllers
{
    public class MatkrisController : Controller
    {
        private DataAccess.DataAccess dataAccess;

        // GET: Matkris
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult foretag()
        {
            return View();
        }

        [HttpPost]
        public ActionResult foretag(string email, string password)
        {
            if (dataAccess.ValidateUser(email, password))
            {
                return View("~/Views/Matkris/Upload.cshtml");
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult Upload(string emptyParameter)
        {
            var file = Request.Files[0];

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(file.InputStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lines.Count > 0)
                        {
                            lines.Add(line);
                            Debug.WriteLine(line);
                        }
                    }
                }
                // dataAccess.UpdateProductlist(lines, );

                return View(model: "Allting gick bra");
                //   return View("~/Views/Matkris/Upload.cshtml", null, "Allting gick bra");
            }
            return View(model: "Allting gick dåligt");
            //return View("~/Views/Matkris/Upload.cshtml", null, "Allting gick dåligt");
        }
    }
}