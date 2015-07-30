using System;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class EditController : Controller
    {
        #region public

        public EditController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult EditMenu()
        {
            return View();
        }

        public ActionResult Index(int id)
        {
            int currentProductId = id;
            var product = _dataManager.Products.GetProductById(currentProductId);
            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomersByProductId(currentProductId);

            if (product == null || productsCustomers == null)
                return RedirectToAction("Pity", "Error");

            if (productsCustomers.CustomerId != (int) Session["UserId"])
                return RedirectToAction("Index", "Purchase", new { id = currentProductId });

            Session["CurrentProductId"] = currentProductId;
            _model = new CreateProduct
            {
                Name = product.Name,
                Cost = product.Cost,
                Image = product.Image,
                Description = product.Description,
                IsAvailable = product.IsAvailable,
                Count = productsCustomers.Count
            };

            return View(_model);
        }

        [HttpPost]
        public ActionResult Index(CreateProduct model, FormCollection form, HttpPostedFileBase uploadImage)
        {
            if (form.GetKey(form.Keys.Count - 1) == "delete") // if input 'delete' stands always in the end
            {
                if (_dataManager.Products != null)
                    return Redirect(Auxiliary.Actions.Delete, DeleteProduct(model) 
                        ? Auxiliary.Result.OperationSuccess : Auxiliary.Result.Error);
            }
            else
            {
                if (!ModelState.IsValid) 
                    return View(model);
                if (_dataManager.Products != null)
                {
                    var oldProduct = _dataManager.Products.GetProductById(model.Id);
                    if (oldProduct == null)
                        return RedirectToAction("Pity", "Error");

                    var item = new Product();
                    ReadModel(model, item);
                    item.Id = (int) Session["CurrentProductId"];

                    if (uploadImage != null)
                        ReadImage(uploadImage, item);
                    else
                        item.Image = oldProduct.Image;

                    if (_dataManager.Products != null)
                        if (UpdateProduct(item, model))
                            return Redirect(Auxiliary.Actions.Delete, 
                                _dataManager.ProductsCustomers.UpdateProdCastRelation(item.Id, model.Count) 
                                ? Auxiliary.Result.OperationSuccess : Auxiliary.Result.Error);
                }
            }
            return View(model);
        }

        public ActionResult Redirect(Auxiliary.Actions action, Auxiliary.Result r)
        {
            return RedirectToAction("Finality", "Error", new { reaction = action , result = r });
        }
        
        #endregion

        #region private

        private bool UpdateProduct(IProduct item, CreateProduct model)
        {
            try
            {
                var currentProductId = _dataManager.Products.UpdateProduct(item);
                if (currentProductId == -1) 
                    return false;
                if (_dataManager.ProductsCustomers.UpdateProdCastRelation(currentProductId, model.Count))
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DeleteProduct(CreateProduct model)
        {
            var orders = _dataManager.Orders.GetOrderById(model.Id);
            return orders != null ? _dataManager.ProductsCustomers.DeleteProdCustRelation(model.Id) 
                                  : _dataManager.Products.DeleteProduct(model.Id);
        }

        private void ReadModel(CreateProduct model, IProduct item)
        {
            item.Name = model.Name;
            item.Image = model.Image;
            item.Description = model.Description;
            item.Cost = model.Cost;
            item.IsAvailable = model.IsAvailable;
        }

        private void ReadImage(HttpPostedFileBase uploadImage, IProduct item)
        {
            Auxiliary.ReadImage(uploadImage, item);
        }

        private readonly DataManager _dataManager;

        private CreateProduct _model;

        #endregion
    }
}
