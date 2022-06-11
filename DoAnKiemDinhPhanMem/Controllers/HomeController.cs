using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnKiemDinhPhanMem.Models;

namespace DoAnKiemDinhPhanMem.Controllers
{
    public class HomeController : Controller
    {
        private cuasneakerEntities db = new cuasneakerEntities();
        public ActionResult Index(string searchString)
        {
            var products = db.Products.Include(p => p.ProductTypes);
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Productname.Contains(searchString) || p.Description.Contains(searchString));
            }
            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}