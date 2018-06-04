using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Mark
{
    public class ViewVisitorModel
    {
        public string Id { get; set; }

        [Display(Name = "Номер группы")]
        public string GroupId { get; set; }
        //[Display(Name = "Дата прибытия")]
        //public string DateArrived { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }
        //[Display(Name = "Туроператор")]
        //public string Operator { get; set; }
        [Display(Name = "Отметка о прибытии")]
        public bool Arrived { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }
    }
}