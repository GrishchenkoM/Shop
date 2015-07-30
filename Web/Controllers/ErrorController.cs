using System;
using System.Web.Mvc;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class ErrorController : Controller
    {
        #region public

        public ActionResult Finality(int id)
        {
            switch (id)
            {
                case 0:
                    ViewBag.Message = "Произошла непредвиденная ошибка!";
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
        
        #endregion
    }
}
