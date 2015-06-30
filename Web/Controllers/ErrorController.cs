using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Finality(int id)
        {
            switch (id)
            {
                case 0:
                    ViewBag.Message = "Произошла непредвиденная ошибка при добавдении товара";
                    break;
                case 1:
                    ViewBag.Message = "Товар добавлен!";
                    break;
                case 2: ViewBag.Message = "Готово!";
                    break;
            }
            return View();
        }

        public ActionResult Pity()
        {
            return View();
        }
    }
}
