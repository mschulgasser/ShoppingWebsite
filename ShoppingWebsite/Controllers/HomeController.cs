using Shopping.Data;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShoppingWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? catId)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var vm = new IndexViewModel();
            vm.Categories = repo.GetCategories();
            vm.Products = repo.GetProducts(catId);
            vm.Images = repo.GetAllImages();
            return View(vm);
        }
        public ActionResult Product(int id)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var vm = new ProductViewModel();
            vm.Product = repo.GetProduct(id);
            vm.Categories = repo.GetCategories();
            return View(vm);
        }
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string email, string password)
        {
            var repo = new AccountRepository(Properties.Settings.Default.ConStr);
            Admin a = repo.Login(email, password);
            if (a != null)
            {
                FormsAuthentication.SetAuthCookie(email, true);
                return Redirect("/admin/index");
            }
            return Redirect("/home/login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            if (Session["CartId"] == null)
            {
                int id = repo.AddShoppingCart(new ShoppingCart());
                Session["CartId"] = id;
                ViewBag.CartId = Session["CartId"];
            }
            repo.AddShoppingCartItem(new ShoppingCartItem()
            {
                CartId = (int)Session["CartId"],
                ProductId = productId,
                Quantity = quantity
            });
            return Redirect("/");
        }
        public ActionResult Cart()
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var vm = new CartViewModel();
            if(Session["CartId"] != null)
            {
                int id = (int)Session["CartId"];
                vm.Cart = repo.GetCart(id);
                vm.TotalQuantity = repo.GetTotalQuantity(id);
                vm.TotalPrice = repo.GetTotalPrice(id);
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.DeleteItem(id);
            return Redirect("/home/cart");
        }
        [HttpPost]
        public ActionResult UpdateItem(int quantity, int id)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.UpdateItem(quantity, id);
            return Redirect("/home/cart");
        }

    }
}