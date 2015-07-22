using System;
using System.Web.Mvc;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class AboutSiteController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
                ViewBag.CustomerId = (int) Session["UserId"];
            return View();
        }

    }
}
