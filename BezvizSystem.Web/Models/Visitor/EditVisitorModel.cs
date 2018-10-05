﻿using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    [DateArrivalLess(ErrorMessage = "Дата прибытия больше даты убытия")]
    public class EditVisitorModel
    {
        public int Id { get; set; }
        public bool Group { get; set; }

        public EditInfoVisitorModel Info { get; set; }

        [Required(ErrorMessage = "Дата прибытия не введена")]
        [Display(Name = "Дата прибытия")]
        [DataType(DataType.Date)]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Дата убытия не введена")]
        [Display(Name = "Дата убытия")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }

        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Required(ErrorMessage = "Пункт пропуска для пребывания не введен")]
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
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public bool ExtraSend { get; set; }
        [Required]
        public string TranscriptUser { get; set; }
        [Required]
        public DateTime? DateInSystem { get; set; }
        [Required]
        public string UserInSystem { get; set; }
        [Required]
        public DateTime? DateEdit { get; set; }
        [Required]
        public string UserEdit { get; set; }
    }
}