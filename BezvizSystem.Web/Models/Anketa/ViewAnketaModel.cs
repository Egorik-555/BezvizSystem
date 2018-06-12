using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Anketa
{
    public class ViewAnketaModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        [Required(ErrorMessage = "Дата прибытия не была введена")]
        [Display(Name = "Дата прибытия")]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Количество дней пребывания не было введено")]
        [RegularExpression(@"[0-9]{1,3}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Required(ErrorMessage = "Пункт пропуска для въезда не был введен")]
        [Display(Name = "Пункт пропуска для въезда")]
        public string CheckPoint { get; set; }

        [Required(ErrorMessage = "Количество участников не было введено")]
        [Display(Name = "Количество участников")]
        public int CountMembers { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Туроператор не был введен")]
        [Display(Name = "Туроператор")]
        public string Operator { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }
    }
}