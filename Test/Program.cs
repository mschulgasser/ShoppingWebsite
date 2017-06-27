using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoppingRepository repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            //Category c = new Category()
            //{
            //    Name = "Clothing"
            //};
            //repo.AddCategory(c);
            //List<Image> images = new List<Image>();
            //            repo.AddProduct(new Product()
            //{
            //    Title = "Shirt",
            //    Price = 20,
            //    CategoryId = 1,
            //    Description = "Very nice shirt"
            //}, images);
            IEnumerable<Product> products = repo.GetProducts(1); 
            foreach(Product p in products)
            {
                Console.WriteLine($"{p.Id} {p.Title} {p.Images.Count} {p.Category.Name}");
            }
            Console.ReadKey(true);
        }
    }
}
