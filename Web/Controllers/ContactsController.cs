﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Pity")]
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
                ViewBag.CustomerId = (int) Session["UserId"];
            return View();
        }
    }
}