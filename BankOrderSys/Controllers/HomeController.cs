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
        List<string> bank_dep_l     = new List<string>();

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
                        order_list = order_list.OrderBy(obj => obj.date);
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

            db_man.OrderList.Add(order);
            db_man.SaveChanges();

            return RedirectToAction("Index");//"Index"
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditOrder(int index)
        {
            OrderFormView tmp = db_man.OrderList.Find(index);

            var obj_list = db_man.ObjectList.Where(c => c.OrderFormViewId == index );

            if (obj_list != null)
                tmp.obj_list = obj_list.ToList();

            ViewBag.edit_type = 1;
            return View("Order", tmp );
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOrder(OrderFormView order)
        {
            var list = db_man.ObjectList.Where(c => c.OrderFormViewId == order.id).AsEnumerable();

            if (list != null)
                foreach (var obj in list)
                    db_man.Entry(obj).State = EntityState.Deleted;

            foreach (var obj in order.obj_list)
                db_man.ObjectList.Add(obj);

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
            item.OrderFormViewId = order.id;
            item.OrderFormView = order;
            order.obj_list.Add(item);

            ViewBag.edit_type = edit_type;

            return View("Order", order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DelObj(OrderFormView order, int? edit_type, int? index)
        {
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