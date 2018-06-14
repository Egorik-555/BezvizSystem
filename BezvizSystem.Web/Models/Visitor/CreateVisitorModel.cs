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

        [Required(ErrorMessage = "Дата прибытия не введена")]
        // [RegularExpression(@"(?<dd>^\d{2})-(?<MM>\d{2})-(?<yyyy>)\d{4}$", ErrorMessage ="Неверный фармат даты")]
        [Display(Name = "Дата прибытия")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/2000", "1/1/3000", ErrorMessage = "Неверный формат даты")]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Дата убытия не введена")]
        [Display(Name = "Дата убытия")]
        [Range(typeof(DateTime), "1/1/2000", "1/1/3000", ErrorMessage = "Неверный фармат даты")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }

        //[Required(ErrorMessage = "Количество дней пребывания не была введено")]
        [RegularExpression(@"[0-9]{1,3}", ErrorMessage = "Только цифровые знRачения")]
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

        public bool Arrived { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}