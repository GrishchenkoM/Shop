using System;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic;
using Web.Models;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class AccountController : Controller
    {
        #region public

        public AccountController(DataManager manager)
        {
            _dataManager = manager;
        }

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dataManager.Provider.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неудачная попытка входа на сайт");
            }
            return View(model);
        }

        #endregion

        #region LogIn

        public ActionResult LogIn(int id = -1)
        {
            if (id != -1)
                Session.Add("CurrentProductId", id);
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dataManager.Provider.ValidateUser(model.UserName, model.Password))
                {
                    var customer = _dataManager.Customers.GetCustomerByName(model.UserName);
                    var tempId = -1;
                    if (customer != null)
                        tempId = customer.Id;
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    Session["UserId"] = tempId;
                    
                    if (Session["CurrentProductId"] != null)
                        return RedirectToAction("Index", "Purchase", new { id = (int)Session["CurrentProductId"] });
                    return RedirectToAction("Index", "Home", new { id = tempId});
                }
                ModelState.AddModelError("", "Неудачная попытка входа на сайт");
            }
            return View(model);
        }

        #endregion

        #region Register
        
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = _dataManager
                    .Provider.CreateUser(
                        model.UserName,
                        model.Password,
                        model.Email,
                        model.FirstName,
                        model.LastName);
                if (status == MembershipCreateStatus.Success)
                    return View("Success");
                ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
            }
            return View(model);
        }
        
        #endregion
        
        #region LogOut

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["UserId"] = null;
            Session["HomeViewModel"] = null;
            Session["CurrentProductId"] = null;
            return RedirectToAction("Index", "Home");
        }

        #endregion

        public string GetMembershipCreateStatusResultText(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.DuplicateEmail)
                return "Пользователь с таким email адресом уже существует";
            if (status == MembershipCreateStatus.DuplicateUserName)
                return "Пользователь с таким именем уже существует";
            return "Неизвестная ошибка";
        }

        #endregion

        #region private

        private readonly DataManager _dataManager;

        #endregion
    }
}
