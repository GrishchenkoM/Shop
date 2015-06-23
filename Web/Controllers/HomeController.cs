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
                if (!_dataManager.Products.AddProduct(item))
                    result = (int)Result.Error;
                else
                    result = (int)Result.Success;

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id">Id товара</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Find(int id = -1)
        //{
        //    IEnumerable<IProduct> model;
        //    if (id == -1)
        //        model = _dataManager.Products.GetProducts().Where(x => x.Id == id); // связать с customerId
        //    return RedirectToAction("Edit", "Home", new { id = 0 }); // !!!!!!! id = 0!!!
        //}

        public ActionResult Edit(IProduct id)
        {
            var model = new CreateProduct
                {
                    Name = id.Name,
                    Cost = id.Cost,
                    Image = id.Image,
                    Description = id.Description,
                    IsAvailable = id.IsAvailable
                };
            return View(model);
        }

        public ActionResult Log()
        {
            return View();
        }

        public ActionResult Finality(int id)
        {
            ViewBag.Message = id != 0 ? "Товар добавлен!" : "Произошла непредвиденная ошибка при добавдении товара";
            return View();
        }

        public int Id { get; private set; }

        private readonly DataManager _dataManager;
        private int _currentCustomerId;
        private enum Result
        {
            Error,Success
        }
    }
}
