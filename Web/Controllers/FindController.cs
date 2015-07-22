using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class FindController : Controller
    {
        public FindController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            ViewBag.SearchString = "";
            return View();
        }
        [HttpPost, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
        public ActionResult Index(string searchString)
        {
            ViewBag.SearchString = searchString;
            return View();
        }

        public ActionResult GetFoundProducts(string searchString)
        {
            IEnumerable<IProduct> products;
            if (searchString == "")
                products = _dataManager.Products.GetProducts();
            else
                products = _dataManager.Products.GetProducts().Where(
                    x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()));

            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();

            if (products == null || productsCustomers == null) return RedirectToAction("Index", "Find");

            var model = new SearchViewModel {SearchResultList = new List<IProduct>()};


            model.SearchResultList = (from product in products
                                          join prodCust in productsCustomers
                                              on product.Id equals prodCust.ProductId
                                          where prodCust.CustomerId == (int)Session["UserId"]
                                          select product).ToList();

            return View("GetProducts",model);
        }

        private readonly DataManager _dataManager;
    }
}
