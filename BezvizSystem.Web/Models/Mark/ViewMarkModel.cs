using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Mark
{
    public class ViewMarkModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        [Display(Name = "Дата прибытия")]
        public DateTime? DateArrival { get; set; }
        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }
        [Display(Name = "Пункт пропуска для въезда")]
        public string CheckPoint { get; set; }
        [Display(Name = "Количество участников")]
        public int CountMembers { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }
        [Display(Name = "Туроператор")]
        public string Operator { get; set; }
        [Display(Name = "Отметка о прибытии")]
        public string Arrived { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }
    }
}