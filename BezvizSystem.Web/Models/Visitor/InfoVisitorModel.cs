using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class InfoVisitorModel
    {
        [Required(ErrorMessage = "Фамилия не была введена")]
        [RegularExpression(@"[а-яА-Яa-zA-Z]{3,50}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Имя не было введено")]
        [RegularExpression(@"[а-яА-Яa-zA-Z]{3,50}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Серия или номер паспота не был введены")]
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }

        [Required(ErrorMessage = "Пол не был выбран")]
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Дата рождения не была введена")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? BithDate { get; set; }

        [Required(ErrorMessage = "Граданство не было введено")]
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}