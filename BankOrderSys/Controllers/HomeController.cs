using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankOrderSys.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;
using System.Globalization;

namespace BankOrderSys.Controllers
{
    public class HomeController : Controller
    {
        List<string> bank_dep_l = new List<string>();
        ManagerDB db_man = new ManagerDB();

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
                case 1:
                    if (type != null)
                        db_man.AddRefItem((int)type);
                    break;
                case 2:
                    if (id != null)
                        db_man.DelRefItemById((int)id);
                    break;
                case 3:
                    if (id != null && value != null)
                        db_man.UpdateRefItem((int)id, value);
                    break;
            }
            return View(db_man.ReferenseBook);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Sign(int? id)
        {
            if(id != null)
                db_man.OrderSign((int)id);

            return RedirectToAction("Index");
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

            order_list = FilterOrders(order_list, type, status, date, date_start, date_end);

            if (sort != null)
                order_list = SortOrders( order_list, (Math.Abs( (int)sort) ) );

            var model = order_list.AsEnumerable();
            if ( sort != null && sort < 0)
                model = model.Reverse();

            return View(model);
        }

        private IQueryable<OrderFormView> FilterOrders(IQueryable<OrderFormView> list, string type, string status, int? date, string date_start, string date_end )
        {
            IQueryable<OrderFormView> n_list = list;

            if (date != null)
            {
                switch (date)
                {
                    case 0: break;//no date sort
                    case 1:
                        n_list = FilterDataPeriod(n_list, date_start, date_end);

                        break;//period
                    case 2:
                        n_list = FilterDataCurrentWeek(n_list);
                        break;//week
                    case 3:
                        n_list = n_list.Where(c => c.date.Month == DateTime.Now.Month);
                        break;//mounth
                }
            }

            if (status != null && status != "")
                n_list = n_list.Where(c => c.status == status);

            if (type != null && type != "")
                n_list = n_list.Where(c => c.type == type);

            return n_list;

        }
        private IQueryable<OrderFormView> FilterDataPeriod( IQueryable<OrderFormView> list, string date_start, string date_end )
        {
            if (date_start == "" || date_start == null)
                if (date_end == "" || date_end == null)
                    return list;

            DateTime d_s = DateTime.ParseExact(date_start, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime d_e = DateTime.ParseExact(date_end, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            return list.Where(c => (d_s <= c.date && d_e >= c.date));
        }
        private IQueryable<OrderFormView> FilterDataCurrentWeek(IQueryable<OrderFormView> list)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            int now_week = cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            //order_list = order_list.Where(c => ( cal.GetWeekOfYear(c.date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == now_week) );
            //Because LINQ not work
            var obj_tmp_l = list.ToList();
            var n_obj_l = new List<OrderFormView>();

            foreach (var obj in obj_tmp_l)
                if (cal.GetWeekOfYear(obj.date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == now_week)
                    n_obj_l.Add(obj);

            return n_obj_l.AsQueryable();
        }
        private IQueryable<OrderFormView> SortOrders(IQueryable<OrderFormView> list, int type)
        {
            IQueryable<OrderFormView> n_list = list;
            switch ( type )
            {
                case 1:
                    n_list = n_list.OrderBy(obj => obj.date);
                    break;
                case 2:
                    n_list = n_list.OrderBy(obj => obj.number);
                    break;
                case 3:
                    n_list = n_list.OrderBy(obj => obj.status);
                    break;
            }

            return n_list;
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
            if (order != null)
                db_man.AddNewOrder(order);

            return RedirectToAction("Index");//"Index"
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditOrder(int? index)
        {
            if(index == null)
                return RedirectToAction("Index");

            ViewBag.edit_type = 1;

            return View("Order", db_man.GetOrderById( (int)index ) );
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOrder(OrderFormView order)
        {
            if (order != null)
                db_man.UpdateOrder(order);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelOrder(OrderFormView order)
        {
            if(order != null)
                db_man.SetDeletedStateOrder(order);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddObj(OrderFormView order, int? edit_type)
        {
            if (order == null)
                return RedirectToAction("Index");

            ViewBag.edit_type = edit_type;

            return View( "Order", NewObjOfOrder(order) );
        }
        private OrderFormView NewObjOfOrder(OrderFormView order)
        {
            ObjectIncasation item = new ObjectIncasation();
            item.OrderFormViewId = order.id;
            item.OrderFormView = order;
            order.obj_list.Add(item);

            return order;
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelObj(OrderFormView order, int? edit_type, int? index)
        {
            if (order == null)
                return RedirectToAction("Index");

            if (order.obj_list.Count != 0 && index != null )
                order.obj_list.Remove( order.obj_list.ElementAt( (int)index )) ;

            ViewBag.edit_type = edit_type;

            return View("Order", order);
        }

        public HomeController()
        {
            ViewBag.admin = false;

            ViewBag.ref_book = db_man.ReferenseBook.AsEnumerable();
            ViewBag.bank_dep_l = bank_dep_l;

            bank_dep_l.Add("Подразделение 1");
            bank_dep_l.Add("Подразделение 2");
            bank_dep_l.Add("Подразделение 3");
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