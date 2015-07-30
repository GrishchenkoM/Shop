using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class FindController : Controller
    {
        #region public

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
            var products = searchString == "" ? _dataManager.Products.GetProducts() : _dataManager.Products.GetProductsByName(searchString);
            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();

            if (products == null || productsCustomers == null) return RedirectToAction("Index", "Find");

            var model = new SearchViewModel {SearchResultList = new List<IProduct>()};
            model.SearchResultList = (from product in products
                                      join prodCust in productsCustomers
                                          on product.Id equals prodCust.ProductId
                                      where prodCust.CustomerId == (int) Session["UserId"]
                                      select product).ToList();

            return View("GetProducts",model);
        }
        
        #endregion

        #region private

        private readonly DataManager _dataManager;

        #endregion
    }
}
