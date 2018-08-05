﻿using BezvizSystem.Web.Infrustructure;
using BezvizSystem.Web.Models.Visitor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Group
{
    [DateArrivalLess(ErrorMessage = "Дата прибытия больше даты убытия")]
    public class EditGroupModel
    {
        public EditGroupModel()
        {
            Infoes = new List<EditInfoVisitorModel>();
        }

        public int Id { get; set; }
        public bool Group { get; set; }

        [Display(Name = "Информация о туристах")]
        public ICollection<EditInfoVisitorModel> Infoes { get; set; }

        [Display(Name = "Дата прибытия")]
        [Required(ErrorMessage = "Дата прибытия не введена")]
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

        // [Required(ErrorMessage = "Программа путешествия не была введена")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Программа путешествия")]
        public string ProgramOfTravel { get; set; }

        // [Required(ErrorMessage = "Организационно-правовая форма не была введена")]
        [Display(Name = "Организационно-правовая форма")]
        public string OrganizeForm { get; set; }

        // [Required(ErrorMessage = "Название не было введено")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        // [Required(ErrorMessage = "Номер договора с туристом не был введен")]
        [Display(Name = "Номер договора с туристом")]
        public string NumberOfContract { get; set; }

        // [Required(ErrorMessage = "Дата заключения договора не была введена")]
        [Display(Name = "Дата заключения договора")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Дата заключения договора не может быть будущей")]
        public DateTime? DateOfContract { get; set; }

        [Display(Name = "Другая информация")]
        public string OtherInfo { get; set; }

        public bool ExtraSend { get; set; }
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }
    }
}