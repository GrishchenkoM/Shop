using System;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class PurchaseController : Controller
    {
        #region public

        public PurchaseController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            var currentId = id;
            if (Session["UserId"] != null && (int)Session["UserId"] == -1)
                return RedirectToAction("LogIn", "Account", new { id = currentId });

            var product = _dataManager.Products.GetProductById(id);
            if (product == null) 
                return RedirectToAction("Pity", "Error");

            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomersByProductId(product.Id);

            Session["CurrentProductId"] = product.Id;
            CreateProduct model;
            FillModel(product, productsCustomers, out model);
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

            var product = _dataManager.Products.GetProductById((int)Session["CurrentProductId"]);
            if (product == null) 
                return RedirectToAction("Pity", "Error");

            AddToCart(model, product);

            ViewBag.IsAddToCart = true;
            return View(Session["CreateProductModel"]);
        }

        public ActionResult Success(int id = 0)
        {
            ViewBag.IsAddToCart = null;
            return View(Session["CreateProductModel"]);
        }
        
        #endregion

        #region private

        private void FillModel(IProduct product, IProductsCustomers productsCustomers, out CreateProduct model)
        {
            model = new CreateProduct
                {
                    Name = product.Name,
                    Image = product.Image,
                    Cost = product.Cost,
                    Description = product.Description,
                    IsAvailable = product.IsAvailable,
                    IsMine = productsCustomers.CustomerId == (int) Session["UserId"]
                };
            Session["CreateProductModel"] = model;
        }

        private void AddToCart(CreateProduct model, IProduct product)
        {
            GetCart().AddItem(product, model.Count);
            Session["Cart"] = GetCart();
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

        private readonly DataManager _dataManager;
        
        #endregion
    }
}
