using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Authorize, HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class ProfileController : Controller
    {
        public ProfileController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null || (int) Session["UserId"] == -1)
                return RedirectToAction("LogIn", "Account");
            IEnumerable<ICustomer> customers = _dataManager.Customers.GetCustomers();
            ICustomer customer = customers.FirstOrDefault(x => x.Id == (int) Session["UserId"]);
            if (customer == null)
                return RedirectToAction("Pity", "Error");

            var model = new RegisterViewModel
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    ConfirmEmail = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Password = customer.Password,
                    ConfirmPassword = customer.Password,
                    Sex = string.IsNullOrEmpty(customer.Sex) ? "" : customer.Sex,
                    Phone = string.IsNullOrEmpty(customer.Phone) ? "" : customer.Phone,
                    Address = string.IsNullOrEmpty(customer.Address) ? "" : customer.Address
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if ((int) Session["UserId"] > 0)
                {
                    ICustomer customer = new Customer
                        {
                            Id = (int) Session["UserId"],
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Password = model.Password,
                            Sex = string.IsNullOrEmpty(model.Sex) ? "" : model.Sex,
                            Phone = string.IsNullOrEmpty(model.Phone) ? "" : model.Phone,
                            Address = string.IsNullOrEmpty(model.Address) ? "" : model.Address
                        };

                    if (_dataManager.Customers.UpdateCustomer(customer) == 1)
                        ViewBag.Message = "Профиль сохранен!";
                    else
                        ViewBag.Message = "Не удалось сохранить профиль!";
                }
                else
                    return RedirectToAction("LogIn", "Account");
            }
            return View(model);
        }

        private readonly DataManager _dataManager;
    }
}
