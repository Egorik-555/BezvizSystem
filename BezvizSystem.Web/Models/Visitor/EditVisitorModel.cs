using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Visitor
{
    [DateArrivalLess(ErrorMessage = "Дата прибытия больше даты убытия")]
    public class EditVisitorModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        public EditInfoVisitorModel Info { get; set; }

        [Required(ErrorMessage = "Укажите дату прибытия")]
        [Display(Name = "Дата прибытия")]
        [LessThanOtherDate("DateDeparture", ErrorMessage = "Укажите дату прибытия меньше либо равной дате убытия")]
        [DataType(DataType.Date)]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Укажите дату убытия")]
        [Display(Name = "Дата убытия")]
        [MoreThanOtherDate("DateArrival", ErrorMessage = "Укажите дату убытия больше либо равной дате прибытия")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }

        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Required(ErrorMessage = "Укажите пункт пропуска для прибытия")]
        [Display(Name = "Пункт пропуска для пребывания")]
        public string CheckPoint { get; set; }

        [Display(Name = "Инфо о месте пребывания")]
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

        public bool ExtraSend { get; set; }
        public string TranscriptUser { get; set; }
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }
    }
}