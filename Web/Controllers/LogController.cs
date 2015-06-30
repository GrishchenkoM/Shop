using System.Web.Mvc;
using BusinessLogic;

namespace Web.Controllers
{
    public class LogController : Controller
    {
        public LogController(DataManager manager)
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
