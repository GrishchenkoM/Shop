using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class CartController : Controller
    {
        public CartController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            return View(new CartViewModel()
            {
                Cart = GetCart()
            });
        }

        [HttpPost]
        public ActionResult Index(CartViewModel model)
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null) return RedirectToAction("Index", "Home");
            var resultModel = new CartViewModel();
            resultModel.Cart = new Cart();

            int newCount, purchaseCount;
            DateTime time = DateTime.Now;

            foreach (CartLine item in cart.Lines)
            {
                var product = _dataManager.Products.GetProducts().FirstOrDefault(x => x.Id == item.Product.Id);
                var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers()
                    .FirstOrDefault(x => x.ProductId == item.Product.Id);

                if (item.Quantity < productsCustomers.Count)
                {
                    newCount = productsCustomers.Count - item.Quantity;
                    purchaseCount = item.Quantity;
                }
                else
                {
                    newCount = 0;
                    purchaseCount = productsCustomers.Count;
                }

                if (!_dataManager.Orders.AddNewOrder((int)Session["UserId"],
                                                     item.Product.Id, time, purchaseCount))
                    continue;

                if (!_dataManager.ProductsCustomers.UpdateProdCastRelation(item.Product.Id, newCount))
                    _dataManager.Orders.DeleteOrder((int)Session["UserId"],
                                                (int)Session["CurrentProductId"], time);
                if (newCount == 0)
                {
                    product.IsAvailable = false;
                    _dataManager.Products.UpdateProduct(product);
                }
                
                resultModel.Cart.AddItem(item.Product, item.Quantity);
            }
            Session["BoughtProducts"] = resultModel;
            Session["Cart"] = null;
            return RedirectToAction("Success","Purchase");
        }

        // public for tests
        public Cart GetCart()
        {
            Cart cart = null;
            if (Session["Cart"]!= null)
                cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        //public ActionResult RemoveFromCart(int id)
        //{
        //    var product = _dataManager.ProductsCustomers.GetProductsCustomers()
        //                                        .FirstOrDefault(x => x.ProductId == (int)Session["CurrentProductId"]);

        //    if (product != null)
        //    {
        //        GetCart().RemoveLine((IProduct)product);
        //        ViewBag.Message = "Товар удален из корзины";
        //    }
        //    else
        //        ViewBag.Message = "Товар не может быть удален из корзины";
        //    return View(Session["Cart"]);
        //}

        private readonly DataManager _dataManager;
    }
}
