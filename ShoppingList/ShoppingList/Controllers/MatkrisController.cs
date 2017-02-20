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
        // GET: Matkris
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateFileUpload()
        {
            if (Request.Files.Count > 0)
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
                            lines.Add(line);
                            Debug.WriteLine(line);
                        }
                        return Json(new
                        { Response = "Din produktlista är godkänd, uppdatering av valda produkter sker automatiskt.", Success = true }
                        );
                    }
                    //hello
                }
            }
            return Json(new
            { Response = "Ett fel har inträffat. Vänligen se över exempelfilen innan uppladdning", Success = false }
            );
            //hello
        }
    }
}