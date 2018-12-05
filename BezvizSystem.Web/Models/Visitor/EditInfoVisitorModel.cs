using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class EditInfoVisitorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите фамилию туриста")]
        [Display(Name = "Фамилия")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Укажите фамилию только латинскими символами")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя туриста")]
        [Display(Name = "Имя")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Укажите имя только латинскими символами")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите серию и номер паспорта туриста")]
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }

        [Required(ErrorMessage = "Укажите пол туриста")]
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [PastDate(ErrorMessage = "Укажите дату рождения, относящуюся к прошлому")]
        [Display(Name = "Дата рождения")]  
        public DateTime? BithDate { get; set; }

        [Required(ErrorMessage = "Укажите гражданство туриста")]
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }

        public bool Arrived { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

    }
}