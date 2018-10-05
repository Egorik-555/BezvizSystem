using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class InfoVisitorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите фамилию туриста")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя туриста")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите серию и номер паспорта туриста")]
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }

        [Required(ErrorMessage = "Укажите пол туриста")]
        [Display(Name = "Пол")]
        public string Gender { get; set; }
    
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Укажите дату рождения, относящуюся к прошлому")]
        public DateTime? BithDate { get; set; }

        [Required(ErrorMessage = "Укажите гражданство туриста")]
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }

        public bool Arrived { get; set; }

        [Required]
        public DateTime? DateInSystem { get; set; }
        [Required]
        public string UserInSystem { get; set; }
    }
}