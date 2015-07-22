using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class LogController : Controller
    {
        public LogController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("LogIn", "Account");
            var model = new LogViewModel {CustomerId = (int) Session["UserId"]};
            
            // All Orders
            var orders = _dataManager.Orders.GetOrders();
            // All Products
            var products = _dataManager.Products.GetProducts();
            // All ProductsCustomers
            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();
            // All Customers
            var cutomers = _dataManager.Customers.GetCustomers();

            if (orders == null || products == null || productsCustomers == null) return View(model);
            
            // Products of current Customer
            var myProducts = from product in products
                              join prodCust in productsCustomers
                              on product.Id equals prodCust.ProductId
                              where prodCust.CustomerId == (int)Session["UserId"]
                              select product;

            // Sold Products Orders of current Customer
            List<IOrder> mySoldOrders = (from order in orders
                            join ownProduct in myProducts
                            on order.ProductId equals ownProduct.Id
                            select order).ToList();

            // Bought Products Orders of current Customer
            List<IOrder> myBoughtOrders = orders.Where(x => x.CustomerId == (int)Session["UserId"]).Distinct().ToList();

            //if (ownSoldOrders.Count == 0) return View(model);

            // Sold PRODUCTS of current Customer
            List<IProduct> soldProducts = (from ownProduct in myProducts
                                  join ownSoldOrder in mySoldOrders
                                  on ownProduct.Id equals ownSoldOrder.ProductId
                                  select ownProduct).Distinct().ToList();
            // Customers who bought products of current Customer
            List<ICustomer> soldCustomers = (from cutomer in cutomers
                                   join ownSoldOrder in mySoldOrders
                                   on cutomer.Id equals ownSoldOrder.CustomerId
                                   select cutomer).Distinct().ToList();

            // Bought PRODUCTS by current Customer
            List<IProduct> boughtProducts = (from product in products
                                                    join ownBoughtOrder in myBoughtOrders
                                                  on product.Id equals ownBoughtOrder.ProductId
                                                    select product).Distinct().ToList();

            // ProductsCustomers of Products which were bought by current Customer
            List<IProductsCustomers> productsCustomersOfProducts = (from prodCust in productsCustomers
                                                                   join currentBoughtProduct in boughtProducts
                                                                   on prodCust.ProductId equals currentBoughtProduct.Id
                                                                   select prodCust).Distinct().ToList();

            // Customers which Products were bought by current Customer
            List<ICustomer> boughtCustomers = (from cutomer in cutomers
                                                                join productsCustomersOfProductsItem in productsCustomersOfProducts
                                                                on cutomer.Id equals productsCustomersOfProductsItem.CustomerId
                                                                select cutomer).Distinct().ToList();

            

            model.ItemsSold = (from order in mySoldOrders
                               join customer in soldCustomers on order.CustomerId equals customer.Id
                               join product in soldProducts on order.ProductId equals product.Id
                               select new LogItem
                                   {
                                       ProductId = product.Id,
                                       ProductName = product.Name,
                                       ProductImage = product.Image,
                                       CustomerId = customer.Id,
                                       CustomerName = customer.UserName,
                                       Count = order.Count,
                                       OrderDate = order.OrderDateTime
                                   }).Distinct().OrderByDescending(x=>x.OrderDate).ToList();
                
            model.ItemsBought = (
                from product in boughtProducts
                from customer in boughtCustomers
                from order in myBoughtOrders
                from productCustomer in productsCustomers
                where ((order.ProductId == productCustomer.ProductId) 
                && (customer.Id == productCustomer.CustomerId)
                && (order.ProductId == product.Id))
                orderby order.OrderDateTime descending 
            select new LogItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    CustomerId = customer.Id,
                    CustomerName = customer.UserName,
                    Count = order.Count,
                    OrderDate = order.OrderDateTime,
                    IsMine = myProducts.FirstOrDefault(x => x.Id == product.Id) != null
                }).Distinct().ToList();

            int approximateAmount = soldProducts.Sum(x => Convert.ToInt32(x.Cost));
            
            model.ApproximateAmount = approximateAmount;
            model.CustomerId = (int) Session["UserId"];

            Session.Add("Log", model);

            return View(model);
        }


        public ActionResult Submit(int id = 0)
        {
            //System.Collections.Specialized.NameValueCollection s = Request.Form;
            //List<string> str = new List<string>();
            //foreach (var it in s)
            //{
            //    str.Add(it.ToString());
            //}
            //string submitName = Request.Form.ToString();
            var model = (LogViewModel) Session["Log"];
            if (model == null) return View();

            if (Request.Form.ToString().Contains("ClearBoughtItems"))
                foreach (var item in model.ItemsBought)
                    //_dataManager.Orders.DeleteOrder(model.CustomerId, item.ProductId, item.OrderDate);
                    _dataManager.Orders.DeleteOrder(item.ProductId, item.OrderDate);
            else
                foreach (var item in model.ItemsSold)
                    //_dataManager.Orders.DeleteOrder(model.CustomerId, item.ProductId, item.OrderDate);
                    _dataManager.Orders.DeleteOrder(item.ProductId, item.OrderDate);

            return RedirectToAction("Index");
        }

        private readonly DataManager _dataManager;
    }
}
