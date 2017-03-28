using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShoppingList.Controllers
{
    public class MatkrisController : Controller
    {
        private DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

        // GET: Matkris
        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult index(List<Product> products)
        {
            if (!products.Any(p => p.Antal < 1 || p.Antal > 99))
            {
                products = products.Where(p => p != null).ToList();

                var suppliers = dataAccess.MatchSuppliersWithProducts(products);

                Score.AddProductPrices(suppliers);

                //Vi går vidare till rätt View här
                return View("resultat", suppliers);
            }
            return RedirectToAction("/index");
        }

        public ActionResult resultat(List<Supplier> suppliers = null)
        {
            if(suppliers == null)
                return RedirectToAction("/index");
            else 
                return View(suppliers);
        }

        [Authorize]
        public ActionResult uppladdning()
        {
            string businessname = System.Web.HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;

            return View(new UploadViewModel { Businessname = businessname });
        }

        public ActionResult foretag()
        {
            return View();
        }

        [HttpPost]
        public ActionResult foretag(string email, string password)
        {
            var businessname = dataAccess.ValidateUser(email, password);

            if (businessname != null)
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, businessname)},
                    "ApplicationCookie"
                    );

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return RedirectToAction("/uppladdning");
            }
            else
            {
                return RedirectToAction("/foretag");
            }
        }

        [HttpPost]
        public ActionResult uppladdning(string emptyParameter)
        {
            var file = Request.Files[0];
            List<Error> errors = new List<Error>();

            if (file != null && file.ContentLength > 1)
            {
                var fileName = Path.GetFileName(file.FileName);
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(file.InputStream, Encoding.UTF8))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("\uFFFD"))
                        {
                            errors.Add(new Error(0, "Den uppladdade filten är inte i korrekt format. Spara filen som UTF-8 för att fortsätta."));
                            break;
                        }
                        lines.Add(line);
                    }
                    if (errors.Count != 1)
                    {
                        ValidateProductlist(lines, ref errors);
                    }
                }

                if (errors.Count == 0)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    string businessemail = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

                    dataAccess.UpdateProductlist(lines, businessemail);
                }
            }
            else
            {
                errors.Add(new Error(0, "Den uppladdade filten är ej giltig eller saknar innehåll. Se exempel för hur filen ska se ut."));
            }

            string businessname = System.Web.HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;

            return View(new UploadViewModel { Errors = errors, Businessname = businessname });
        }

        private void ValidateProductlist(List<string> productlist, ref List<Error> errorlist)
        {
            if (productlist[0].ToLower() != ("Artnummer;Produktnamn;Pris;jmf;kategori;typ;Bild-URL").ToLower())
            {
                errorlist.Add(new Error(1, "Första raden på prisfilen är ej korrekt formatterad. Se exempel för hur raden ska se ut."));
            }

            else
            {
                for (int i = 1; i < productlist.Count; i++)
                {
                    var details = productlist[i].Split(';');
                    var error = "";

                    if (details.Length != 7)
                    {
                        error += "Raden innehåller inte korrekt antal fält. \n";
                    }
                    else
                    {
                        int value = 0;
                        if (!int.TryParse(details[0], out value))
                        {
                            error += "Fältet Artnummer är inte ett nummer: " + details[0] + ". \n";
                        }
                        else
                        {
                            if (details[0].Length != 4)
                            {
                                error += "Fältet Artnummer är inte korrekt längd: " + details[0] + ". \n";
                            }
                        }
                        details[2].Replace(',', '.');
                        decimal dvalue = 0;

                        if (!decimal.TryParse(details[2], out dvalue))
                        {
                            error += "Fältet Pris är inte ett belopp: " + details[2] + ". \n";
                        }

                        if (!decimal.TryParse(details[3], out dvalue))
                        {
                            error += "Fältet jmf är inte ett belopp: " + details[2] + ". \n";
                        }

                    }
                    if (error != "")
                    {
                        errorlist.Add(new Error(i + 1, error));
                        errorlist[errorlist.Count - 1].ErrorText += error;
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("/foretag");
        }

        [HttpPost]
        public JsonResult search(string searchterm)
        {
            List<Product> productList;

            productList = dataAccess.GetTopMatchesByName(searchterm);

            return Json(productList, JsonRequestBehavior.AllowGet);
        }
    }
}