using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;

namespace Web.Controllers
{
    public class TextController : Controller
    {
        private DataManager _dataManager;

        public TextController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
