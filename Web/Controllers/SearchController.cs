using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic;
using Domain.Entities;

namespace Web.Controllers
{
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
            IEnumerable<Product> model =
                (IEnumerable<Product>)
                _manager.Products.GetProducts()
                        .Where(x => x.Name.ToLowerInvariant().StartsWith(keyword.ToLowerInvariant()));
            return View(model);
        }

        private DataManager _manager;
    }
}
