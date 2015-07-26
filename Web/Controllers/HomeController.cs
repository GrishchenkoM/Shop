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
        #region public

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
                SetSearchResultListModel(ref model, products);

            return View("GetFoundProducts", model);
        }

        #endregion

        #region private

        private void CreateModel(ref HomeViewModel model, int id = -1)
        {
            ModelAndUserIdSetUp(ref model, id);
            SetContentInModel(model);
        }

        private void ModelAndUserIdSetUp(ref HomeViewModel model, int id)
        {
            if (!HomeViewModelSessionExist() && !UserIdSessionExist())
            {
                SetUserId(id);
                SetHomeViewModel(ref model, id);
            }
            else if (!HomeViewModelSessionExist() && UserIdSessionExist() &&
                     ((id == -1 && UserIdSession != -1) ||
                      (id != -1 && UserIdSession != id)))
            {
                if (UserIdSession != -1) SetId(out id);
                else if (UserIdSession != id) SetUserId(id);

                SetHomeViewModel(ref model, id);
            }
            else if (HomeViewModelSessionExist() && !UserIdSessionExist())
            {
                SetHomeViewModelAs(out model, id);
                SetUserId(model.CustomerId);
            }
            else if (HomeViewModelSessionExist() && UserIdSessionExist())
            {
                SetHomeViewModelAs(out model, id);

                if (id == -1)
                {
                    if (UserIdSession != -1) SetId(out id);
                    model.CustomerId = id;
                }
                else
                {
                    if (UserIdSession != id) SetUserId(id);
                    if (model.CustomerId <= 0)
                        model.CustomerId = UserIdSession;
                }
            }
        }

        private void SetContentInModel(HomeViewModel model)
        {
            IEnumerable<IProductsCustomers> productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();
            IEnumerable<IProduct> products = _dataManager.Products.GetProducts();
            IEnumerable<IOrder> orders = _dataManager.Orders.GetOrders();

            if (productsCustomers != null && products != null)
            {
                SetModelProducts(model, products, productsCustomers);

                if (orders != null)
                    SetModelPopularProducts(model, products, orders, productsCustomers);
            }
        }

        private void SetModelPopularProducts(HomeViewModel model, IEnumerable<IProduct> products, IEnumerable<IOrder> orders,
                                             IEnumerable<IProductsCustomers> productsCustomers)
        {
            var innerJoinQuery =
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

        private void SetModelProducts(HomeViewModel model, IEnumerable<IProduct> products, IEnumerable<IProductsCustomers> productsCustomers)
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
        }
        
        private void SetId(out int id)
        {
            id = (int) Session["UserId"];
        }

        private void SetUserId(int id)
        {
            Session["UserId"] = id;
        }

        private void SetHomeViewModelAs(out HomeViewModel model, int id)
        {
            model = Session["HomeViewModel"] as HomeViewModel ?? new HomeViewModel { CustomerId = id };
        }

        private void SetHomeViewModel(ref HomeViewModel model, int id)
        {
            model = new HomeViewModel {CustomerId = id};
            Session["HomeViewModel"] = model;
        }

        private void SetSearchResultListModel(ref SearchViewModel model, IEnumerable<IProduct> products)
        {
            model = new SearchViewModel {SearchResultList = new List<IProduct>()};
            foreach (var product in products)
                model.SearchResultList.Add(product);
        }

        private bool HomeViewModelSessionExist()
        {
            return Session["HomeViewModel"] != null;
        }

        private bool UserIdSessionExist()
        {
            return Session["UserId"] != null;
        }
        
        private int UserIdSession
        {
            get { return (int)Session["UserId"]; }
        }

        private readonly DataManager _dataManager;

        #endregion
    }
}
