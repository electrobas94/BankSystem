using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankOrderSys.Models
{
    [Table("dfcz")]
    public class ObjectIncasation
    {
        [Key]
        public int id_obj { get; set; }
        public DateTime time { get; set; }
        public string type_get_money { get; set; }
        public string period { get; set; }
        public string week_day { get; set; }
        public string count_money { get; set; }
        public float num_bank_score { get; set; }
        public string code_money { get; set; }
        public string boss_object { get; set; }
        public string start_data { get; set; }
        public string type_adress { get; set; }
        public string type_city { get; set; }
        public string title_city { get; set; }
        public string title_street { get; set; }
        public string num_build { get; set; }
        public string title_part { get; set; }
        public string type_order { get; set; }
        public int? OrderFormViewId { get; set; }
        public OrderFormView OrderFormView { get; set; }

    }

    public class OrderFormView
    {
        [Key]
        public int id { get; set; }
        public string number { get; set; }
        public string type { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public string bank_dep { get; set; }
        public string INN_KIO { get; set; }
        public string kpp { get; set; }
        public string title_org { get; set; }
        public string OGRN { get; set; }
        public string name_worker { get; set; }
        public string phone_worker { get; set; }
        public string num_score_push { get; set; }
        public string BIK { get; set; }
        public string num_bank_score { get; set; }
        public string title_bank { get; set; }
        public string other_rec_bank { get; set; }
        public string SWIFT { get; set; }

        public ICollection<ObjectIncasation> obj_list { get; set; }

        public OrderFormView()
        {
            obj_list = new List<ObjectIncasation>();
        }
    }
}