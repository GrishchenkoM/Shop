using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class HomeController : Controller
    {
        public HomeController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            
            HomeViewModel model = null;

            CreateModel(ref model, id);

            ViewBag.SearchString = model.SearchString;
            return View(model);
        }

        private void CreateModel(ref HomeViewModel model, int id = -1)
        {
            if (Session["HomeViewModel"] == null &&
                Session["UserId"] == null && id == -1)
            {
                Session.Add("UserId", id);
                model = new HomeViewModel {CustomerId = id};
                Session.Add("HomeViewModel", model);
            }
            else if (Session["HomeViewModel"] == null &&
                     Session["UserId"] == null && id != -1)
            {
                Session.Add("UserId", id);
                model = new HomeViewModel {CustomerId = id};
                Session.Add("HomeViewModel", model);
            }

            else if (Session["HomeViewModel"] == null &&
                     Session["UserId"] != null && id == -1)
            {
                if ((int) Session["UserId"] != -1)
                    id = (int) Session["UserId"];

                model = new HomeViewModel {CustomerId = id};
                Session.Add("HomeViewModel", model);
            }
            else if (Session["HomeViewModel"] == null &&
                     Session["UserId"] != null && id != -1)
            {
                if ((int) Session["UserId"] != id)
                    Session["UserId"] = id;

                model = new HomeViewModel {CustomerId = id};
                Session.Add("HomeViewModel", model);
            }

            else if (Session["HomeViewModel"] != null &&
                     Session["UserId"] == null && id == -1)
            {
                model = Session["HomeViewModel"] as HomeViewModel ?? new HomeViewModel {CustomerId = id};
                Session.Add("UserId", model.CustomerId);
            }
            else if (Session["HomeViewModel"] != null &&
                     Session["UserId"] == null && id != -1)
            {
                model = Session["HomeViewModel"] as HomeViewModel ?? new HomeViewModel {CustomerId = id};
                Session.Add("UserId", model.CustomerId);
            }

            else if (Session["HomeViewModel"] != null &&
                     Session["UserId"] != null && id == -1)
            {
                model = Session["HomeViewModel"] as HomeViewModel ?? new HomeViewModel {CustomerId = id};
                if ((int) Session["UserId"] != -1)
                    id = (int) Session["UserId"];
                model.CustomerId = id;
            }
            else if (Session["HomeViewModel"] != null &&
                     Session["UserId"] != null && id != -1)
            {
                model = Session["HomeViewModel"] as HomeViewModel ?? new HomeViewModel {CustomerId = id};

                if ((int) Session["UserId"] != id)
                    Session["UserId"] = id;

                if (model.CustomerId <= 0)
                    model.CustomerId = (int) Session["UserId"];
            }


            IEnumerable<IProductsCustomers> productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();
            IEnumerable<IProduct> products = _dataManager.Products.GetProducts();
            IEnumerable<IOrder> orders = _dataManager.Orders.GetOrders();

            if (productsCustomers != null && products != null)
            {
                var innerJoinQuery =
                    from prod in products
                    join prodCust in productsCustomers
                        on prod.Id equals prodCust.ProductId
                    where prodCust.Count > 0
                    orderby prod.Id descending
                    select prod;
                model.Products = new List<IProduct>();
                foreach (var product in innerJoinQuery)
                {
                    model.Products.Add(product);
                }

                // чаще всего покупается. если нет - последний добавленый
                if (orders != null)
                {
                    innerJoinQuery =
                        (from prod in products
                         join order in orders
                             on prod.Id equals order.ProductId
                         join prodCust in productsCustomers
                             on prod.Id equals prodCust.ProductId
                         where prodCust.Count > 0
                         orderby order.Count descending
                         select prod);

                    var distinctInnerJoinQuery = innerJoinQuery.Distinct();

                    model.PopularProducts = new List<IProduct>();
                    foreach (var product in distinctInnerJoinQuery)
                    {
                        model.PopularProducts.Add(product);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            string tempSearchString = model.SearchString;
            CreateModel(ref model);
            ViewBag.SearchString = tempSearchString;
            return View(model);
        }
        
        public ActionResult GetFoundProducts(string searchString)
        {
            IEnumerable<IProduct> products = null;

            if (!string.IsNullOrEmpty(searchString))
                products = _dataManager.Products.GetProducts().Where(
                    x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()));

            SearchViewModel model = null;
            if (products != null)
            {
                model = new SearchViewModel {SearchResultList = new List<IProduct>()};
                foreach (var product in products)
                    model.SearchResultList.Add(product);
            }
            return View("GetFoundProducts", model);
        }
        
        public int Id { get; private set; }

        private readonly DataManager _dataManager;
    }
}
