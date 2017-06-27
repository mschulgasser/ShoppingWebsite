using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}