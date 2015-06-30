using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public HomeController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            //if (id == -1) 
            //    return RedirectToAction("LogIn", "Account");
            if (id != -1)
                Session["UserId"] = id;
                
            var model = new HomeViewModel {CustomerId = id};

            IEnumerable<IProductsCustomers> productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();
            IEnumerable<IProduct> products = _dataManager.Products.GetProducts();

            var innerJoinQuery =
                from prod in products
                join prodCust in productsCustomers
                    on prod.Id equals prodCust.ProductId
                select prod;
            model.Products = new List<IProduct>();
            foreach (var product in innerJoinQuery)
            {
                model.Products.Add(product);
            }
            
            // реализовать популярные товары

            return View(model);
        }

        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }

        
        public int Id { get; private set; }

        private readonly DataManager _dataManager;
    }
}
