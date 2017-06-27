using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}