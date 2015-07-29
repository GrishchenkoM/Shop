using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class LogController : Controller
    {
        #region public
        
        public LogController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("LogIn", "Account");
            var model = new LogViewModel {CustomerId = (int) Session["UserId"]};

            IEnumerable<IOrder> orders;
            IEnumerable<IProduct> products;
            IEnumerable<IProductsCustomers> productsCustomers;
            IEnumerable<ICustomer> cutomers;
            GetAllDataFromDb(out orders, out products, out productsCustomers, out cutomers);

            if (orders == null || products == null || productsCustomers == null) return View(model);

            IEnumerable<IProduct> myProducts;
            List<IOrder> mySoldOrders, myBoughtOrders;
            List<IProduct> soldProducts, boughtProducts;
            List<ICustomer> soldCustomers, boughtCustomers;
            CreateInfoLists(products, productsCustomers, orders, cutomers, out myProducts, out mySoldOrders, 
                            out myBoughtOrders, out soldProducts, out soldCustomers, out boughtProducts, out boughtCustomers);

            FillModel(model, mySoldOrders, soldCustomers, soldProducts, boughtProducts, 
                      boughtCustomers, myBoughtOrders, productsCustomers, myProducts);

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

#endregion

        #region private

        private void CreateInfoLists(IEnumerable<IProduct> products, IEnumerable<IProductsCustomers> productsCustomers, IEnumerable<IOrder> orders,
                                     IEnumerable<ICustomer> cutomers, out IEnumerable<IProduct> myProducts, out List<IOrder> mySoldOrders,
                                     out List<IOrder> myBoughtOrders, out List<IProduct> soldProducts, out List<ICustomer> soldCustomers,
                                     out List<IProduct> boughtProducts, out List<ICustomer> boughtCustomers)
        {
            // Products of current Customer
            MyProducts(products, productsCustomers, out myProducts);

            // Sold Products Orders of current Customer
            MySoldOrders(orders, myProducts, out mySoldOrders);

            // Bought Products Orders of current Customer
            MyBoughtOrders(orders, out myBoughtOrders);

            // Sold PRODUCTS of current Customer
            SoldProducts(myProducts, mySoldOrders, out soldProducts);

            // Customers who bought products of current Customer
            SoldCustomers(cutomers, mySoldOrders, out soldCustomers);

            // Bought PRODUCTS by current Customer
            BoughtProducts(products, myBoughtOrders, out boughtProducts);

            // ProductsCustomers of Products which were bought by current Customer
            List<IProductsCustomers> productsCustomersOfProducts;
            ProductsCustomersOfProducts(productsCustomers, boughtProducts, out productsCustomersOfProducts);

            // Customers which Products were bought by current Customer
            BoughtCustomers(cutomers, productsCustomersOfProducts, out boughtCustomers);
        }

        private void MyProducts(IEnumerable<IProduct> products, IEnumerable<IProductsCustomers> productsCustomers, out IEnumerable<IProduct> myProducts)
        {
            myProducts = from product in products
                         join prodCust in productsCustomers
                             on product.Id equals prodCust.ProductId
                         where prodCust.CustomerId == (int) Session["UserId"]
                         select product;
        }

        private void FillModel(LogViewModel model, List<IOrder> mySoldOrders, List<ICustomer> soldCustomers, List<IProduct> soldProducts, List<IProduct> boughtProducts,
                               List<ICustomer> boughtCustomers, List<IOrder> myBoughtOrders, IEnumerable<IProductsCustomers> productsCustomers, IEnumerable<IProduct> myProducts)
        {
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
                                   }).Distinct().OrderByDescending(x => x.OrderDate).ToList();

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
        }

        private void BoughtCustomers(IEnumerable<ICustomer> cutomers, List<IProductsCustomers> productsCustomersOfProducts, out List<ICustomer> boughtCustomers)
        {
            boughtCustomers = (from cutomer in cutomers
                               join productsCustomersOfProductsItem in productsCustomersOfProducts
                                   on cutomer.Id equals productsCustomersOfProductsItem.CustomerId
                               select cutomer).Distinct().ToList();
        }

        private void ProductsCustomersOfProducts(IEnumerable<IProductsCustomers> productsCustomers, List<IProduct> boughtProducts,
                                                 out List<IProductsCustomers> productsCustomersOfProducts)
        {
            productsCustomersOfProducts = (from prodCust in productsCustomers
                                           join currentBoughtProduct in boughtProducts
                                               on prodCust.ProductId equals currentBoughtProduct.Id
                                           select prodCust).Distinct().ToList();
        }

        private void BoughtProducts(IEnumerable<IProduct> products, List<IOrder> myBoughtOrders, out List<IProduct> boughtProducts)
        {
            boughtProducts = (from product in products
                              join ownBoughtOrder in myBoughtOrders
                                  on product.Id equals ownBoughtOrder.ProductId
                              select product).Distinct().ToList();
        }

        private void SoldCustomers(IEnumerable<ICustomer> cutomers, List<IOrder> mySoldOrders, out List<ICustomer> soldCustomers)
        {
            soldCustomers = (from cutomer in cutomers
                             join ownSoldOrder in mySoldOrders
                                 on cutomer.Id equals ownSoldOrder.CustomerId
                             select cutomer).Distinct().ToList();
        }

        private void SoldProducts(IEnumerable<IProduct> myProducts, List<IOrder> mySoldOrders, out List<IProduct> soldProducts)
        {
            soldProducts = (from ownProduct in myProducts
                            join ownSoldOrder in mySoldOrders
                                on ownProduct.Id equals ownSoldOrder.ProductId
                            select ownProduct).Distinct().ToList();
        }

        private void MyBoughtOrders(IEnumerable<IOrder> orders, out List<IOrder> myBoughtOrders)
        {
            myBoughtOrders = orders.Where(x => x.CustomerId == (int) Session["UserId"]).Distinct().ToList();
        }

        private void MySoldOrders(IEnumerable<IOrder> orders, IEnumerable<IProduct> myProducts, out List<IOrder> mySoldOrders)
        {
            mySoldOrders = new List<IOrder>();
            try
            {
                mySoldOrders = (from order in orders
                             join ownProduct in myProducts
                                 on order.ProductId equals ownProduct.Id
                             select order).ToList();
            }
            catch (Exception)
            {}
        }

        private void GetAllDataFromDb(out IEnumerable<IOrder> orders, out IEnumerable<IProduct> products, out IEnumerable<IProductsCustomers> productsCustomers,
                                      out IEnumerable<ICustomer> cutomers)
        {
            // All Orders
            orders = _dataManager.Orders.GetOrders();
            // All Products
            products = _dataManager.Products.GetProducts();
            // All ProductsCustomers
            productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers();
            // All Customers
            cutomers = _dataManager.Customers.GetCustomers();
        }

        private readonly DataManager _dataManager;
        
        #endregion
    }
}
