using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class PurchaseController : Controller
    {
        public PurchaseController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            int currentId = id;
            if (Session["UserId"] != null && (int)Session["UserId"] == -1)
                return RedirectToAction("LogIn", "Account", new { id = currentId });

            var product = _dataManager.Products.GetProducts().FirstOrDefault(x => x.Id == id);
            if (product == null) return RedirectToAction("Pity", "Error");

            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers()
                .FirstOrDefault(x => x.ProductId == product.Id);

            Session.Add("CurrentProductId", product.Id);
            var model = new CreateProduct
                {
                    Name = product.Name,
                    Image = product.Image,
                    Cost = product.Cost,
                    Description = product.Description,
                    IsAvailable = product.IsAvailable,
                    IsMine = productsCustomers.CustomerId == (int)Session["UserId"]
                };
            if (Session["CreateProductModel"] == null)
                Session.Add("CreateProductModel", model);
            else
                Session["CreateProductModel"] = model;
            return View(model);
        }

        
        [HttpPost,Authorize]
        public ActionResult Index(CreateProduct model)
        {
            if (Session["UserId"] != null && (int) Session["UserId"] == -1)
                return RedirectToAction("LogIn", "Account");

            if (model.Count <= 0)
            {
                model = Session["CreateProductModel"] as CreateProduct;
                return View(model);
            }

            var product = _dataManager.Products.GetProducts()
                                                .FirstOrDefault(x => x.Id == (int) Session["CurrentProductId"]);
            
            if (product == null) return RedirectToAction("Pity", "Error");

            // add to cart
            GetCart().AddItem(product, model.Count);
            Session["Cart"] = GetCart();
            
            ViewBag.Message = "Товар добавлен в корзину!";
            ViewBag.Cart = true;
            return View(Session["CreateProductModel"]);
        }

        
        private Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult Success(int id = 0)
        {
            return View(Session["CreateProductModel"]);
        }

        private readonly DataManager _dataManager;
    }
}
