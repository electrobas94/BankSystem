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
        public ActionResult AddOrder(OrderFormView order)
        {
            if( order == null)
                return View(new OrderFormView() );
            else
                return View(order);
        }
        public ActionResult ShowOrder()
        {
            return View();
        }

        public ActionResult EditOrder()
        {
            return View(new OrderFormView());
        }

        [HttpPost]
        public ActionResult AddObj(OrderFormView order)
        {
            ObjectIncasation item = new ObjectIncasation();
            order.obj_list.Add(item);

            return View("AddOrder", order);
        }

        [HttpPost]
        public ActionResult DelObj(OrderFormView order)
        {
            if (order.obj_list.Count != 0)
                order.obj_list.Remove(order.obj_list.Last());

            return View("AddOrder", order);
        }
    }
}