using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class CustomerController : Controller
    {
        public CustomerController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index(int id)
        {
            if (Session["UserId"] == null) return RedirectToAction("LogIn", "Account");

            ViewBag.CustomerId = (int) Session["UserId"];
            var customer = _dataManager.Customers.GetCustomers().FirstOrDefault(x => x.Id == id);
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

        private DataManager _dataManager;
    }
}
