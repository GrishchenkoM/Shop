using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CustomerId = 1;
            return View();
        }
    }
}
