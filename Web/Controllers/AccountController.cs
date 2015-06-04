using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;

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


        private DataManager _dataManager;
    }
}
