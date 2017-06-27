using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; } 
    }
}