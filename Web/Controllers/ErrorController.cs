using System;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class ErrorController : Controller
    {
        #region public

        public ActionResult Finality(Auxiliary.Actions reaction, Auxiliary.Result result)
        {
            string answer = null;

            CreateResultAnswer(ref answer, result);
            CreateActionsAnswer(ref answer, reaction);
            CreateResultAnswer(ref answer, result);

            ViewBag.Message = answer;
            return View();
        }
        
        public ActionResult Pity()
        {
            return View();
        }
        
        #endregion

        #region private

        private void CreateActionsAnswer(ref string answer, Auxiliary.Actions action)
        {
            switch (action)
            {
                case Auxiliary.Actions.Create:
                    answer += " Операция создания товара ";
                    break;
                case Auxiliary.Actions.Update:
                    answer += " Операция обновления товара ";
                    break;
                case Auxiliary.Actions.Delete:
                    answer += " Операция удаления товара ";
                    break;
                case Auxiliary.Actions.Purchase:
                    answer += " Покупка товара "; 
                    break;
                default:
                    answer += " Операция "; 
                    break;
            }
        }

        private void CreateResultAnswer(ref string answer, Auxiliary.Result result)
        {
            if (answer == null)
                CreateFirstPartOfAnswer(out answer, result);
            else
                CreateSecondPartOfAnswer(ref answer, result);
        }

        private void CreateSecondPartOfAnswer(ref string answer, Auxiliary.Result result)
        {
            switch (result)
            {
                case Auxiliary.Result.Error:
                    answer += "не выполнена! Произошла непредвиденная ошибка! ";
                    break;
                case Auxiliary.Result.OperationSuccess:
                    answer += "прошла успешно! Поздравляем!";
                    break;
                default:
                    answer += "";
                    break;
            }
        }

        private void CreateFirstPartOfAnswer(out string answer, Auxiliary.Result result)
        {
            switch (result)
            {
                case Auxiliary.Result.Error:
                    answer = "Ошибка!";
                    break;
                case Auxiliary.Result.OperationSuccess:
                    answer = "Готово!";
                    break;
                default:
                    answer = "";
                    break;
            }
        }
        
        #endregion
    }
}
