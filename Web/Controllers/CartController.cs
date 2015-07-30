using System;
using System.Web.Mvc;
using BusinessLogic;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof (Exception), View = "Pity")]
    public class CartController : Controller
    {
        #region public

        public CartController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            return View(new CartViewModel
                {
                    Cart = GetCart()
                });
        }

        [HttpPost]
        public ActionResult Index(CartViewModel model, FormCollection form)
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null) 
                return RedirectToAction("Index", "Home");

            var resultModel = new CartViewModel {Cart = new Cart()};
            try
            {
                if (form.Keys.Count != 0)
                    for (int i = 0; i < form.Keys.Count; ++i)
                    {
                        var name = form.GetKey(i);
                        if (name.Contains("delete_all"))
                        {
                            GetCart().Clear();
                            Session["Cart"] = null;
                            ViewBag.IsAddToCart = null;
                            ViewBag.CartIsEmpty = true;
                            return RedirectToAction("Index", "Home");
                        }
                    }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            if (!OrderService(cart, resultModel))
                return RedirectToAction("Finality", "Error", new
                    {
                        action = Auxiliary.Actions.Purchase, 
                        result = Auxiliary.Result.Error
                    });

            Session["BoughtProducts"] = resultModel;
            Session["Cart"] = null;
            return RedirectToAction("Success", "Purchase");
        }

        public ActionResult ProductsList(Cart model, FormCollection form)
        {
            if (form.Keys.Count != 0) // if input exists
            {
                for (var i = 0; i < form.Keys.Count; ++i)
                {
                    string name = form.GetKey(i);
                    if (name.Contains("delete"))
                    {
                        var id = name.Substring(name.IndexOf('_') + 1, name.Length - 1 - name.IndexOf('_'));
                        var product = _dataManager.Products.GetProductById(Convert.ToInt32(id));

                        GetCart().RemoveItem(product);
                        if (GetCart().IsEmpty)
                        {
                            ViewBag.IsAddToCart = null;
                            Session["Cart"] = null;
                        }
                        else
                        {
                            Session["Cart"] = GetCart();
                        }
                        break;
                    }
                }
            }

            if (Session["Cart"] == null)
            {
                ViewBag.CartIsEmpty = true;
                return View("ProductsList", null);
            }

            return View("ProductsList", new CartViewModel
                {
                    Cart = GetCart()
                });
        }

        #endregion

        #region private

        private bool OrderService(Cart cart, CartViewModel resultModel)
        {
            var time = DateTime.Now;

            foreach (var item in cart.Lines)
            {
                var product = _dataManager.Products.GetProductById(item.Product.Id);
                var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomersByProductId(item.Product.Id);
                if (product == null || productsCustomers == null)
                    return false;

                int newCount, purchaseCount;
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

                if (!_dataManager.Orders.AddNewOrder((int) Session["UserId"],
                                                     item.Product.Id, time, purchaseCount))
                    continue;

                if (!_dataManager.ProductsCustomers.UpdateProdCastRelation(item.Product.Id, newCount))
                    _dataManager.Orders.DeleteOrder((int) Session["UserId"],
                                                    (int) Session["CurrentProductId"], time);
                if (newCount == 0)
                {
                    product.IsAvailable = false;
                    _dataManager.Products.UpdateProduct(product);
                }

                resultModel.Cart.AddItem(item.Product, item.Quantity);
            }
            return true;
        }

        private Cart GetCart()
        {
            Cart cart = null;
            if (Session["Cart"] != null)
                cart = (Cart) Session["Cart"];
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
