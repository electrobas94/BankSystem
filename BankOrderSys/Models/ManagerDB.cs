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

    }
}