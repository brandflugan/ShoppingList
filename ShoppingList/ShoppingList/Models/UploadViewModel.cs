using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class UploadViewModel
    {
        public List<Error> Errors { get; set; }
        public string Businessname { get; set; }
    }
}