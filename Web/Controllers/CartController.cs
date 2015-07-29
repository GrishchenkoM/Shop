using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
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
            return View(new CartViewModel
                {
                Cart = GetCart()
            });
        }

        [HttpPost]
        public ActionResult Index(CartViewModel model, FormCollection form)
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null) return RedirectToAction("Index", "Home");
            var resultModel = new CartViewModel {Cart = new Cart()};

            if (form.Keys.Count != 0) 
            {
                for (int i = 0; i < form.Keys.Count; ++i)
                {
                    string name = form.GetKey(i);
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

            DateTime time = DateTime.Now;

            foreach (CartLine item in cart.Lines)
            {
                var product = _dataManager.Products.GetProducts().FirstOrDefault(x => x.Id == item.Product.Id);
                var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers()
                    .FirstOrDefault(x => x.ProductId == item.Product.Id);

                int newCount;
                int purchaseCount;
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

        private Cart GetCart()
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

        public ActionResult ProductsList(Cart model, FormCollection form)
        {
            // make in cycle
            if (form.Keys.Count != 0) // if input exists
            {
                for (int i = 0; i < form.Keys.Count; ++i )
                {
                    string name = form.GetKey(i);
                    if (name.Contains("delete"))
                    {
                        string id = name.Substring(name.IndexOf('_') + 1, name.Length - 1 - name.IndexOf('_'));

                        var product = _dataManager.Products.GetProducts().FirstOrDefault(x=>x.Id == Convert.ToInt32(id));

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

            return View("ProductsList", new CartViewModel()
            {
                Cart = GetCart()
            });
        }

        private readonly DataManager _dataManager;
    }
}
