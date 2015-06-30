using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class EditController : Controller
    {
        public EditController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id)
        {
            var product = _dataManager.Products.GetProducts()
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
                return RedirectToAction("Pity", "Error");

            var productsCustomers = _dataManager.ProductsCustomers.GetProductsCustomers()
                .FirstOrDefault(x => x.ProductId == id);

            if (productsCustomers == null)
                return RedirectToAction("Pity", "Error");

            Session.Add("CurrentProductId", id);
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
            bool answer = false;

            if (form.GetKey(form.Keys.Count - 1) == "delete") // if input 'delete' stands always in the end
            {
                if (_dataManager.Products != null)
                    answer = DeleteProduct(model);
            }
            else
            {
                if (!ModelState.IsValid) return View(model);
                if (_dataManager.Products != null)
                {
                    var oldProduct = _dataManager.Products.GetProducts()
                                                 .FirstOrDefault(x => x.Id == model.Id);
                    if (oldProduct == null)
                        return RedirectToAction("Pity", "Error");

                    IProduct item = new Product();
                    ReadModel(model, item);
                    item.Id = (int) Session["CurrentProductId"];

                    if (uploadImage != null)
                        ReadImage(item, uploadImage);
                    else
                        item.Image = oldProduct.Image;

                    if (_dataManager.Products != null)
                        answer = UpdateProduct(item, model);
                    if (answer)
                        answer = _dataManager.ProductsCustomers.UpdateProdCastRelation((int) Session["UserId"], item.Id,
                                                                                       model.Count);
                }
            }
            return Redirect(answer);
        }

        public ActionResult Redirect(bool answer)
        {
            int result;
            if (answer)
                result = (int)Result.OperationSuccess;
            else
                result = (int)Result.Error;

            return RedirectToAction("Finality", "Error", new { id = result });
        }

        public enum Result
        {
            Error, AdditionSuccess, OperationSuccess
        }

        public ActionResult EditMenu()
        {
            return View();
        }

        private bool UpdateProduct(IProduct item, CreateProduct model)
        {
            try
            {
                int currentProductId = _dataManager.Products.UpdateProduct(item);
                if (currentProductId == -1) return false;
                if (_dataManager.ProductsCustomers.UpdateProdCastRelation(
                    (int)Session["UserId"], currentProductId, model.Count))
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
            // if the product was bought, it mustn't be deleted from Products
            // but must be deleted from CustomersProducts
            return _dataManager.Products.DeleteProduct(model.Id);
        }
        private void ReadModel(CreateProduct model, IProduct item)
        {
            item.Name = model.Name;
            item.Image = model.Image;
            item.Description = model.Description;
            item.Cost = model.Cost;
            item.IsAvailable = model.IsAvailable;
        }

        private void ReadImage(IProduct item, HttpPostedFileBase uploadImage)
        {
            // считываем переданный файл в массив байтов
            byte[] imageData;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            // установка массива байтов
            item.Image = imageData;
        }

        private readonly DataManager _dataManager;
        private CreateProduct _model;
    }
}
