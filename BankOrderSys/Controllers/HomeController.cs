using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankOrderSys.Models;

namespace BankOrderSys.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult AddOrder()
        {
            return View();
        }
        public ActionResult ShowOrder()
        {
            return View();
        }

        public ActionResult EditOrder()
        {
            return View(new OrderFormView());
        }
    }
}