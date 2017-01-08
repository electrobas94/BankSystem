using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BankOrderSys.Models
{

    public class ManagerDB:DbContext
    {
        public ManagerDB()
            : base("DefaultConnection")
        {
        }
        public DbSet<OrderFormView> OrderList { get; set; }
        public DbSet<ObjectIncasation> ObjectList { get; set; }
        public DbSet<ItemList> ReferenseBook { get; set; }

        public void AddRefItem(int type)
        {
            ItemList tmp = new ItemList();
            tmp.title = "Новый";
            tmp.type = type;

            ReferenseBook.Add(tmp);
            SaveChanges();
        }
        public void DelRefItemById(int id)
        {
            ItemList del_item = ReferenseBook.Find(id);
            Entry(del_item).State = EntityState.Deleted;
            SaveChanges();
        }
        public void UpdateRefItem(int id, string value)
        {
            ItemList mod_item = ReferenseBook.Find(id);
            mod_item.title = value;
            SaveChanges();
        }
        public void OrderSign(int id)
        {
            OrderFormView tmp = OrderList.Find(id);
            tmp.status = "Подписан";

            Entry(tmp).State = EntityState.Modified;
            SaveChanges();
        }

        public void AddNewOrder(OrderFormView order)
        {
            OrderList.Add(order);
            SaveChanges();
        }

        public OrderFormView GetOrderById(int id)
        {
            OrderFormView tmp = OrderList.Find(id);

            if (tmp != null)
            {
                var obj_list = ObjectList.Where(c => c.OrderFormViewId == id);

                if (obj_list != null)
                    tmp.obj_list = obj_list.ToList();
            }

            return tmp;
        }

        public void UpdateOrder(OrderFormView order)
        {
            var list = ObjectList.Where(c => c.OrderFormViewId == order.id).AsEnumerable();

            if (list != null)
                foreach (var obj in list)
                    Entry(obj).State = EntityState.Deleted;

            foreach (var obj in order.obj_list)
                ObjectList.Add(obj);

            Entry(order).State = EntityState.Modified;
            SaveChanges();
        }

        public void SetDeletedStateOrder(OrderFormView order)
        {
            order.status = "Удаленная";
            Entry(order).State = EntityState.Modified;
            SaveChanges();
        }
    }
}