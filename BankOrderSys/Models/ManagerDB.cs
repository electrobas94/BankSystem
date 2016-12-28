using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BankOrderSys.Models
{
    public class ManagerDB:DbContext
    {
        public DbSet<OrderFormView> OrderList { get; set; }
    }
}