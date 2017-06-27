using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite
{
    public class LayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repo = new ShoppingRepository(Properties.Settings.Default.ConStr);
            int id = System.Web.HttpContext.Current.Session["CartId"] != null
                ? (int)System.Web.HttpContext.Current.Session["CartId"] : 0;
            filterContext.Controller.ViewBag.Items = repo.GetCartItemsCount(id);
            base.OnActionExecuting(filterContext);
        }
    }
}