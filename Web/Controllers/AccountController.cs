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


        private DataManager _dataManager;
    }
}
