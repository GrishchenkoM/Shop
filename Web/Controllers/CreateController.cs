using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class CreateController : Controller
    {
        public CreateController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CreateProduct model, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
                ReadImage(model, uploadImage);

            if (ModelState.IsValid && uploadImage != null)
            {
                IProduct item = new Product();
                ReadModel(model, item);

                return Redirect(CreateProduct(model, item));
            }
            return View(model);
        }

        private bool CreateProduct(CreateProduct model, IProduct item)
        {
            try
            {
                int currentProductId = _dataManager.Products.AddProduct(item);
                if (currentProductId == -1) return false;
                if (_dataManager.ProductsCustomers.AddProdCustRelation(
                    (int)Session["UserId"], currentProductId, model.Count))
                    return true;
                _dataManager.Products.DeleteProduct(item.Id);
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private void ReadImage(CreateProduct model, HttpPostedFileBase uploadImage)
        {
            // считываем переданный файл в массив байтов
            byte[] imageData;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            // установка массива байтов
            model.Image = imageData;
        }



        public ActionResult Redirect(bool answer)
        {
            int result;
            if (answer)
                result = (int)EditController.Result.OperationSuccess;
            else
                result = (int)EditController.Result.Error;

            return RedirectToAction("Finality", "Error", new { id = result });
        }

        private void ReadModel(CreateProduct model, IProduct item)
        {
            item.Name = model.Name;
            item.Image = model.Image;
            item.Description = model.Description;
            item.Cost = model.Cost;
            item.IsAvailable = model.IsAvailable;
        }

        public enum Result
        {
            Error, AdditionSuccess, OperationSuccess
        }

        private DataManager _dataManager;
    }
}
