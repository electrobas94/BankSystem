using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankOrderSys.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;

namespace BankOrderSys.Controllers
{
    public class HomeController : Controller
    {
        List<string> period_list    = new List<string>();
        List<string> week_days_l    = new List<string>();
        List<string> type_servece_l = new List<string>();
        List<string> type_money_l   = new List<string>();
        List<string> type_adress_l  = new List<string>();
        List<string> type_city_l    = new List<string>();
        List<string> type_get_money_l = new List<string>();
        List<string> type_order_l   = new List<string>();
        List<string> bank_dep_l     = new List<string>();

        ManagerDB db_man = new ManagerDB();

        int? cur_sort = 1;
        IEnumerable<OrderFormView> sort_list;

        [Authorize]
        public ActionResult Lists()
        {
            return View(db_man.ReferenseBook);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Lists(int? act, int? type, int? id, string value)
        {
                switch(act)
                {
                    //add new
                    case 1:
                        ItemList tmp = new ItemList();
                        tmp.title = "Новый";
                        tmp.type = (int)type;
                        db_man.ReferenseBook.Add(tmp);
                        db_man.SaveChanges();
                        break;

                    //del item
                    case 2:
                        if (id == null)
                            break;
                        ItemList del_item = db_man.ReferenseBook.Find((int)id);
                        db_man.Entry(del_item).State = EntityState.Deleted;
                        db_man.SaveChanges();
                        break;

                    //modify item
                    case 3:
                        if (id == null || value == null)
                            break;
                        ItemList mod_item = db_man.ReferenseBook.Find((int)id);
                        mod_item.title = value;
                        db_man.SaveChanges();
                        break;
            }
            return View(db_man.ReferenseBook);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Sign(int id)
        {
            OrderFormView tmp = db_man.OrderList.Find(id);

            tmp.status = "Подписан";

            db_man.Entry(tmp).State = EntityState.Modified;
            db_man.SaveChanges();

            return RedirectToAction("Index");//"Index"
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(db_man.OrderList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index(int? sort, int? admin, string type, string status, int? date, string date_start, string date_end)
        {
            if (admin == 1)
                ViewBag.admin = true;
            else if(admin == 0)
                ViewBag.admin = false;

            ViewBag.sort        = sort;
            ViewBag.type        = type;
            ViewBag.status      = status;
            ViewBag.date        = date;
            ViewBag.date_start  = date_start;
            ViewBag.date_end    = date_end;

            var order_list = db_man.OrderList.AsQueryable();

            if(date != null)
            {
                switch(date)
                {
                    case 0:break;//no date sort
                    case 1:
                        if (date_start == "" || date_start == null)
                            break;
                        if (date_end == "" || date_end == null)
                            break;

                        date_start.Split('.');
                        DateTime d_s = DateTime.ParseExact(date_start, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime d_e = DateTime.ParseExact(date_end, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                        order_list = order_list.Where(c => ( d_s <= Convert.ToDateTime(c.date) && d_e >= Convert.ToDateTime(c.date) ));

                        break;//period
                    case 2:
                        order_list = order_list.Where(c => Convert.ToDateTime(c.date).DayOfWeek == DateTime.Today.DayOfWeek);
                        break;//week
                    case 3:
                        order_list = order_list.Where(c => Convert.ToDateTime( c.date ).Month == DateTime.Today.Month );
                        break;//mounth
                }
            }

            if(status != null && status != "")
            {
                order_list = order_list.Where(c => c.status == status);
            }

            if (type != null && type != "")
            {
                order_list = order_list.Where(c => c.type == type);
            }

            if(sort != null)
            {
                switch (Math.Abs((int)sort))
                {
                    case 1:
                        order_list = order_list.OrderBy(obj => obj.date);//db_man.OrderList.AsEnumerable().OrderBy(obj => obj.date);
                        break;
                    case 2:
                        order_list = order_list.OrderBy(obj => obj.number);
                        break;
                    case 3:
                        order_list = order_list.OrderBy(obj => obj.status);
                        break;
                }

            }

            var model = order_list.AsEnumerable();
            if ( sort != null && sort < 0)
                model = model.Reverse();

            return View(model);
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

            //db_man.Entry(order).State = EntityState.Added;
            db_man.OrderList.Add(order);
            db_man.SaveChanges();

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
            OrderFormView tmp = null;// db_man.OrderList.Find(index);

            foreach(var order in db_man.OrderList)
            {
                if(order.id == index)
                    tmp = order;
            }
            //db_man.OrderList.Where( (OrderFormViewId, index) => OrderFormViewId == index );

            //          ¯\_(ツ)_/¯
            var obj_list = db_man.ObjectList.Where(c => c.OrderFormViewId == index );
            if (obj_list != null)
            {
                //var ob = obj_list.AsEnumerable();
                tmp.obj_list = obj_list.ToList();

                /*
                //var obj_list = db_man.ObjectList.ToList();
                foreach (ObjectIncasation obj_inc in ob)
                {
                    if (obj_inc.OrderFormViewId == index)
                        tmp.obj_list.Add(obj_inc);
                }
                */
            }

            ViewBag.edit_type = 1;
            return View("Order", tmp );
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOrder(OrderFormView order)
        {

            db_man.Entry(order).State = EntityState.Modified;
            db_man.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelOrder(OrderFormView order)
        {

            order.status = "Удаленная";
            db_man.Entry(order).State = EntityState.Modified;
            db_man.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddObj(OrderFormView order, int? edit_type)
        {
            ObjectIncasation item = new ObjectIncasation();
            order.obj_list.Add(item);

            ViewBag.edit_type = edit_type;

            return View("Order", order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelObj(OrderFormView order, int? edit_type)
        {
            if (order.obj_list.Count != 0)
                order.obj_list.Remove(order.obj_list.Last());

            ViewBag.edit_type = edit_type;

            return View("Order", order);
        }

        public HomeController()
        {
            sort_list = db_man.OrderList;
            ViewBag.admin = false;

            ViewBag.ref_book = db_man.ReferenseBook;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db_man.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}