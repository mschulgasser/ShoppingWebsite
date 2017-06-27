using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class CartViewModel
    {
        public ShoppingCart Cart { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}