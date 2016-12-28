using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankOrderSys.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace BankOrderSys.Controllers
{
    public class HomeController : Controller
    {
        List<string> period_list = new List<string>();
        List<string> week_days_l = new List<string>();
        List<string> type_servece_l = new List<string>();
        List<string> type_money_l = new List<string>();
        List<string> type_adress_l = new List<string>();
        List<string> type_city_l = new List<string>();
        List<string> type_get_money_l = new List<string>();
        List<string> type_order_l = new List<string>();
        List<string> bank_dep_l = new List<string>();

        ManagerDB db_man = new ManagerDB();

        int? cur_sort = 1;
        IEnumerable<OrderFormView> sort_list;

        [Authorize]
        public ActionResult Index()
        {
            return View(db_man.OrderList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index(int? sort)
        {
            if (sort != null)
            {
                if (cur_sort == sort)
                    sort_list = sort_list.Reverse();
                else
                {
                    switch (sort)
                    {
                        case 0:
                            sort_list = db_man.OrderList.OrderBy(obj => obj.date);
                            break;
                        case 1:
                            sort_list = db_man.OrderList.OrderBy(obj => obj.number);
                            break;
                        case 2:
                            sort_list = db_man.OrderList.OrderBy(obj => obj.status);
                            break;
                    }

                    cur_sort = sort;
                }
            }
            else
                sort_list = db_man.OrderList;

            return View(sort_list);
        }

        [Authorize]
        public ActionResult AddOrder()
        {
            return View("Order", new OrderFormView() );
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOrder(OrderFormView order)
        {
            ViewBag.edit_type = 0;

            db_man.Entry(order).State = EntityState.Added;
            try
            {
                db_man.SaveChanges();
            }
            catch { }

            //string tmp = "";
            //foreach (DbEntityValidationResult  a in db_man.GetValidationErrors())
                //foreach ( var b in a.ValidationErrors)
                    //tmp += b.ErrorMessage;

            return RedirectToAction("Index");//"Index"
            //return View(new OrderFormView());
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditOrder(int index)
        {
            OrderFormView tmp = db_man.OrderList.Find(index);
            ViewBag.edit_type = 1;
            return View("Order",tmp );
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOrder(OrderFormView order)
        {
            db_man.Entry(order).State = EntityState.Modified;
            try
            {
                db_man.SaveChanges();
            }
            catch { }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelOrder(OrderFormView order)
        {
            
            try
            {
                db_man.Entry(order).State = EntityState.Deleted;
                db_man.SaveChanges();
            }
            catch { }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddObj(OrderFormView order)
        {
            ObjectIncasation item = new ObjectIncasation();
            order.obj_list.Add(item);

            //ViewBag.edit_type = 0;

            return View("Order", order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelObj(OrderFormView order)
        {
            if (order.obj_list.Count != 0)
                order.obj_list.Remove(order.obj_list.Last());

            //ViewBag.edit_type = 0;

            return View("Order", order);
        }

        public HomeController()
        {
            sort_list = db_man.OrderList;

            ViewBag.week_days_l = week_days_l;
            ViewBag.period_list = period_list;
            ViewBag.type_servece_l = type_servece_l;
            ViewBag.type_money_l = type_money_l;
            ViewBag.type_adress_l = type_adress_l;
            ViewBag.type_city_l = type_city_l;
            ViewBag.type_get_money_l = type_get_money_l;
            ViewBag.type_order_l = type_order_l;
            ViewBag.bank_dep_l = bank_dep_l;

            type_order_l.Add("Тип 1");
            type_order_l.Add("Тип 2");
            type_order_l.Add("Тип 3");

            bank_dep_l.Add("Подразделение 1");
            bank_dep_l.Add("Подразделение 2");
            bank_dep_l.Add("Подразделение 3");

            type_get_money_l.Add("Способ 1");
            type_get_money_l.Add("Способ 2");
            type_get_money_l.Add("Способ 3");

            type_adress_l.Add("тип 1");
            type_adress_l.Add("тип 2");
            type_adress_l.Add("тип 3");

            type_city_l.Add("тип 1");
            type_city_l.Add("тип 2");
            type_city_l.Add("тип 3");

            type_money_l.Add("BYR");
            type_money_l.Add("RU");
            type_money_l.Add("EU");
            type_money_l.Add("US");

            type_servece_l.Add("Услуга 1");
            type_servece_l.Add("Услуга 2");
            type_servece_l.Add("Услуга 3");

            period_list.Add("Ежедневно");
            period_list.Add("Рабочие дни");
            period_list.Add("Через день");
            period_list.Add("День недели");
            period_list.Add("По заявке");
            period_list.Add("По звонку");

            week_days_l.Add("Понедельник");
            week_days_l.Add("Вторник");
            week_days_l.Add("Среда");
            week_days_l.Add("Четверг");
            week_days_l.Add("Пятница");
            week_days_l.Add("Суббота");
            week_days_l.Add("Воскресенье");
        }
    }
}