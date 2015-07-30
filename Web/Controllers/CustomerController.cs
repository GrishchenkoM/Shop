using System;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class CustomerController : Controller
    {
        #region public

        public CustomerController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id)
        {
            if (Session["UserId"] == null) return RedirectToAction("LogIn", "Account");

            ViewBag.CustomerId = (int) Session["UserId"];
            var customer = _dataManager.Customers.GetCustomerById(id);
            var model = new Customer
                {
                    UserName = customer.UserName,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CreatedDate = customer.CreatedDate,
                    Email = customer.Email,
                    Sex = customer.Sex,
                    Phone = customer.Phone,
                    Address = customer.Address
                };
            return View(model);
        }
        
        #endregion

        #region private

        private readonly DataManager _dataManager;
        
        #endregion
    }
}
