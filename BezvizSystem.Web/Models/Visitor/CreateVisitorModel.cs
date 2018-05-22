using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class CreateVisitorModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        public InfoVisitorModel Info { get; set; }

        [Display(Name = "Дата прибытия")]
        [DataType(DataType.Date)]
        public DateTime? DateArrival { get; set; }
        [Display(Name = "Дата убытия")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }

        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Display(Name = "Пункт пропуска для пребывания")]
        public string CheckPoint { get; set; }
        [Display(Name = "Инфо о месте пребывания")]
        public string PlaceOfRecidense { get; set; }
   
        [Display(Name = "Программа путешествия")]
        [DataType(DataType.MultilineText)]
        public string ProgramOfTravel { get; set; }
        [Display(Name = "Время работы пунктов пропуска")]
        public string TimeOfWork { get; set; }
        [Display(Name = "Ссылка на сайт")]
        public string SiteOfOperator { get; set; }
        [Display(Name = "Телефон")]
        public string TelNumber { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}