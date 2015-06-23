using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(DataManager manager)
        {
            _dataManager = manager;
        }

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

        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dataManager.Provider.ValidateUser(model.UserName, model.Password))
                {
                    var customer = _dataManager.Customers.GetCustomers()
                                                .FirstOrDefault(x => x.UserName == model.UserName);
                    int tempId = -1;
                    if (customer != null)
                        tempId = customer.Id;
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home", new { id = tempId});
                }
                ModelState.AddModelError("", "Неудачная попытка входа на сайт");
            }
            return View(model);
        }


        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status = _dataManager
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
        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public string GetMembershipCreateStatusResultText(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.DuplicateEmail)
                return "Пользователь с таким email адресом уже существует";
            if (status == MembershipCreateStatus.DuplicateUserName)
                return "Пользователь с таким именем уже существует";
            return "Неизвестная ошибка";
        }


        private readonly DataManager _dataManager;
    }
}
