using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities.Interfaces;

namespace Web.Controllers
{
    public class FindController : Controller
    {
        public FindController(DataManager manager)
        {
            _dataManager = manager;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string name)
        {
            IEnumerable<IProduct> model;
            if (name == "")
                model = _dataManager.Products.GetProducts();
            else
                model = _dataManager.Products.GetProducts().Where(
                    x => x.Name.ToLowerInvariant().StartsWith(name.ToLowerInvariant()));

            return View(model);
        }

        private readonly DataManager _dataManager;
    }
}
