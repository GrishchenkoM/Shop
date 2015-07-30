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
    public class CreateController : Controller
    {
        #region public

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
                ReadImage(uploadImage, model);

            if (ModelState.IsValid && uploadImage != null)
            {
                IProduct item = new Product();
                ReadModel(model, item);

                return Redirect(Auxiliary.Actions.Create, CreateProduct(model, item)
                        ? Auxiliary.Result.OperationSuccess : Auxiliary.Result.Error);
            }
            return View(model);
        }

        public ActionResult Redirect(Auxiliary.Actions action, Auxiliary.Result r)
        {
            return RedirectToAction("Finality", "Error", new { reaction = action, result = r });
        }
        
        #endregion

        #region private

        private void ReadModel(CreateProduct model, IProduct item)
        {
            item.Name = model.Name;
            item.Image = model.Image;
            item.Description = model.Description;
            item.Cost = model.Cost;
            item.IsAvailable = model.IsAvailable;
        }

        private void ReadImage(HttpPostedFileBase uploadImage, CreateProduct model)
        {
            Auxiliary.ReadImage(uploadImage, model);
        }

        private bool CreateProduct(CreateProduct model, IProduct item)
        {
            try
            {
                var currentProductId = _dataManager.Products.AddProduct(item);
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
        
        private readonly DataManager _dataManager;

        #endregion
    }
}
