using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    [DateArrivalLess(ErrorMessage = "Дата прибытия должна быть раньше, чем дата убытия")]
    public class CreateVisitorModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        public InfoVisitorModel Info { get; set; }

        [Display(Name = "Дата прибытия")]
        [Required(ErrorMessage = "Укажите дату прибытия")]
        [LessThanOtherDate("DateDeparture", ErrorMessage = "Укажите дату прибытия меньше либо равной дате убытия")]
        //[FutureDate(ErrorMessage = "Укажите дату прибытия, относящуюся к будущему")]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Укажите дату убытия")]
        [Display(Name = "Дата убытия")]      
        [MoreThanOtherDate("DateArrival", ErrorMessage = "Укажите дату убытия больше либо равной дате прибытия")]
        public DateTime? DateDeparture { get; set; }

        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Required(ErrorMessage = "Укажите пункт пропуска для прибытия")]
        [Display(Name = "Пункт пропуска для прибытия")]
        public string CheckPoint { get; set; }

        [Display(Name = "Инфо о предполагаемом месте пребывания")]
        public string PlaceOfRecidense { get; set; }

        //[Required(ErrorMessage = "Программа путешествия не введена")]
        [Display(Name = "Программа путешествия")]
        [DataType(DataType.MultilineText)]
        public string ProgramOfTravel { get; set; }

       // [Required(ErrorMessage = "Время работы пунктов пропуска не было введено")]
        [Display(Name = "Время работы пунктов пропуска")]
        public string TimeOfWork { get; set; }

       // [Required(ErrorMessage = "Ссылка на сайт не была введена")]
        [Display(Name = "Ссылка на сайт")]
        public string SiteOfOperator { get; set; }

       // [Required(ErrorMessage = "Телефон не был введен")]
        [Display(Name = "Телефон")]
        public string TelNumber { get; set; }

       // [Required(ErrorMessage = "E-mail не был введен")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Укажите корректный адрес")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}