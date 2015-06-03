using System.Web.Mvc;
using BusinessLogic;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        

        //public HomeController(DataManager dataManager)
        //{
        //    _dataManager = dataManager;
        //}

        public ActionResult Index()
        {
            ViewBag.CustomerId = 1;
            return View();
        }

    //    [HttpPost]
    //    public ActionResult Index(DataManager dataManager)
    //    {
            
    //        ViewBag.CustomerId = 1;
    //        return View(_dataManager.Customers.GetCustomerById(1));
    //    }
    }
}
