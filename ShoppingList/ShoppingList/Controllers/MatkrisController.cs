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
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Upload()
        {
            string businessname = System.Web.HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;

            return View(new UploadViewModel { Businessname = businessname });
        }

        public ActionResult Foretag()
        {
            return View();
        }

        [HttpPost]
        public ActionResult foretag(string email, string password)
        {
            if (dataAccess.ValidateUser(email, password))
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, dataAccess.GetBusinessname(email))},
                    "ApplicationCookie"
                    );

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return RedirectToAction("upload");
            }
            else
            {
                return RedirectToAction("foretag");
            }
        }

        [HttpPost]
        public ActionResult Upload(string emptyParameter)
        {
            var file = Request.Files[0];
            List<Error> errors = new List<Error>();

            if (file != null && file.ContentLength > 1)
            {
                var fileName = Path.GetFileName(file.FileName);
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(file.InputStream, Encoding.UTF8))
                {
                    UTF8Encoding ascii = new UTF8Encoding();
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
                    // dataAccess.UpdateProductlist(lines, );
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
            if (productlist[0].ToLower() != ("Artnummer;Produktnamn;Pris;kategori;typ;Bild-URL").ToLower())
            {
                errorlist.Add(new Error(1, "Första raden på prisfilen är ej korrekt formatterad. Se exempel för hur raden ska se ut."));
            }

            else
            {
                for (int i = 1; i < productlist.Count; i++)
                {
                    var details = productlist[i].Split(';');
                    var error = "";

                    if (details.Length != 6)
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
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("foretag");
        }
    }
}