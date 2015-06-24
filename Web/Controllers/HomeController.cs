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
            if (id != -1)
                _currentCustomerId = id;
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
            {
                // считываем переданный файл в массив байтов
                byte[] imageData;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                // установка массива байтов
                model.Image = imageData;
            }
            if (ModelState.IsValid && uploadImage != null)
            {
                IProduct item = new Product
                    {
                        Name = model.Name,
                        Image = model.Image,
                        Description = model.Description,
                        Cost = model.Cost,
                        IsAvailable = model.IsAvailable
                    };

                int result;
                _dataManager.Products.AddProduct(item);
                var curProduct = _dataManager.Products.GetProducts()
                                             .FirstOrDefault(
                                                 x =>
                                                 x.Name == item.Name 
                                                 && x.Image == item.Image 
                                                 && x.Cost == item.Cost);
                _dataManager.ProductsCustomers.AddRelation(, curProduct.Id);

                if (!)
                    result = (int)Result.Error;
                else
                    result = (int)Result.AdditionSuccess;

                return RedirectToAction("Finality", new { id = result });
            }
            return View(model);
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
        
        public ActionResult Edit(int id)
        {
            var product = _dataManager.Products.GetProducts()
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
                return RedirectToAction("Pity");

            _model = new CreateProduct();
            _model.Id = id;
            _model.Name = product.Name;
            _model.Cost = product.Cost;
            _model.Image = product.Image;
            _model.Description = product.Description;
            _model.IsAvailable = product.IsAvailable;
            return View(_model);
        }

        [HttpPost]
        public ActionResult Edit(CreateProduct model, FormCollection form, HttpPostedFileBase uploadImage)
        {
            bool answer = false;
            int result;

            if (form.GetKey(6) == "delete")
            {
                if (_dataManager.Products != null)
                    answer = _dataManager.Products.DeleteProduct(model.Id);
            }
            else
            {
                if (!ModelState.IsValid) return View(model);
                var oldProduct = _dataManager.Products.GetProducts()
                        .FirstOrDefault(x => x.Id == model.Id);
                if (oldProduct == null)
                    return RedirectToAction("Pity");

                IProduct item = new Product
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Cost = model.Cost,
                        IsAvailable = model.IsAvailable
                    };

                if (uploadImage != null)
                {
                    // считываем переданный файл в массив байтов
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    // установка массива байтов
                    item.Image = imageData;
                }
                else
                {
                    item.Image = oldProduct.Image;
                }

                //item.Id = oldProduct.Id;

                if (_dataManager.Products != null)
                    answer = _dataManager.Products.UpdateProduct(item);
            }

            if (!answer)
                result = (int) Result.Error;
            else
                result = (int) Result.OperationSuccess;

            return RedirectToAction("Finality", new {id = result});
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

        private readonly DataManager _dataManager;
        private int _currentCustomerId;
        private CreateProduct _model;
        private enum Result
        {
            Error, AdditionSuccess, OperationSuccess
        }
    }
}
