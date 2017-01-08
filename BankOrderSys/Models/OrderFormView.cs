using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankOrderSys.Models
{
    public class ObjectIncasation
    {
        [Key]
        public int id_obj { get; set; }
        public string time { get; set; }
        public string work_time_start { get; set; }
        public string work_time_end { get; set; }
        public string saturd_time_start { get; set; }
        public string saturd_time_end { get; set; }
        public string sund_time_start { get; set; }
        public string sund_time_end { get; set; }
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
        public string user_name { get; set; }
        public string number { get; set; }
        //[Required]
        [Display(Name = "Тип заявки")]
        public string type { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        //[Required]
        [Display(Name = "Подразделение банка")]
        public string bank_dep { get; set; }
        //[Required]
        [Display(Name = "ИИН/КИО")]
        public string INN_KIO { get; set; }
        [Display(Name = "КПП")]
        public string kpp { get; set; }
        //[Required]
        //[StringLength(250, ErrorMessage = "{0} должен быть не меньше {2} символов.", MinimumLength = 0)]
        [Display(Name = "Полное наименование организации")]
        public string title_org { get; set; }
        [Display(Name = "ОГРН")]
        public string OGRN { get; set; }
        //[Required]
        [Display(Name = "Фамилия Имя Отчество")]
        public string name_worker { get; set; }
        [Required]
        [Display(Name = "Номер телефона, факса уполномоченного сотрудника")]
        public string phone_worker { get; set; }
        //[Required]
        [Display(Name = "Номер счета зачисления")]
        public string num_score_push { get; set; }
        //[Required]
        [Display(Name = "БИК")]
        public string BIK { get; set; }
        //[Required]
        [Display(Name = "Номер корр. счета банка")]
        public string num_bank_score { get; set; }
        //[Required]
        [Display(Name = "Наименование банка")]
        public string title_bank { get; set; }
        [Display(Name = "Иные реквизиты банка зачисления валюты")]
        public string other_rec_bank { get; set; }
        //[Required]
        [Display(Name = "SWIFT")]
        public string SWIFT { get; set; }

        public ICollection<ObjectIncasation> obj_list { get; set; }

        public OrderFormView()
        {
            obj_list = new List<ObjectIncasation>();
        }
    }
}