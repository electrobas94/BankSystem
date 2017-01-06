using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankOrderSys.Models
{
    public class ItemList
    {
        [Key]
        public int id { get; set; }
        public int type { get; set; }
        public string title { get; set; }
    }
}