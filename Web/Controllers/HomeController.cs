using System;
using System.Collections.Generic;
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
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id = -1)
        {
            if (id == -1) 
                return RedirectToAction("LogIn", "Account");
            Session.Add("UserId", id);
            return View();
        }

        public ActionResult EditMenu()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateProduct model, HttpPostedFileBase uploadImage)
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
        
        public ActionResult Edit(int id)
        {
            var product = _dataManager.Products.GetProducts()
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
                return RedirectToAction("Pity");

            Session.Add("ProductId", id);
            _model = new CreateProduct
            {
                Name = product.Name,
                Cost = product.Cost,
                Image = product.Image,
                Description = product.Description,
                IsAvailable = product.IsAvailable
            };

            return View(_model);
        }
        [HttpPost]
        public ActionResult Edit(CreateProduct model, FormCollection form, HttpPostedFileBase uploadImage)
        {
            bool answer = false;
            int result;

            if (form.GetKey(form.Keys.Count - 1) == "delete") // if input 'delete' stands always in the end
                if (_dataManager.Products != null)
                    answer = DeleteProduct(model);
            else
            {
                if (!ModelState.IsValid) return View(model);
                var oldProduct = _dataManager.Products.GetProducts()
                        .FirstOrDefault(x => x.Id == model.Id);
                if (oldProduct == null)
                    return RedirectToAction("Pity");

                IProduct item = new Product();
                ReadModel(model, item);
                item.Id = (int)Session["ProductId"];

                if (uploadImage != null)
                    ReadImage(item, uploadImage);
                else
                    item.Image = oldProduct.Image;
                
                if (_dataManager.Products != null)
                    answer = UpdateProduct(item, model);
            }

            return Redirect(answer);
        }
        
        public ActionResult Find()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Find(string name)
        {
            IEnumerable<IProduct> model;
            if (name == "")
                model = _dataManager.Products.GetProducts();
            else
                model = _dataManager.Products.GetProducts().Where(
                    x => x.Name.ToLowerInvariant().StartsWith(name.ToLowerInvariant()));
            
            return View(model);
        }
        
        public ActionResult Log()
        {
            return View();
        }

        public ActionResult Finality(int id)
        {
            switch (id)
            {
                case 0:
                    ViewBag.Message = "Произошла непредвиденная ошибка при добавдении товара";
                    break;
                case 1:
                    ViewBag.Message = "Товар добавлен!";
                    break;
                case 2: ViewBag.Message = "Готово!";
                    break;
            }
            return View();
        }

        public ActionResult Pity()
        {
            return View();
        }

        public int Id { get; private set; }


        private bool CreateProduct(CreateProduct model, IProduct item)
        {
            try
            {
                int currentProductId = _dataManager.Products.AddProduct(item);
                if (currentProductId == -1) return false;
                if (_dataManager.ProductsCustomers.AddProdCustRelation(
                    (int)Session["UserId"], currentProductId, model.Count))
                    return true;
                bool result = _dataManager.Products.DeleteProduct(item.Id);
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private bool UpdateProduct(IProduct item, CreateProduct model)
        {
            try
            {
                int currentProductId = _dataManager.Products.UpdateProduct(item);
                if (currentProductId == -1) return false;
                if (_dataManager.ProductsCustomers.UpdateProdCastRelation(
                    (int) Session["UserId"], currentProductId, model.Count))
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
        private void ReadImage(CreateProduct model, HttpPostedFileBase uploadImage)
        {
            // считываем переданный файл в массив байтов
            byte[] imageData;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            // установка массива байтов
            model.Image = imageData;
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
        private ActionResult Redirect(bool answer)
        {
            int result;
            if (answer)
                result = (int)Result.OperationSuccess;
            else
                result = (int)Result.Error;

            return RedirectToAction("Finality", new { id = result });
        }

        private readonly DataManager _dataManager;
        private int _currentCustomerId;
        private CreateProduct _model;
        private enum Result
        {
            Error, AdditionSuccess, OperationSuccess
        }
    }
}
