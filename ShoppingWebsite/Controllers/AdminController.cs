using Shopping.Data;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var vm = new CategoriesViewModel();
            vm.Categories = repo.GetCategories();
            return View(vm);
        }
        [HttpPost]
        public void AddCategory(string name)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.AddCategory(new Category() { Name = name });
        }
        [HttpPost]
        public void DeleteCategory(int id)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.DeleteCategory(id);
        }
        [HttpPost]
        public void UpdateCategory(int id,string name)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.UpdateCategory(id, name);
        }
        public ActionResult Products()
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var vm = new IndexViewModel();
            vm.Products = repo.GetAllProducts();
            vm.Categories = repo.GetCategories();
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddProduct(Product p, IEnumerable<HttpPostedFileBase> files)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            List<Image> images = new List<Image>();
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Images/") + fileName);
                    images.Add(new Image()
                    {
                        FileName = fileName,
                        ProductId = p.Id
                    });
                }
            }
            repo.AddProduct(p, images);
            return Redirect("/admin/products");
        }
        [HttpPost]
        public void DeleteProduct(int id)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.DeleteProduct(id);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product p)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            repo.UpdateProduct(p);
            return Redirect("/admin/products");
        }
        public ActionResult GetImages(int productId)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            var images = repo.GetImages(productId).Select(i => new {
                fileName = i.FileName
            });
            return Json(new { images = images }, JsonRequestBehavior.AllowGet);
        }
    }
}