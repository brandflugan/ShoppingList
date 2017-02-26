using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class Error
    {
        public int ErrorRow { get; set; }
        public string ErrorText { get; set; }

        public Error(int row, string text)
        {
            ErrorRow = row;
            ErrorText = text;
        }
    }
}