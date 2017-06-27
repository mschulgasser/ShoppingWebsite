using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data
{
    public class ShoppingRepository
    {
        private string _connectionString;
        public ShoppingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddCategory(Category c)
        {
            using(var context = new ShoppingDbDataContext(_connectionString))
            {
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
            }
        }
        public void AddProduct(Product p, IEnumerable<Image> images)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.Products.InsertOnSubmit(p);
                context.SubmitChanges();
                foreach(Image i in images)
                {
                    i.ProductId = p.Id;
                    context.Images.InsertOnSubmit(i);
                }
                context.SubmitChanges();
            }
        }
       
        public int AddShoppingCart(ShoppingCart cart)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                cart.DateCreated = DateTime.Now.Date;
                context.ShoppingCarts.InsertOnSubmit(cart);
                context.SubmitChanges();
                return cart.Id;
            }
        }
        public void AddShoppingCartItem(ShoppingCartItem item)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var existingItem = context.ShoppingCartItems.FirstOrDefault(i => i.ProductId == item.ProductId
                    && i.CartId == item.CartId);
                if(existingItem != null)
                {
                    context.ExecuteCommand("UPDATE ShoppingCartItems SET Quantity = Quantity + {0} WHERE Id = {1} ",
                         item.Quantity, existingItem.Id);
                }
                else
                {
                    context.ShoppingCartItems.InsertOnSubmit(item);
                    context.SubmitChanges();
                }
            }
        }
        public IEnumerable<Category> GetCategories()
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Category>(c => c.Products);
                loadOptions.LoadWith<Product>(p => p.Images);
                context.LoadOptions = loadOptions;

                return context.Categories.ToList();
            }
        }
        public IEnumerable<Product> GetProducts(int? categoryId)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(p => p.Category);
                loadOptions.LoadWith<Product>(p => p.Images);
                context.LoadOptions = loadOptions;
                if (categoryId.HasValue)
                {
                    return context.Products.Where(p => p.CategoryId == categoryId).ToList();
                }
                return context.Categories.First().Products.ToList();
            }
        }
        public IEnumerable<Product> GetAllProducts()
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(p => p.Category);
                loadOptions.LoadWith<Product>(p => p.Images);
                context.LoadOptions = loadOptions;
                return context.Products.ToList();
            }
        }
        public IEnumerable<ShoppingCartItem> GetShoppingCartItems(int cartId)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ShoppingCartItem>(item => item.Product);
                loadOptions.LoadWith<ShoppingCartItem>(item => item.ShoppingCart);
                loadOptions.LoadWith<Product>(p => p.Images);
                loadOptions.LoadWith<Product>(p => p.Category);
                context.LoadOptions = loadOptions;

                return context.ShoppingCartItems.Where(item => item.CartId == cartId).ToList();
            }
        }
        public IEnumerable<Image> GetAllImages()
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.Images.ToList();
            }
        }
        public Product GetProduct(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(p => p.Images);
                loadOptions.LoadWith<Product>(p => p.Category);
                context.LoadOptions = loadOptions;
                return context.Products.FirstOrDefault(p => p.Id == id);
            }
        }
        public void DeleteCategory(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE Categories WHERE Id = {0}", id);
            }
        }

        public void UpdateCategory(int id, string name)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Categories SET Name = {0} WHERE Id = {1}", name, id);
            }
        }
        public int GetCartItemsCount(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.ShoppingCartItems.Count(i => i.CartId == id);
            }
        }
        public ShoppingCart GetCart(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ShoppingCart>(c => c.ShoppingCartItems);
                loadOptions.LoadWith<ShoppingCartItem>(i => i.Product);
                loadOptions.LoadWith<Product>(p => p.Images);
                loadOptions.LoadWith<Product>(p => p.Category);
                context.LoadOptions = loadOptions;
                return context.ShoppingCarts.FirstOrDefault(c => c.Id == id);
            }
        }
        public void DeleteItem(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE ShoppingCartItems WHERE Id = {0}", id);
            }
        }
        public int GetTotalQuantity(int cartId)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.ShoppingCartItems.Where(i => i.CartId == cartId)
                    .Sum(i => i.Quantity);
            }
        }
        public decimal GetTotalPrice(int cartId)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.ShoppingCartItems.Where(i => i.CartId == cartId)
                    .Sum(i => i.Quantity * i.Product.Price);
            }
        }
        public void UpdateItem(int quantity, int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand(@"UPDATE ShoppingCartItems SET 
                    Quantity = {0} WHERE Id = {1}", quantity, id);
            }
        }
        public void DeleteProduct(int id)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE Images WHERE ProductId = {0}; DELETE Products WHERE Id = {0}", id);
            }
        }
        public void UpdateProduct(Product p)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                context.ExecuteCommand(@"UPDATE Products SET CategoryId = {0}, Title = {1},
                    Description = {2}, Price = {3} WHERE Id = {4}", p.CategoryId, p.Title, p.Description, p.Price, p.Id);
            }
        }
        public IEnumerable<Image> GetImages(int productId)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.Images.Where(i => i.ProductId == productId);
            }
        }
    }
}
