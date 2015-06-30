using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Web.Models;

namespace Web.Controllers
{
    public class PurchaseController : Controller
    {
        public PurchaseController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            var product = _dataManager.Products.GetProducts().FirstOrDefault(x => x.Id == id);
            if (product == null) return RedirectToAction("Pity", "Error");

            Session.Add("CurrentProductId", product.Id);
            var model = new CreateProduct
                {
                    Name = product.Name,
                    Image = product.Image,
                    Cost = product.Cost,
                    Description = product.Description,
                    IsAvailable = product.IsAvailable
                };
            Session.Add("Model", model);
            return View(model);
        }

        
        [HttpPost,Authorize]
        public ActionResult Index(CreateProduct model)
        {
            int newCount, purchaseCount;
            if ((int) Session["UserId"] == -1)
                return RedirectToAction("LogIn", "Account");
            // проверить кол-во. если просит больше - вернуть все, что осталось
            var item = _dataManager.ProductsCustomers.GetProductsCustomers()
                                                .FirstOrDefault(x => x.ProductId == (int) Session["CurrentProductId"]);
            if (item == null) return RedirectToAction("Pity", "Error");

            if (item.Count >= model.Count)
            {
                newCount = item.Count - model.Count;
                purchaseCount = model.Count;
            }
            else
            {
                newCount = 0;
                purchaseCount = item.Count;
            }

            // создать запись в Order
            DateTime time = DateTime.Now;
            if (!_dataManager.Orders.AddNewOrder((int) Session["UserId"],
                                                 (int)Session["CurrentProductId"], time, newCount))
                return RedirectToAction("Pity", "Error");

            // вычесть из ProductCustomers
            if (_dataManager.ProductsCustomers.UpdateProdCastRelation((int) Session["UserId"],
                                                                      (int)Session["CurrentProductId"], purchaseCount))
                return RedirectToAction("Success", "Purchase", new {id = purchaseCount});

            _dataManager.Orders.DeleteOrder((int) Session["UserId"],
                                            (int)Session["CurrentProductId"], time);
            return RedirectToAction("Pity", "Error");
        }

        public ActionResult Success(int id)
        {
            ViewBag.PurchaseCount = id;

            return View(Session["Model"]);
        }

        private readonly DataManager _dataManager;
    }
}
