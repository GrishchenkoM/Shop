using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class SearchController : Controller
    {

        public SearchController(DataManager dataManager)
        {
            _manager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string keyword)
        {
            var model =
                (IEnumerable<Product>)
                _manager.Products.GetProducts()
                        .Where(x => x.Name.ToLowerInvariant().StartsWith(keyword.ToLowerInvariant()));
            return View(model);
        }

        private readonly DataManager _manager;
    }
}
